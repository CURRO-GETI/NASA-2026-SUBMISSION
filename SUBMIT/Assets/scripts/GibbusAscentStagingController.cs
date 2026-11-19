using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GIBBUS.Ascent
{
    [DisallowMultipleComponent]
    public class GibbusAscentStagingController : MonoBehaviour
    {
        [Serializable]
        public class StageModule
        {
            [Tooltip("Unique ID sent by the Ceffy button, e.g. boosters.")]
            public string id;

            public string displayName;
            public GameObject module;

            [Tooltip("Optional transform defining the direction in which this module separates.")]
            public Transform separationDirection;

            [Min(0f)] public float separationImpulse = 8f;
            [Min(0f)] public float separationTorque = 2f;
            [Min(0f)] public float atmosphereThicknessAfterSeparation = 0.8f;
            [Min(0.05f)] public float atmosphereTransitionDuration = 3f;

            [Tooltip("Invoke this for the stage that should trigger upper-stage ignition.")]
            public bool triggerUpperStageIgnition;

            public UnityEvent onSeparated = new UnityEvent();

            [HideInInspector] public bool separated;
        }

        [Header("Vehicle")]
        [SerializeField] private Transform spacecraftRoot;
        [SerializeField] private List<StageModule> stages = new List<StageModule>();

        [Header("Atmosphere")]
        [SerializeField] private Material skyboxMaterial;
        [SerializeField] private string atmosphereProperty = "_AtmosphereThickness";
        [SerializeField] private float initialAtmosphereThickness = 1f;
        [SerializeField] private float spaceAtmosphereThickness = 0.05f;

        [Header("Flight events")]
        [SerializeField] private UnityEvent onUpperStageIgnition = new UnityEvent();
        [SerializeField] private UnityEvent onSpacecraftReleased = new UnityEvent();
        [SerializeField] private UnityEvent<string> onStageSeparated = new UnityEvent<string>();

        private Material runtimeSkybox;
        private int nextStageIndex;
        private Coroutine atmosphereRoutine;

        public int NextStageIndex => nextStageIndex;
        public bool AllStagesSeparated => nextStageIndex >= stages.Count;
        public Transform SpacecraftRoot => spacecraftRoot;

        private void Awake()
        {
            if (spacecraftRoot == null)
                Debug.LogError("[GIBBUS ASCENT] Assign the spacecraft root.", this);

            Material source = skyboxMaterial != null ? skyboxMaterial : RenderSettings.skybox;
            if (source != null)
            {
                runtimeSkybox = new Material(source);
                RenderSettings.skybox = runtimeSkybox;

                if (runtimeSkybox.HasProperty(atmosphereProperty))
                    runtimeSkybox.SetFloat(atmosphereProperty, initialAtmosphereThickness);
                else
                    Debug.LogWarning(
                        $"[GIBBUS ASCENT] Skybox shader has no '{atmosphereProperty}' property. Check the material shader.",
                        this);
            }

            nextStageIndex = 0;
            for (int i = 0; i < stages.Count; i++)
            {
                stages[i].separated = false;
                if (stages[i].module == null)
                    Debug.LogWarning($"[GIBBUS ASCENT] Stage '{stages[i].id}' has no module assigned.", this);
            }
        }

        /// <summary>
        /// Called by the Ceffy bridge. Only the next configured stage can be separated.
        /// </summary>
        public bool RequestStageSeparation(string stageId)
        {
            if (nextStageIndex >= stages.Count)
            {
                Debug.Log("[GIBBUS ASCENT] No remaining launch modules.");
                return false;
            }

            StageModule stage = stages[nextStageIndex];

            if (!string.Equals(stage.id, stageId, StringComparison.OrdinalIgnoreCase))
            {
                Debug.LogWarning(
                    $"[GIBBUS ASCENT] Separation rejected. Expected '{stage.id}', received '{stageId}'.",
                    this);
                return false;
            }

            if (stage.separated || stage.module == null)
                return false;

            StartCoroutine(SeparateStage(stage));
            return true;
        }

        private IEnumerator SeparateStage(StageModule stage)
        {
            stage.separated = true;

            GameObject detached = stage.module;
            detached.transform.SetParent(null, true);

            Vector3 direction = stage.separationDirection != null
                ? stage.separationDirection.forward
                : detached.transform.up;

            if (direction.sqrMagnitude < 0.001f)
                direction = Vector3.up;

            direction.Normalize();

            Rigidbody body = detached.GetComponent<Rigidbody>();
            if (body == null)
                body = detached.AddComponent<Rigidbody>();

            body.isKinematic = false;
            body.useGravity = false;
            body.AddForce(direction * stage.separationImpulse, ForceMode.Impulse);

            Vector3 torque = Vector3.Cross(direction, Vector3.up);
            if (torque.sqrMagnitude < 0.001f)
                torque = Vector3.right;
            body.AddTorque(torque.normalized * stage.separationTorque, ForceMode.Impulse);

            if (atmosphereRoutine != null)
                StopCoroutine(atmosphereRoutine);

            atmosphereRoutine = StartCoroutine(
                TransitionAtmosphere(
                    stage.atmosphereThicknessAfterSeparation,
                    stage.atmosphereTransitionDuration));

            stage.onSeparated?.Invoke();
            onStageSeparated?.Invoke(stage.id);

            Debug.Log($"[GIBBUS ASCENT] {stage.displayName} separated.");

            if (stage.triggerUpperStageIgnition)
            {
                onUpperStageIgnition?.Invoke();
                Debug.Log("[GIBBUS ASCENT] Upper-stage ignition event invoked.");
            }

            nextStageIndex++;

            if (nextStageIndex >= stages.Count)
            {
                onSpacecraftReleased?.Invoke();
                Debug.Log("[GIBBUS ASCENT] All configured launch modules separated. Spacecraft remains.");
            }

            yield return null;
        }

        private IEnumerator TransitionAtmosphere(float target, float duration)
        {
            if (runtimeSkybox == null || !runtimeSkybox.HasProperty(atmosphereProperty))
                yield break;

            float start = runtimeSkybox.GetFloat(atmosphereProperty);
            float elapsed = 0f;
            duration = Mathf.Max(0.05f, duration);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / duration));
                runtimeSkybox.SetFloat(atmosphereProperty, Mathf.Lerp(start, target, t));
                yield return null;
            }

            runtimeSkybox.SetFloat(atmosphereProperty, target);
            DynamicGI.UpdateEnvironment();
            atmosphereRoutine = null;
        }

        public void SetAtmosphereThickness(float value, float duration = 2f)
        {
            if (atmosphereRoutine != null)
                StopCoroutine(atmosphereRoutine);

            atmosphereRoutine = StartCoroutine(TransitionAtmosphere(value, duration));
        }

        public void ResetStaging()
        {
            StopAllCoroutines();
            nextStageIndex = 0;

            for (int i = 0; i < stages.Count; i++)
                stages[i].separated = false;

            if (runtimeSkybox != null && runtimeSkybox.HasProperty(atmosphereProperty))
                runtimeSkybox.SetFloat(atmosphereProperty, initialAtmosphereThickness);
        }

        private void OnDestroy()
        {
            if (runtimeSkybox != null)
                Destroy(runtimeSkybox);
        }
    }
}

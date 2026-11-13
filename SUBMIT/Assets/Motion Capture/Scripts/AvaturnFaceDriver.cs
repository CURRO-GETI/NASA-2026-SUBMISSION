
using System.Collections.Generic;
using UnityEngine;

namespace AvaturnMocap
{
    public class AvaturnFaceDriver : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] private SkinnedMeshRenderer faceRenderer;

        [Header("Animation")]
        [SerializeField, Range(0f, 30f)]
        private float smoothing = 18f;

        private readonly Dictionary<string, int> indices = new();
        private readonly Dictionary<string, float> targets = new();
        private readonly Dictionary<string, float> current = new();

        public SkinnedMeshRenderer FaceRenderer => faceRenderer;

        private void Awake()
        {
            if (faceRenderer == null)
            {
                Debug.LogError(
                    "[Avaturn Mocap] Face renderer is missing.",
                    this);
                enabled = false;
                return;
            }

            Mesh mesh = faceRenderer.sharedMesh;

            if (mesh == null)
            {
                Debug.LogError(
                    "[Avaturn Mocap] Face mesh is missing.",
                    this);
                enabled = false;
                return;
            }

            for (int i = 0; i < mesh.blendShapeCount; i++)
            {
                string shapeName = mesh.GetBlendShapeName(i);

                indices[shapeName] = i;
                targets[shapeName] = 0f;
                current[shapeName] = 0f;
            }

            Debug.Log(
                $"[Avaturn Mocap] Registered {indices.Count} blendshapes.");
        }

        public void SetTargetWeight(string shapeName, float value)
        {
            if (!targets.ContainsKey(shapeName))
                return;

            targets[shapeName] = Mathf.Clamp01(value);
        }

        public void SetTargetFrame(
            IDictionary<string, float> values)
        {
            foreach (var pair in values)
                SetTargetWeight(pair.Key, pair.Value);
        }

        private void LateUpdate()
        {
            float blend = smoothing <= 0f
                ? 1f
                : 1f - Mathf.Exp(-smoothing * Time.unscaledDeltaTime);

            foreach (var pair in targets)
            {
                string shapeName = pair.Key;
                float target = pair.Value;

                current[shapeName] = Mathf.Lerp(
                    current[shapeName],
                    target,
                    blend);

                faceRenderer.SetBlendShapeWeight(
                    indices[shapeName],
                    current[shapeName] * 100f);
            }
        }

        public float GetCurrentWeight(string shapeName)
        {
            return current.TryGetValue(shapeName, out float value)
                ? value
                : 0f;
        }

        public bool HasBlendShape(string shapeName)
        {
            return indices.ContainsKey(shapeName);
        }

        public List<string> GetBlendShapeNames()
        {
            return new List<string>(indices.Keys);
        }
    }
}

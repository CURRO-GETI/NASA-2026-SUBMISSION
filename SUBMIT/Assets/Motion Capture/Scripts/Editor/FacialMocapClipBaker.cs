
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace AvaturnMocap.Editor
{
    public static class FacialMocapClipBaker
    {
        [MenuItem("Tools/Facial Mocap/Bake Recording to AnimationClip")]
        private static void BakeRecording()
        {
            GameObject root = Selection.activeGameObject;

            if (root == null)
            {
                EditorUtility.DisplayDialog(
                    "Facial Mocap",
                    "Select your character root in the Hierarchy first.",
                    "OK");
                return;
            }

            string jsonPath = EditorUtility.OpenFilePanel(
                "Select Facial Recording",
                Application.persistentDataPath,
                "json");

            if (string.IsNullOrEmpty(jsonPath))
                return;

            string json = File.ReadAllText(jsonPath);

            FacialMocapRecordingData data =
                JsonUtility.FromJson<FacialMocapRecordingData>(json);

            if (data == null || data.frames.Count < 2)
            {
                Debug.LogError("Invalid facial recording.");
                return;
            }

            var renderers =
                root.GetComponentsInChildren<SkinnedMeshRenderer>(true);

            Dictionary<string, SkinnedMeshRenderer> shapeRenderers =
                new Dictionary<string, SkinnedMeshRenderer>();

            foreach (var renderer in renderers)
            {
                if (renderer.sharedMesh == null)
                    continue;

                for (int i = 0;
                     i < renderer.sharedMesh.blendShapeCount;
                     i++)
                {
                    string name =
                        renderer.sharedMesh.GetBlendShapeName(i);

                    if (!shapeRenderers.ContainsKey(name))
                        shapeRenderers.Add(name, renderer);
                }
            }

            AnimationClip clip = new AnimationClip
            {
                frameRate = data.sampleRate,
                name = Path.GetFileNameWithoutExtension(jsonPath)
            };

            int generated = 0;

            for (int shapeIndex = 0;
                 shapeIndex < data.blendshapeNames.Count;
                 shapeIndex++)
            {
                string shapeName =
                    data.blendshapeNames[shapeIndex];

                if (!shapeRenderers.TryGetValue(
                    shapeName, out var renderer))
                    continue;

                List<Keyframe> keys = new List<Keyframe>();

                foreach (var frame in data.frames)
                {
                    if (shapeIndex >= frame.values.Count)
                        continue;

                    keys.Add(new Keyframe(
                        frame.time,
                        frame.values[shapeIndex] * 100f));
                }

                if (keys.Count < 2)
                    continue;

                AnimationCurve curve =
                    new AnimationCurve(keys.ToArray());

                string path = AnimationUtility.CalculateTransformPath(
                    renderer.transform,
                    root.transform);

                EditorCurveBinding binding =
                    EditorCurveBinding.FloatCurve(
                        path,
                        typeof(SkinnedMeshRenderer),
                        "blendShape." + shapeName);

                AnimationUtility.SetEditorCurve(
                    clip,
                    binding,
                    curve);

                generated++;
            }

            if (generated == 0)
            {
                Debug.LogError(
                    "No matching blendshapes were found.");
                Object.DestroyImmediate(clip);
                return;
            }

            string assetPath =
                EditorUtility.SaveFilePanelInProject(
                    "Save Facial Animation",
                    clip.name,
                    "anim",
                    "Choose where to save your animation.");

            if (string.IsNullOrEmpty(assetPath))
            {
                Object.DestroyImmediate(clip);
                return;
            }

            AssetDatabase.CreateAsset(clip, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(
                $"[Facial Mocap] Created {assetPath} " +
                $"with {generated} animated blendshapes.");
        }
    }
}

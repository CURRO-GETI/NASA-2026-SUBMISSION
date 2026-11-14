
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AvaturnMocap
{
    [Serializable]
    public class FacialMocapFrame
    {
        public float time;
        public List<float> values = new List<float>();
    }

    [Serializable]
    public class FacialMocapRecordingData
    {
        public float sampleRate;
        public List<string> blendshapeNames = new List<string>();
        public List<FacialMocapFrame> frames =
            new List<FacialMocapFrame>();
    }

    public class FacialMocapRecorder : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] private AvaturnFaceDriver faceDriver;

        [Header("Recording")]
        [SerializeField, Range(10, 60)]
        private int sampleRate = 30;

        [SerializeField]
        private string fileName = "FacialRecording";

        private FacialMocapRecordingData recording;

        private float recordingElapsed;
        private float sampleAccumulator;

        private bool isRecording;
        private bool isPlaying;

        private float playbackTime;

        private string RecordingDirectory =>
            Path.Combine(
                Application.persistentDataPath,
                "FacialMocap");

        public bool IsRecording => isRecording;
        public bool IsPlaying => isPlaying;

        public int RecordedFrames =>
            recording != null ? recording.frames.Count : 0;

        private void Update()
        {
            if (isRecording)
                RecordUpdate();

            if (isPlaying)
                PlaybackUpdate();
        }

        public void StartRecording()
        {
            if (faceDriver == null)
            {
                Debug.LogError("[Facial Recorder] Face Driver missing.");
                return;
            }

            StopPlayback();

            recording = new FacialMocapRecordingData();
            recording.sampleRate = sampleRate;
            recording.blendshapeNames =
                faceDriver.GetBlendShapeNames();

            recordingElapsed = 0f;
            sampleAccumulator = 0f;

            isRecording = true;

            Debug.Log("[Facial Recorder] Recording started.");
        }

        public void StopRecording()
        {
            if (!isRecording)
                return;

            isRecording = false;

            Debug.Log(
                $"[Facial Recorder] Recording stopped. " +
                $"{recording.frames.Count} frames captured.");
        }

        private void RecordUpdate()
        {
            float delta = Time.unscaledDeltaTime;

            recordingElapsed += delta;
            sampleAccumulator += delta;

            float interval = 1f / sampleRate;

            while (sampleAccumulator >= interval)
            {
                sampleAccumulator -= interval;
                CaptureFrame(recordingElapsed - sampleAccumulator);
            }
        }

        private void CaptureFrame(float timestamp)
        {
            FacialMocapFrame frame = new FacialMocapFrame();

            frame.time = timestamp;

            foreach (string shapeName in recording.blendshapeNames)
            {
                frame.values.Add(
                    faceDriver.GetCurrentWeight(shapeName));
            }

            recording.frames.Add(frame);
        }

        public void SaveRecording()
        {
            if (recording == null || recording.frames.Count == 0)
            {
                Debug.LogWarning("[Facial Recorder] Nothing to save.");
                return;
            }

            StopRecording();

            Directory.CreateDirectory(RecordingDirectory);

            string path = Path.Combine(
                RecordingDirectory,
                fileName + ".json");

            string json = JsonUtility.ToJson(recording, true);

            File.WriteAllText(path, json);

            Debug.Log(
                "[Facial Recorder] Saved recording: " + path);
        }

        public void LoadRecording(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError(
                    "[Facial Recorder] File not found: " + path);
                return;
            }

            string json = File.ReadAllText(path);

            recording =
                JsonUtility.FromJson<FacialMocapRecordingData>(json);

            Debug.Log(
                $"[Facial Recorder] Loaded {recording.frames.Count} frames.");
        }

        public void StartPlayback()
        {
            if (recording == null || recording.frames.Count < 2)
            {
                Debug.LogWarning(
                    "[Facial Recorder] No valid recording loaded.");
                return;
            }

            StopRecording();

            playbackTime = 0f;
            isPlaying = true;

            Debug.Log("[Facial Recorder] Playback started.");
        }

        public void StopPlayback()
        {
            isPlaying = false;
        }

        private void PlaybackUpdate()
        {
            playbackTime += Time.unscaledDeltaTime;

            List<FacialMocapFrame> frames = recording.frames;

            if (playbackTime >= frames[frames.Count - 1].time)
            {
                ApplyFrame(frames[frames.Count - 1]);
                StopPlayback();
                return;
            }

            int nextIndex = 1;

            while (nextIndex < frames.Count &&
                   frames[nextIndex].time < playbackTime)
            {
                nextIndex++;
            }

            FacialMocapFrame previous = frames[nextIndex - 1];
            FacialMocapFrame next = frames[nextIndex];

            float duration = next.time - previous.time;

            float t = duration > 0f
                ? Mathf.Clamp01(
                    (playbackTime - previous.time) / duration)
                : 0f;

            for (int i = 0; i < recording.blendshapeNames.Count; i++)
            {
                float value = Mathf.Lerp(
                    previous.values[i],
                    next.values[i],
                    t);

                faceDriver.SetTargetWeight(
                    recording.blendshapeNames[i],
                    value);
            }
        }

        private void ApplyFrame(FacialMocapFrame frame)
        {
            for (int i = 0; i < recording.blendshapeNames.Count; i++)
            {
                faceDriver.SetTargetWeight(
                    recording.blendshapeNames[i],
                    frame.values[i]);
            }
        }
    }
}

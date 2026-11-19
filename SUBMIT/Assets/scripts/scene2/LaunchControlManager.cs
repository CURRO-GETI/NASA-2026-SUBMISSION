
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;

public class LaunchControlManager : MonoBehaviour
{
    [Header("Interface")]
    public CanvasGroup htmlInterface;
    public CanvasGroup launchControlUI;

    [Header("Camera")]
    public Transform mainCamera;
    public Transform cinematicCameraPosition;
    public float cameraTransitionDuration = 3f;

    [Header("Audio")]
    public AudioSource missionControlAudio;
    public AudioSource countdownAudio;

    [Header("Timeline")]
    public PlayableDirector launchTimeline;

    [Header("Countdown UI")]
    public TMP_Text countdownText;
    public TMP_Text announcementText;

    [Header("Timing")]
    public float fadeDuration = 1.5f;
    public float countdownInterval = 2f;

    private bool launchSequenceStarted;
    private bool timelineStarted;

    public void StartLaunchSequence()
    {
        if (launchSequenceStarted) return;

        launchSequenceStarted = true;

        StartCoroutine(LaunchSequence());
    }

    private IEnumerator LaunchSequence()
    {
        // 1. Fade out HTML interface
        yield return StartCoroutine(
            FadeCanvas(htmlInterface, 1f, 0f)
        );

        htmlInterface.gameObject.SetActive(false);

        // 2. Show launch control UI
        launchControlUI.gameObject.SetActive(true);

        yield return StartCoroutine(
            FadeCanvas(launchControlUI, 0f, 1f)
        );

        // 3. Transition to cinematic camera
        yield return StartCoroutine(
            TransitionCamera()
        );

        // 4. Begin mission control audio
        if (missionControlAudio != null)
            missionControlAudio.Play();

        // 5. Start countdown
        yield return StartCoroutine(
            RunCountdown()
        );
    }

    private IEnumerator FadeCanvas(
        CanvasGroup canvas,
        float start,
        float end)
    {
        if (canvas == null) yield break;

        canvas.alpha = start;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            canvas.alpha = Mathf.Lerp(
                start,
                end,
                elapsed / fadeDuration
            );

            yield return null;
        }

        canvas.alpha = end;
    }

    private IEnumerator TransitionCamera()
    {
        if (mainCamera == null ||
            cinematicCameraPosition == null)
            yield break;

        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;

        float elapsed = 0f;

        while (elapsed < cameraTransitionDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.SmoothStep(
                0f,
                1f,
                elapsed / cameraTransitionDuration
            );

            mainCamera.transform.position = Vector3.Lerp(
                startPosition,
                cinematicCameraPosition.position,
                t
            );

            mainCamera.transform.rotation = Quaternion.Slerp(
                startRotation,
                cinematicCameraPosition.rotation,
                t
            );

            yield return null;
        }
    }



    private IEnumerator RunCountdown()
    {
        const float countdownDuration = 60f;

        float elapsed = 0f;
        int lastDisplayedSecond = -1;

        bool[] announcementPlayed = new bool[5];

        string[] times =
        {
        "T-60",
        "T-45",
        "T-30",
        "T-15",
        "T-10"
    };

        string[] announcements =
        {
        "Launch sequence initiated.",
        "Guidance and navigation systems verified.",
        "Propulsion systems entering launch configuration.",
        "Final flight readiness verification.",
        "Automated countdown initiated."
    };

        float[] announcementTimes =
        {
        0f, 15f, 30f, 45f, 50f
    };

        while (elapsed < countdownDuration)
        {
            // Calculate remaining countdown time
            float remaining = countdownDuration - elapsed;

            int secondsRemaining = Mathf.CeilToInt(remaining);

            // Update UI only when the displayed second changes
            if (secondsRemaining != lastDisplayedSecond)
            {
                countdownText.text = $"T-{secondsRemaining}";
                lastDisplayedSecond = secondsRemaining;
            }

            // Mission control announcements
            for (int i = 0; i < announcementTimes.Length; i++)
            {
                if (!announcementPlayed[i] &&
                    elapsed >= announcementTimes[i])
                {
                    announcementText.text = announcements[i];
                    announcementPlayed[i] = true;

                    // At T-10
                    if (i == 4)
                    {
                        // Start countdown audio
                        if (countdownAudio != null)
                            countdownAudio.Play();

                        // Start cinematic Timeline
                        StartTimeline();
                    }
                }
            }

            elapsed += Time.deltaTime;

            yield return null;
        }

        // Final countdown state
        launchTimeline.Play();
        countdownText.text = "T-0";
        announcementText.text = "LIFTOFF!";
    }



    public void StartTimeline()
    {
        if (timelineStarted) return;

        timelineStarted = true;

        if (launchTimeline != null)
        {
            launchTimeline.time = 0;
            
        }
    }

    public void SkipToLaunch()
    {
        StopAllCoroutines();
        StartTimeline();
    }
}

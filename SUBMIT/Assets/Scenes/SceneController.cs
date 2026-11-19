using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    [Header("Loading Screen UI (Optional)")]
    [SerializeField] private GameObject loadingScreenCanvas;
    [SerializeField] private Slider progressBar;

    [Header("Transition Settings")]
    [SerializeField] private float minLoadTime = 1f;

    public event Action OnSceneLoaded;

    private void Awake()
    {
        // Singleton setup ensuring persistence across scene loads
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Loads a scene instantly by name.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        OnSceneLoaded?.Invoke();
    }

    /// <summary>
    /// Loads a scene asynchronously with an optional loading screen progress bar.
    /// </summary>
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncRoutine(sceneName));
    }

    private IEnumerator LoadSceneAsyncRoutine(string sceneName)
    {
        float timer = 0f;

        if (loadingScreenCanvas != null)
        {
            loadingScreenCanvas.SetActive(true);
        }

        // Begin asynchronous operation
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            timer += Time.unscaledDeltaTime;

            // Unity's progress caps at 0.9 when allowSceneActivation is false
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            // Ensure the loading screen stays visible for at least minLoadTime for smooth pacing
            if (operation.progress >= 0.9f && timer >= minLoadTime)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        if (loadingScreenCanvas != null)
        {
            loadingScreenCanvas.SetActive(false);
        }

        OnSceneLoaded?.Invoke();
    }

    /// <summary>
    /// Reloads the currently active scene.
    /// </summary>
    public void ReloadCurrentScene()
    {
        LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Quits the application (works in built builds, ignored in Editor).
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
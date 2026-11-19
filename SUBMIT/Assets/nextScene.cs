using UnityEngine;

public class nextScene : MonoBehaviour
{
    public void NextScene(string sceneName)
    {
        SceneController.Instance.LoadScene(sceneName);
    }
}


using UnityEngine;
using UnityEngine.EventSystems;

public class OSWindow : MonoBehaviour, IPointerDownHandler
{
    [Header("Window Information")]
    [SerializeField] private string windowName;

    [Header("OS Reference")]
    [SerializeField] private OSManager osManager;

    public string WindowName => windowName;

    public void Open()
    {
        gameObject.SetActive(true);
        osManager?.FocusWindow(this);

        Debug.Log($"[OS] Opened: {windowName}");
    }

    public void Close()
    {
        gameObject.SetActive(false);

        Debug.Log($"[OS] Closed: {windowName}");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        osManager?.FocusWindow(this);
    }
}

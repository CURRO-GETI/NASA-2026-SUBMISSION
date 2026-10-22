
using UnityEngine;
using Unity.Cinemachine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private Transform cameraTarget;

    [Header("Mouse Settings")]
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float minPitch = -35f;
    [SerializeField] private float maxPitch = 70f;

    [Header("Cursor")]
    [SerializeField] private bool lockCursor = true;

    private float yaw;
    private float pitch;

    private void Start()
    {
        if (virtualCamera == null)
        {
            virtualCamera = GetComponent<CinemachineCamera>();
        }

        if (cameraTarget != null)
        {
            Vector3 angles = cameraTarget.eulerAngles;
            yaw = angles.y;
            pitch = angles.x;

            if (pitch > 180f)
                pitch -= 360f;
        }

        UpdateCursor();
    }

    private void LateUpdate()
    {
        if (cameraTarget == null)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            lockCursor = false;
            UpdateCursor();
        }

        if (Input.GetMouseButtonDown(0) && !lockCursor)
        {
            lockCursor = true;
            UpdateCursor();
        }

        if (!lockCursor)
            return;

        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void UpdateCursor()
    {
        Cursor.lockState = lockCursor
            ? CursorLockMode.Locked
            : CursorLockMode.None;

        Cursor.visible = !lockCursor;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

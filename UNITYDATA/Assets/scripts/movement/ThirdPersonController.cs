
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTarget;

    [Header("Camera")]
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float minPitch = -35f;
    [SerializeField] private float maxPitch = 70f;
    [SerializeField]
    private Vector3 cameraOffset =
        new Vector3(0f, 1.6f, 0f);

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 12f;

    private float cameraYaw;
    private float cameraPitch;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (cameraTarget != null)
        {
            cameraYaw = cameraTarget.eulerAngles.y;
            cameraPitch = cameraTarget.eulerAngles.x;

            if (cameraPitch > 180f)
                cameraPitch -= 360f;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleCamera();
        HandleAnimation();
    }

    private void LateUpdate()
    {
        if (cameraTarget != null)
        {
            cameraTarget.position =
                transform.position + cameraOffset;
        }
    }

    private void HandleCamera()
    {
        if (cameraTarget == null || Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        cameraYaw += mouseDelta.x * mouseSensitivity;
        cameraPitch -= mouseDelta.y * mouseSensitivity;

        cameraPitch = Mathf.Clamp(
            cameraPitch, minPitch, maxPitch
        );

        cameraTarget.rotation = Quaternion.Euler(
            cameraPitch, cameraYaw, 0f
        );
    }

    private void HandleAnimation()
    {
        if (animator == null || Keyboard.current == null)
            return;

        float moveX = 0f;
        float moveY = 0f;

        if (Keyboard.current.aKey.isPressed) moveX -= 1f;
        if (Keyboard.current.dKey.isPressed) moveX += 1f;
        if (Keyboard.current.sKey.isPressed) moveY -= 1f;
        if (Keyboard.current.wKey.isPressed) moveY += 1f;

        Vector2 movement = Vector2.ClampMagnitude(
            new Vector2(moveX, moveY), 1f
        );

        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);

        // Rotate towards the camera-relative movement direction.
        if (movement.sqrMagnitude > 0.001f &&
            cameraTarget != null)
        {
            Vector3 forward = cameraTarget.forward;
            Vector3 right = cameraTarget.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 direction =
                forward * movement.y +
                right * movement.x;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

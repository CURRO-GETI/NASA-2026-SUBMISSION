
using UnityEngine;
using UnityEngine.InputSystem;
public class ThirdPersonController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [Header("Camera Follow Turning")]
    [SerializeField] private float turnSpeed = 180f;
    [SerializeField] private float turnStartAngle = 15f;
    [SerializeField] private float turnStopAngle = 3f;

    private static readonly int TurnDirection =
        Animator.StringToHash("TurnDirection");

    private static readonly int IsTurning =
        Animator.StringToHash("IsTurning");

    private bool isTurning;

    [Header("Camera")]
    [SerializeField]    
    private Vector3 cameraOffset =
        new Vector3(0f, 1.5f, 0f);

    [SerializeField] private float mouseSensitivity = 0.12f;
    [SerializeField] private float minPitch = -35f;
    [SerializeField] private float maxPitch = 70f;

    [Header("Character Rotation")]
    [SerializeField] private float rotationSpeed = 12f;

    [Header("Animation")]
    [SerializeField] private float animationDampTime = 0.1f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;

    [Header("References")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    [SerializeField]private Animator animator;

    private float cameraYaw;
    private float cameraPitch;
    private float verticalVelocity;

    private Vector2 movementInput;
    private bool hasMoveInput;

    private static readonly int MoveX =
        Animator.StringToHash("moveX");

    private static readonly int MoveY =
        Animator.StringToHash("moveY");

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        animator.applyRootMotion = true;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ReadInput();
        HandleCamera();
        HandleRotation();
        HandleGravity();
        UpdateAnimation();
    }

    private void LateUpdate()
    {
        if (cameraTarget != null)
        {
            cameraTarget.position =
                transform.position + cameraOffset;
        }
    }

    private void ReadInput()
    {
        movementInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
                movementInput.y += 1f;

            if (Keyboard.current.sKey.isPressed)
                movementInput.y -= 1f;

            if (Keyboard.current.aKey.isPressed)
                movementInput.x -= 1f;

            if (Keyboard.current.dKey.isPressed)
                movementInput.x += 1f;
        }

        movementInput =
            Vector2.ClampMagnitude(movementInput, 1f);

        hasMoveInput = movementInput.sqrMagnitude > 0.001f;
    }

    private void HandleCamera()
    {
        if (cameraTarget == null || Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        cameraYaw += mouseDelta.x * mouseSensitivity;
        cameraPitch -= mouseDelta.y * mouseSensitivity;

        cameraPitch = Mathf.Clamp(
            cameraPitch,
            minPitch,
            maxPitch
        );

        cameraTarget.rotation = Quaternion.Euler(
            cameraPitch,
            cameraYaw,
            0f
        );
    }

    private void HandleRotation()
    {
        if (cameraTransform == null)
            return;

        float angle = Mathf.DeltaAngle(
            transform.eulerAngles.y,
            cameraTransform.eulerAngles.y
        );

        if (!isTurning && Mathf.Abs(angle) > turnStartAngle)
            isTurning = true;

        if (isTurning && Mathf.Abs(angle) < turnStopAngle)
            isTurning = false;

        animator.SetBool("IsTurning", isTurning);

        if (isTurning)
        {
            animator.SetFloat(
                "TurnDirection",
                Mathf.Sign(angle)
            );
        }
    }

    private void HandleGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    private void UpdateAnimation()
    {
        float targetX = isTurning ? 0f : movementInput.x;
        float targetY = isTurning ? 0f : movementInput.y;

        animator.SetFloat(
            "moveX",
            targetX,
            animationDampTime,
            Time.deltaTime
        );

        animator.SetFloat(
            "moveY",
            targetY,
            animationDampTime,
            Time.deltaTime
        );
    }

    private void HandleMovement()
    {
        if (cameraTransform == null)
            return;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 direction =
            forward * movementInput.y +
            right * movementInput.x;

        Vector3 velocity = direction * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDisable()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

}

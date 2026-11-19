
using UnityEngine;

namespace GIBBUS
{
    public class CursorController : MonoBehaviour
    {
        public static CursorController Instance { get; private set; }

        [Header("Cursor Settings")]
        [SerializeField] private bool cursorVisible = true;

        [SerializeField]
        private CursorLockMode cursorLockMode =
            CursorLockMode.None;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            ApplyCursorSettings();
        }

        private void ApplyCursorSettings()
        {
            Cursor.visible = cursorVisible;
            Cursor.lockState = cursorLockMode;
        }

        // Show cursor
        public void ShowCursor()
        {
            cursorVisible = true;
            Cursor.visible = true;
        }

        // Hide cursor
        public void HideCursor()
        {
            cursorVisible = false;
            Cursor.visible = false;
        }

        // Enable cursor movement
        public void UnlockCursor()
        {
            cursorLockMode = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.None;
        }

        // Disable cursor movement
        public void LockCursor()
        {
            cursorLockMode = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Confine cursor to game window
        public void ConfineCursor()
        {
            cursorLockMode = CursorLockMode.Confined;
            Cursor.lockState = CursorLockMode.Confined;
        }

        // Show and unlock
        public void EnableUIInteraction()
        {
            ShowCursor();
            UnlockCursor();
        }

        // Hide and lock
        public void EnableGameplayControl()
        {
            HideCursor();
            LockCursor();
        }

        // Toggle visibility
        public void ToggleCursorVisibility()
        {
            ShowOrHide(!Cursor.visible);
        }

        private void ShowOrHide(bool visible)
        {
            cursorVisible = visible;
            Cursor.visible = visible;
        }

        // Toggle lock state
        public void ToggleCursorMovement()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                UnlockCursor();
            else
                LockCursor();
        }
    }
}

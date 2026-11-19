using UnityEngine;

namespace GIBBUS.Ascent
{
    /// <summary>
    /// Optional cinematic ascent motion for scenes that do not already animate the vehicle.
    /// Disable this if an Animator, Timeline, or flight controller already moves the rocket.
    /// </summary>
    public class GibbusCinematicAscentMotion : MonoBehaviour
    {
        [SerializeField] private Transform vehicle;
        [SerializeField] private Vector3 localTravelDirection = Vector3.up;
        [SerializeField] private float movementSpeed = 12f;
        [SerializeField] private bool ascentActive;

        public void BeginAscent() => ascentActive = true;
        public void PauseAscent() => ascentActive = false;

        private void Update()
        {
            if (!ascentActive || vehicle == null) return;

            Vector3 direction = vehicle.TransformDirection(localTravelDirection.normalized);
            vehicle.position += direction * movementSpeed * Time.deltaTime;
        }
    }
}

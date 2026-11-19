using UnityEngine;

namespace GIBBUS.Ascent
{
    [DisallowMultipleComponent]
    public class GibbusAscentCameraFollow : MonoBehaviour
    {
        [Header("Follow")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 followOffset = new Vector3(0f, 5f, -18f);
        [SerializeField] private float positionSmoothTime = 1.2f;
        [SerializeField] private float rotationLerpSpeed = 3f;
        [SerializeField] private bool lookAtTarget = true;

        private Vector3 positionVelocity;
        private bool following = true;

        public void SetTarget(Transform newTarget) => target = newTarget;
        public void SetFollowing(bool value) => following = value;

        private void LateUpdate()
        {
            if (!following || target == null) return;

            Vector3 desiredPosition = target.TransformPoint(followOffset);
            transform.position = Vector3.SmoothDamp(
                transform.position, desiredPosition, ref positionVelocity, positionSmoothTime);

            if (lookAtTarget)
            {
                Vector3 direction = target.position - transform.position;
                if (direction.sqrMagnitude > 0.001f)
                {
                    Quaternion desiredRotation = Quaternion.LookRotation(direction.normalized, target.up);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation, desiredRotation, rotationLerpSpeed * Time.deltaTime);
                }
            }
        }
    }
}

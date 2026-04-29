using UnityEngine;

namespace WatchTryOn
{
    /// <summary>
    /// Handles the transformation, alignment, scaling, and smoothing of the watch 
    /// based on the target pose calculated from the hand tracking data.
    /// </summary>
    public class WatchPlacementController : MonoBehaviour
    {
        [Header("Placement Settings")]
        [Tooltip("Offset the watch towards the forearm (along Y axis). Positive values move it further down the arm.")]
        public float offsetTowardForearm = 0.04f; // 4 cm
        [Tooltip("Offset inward/outward (along Z axis). Negative values push it into the skin to avoid floating.")]
        public float offsetInward = -0.015f; // -1.5 cm inward
        
        [Header("Scaling")]
        [Tooltip("Multiplier relative to the distance between Index and Pinky knuckles.")]
        public float scaleMultiplier = 1.2f;
        
        [Header("Smoothing (Temporal Consistency)")]
        [Tooltip("Rate at which position catches up to target. Higher = faster/less smooth.")]
        [Range(1f, 30f)]
        public float positionSmoothFactor = 15f; 
        [Tooltip("Rate at which rotation catches up to target.")]
        [Range(1f, 30f)]
        public float rotationSmoothFactor = 15f;

        private Vector3 targetPosition;
        private Quaternion targetRotation;
        private Vector3 targetScale = Vector3.one;

        private void Start()
        {
            // Initialize targets to current transforms to avoid initial snapping
            targetPosition = transform.position;
            targetRotation = transform.rotation;
        }

        /// <summary>
        /// Updates the internal target variables based on the latest mathematical pose.
        /// </summary>
        public void UpdatePose(Vector3 wrist, Vector3 xAxis, Vector3 yAxis, Vector3 zAxis, float wristWidth)
        {
            // 1. Target Rotation
            // Quaternion.LookRotation takes the forward vector (Z) and the upwards vector (Y).
            // Based on WatchPoseCalculator, Z points outwards (face of watch) and Y points down arm.
            targetRotation = Quaternion.LookRotation(zAxis, yAxis);

            // 2. Target Position
            // Position = wrist + (Y * offset toward forearm) + (Z * offset to avoid floating)
            // Note: Since Y points down the arm, adding it moves the watch towards the forearm.
            targetPosition = wrist + (yAxis * offsetTowardForearm) + (zAxis * offsetInward);

            // 3. Target Scale
            float scale = wristWidth * scaleMultiplier;
            targetScale = new Vector3(scale, scale, scale);
        }

        private void LateUpdate()
        {
            // Apply smoothing to reduce jitter (temporal consistency)
            // We use Time.deltaTime so the smoothing is frame-rate independent
            
            // Position: Vector3.Lerp(previousPos, targetPos, smoothFactor)
            transform.position = Vector3.Lerp(transform.position, targetPosition, positionSmoothFactor * Time.deltaTime);
            
            // Rotation: Quaternion.Slerp(previousRot, targetRot, smoothFactor)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothFactor * Time.deltaTime);
            
            // Scale smoothing
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, positionSmoothFactor * Time.deltaTime);
        }
    }
}

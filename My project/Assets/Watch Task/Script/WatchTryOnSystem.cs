using UnityEngine;

namespace WatchTryOn
{
    /// <summary>
    /// The core manager script that bridges hand tracking data to watch placement.
    /// Handles hand selection logic, visibility toggling, and data routing.
    /// </summary>
    public class WatchTryOnSystem : MonoBehaviour
    {
        [Header("Hand Selection")]
        [Tooltip("Which hand should the watch attach to?")]
        public Handedness targetHand = Handedness.Left;

        [Header("References")]
        [Tooltip("The controller responsible for moving the watch.")]
        public WatchPlacementController watchController;
        
        [Tooltip("The root visual object of the watch to disable when tracking is lost.")]
        public GameObject watchModelRoot;

        [Tooltip("Handles dynamic GLB loading for different watch styles.")]
        public GLBModelLoader modelLoader;

        public string path;
        private void Start()
        {
            // Default state: hidden until hand is detected
            SetWatchVisibility(false);
        }

        /// <summary>
        /// Event listener to be called by your MediaPipe integration when a hand updates.
        /// </summary>
        public void OnHandTracked(HandTrackingData handData)
        {
            // 1. Hand Selection Logic
            // If there's no data, or it's lost, or it's the WRONG hand -> hide watch
            if (handData == null || !handData.IsTracked || handData.HandType != targetHand)
            {
                SetWatchVisibility(false);
                return;
            }

            // Ensure MediaPipe has provided the full set of 21 landmarks
            if (handData.Landmarks == null || handData.Landmarks.Length < 21)
            {
                SetWatchVisibility(false);
                return;
            }

            // Valid target hand detected!
            SetWatchVisibility(true);

            // 2. Extract required landmarks
            Vector3 wrist = handData.Landmarks[0];
            Vector3 indexMCP = handData.Landmarks[5];
            Vector3 pinkyMCP = handData.Landmarks[17];

            // 3. Compute Wrist Coordinate System
            WatchPoseCalculator.ComputeWristPose(wrist, indexMCP, pinkyMCP, 
                out Vector3 xAxis, out Vector3 yAxis, out Vector3 zAxis, out float wristWidth);

            // 4. Update the watch placement
            watchController.UpdatePose(wrist, xAxis, yAxis, zAxis, wristWidth);

            // Optional Palm vs Back visibility check:
            // You can use the dot product of the camera forward vector and the wrist Z-axis.
            // If Dot(camForward, zAxis) > 0, the palm is facing the camera, so you could hide the watch.
            // float facingDot = Vector3.Dot(Camera.main.transform.forward, zAxis);
            // if (facingDot > 0.3f) SetWatchVisibility(false); // Example usage
        }

        /// <summary>
        /// Event listener for when the MediaPipe tracker completely loses all hands.
        /// </summary>
        public void OnTrackingLost()
        {
            SetWatchVisibility(false);
        }

        /// <summary>
        /// Basic visibility handling.
        /// </summary>
        private void SetWatchVisibility(bool isVisible)
        {
            if (watchModelRoot != null && watchModelRoot.activeSelf != isVisible)
            {
                watchModelRoot.SetActive(isVisible);
            }
        }

        /// <summary>
        /// Swap between different watch models at runtime.
        /// </summary>
        /// <param name="glbPath">Path to the .glb file</param>
        public void SwitchWatch(string glbPath)
        {
            if (modelLoader != null)
            {
                // We load it as a child of the watchController transform
                modelLoader.LoadModel(glbPath, watchController.transform);
            }
        }
    }
}

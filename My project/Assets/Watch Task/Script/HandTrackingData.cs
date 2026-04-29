using UnityEngine;

namespace WatchTryOn
{
    public enum Handedness
    {
        Left,
        Right
    }

    /// <summary>
    /// Standardized container for hand tracking data coming from MediaPipe.
    /// This abstracts the raw MediaPipe SDK to keep our core logic clean.
    /// </summary>
    public class HandTrackingData
    {
        public Handedness HandType;
        public bool IsTracked;
        
        /// <summary>
        /// Array of 21 landmarks provided by MediaPipe Hands.
        /// Ensure indices map properly: 0=Wrist, 5=Index MCP, 17=Pinky MCP
        /// </summary>
        public Vector3[] Landmarks; 
    }
}

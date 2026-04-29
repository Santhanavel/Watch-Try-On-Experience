using UnityEngine;

namespace WatchTryOn
{
    /// <summary>
    /// Pure math utility class that handles computing the stable local coordinate system 
    /// for the wrist based on the given tracking landmarks.
    /// </summary>
    public static class WatchPoseCalculator
    {
        /// <summary>
        /// Computes coordinate basis vectors and the wrist width for scaling.
        /// </summary>
        /// <param name="wrist">Landmark 0</param>
        /// <param name="indexMCP">Landmark 5</param>
        /// <param name="pinkyMCP">Landmark 17</param>
        /// <param name="xAxis">Computed X Axis (width direction)</param>
        /// <param name="yAxis">Computed Y Axis (arm direction)</param>
        /// <param name="zAxis">Computed Z Axis (surface normal)</param>
        /// <param name="wristWidth">Calculated width of the wrist based on knuckles</param>
        public static void ComputeWristPose(Vector3 wrist, Vector3 indexMCP, Vector3 pinkyMCP, 
            out Vector3 xAxis, out Vector3 yAxis, out Vector3 zAxis, out float wristWidth)
        {
            // 1. Calculate wrist width for dynamic scaling later
            wristWidth = Vector3.Distance(indexMCP, pinkyMCP);

            // Calculate the midpoint between Index MCP and Pinky MCP
            Vector3 midpoint = (indexMCP + pinkyMCP) / 2f;

            // 2. X axis (wrist width direction)
            // X = normalize(pinky (17) - index (5))
            xAxis = (pinkyMCP - indexMCP).normalized;

            // 3. Y axis (arm direction)
            // Y = normalize(wrist (0) - midpoint(index, pinky))
            // This vector points from the knuckles towards the forearm
            yAxis = (wrist - midpoint).normalized;

            // 4. Z axis (surface normal)
            // Z = normalize(cross(X, Y))
            // Assuming Unity's left-handed coordinate system, this cross product 
            // naturally points "outward" (away from the skin on the back of the hand)
            zAxis = Vector3.Cross(xAxis, yAxis).normalized;

            // 5. Re-orthogonalize Y axis
            // Y = normalize(cross(Z, X))
            // This ensures our 3 axes remain perfectly perpendicular 
            // even if the tracked landmarks form an oblique triangle.
            yAxis = Vector3.Cross(zAxis, xAxis).normalized;
        }
    }
}

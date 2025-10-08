using UnityEngine;

namespace XRTemplate.AssemblySystem
{
    /// <summary>
    /// Operation that handles snapping a part to a target connection point.
    /// Configuration lives here, execution logic is handled by VRAssemblyManager.
    /// </summary>
    [CreateAssetMenu(menuName = "XRTemplate/Assembly/Operations/SnapPartOperation")]
    public class SnapPartOperation : VRStepOperationBase
    {
        [Header("Snap Settings")]
        [Tooltip("Index of the connection point inside the part to snap to.")]
        public int ConnectionPointIndex = 0;

        [Tooltip("Optional ghost offset for placement preview.")]
        public Vector3 GhostOffset = Vector3.zero;

        [Tooltip("Distance threshold to auto-snap (in meters).")]
        [Min(0.01f)] public float SnapThreshold = 0.05f;

        private bool isCompleted;

        #region IStepOperation Implementation
        public override void Execute()
        {
            // Execution handled externally by VRAssemblyManager.
            // This method acts as an entry point for triggering snap validation.
            Debug.Log($"[SnapPartOperation] Execute called for CP Index {ConnectionPointIndex} | Threshold = {SnapThreshold}");
        }

        public override bool IsCompleted() => isCompleted;
        #endregion

        /// <summary>
        /// Called by VRAssemblyManager once snap is successfully completed.
        /// </summary>
        public void MarkCompleted()
        {
            isCompleted = true;
            Debug.Log($"[SnapPartOperation] Completed at ConnectionPoint {ConnectionPointIndex}");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (SnapThreshold < 0.01f)
                SnapThreshold = 0.01f; // enforce minimum
        }
#endif
    }
}

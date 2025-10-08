using UnityEngine;

namespace XRTemplate.AssemblySystem
{
    /// <summary>
    /// Operation for cleaning a part using a tool or particle effect.
    /// </summary>
    [CreateAssetMenu(menuName = "XRTemplate/Assembly/Operations/CleanOperation")]
    public class CleanOperation : VRStepOperationBase
    {
        [Header("Clean Settings")]
        [Tooltip("Tool prefab for cleaning.")]
        public GameObject ToolPrefab;

        [Tooltip("Optional particle effect for cleaning.")]
        public ParticleSystem CleaningEffect;

        private bool isCompleted = false;

        #region IStepOperation Implementation
        public override void Execute()
        {
            // Execution handled externally by VRAssemblyManager or interaction system.
            Debug.Log($"[CleanOperation] Execute called: Cleaning using {ToolPrefab?.name ?? "No Tool Assigned"}");
        }

        public override bool IsCompleted() => isCompleted;
        #endregion

        /// <summary>
        /// Called when cleaning is successfully completed.
        /// </summary>
        public void MarkCompleted()
        {
            isCompleted = true;
            Debug.Log("[CleanOperation] Completed cleaning operation.");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (ToolPrefab == null)
                Debug.LogWarning("[CleanOperation] ToolPrefab is not assigned.");

            if (CleaningEffect == null)
                Debug.Log("[CleanOperation] No CleaningEffect assigned. This is optional.");
        }
#endif
    }
}

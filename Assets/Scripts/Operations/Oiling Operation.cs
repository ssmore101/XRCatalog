using UnityEngine;

namespace XRTemplate.AssemblySystem
{
    /// <summary>
    /// Operation for applying grease to a part using a tool or applicator.
    /// </summary>
    [CreateAssetMenu(menuName = "XRTemplate/Assembly/Operations/GreaseOperation")]
    public class GreaseOperation : VRStepOperationBase
    {
        [Header("Grease Settings")]
        [Tooltip("Amount of grease to apply (ml or units).")]
        [Min(0f)] public float Amount = 1f;

        [Tooltip("Prefab of the grease tool or applicator.")]
        public GameObject ToolPrefab;

        private bool isCompleted = false;

        #region IStepOperation Implementation
        public override void Execute()
        {
            // Execution handled externally by VRAssemblyManager or interaction system.
            Debug.Log($"[GreaseOperation] Execute called: Apply {Amount} units with {ToolPrefab?.name ?? "No Tool Assigned"}");
        }

        public override bool IsCompleted() => isCompleted;
        #endregion

        /// <summary>
        /// Called when grease application is successfully completed.
        /// </summary>
        public void MarkCompleted()
        {
            isCompleted = true;
            Debug.Log($"[GreaseOperation] Completed: {Amount} units applied.");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Amount < 0f)
                Amount = 0f;

            if (ToolPrefab == null)
                Debug.LogWarning("[GreaseOperation] ToolPrefab is not assigned.");
        }
#endif
    }
}

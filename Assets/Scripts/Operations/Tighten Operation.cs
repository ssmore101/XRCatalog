using UnityEngine;

namespace XRTemplate.AssemblySystem
{
    /// <summary>
    /// Operation for tightening a bolt using a tool with a specified torque value.
    /// </summary>
    [CreateAssetMenu(menuName = "XRTemplate/Assembly/Operations/TightenBoltOperation")]
    public class TightenBoltOperation : VRStepOperationBase
    {
        [Header("Tighten Settings")]
        [Tooltip("Torque value to apply (Nm).")]
        [Min(0f)] public float TorqueValue = 10f;

        [Tooltip("Prefab of the tool (e.g., spanner) required for this operation).")]
        public GameObject ToolPrefab;

        private bool isCompleted = false;

        #region IStepOperation Implementation
        public override void Execute()
        {
            // Execution handled externally by VRAssemblyManager or interaction system.
            Debug.Log($"[TightenBoltOperation] Execute called: Apply {TorqueValue}Nm with {ToolPrefab?.name ?? "No Tool Assigned"}");
        }

        public override bool IsCompleted() => isCompleted;
        #endregion

        /// <summary>
        /// Called when torque application is successfully completed.
        /// </summary>
        public void MarkCompleted()
        {
            isCompleted = true;
            Debug.Log($"[TightenBoltOperation] Completed with Torque {TorqueValue}Nm");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (TorqueValue < 0f)
                TorqueValue = 0f;

            if (ToolPrefab == null)
                Debug.LogWarning("[TightenBoltOperation] ToolPrefab is not assigned.");
        }
#endif
    }
}

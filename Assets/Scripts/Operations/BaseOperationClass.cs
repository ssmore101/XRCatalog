using UnityEngine;

namespace XRTemplate.AssemblySystem
{
    /// <summary>
    /// Base class for all VR assembly operations.
    /// </summary>
    public abstract class VRStepOperationBase : ScriptableObject, IStepOperation
    {
        [Header("Operation Info")]
        [Tooltip("Name of the operation for debug/log purposes.")]
        public string OperationName;

        /// <summary>
        /// Execute the operation logic (e.g., snap part, apply torque).
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Returns true when this operation is completed.
        /// </summary>
        public abstract bool IsCompleted();
    }
}

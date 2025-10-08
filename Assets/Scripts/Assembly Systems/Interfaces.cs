using UnityEngine;

namespace XRTemplate.AssemblySystem
{
    /// <summary>
    /// Interface for any part or subassembly that can be connected in the assembly system.
    /// </summary>
    public interface IConnectable
    {
        /// <summary>
        /// Unique identifier for the part/subassembly.
        /// </summary>
        string PartId { get; }

        /// <summary>
        /// Called when this object is successfully connected.
        /// </summary>
        void OnConnected();

        /// <summary>
        /// Called when this object is disconnected.
        /// </summary>
        void OnDisconnected();
    }

    /// <summary>
    /// Interface for any operation that can be executed as part of a VR assembly step.
    /// </summary>
    public interface IStepOperation
    {
        /// <summary>
        /// Execute the operation logic (e.g., snap part, apply torque, grease).
        /// </summary>
        void Execute();

        /// <summary>
        /// Returns true when the operation is completed.
        /// </summary>
        bool IsCompleted();
    }
}

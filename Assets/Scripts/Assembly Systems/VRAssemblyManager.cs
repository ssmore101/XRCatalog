using System.Collections.Generic;
using UnityEngine;

namespace XRTemplate.AssemblySystem
{
    /// <summary>
    /// Central manager for handling VR assembly sequences using table-based parts.
    /// Handles ghost previews, LOD swap, picking from table, and executing step operations.
    /// Memory-efficient, modular, and scalable for large assemblies.
    /// </summary>
    public class VRAssemblyManager : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Root parent for spawning/placing real part instances.")]
        public Transform PartParent;

        [Tooltip("Optional root parent for organizing ghost previews.")]
        public Transform GhostParent;

        [Header("Active State (Runtime Only)")]
        [SerializeField, Tooltip("Currently loaded assembly sequence.")]
        private VRAssemblySequence activeSequence;

        [SerializeField, Tooltip("Index of the current step in the sequence.")]
        private int currentStepIndex = -1;

        /// <summary>
        /// Currently active ghost instance for previewing placement.
        /// </summary>
        private GameObject currentGhostInstance;

        /// <summary>
        /// Currently picked/placed real part instance.
        /// </summary>
        private TablePart currentPickedPart;

        #region Public API

        /// <summary>
        /// Loads a new assembly sequence and resets progress.
        /// </summary>
        public void LoadSequence(VRAssemblySequence sequence)
        {
            if (sequence == null)
            {
                Debug.LogError("[VRAssemblyManager] Cannot load null sequence.");
                return;
            }

            CleanupActiveStep();

            activeSequence = sequence;
            currentStepIndex = -1;

            Debug.Log($"[VRAssemblyManager] Loaded sequence: {sequence.SequenceName}");
        }

        /// <summary>
        /// Move to the next step in the sequence.
        /// </summary>
        public void NextStep()
        {
            if (activeSequence == null)
            {
                Debug.LogWarning("[VRAssemblyManager] No sequence loaded.");
                return;
            }

            int nextIndex = currentStepIndex + 1;
            if (nextIndex >= activeSequence.StepCount)
            {
                Debug.Log("[VRAssemblyManager] Reached end of sequence.");
                return;
            }

            SetStep(nextIndex);
        }

        /// <summary>
        /// Move to the previous step in the sequence.
        /// </summary>
        public void PreviousStep()
        {
            if (activeSequence == null)
            {
                Debug.LogWarning("[VRAssemblyManager] No sequence loaded.");
                return;
            }

            int prevIndex = currentStepIndex - 1;
            if (prevIndex < 0)
            {
                Debug.Log("[VRAssemblyManager] Already at first step.");
                return;
            }

            SetStep(prevIndex);
        }

        /// <summary>
        /// Jump directly to a specific step index.
        /// </summary>
        public void SetStep(int index)
        {
            if (activeSequence == null)
            {
                Debug.LogWarning("[VRAssemblyManager] No sequence loaded.");
                return;
            }

            if (index < 0 || index >= activeSequence.StepCount)
            {
                Debug.LogError($"[VRAssemblyManager] Invalid step index: {index}");
                return;
            }

            CleanupActiveStep();

            currentStepIndex = index;
            VRAssemblyStep step = activeSequence.GetStep(index);

            if (step == null)
            {
                Debug.LogError($"[VRAssemblyManager] Step {index} is null.");
                return;
            }

            Debug.Log($"[VRAssemblyManager] Starting step {index}: {step.StepName}");

            // Spawn ghost preview
            SpawnGhost(step);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Spawns the ghost prefab for placement preview.
        /// </summary>
        private void SpawnGhost(VRAssemblyStep step)
        {
            if (step.Part == null)
            {
                Debug.LogError("[VRAssemblyManager] Step has no part definition.");
                return;
            }

            if (step.Part.GhostPrefab != null)
            {
                // Position ghost at first connection point + optional offset
                Vector3 ghostPos = step.Part.ConnectionPoints.Count > 0
                    ? step.Part.ConnectionPoints[0].LocalPosition + step.GhostOffset
                    : step.GhostOffset;

                Quaternion ghostRot = step.Part.ConnectionPoints.Count > 0
                    ? Quaternion.Euler(step.Part.ConnectionPoints[0].LocalRotation)
                    : Quaternion.identity;

                currentGhostInstance = Instantiate(
                    step.Part.GhostPrefab,
                    ghostPos,
                    ghostRot,
                    GhostParent != null ? GhostParent : transform
                );
                currentGhostInstance.name = $"Ghost_{step.Part.PartName}";
            }
        }

        /// <summary>
        /// Called when user picks a part from the table.
        /// Matches picked part with current step PartDefinition.
        /// </summary>
        public void PickPartFromTable(TablePart tablePart)
        {
            if (tablePart == null || activeSequence == null)
                return;

            VRAssemblyStep step = activeSequence.GetStep(currentStepIndex);
            if (step == null)
                return;

            if (tablePart.PartDefinition != step.Part)
            {
                Debug.LogWarning($"[VRAssemblyManager] Picked part '{tablePart.PartDefinition.PartName}' is not for current step '{step.Part.PartName}'");
                return;
            }

            // Switch to high LOD for placement
            tablePart.SwitchLOD(true);

            currentPickedPart = tablePart;

            Debug.Log($"[VRAssemblyManager] Picked part '{tablePart.PartDefinition.PartName}' for step '{step.StepName}'");
        }

        /// <summary>
        /// Call this when user places/snaps the part correctly.
        /// </summary>
        public void PlacePickedPart()
        {
            if (currentPickedPart == null)
            {
                Debug.LogWarning("[VRAssemblyManager] No part picked to place.");
                return;
            }

            VRAssemblyStep step = activeSequence.GetStep(currentStepIndex);
            if (step == null)
                return;

            // Snap to target connection point
            if (step.Part.ConnectionPoints.Count > 0)
            {
                ConnectionPoint cp = step.Part.ConnectionPoints[0];
                currentPickedPart.transform.position = cp.LocalPosition;
                currentPickedPart.transform.rotation = Quaternion.Euler(cp.LocalRotation);
            }

            currentPickedPart.OnPlaced();

            // Destroy ghost
            if (currentGhostInstance != null)
            {
                Destroy(currentGhostInstance);
                currentGhostInstance = null;
            }

            // Execute step operations
            foreach (var op in step.Operations)
            {
                if (op != null)
                    op.Execute();
            }

            Debug.Log($"[VRAssemblyManager] Placed part '{currentPickedPart.PartDefinition.PartName}' and executed operations.");

            currentPickedPart = null;
        }

        /// <summary>
        /// Cleans up ghost/part instances from the current step.
        /// </summary>
        private void CleanupActiveStep()
        {
            if (currentGhostInstance != null)
                Destroy(currentGhostInstance);

            currentGhostInstance = null;
            currentPickedPart = null;
        }

        #endregion
    }
}

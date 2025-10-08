using System.Collections.Generic;
using UnityEngine;

namespace XRTemplate.AssemblySystem
{
    /// <summary>
    /// Represents a single step in an assembly sequence.
    /// Each step contains a part reference, instructions, and a list of operations.
    /// </summary>
    [System.Serializable]
    public class VRAssemblyStep
    {
        [Header("Step Info")]
        [Tooltip("Name of the step for easy identification.")]
        public string StepName;

        [Header("Step Core")]
        [Tooltip("Reference to the part or subassembly to assemble.")]
        public PartDefinition Part;

        [Header("Guidance")]
        [Tooltip("Optional ghost offset for preview.")]
        public Vector3 GhostOffset;

        [Tooltip("Text instruction for this step.")]
        [TextArea]
        public string TextInstruction;

        [Tooltip("Voice instruction for this step.")]
        public AudioClip VoiceInstruction;

        [Header("Operations")]
        [Tooltip("List of operations to execute for this step.")]
        public List<VRStepOperationBase> Operations = new List<VRStepOperationBase>();

        [Header("Validation")]
        [Tooltip("If true, checks that part is present on table before starting this step.")]
        public bool ValidatePartPresence = true;

        #region Runtime Checks
        /// <summary>
        /// Validate this step at runtime or in editor.
        /// </summary>
        public void ValidateStep()
        {
            if (Part == null)
            {
                Debug.LogError($"[VRAssemblyStep] Step '{StepName}' has no Part assigned!");
            }

            if (Operations != null)
            {
                foreach (var op in Operations)
                {
                    if (op == null)
                        Debug.LogWarning($"[VRAssemblyStep] Step '{StepName}' has a null operation in list.");
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// ScriptableObject representing an ordered sequence of assembly steps.
    /// Can be used for main assemblies or subassemblies.
    /// </summary>
    [CreateAssetMenu(fileName = "NewVRAssemblySequence", menuName = "XRTemplate/Assembly/VRAssemblySequence")]
    public class VRAssemblySequence : ScriptableObject
    {
        [Header("Sequence Info")]
        [Tooltip("Name of the assembly sequence.")]
        public string SequenceName;

        [Tooltip("Optional description or notes about this assembly sequence.")]
        [TextArea]
        public string Description;

        [Header("Assembly Steps")]
        [Tooltip("Ordered list of assembly steps.")]
        public List<VRAssemblyStep> Steps = new List<VRAssemblyStep>();

        #region Utility Methods
        /// <summary>
        /// Safely get a step at a given index.
        /// </summary>
        public VRAssemblyStep GetStep(int index)
        {
            if (index >= 0 && index < Steps.Count)
                return Steps[index];

            Debug.LogWarning($"[VRAssemblySequence] Requested step index {index} out of range in sequence '{SequenceName}'");
            return null;
        }

        /// <summary>
        /// Total number of steps in this sequence.
        /// </summary>
        public int StepCount => Steps.Count;

        /// <summary>
        /// Runtime validation of all steps in this sequence.
        /// </summary>
        public void ValidateSequence()
        {
            if (Steps == null || Steps.Count == 0)
            {
                Debug.LogWarning($"[VRAssemblySequence] Sequence '{SequenceName}' has no steps defined.");
                return;
            }

            foreach (var step in Steps)
            {
                if (step != null)
                    step.ValidateStep();
                else
                    Debug.LogWarning($"[VRAssemblySequence] Null step found in sequence '{SequenceName}'.");
            }
        }
        #endregion
    }
}

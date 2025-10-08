using UnityEngine;
using System.Collections.Generic;

namespace XRTemplate.AssemblySystem
{
    /// <summary>
    /// Component to attach to parts on the assembly table.
    /// Handles LOD, PartDefinition reference, and placement logic.
    /// </summary>
    public class TablePart : MonoBehaviour
    {
        [Header("Part Info")]
        public PartDefinition PartDefinition;

        [Header("LOD")]
        [Tooltip("Assign low LOD mesh for table.")]
        public GameObject LowLOD;

        [Tooltip("Assign high LOD mesh for assembly.")]
        public GameObject HighLOD;

        // Registry to find table parts by ID
        private static Dictionary<string, TablePart> partRegistry = new Dictionary<string, TablePart>();

        private void Awake()
        {
            if (PartDefinition != null && !partRegistry.ContainsKey(PartDefinition.PartId))
            {
                partRegistry.Add(PartDefinition.PartId, this);
            }

            // Ensure correct LOD at start
            SwitchLOD(false);
        }

        private void OnDestroy()
        {
            if (PartDefinition != null && partRegistry.ContainsKey(PartDefinition.PartId))
                partRegistry.Remove(PartDefinition.PartId);
        }

        /// <summary>
        /// Static helper to find a part on the table by PartId.
        /// </summary>
        public static TablePart GetPartById(string partId)
        {
            if (partRegistry.TryGetValue(partId, out TablePart part))
                return part;
            return null;
        }

        /// <summary>
        /// Switch between low/high LOD meshes.
        /// </summary>
        public void SwitchLOD(bool highLOD)
        {
            if (LowLOD != null)
                LowLOD.SetActive(!highLOD);
            if (HighLOD != null)
                HighLOD.SetActive(highLOD);
        }

        /// <summary>
        /// Called when part is snapped/placed in assembly.
        /// Can add further logic if needed.
        /// </summary>
        public void OnPlaced()
        {
            // Example: disable table interaction, freeze rigidbody, etc.
            var rb = GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = true;
        }
    }
}

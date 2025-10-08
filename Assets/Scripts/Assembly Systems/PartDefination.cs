using System;
using System.Collections.Generic;
using UnityEngine;

namespace XRTemplate.AssemblySystem
{
    /// <summary>
    /// Represents a part or subassembly in the VR assembly system.
    /// Contains prefab references, connection points, and optional subassembly sequence.
    /// </summary>
    [CreateAssetMenu(fileName = "NewPartDefinition", menuName = "XRTemplate/Assembly/PartDefinition")]
    public class PartDefinition : ScriptableObject, IConnectable
    {
        [Header("Identity")]
        [Tooltip("Display name of the part or subassembly.")]
        public string PartName;

        [Tooltip("Unique identifier for this part. Auto-generated if empty.")]
        [SerializeField] private string partId;
        public string PartId => partId;

        [Header("Prefabs")]
        [Tooltip("Main interactable prefab.")]
        public GameObject Prefab;

        [Tooltip("Optional ghost prefab for placement preview.")]
        public GameObject GhostPrefab;

        [Header("Properties")]
        [Tooltip("Weight for physics/assembly realism.")]
        public float Weight = 1f;

        [Tooltip("Mark as a tool if applicable.")]
        public bool IsTool = false;

        [Header("Connection Points")]
        [Tooltip("List of connection points inside this part/subassembly.")]
        public List<ConnectionPoint> ConnectionPoints = new List<ConnectionPoint>();

        [Header("Optional Subassembly Sequence")]
        [Tooltip("Define assembly steps if this is a subassembly.")]
        public VRAssemblySequence SubAssemblySequence;

        #region IConnectable Implementation
        public void OnConnected()
        {
            // Trigger any effects, analytics, or events on connection
        }

        public void OnDisconnected()
        {
            // Trigger any effects or events on disconnection
        }
        #endregion

        #region Unity Callbacks
        private void OnEnable()
        {
            // Ensure unique PartId
            if (string.IsNullOrEmpty(partId))
            {
                partId = Guid.NewGuid().ToString();
            }

            // Validate prefabs
            if (Prefab == null)
            {
                Debug.LogWarning($"[PartDefinition] Prefab not assigned for Part: {PartName}");
            }

            if (GhostPrefab == null)
            {
                Debug.LogWarning($"[PartDefinition] GhostPrefab not assigned for Part: {PartName}");
            }
        }
        #endregion
    }

    /// <summary>
    /// Represents a connection point inside a PartDefinition.
    /// Stores local position and rotation relative to the part prefab.
    /// </summary>
    [Serializable]
    public class ConnectionPoint
    {
        [Tooltip("Unique identifier for the connection point.")]
        public string PointId;

        [Tooltip("Local position relative to the part.")]
        public Vector3 LocalPosition;

        [Tooltip("Local rotation relative to the part (Euler angles).")]
        public Vector3 LocalRotation;

        /// <summary>
        /// Returns world position based on the parent transform.
        /// </summary>
        public Vector3 GetWorldPosition(Transform parent) => parent.TransformPoint(LocalPosition);

        /// <summary>
        /// Returns world rotation based on the parent transform.
        /// </summary>
        public Quaternion GetWorldRotation(Transform parent) => parent.rotation * Quaternion.Euler(LocalRotation);
    }
}

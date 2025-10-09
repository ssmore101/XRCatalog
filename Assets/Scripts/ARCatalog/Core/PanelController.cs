/*
 * Author      : Swapnil More
 * Description : Attach this script to any UI panel prefab with a close button.
 *               Automatically closes the panel when the assigned close button is clicked.
 */

using UnityEngine;
using UnityEngine.UI;

namespace ARCatalogSystem
{
    /// <summary>
    /// Controls a UI panel with a close button.
    /// Ensures panel closes safely when button is pressed.
    /// </summary>
    public class PanelController : MonoBehaviour
    {
        #region Inspector References

        [Header("Close Button")]
        [Tooltip("Assign the close button of this panel.")]
        public Button closeButton;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(ClosePanel);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Closes the panel safely by deactivating it.
        /// </summary>
        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}

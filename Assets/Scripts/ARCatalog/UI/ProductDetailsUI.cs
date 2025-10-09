/*
 * Author      : Swapnil More
 * Description : Displays product details in the Info panel of the AR Catalog.
 *               Concatenates all info points into a scrollable TextMeshProUGUI.
 *               Listens to EventBus for product selection events.
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARCatalogSystem
{
    /// <summary>
    /// Handles the Info panel UI for a selected product.
    /// Updates product name and concatenated information text.
    /// </summary>
    public class ProductDetailsUI : MonoBehaviour
    {
        #region Inspector References

        [Header("UI References")]
        [Tooltip("TextMeshProUGUI for displaying the product name.")]
        public TextMeshProUGUI productNameText;

        [Tooltip("TextMeshProUGUI for displaying all product information points.")]
        public TextMeshProUGUI informationText;

        [Tooltip("Close button to hide the info panel.")]
        public Button closeButton;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            EventBus.OnProductSelected += UpdateInfoPanel;
        }

        private void OnDisable()
        {
            EventBus.OnProductSelected -= UpdateInfoPanel;
        }

        private void Start()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the Info panel with the selected product's details.
        /// Concatenates all info points into a single text field.
        /// </summary>
        /// <param name="product">The selected product.</param>
        private void UpdateInfoPanel(Product product)
        {
            if (product == null) return;

            // Update product name
            if (productNameText != null)
                productNameText.text = product.productName;

            // Concatenate all info points
            if (informationText != null)
            {
                if (product.infoPoints != null && product.infoPoints.Count > 0)
                {
                    string combinedInfo = "";
                    foreach (var point in product.infoPoints)
                    {
                        if (point == null) continue;
                        combinedInfo += $"<b>{point.infoTitle}:</b>\n{point.infoDescription}\n\n";
                    }
                    informationText.text = combinedInfo;
                }
                else
                {
                    informationText.text = "No detailed information available.";
                }
            }
        }

        #endregion
    }
}

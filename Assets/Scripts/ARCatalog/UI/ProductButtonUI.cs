/*
 * Author      : Swapnil More
 * Description : UI component for product buttons in the AR Catalog.
 *               Updates product name, thumbnail, and handles click events.
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARCatalogSystem
{
    /// <summary>
    /// Handles the display and click interaction for a product button prefab.
    /// </summary>
    public class ProductButtonUI : MonoBehaviour
    {
        #region Inspector References

        [Header("UI Components")]
        [Tooltip("Text component to display the product name.")]
        public TextMeshProUGUI productNameText;

        [Tooltip("Image component to display the product thumbnail.")]
        public Image productThumbnailImage;

        [Tooltip("Button component to detect user clicks.")]
        public Button button;

        #endregion

        #region Public Methods

        /// <summary>
        /// Assigns product data to UI elements and sets the click callback.
        /// </summary>
        /// <param name="product">The product data to display.</param>
        /// <param name="onClick">Callback invoked when button is clicked.</param>
        public void SetProduct(Product product, System.Action<Product> onClick)
        {
            if (product == null) return;

            if (productNameText != null)
                productNameText.text = product.productName;

            if (productThumbnailImage != null && product.productThumbnail != null)
                productThumbnailImage.sprite = product.productThumbnail;

            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => onClick?.Invoke(product));
            }
        }

        #endregion
    }
}

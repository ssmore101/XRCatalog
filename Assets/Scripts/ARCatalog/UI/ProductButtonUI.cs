using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARCatalogSystem
{
    public class ProductButtonUI : MonoBehaviour
    {
        public TextMeshProUGUI productNameText;
        public Image productThumbnailImage;
        public Button button;

        public void SetProduct(Product product, System.Action<Product> onClick)
        {
            if (productNameText != null) productNameText.text = product.productName;
            if (productThumbnailImage != null && product.productThumbnail != null) productThumbnailImage.sprite = product.productThumbnail;

            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => { Debug.Log($"[ProductButtonUI] Clicked {product.productName}"); onClick?.Invoke(product); });
            }
        }
    }
}

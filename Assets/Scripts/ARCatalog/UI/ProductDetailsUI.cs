using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ARCatalogSystem
{
    public class ProductDetailsUI : MonoBehaviour
    {
        public TextMeshProUGUI productNameText;
        public TextMeshProUGUI informationText;
        public Button closeButton;

        private void Awake()
        {
            EventBus.OnProductSelected += UpdateInfoPanel;
            Debug.Log("[ProductDetailsUI] Subscribed to OnProductSelected in Awake.");
        }

        private void OnDestroy()
        {
            EventBus.OnProductSelected -= UpdateInfoPanel;
        }

        private void Start()
        {
            if (closeButton != null) closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        private void UpdateInfoPanel(Product product)
        {
            Debug.Log($"[ProductDetailsUI] UpdateInfoPanel called for: {product?.productName}");
            if (product == null) return;

            if (productNameText != null) productNameText.text = product.productName ?? "";
            if (informationText != null) informationText.text = string.IsNullOrEmpty(product.information) ? "No detailed information available." : product.information;
        }
    }
}

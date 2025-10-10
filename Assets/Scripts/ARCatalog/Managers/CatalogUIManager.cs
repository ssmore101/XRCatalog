using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARCatalogSystem
{
    public class CatalogUIManager : MonoBehaviour
    {
        [Header("Catalog Data")]
        public CatalogSO catalogData;

        [Header("UI References")]
        public Transform categoryButtonParent;
        public Transform productButtonParent;
        public GameObject categoryButtonPrefab;
        public GameObject productButtonPrefab;

        [Header("Startup Settings")]
        public bool loadDefaultProductOnStart = false;

        private readonly List<GameObject> _categoryButtons = new List<GameObject>();
        private readonly List<GameObject> _productButtons = new List<GameObject>();

        private void Start()
        {
            Debug.Log("[CatalogUIManager] Start");
            if (catalogData == null)
            {
                Debug.LogError("[CatalogUIManager] catalogData is null!");
                return;
            }

            GenerateCategoryButtons();

            if (loadDefaultProductOnStart && catalogData.HasCategories)
            {
                Category firstCategory = catalogData.categories[0];
                HandleCategorySelected(firstCategory);

                if (firstCategory.products != null && firstCategory.products.Count > 0)
                {
                    // defer one frame to be safe (listeners may subscribe in Awake)
                    StartCoroutine(DeferredProductSelection(firstCategory.products[0]));
                }
            }
            else
            {
                Debug.Log("[CatalogUIManager] Lazy mode: waiting for user selection.");
            }
        }

        private IEnumerator DeferredProductSelection(Product p)
        {
            yield return null;
            HandleProductSelected(p);
        }

        private void GenerateCategoryButtons()
        {
            ClearChildren(categoryButtonParent, _categoryButtons);

            if (catalogData.categories == null) return;

            foreach (var category in catalogData.categories)
            {
                if (category == null) continue;
                var go = Instantiate(categoryButtonPrefab, categoryButtonParent);
                go.name = category.categoryName ?? "Category";
                var ui = go.GetComponent<CategoryButtonUI>();
                ui?.SetCategory(category, HandleCategorySelected);
                _categoryButtons.Add(go);
            }

            Debug.Log($"[CatalogUIManager] Generated {_categoryButtons.Count} category buttons.");
        }

        private void HandleCategorySelected(Category category)
        {
            Debug.Log($"[CatalogUIManager] Category selected: {category?.categoryName}");
            if (category == null) return;
            DataBridge.SelectedCategory = category;
            GenerateProductButtons(category);
        }

        private void GenerateProductButtons(Category category)
        {
            ClearChildren(productButtonParent, _productButtons);
            if (category?.products == null) return;

            foreach (var product in category.products)
            {
                if (product == null) continue;
                var go = Instantiate(productButtonPrefab, productButtonParent);
                go.name = product.productName ?? "Product";
                var ui = go.GetComponent<ProductButtonUI>();
                ui?.SetProduct(product, HandleProductSelected);
                _productButtons.Add(go);
            }

            Debug.Log($"[CatalogUIManager] Generated {_productButtons.Count} product buttons for category '{category.categoryName}'.");
        }

        private void HandleProductSelected(Product product)
        {
            Debug.Log($"[CatalogUIManager] Product selected: {product?.productName}");
            if (product == null) return;

            DataBridge.SelectedProduct = product;
            EventBus.RaiseProductSelected(product); // updates info UI (if subscribed)

            // Close catalog panel only (keep others closed)
            UIButtonsManager.Instance?.CloseAllPanelsExcept();

            if (!product.HasValidModel)
            {
                Debug.LogWarning("[CatalogUIManager] Product model missing: " + product.productName);
                ErrorPopupManager.Instance?.ShowError("3D model not available for this product.", 5f);
            }
        }

        private void ClearChildren(Transform parent, List<GameObject> list)
        {
            if (parent == null || list == null) return;
            foreach (Transform t in parent) if (t != null) Destroy(t.gameObject);
            list.Clear();
        }
    }
}

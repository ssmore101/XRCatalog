/*
 * Author      : Swapnil More
 * Description : Dynamically generates category and product buttons for the AR Catalog UI.
 *               Automatically selects the first category and product on start.
 *               Fires EventBus events when a product or category is selected.
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ARCatalogSystem
{
    /// <summary>
    /// Manages the AR Catalog UI, including categories and products.
    /// Handles dynamic generation and selection logic.
    /// </summary>
    public class CatalogUIManager : MonoBehaviour
    {
        #region Inspector References

        [Header("Catalog Data")]
        [Tooltip("Reference to the Catalog ScriptableObject containing all categories and products.")]
        public CatalogSO catalogData;

        [Header("UI References")]
        [Tooltip("Parent transform where category buttons will be instantiated.")]
        public Transform categoryButtonParent;

        [Tooltip("Parent transform where product buttons will be instantiated.")]
        public Transform productButtonParent;

        [Tooltip("Prefab for category buttons.")]
        public GameObject categoryButtonPrefab;

        [Tooltip("Prefab for product buttons.")]
        public GameObject productButtonPrefab;

        #endregion

        #region Private Variables

        private List<GameObject> _categoryButtons = new List<GameObject>();
        private List<GameObject> _productButtons = new List<GameObject>();

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (catalogData == null)
            {
                Debug.LogError("[CatalogUIManager] CatalogSO reference is missing!");
                return;
            }

            // Generate category buttons
            GenerateCategoryButtons();

            // Auto-select the first category and first product
            if (catalogData.categories.Count > 0 && catalogData.categories[0] != null)
            {
                Category firstCategory = catalogData.categories[0];
                SelectCategory(firstCategory);

                if (firstCategory.products != null && firstCategory.products.Count > 0)
                {
                    Product firstProduct = firstCategory.products[0];
                    SelectProduct(firstProduct);
                }
            }
        }

        #endregion

        #region Category Buttons

        /// <summary>
        /// Dynamically generates category buttons based on CatalogSO.
        /// </summary>
        private void GenerateCategoryButtons()
        {
            // Clear existing buttons
            foreach (Transform child in categoryButtonParent)
                Destroy(child.gameObject);
            _categoryButtons.Clear();

            // Instantiate new buttons
            foreach (var category in catalogData.categories)
            {
                if (category == null) continue;

                GameObject btnGO = Instantiate(categoryButtonPrefab, categoryButtonParent);
                btnGO.name = category.categoryName;

                var btnUI = btnGO.GetComponent<CategoryButtonUI>();
                if (btnUI != null)
                    btnUI.SetCategory(category, SelectCategory);

                _categoryButtons.Add(btnGO);
            }
        }

        /// <summary>
        /// Handles category selection, updates DataBridge, and regenerates product buttons.
        /// Auto-selects the first product of the category.
        /// </summary>
        /// <param name="category">Selected category.</param>
        private void SelectCategory(Category category)
        {
            if (category == null) return;

            DataBridge.SelectedCategory = category;

            // Generate product buttons for this category
            GenerateProductButtons(category);

            // Auto-select first product
            if (category.products != null && category.products.Count > 0)
                SelectProduct(category.products[0]);
        }

        #endregion

        #region Product Buttons

        /// <summary>
        /// Dynamically generates product buttons for the selected category.
        /// </summary>
        /// <param name="category">Category to display products from.</param>
        private void GenerateProductButtons(Category category)
        {
            // Clear existing buttons
            foreach (Transform child in productButtonParent)
                Destroy(child.gameObject);
            _productButtons.Clear();

            // Instantiate new buttons
            foreach (var product in category.products)
            {
                if (product == null) continue;

                GameObject btnGO = Instantiate(productButtonPrefab, productButtonParent);
                btnGO.name = product.productName;

                var btnUI = btnGO.GetComponent<ProductButtonUI>();
                if (btnUI != null)
                    btnUI.SetProduct(product, SelectProduct);

                _productButtons.Add(btnGO);
            }
        }

        /// <summary>
        /// Handles product selection, updates DataBridge, and fires EventBus events.
        /// </summary>
        /// <param name="product">Selected product.</param>
        private void SelectProduct(Product product)
        {
            if (product == null) return;

            DataBridge.SelectedProduct = product;

            // Fire events to update info and media panels
            EventBus.RaiseProductSelected(product);
            EventBus.RaiseOpenMediaRequested(product);
        }

        #endregion
    }
}

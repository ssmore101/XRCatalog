using UnityEngine;

namespace ARCatalogSystem
{
    /// <summary>
    /// Centralized data holder for currently selected category and product.
    /// Automatically notifies all listeners via <see cref="EventBus"/> when selections change.
    /// </summary>
    public static class DataBridge
    {
        #region Selected Category

        private static Category _selectedCategory;

        /// <summary>
        /// Gets or sets the currently selected <see cref="Category"/>.
        /// When the value changes, <see cref="EventBus.RaiseCategorySelected"/> is triggered.
        /// </summary>
        public static Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (value == _selectedCategory || value == null) return;

                _selectedCategory = value;
                EventBus.RaiseCategorySelected(_selectedCategory);
            }
        }

        #endregion

        #region Selected Product

        private static Product _selectedProduct;

        /// <summary>
        /// Gets or sets the currently selected <see cref="Product"/>.
        /// When the value changes, <see cref="EventBus.RaiseProductSelected"/> is triggered.
        /// </summary>
        public static Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (value == _selectedProduct || value == null) return;

                _selectedProduct = value;
                EventBus.RaiseProductSelected(_selectedProduct);
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Clears the current selections for both category and product.
        /// Does not trigger events.
        /// </summary>
        public static void ClearSelection()
        {
            _selectedCategory = null;
            _selectedProduct = null;
        }

        #endregion
    }
}

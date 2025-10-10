using UnityEngine;

namespace ARCatalogSystem
{
    public static class DataBridge
    {
        private static Category _selectedCategory;
        public static Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (value == _selectedCategory || value == null) return;
                _selectedCategory = value;
                Debug.Log("[DataBridge] SelectedCategory set: " + _selectedCategory.categoryName);
                EventBus.RaiseCategorySelected(_selectedCategory);
            }
        }

        private static Product _selectedProduct;
        public static Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (value == _selectedProduct || value == null) return;
                _selectedProduct = value;
                Debug.Log("[DataBridge] SelectedProduct set: " + _selectedProduct.productName);
                EventBus.RaiseProductSelected(_selectedProduct);
            }
        }

        public static void ClearSelection()
        {
            _selectedCategory = null;
            _selectedProduct = null;
        }
    }
}

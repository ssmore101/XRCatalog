using System;
using UnityEngine;

namespace ARCatalogSystem
{
    public static class EventBus
    {
        public static event Action<Product> OnProductSelected;
        public static event Action<Category> OnCategorySelected;
        public static event Action<Product> OnOpenMediaRequested;
        public static event Action<Product> OnARViewRequested;
        public static event Action<int> OnBackgroundChanged;
        public static event Action OnScreenshotRequested;

        public static void RaiseProductSelected(Product p)
        {
            if (p == null) { Debug.LogWarning("[EventBus] RaiseProductSelected called with null."); return; }
            OnProductSelected?.Invoke(p);
        }

        public static void RaiseCategorySelected(Category c)
        {
            if (c == null) { Debug.LogWarning("[EventBus] RaiseCategorySelected called with null."); return; }
            OnCategorySelected?.Invoke(c);
        }

        public static void RaiseOpenMediaRequested(Product p)
        {
            if (p == null) { Debug.LogWarning("[EventBus] RaiseOpenMediaRequested null."); return; }
            OnOpenMediaRequested?.Invoke(p);
        }

        public static void RaiseARViewRequested(Product p)
        {
            if (p == null) { Debug.LogWarning("[EventBus] RaiseARViewRequested null."); return; }
            OnARViewRequested?.Invoke(p);
        }

        public static void RaiseBackgroundChanged(int idx) => OnBackgroundChanged?.Invoke(idx);
        public static void RaiseScreenshotRequested() => OnScreenshotRequested?.Invoke();
    }
}

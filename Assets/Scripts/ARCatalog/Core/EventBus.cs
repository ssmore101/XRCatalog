using System;
using UnityEngine;

namespace ARCatalogSystem
{
    /// <summary>
    /// Global static event bus for decoupled communication between UI and core logic.
    /// Allows any system to subscribe to events such as product selection, category selection, media requests, AR view, background changes, and screenshots.
    /// </summary>
    public static class EventBus
    {
        #region Product Events

        /// <summary>
        /// Event triggered when a <see cref="Product"/> is selected from the catalog.
        /// </summary>
        public static event Action<Product> OnProductSelected;

        /// <summary>
        /// Safely invokes the <see cref="OnProductSelected"/> event.
        /// </summary>
        /// <param name="product">The product that was selected.</param>
        public static void RaiseProductSelected(Product product)
        {
            if (product == null)
            {
                Debug.LogWarning("[EventBus] Tried to raise OnProductSelected with NULL product.");
                return;
            }

            OnProductSelected?.Invoke(product);
        }

        #endregion

        #region Category Events

        /// <summary>
        /// Event triggered when a <see cref="Category"/> is selected from the catalog.
        /// </summary>
        public static event Action<Category> OnCategorySelected;

        /// <summary>
        /// Safely invokes the <see cref="OnCategorySelected"/> event.
        /// </summary>
        /// <param name="category">The category that was selected.</param>
        public static void RaiseCategorySelected(Category category)
        {
            if (category == null)
            {
                Debug.LogWarning("[EventBus] Tried to raise OnCategorySelected with NULL category.");
                return;
            }

            OnCategorySelected?.Invoke(category);
        }

        #endregion

        #region Media Events

        /// <summary>
        /// Event triggered when the user requests to open the media panel for a product.
        /// </summary>
        public static event Action<Product> OnOpenMediaRequested;

        /// <summary>
        /// Safely invokes the <see cref="OnOpenMediaRequested"/> event.
        /// </summary>
        /// <param name="product">The product for which media panel is requested.</param>
        public static void RaiseOpenMediaRequested(Product product)
        {
            if (product == null)
            {
                Debug.LogWarning("[EventBus] Tried to raise OnOpenMediaRequested with NULL product.");
                return;
            }

            OnOpenMediaRequested?.Invoke(product);
        }

        #endregion

        #region AR View Event

        /// <summary>
        /// Event triggered when the user requests AR view for a selected product.
        /// </summary>
        public static event Action<Product> OnARViewRequested;

        /// <summary>
        /// Safely invokes the <see cref="OnARViewRequested"/> event.
        /// </summary>
        /// <param name="product">The product for AR view request.</param>
        public static void RaiseARViewRequested(Product product)
        {
            if (product == null)
            {
                Debug.LogWarning("[EventBus] Tried to raise OnARViewRequested with NULL product.");
                return;
            }

            OnARViewRequested?.Invoke(product);
        }

        #endregion

        #region Background and Screenshot Events

        /// <summary>
        /// Event triggered when the background environment index changes (e.g., 0 = City, 1 = Road, 2 = Studio).
        /// </summary>
        public static event Action<int> OnBackgroundChanged;

        /// <summary>
        /// Safely invokes the <see cref="OnBackgroundChanged"/> event.
        /// </summary>
        /// <param name="envIndex">The new background environment index.</param>
        public static void RaiseBackgroundChanged(int envIndex)
        {
            OnBackgroundChanged?.Invoke(envIndex);
        }

        /// <summary>
        /// Event triggered when a screenshot is requested.
        /// </summary>
        public static event Action OnScreenshotRequested;

        /// <summary>
        /// Safely invokes the <see cref="OnScreenshotRequested"/> event.
        /// </summary>
        public static void RaiseScreenshotRequested()
        {
            OnScreenshotRequested?.Invoke();
        }

        #endregion
    }
}

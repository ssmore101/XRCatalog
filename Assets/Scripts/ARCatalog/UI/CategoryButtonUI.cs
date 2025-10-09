/*
 * Author      : Swapnil More
 * Description : UI component for category buttons in the AR Catalog.
 *               Updates text and icon dynamically and handles click events.
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARCatalogSystem
{
    /// <summary>
    /// Handles the display and click interaction for a category button prefab.
    /// </summary>
    public class CategoryButtonUI : MonoBehaviour
    {
        #region Inspector References

        [Header("UI Components")]
        [Tooltip("Text component to display the category name.")]
        public TextMeshProUGUI categoryNameText;

        [Tooltip("Image component to display the category icon.")]
        public Image categoryIconImage;

        [Tooltip("Button component to detect user clicks.")]
        public Button button;

        #endregion

        #region Public Methods

        /// <summary>
        /// Assigns category data to UI elements and sets the click callback.
        /// </summary>
        /// <param name="category">The category data to display.</param>
        /// <param name="onClick">Callback invoked when button is clicked.</param>
        public void SetCategory(Category category, System.Action<Category> onClick)
        {
            if (category == null) return;

            if (categoryNameText != null)
                categoryNameText.text = category.categoryName;

            if (categoryIconImage != null)
                categoryIconImage.sprite = category.categoryIcon;

            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => onClick?.Invoke(category));
            }
        }

        #endregion
    }
}

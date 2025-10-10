using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARCatalogSystem
{
    public class CategoryButtonUI : MonoBehaviour
    {
        public TextMeshProUGUI categoryNameText;
        public Image categoryIconImage;
        public Button button;

        public void SetCategory(Category category, System.Action<Category> onClick)
        {
            if (categoryNameText != null) categoryNameText.text = category.categoryName;
            if (categoryIconImage != null) categoryIconImage.sprite = category.categoryIcon;
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => { Debug.Log($"[CategoryButtonUI] Clicked {category.categoryName}"); onClick?.Invoke(category); });
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace ARCatalogSystem
{
    public class PanelController : MonoBehaviour
    {
        public Button closeButton;

        private void Awake()
        {
            if (closeButton != null) closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}

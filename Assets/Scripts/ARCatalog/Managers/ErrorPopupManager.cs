using UnityEngine;
using TMPro;
using System.Collections;

namespace ARCatalogSystem
{
    public class ErrorPopupManager : MonoBehaviour
    {
        public static ErrorPopupManager Instance { get; private set; }

        public GameObject popupPanel;
        public TextMeshProUGUI popupText;
        public CanvasGroup popupCanvasGroup;
        public float popupDuration = 5f;
        private Coroutine _current;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (popupPanel != null) popupPanel.SetActive(false);
            if (popupCanvasGroup != null) popupCanvasGroup.alpha = 0f;
        }

        public void ShowError(string message, float duration = -1f)
        {
            if (popupPanel == null || popupText == null)
            {
                Debug.LogWarning("[ErrorPopupManager] UI refs missing.");
                return;
            }

            popupText.text = message;
            popupPanel.SetActive(true);

            if (_current != null) StopCoroutine(_current);
            _current = StartCoroutine(DoHideAfter(duration > 0 ? duration : popupDuration));
        }

        private IEnumerator DoHideAfter(float seconds)
        {
            float fade = 0.25f;
            if (popupCanvasGroup != null)
            {
                float t = 0f;
                while (t < fade)
                {
                    t += Time.deltaTime;
                    popupCanvasGroup.alpha = Mathf.Clamp01(t / fade);
                    yield return null;
                }
                popupCanvasGroup.alpha = 1f;
            }

            yield return new WaitForSeconds(seconds);

            if (popupCanvasGroup != null)
            {
                float t = 0f;
                while (t < fade)
                {
                    t += Time.deltaTime;
                    popupCanvasGroup.alpha = 1f - Mathf.Clamp01(t / fade);
                    yield return null;
                }
                popupCanvasGroup.alpha = 0f;
            }

            popupPanel.SetActive(false);
        }
    }
}

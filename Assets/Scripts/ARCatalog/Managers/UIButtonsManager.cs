using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARCatalogSystem
{
    public class UIButtonsManager : MonoBehaviour
    {
        [Header("Buttons")]
        public Button menuButton;
        public Button catalogButton;
        public Button infoButton;
        public Button mediaButton;
        public Button screenshotButton;
        public Button viewInARButton;
        public Button backgroundButton;
        public Button helpButton;
        public Button settingsButton;
        public Button quitButton;

        [Header("Panels")]
        public GameObject menuPanel;
        public GameObject catalogPanel;
        public GameObject infoPanel;
        public GameObject mediaPanel;
        public GameObject helpPanel;
        public GameObject settingsPanel;
        public GameObject quitPanel;

        [Header("Backgrounds")]
        public List<GameObject> backgroundObjects;

        [Header("Quit Buttons")]
        public Button quitYesButton;
        public Button quitNoButton;

        private List<GameObject> _allPanels = new List<GameObject>();
        private int _currentBackgroundIndex = 0;

        public static UIButtonsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            _allPanels = new List<GameObject> { menuPanel, catalogPanel, infoPanel, mediaPanel, helpPanel, settingsPanel, quitPanel };
            SetupButtons();
            Debug.Log("[UIButtonsManager] Initialized.");
        }

        private void SetupButtons()
        {
            menuButton?.onClick.AddListener(() => TogglePanel(menuPanel));
            catalogButton?.onClick.AddListener(() => { CloseAllPanelsExcept(catalogPanel); TogglePanel(catalogPanel); });
            infoButton?.onClick.AddListener(() => { CloseAllPanelsExcept(infoPanel); TogglePanel(infoPanel); });

            mediaButton?.onClick.AddListener(() =>
            {
                var selected = DataBridge.SelectedProduct;
                if (selected != null && (selected.HasImages || selected.HasVideo))
                {
                    CloseAllPanelsExcept(mediaPanel);
                    mediaPanel.SetActive(true);
                    EventBus.RaiseOpenMediaRequested(selected);
                }
                else
                {
                    ErrorPopupManager.Instance?.ShowError("No media available for this product", 5f);
                }
            });

            screenshotButton?.onClick.AddListener(() => EventBus.RaiseScreenshotRequested());
            viewInARButton?.onClick.AddListener(() =>
            {
                var selected = DataBridge.SelectedProduct;
                if (selected != null && selected.HasValidModel) EventBus.RaiseARViewRequested(selected);
                else ErrorPopupManager.Instance?.ShowError("Cannot view in AR. Model not available.", 5f);
            });

            backgroundButton?.onClick.AddListener(CycleBackgrounds);
            helpButton?.onClick.AddListener(() => { CloseAllPanelsExcept(helpPanel); TogglePanel(helpPanel); });
            settingsButton?.onClick.AddListener(() => { CloseAllPanelsExcept(settingsPanel); TogglePanel(settingsPanel); });
            quitButton?.onClick.AddListener(() => { CloseAllPanelsExcept(quitPanel); quitPanel.SetActive(true); });

            quitYesButton?.onClick.AddListener(QuitApplication);
            quitNoButton?.onClick.AddListener(() => quitPanel?.SetActive(false));
        }

        public void CloseAllPanelsExcept(params GameObject[] keepOpen)
        {
            foreach (var p in _allPanels)
            {
                if (p == null) continue;
                bool keep = keepOpen != null && System.Array.Exists(keepOpen, x => x == p);
                if (!keep) p.SetActive(false);
            }
        }

        private void TogglePanel(GameObject panel)
        {
            if (panel == null) return;
            bool active = panel.activeSelf;
            CloseAllPanelsExcept(panel);
            panel.SetActive(!active);
        }

        private void CycleBackgrounds()
        {
            if (backgroundObjects == null || backgroundObjects.Count == 0) return;
            backgroundObjects[_currentBackgroundIndex].SetActive(false);
            _currentBackgroundIndex = (_currentBackgroundIndex + 1) % backgroundObjects.Count;
            backgroundObjects[_currentBackgroundIndex].SetActive(true);
            EventBus.RaiseBackgroundChanged(_currentBackgroundIndex);
        }

        private void QuitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

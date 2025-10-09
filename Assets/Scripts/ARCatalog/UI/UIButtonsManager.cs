/*
 * Author      : Swapnil More
 * Description : Handles all main UI buttons and panel management for the AR Catalog.
 *               Ensures only one panel is open at a time and closes others automatically.
 *               Supports catalog, info, media, menu, background, help, settings, and quit panels.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARCatalogSystem
{
    /// <summary>
    /// Central manager for all UI buttons and panels in the AR Catalog.
    /// Handles mutual exclusivity, background cycling, and product-dependent media panels.
    /// </summary>
    public class UIButtonsManager : MonoBehaviour
    {
        #region Inspector References

        [Header("Buttons")]
        [Tooltip("Menu panel toggle button.")]
        public Button menuButton;
        [Tooltip("Catalog panel toggle button.")]
        public Button catalogButton;
        [Tooltip("Screenshot button.")]
        public Button screenshotButton;
        [Tooltip("View in AR button.")]
        public Button viewInARButton;
        [Tooltip("Info panel button.")]
        public Button infoButton;
        [Tooltip("Background cycle button.")]
        public Button backgroundButton;
        [Tooltip("Media panel button.")]
        public Button mediaButton;
        [Tooltip("Help panel button.")]
        public Button helpButton;
        [Tooltip("Settings panel button.")]
        public Button settingsButton;
        [Tooltip("Quit panel button.")]
        public Button quitButton;

        [Header("Panels")]
        [Tooltip("Menu panel GameObject.")]
        public GameObject menuPanel;
        [Tooltip("Catalog panel GameObject.")]
        public GameObject catalogPanel;
        [Tooltip("Info panel GameObject.")]
        public GameObject infoPanel;
        [Tooltip("Media panel GameObject.")]
        public GameObject mediaPanel;
        [Tooltip("Help panel GameObject.")]
        public GameObject helpPanel;
        [Tooltip("Settings panel GameObject.")]
        public GameObject settingsPanel;
        [Tooltip("Quit confirmation panel GameObject.")]
        public GameObject quitPanel;

        [Header("Backgrounds")]
        [Tooltip("List of background objects to cycle through.")]
        public List<GameObject> backgroundObjects;

        [Header("Quit Panel Buttons")]
        [Tooltip("Yes button to quit application.")]
        public Button quitYesButton;
        [Tooltip("No button to cancel quit.")]
        public Button quitNoButton;

        #endregion

        #region Private Variables

        private int _currentBackgroundIndex = 0;
        private List<GameObject> _allPanels = new List<GameObject>();

        #endregion

        #region Singleton

        public static UIButtonsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        #endregion

        #region Unity Methods

        private void Start()
        {
            // Collect all panels for mutual exclusivity
            _allPanels = new List<GameObject>()
            {
                menuPanel, catalogPanel, infoPanel, mediaPanel, helpPanel, settingsPanel, quitPanel
            };

            SetupAllButtons();
        }

        #endregion

        #region Button Setup

        /// <summary>
        /// Initializes all button callbacks.
        /// </summary>
        private void SetupAllButtons()
        {
            // Menu
            if (menuButton != null)
                menuButton.onClick.AddListener(() => TogglePanel(menuPanel));

            // Catalog
            if (catalogButton != null)
                catalogButton.onClick.AddListener(() =>
                {
                    CloseAllPanelsExcept(catalogPanel);
                    TogglePanel(catalogPanel);
                });

            // Screenshot
            if (screenshotButton != null)
                screenshotButton.onClick.AddListener(() => EventBus.RaiseScreenshotRequested());

            // View in AR
            if (viewInARButton != null)
                viewInARButton.onClick.AddListener(() =>
                {
                    CloseAllPanelsExcept();
                    OnViewInAR();
                });

            // Info Panel
            if (infoButton != null)
                infoButton.onClick.AddListener(() =>
                {
                    CloseAllPanelsExcept(infoPanel);
                    infoPanel.SetActive(!infoPanel.activeSelf);
                });

            // Background
            if (backgroundButton != null)
                backgroundButton.onClick.AddListener(() =>
                {
                    CloseAllPanelsExcept();
                    CycleBackgrounds();
                });

            // Media Panel
            if (mediaButton != null)
                mediaButton.onClick.AddListener(() =>
                {
                    var selectedProduct = DataBridge.SelectedProduct;
                    if (selectedProduct != null)
                    {
                        CloseAllPanelsExcept(mediaPanel);
                        mediaPanel.SetActive(true);
                        EventBus.RaiseOpenMediaRequested(selectedProduct);
                    }
                    else
                        Debug.LogWarning("[UIButtonsManager] No product selected to show media!");
                });

            // Help Panel
            if (helpButton != null)
                helpButton.onClick.AddListener(() =>
                {
                    CloseAllPanelsExcept(helpPanel);
                    helpPanel.SetActive(!helpPanel.activeSelf);
                });

            // Settings Panel
            if (settingsButton != null)
                settingsButton.onClick.AddListener(() =>
                {
                    CloseAllPanelsExcept(settingsPanel);
                    settingsPanel.SetActive(!settingsPanel.activeSelf);
                });

            // Quit Panel
            if (quitButton != null)
                quitButton.onClick.AddListener(() =>
                {
                    CloseAllPanelsExcept(quitPanel);
                    quitPanel.SetActive(true);
                });

            // Quit Panel Yes/No buttons
            if (quitYesButton != null)
                quitYesButton.onClick.AddListener(OnQuitConfirmed);

            if (quitNoButton != null)
                quitNoButton.onClick.AddListener(() =>
                {
                    if (quitPanel != null) quitPanel.SetActive(false);
                });
        }

        #endregion

        #region Panel Management

        /// <summary>
        /// Toggles the target panel on/off.
        /// </summary>
        private void TogglePanel(GameObject panel)
        {
            if (panel == null) return;

            bool isActive = panel.activeSelf;
            CloseAllPanelsExcept(panel);
            panel.SetActive(!isActive);
        }

        /// <summary>
        /// Closes all panels except specified ones.
        /// </summary>
        /// <param name="keepOpen">Panels to keep open</param>
        public void CloseAllPanelsExcept(params GameObject[] keepOpen)
        {
            foreach (var panel in _allPanels)
            {
                if (panel == null) continue;
                bool keep = keepOpen != null && System.Array.Exists(keepOpen, x => x == panel);
                if (!keep) panel.SetActive(false);
            }
        }

        #endregion

        #region Background Management

        /// <summary>
        /// Cycles through the background objects in order.
        /// </summary>
        private void CycleBackgrounds()
        {
            if (backgroundObjects == null || backgroundObjects.Count == 0) return;

            backgroundObjects[_currentBackgroundIndex].SetActive(false);
            _currentBackgroundIndex = (_currentBackgroundIndex + 1) % backgroundObjects.Count;
            backgroundObjects[_currentBackgroundIndex].SetActive(true);

            EventBus.RaiseBackgroundChanged(_currentBackgroundIndex);
        }

        #endregion

        #region AR Handling

        /// <summary>
        /// Fires the AR view request for the selected product.
        /// </summary>
        private void OnViewInAR()
        {
            var selectedProduct = DataBridge.SelectedProduct;
            if (selectedProduct != null)
                EventBus.RaiseARViewRequested(selectedProduct);
            else
                Debug.LogWarning("[UIButtonsManager] No product selected for AR view!");
        }

        #endregion

        #region Quit Handling

        /// <summary>
        /// Handles quit confirmation.
        /// </summary>
        private void OnQuitConfirmed()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #endregion
    }
}

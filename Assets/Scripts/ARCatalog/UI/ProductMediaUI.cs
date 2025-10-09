/*
 * Author      : Swapnil More
 * Description : Handles Media Panel UI for a selected product. 
 *               Displays product images and video, and shows error messages 
 *               when media is unavailable. Error messages appear only when relevant buttons are clicked.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

namespace ARCatalogSystem
{
    /// <summary>
    /// Manages the Media Panel UI for displaying product images and videos.
    /// Handles button interactions and error messages.
    /// </summary>
    public class ProductMediaUI : MonoBehaviour
    {
        #region Inspector References

        [Header("Media Display")]
        [Tooltip("UI Image to display product images.")]
        public Image displayImage;

        [Tooltip("Video Player to play product videos.")]
        public VideoPlayer videoPlayer;

        [Header("Error Handling")]
        [Tooltip("Panel to show errors when media is not available.")]
        public GameObject errorPanel;

        [Tooltip("Text component to show error messages.")]
        public TextMeshProUGUI errorText;

        [Header("Control Buttons")]
        [Tooltip("Button to close media panel.")]
        public Button closeButton;

        [Tooltip("Button to show next image.")]
        public Button nextButton;

        [Tooltip("Button to show previous image.")]
        public Button previousButton;

        [Tooltip("Button to switch to images view.")]
        public Button imagesButton;

        [Tooltip("Button to switch to video view.")]
        public Button videoButton;

        #endregion

        #region Private Variables

        private List<Sprite> _images = new List<Sprite>();
        private VideoClip _video;
        private int _currentIndex = 0;
        private bool _showingImages = true;
        private Coroutine _errorCoroutine;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            EventBus.OnOpenMediaRequested += OpenMedia;
        }

        private void OnDisable()
        {
            EventBus.OnOpenMediaRequested -= OpenMedia;
        }

        private void Start()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(() => gameObject.SetActive(false));

            if (nextButton != null)
                nextButton.onClick.AddListener(ShowNext);

            if (previousButton != null)
                previousButton.onClick.AddListener(ShowPrevious);

            if (imagesButton != null)
                imagesButton.onClick.AddListener(SwitchToImages);

            if (videoButton != null)
                videoButton.onClick.AddListener(SwitchToVideo);

            // Ensure media panel is initially hidden
            gameObject.SetActive(false);
        }

        #endregion

        #region Media Handling

        /// <summary>
        /// Initializes media panel with selected product's images and video.
        /// </summary>
        /// <param name="product">Selected product to display media for.</param>
        private void OpenMedia(Product product)
        {
            if (product == null) return;

            _images = product.productImages ?? new List<Sprite>();
            _video = product.productVideo;
            _currentIndex = 0;
            _showingImages = (_images.Count > 0);

            gameObject.SetActive(true);
            displayImage.gameObject.SetActive(false);
            videoPlayer.gameObject.SetActive(false);
            CloseErrorPanel();

            UpdateMediaDisplay();
        }

        /// <summary>
        /// Updates the display to show current image or video.
        /// </summary>
        private void UpdateMediaDisplay()
        {
            if (_showingImages && _images.Count > 0)
            {
                displayImage.gameObject.SetActive(true);
                videoPlayer.gameObject.SetActive(false);
                displayImage.sprite = _images[_currentIndex];
            }
            else if (!_showingImages && _video != null)
            {
                displayImage.gameObject.SetActive(false);
                videoPlayer.gameObject.SetActive(true);
                videoPlayer.clip = _video;
                videoPlayer.Play();
            }
        }

        /// <summary>
        /// Shows next image in the gallery.
        /// </summary>
        private void ShowNext()
        {
            if (!_showingImages || _images.Count == 0) return;
            _currentIndex = (_currentIndex + 1) % _images.Count;
            UpdateMediaDisplay();
        }

        /// <summary>
        /// Shows previous image in the gallery.
        /// </summary>
        private void ShowPrevious()
        {
            if (!_showingImages || _images.Count == 0) return;
            _currentIndex--;
            if (_currentIndex < 0) _currentIndex = _images.Count - 1;
            UpdateMediaDisplay();
        }

        /// <summary>
        /// Switches display to product images.
        /// Shows error if no images are available.
        /// </summary>
        private void SwitchToImages()
        {
            if (_images.Count == 0)
            {
                ShowError("No product images available.", 3f);
                return;
            }
            _showingImages = true;
            _currentIndex = 0;
            UpdateMediaDisplay();
        }

        /// <summary>
        /// Switches display to product video.
        /// Shows error if no video is available.
        /// </summary>
        private void SwitchToVideo()
        {
            if (_video == null)
            {
                ShowError("No product video available.", 3f);
                return;
            }
            _showingImages = false;
            UpdateMediaDisplay();
        }

        #endregion

        #region Error Handling

        /// <summary>
        /// Shows error message in the error panel for a duration.
        /// </summary>
        /// <param name="message">Error message to display.</param>
        /// <param name="duration">Duration in seconds to show the error.</param>
        private void ShowError(string message, float duration)
        {
            if (errorPanel != null && errorText != null)
            {
                errorText.text = message;
                errorPanel.SetActive(true);

                if (_errorCoroutine != null) StopCoroutine(_errorCoroutine);
                _errorCoroutine = StartCoroutine(HideErrorAfterDelay(duration));
            }

            displayImage.gameObject.SetActive(false);
            videoPlayer.gameObject.SetActive(false);
        }

        /// <summary>
        /// Coroutine to hide error panel after a delay.
        /// </summary>
        private IEnumerator HideErrorAfterDelay(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            CloseErrorPanel();
        }

        /// <summary>
        /// Hides the error panel immediately.
        /// </summary>
        private void CloseErrorPanel()
        {
            if (errorPanel != null)
                errorPanel.SetActive(false);
        }

        #endregion
    }
}

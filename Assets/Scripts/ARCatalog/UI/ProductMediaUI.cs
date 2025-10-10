using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace ARCatalogSystem
{
    public class ProductMediaUI : MonoBehaviour
    {
        public Image displayImage;
        public VideoPlayer videoPlayer;
        public Button closeButton;
        public Button nextButton;
        public Button previousButton;
        public Button imagesButton;
        public Button videoButton;

        private List<Sprite> _images = new List<Sprite>();
        private VideoClip _video;
        private int _currentIndex = 0;
        private bool _showingImages = true;

        private void Awake()
        {
            EventBus.OnOpenMediaRequested += OpenMedia;
            Debug.Log("[ProductMediaUI] Subscribed to OnOpenMediaRequested in Awake.");
        }

        private void OnDestroy()
        {
            EventBus.OnOpenMediaRequested -= OpenMedia;
        }

        private void Start()
        {
            closeButton?.onClick.AddListener(() => gameObject.SetActive(false));
            nextButton?.onClick.AddListener(ShowNext);
            previousButton?.onClick.AddListener(ShowPrevious);
            imagesButton?.onClick.AddListener(SwitchToImages);
            videoButton?.onClick.AddListener(SwitchToVideo);

            gameObject.SetActive(false);
        }

        private void OpenMedia(Product product)
        {
            Debug.Log($"[ProductMediaUI] OpenMedia called for: {product?.productName}");
            if (product == null) return;

            _images = product.productImages ?? new List<Sprite>();
            _video = product.productVideo;
            _currentIndex = 0;
            _showingImages = (_images.Count > 0);

            Debug.Log($"[ProductMediaUI] images: {_images.Count}, hasVideo: {_video != null}");

            if (_images.Count == 0 && _video == null)
            {
                ErrorPopupManager.Instance?.ShowError("No media available for this product", 5f);
                return;
            }

            gameObject.SetActive(true);
            UpdateMediaDisplay();
        }

        private void UpdateMediaDisplay()
        {
            if (_showingImages)
            {
                if (_images.Count == 0)
                {
                    Debug.Log("[ProductMediaUI] No images to show.");
                    return;
                }

                if (videoPlayer != null && videoPlayer.isPlaying) videoPlayer.Stop();
                if (displayImage != null)
                {
                    displayImage.gameObject.SetActive(true);
                    displayImage.sprite = _images[_currentIndex];
                }

                if (videoPlayer != null) videoPlayer.gameObject.SetActive(false);
            }
            else
            {
                if (_video == null)
                {
                    ErrorPopupManager.Instance?.ShowError("No product video available", 5f);
                    return;
                }

                if (displayImage != null) displayImage.gameObject.SetActive(false);
                if (videoPlayer != null)
                {
                    videoPlayer.gameObject.SetActive(true);
                    videoPlayer.clip = _video;
                    videoPlayer.Play();
                }
            }
        }

        private void ShowNext()
        {
            if (!_showingImages || _images.Count == 0) return;
            _currentIndex = (_currentIndex + 1) % _images.Count;
            UpdateMediaDisplay();
        }

        private void ShowPrevious()
        {
            if (!_showingImages || _images.Count == 0) return;
            _currentIndex--;
            if (_currentIndex < 0) _currentIndex = _images.Count - 1;
            UpdateMediaDisplay();
        }

        private void SwitchToImages()
        {
            if (_images.Count == 0)
            {
                ErrorPopupManager.Instance?.ShowError("No product images available", 5f);
                return;
            }
            _showingImages = true;
            _currentIndex = 0;
            UpdateMediaDisplay();
        }

        private void SwitchToVideo()
        {
            if (_video == null)
            {
                ErrorPopupManager.Instance?.ShowError("No product video available", 5f);
                return;
            }
            _showingImages = false;
            UpdateMediaDisplay();
        }
    }
}

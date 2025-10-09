/*
 * Author      : Swapnil More
 * Description : ScriptableObject for AR Catalog containing all categories, products, 
 *               images, info points, videos, and AR placement settings.
 *               Designed for internal AR catalog application.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace ARCatalogSystem
{
    #region Catalog ScriptableObject

    /// <summary>
    /// ScriptableObject holding all categories and products for the AR Catalog.
    /// </summary>
    [CreateAssetMenu(fileName = "ARCatalog", menuName = "ARCatalog/AllCategories")]
    public class CatalogSO : ScriptableObject
    {
        [Header("Categories")]
        [Tooltip("List of all categories in the AR catalog.")]
        public List<Category> categories = new List<Category>();

        /// <summary>
        /// Returns true if the catalog contains at least one category.
        /// </summary>
        public bool HasCategories => categories != null && categories.Count > 0;
    }

    #endregion

    #region Category

    /// <summary>
    /// Represents a category in the catalog containing multiple products.
    /// </summary>
    [System.Serializable]
    public class Category
    {
        [Header("Category Info")]
        [Tooltip("Name of the category (e.g., Artillery).")]
        public string categoryName;

        [Tooltip("Optional icon representing the category in UI.")]
        public Sprite categoryIcon;

        [Header("Products")]
        [Tooltip("List of products contained in this category.")]
        public List<Product> products = new List<Product>();

        /// <summary>
        /// Returns true if the category contains at least one product.
        /// </summary>
        public bool HasProducts => products != null && products.Count > 0;
    }

    #endregion

    #region Product

    /// <summary>
    /// Represents a product inside a category with media, info points, and AR settings.
    /// </summary>
    [System.Serializable]
    public class Product
    {
        [Header("Product Info")]
        [Tooltip("Name of the product.")]
        public string productName;

        [Tooltip("Thumbnail image for UI.")]
        public Sprite productThumbnail;

        [Tooltip("3D model prefab to spawn in AR.")]
        public GameObject modelPrefab;

        [Header("Product Media")]
        [Tooltip("Gallery of images associated with this product.")]
        public List<Sprite> productImages = new List<Sprite>();

        [Tooltip("Optional video clip for the product.")]
        public VideoClip productVideo;

        [Header("Detailed Information")]
        [Tooltip("List of informational points about the product.")]
        public List<InfoPoint> infoPoints = new List<InfoPoint>();

        [Header("AR Settings")]
        [Tooltip("Offset position when spawning the model in AR.")]
        public Vector3 arSpawnPositionOffset = Vector3.zero;

        [Tooltip("Scale of the product model in AR.")]
        public Vector3 arScale = Vector3.one;

        /// <summary>
        /// Returns true if the product has a valid 3D model prefab assigned.
        /// </summary>
        public bool HasValidModel => modelPrefab != null;

        /// <summary>
        /// Returns true if the product contains at least one info point.
        /// </summary>
        public bool HasInfoPoints => infoPoints != null && infoPoints.Count > 0;

        /// <summary>
        /// Returns true if the product contains at least one image.
        /// </summary>
        public bool HasImages => productImages != null && productImages.Count > 0;

        /// <summary>
        /// Returns true if the product has a video assigned.
        /// </summary>
        public bool HasVideo => productVideo != null;
    }

    #endregion

    #region InfoPoint

    /// <summary>
    /// Represents a single information point for a product with title and description.
    /// </summary>
    [System.Serializable]
    public class InfoPoint
    {
        [Header("Info Point")]
        [Tooltip("Title of the information point.")]
        public string infoTitle;

        [Tooltip("Description or details of the information point.")]
        [TextArea]
        public string infoDescription;
    }

    #endregion
}

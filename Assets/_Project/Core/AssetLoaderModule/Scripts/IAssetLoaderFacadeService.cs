using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Project.Core.AssetLoaderModule 
{
    /// <summary>
    /// Service for simple asset loading and ability to choose to load it from addressables or resources
    /// </summary>
    [PublicAPI]
    public interface IAssetLoaderFacadeService 
    {
        /// <summary>
        /// Loads asset asynchronously and stores it in cache
        /// </summary>
        /// <param name="key">When you load asset from addressables key is address of the asset in asset groups. When you load asset from resources key is path to the asset</param>
        /// <param name="assetLoadSource"></param>
        /// <param name="groupName">Group under which assets are stored. You can release assets from this group, when you no longer need it for otimizing memory usage</param>
        /// <typeparam name="TAsset">Asset type you are trying to load</typeparam>
        /// <returns></returns>
        public Task<TAsset> LoadAssetAsync<TAsset>(string key, AssetLoadSource assetLoadSource, string groupName = "Default") where TAsset : Object;
        /// <summary>
        /// Loads asset and stores it in cache
        /// </summary>
        /// <param name="key">When you load asset from addressables key is address of the asset in asset groups. When you load asset from resources key is path to the asset</param>
        /// <param name="assetLoadSource"></param>
        /// <param name="groupName">Group under which assets are stored. You can release assets from this group, when you no longer need it for otimizing memory usage</param>
        /// <typeparam name="TAsset">Asset type you are trying to load</typeparam>
        /// <returns></returns>
        public TAsset LoadAsset<TAsset>(string key, AssetLoadSource assetLoadSource, string groupName = "Default") where TAsset : Object;
        /// <summary>
        /// Release cached assets under specified group from memory
        /// </summary>
        /// <param name="assetLoadSource"></param>
        /// <param name="groupName">Group you want to release</param>
        public void ReleaseAssetsInGroup(AssetLoadSource assetLoadSource, string groupName = "Default");
        /// <summary>
        /// Release all cached assets from memory
        /// </summary>
        /// <param name="assetLoadSource"></param>
        public void ReleaseAllAssets(AssetLoadSource assetLoadSource);
        /// <summary>
        /// Check if asset you are searching for is cached in memory
        /// </summary>
        /// <param name="key">When you load asset from addressables key is address of the asset in asset groups. When you load asset from resources key is path to the asset</param>
        /// <param name="assetLoadSource"></param>
        /// <param name="groupName">Group under which assets are stored.</param>
        /// <returns></returns>
        public bool HasLoadedAsset(string key, AssetLoadSource assetLoadSource, string groupName = "Default");
        /// <summary>
        /// Get seperate Asset Loader Service with already specified asset loader source
        /// </summary>
        /// <param name="assetLoadSource"></param>
        /// <returns></returns>
        IAssetLoaderService GetAssetLoaderService(AssetLoadSource assetLoadSource);
    }
}
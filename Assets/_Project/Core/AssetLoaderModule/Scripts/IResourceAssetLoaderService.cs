using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Core.AssetLoaderModule 
{
    /// <summary>
    /// Service from loading assets from Resources
    /// </summary>
    public interface IResourceAssetLoaderService : IAssetLoaderService 
    {
        /// <summary>
        /// Loads asset asynchronously and stores it in cache
        /// </summary>
        /// <param name="key">Path to the asset</param>
        /// <param name="groupName">Group under which assets are stored. You can release assets from this group, when you no longer need it for otimizing memory usage</param>
        /// <typeparam name="TAsset">Asset type you are trying to load</typeparam>
        /// <returns></returns>
        public new UniTask<TAsset> LoadAssetAsync<TAsset>(string key, string groupName = "Default") where TAsset : Object;
        /// <summary>
        /// Loads asset and stores it in cache
        /// </summary>
        /// <param name="key">Path to the asset</param>
        /// <param name="groupName">Group under which assets are stored. You can release assets from this group, when you no longer need it for otimizing memory usage</param>
        /// <typeparam name="TAsset">Asset type you are trying to load</typeparam>
        /// <returns></returns>
        public new TAsset LoadAsset<TAsset>(string key, string groupName = "Default") where TAsset : Object;
        /// <summary>
        /// Release cached assets under specified group from memory
        /// </summary>
        /// <param name="groupName">Group you want to release</param>
        public new void ReleaseAssetsInGroup(string groupName = "Default");
        /// <summary>
        /// Release all cached assets from memory
        /// </summary>
        public new void ReleaseAllAssets();
        /// <summary>
        /// Check if asset you are searching for is cached in memory
        /// </summary>
        /// <param name="key">Path to the asset</param>
        /// <param name="groupName">Group under which assets are stored.</param>
        /// <returns></returns>
        public new bool HasLoadedAsset(string key, string groupName = "Default");
    }
}
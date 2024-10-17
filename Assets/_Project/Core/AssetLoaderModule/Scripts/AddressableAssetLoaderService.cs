using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Object = UnityEngine.Object;

namespace Project.Core.AssetLoaderModule 
{
    public class AddressableAssetLoaderService : IAddressablesAssetLoaderService 
    {
        private const float TIME_OUT_THRESHOLD = 10f;
        protected readonly Dictionary<string, AddressablesGroupHandleContainer> _handlesContainerByGroupName = new();
        public async UniTask<TAsset> LoadAssetAsync<TAsset>(string key, string groupName = "Default") where TAsset : Object 
        {
            AddressablesGroupHandleContainer handleContainer = GetHandleContainer(groupName);

            return handleContainer.CompletedHandles.TryGetValue(key, out AsyncOperationHandle cachedHandle)
                ? cachedHandle.Result as TAsset
                : await ProcessHandleAsync<TAsset>(key, handleContainer);
        }

        public TAsset LoadAsset<TAsset>(string key, string groupName = "Default")  where TAsset : Object 
        {
            AddressablesGroupHandleContainer handleContainer = GetHandleContainer(groupName);
            
            return handleContainer.CompletedHandles.TryGetValue(key, out AsyncOperationHandle cachedHandle)
                ? cachedHandle.Result as TAsset
                : ProcessHandle<TAsset>(key, handleContainer);
        }

        public void ReleaseAssetsInGroup(string groupName = "Default")
        {
            if (_handlesContainerByGroupName.TryGetValue(groupName, out AddressablesGroupHandleContainer handleContainer) is false)
                return;

            foreach (KeyValuePair<string, List<AsyncOperationHandle>> allHandlesInContainer in handleContainer.AllHandles)
                foreach (AsyncOperationHandle handle in allHandlesInContainer.Value)
                    Addressables.Release(handle);
            
            handleContainer.AllHandles.Clear();
            handleContainer.CompletedHandles.Clear();
        }

        public void ReleaseAllAssets() 
        {
            foreach (KeyValuePair<string, AddressablesGroupHandleContainer> handlesContainer in _handlesContainerByGroupName.ToList()) 
                ReleaseAssetsInGroup(handlesContainer.Key);
        }

        public bool HasLoadedAsset(string key, string groupName = "Default") =>
            _handlesContainerByGroupName.TryGetValue(groupName, out AddressablesGroupHandleContainer handleContainer) is true
            && handleContainer.CompletedHandles.ContainsKey(key);

        private async UniTask<TAsset> ProcessHandleAsync<TAsset>(string key, AddressablesGroupHandleContainer handleContainer) where TAsset : Object 
        {
            AsyncOperationHandle<TAsset> handle = Addressables.LoadAssetAsync<TAsset>(key);
            handle.Completed += completedHandle => handleContainer.CompletedHandles[key] = completedHandle;
            AddHandle(key, handle, handleContainer);

            return await handle.Task;
        }

        private TAsset ProcessHandle<TAsset>(string key, AddressablesGroupHandleContainer handleContainer) where TAsset : Object
        {
            AsyncOperationHandle<TAsset> handle = Addressables.LoadAssetAsync<TAsset>(key);

            handleContainer.CompletedHandles[key] = handle;
            AddHandle(key, handle, handleContainer);
            return handle.WaitForCompletion();
        }

        private void AddHandle<TAsset>(string key, AsyncOperationHandle<TAsset> handle, AddressablesGroupHandleContainer handleContainer) where TAsset : Object 
        {
            if (!handleContainer.AllHandles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles)) {
                resourceHandles = new List<AsyncOperationHandle>();
                handleContainer.AllHandles[key] = resourceHandles;
            }

            resourceHandles.Add(handle);
        }

        private bool ProcessTimeOut<TAsset>(AsyncOperationHandle<TAsset> handle) where TAsset : Object 
        {
            DateTime dateTimeToBreakLoop = DateTime.Now.AddSeconds(TIME_OUT_THRESHOLD);
            while (handle.IsDone is false) {
                if (DateTime.Now > dateTimeToBreakLoop)
                    return true;
            }

            return false;
        }

        private AddressablesGroupHandleContainer GetHandleContainer(string groupName) 
        {
            if (_handlesContainerByGroupName.TryGetValue(groupName, out AddressablesGroupHandleContainer handleContainer) is true)
                return handleContainer;

            handleContainer = new AddressablesGroupHandleContainer();
            _handlesContainerByGroupName[groupName] = handleContainer;
            return handleContainer;
        }
    }
}
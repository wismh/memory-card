using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Project.Core.SceneLoaderServiceModule 
{
    public class AddressablesSceneLoaderService
    {
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> _loadedScenes = new();
        
        public async UniTask LoadSceneAsync(string sceneToLoad, bool unloadRedundant) 
        {
            if (_loadedScenes.ContainsKey(sceneToLoad))
                return;
            
            LoadSceneMode loadSceneMode = unloadRedundant == false
                ? LoadSceneMode.Additive
                : LoadSceneMode.Single;

            AsyncOperationHandle<SceneInstance> asyncOperationHandler = Addressables.LoadSceneAsync(sceneToLoad, loadSceneMode);

            _loadedScenes.Add(sceneToLoad, asyncOperationHandler);
            await asyncOperationHandler.Task;
        }

        public async UniTask LoadScenesAsync(List<string> scenesToLoad, bool unloadRedundant) 
        {
            if (unloadRedundant)
                await UnloadScenesAsync(_loadedScenes.Keys.Except(scenesToLoad).ToList());

            foreach (string sceneName in scenesToLoad)
                await LoadSceneAsync(sceneName, false);
        }

        public async UniTask UnloadSceneAsync(string sceneToUnload) {
            if (_loadedScenes.ContainsKey(sceneToUnload) is false)
            {
                Debug.LogError($"Cannot unload scene {sceneToUnload} as it is not loaded. Ignoring attempt");
                return;
            }
            
            AsyncOperationHandle<SceneInstance> asyncOperationHandler = Addressables.UnloadSceneAsync(_loadedScenes[sceneToUnload]);
            await asyncOperationHandler.Task;
            _loadedScenes.Remove(sceneToUnload);
        }

        public async UniTask UnloadScenesAsync(List<string> scenesToUnload) 
        {
            foreach (string sceneToUnload in scenesToUnload)
                await UnloadSceneAsync(sceneToUnload);
        }
    }
}
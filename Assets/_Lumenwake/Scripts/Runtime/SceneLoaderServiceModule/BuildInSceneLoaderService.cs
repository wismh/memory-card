using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Core.SceneLoaderServiceModule 
{
    public class BuildInSceneLoaderService 
    {
        private readonly List<string> _loadedScenes = new();
        
        public async UniTask LoadSceneAsync(string sceneToLoad, bool unloadRedundant)
        {
            if (_loadedScenes.Contains(sceneToLoad))
                return;
            
            LoadSceneMode loadSceneMode = unloadRedundant == false
                ? LoadSceneMode.Additive
                : LoadSceneMode.Single;
            
            await SceneManager.LoadSceneAsync(sceneToLoad, loadSceneMode);
            _loadedScenes.Add(sceneToLoad);
        }

        public async UniTask LoadScenesAsync(List<string> scenesToLoad, bool unloadRedundant)
        {
            if (unloadRedundant)
                await UnloadScenesAsync(_loadedScenes.Except(scenesToLoad).ToList());

            foreach (string sceneName in scenesToLoad)
                await LoadSceneAsync(sceneName, false);
        }

        public async UniTask UnloadSceneAsync(string sceneToUnload)
        {
            if (_loadedScenes.Contains(sceneToUnload) is false) 
            {
                Debug.LogError($"Cannot unload scene {sceneToUnload} as it is not loaded. Ignoring attempt");
                return;
            }

            await SceneManager.UnloadSceneAsync(sceneToUnload);
            _loadedScenes.Remove(sceneToUnload);
        }

        public async UniTask UnloadScenesAsync(List<string> scenesToUnload)
        {
            foreach (string sceneToUnload in scenesToUnload)
                await UnloadSceneAsync(sceneToUnload);
        }
    }
}
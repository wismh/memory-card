using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Generated;
using UnityEngine.SceneManagement;

namespace Project.Core.SceneLoaderServiceModule 
{
    public class SceneLoaderServiceFacade : ISceneLoaderService {
        private BuildInSceneLoaderService _buildInSceneLoaderService;
        private AddressablesSceneLoaderService _addressablesSceneLoaderService;
        
        public SceneLoaderServiceFacade(BuildInSceneLoaderService buildInSceneLoaderService, AddressablesSceneLoaderService addressablesSceneLoaderService) {
            _addressablesSceneLoaderService = addressablesSceneLoaderService;
            _buildInSceneLoaderService = buildInSceneLoaderService;
        }
    
        public async UniTask LoadSceneAsync(string sceneToLoad, bool unloadRedundant) {
            if (Address.AllSceneKeys.Contains(sceneToLoad))
                await _addressablesSceneLoaderService.LoadSceneAsync(sceneToLoad, unloadRedundant);
            else
                await _buildInSceneLoaderService.LoadSceneAsync(sceneToLoad, unloadRedundant);
        }
        
        public async UniTask LoadScenesAsync(List<string> scenesToLoad, string activeScene, bool unloadRedundant) {
            var scenesNotInAddressables = scenesToLoad.Except(Address.AllSceneKeys).ToList();

            if (scenesNotInAddressables.Any() is false) {
                await _addressablesSceneLoaderService.LoadScenesAsync(scenesToLoad, unloadRedundant);
                SetActiveScene(activeScene);
                return;
            }

            if (scenesNotInAddressables.Count == scenesToLoad.Count) {
                await _buildInSceneLoaderService.LoadScenesAsync(scenesToLoad, unloadRedundant);
                SetActiveScene(activeScene);
                return;
            }

            await _buildInSceneLoaderService.LoadScenesAsync(scenesNotInAddressables, unloadRedundant);
            await _addressablesSceneLoaderService.LoadScenesAsync(scenesToLoad.Except(scenesNotInAddressables).ToList(), unloadRedundant);
            SetActiveScene(activeScene);
        }
        
        private void SetActiveScene(string activeScene) {
            if (SceneManager.GetActiveScene().name == activeScene)
                return;
            
            Scene newActiveScene = SceneManager.GetSceneByName(activeScene);
            SceneManager.SetActiveScene(newActiveScene);
        }

        public async UniTask UnloadSceneAsync(string sceneToUnload) {
            if (Address.AllSceneKeys.Contains(sceneToUnload))
                await _addressablesSceneLoaderService.UnloadSceneAsync(sceneToUnload);
            else
                await _buildInSceneLoaderService.UnloadSceneAsync(sceneToUnload);
        }

        public async UniTask UnloadScenesAsync(List<string> scenesToUnload) {
            var scenesNotInAddressables = scenesToUnload.Except(Address.AllSceneKeys)
                .ToList();

            if (scenesNotInAddressables.Any() is false) {
                await _addressablesSceneLoaderService.UnloadScenesAsync(scenesToUnload);
                return;
            }

            if (scenesNotInAddressables.Count == scenesToUnload.Count) {
                await _buildInSceneLoaderService.UnloadScenesAsync(scenesToUnload);
                return;
            }

            await _buildInSceneLoaderService.UnloadScenesAsync(scenesNotInAddressables);
            await _addressablesSceneLoaderService.UnloadScenesAsync(scenesToUnload.Except(scenesNotInAddressables)
                .ToList());
        }
    }
}
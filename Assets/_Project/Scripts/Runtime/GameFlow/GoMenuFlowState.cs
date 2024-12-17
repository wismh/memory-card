using System.Collections.Generic;
using Project.Core.SceneLoaderServiceModule;
using Project.Core.StateMachineModule;
using Project.Generated;
namespace Project.GameFlow
{
    public class GoMenuFlowState : StateBase
    {
        private readonly ISceneLoaderService _sceneLoaderService;

        public GoMenuFlowState(ISceneLoaderService sceneLoaderService) =>
            _sceneLoaderService = sceneLoaderService;

        public override void Enter() {
            var enabledScenes = new List<string>
            {
                SceneInBuild.GlobalScene, 
                SceneInBuild.MenuScene
            };
                
            _sceneLoaderService.LoadScenesAsync(enabledScenes, SceneInBuild.MenuScene, true);    
        }

        public override void Exit() { }
    }
}

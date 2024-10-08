using System.Collections.Generic;
using Project.Core.SceneLoaderServiceModule;
using Project.Generated;

namespace Project.Features.GameFlowStateMachineModule
{
    public class StartRoundFlowState : GameFlowStateBase
    {
        private readonly ISceneLoaderService _sceneLoaderService;

        public StartRoundFlowState(ISceneLoaderService sceneLoaderService) =>
            _sceneLoaderService = sceneLoaderService;

        public override void Enter() {
            var enabledScenes = new List<string>
            {
                SceneInBuild.GlobalScene, 
                SceneInBuild.SampleScene
            };
                
            _sceneLoaderService.LoadScenesAsync(enabledScenes, SceneInBuild.SampleScene, true);    
        }

        public override void Exit() { }
    }
}

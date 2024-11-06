using Project.Core.SceneLoaderServiceModule;
using Project.Core.StateMachineModule;
using Project.Generated;

namespace Project.Features.GameFlowStateMachineModule 
{
    public class GlobalGameFlowState : StateBase
    {
        private readonly ISceneLoaderService _sceneLoaderService;

        public GlobalGameFlowState(ISceneLoaderService sceneLoaderService) =>
            _sceneLoaderService = sceneLoaderService;

        public override void Enter() =>
            _sceneLoaderService.LoadSceneAsync(SceneInBuild.GlobalScene, true);

        public override void Exit() { }
    }
}
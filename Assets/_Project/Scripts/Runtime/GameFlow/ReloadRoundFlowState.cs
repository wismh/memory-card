using Project.Core.SceneLoaderServiceModule;
using Project.Core.StateMachineModule;
using Project.Generated;

namespace Project.GameFlow
{
    public class ReloadRoundFlowState : StateBase
    {
        private readonly ISceneLoaderService _sceneLoaderService;

        public ReloadRoundFlowState(ISceneLoaderService sceneLoaderService) =>
            _sceneLoaderService = sceneLoaderService;

        public override async void Enter()
        {
            await _sceneLoaderService.UnloadSceneAsync(SceneInBuild.RoundScene);
            await _sceneLoaderService.LoadSceneAsync(SceneInBuild.RoundScene, false);
        }

        public override void Exit() { }
    }
}

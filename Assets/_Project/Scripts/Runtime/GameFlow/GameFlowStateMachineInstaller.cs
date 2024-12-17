using Zenject;

namespace Project.GameFlow
{
    public class GameFlowStateMachineInstaller : Installer<GameFlowStateMachineInstaller> 
    {
        public override void InstallBindings() 
        {
            Container.Bind<GameFlowStateMachine>()
                .AsSingle();

            Container.Bind<GlobalGameFlowState>()
                .AsSingle();
            
            Container.Bind<StartRoundFlowState>()
                .AsSingle();

            Container.Bind<GoMenuFlowState>()
                .AsSingle();

            Container.Bind<ReloadRoundFlowState>()
                .AsSingle();
        }
    }
}
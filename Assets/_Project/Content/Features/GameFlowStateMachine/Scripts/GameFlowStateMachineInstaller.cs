using Zenject;

namespace Project.Features.GameFlowStateMachineModule
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
        }
    }
}
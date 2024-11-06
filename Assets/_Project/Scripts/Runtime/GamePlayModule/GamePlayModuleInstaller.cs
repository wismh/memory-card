using Project.Features.GameFlowStateMachineModule;
using Zenject;

namespace Project.Features.CardModule
{
    public class GamePlayModuleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GamePlayPresenter>()
                .AsSingle()
                .NonLazy();
        }
    }
}

using Zenject;

namespace Project.GamePlay
{
    public class GamePlayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GamePlayPresenter>()
                .AsSingle()
                .NonLazy();
        }
    }
}

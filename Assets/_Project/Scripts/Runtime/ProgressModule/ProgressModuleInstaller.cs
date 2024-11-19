using Zenject;

namespace Project.Features.ProgressModule
{
    public class ProgressModuleInstaller : Installer<ProgressModuleInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameSavePersistence>()
                .To<JsonFileGameSavePersistence>()
                .AsSingle();

            Container.BindInterfacesTo<LevelProgressService>()
                .AsSingle()
                .NonLazy();

            Container.Bind<IMapAnimationIntent>()
                .To<MapAnimationIntent>()
                .AsSingle();
        }
    }
}
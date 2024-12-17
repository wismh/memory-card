using Project.Core.AssetLoaderModule;
using Project.Generated;
using UnityEngine;
using Zenject;

namespace Project.Board
{
    public class BoardInstaller : MonoInstaller
    {
        [SerializeField] private BoardView _board;
        
        public override void InstallBindings()
        {
            var addressablesAssetLoaderService = Container.Resolve<IAddressablesAssetLoaderService>();
            var boardComposerConfig = addressablesAssetLoaderService.LoadAsset<BoardComposerConfig>(Address.DefaultLocalGroup.BoardComposerConfig);
            var boardPresenterConfig = addressablesAssetLoaderService.LoadAsset<BoardPresenterConfig>(Address.DefaultLocalGroup.BoardPresenterConfig);

            Container.Bind<BoardComposerConfig>()
                .FromInstance(boardComposerConfig)
                .AsSingle();
            
            Container.Bind<BoardPresenterConfig>()
                .FromInstance(boardPresenterConfig)
                .AsSingle();
            
            Container.Bind<BoardView>()
                .FromInstance(_board)
                .AsSingle();
            
            Container.Bind<BoardComposer>()
                .AsSingle();
            
            Container.Bind<BoardPresenter>()
                .AsSingle()
                .NonLazy();
        }
    }
}
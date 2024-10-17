using Project.Core.AssetLoaderModule;

using Project.Generated;
using UnityEngine;
using Zenject;

namespace Project.Features.CardModule
{
    [CreateAssetMenu(menuName = "Configurations/Card/" + nameof(CardModuleInstaller),
        fileName = nameof(CardModuleInstaller) + "_Default")]
    public class CardModuleInstaller : ScriptableObjectInstaller<CardModuleInstaller>
    {
        [SerializeField] private CardView _cardPrefab;
        
        public override void InstallBindings()
        {
            var addressablesAssetLoaderService = Container.Resolve<IAddressablesAssetLoaderService>();
            var clipFlipAnimationConfig = addressablesAssetLoaderService.LoadAsset<CardFlipAnimationConfig>(
                Address.DefaultLocalGroup.CardFlipAnimationConfig
            );

            Container.Bind<CardFlipAnimationConfig>()
                .FromInstance(clipFlipAnimationConfig)
                .AsSingle();
            
            Container.Bind<CardFlipAnimator>()
                .AsTransient();
            Container.Bind<CardFactory>()
                .FromInstance(new CardFactory(Container, _cardPrefab));
        }
    }
}

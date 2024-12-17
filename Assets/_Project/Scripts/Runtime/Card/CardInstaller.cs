using Project.Core.AssetLoaderModule;

using Project.Generated;
using UnityEngine;
using Zenject;

namespace Project.Card
{
    [CreateAssetMenu(menuName = "Configurations/Card/" + nameof(CardInstaller),
        fileName = nameof(CardInstaller) + "_Default")]
    public class CardInstaller : ScriptableObjectInstaller<CardInstaller>
    {
        [SerializeField] private CardView _cardPrefab;
        
        public override void InstallBindings()
        {
            var addressablesAssetLoaderService = Container.Resolve<IAddressablesAssetLoaderService>();
            var clipFlipAnimationConfig = addressablesAssetLoaderService.LoadAsset<CardViewConfig>(
                Address.DefaultLocalGroup.CardViewConfig
            );

            Container.Bind<CardViewConfig>()
                .FromInstance(clipFlipAnimationConfig)
                .AsSingle();
            
            Container.Bind<CardFlipAnimator>()
                .AsTransient();
            
            Container.Bind<CardModel>()
                .AsTransient();
            
            Container.Bind<CardFactory>()
                .FromInstance(new CardFactory(Container, _cardPrefab));
        }
    }
}

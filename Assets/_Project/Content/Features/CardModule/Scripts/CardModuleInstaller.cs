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
            Container.Bind<CardFactory>()
                .FromInstance(new CardFactory(Container, _cardPrefab));
        }
    }
}

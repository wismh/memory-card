using UnityEngine;
using Zenject;

namespace Project
{
    public class BoardInstaller : MonoInstaller
    {
        [SerializeField] private CardView _cardPrefab;
        [SerializeField] private BoardView _board;
        
        public override void InstallBindings()
        {
            Container
                .Bind<CardView>()
                .FromInstance(_cardPrefab)
                .WhenInjectedInto<CardFactory>();
            Container.Bind<CardFactory>().AsSingle();

            Container.Bind<BoardView>().FromInstance(_board).AsSingle();
            Container.Bind<BoardComposer>().AsSingle();
        }
    }
}
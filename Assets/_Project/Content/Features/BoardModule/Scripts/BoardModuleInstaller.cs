using UnityEngine;
using Zenject;

namespace Project.Features.BoardModule
{
    public class BoardInstaller : MonoInstaller
    {
        [SerializeField] private BoardView _board;
        
        public override void InstallBindings()
        {
            Container.Bind<BoardView>().FromInstance(_board).AsSingle();
            Container.Bind<BoardComposer>().AsSingle();
        }
    }
}
using UnityEngine;
using Zenject;

using Project.Features.BoardModule;

namespace Project.Features.GameBootstrapModule
{
    public class RoundSceneBootstrap : MonoBehaviour
    {
        private BoardComposer _boardComposer;
        
        [Inject]
        public void InjectDependencies(BoardComposer boardComposer)
        {
            _boardComposer = boardComposer;
        }

        private void Start()
        {
            _boardComposer.Compose();
        }
    }
}

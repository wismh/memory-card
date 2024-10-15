using UnityEngine;
using Zenject;

using Project.Features.BoardModule;
using Project.Features.LevelsModule;

namespace Project.Features.GameBootstrapModule
{
    public class RoundSceneBootstrap : MonoBehaviour
    {
        private BoardComposer _boardComposer;
        private LevelContext _levelContext;
        
        [Inject]
        public void InjectDependencies(BoardComposer boardComposer,
                                       LevelContext levelContext)
        {
            _boardComposer = boardComposer;
            _levelContext = levelContext;
        }

        private void Start()
        {
            _boardComposer.Compose(_levelContext.LevelConfig.CardCount);
            _levelContext.LevelStartTime = Time.time;
        }
    }
}

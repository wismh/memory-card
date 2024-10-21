using UnityEngine;
using Zenject;

using Project.Features.BoardModule;
using Project.Features.GameFlowStateMachineModule;
using Project.Features.LevelsModule;

namespace Project.Features.GameBootstrapModule
{
    public class RoundSceneBootstrap : MonoBehaviour
    {
        private BoardComposer _boardComposer;
        private LevelContext _levelContext;
        private GameFlowStateMachine _gameFlowStateMachine;
        
        [Inject]
        public void InjectDependencies(BoardComposer boardComposer,
                                       LevelContext levelContext,
                                       GameFlowStateMachine gameFlowStateMachine)
        {
            _gameFlowStateMachine = gameFlowStateMachine;
            _boardComposer = boardComposer;
            _levelContext = levelContext;
        }

        private void Start()
        {
            _boardComposer.Compose(_levelContext.LevelConfig.CardCount);
            _gameFlowStateMachine.Enter<RoundFlowState>();
            _levelContext.LevelStartTime = Time.time;
        }
    }
}

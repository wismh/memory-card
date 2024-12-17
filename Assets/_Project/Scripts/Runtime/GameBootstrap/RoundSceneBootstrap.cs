using UnityEngine;
using Zenject;

using Project.Board;
using Project.GameFlow;
using Project.Levels;
using Project.Progress;

namespace Project.GameBootstrap
{
    public class RoundSceneBootstrap : MonoBehaviour
    {
        private BoardComposer _boardComposer;
        private LevelContext _levelContext;
        private ILevelProgressService _levelProgress;
        private GameFlowStateMachine _gameFlowStateMachine;

        [Inject]
        public void InjectDependencies(BoardComposer boardComposer,
                                       LevelContext levelContext,
                                       ILevelProgressService levelProgress,
                                       GameFlowStateMachine gameFlowStateMachine)
        {
            _boardComposer = boardComposer;
            _levelContext = levelContext;
            _levelProgress = levelProgress;
            _gameFlowStateMachine = gameFlowStateMachine;
        }

        private void Start()
        {
            var config = _levelContext.LevelConfig;
            if (config == null || !_levelProgress.IsUnlocked(config))
            {
                _gameFlowStateMachine.Enter<GoMenuFlowState>();
                return;
            }

            _boardComposer.Compose(config.CardCount);
            _levelContext.LevelStartTime = Time.time;
        }
    }
}

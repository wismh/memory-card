using Cysharp.Threading.Tasks;
using Lumenwake.UIModule;
using Project.Board;
using Project.GameFlow;
using Project.Hud;
using Project.Levels;
using Project.Progress;

using System;
using Zenject;
using UnityEngine;

namespace Project.GamePlay
{
    public class GamePlayPresenter : IInitializable, IDisposable
    {
        private readonly BoardPresenter _boardPresenter;
        private readonly GameFlowStateMachine _gameFlowStateMachine;
        private readonly LevelContext _levelContext;
        private readonly WonPopupPresenter _wonPopupPresenter;
        private readonly IScreenManager _screenManager;
        private readonly ILevelProgressService _levelProgress;
        private readonly LevelsDb _levelsDb;
        private readonly IMapAnimationIntent _mapAnimationIntent;

        public GamePlayPresenter(GameFlowStateMachine gameFlowStateMachine,
                                 BoardPresenter boardPresenter,
                                 LevelContext levelContext,
                                 WonPopupPresenter wonPopupPresenter,
                                 IScreenManager screenManager,
                                 ILevelProgressService levelProgress,
                                 LevelsDb levelsDb,
                                 IMapAnimationIntent mapAnimationIntent)
        {
            _gameFlowStateMachine = gameFlowStateMachine;
            _wonPopupPresenter = wonPopupPresenter;
            _screenManager = screenManager;
            _boardPresenter = boardPresenter;
            _levelContext = levelContext;
            _levelProgress = levelProgress;
            _levelsDb = levelsDb;
            _mapAnimationIntent = mapAnimationIntent;
        }
        
        public void Initialize()
        {
            _boardPresenter.OnBoardComplete += HandleBoardComplete;
        }
        
        public void Dispose()
        {
            _boardPresenter.OnBoardComplete -= HandleBoardComplete;   
        }
        
        private void HandleBoardComplete()
        {
            if (_levelContext.LevelConfig != null)
            {
                _levelProgress.RecordLevelCompleted(_levelContext.LevelConfig);

                var completedIndex = _levelsDb.IndexOf(_levelContext.LevelConfig);
                if (completedIndex >= 0)
                    _mapAnimationIntent.QueueAnimationAfterStageComplete(
                        completedIndex,
                        _levelsDb.LevelConfigs.Count);
            }

            var result = new LevelResult()
            {
                Time = Time.time - _levelContext.LevelStartTime
            };
            
            _wonPopupPresenter.Show(result);
            _screenManager.OpenScreen<WonPopupPresenter>().Forget();
        }
    }
}

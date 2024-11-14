using Project.Features.BoardModule;
using Project.Features.HudModule;
using Project.Features.LevelsModule;
using Project.Features.ProgressModule;

using System;
using Zenject;
using UnityEngine;

namespace Project.Features.GameFlowStateMachineModule
{
    public class GamePlayPresenter : IInitializable, IDisposable 
    {
        private readonly BoardPresenter _boardPresenter;
        private readonly GameFlowStateMachine _gameFlowStateMachine;
        private readonly LevelContext _levelContext;
        private readonly WonPopupPresenter _wonPopupPresenter;
        private readonly ILevelProgressService _levelProgress;
        
        public GamePlayPresenter(GameFlowStateMachine gameFlowStateMachine,
                                 BoardPresenter boardPresenter,
                                 LevelContext levelContext,
                                 WonPopupPresenter wonPopupPresenter,
                                 ILevelProgressService levelProgress)
        {
            _gameFlowStateMachine = gameFlowStateMachine;
            _wonPopupPresenter = wonPopupPresenter;
            _boardPresenter = boardPresenter;
            _levelContext = levelContext;
            _levelProgress = levelProgress;
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
                _levelProgress.RecordLevelCompleted(_levelContext.LevelConfig);

            var result = new LevelResult()
            {
                Time = Time.time - _levelContext.LevelStartTime
            };
            
            _wonPopupPresenter.Show(result);
        }
    }
}

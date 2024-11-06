using Project.Features.BoardModule;
using Project.Features.HudModule;
using Project.Features.LevelsModule;

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
        
        public GamePlayPresenter(GameFlowStateMachine gameFlowStateMachine,
                                 BoardPresenter boardPresenter,
                                 LevelContext levelContext,
                                 WonPopupPresenter wonPopupPresenter)
        {
            _gameFlowStateMachine = gameFlowStateMachine;
            _wonPopupPresenter = wonPopupPresenter;
            _boardPresenter = boardPresenter;
            _levelContext = levelContext;
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
            var result = new LevelResult()
            {
                Time = Time.time - _levelContext.LevelStartTime
            };
            
            _wonPopupPresenter.Show(result);
        }
    }
}

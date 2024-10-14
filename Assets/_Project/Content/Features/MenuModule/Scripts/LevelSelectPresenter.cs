using System.Collections.Generic;
using UnityEngine;
using Zenject;

using Project.Features.GameFlowStateMachineModule;
using Project.Features.LevelsModule;

namespace Project.Features.MenuModule
{
    public class LevelSelectPresenter : MonoBehaviour
    {
        [SerializeField] private List<LevelSelectView> _levelSelectViews;

        private GameFlowStateMachine _gameFlowStateMachine;
        private LevelContext _levelContext;
        private LevelsDb _levelsDb;

        [Inject]
        public void InjectDependencies(GameFlowStateMachine gameFlowStateMachine,
                                       LevelContext levelContext,
                                       LevelsDb levelsDb
            )
        {
            _gameFlowStateMachine = gameFlowStateMachine;
            _levelContext = levelContext;
            _levelsDb = levelsDb;
        }
        
        private void Start()
        {
            foreach (var view in _levelSelectViews)
                view.OnLevelSelect += HandleLevelSelect;
        }

        private void OnDestroy()
        {
            foreach (var view in _levelSelectViews)
                view.OnLevelSelect -= HandleLevelSelect;
        }

        private void HandleLevelSelect(int levelIndex)
        {
            _levelContext.LevelConfig = _levelsDb.LevelConfigs[levelIndex];
            _gameFlowStateMachine.Enter<StartRoundFlowState>();   
        }
    }
}

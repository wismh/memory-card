using System.Collections.Generic;
using UnityEngine;
using Zenject;

using Project.Features.GameFlowStateMachineModule;
using Project.Features.LevelsModule;
using Project.Features.ProgressModule;

namespace Project.Features.MenuModule
{
    public class LevelSelectPresenter : MonoBehaviour
    {
        [SerializeField] private List<LevelSelectView> _levelSelectViews;

        private GameFlowStateMachine _gameFlowStateMachine;
        private LevelContext _levelContext;
        private LevelsDb _levelsDb;
        private ILevelProgressService _levelProgress;

        [Inject]
        public void InjectDependencies(GameFlowStateMachine gameFlowStateMachine,
                                       LevelContext levelContext,
                                       LevelsDb levelsDb,
                                       ILevelProgressService levelProgress)
        {
            _gameFlowStateMachine = gameFlowStateMachine;
            _levelContext = levelContext;
            _levelsDb = levelsDb;
            _levelProgress = levelProgress;
        }

        private void OnEnable()
        {
            RefreshLevelButtons();
        }

        private void Start()
        {
            if (_levelProgress != null)
                _levelProgress.ProgressChanged += RefreshLevelButtons;

            foreach (var view in _levelSelectViews)
                view.OnLevelSelect += HandleLevelSelect;

            RefreshLevelButtons();
        }

        private void OnDestroy()
        {
            if (_levelProgress != null)
                _levelProgress.ProgressChanged -= RefreshLevelButtons;

            foreach (var view in _levelSelectViews)
                view.OnLevelSelect -= HandleLevelSelect;
        }

        private void RefreshLevelButtons()
        {
            if (_levelsDb == null || _levelProgress == null)
                return;

            var configs = _levelsDb.LevelConfigs;
            foreach (var view in _levelSelectViews)
            {
                var idx = view.LinkedLevelIndex;
                if (idx < 0 || idx >= configs.Count)
                    continue;

                var config = configs[idx];
                view.SetLocked(!_levelProgress.IsUnlocked(config));
            }
        }

        private void HandleLevelSelect(int levelIndex)
        {
            var configs = _levelsDb.LevelConfigs;
            if (levelIndex < 0 || levelIndex >= configs.Count)
                return;

            var config = configs[levelIndex];
            if (!_levelProgress.IsUnlocked(config))
                return;

            _levelContext.LevelConfig = config;
            _gameFlowStateMachine.Enter<StartRoundFlowState>();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using Zenject;

using Project.Features.GameFlowStateMachineModule;

namespace Project.Features.MenuModule
{
    public class LevelSelectPresenter : MonoBehaviour
    {
        [SerializeField] private List<LevelSelectView> _levelSelectViews;

        private GameFlowStateMachine _gameFlowStateMachine;

        [Inject]
        public void InjectDependencies(GameFlowStateMachine gameFlowStateMachine)
        {
            _gameFlowStateMachine = gameFlowStateMachine;
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
            _gameFlowStateMachine.Enter<StartRoundFlowState>();   
        }
    }
}

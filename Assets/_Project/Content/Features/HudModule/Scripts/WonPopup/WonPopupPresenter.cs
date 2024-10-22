using Project.Features.GameFlowStateMachineModule;
using Project.Features.LevelsModule;
using UnityEngine;
using Zenject;

namespace Project.Features.HudModule
{
    public class WonPopupPresenter : MonoBehaviour
    {
        [SerializeField] private WonPopupView _view;
        
        private GameFlowStateMachine _gameFlowStateMachine;
        
        [Inject]
        public void InjectDependencies(GameFlowStateMachine gameFlowStateMachine)
        {
            _gameFlowStateMachine = gameFlowStateMachine;
        }
        
        public void Show(LevelResult levelResult)
        {
            _view.Show(levelResult.Time);
        }

        private void Start()
        {
            _view.OnRetry += HandleRetry;
            _view.OnGoBack += HandleGoBack;
            
            _view.Hide();
        }

        private void OnDestroy()
        {
            _view.OnRetry -= HandleRetry;
            _view.OnGoBack -= HandleGoBack;
        }
        
        private void HandleGoBack()
        {
            _gameFlowStateMachine.Enter<GoMenuFlowState>();
        }
        
        private void HandleRetry()
        {
            _gameFlowStateMachine.Enter<ReloadRoundFlowState>();
        }
    }
}

using Project.GameFlow;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Hud
{
    public class BackPresenter : MonoBehaviour
    {
        [SerializeField] private Button _backButton;

        private GameFlowStateMachine _gameFlowStateMachine;

        [Inject]
        public void InjectDependencies(GameFlowStateMachine gameFlowStateMachine)
        { 
            _gameFlowStateMachine = gameFlowStateMachine;
        }
        
        private void Start()
        {
            _backButton.onClick.AddListener(HandleBack);
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveListener(HandleBack);
        }
        
        private void HandleBack()
        {
            _gameFlowStateMachine.Enter<GoMenuFlowState>();
        }
    }
}

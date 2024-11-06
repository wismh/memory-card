using Project.Features.GameFlowStateMachineModule;
using UnityEngine;
using Zenject;

namespace Project.Features.GameBootstrapModule
{
    public class GlobalSceneBootstrap : MonoBehaviour
    {
        private GameFlowStateMachine _gameFlowStateMachine;

        [Inject]
        public void InjectDependencies(GameFlowStateMachine gameFlowStateMachine) =>
            _gameFlowStateMachine = gameFlowStateMachine;

        private void Start() =>
            _gameFlowStateMachine.Enter<GoMenuFlowState>();
    }
}
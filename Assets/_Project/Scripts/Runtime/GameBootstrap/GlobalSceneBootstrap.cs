using Project.GameFlow;
using UnityEngine;
using Zenject;

namespace Project.GameBootstrap
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
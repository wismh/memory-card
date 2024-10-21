using UnityEngine;

namespace Project.Features.GameFlowStateMachineModule
{
    public class RoundFlowState : GameFlowStateBase
    {
        public override void Enter()
        {
            Debug.LogError("Enter");
        }
        public override void Exit()
        {
            
        }
    }
}

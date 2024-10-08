using System.Collections.Generic;
using Project.Core.StateMachineModule;

namespace Project.Features.GameFlowStateMachineModule
{
    public class GameFlowStateMachine : StateMachineBehaviour<GameFlowStateBase> 
    {
        public GameFlowStateMachine(GlobalGameFlowState bootstrapGameFlowState,
                                    StartRoundFlowState startRoundFlowState) =>
            SetStates(new List<GameFlowStateBase>() {
                bootstrapGameFlowState, 
                startRoundFlowState
            });
    }
}
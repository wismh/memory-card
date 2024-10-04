using System.Collections.Generic;
using Project.Core.StateMachineModule;

namespace Project.Features.GameFlowStateMachineModule {
    public class GameFlowStateMachine : StateMachineBehaviour<GameFlowStateBase> 
    {
        public GameFlowStateMachine(GlobalGameFlowState bootstrapGameFlowState) =>
            SetStates(new List<GameFlowStateBase>() {
                bootstrapGameFlowState
            });
    }
}
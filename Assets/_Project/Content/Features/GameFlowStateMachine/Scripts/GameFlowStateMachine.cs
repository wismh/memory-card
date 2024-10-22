using System.Collections.Generic;
using Project.Core.StateMachineModule;

namespace Project.Features.GameFlowStateMachineModule
{
    public class GameFlowStateMachine : StateMachineBehaviour<StateBase> 
    {
        public GameFlowStateMachine(GlobalGameFlowState bootstrapGameFlowState,
                                    StartRoundFlowState startRoundFlowState,
                                    GoMenuFlowState goMenuFlowState, 
                                    ReloadRoundFlowState reloadRoundFlowState) =>
            SetStates(new List<StateBase>() {
                bootstrapGameFlowState, 
                startRoundFlowState,
                goMenuFlowState,
                reloadRoundFlowState
            });
    }
}
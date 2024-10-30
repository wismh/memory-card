namespace Project.Core.StateMachineModule {
    public abstract class PayLoadedStateBase<TPayload> : StateBase 
    {
        public virtual void Enter(TPayload payLoad)
        {
            Enter();
        }
    }
}
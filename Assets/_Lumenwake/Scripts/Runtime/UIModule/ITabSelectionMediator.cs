namespace Lumenwake.UIModule
{
    /// <summary>
    /// Entry point for tab selection requests (mediator). Keeps tab UI decoupled from screens.
    /// </summary>
    public interface ITabSelectionMediator
    {
        void RequestSelection(int tabIndex);
    }
}

namespace Lumenwake.UIModule
{
    /// <summary>
    /// <see cref="BaseScreenManager"/> keeps one independent screen stack per layer — opening a screen
    /// only hides the previous top of its OWN layer, never screens in other layers. This is what lets
    /// e.g. a persistent HUD stay visible while a modal panel opens on top of it. Add a layer here if a
    /// third independent stack is genuinely needed (e.g. toast/notification), rather than repurposing
    /// an existing one for something it doesn't mean.
    /// </summary>
    public enum ScreenLayer
    {
        /// <summary>Always-open screens (HUD). Default for any screen that doesn't override <see cref="BaseScreen.Layer"/>
        /// — matches the manager's old single-stack behavior when only this layer is used.</summary>
        Default,

        /// <summary>Modal/overlay panels (upgrade panel, settings, confirmation prompts) — stack among
        /// themselves independently of whatever is open in <see cref="Default"/>.</summary>
        Overlay,
    }
}

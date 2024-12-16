using Zenject;

namespace Lumenwake.UIModule
{
    /// <summary>
    /// Registers <see cref="InputRebindManager"/> as a scene/project singleton. Call <c>Install(Container)</c> from a parent installer.
    /// </summary>
    public sealed class InputRebindInstaller : Installer<InputRebindInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputRebindManager>().AsSingle();
        }
    }
}

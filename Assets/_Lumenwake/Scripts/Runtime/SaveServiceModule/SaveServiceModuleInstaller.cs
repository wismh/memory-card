using Project.Core.SaveServiceModule.Serialization;
using Zenject;

namespace Project.Core.SaveServiceModule
{
    /// <summary>Binds serializer infrastructure shared by all projects.</summary>
    public class SaveServiceModuleInstaller : Installer<SaveServiceModuleInstaller>
    {
        private readonly bool _useCamelCase;
        private readonly bool _prettyPrintInEditor;

        public SaveServiceModuleInstaller(bool useCamelCase = false, bool prettyPrintInEditor = true)
        {
            _useCamelCase = useCamelCase;
            _prettyPrintInEditor = prettyPrintInEditor;
        }

        public override void InstallBindings()
        {
            Container.Bind<ISerializer>()
                .FromInstance(new JsonSerializer(_useCamelCase, _prettyPrintInEditor))
                .AsSingle();
        }
    }
}

using Project.Core.AssetLoaderModule;
using Project.Core.SceneLoaderServiceModule;
using Project.GameFlow;
using Project.Progress;
using UnityEngine;
using Zenject;

namespace Project.GameBootstrap 
{
    [CreateAssetMenu(menuName = "Configurations/GameBootstrap/" + nameof(ProjectContextInstaller),
        fileName = nameof(ProjectContextInstaller) + "_Default", order = 0)]
    public class ProjectContextInstaller : ScriptableObjectInstaller<ProjectContextInstaller>
    {
        public override void InstallBindings()
        {
            ProgressInstaller.Install(Container);
            SceneLoaderServiceModuleInstaller.Install(Container);
            GameFlowStateMachineInstaller.Install(Container);
            AssetLoaderInstaller.Install(Container);
        }
    }
}

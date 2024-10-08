using Project.Core.SceneLoaderServiceModule;
using Project.Features.GameFlowStateMachineModule;
using UnityEngine;
using Zenject;

namespace Project.Features.GameBootstrapModule 
{
    [CreateAssetMenu(menuName = "Configurations/GameBootstrap/" + nameof(ProjectContextInstaller),
        fileName = nameof(ProjectContextInstaller) + "_Default", order = 0)]
    public class ProjectContextInstaller : ScriptableObjectInstaller<ProjectContextInstaller>
    {
        public override void InstallBindings() 
        {
            SceneLoaderServiceModuleInstaller.Install(Container);
            GameFlowStateMachineInstaller.Install(Container);
        }
    }
}

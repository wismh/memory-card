using Project.Core.SceneLoaderServiceModule;
using Project.Features.GameFlowStateMachineModule;
using Project.Features.LevelsModule;
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
            Container.Bind<LevelContext>().AsSingle();
            
            SceneLoaderServiceModuleInstaller.Install(Container);
            GameFlowStateMachineInstaller.Install(Container);
        }
    }
}

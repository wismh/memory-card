using UnityEngine;
using Zenject;

namespace Project.Features.LevelsModule
{
    [CreateAssetMenu(menuName = "Configurations/Levels/" + nameof(LevelModuleInstaller),
        fileName = nameof(LevelModuleInstaller) + "_Default")]
    public class LevelModuleInstaller : ScriptableObjectInstaller<LevelModuleInstaller>
    {
        [SerializeField] private LevelsDb _levels;
        
        public override void InstallBindings()
        {
            Container.Bind<LevelsDb>()
                .FromInstance(_levels);
        }
    }
}

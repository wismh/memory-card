using UnityEngine;
using Zenject;

namespace Project.Levels
{
    [CreateAssetMenu(menuName = "Configurations/Levels/" + nameof(LevelsInstaller),
        fileName = nameof(LevelsInstaller) + "_Default")]
    public class LevelsInstaller : ScriptableObjectInstaller<LevelsInstaller>
    {
        [SerializeField] private LevelsDb _levelsDb;

        public override void InstallBindings()
        {
            Container.Bind<LevelContext>().AsSingle();

            Container.Bind<LevelsDb>()
                .FromInstance(_levelsDb)
                .AsSingle();
        }
    }
}

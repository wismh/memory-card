using UnityEngine;

namespace Project.Features.LevelsModule
{
    [CreateAssetMenu(menuName = "Configurations/Levels/" + nameof(LevelConfig),
        fileName = nameof(LevelConfig) + "_Default")]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public int CardCount { get; private set; }
    }
}
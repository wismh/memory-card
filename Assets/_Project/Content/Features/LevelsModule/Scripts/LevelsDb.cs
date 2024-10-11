using System.Collections.Generic;
using UnityEngine;

namespace Project.Features.LevelsModule
{
    [CreateAssetMenu(menuName = "Configurations/Levels/" + nameof(LevelsDb),
        fileName = nameof(LevelsDb) + "_Default")]
    public class LevelsDb : ScriptableObject
    {
        [SerializeField] private List<LevelConfig> _levelConfigs;
        
        public IReadOnlyList<LevelConfig> LevelConfigs =>
            _levelConfigs.AsReadOnly();
    }
}

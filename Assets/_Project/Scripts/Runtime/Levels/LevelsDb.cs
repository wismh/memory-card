using System.Collections.Generic;
using UnityEngine;

namespace Project.Levels
{
    [CreateAssetMenu(menuName = "Configurations/Levels/" + nameof(LevelsDb),
        fileName = nameof(LevelsDb) + "_Default")]
    public class LevelsDb : ScriptableObject
    {
        [SerializeField] private List<LevelConfig> _levelConfigs;

        public IReadOnlyList<LevelConfig> LevelConfigs =>
            _levelConfigs.AsReadOnly();

        public int IndexOf(LevelConfig level)
        {
            if (level == null || _levelConfigs == null)
                return -1;

            return _levelConfigs.IndexOf(level);
        }
    }
}

using UnityEngine;

namespace Project.Levels
{
    [CreateAssetMenu(menuName = "Configurations/Levels/" + nameof(LevelConfig),
        fileName = nameof(LevelConfig) + "_Default")]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public int CardCount { get; private set; }

        [Tooltip("Unique id for saves and progression. If empty, the asset name is used.")]
        [SerializeField] private string _levelId;

        public string StableId => string.IsNullOrEmpty(_levelId) ? name : _levelId;
    }
}
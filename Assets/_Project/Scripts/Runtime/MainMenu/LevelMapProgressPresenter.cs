using Project.Progress;
using Project.Levels;
using Project.UIMapPath;
using UnityEngine;
using Zenject;

namespace Project.MainMenu
{
    public class LevelMapProgressPresenter : MonoBehaviour
    {
        [SerializeField] private LevelMapPlayer _player;

        private IMapAnimationIntent _mapAnimationIntent;
        private LevelsDb _levelsDb;
        private ILevelProgressService _levelProgress;

        [Inject]
        public void InjectDependencies(IMapAnimationIntent mapAnimationIntent,
                                       LevelsDb levelsDb,
                                       ILevelProgressService levelProgress)
        {
            _mapAnimationIntent = mapAnimationIntent;
            _levelsDb = levelsDb;
            _levelProgress = levelProgress;
        }

        private void Start()
        {
            ApplyMapVisual();
        }

        private void ApplyMapVisual()
        {
            if (_player == null || _levelsDb == null || _levelProgress == null)
                return;

            if (_mapAnimationIntent.TryConsumeStageCompleteAnimation(out var from, out var to))
            {
                if (from != to)
                {
                    _player.SetPositionInstant(from);
                    _player.MoveToLevel(to);
                }
                else
                    _player.SetPositionInstant(to);

                return;
            }

            _player.SetPositionInstant(GetFarthestUnlockedLevelIndex());
        }

        private int GetFarthestUnlockedLevelIndex()
        {
            var configs = _levelsDb.LevelConfigs;
            var best = 0;
            for (var i = 0; i < configs.Count; i++)
            {
                if (_levelProgress.IsUnlocked(configs[i]))
                    best = i;
            }

            return best;
        }
    }
}

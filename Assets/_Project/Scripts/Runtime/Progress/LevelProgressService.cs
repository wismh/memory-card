using System;
using System.Collections.Generic;
using Project.Core.SaveServiceModule;
using Project.Levels;
using Zenject;

namespace Project.Progress
{
    public class LevelProgressService : ILevelProgressService, IInitializable
    {
        private readonly LevelsDb _levelsDb;
        private readonly ISaveService<GameSaveData> _saveService;

        private GameSaveData _data;
        private readonly HashSet<string> _completedIds = new();

        public event Action ProgressChanged;

        public LevelProgressService(LevelsDb levelsDb, ISaveService<GameSaveData> saveService)
        {
            _levelsDb = levelsDb;
            _saveService = saveService;
        }

        void IInitializable.Initialize()
        {
            LoadOrCreate();
        }

        public bool IsUnlocked(LevelConfig level)
        {
            if (level == null)
                return false;

            var index = _levelsDb.IndexOf(level);
            if (index < 0)
                return false;

            if (index == 0)
                return true;

            var previous = _levelsDb.LevelConfigs[index - 1];
            return IsCompleted(previous);
        }

        public bool IsCompleted(LevelConfig level)
        {
            if (level == null)
                return false;

            var id = level.StableId;
            return _completedIds.Contains(id);
        }

        public void RecordLevelCompleted(LevelConfig level)
        {
            if (level == null)
                return;

            var id = level.StableId;
            if (!_completedIds.Add(id))
                return;

            Persist();
            ProgressChanged?.Invoke();
        }

        public void ResetProgress()
        {
            _data.completedLevelIds = Array.Empty<string>();
            RebuildCompletedCache();
            _saveService.SaveImmediate();
            ProgressChanged?.Invoke();
        }

        private void LoadOrCreate()
        {
            _data = _saveService.Load();
            _data.completedLevelIds ??= Array.Empty<string>();
            RebuildCompletedCache();
        }

        private void RebuildCompletedCache()
        {
            _completedIds.Clear();
            if (_data.completedLevelIds == null)
                return;

            foreach (var id in _data.completedLevelIds)
            {
                if (!string.IsNullOrEmpty(id))
                    _completedIds.Add(id);
            }
        }

        private void Persist()
        {
            var arr = new string[_completedIds.Count];
            _completedIds.CopyTo(arr, 0);
            _data.completedLevelIds = arr;
            _saveService.SaveImmediate();
        }
    }
}

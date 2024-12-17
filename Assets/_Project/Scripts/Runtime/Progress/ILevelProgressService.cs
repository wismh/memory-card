using System;
using Project.Levels;

namespace Project.Progress
{
    public interface ILevelProgressService
    {
        event Action ProgressChanged;

        bool IsUnlocked(LevelConfig level);
        bool IsCompleted(LevelConfig level);

        void RecordLevelCompleted(LevelConfig level);
        void ResetProgress();
    }
}

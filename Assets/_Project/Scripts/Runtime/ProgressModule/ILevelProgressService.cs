using System;
using Project.Features.LevelsModule;

namespace Project.Features.ProgressModule
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

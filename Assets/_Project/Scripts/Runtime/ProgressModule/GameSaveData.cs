using System;

namespace Project.Features.ProgressModule
{
    [Serializable]
    public class GameSaveData
    {
        public int version = GameSaveVersion.Current;
        public string[] completedLevelIds = Array.Empty<string>();
    }
}

using System;

namespace Project.Progress
{
    [Serializable]
    public class GameSaveData
    {
        public int version = GameSaveVersion.Current;
        public string[] completedLevelIds = Array.Empty<string>();
    }
}

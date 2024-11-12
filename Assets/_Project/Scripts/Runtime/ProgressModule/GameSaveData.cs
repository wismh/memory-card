using System;
using System.Collections.Generic;

namespace Project.Features.ProgressModule
{
    [Serializable]
    public class GameSaveData
    {
        public int version = GameSaveVersion.Current;

        public List<string> completedLevelIds = new();

        public string reservedExtensionJson;
    }
}

using System.IO;
using UnityEngine;

namespace Project.Features.ProgressModule
{
    public class JsonFileGameSavePersistence : IGameSavePersistence
    {
        private const string FileName = "game_progress.json";

        private static string FilePath =>
            Path.Combine(Application.persistentDataPath, FileName);

        public bool TryLoad(out GameSaveData data)
        {
            data = null;
            var path = FilePath;
            if (!File.Exists(path))
                return false;

            try
            {
                var json = File.ReadAllText(path);
                data = JsonUtility.FromJson<GameSaveData>(json);
                if (data == null)
                    return false;

                data.completedLevelIds ??= new System.Collections.Generic.List<string>();
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public void Save(GameSaveData data)
        {
            if (data.completedLevelIds == null)
                data.completedLevelIds = new System.Collections.Generic.List<string>();

            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(FilePath, json);
        }

        public void DeleteSave()
        {
            var path = FilePath;
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}

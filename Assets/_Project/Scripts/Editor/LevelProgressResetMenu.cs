using System.IO;
using UnityEditor;
using UnityEngine;

namespace Project.Features.ProgressModule.Editor
{
    public static class LevelProgressResetMenu
    {
        private const string FileName = "game_progress.json";

        [MenuItem("Tools/Progress/Reset Level Progress")]
        public static void ResetLevelProgress()
        {
            var path = Path.Combine(Application.persistentDataPath, FileName);
            if (File.Exists(path))
                File.Delete(path);

            Debug.Log($"Level progress cleared. Save file removed if present: {path}");
        }
    }
}

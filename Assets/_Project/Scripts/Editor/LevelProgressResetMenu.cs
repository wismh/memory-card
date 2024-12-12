using System.IO;
using Lumenwake;
using UnityEditor;
using UnityEngine;

namespace Project.Features.ProgressModule.Editor
{
    public static class LevelProgressResetMenu
    {
        [MenuItem("Tools/Progress/Reset Level Progress")]
        public static void ResetLevelProgress()
        {
            var path = Path.Combine(Application.persistentDataPath, ProgressModuleInstaller.SaveFileName);
            if (File.Exists(path))
                File.Delete(path);

            LoggingSystem.Log($"Level progress cleared. Save file removed if present: {path}");
        }
    }
}

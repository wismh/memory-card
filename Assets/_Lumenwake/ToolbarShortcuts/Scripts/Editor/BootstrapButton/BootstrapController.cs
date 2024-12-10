using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ToolbarShortcuts.Editor.Bootstrap
{
    public class BootstrapController
    {
        private static BootstrapConfig _config;

        private const string k_previousSceneKey = "com.lumenwake.toolbar-shortcuts.Bootstrap.PreviousScene";
        private const string k_playTargetKey = "com.lumenwake.toolbar-shortcuts.Bootstrap.PlayTarget";

        private static BootstrapConfig BootstrapConfig
        {
            get
            {
                if (_config == null)
                    _config = LoadBootstrapConfig();

                return _config;
            }
        }

        public static BootstrapPlayTarget SelectedPlayTarget
        {
            get => (BootstrapPlayTarget)EditorPrefs.GetInt(k_playTargetKey, (int)BootstrapPlayTarget.Bootstrap);
            set
            {
                EditorPrefs.SetInt(k_playTargetKey, (int)value);
                BootstrapToolbar.Refresh();
            }
        }

        public static event Action OnPlay;
        public static event Action OnStop;

        public static void TogglePlay()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;

            if (EditorApplication.isPlaying)
            {
                OnStop?.Invoke();
                EditorApplication.isPlaying = false;
                return;
            }

            string activePath = SceneManager.GetActiveScene().path;
            SessionState.SetString(k_previousSceneKey, activePath);

            if (SelectedPlayTarget == BootstrapPlayTarget.Bootstrap)
            {
                string bootstrapPath = GetBootstrapScenePath();
                if (string.IsNullOrEmpty(bootstrapPath))
                {
                    Debug.LogError($"{nameof(BootstrapController)}: Bootstrap scene is not set in Build Settings (index 0).");
                    EditorApplication.playModeStateChanged -= OnPlayModeChanged;
                    return;
                }

                if (activePath != bootstrapPath)
                {
                    if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
                        return;
                    }

                    EditorSceneManager.OpenScene(bootstrapPath);
                }
            }
            else if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorApplication.playModeStateChanged -= OnPlayModeChanged;
                return;
            }

            OnPlay?.Invoke();
            EditorApplication.isPlaying = true;
        }

        public static void SetPaused(bool paused)
        {
            EditorApplication.isPaused = paused;
            BootstrapToolbar.Refresh();
        }

        public static string GetBootstrapScenePath()
        {
            if (BootstrapConfig?.bootstrapScene != null)
                return AssetDatabase.GetAssetPath(BootstrapConfig.bootstrapScene);
            
            var buildScenes = EditorBuildSettings.scenes;
            if (buildScenes.Length > 0 && !string.IsNullOrEmpty(buildScenes[0].path))
                return buildScenes[0].path;

            return null;
        }

        public static string GetLaunchScenePath() =>
            SelectedPlayTarget == BootstrapPlayTarget.Bootstrap
                ? GetBootstrapScenePath()
                : SceneManager.GetActiveScene().path;

        public static string GetPlayTargetLabel()
        {
            string path = GetLaunchScenePath();
            if (string.IsNullOrEmpty(path))
                return SelectedPlayTarget == BootstrapPlayTarget.Bootstrap ? "Bootstrap" : "Current";

            return Path.GetFileNameWithoutExtension(path);
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                string previousScene = SessionState.GetString(k_previousSceneKey, null);

                if (!string.IsNullOrEmpty(previousScene))
                {
                    EditorSceneManager.OpenScene(previousScene);
                    SessionState.EraseString(k_previousSceneKey);
                }

                EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            }

            BootstrapToolbar.Refresh();
        }

        private static BootstrapConfig LoadBootstrapConfig()
        {
            var config = Resources.Load<BootstrapConfig>("BootstrapConfig");
            if (config != null)
                return config;

            string[] guids = AssetDatabase.FindAssets($"t:{nameof(BootstrapConfig)}");
            if (guids.Length == 0)
                return null;

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<BootstrapConfig>(path);
        }
    }
}

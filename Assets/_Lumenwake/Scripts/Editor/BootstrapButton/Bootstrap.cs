using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lumenwake.Editor.BootstrapButton
{
    public class Bootstrap
    {
        private static BootstrapConfig _config;
        private const string k_previousSceneKey = "Bootstrap_PreviousScene";

        private static BootstrapConfig Config
        {
            get
            {
                if (_config == null)
                    _config = LoadConfig();
                
                return _config;
            }
        }

        public static void PlayFromBootstrap()
        {
            string bootstrapPath = AssetDatabase.GetAssetPath(Config.bootstrapScene);
            
            EditorApplication.playModeStateChanged += OnPlayModeChanged;

            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                return;
            }

            string previousScene = SceneManager.GetActiveScene().path;
            SessionState.SetString(k_previousSceneKey, previousScene);

            if (previousScene != bootstrapPath)
            {
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    return;

                EditorSceneManager.OpenScene(bootstrapPath);
            }

            EditorApplication.isPlaying = true;
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
        }
        
        private static BootstrapConfig LoadConfig()
        {
            var config = Resources.Load<BootstrapConfig>("BootstrapConfig");
            if (!config)
            {
                Debug.LogError($"{nameof(Bootstrap)}: Failed to find {nameof(BootstrapConfig)}");
                return null;
            }

            return config;
        }
    }
}
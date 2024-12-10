using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;

namespace ToolbarShortcuts.Editor.Bootstrap
{
    [InitializeOnLoad]
    public static class BootstrapToolbar
    {
        public const string ElementPath = "ToolbarShortcuts/Play";

        [MainToolbarElement(ElementPath, defaultDockPosition = MainToolbarDockPosition.Middle)]
        public static IEnumerable<MainToolbarElement> CreatePlayControls()
        {
            yield return CreatePlayButton();
            yield return CreatePauseToggle();
            yield return CreateSceneDropdown();
        }

        public static void Refresh() => MainToolbar.Refresh(ElementPath);

        static BootstrapToolbar()
        {
            EditorApplication.playModeStateChanged += _ => Refresh();
            EditorApplication.pauseStateChanged += _ => Refresh();
            EditorSceneManager.activeSceneChangedInEditMode += (_, _) => Refresh();
        }

        static MainToolbarButton CreatePlayButton()
        {
            bool isPlaying = EditorApplication.isPlaying;
            var icon = EditorGUIUtility.IconContent(isPlaying ? "d_PlayButton On@2x" : "d_PlayButton@2x").image as Texture2D;
            var text = isPlaying ? "Stop" : "Play";
            var tooltip = isPlaying ? "Stop Play Mode" : "Enter Play Mode";
            var content = new MainToolbarContent(text, icon, tooltip);

            return new MainToolbarButton(content, BootstrapController.TogglePlay);
        }

        static MainToolbarToggle CreatePauseToggle()
        {
            bool isPlaying = EditorApplication.isPlaying;
            bool isPaused = EditorApplication.isPaused;
            var icon = EditorGUIUtility.IconContent(isPaused ? "d_PauseButton On@2x" : "d_PauseButton@2x").image as Texture2D;
            var content = new MainToolbarContent("Pause", icon, "Pause Play Mode");

            return new MainToolbarToggle(content, isPaused, BootstrapController.SetPaused)
            {
                enabled = isPlaying
            };
        }

        static MainToolbarDropdown CreateSceneDropdown()
        {
            var icon = EditorGUIUtility.IconContent("SceneAsset Icon").image as Texture2D;
            var label = BootstrapController.GetPlayTargetLabel();
            var content = new MainToolbarContent(label, icon, "Scene to enter Play Mode with");

            return new MainToolbarDropdown(content, ShowSceneDropdown)
            {
                enabled = !EditorApplication.isPlaying
            };
        }

        static void ShowSceneDropdown(Rect dropDownRect)
        {
            var menu = new GenericMenu();
            var selected = BootstrapController.SelectedPlayTarget;

            string bootstrapPath = BootstrapController.GetBootstrapScenePath();
            string bootstrapLabel = FormatTargetLabel("Bootstrap", bootstrapPath);

            string currentPath = EditorSceneManager.GetActiveScene().path;
            string currentLabel = FormatTargetLabel("Current", currentPath);

            menu.AddItem(
                new GUIContent(bootstrapLabel),
                selected == BootstrapPlayTarget.Bootstrap,
                () => BootstrapController.SelectedPlayTarget = BootstrapPlayTarget.Bootstrap);

            menu.AddItem(
                new GUIContent(currentLabel),
                selected == BootstrapPlayTarget.CurrentScene,
                () => BootstrapController.SelectedPlayTarget = BootstrapPlayTarget.CurrentScene);

            menu.DropDown(dropDownRect);
        }

        static string FormatTargetLabel(string prefix, string scenePath)
        {
            if (string.IsNullOrEmpty(scenePath))
                return prefix;

            return $"{prefix} ({Path.GetFileNameWithoutExtension(scenePath)})";
        }
    }
}

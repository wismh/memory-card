using ToolbarShortcuts.Editor;
using UnityEditor;
using UnityEngine;

namespace ToolbarShortcuts.Editor.Setup
{
    public class ToolbarShortcutsWelcomeWindow : EditorWindow
    {
        const string k_menuPath = ToolbarShortcutsMenuPaths.Welcome;
        const string k_discordUrl = "https://discord.gg/A3XWVXKGhX";

        Vector2 _scrollPosition;

        [MenuItem(k_menuPath, priority = 0)]
        public static void ShowFromMenu() => ShowWindow();

        internal static void TryShowOnImport()
        {
            if (ToolbarShortcutsSettings.IsSetupCompleted)
                return;

            EditorApplication.delayCall += ShowWindow;
        }

        static void ShowWindow()
        {
            var window = GetWindow<ToolbarShortcutsWelcomeWindow>(true, "Toolbar Shortcuts", true);
            window.minSize = new Vector2(460, 500);
            window.maxSize = new Vector2(640, 760);
            window.Show();
        }

        void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            GUILayout.Space(8);
            EditorGUILayout.LabelField("Toolbar Shortcuts", EditorStyles.boldLabel);
            GUILayout.Space(4);

            EditorGUILayout.LabelField(
                "An editor package by Lumenwake for Unity 6 with a custom Play workflow (bootstrap scene) and " +
                "configurable quick actions on the main toolbar.",
                EditorStyles.wordWrappedLabel);

            GUILayout.Space(12);
            DrawSection(
                "Bootstrap Play",
                "Replaces the built-in Play / Pause / Step controls with ToolbarShortcuts/Play on the toolbar. " +
                "Enter Play Mode from the bootstrap scene (first scene in Build Settings) or from the active scene. " +
                "When you exit Play Mode, the Editor restores the scene you started from.");

            DrawSection(
                "Toolbar Shortcuts",
                "Quick actions on the left and right sides of the main toolbar. Configure entries in " +
                "ToolbarShortcutsConfig (included under Assets/Lumenwake/Resources, or create a copy via Assets → Create → ToolbarShortcuts → Toolbar Shortcuts Config). " +
                "Supported actions include opening scenes, assets, Editor windows, and static methods.");

            DrawSection(
                "Shortcuts Window",
                "A compact panel for shortcuts whose Placement is Window or Both. Add or edit the same entries in " +
                "ToolbarShortcutsConfig — set Placement to Window or Both to show a button here. " +
                "After Setup, this window opens docked above the Inspector when possible.");

            DrawSection(
                "Getting started",
                "1. Put your bootstrap scene first in File → Build Settings.\n" +
                "2. Click Setup below — hides the default Play controls and enables Toolbar Shortcuts toolbar items.\n" +
                "3. Edit ToolbarShortcutsConfig as needed. Use Tools → Toolbar Shortcuts → Refresh Toolbar to update the toolbar.");

            GUILayout.Space(12);
            DrawDiscordLink();

            GUILayout.Space(16);

            using (new EditorGUI.DisabledScope(ToolbarShortcutsSettings.IsSetupCompleted))
            {
                if (GUILayout.Button("Setup", GUILayout.Height(32)))
                    RunSetup();
            }

            if (ToolbarShortcutsSettings.IsSetupCompleted)
            {
                EditorGUILayout.HelpBox(
                    "Toolbar is already configured. To run Setup again, use Tools → Toolbar Shortcuts → Setup Toolbar.",
                    MessageType.Info);
            }

            GUILayout.Space(8);
            EditorGUILayout.EndScrollView();
        }

        static void DrawSection(string title, string body)
        {
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            EditorGUILayout.LabelField(body, EditorStyles.wordWrappedLabel);
            GUILayout.Space(10);
        }

        static void DrawDiscordLink()
        {
            EditorGUILayout.LabelField("Community", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(
                "Questions, feedback, and updates:",
                EditorStyles.wordWrappedLabel);

            var linkStyle = new GUIStyle(EditorStyles.linkLabel) { wordWrap = true };
            if (GUILayout.Button(new GUIContent("Join the Lumenwake Discord", k_discordUrl), linkStyle))
                Application.OpenURL(k_discordUrl);
        }

        static void RunSetup()
        {
            ToolbarShortcutsSetup.Run();
            Debug.Log("Toolbar Shortcuts: setup completed.");

            if (HasOpenInstances<ToolbarShortcutsWelcomeWindow>())
                GetWindow<ToolbarShortcutsWelcomeWindow>().Close();
        }
    }
}

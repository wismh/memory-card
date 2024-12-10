using System;
using ToolbarShortcuts.Editor.Bootstrap;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

namespace ToolbarShortcuts.Editor.Setup
{
    public static class ToolbarShortcutsSetup
    {
        const int k_maxApplyAttempts = 8;

        public static void Run()
        {
            EnableAllShortcuts();
            ApplyToolbarVisibility(0);
            ToolbarShortcutsSettings.MarkSetupCompleted();

            ToolbarShortcutsToolbar.RefreshAll();
            BootstrapToolbar.Refresh();
            EditorApplication.delayCall += OpenShortcutsWindow;
        }

        static void OpenShortcutsWindow() => ToolbarShortcutsWindow.OpenDockedAboveInspector();

        static void EnableAllShortcuts()
        {
            ToolbarShortcutsConfig config = ToolbarShortcutsRegistry.Config;
            if (config == null || config.shortcuts == null)
                return;

            bool changed = false;
            foreach (ToolbarShortcutEntry entry in config.shortcuts)
            {
                if (entry == null || entry.enabled)
                    continue;

                entry.enabled = true;
                changed = true;
            }

            if (!changed)
                return;

            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssetIfDirty(config);
        }

        static void ApplyToolbarVisibility(int attempt)
        {
            if (TryApplyToolbarVisibility())
                return;

            if (attempt >= k_maxApplyAttempts)
            {
                Debug.LogWarning(
                    "Toolbar Shortcuts: could not configure the main toolbar yet. " +
                    "Open Tools → Toolbar Shortcuts → Setup Toolbar after the Editor finishes loading.");
                return;
            }

            EditorApplication.delayCall += () => ApplyToolbarVisibility(attempt + 1);
        }

        static bool TryApplyToolbarVisibility()
        {
            EditorWindow toolbarWindow = FindMainToolbarWindow();
            if (toolbarWindow == null)
                return false;

            OverlayCanvas canvas = toolbarWindow.overlayCanvas;
            if (canvas == null || canvas.overlays == null)
                return false;

            bool playHidden = false;
            bool anyPackageOverlay = false;

            foreach (Overlay overlay in canvas.overlays)
            {
                if (overlay == null || string.IsNullOrEmpty(overlay.id))
                    continue;

                if (overlay.id == ToolbarShortcutsSettings.DefaultPlayOverlayId)
                {
                    overlay.displayed = false;
                    playHidden = true;
                    continue;
                }

                if (!overlay.id.StartsWith(ToolbarShortcutsSettings.PackageOverlayPrefix, StringComparison.Ordinal))
                    continue;

                overlay.displayed = true;
                anyPackageOverlay = true;
            }

            return playHidden && anyPackageOverlay;
        }

        static EditorWindow FindMainToolbarWindow()
        {
            foreach (EditorWindow window in Resources.FindObjectsOfTypeAll<EditorWindow>())
            {
                if (window != null && window.GetType().Name == "MainToolbarWindow")
                    return window;
            }

            return null;
        }
    }
}

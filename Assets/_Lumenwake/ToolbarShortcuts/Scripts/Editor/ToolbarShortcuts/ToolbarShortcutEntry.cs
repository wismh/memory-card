using System;
using UnityEditor;
using UnityEngine;

namespace ToolbarShortcuts.Editor
{
    [Serializable]
    public class ToolbarShortcutEntry
    {
        [HideInInspector] public bool _foldout = true;

        public bool enabled = true;
        public string label = "Shortcut";
        [TextArea] public string tooltip;

        public ToolbarShortcutPlacement placement = ToolbarShortcutPlacement.Toolbar;
        public ToolbarShortcutDock dock = ToolbarShortcutDock.Left;
        public int order;

        public Texture2D icon;

        public ToolbarShortcutActionType actionType = ToolbarShortcutActionType.OpenAsset;

        public UnityEngine.Object asset;

        public SceneAsset primaryScene;
        public SceneAsset[] additiveScenes;

        public ToolbarShortcutWindowOpenKind windowOpenKind = ToolbarShortcutWindowOpenKind.UnityWindow;
        public ToolbarShortcutWindowTarget windowTarget = ToolbarShortcutWindowTarget.SceneView;
        public string menuPath;

        public string staticTypeName;
        public string staticMethodName;
    }
}

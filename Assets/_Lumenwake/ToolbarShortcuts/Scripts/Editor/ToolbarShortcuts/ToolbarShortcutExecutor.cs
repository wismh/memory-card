using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ToolbarShortcuts.Editor
{
    public static class ToolbarShortcutExecutor
    {
        public static void Execute(ToolbarShortcutEntry entry)
        {
            if (entry == null || !entry.enabled)
                return;

            switch (entry.actionType)
            {
                case ToolbarShortcutActionType.OpenAsset:
                    OpenAsset(entry);
                    break;
                case ToolbarShortcutActionType.SelectAsset:
                    SelectAsset(entry);
                    break;
                case ToolbarShortcutActionType.OpenScene:
                    OpenScenes(entry);
                    break;
                case ToolbarShortcutActionType.OpenWindow:
                    ToolbarShortcutWindowOpener.Open(entry);
                    break;
                case ToolbarShortcutActionType.InvokeStaticMethod:
                    InvokeStaticMethod(entry);
                    break;
            }
        }

        static void OpenAsset(ToolbarShortcutEntry entry)
        {
            if (entry.asset == null)
            {
                Debug.LogWarning("Toolbar shortcut: no asset assigned.");
                return;
            }

            if (entry.asset is SceneAsset sceneAsset)
            {
                OpenSceneAsset(sceneAsset, OpenSceneMode.Single, promptBeforeOpen: true);
                return;
            }

            AssetDatabase.OpenAsset(entry.asset);
        }

        static void SelectAsset(ToolbarShortcutEntry entry)
        {
            if (entry.asset == null)
            {
                Debug.LogWarning("Toolbar shortcut: no asset assigned.");
                return;
            }

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = entry.asset;
            EditorGUIUtility.PingObject(entry.asset);
        }

        static void OpenScenes(ToolbarShortcutEntry entry)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            if (entry.primaryScene != null)
            {
                OpenSceneAsset(entry.primaryScene, OpenSceneMode.Single, promptBeforeOpen: false);

                if (entry.additiveScenes != null)
                {
                    foreach (SceneAsset additive in entry.additiveScenes)
                    {
                        if (additive == null)
                            continue;

                        OpenSceneAsset(additive, OpenSceneMode.Additive, promptBeforeOpen: false);
                    }
                }

                return;
            }

            if (entry.additiveScenes == null || entry.additiveScenes.Length == 0)
            {
                Debug.LogWarning("Toolbar shortcut: no scenes assigned.");
                return;
            }

            foreach (SceneAsset scene in entry.additiveScenes)
            {
                if (scene == null)
                    continue;

                OpenSceneAsset(scene, OpenSceneMode.Additive, promptBeforeOpen: false);
            }
        }

        static void OpenSceneAsset(SceneAsset sceneAsset, OpenSceneMode mode, bool promptBeforeOpen)
        {
            if (promptBeforeOpen && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            string path = AssetDatabase.GetAssetPath(sceneAsset);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning($"Toolbar shortcut: scene asset '{sceneAsset.name}' has no path.");
                return;
            }

            EditorSceneManager.OpenScene(path, mode);
        }

        static void InvokeStaticMethod(ToolbarShortcutEntry entry)
        {
            if (string.IsNullOrWhiteSpace(entry.staticTypeName) || string.IsNullOrWhiteSpace(entry.staticMethodName))
            {
                Debug.LogWarning("Toolbar shortcut: static type or method name is empty.");
                return;
            }

            Type type = ResolveType(entry.staticTypeName.Trim());
            if (type == null)
            {
                Debug.LogError($"Toolbar shortcut: type not found '{entry.staticTypeName}'.");
                return;
            }

            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            MethodInfo method = type.GetMethod(entry.staticMethodName.Trim(), flags);
            if (method == null)
            {
                Debug.LogError(
                    $"Toolbar shortcut: static method '{entry.staticMethodName}' not found on '{type.FullName}'.");
                return;
            }

            if (method.GetParameters().Length > 0)
            {
                Debug.LogError(
                    $"Toolbar shortcut: '{type.FullName}.{method.Name}' must be parameterless.");
                return;
            }

            try
            {
                method.Invoke(null, null);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        static Type ResolveType(string typeName)
        {
            Type type = Type.GetType(typeName);
            if (type != null)
                return type;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(typeName);
                if (type != null)
                    return type;
            }

            return null;
        }
    }
}

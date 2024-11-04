// Copyright (c) 2024 Liquid Glass Studios. All rights reserved.

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Manul.Toolbar
{	
	public class ManulToolbarMenuItems
	{

		#region Upper Menu Bar

		[MenuItem("Tools/Manul Toolbar/Open Manul Toolbar Settings")]
		private static void Toolbar_OpenManulToolbarSettings()
		{
			if (ManulToolbar.settings == null) return;

#if UNITY_2021_1_OR_NEWER
			EditorUtility.OpenPropertyEditor(ManulToolbar.settings);
#else
			// Selection.activeObject = settings;
#endif
		}

		[MenuItem("Tools/Manul Toolbar/Select Manul Toolbar Settings")]
		private static void Toolbar_SelectManulToolbarSettings()
		{
			if (ManulToolbar.settings == null) return;

			Selection.activeObject = ManulToolbar.settings;
		}

		#endregion

		#region Project Window

		/// Left Side Button

		[MenuItem("Assets/Manul Toolbar/(Left Side) Create Button/Open Asset")]
		private static void CreateButtonLeft_OpenAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Left, ToolbarButtonType.OpenAsset);
		}

		[MenuItem("Assets/Manul Toolbar/(Left Side) Create Button/Select Asset")]
		private static void CreateButtonLeft_SelectAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Left, ToolbarButtonType.SelectAsset);
		}

		[MenuItem("Assets/Manul Toolbar/(Left Side) Create Button/Open and Select Asset")]
		private static void CreateButtonLeft_OpenSelectAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Left, ToolbarButtonType.OpenAsset, true);
		}

		[MenuItem("Assets/Manul Toolbar/(Left Side) Create Button/Show in Explorer")]
		private static void CreateButtonLeft_ShowInExplorers(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Left, ToolbarButtonType.ShowAssetInExplorer);
		}

		[MenuItem("Assets/Manul Toolbar/(Left Side) Create Button/Show Properties")]
		private static void CreateButtonLeft_ShowProperties(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Left, ToolbarButtonType.PropertiesWindow);
		}

		[MenuItem("Assets/Manul Toolbar/(Left Side) Create Button/Open Folder")]
		private static void CreateButtonLeft_OpenAssetByPath(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Left, ToolbarButtonType.OpenFolder);
		}

		[MenuItem("Assets/Manul Toolbar/(Left Side) Create Button/Load Scene Additive")]
		private static void CreateButtonLeft_LoadSceneAdditive(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Left, ToolbarButtonType.LoadSceneAdditive);
		}

		/// Left Side List

		[MenuItem("Assets/Manul Toolbar/(Left Side) Create List/Open Asset")]
		private static void CreateListLeft_OpenAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateList(ToolbarEntrySideType.Left, ToolbarButtonType.OpenAsset);
		}

		[MenuItem("Assets/Manul Toolbar/(Left Side) Create List/Select Asset")]
		private static void CreateListLeft_SelectAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateList(ToolbarEntrySideType.Left, ToolbarButtonType.SelectAsset);
		}

		[MenuItem("Assets/Manul Toolbar/(Left Side) Create List/Show in Explorer")]
		private static void CreateListLeft_ShowInExplorers(MenuCommand menuCommand)
		{
			ManulToolbar.CreateList(ToolbarEntrySideType.Left, ToolbarButtonType.ShowAssetInExplorer);
		}

		[MenuItem("Assets/Manul Toolbar/(Left Side) Create List/Show Properties")]
		private static void CreateListLeft_ShowProperties(MenuCommand menuCommand)
		{
			ManulToolbar.CreateList(ToolbarEntrySideType.Left, ToolbarButtonType.PropertiesWindow);
		}

		[MenuItem("Assets/Manul Toolbar/(Left Side) Create List/Open Folder")]
		private static void CreateListLeft_OpenAssetByPath(MenuCommand menuCommand)
		{
			ManulToolbar.CreateList(ToolbarEntrySideType.Left, ToolbarButtonType.OpenFolder);
		}

		[MenuItem("Assets/Manul Toolbar/(Left Side) Create List/Load Scene Additive")]
		private static void CreateListLeft_LoadSceneAdditive(MenuCommand menuCommand)
		{
			ManulToolbar.CreateList(ToolbarEntrySideType.Left, ToolbarButtonType.LoadSceneAdditive);
		}

		/// Right Side Button

		[MenuItem("Assets/Manul Toolbar/(Right Side) Create Button/Open Asset")]
		private static void CreateButtonRight_OpenAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Right, ToolbarButtonType.OpenAsset);
		}

		[MenuItem("Assets/Manul Toolbar/(Right Side) Create Button/Select Asset")]
		private static void CreateButtonRight_SelectAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Right, ToolbarButtonType.SelectAsset);
		}

		[MenuItem("Assets/Manul Toolbar/(Right Side) Create Button/Open and Select Asset")]
		private static void CreateButtonRight_OpenSelectAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Right, ToolbarButtonType.OpenAsset, true);
		}

		[MenuItem("Assets/Manul Toolbar/(Right Side) Create Button/Show in Explorer")]
		private static void CreateButtonRight_ShowInExplorers(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Right, ToolbarButtonType.ShowAssetInExplorer);
		}

		[MenuItem("Assets/Manul Toolbar/(Right Side) Create Button/Show Properties")]
		private static void CreateButtonRight_ShowProperties(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Right, ToolbarButtonType.PropertiesWindow);
		}

		[MenuItem("Assets/Manul Toolbar/(Right Side) Create Button/Open Folder")]
		private static void CreateButtonRight_OpenAssetByPath(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Right, ToolbarButtonType.OpenFolder);
		}

		[MenuItem("Assets/Manul Toolbar/(Right Side) Create Button/Load Scene Additive")]
		private static void CreateButtonRight_LoadSceneAdditive(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Right, ToolbarButtonType.LoadSceneAdditive);
		}

		/// Right Side List

		[MenuItem("Assets/Manul Toolbar/(Right Side) Create List/Open Asset")]
		private static void CreateListRight_OpenAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateList(ToolbarEntrySideType.Right, ToolbarButtonType.OpenAsset);
		}

		[MenuItem("Assets/Manul Toolbar/(Right Side) Create List/Select Asset")]
		private static void CreateListRight_SelectAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateList(ToolbarEntrySideType.Right, ToolbarButtonType.SelectAsset);
		}

		[MenuItem("Assets/Manul Toolbar/(Right Side) Create List/Show in Explorer")]
		private static void CreateListRight_ShowInExplorers(MenuCommand menuCommand)
		{
			ManulToolbar.CreateList(ToolbarEntrySideType.Right, ToolbarButtonType.ShowAssetInExplorer);
		}

		[MenuItem("Assets/Manul Toolbar/(Right Side) Create List/Show Properties")]
		private static void CreateListRight_ShowProperties(MenuCommand menuCommand)
		{
			ManulToolbar.CreateList(ToolbarEntrySideType.Right, ToolbarButtonType.PropertiesWindow);
		}

		[MenuItem("Assets/Manul Toolbar/(Right Side) Create List/Open Folder")]
		private static void CreateListRight_OpenAssetByPath(MenuCommand menuCommand)
		{
			ManulToolbar.CreateList(ToolbarEntrySideType.Right, ToolbarButtonType.OpenFolder);
		}

		[MenuItem("Assets/Manul Toolbar/(Right Side) Create List/Load Scene Additive")]
		private static void CreateListRight_LoadSceneAdditive(MenuCommand menuCommand)
		{
			ManulToolbar.CreateList(ToolbarEntrySideType.Right, ToolbarButtonType.LoadSceneAdditive);
		}

		#endregion

		#region GameObject in Scene

		[MenuItem("GameObject/Manul Toolbar/(Left Side) Create Button/Find GameObject (in current scene)", false, 20)]
		private static void CreateButtonLeft_FindGameObject(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Left, ToolbarButtonType.FindGameobject);
		}

		[MenuItem("GameObject/Manul Toolbar/(Left Side) Create List/Find GameObject (in current scene)", false, 20)]
		private static void CreateLislLeft_FindGameObject(MenuCommand menuCommand)
		{
			ManulToolbar.CreateList(ToolbarEntrySideType.Left, ToolbarButtonType.FindGameobject);
		}

		[MenuItem("GameObject/Manul Toolbar/(Right Side) Create Button/Find GameObject (in current scene)", false, 20)]
		private static void CreateButtonRight_FindGameObject(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Right, ToolbarButtonType.FindGameobject);
		}

		[MenuItem("GameObject/Manul Toolbar/(Right Side) Create List/Find GameObject (in current scene)", false, 20)]
		private static void CreateLislRight_FindGameObject(MenuCommand menuCommand)
		{
			ManulToolbar.CreateList(ToolbarEntrySideType.Right, ToolbarButtonType.FindGameobject);
		}

		#endregion
	}
}

#endif
// Copyright (c) 2024 Liquid Glass Studios. All rights reserved.

#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor.Compilation; 
using UnityEditor;
using System;

namespace Manul.Toolbar
{
	[CreateAssetMenu(fileName = "Manul Toolbar Methods", menuName = "Manul Tools/Manul Toolbar Methods")]
	public class ManulToolbarMethods : ScriptableObject
	{
		public void ClearPrefs()
		{
			if (EditorUtility.DisplayDialog(ManulToolbarMessages.GetModal(Modal.Warning), ManulToolbarMessages.GetModal(Modal.ClearPrefsInfo), "OK", "Cancel"))
			{
				ManulToolbarMessages.ShowMessage(Message.PlayerPrefsCleared, MessageType.Info);
				PlayerPrefs.DeleteAll();
			} 
		}

		public void RecompileScripts()
		{
			CompilationPipeline.RequestScriptCompilation();
		}

		public void ReserializeAssets()
		{
			if (Selection.assetGUIDs.Length == 0)
			{
				ManulToolbarMessages.ShowMessage(Message.NoAssetsSelected, MessageType.Warning);
				return;
			}

			if (EditorUtility.DisplayDialog(ManulToolbarMessages.GetModal(Modal.Warning), ManulToolbarMessages.GetModal(Modal.ReserializeInfo), "Reserialize", "Cancel"))
			{
				AssetDatabase.ForceReserializeAssets(Array.ConvertAll(Selection.assetGUIDs, AssetDatabase.GUIDToAssetPath));
			}
		}

		public void ReserializeAllAssets()
		{
			if (EditorUtility.DisplayDialog(ManulToolbarMessages.GetModal(Modal.Warning), ManulToolbarMessages.GetModal(Modal.ReserializeAllInfo), "Reserialize", "Cancel"))
			{
				AssetDatabase.ForceReserializeAssets();
			}
		} 
	}
}

#endif
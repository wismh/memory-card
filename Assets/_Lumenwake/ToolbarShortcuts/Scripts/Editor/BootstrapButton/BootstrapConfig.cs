using UnityEditor;
using UnityEngine;

namespace ToolbarShortcuts.Editor.Bootstrap
{
    [CreateAssetMenu(fileName = "BootstrapConfig", menuName = "ToolbarShortcuts/Bootstrap Config")]
    public class BootstrapConfig : ScriptableObject
    {
        public SceneAsset bootstrapScene;
    }
}
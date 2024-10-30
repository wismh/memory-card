using UnityEditor;
using UnityEngine;

namespace Lumenwake.Editor.BootstrapButton
{
    [CreateAssetMenu(menuName = "Editor/Bootstrap Config")]
    public class BootstrapConfig : ScriptableObject
    {
        public SceneAsset bootstrapScene;
    }
}
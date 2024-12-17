using UnityEngine;

namespace Project.Sound
{
    [CreateAssetMenu(fileName = "SoundAsset", menuName = "Project/SoundAsset")]
    public class SoundAsset : ScriptableObject
    {
        [field: SerializeField] public AudioClip Clip { get; private set; }
        [field: SerializeField] public float Volume { get; private set; }
    }
}
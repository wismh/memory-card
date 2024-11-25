using UnityEngine;

namespace Project.Features.SoundModule
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        public void Play(SoundAsset sound)
        {
            _audioSource.volume = sound.Volume;
            _audioSource.PlayOneShot(sound.Clip);
        }
    }
}
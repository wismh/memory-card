using UnityEngine;
using UnityEngine.UI;

namespace Project.Features.SoundModule
{
    public class PlaySoundOnButtonClick : MonoBehaviour
    {
        [SerializeField] private AudioPlayer _audioPlayer;
        [SerializeField] private SoundAsset _soundAsset;
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }
        
        private void HandleButtonClick()
        {
            _audioPlayer.Play(_soundAsset);
        }
    }
}
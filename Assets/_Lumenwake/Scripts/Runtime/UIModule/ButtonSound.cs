using Project.Core.AudioSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Lumenwake.UIModule
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public sealed class ButtonSound : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [Header("Optional")]
        [SerializeField] private SoundData _clickSoundOverride;

        private IAudioSystem _audioSystem;
        private UiSoundsConfig _uiSounds;
        
        [Inject]
        public void Construct(IAudioSystem audioSystem, UiSoundsConfig uiSounds)
        {
            _audioSystem = audioSystem;
            _uiSounds = uiSounds;
        }

        private void Awake() =>
            _button.onClick.AddListener(HandleClick);

        private void OnDestroy() =>
            _button.onClick.RemoveListener(HandleClick);

        private void HandleClick()
        {
            SoundData sound = _clickSoundOverride ? _clickSoundOverride : _uiSounds.ButtonClick;
            _audioSystem.PlaySfx(sound);
        }
    }
}

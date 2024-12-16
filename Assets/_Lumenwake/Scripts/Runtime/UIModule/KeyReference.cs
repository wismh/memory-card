using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Lumenwake.UIModule
{
    public enum KeyReferenceState
    {
        Idle,
        WaitingForInput
    }

    /// <summary>
    /// Displays a <see cref="Key"/> and requests rebinding through <see cref="InputRebindManager"/>.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class KeyReference : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Key _initialKey;

        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _label;
        
        [Header("Visuals")]
        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _waitingSprite;
        [SerializeField] private Sprite _conflictSprite;

        [Header("Rebinding")]
        [SerializeField, Min(0f)] private float _listenDebounceSeconds = 0.12f;

        private KeyReferenceState _state = KeyReferenceState.Idle;
        private Key _currentKey;
        private bool _isConflicted;

        private InputRebindManager _rebindManager;

        public KeyReferenceState State => _state;

        public Key CurrentKey => _currentKey;

        /// <summary>Debounce after starting listen, read by <see cref="InputRebindManager"/>.</summary>
        public float ListenDebounceSeconds => _listenDebounceSeconds;

        public event Action<Key> OnKeyChanged;

        [Inject]
        private void Construct(InputRebindManager rebindManager)
        {
            _rebindManager = rebindManager;
        }

        private void Awake()
        {
            _currentKey = _initialKey;
            RefreshLabel();
            RefreshBackgroundSprite();
        }

        private void OnEnable()
        {
            _rebindManager?.Register(this);
        }

        private void OnDisable()
        {
            _rebindManager?.StopListeningIfActive(this);
            _rebindManager?.Unregister(this);
        }

        /// <summary>Sets the bound key and updates the label. Cancels an in-progress rebind.</summary>
        public void SetKey(Key key)
        {
            _rebindManager?.StopListeningIfActive(this);

            if (_currentKey == key)
                return;

            _currentKey = key;
            RefreshLabel();
            OnKeyChanged?.Invoke(key);
            _rebindManager?.RecalculateBindingConflicts();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_state == KeyReferenceState.WaitingForInput)
                return;

            _rebindManager.StartListening(this);
        }

        /// <summary>Called by <see cref="InputRebindManager"/> when duplicate bindings are detected or cleared.</summary>
        public void SetConflictState(bool isConflicted)
        {
            _isConflicted = isConflicted;
            RefreshBackgroundSprite();
        }

        /// <summary>Called by <see cref="InputRebindManager"/> when this control becomes the active listen target.</summary>
        public void EnterWaitingFromManager()
        {
            _state = KeyReferenceState.WaitingForInput;
            RefreshBackgroundSprite();
        }

        /// <summary>Called by <see cref="InputRebindManager"/> when rebinding is cancelled or superseded.</summary>
        public void CancelListeningFromManager()
        {
            if (_state != KeyReferenceState.WaitingForInput)
                return;

            _state = KeyReferenceState.Idle;
            RefreshBackgroundSprite();
        }

        /// <summary>Called by <see cref="InputRebindManager"/> when a new key is chosen.</summary>
        public void ApplyKey(Key key)
        {
            if (_state != KeyReferenceState.WaitingForInput)
                return;

            var changed = _currentKey != key;
            _currentKey = key;
            RefreshLabel();
            if (changed)
                OnKeyChanged?.Invoke(key);

            _state = KeyReferenceState.Idle;
        }

        private void RefreshLabel()
        {
            var kb = Keyboard.current;
            var ctrl = kb?[_currentKey];
            if (ctrl != null)
            {
                _label.text = ctrl.displayName;
                return;
            }

            _label.text = _currentKey.ToString();
        }

        private void RefreshBackgroundSprite()
        {
            if (_state == KeyReferenceState.WaitingForInput && _waitingSprite)
            {
                _background.sprite = _waitingSprite;
                return;
            }

            if (_isConflicted && _conflictSprite)
            {
                _background.sprite = _conflictSprite;
                return;
            }

            if (_normalSprite)
                _background.sprite = _normalSprite;
        }
    }
}

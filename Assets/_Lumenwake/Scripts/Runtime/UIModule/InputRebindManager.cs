using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Zenject;

namespace Lumenwake.UIModule
{
    /// <summary>
    /// Single shared keyboard listener for UI key rebinding. Tracks <see cref="KeyReference"/> registrations and binding conflicts.
    /// </summary>
    public sealed class InputRebindManager : IInitializable, IDisposable
    {
        private readonly HashSet<KeyReference> _registered = new();

        private InputAction _keyboardListenAction;
        private KeyReference _active;
        private float _listenArmTime;

        public void Initialize()
        {
            _keyboardListenAction = new InputAction(
                name: $"{nameof(InputRebindManager)}_KeyboardListen",
                type: InputActionType.PassThrough,
                binding: "<Keyboard>/*");

            _keyboardListenAction.performed += OnKeyboardListenPerformed;
        }

        public void Dispose()
        {
            _keyboardListenAction?.Disable();

            if (_active)
            {
                _active.CancelListeningFromManager();
                _active = null;
            }

            if (_keyboardListenAction != null)
            {
                _keyboardListenAction.performed -= OnKeyboardListenPerformed;
                _keyboardListenAction.Dispose();
                _keyboardListenAction = null;
            }

            _registered.Clear();
        }

        public void Register(KeyReference reference)
        {
            if (!reference)
                return;

            _registered.Add(reference);
            RecalculateBindingConflicts();
        }

        public void Unregister(KeyReference reference)
        {
            if (!reference)
                return;

            _registered.Remove(reference);
            RecalculateBindingConflicts();
        }

        /// <summary>Re-evaluates duplicate keys across registered references and updates conflict visuals.</summary>
        public void RecalculateBindingConflicts()
        {
            if (_registered.Count == 0)
                return;

            var counts = new Dictionary<Key, int>();
            foreach (var r in _registered)
            {
                if (!r)
                    continue;

                var k = r.CurrentKey;
                counts.TryGetValue(k, out var n);
                counts[k] = n + 1;
            }

            foreach (var r in _registered)
            {
                if (!r)
                    continue;

                var conflicted = counts[r.CurrentKey] > 1;
                r.SetConflictState(conflicted);
            }
        }

        /// <summary>Begins listening for the next keyboard key and binds it to <paramref name="target"/>.</summary>
        public void StartListening(KeyReference target)
        {
            if (!target)
                return;

            if (_active && _active != target)
            {
                _keyboardListenAction?.Disable();
                _active.CancelListeningFromManager();
            }

            _active = target;
            _listenArmTime = Time.unscaledTime + target.ListenDebounceSeconds;
            _active.EnterWaitingFromManager();
            _keyboardListenAction?.Enable();
            RecalculateBindingConflicts();
        }

        /// <summary>Ends listening if <paramref name="reference"/> is the active target (e.g. UI disabled).</summary>
        public void StopListeningIfActive(KeyReference reference)
        {
            if (_active != reference)
                return;

            _active.CancelListeningFromManager();
            _keyboardListenAction?.Disable();
            _active = null;
            RecalculateBindingConflicts();
        }

        private void OnKeyboardListenPerformed(InputAction.CallbackContext ctx)
        {
            if (!_active)
                return;

            if (Time.unscaledTime < _listenArmTime)
                return;

            if (!ctx.ReadValueAsButton())
                return;

            if (ctx.control is not KeyControl keyControl)
                return;

            if (string.Equals(keyControl.name, "anyKey", StringComparison.Ordinal))
                return;

            var key = keyControl.keyCode;
            if (key == Key.None)
                return;

            var target = _active;

            if (key == Key.Escape)
            {
                target.CancelListeningFromManager();
                _keyboardListenAction?.Disable();
                _active = null;
                RecalculateBindingConflicts();
                return;
            }

            target.ApplyKey(key);
            _keyboardListenAction?.Disable();
            _active = null;
            RecalculateBindingConflicts();
        }
    }
}

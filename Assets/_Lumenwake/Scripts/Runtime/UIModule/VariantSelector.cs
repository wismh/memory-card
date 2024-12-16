using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lumenwake.UIModule
{
    /// <summary>
    /// Cycles through a string collection with left/right buttons and a TMP label.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class VariantSelector : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private TMP_Text _variantLabel;

        [Header("Parameters")]
        [SerializeField] private List<string> _initialVariants = new();
        [SerializeField] private int _initialIndex;

        private readonly List<string> _variants = new();
        private int _index;

        public event Action<string> OnValueChanged;

        /// <summary>Index of the current variant, or -1 when the collection is empty.</summary>
        public int CurrentIndex => _variants.Count == 0 ? -1 : _index;
        public string CurrentValue => _variants.Count == 0 ? string.Empty : _variants[_index];
        public IReadOnlyList<string> Variants => _variants;

        private void Awake()
        {
            _leftButton.onClick.AddListener(HandlePreviousClicked);
            _rightButton.onClick.AddListener(HandleNextClicked);

            SetVariantsInternal(_initialVariants, _initialIndex);
            RefreshPresentation();
        }

        private void OnDestroy()
        {
            _leftButton.onClick.RemoveListener(HandlePreviousClicked);
            _rightButton.onClick.RemoveListener(HandleNextClicked);
        }

        /// <summary>Replaces the variant list and optional starting index, then updates the UI and raises <see cref="OnValueChanged"/> if the value changed.</summary>
        public void SetVariants(IReadOnlyList<string> variants, int initialIndex = 0)
        {
            var before = CurrentValue;
            SetVariantsInternal(variants, initialIndex);
            RefreshPresentation();

            var after = CurrentValue;
            if (!string.Equals(before, after, StringComparison.Ordinal))
                OnValueChanged?.Invoke(after);
        }

        /// <summary>Sets the current index (clamped). Updates the UI and raises <see cref="OnValueChanged"/> if the value changed.</summary>
        public void SetCurrentIndex(int index)
        {
            var before = CurrentValue;
            SetCurrentIndexInternal(index);
            RefreshPresentation();

            var after = CurrentValue;
            if (!string.Equals(before, after, StringComparison.Ordinal))
                OnValueChanged?.Invoke(after);
        }

        private void HandlePreviousClicked()
        {
            if (TrySelectPrevious(out var value))
            {
                RefreshPresentation();
                OnValueChanged?.Invoke(value);
            }
        }

        private void HandleNextClicked()
        {
            if (TrySelectNext(out var value))
            {
                RefreshPresentation();
                OnValueChanged?.Invoke(value);
            }
        }

        private void SetVariantsInternal(IReadOnlyList<string> variants, int initialIndex)
        {
            _variants.Clear();
            if (variants != null && variants.Count > 0)
                _variants.AddRange(variants);

            _index = _variants.Count == 0 ? 0 : ClampIndex(initialIndex, _variants.Count);
        }

        private void SetCurrentIndexInternal(int index)
        {
            if (_variants.Count == 0)
                return;

            _index = ClampIndex(index, _variants.Count);
        }

        private bool TrySelectNext(out string value)
        {
            value = CurrentValue;
            if (_variants.Count <= 1)
                return false;

            _index = (_index + 1) % _variants.Count;
            value = _variants[_index];
            return true;
        }

        private bool TrySelectPrevious(out string value)
        {
            value = CurrentValue;
            if (_variants.Count <= 1)
                return false;

            _index = (_index - 1 + _variants.Count) % _variants.Count;
            value = _variants[_index];
            return true;
        }

        private static int ClampIndex(int initialIndex, int count)
        {
            if (count <= 0)
                return 0;

            if (initialIndex < 0)
                initialIndex = 0;

            if (initialIndex >= count)
                initialIndex = count - 1;

            return initialIndex;
        }

        private void RefreshPresentation()
        {
            ApplyLabel(CurrentValue);
            ApplyNavigationInteractable();
        }

        private void ApplyLabel(string text) =>
            _variantLabel.text = text;

        private void ApplyNavigationInteractable()
        {
            var count = _variants.Count;
            var enableNav = count > 1;

            _leftButton.interactable = enableNav;
            _rightButton.interactable = enableNav;
        }
    }
}

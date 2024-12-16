using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lumenwake.UIModule
{
    [DisallowMultipleComponent]
    public sealed class TabButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [Header("Optional")]
        [Tooltip("If true, the selected tab becomes non-interactable. Leave false when using reselect (refresh) on the active tab.")]
        [SerializeField] private bool _toggleInteractableWithSelection;

        [Header("Hover")]
        [SerializeField] private HoverEffectView _hoverEffectView;

        private ITabSelectionMediator _mediator;
        private int _tabIndex;
        private bool _initialized;

        public int TabIndex => _tabIndex;

        /// <summary>Fired after the mediator acknowledges a click (before async work finishes).</summary>
        public event Action<int> SelectionRequested;

        private void Awake() =>
            _button.onClick.AddListener(HandleClicked);

        private void OnDestroy() =>
            _button.onClick.RemoveListener(HandleClicked);

        public void Initialize(ITabSelectionMediator mediator, int tabIndex)
        {
            _mediator = mediator;
            _tabIndex = tabIndex;
            _initialized = true;
        }

        public void SetSelectedVisual(bool isSelected)
        {
            if (_toggleInteractableWithSelection)
                _button.interactable = !isSelected;

            _hoverEffectView?.SetPrimarySelectionActive(isSelected);
        }

        private void HandleClicked()
        {
            if (!_initialized)
                return;

            SelectionRequested?.Invoke(_tabIndex);
            _mediator.RequestSelection(_tabIndex);
        }
    }
}

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lumenwake;
using UnityEngine;

namespace Lumenwake.UIModule
{
    public enum TabReSelectBehavior
    {
        Ignore,
        Refresh
    }

    [Serializable]
    public sealed class TabPageBinding
    {
        public TabButton Button;
        public BaseScreen Screen;
    }

    [DisallowMultipleComponent]
    public sealed class TabManager : MonoBehaviour, ITabSelectionMediator
    {
        [SerializeField] private List<TabPageBinding> _tabs = new();
        [SerializeField] private int _defaultTabIndex;
        [SerializeField] private TabReSelectBehavior _reSelectBehavior = TabReSelectBehavior.Ignore;

        private int _activeIndex = -1;
        private bool _busy;
        private int? _queuedTabIndex;

        /// <summary>Raised with (previousIndex, newIndex). previousIndex is -1 when there was no prior tab.</summary>
        public event Action<int, int> OnTabWillChange;

        /// <summary>Raised with the new tab index after the screen is visible.</summary>
        public event Action<int> OnTabDidChange;

        /// <summary>Raised when the active tab is selected again and <see cref="TabReSelectBehavior.Refresh"/> is set.</summary>
        public event Action<int> OnSameTabReselected;

        public int ActiveTabIndex => _activeIndex;

        private void Awake()
        {
            for (var i = 0; i < _tabs.Count; i++)
            {
                var binding = _tabs[i];
                binding.Button.Initialize(this, i);
                binding.Screen.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            if (_tabs.Count == 0)
                return;

            var startIndex = Mathf.Clamp(_defaultTabIndex, 0, _tabs.Count - 1);
            RequestSelection(startIndex);
        }

        private void OnValidate()
        {
            if (_tabs.Count > 0)
                _defaultTabIndex = Mathf.Clamp(_defaultTabIndex, 0, _tabs.Count - 1);
        }

        public void RequestSelection(int tabIndex)
        {
            if (_tabs.Count == 0)
                return;

            if (tabIndex < 0 || tabIndex >= _tabs.Count)
            {
                Debug.LogWarning($"{nameof(TabManager)}: invalid tab index {tabIndex}.", this);
                return;
            }

            if (_busy)
            {
                _queuedTabIndex = tabIndex;
                return;
            }

            ProcessSelectionQueueAsync(tabIndex).Forget();
        }

        private async UniTaskVoid ProcessSelectionQueueAsync(int firstIndex)
        {
            _busy = true;
            try
            {
                var next = (int?)firstIndex;
                while (next != null)
                {
                    var index = next.Value;
                    _queuedTabIndex = null;
                    await SelectTabInternalAsync(index);
                    next = _queuedTabIndex;
                }
            }
            finally
            {
                _busy = false;
            }
        }

        private async UniTask<Result> SelectTabInternalAsync(int tabIndex)
        {
            if (tabIndex == _activeIndex)
            {
                if (_reSelectBehavior == TabReSelectBehavior.Ignore)
                    return Result.Success;

                var binding = _tabs[tabIndex];
                if (binding.Screen is ITabReselectHandler handler)
                    await handler.OnTabReselectedAsync();

                OnSameTabReselected?.Invoke(tabIndex);
                return Result.Success;
            }

            var previousIndex = _activeIndex;

            OnTabWillChange?.Invoke(previousIndex, tabIndex);

            var nextBinding = _tabs[tabIndex];

            var loadResult = await nextBinding.Screen.LoadScreen();
            if (loadResult == Result.Failure)
                return Result.Failure;

            if (previousIndex >= 0)
            {
                var previousScreen = _tabs[previousIndex].Screen;
                await previousScreen.OnClose();
                previousScreen.gameObject.SetActive(false);
            }

            nextBinding.Screen.gameObject.SetActive(true);
            nextBinding.Screen.OnOpen();

            _activeIndex = tabIndex;
            RefreshTabVisuals();

            OnTabDidChange?.Invoke(tabIndex);

            return Result.Success;
        }

        private void RefreshTabVisuals()
        {
            for (var i = 0; i < _tabs.Count; i++)
                _tabs[i].Button.SetSelectedVisual(i == _activeIndex);
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Features.MenuModule
{
    public class LevelSelectView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private int _linkedLevelIndex;
        [SerializeField] private GameObject _lockVisual;

        public int LinkedLevelIndex => _linkedLevelIndex;

        public event Action<int> OnLevelSelect;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _button ??= GetComponentInChildren<Button>();
        }
#endif

        private void Start()
        {
            _button.onClick.AddListener(HandleClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(HandleClick);
        }

        public void SetLocked(bool locked)
        {
            _button.interactable = !locked;
            if (_lockVisual != null)
                _lockVisual.SetActive(locked);
        }

        private void HandleClick()
        {
            OnLevelSelect?.Invoke(_linkedLevelIndex);
        }
    }
}

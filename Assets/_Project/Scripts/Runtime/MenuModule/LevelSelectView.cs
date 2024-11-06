using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Features.MenuModule
{
    public class LevelSelectView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private int _linkedLevelIndex;
        
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
        
        private void OnDestroy() {
            _button.onClick.RemoveListener(HandleClick);
        }

        private void HandleClick()
        {
            OnLevelSelect?.Invoke(_linkedLevelIndex);
        }
    }
}

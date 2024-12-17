using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Hud
{
    public class WonPopupView : MonoBehaviour
    {
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _goBackButton;
        [SerializeField] private TextMeshProUGUI _timeLabel;
        
        public event Action OnRetry;
        public event Action OnGoBack;
        
        public void Show(float time)
        {
            gameObject.SetActive(true);
            SetTime(time);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void Start()
        {
            _retryButton.onClick.AddListener(HandleOnRetry);
            _goBackButton.onClick.AddListener(HandleOnGoBack);
        }

        private void OnDestroy()
        {
            _retryButton.onClick.RemoveListener(HandleOnRetry);
            _goBackButton.onClick.RemoveListener(HandleOnGoBack);
        } 

        private void SetTime(float time)
        {
            time = Mathf.Max(0, time);

            int totalSeconds = Mathf.FloorToInt(time);

            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            _timeLabel.text = $"{minutes:00}:{seconds:00}";
        }

        private void HandleOnRetry()
        {
            OnRetry?.Invoke();
        }

        private void HandleOnGoBack()
        {
            OnGoBack?.Invoke();
        }
    }
}

using TMPro;
using UnityEngine;

namespace Project.Hud
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerLabel;
        
        private int _lastDisplayedSeconds = -1;
        
        public void SetTime(float time)
        {
            time = Mathf.Max(0, time);

            int totalSeconds = Mathf.FloorToInt(time);
            if (totalSeconds == _lastDisplayedSeconds)
                return;

            _lastDisplayedSeconds = totalSeconds;

            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            _timerLabel.text = $"{minutes:00}:{seconds:00}";
        }
    }
}

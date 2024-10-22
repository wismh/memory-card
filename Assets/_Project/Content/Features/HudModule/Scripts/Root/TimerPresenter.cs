using UnityEngine;
using Zenject;

using Project.Features.LevelsModule;

namespace Project.Features.HudModule
{
    public class TimerPresenter : MonoBehaviour
    {
        [SerializeField] private TimerView _timerView;
        
        private LevelContext _levelContext;

        [Inject]
        public void InjectDependencies(LevelContext levelContext)
        {
            _levelContext = levelContext;
        }

        private void Update()
        {
            _timerView.SetTime(Time.time - _levelContext.LevelStartTime);
        }
    }
}

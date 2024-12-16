using Cysharp.Threading.Tasks;
using Lumenwake.UIModule;
using Project.Features.GameFlowStateMachineModule;
using Project.Features.LevelsModule;
using UnityEngine;
using Zenject;

namespace Project.Features.HudModule
{
    public class WonPopupPresenter : BaseScreen
    {
        [SerializeField] private WonPopupView _view;

        private GameFlowStateMachine _gameFlowStateMachine;
        private LevelResult _pendingResult;

        public override ScreenLayer Layer => ScreenLayer.Overlay;

        [Inject]
        public void InjectDependencies(GameFlowStateMachine gameFlowStateMachine)
        {
            _gameFlowStateMachine = gameFlowStateMachine;
        }

        public void Show(LevelResult levelResult)
        {
            _pendingResult = levelResult;
        }

        public override void OnOpen()
        {
            _view.Show(_pendingResult.Time);
        }

        public override UniTask OnClose()
        {
            _view.Hide();
            return UniTask.CompletedTask;
        }

        private void Awake()
        {
            _view.OnRetry += HandleRetry;
            _view.OnGoBack += HandleGoBack;
            _view.Hide();
        }

        private void OnDestroy()
        {
            _view.OnRetry -= HandleRetry;
            _view.OnGoBack -= HandleGoBack;
        }

        private void HandleGoBack()
        {
            _gameFlowStateMachine.Enter<GoMenuFlowState>();
        }

        private void HandleRetry()
        {
            _gameFlowStateMachine.Enter<ReloadRoundFlowState>();
        }
    }
}

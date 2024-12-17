using System.Collections.Generic;
using Lumenwake.UIModule;
using UnityEngine;
using Zenject;
namespace Project.Hud
{
    public class HudInstaller : MonoInstaller
    {
        [SerializeField] private WonPopupPresenter _wonPopupPresenter;

        public override void InstallBindings()
        {
            Container.Bind<WonPopupPresenter>()
                .FromInstance(_wonPopupPresenter);

            var screens = new List<BaseScreen> { _wonPopupPresenter };
            var screenManager = new BaseScreenManager(screens);

            Container.Bind<BaseScreenManager>()
                .FromInstance(screenManager);

            Container.Bind<IScreenManager>()
                .FromInstance(screenManager);
        }
    }
}

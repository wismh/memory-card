using System.Collections.Generic;
using Lumenwake;
using Lumenwake.UIModule;
using UnityEngine;
using Zenject;

namespace Project.Features.MenuModule
{
    public class MenuModuleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var screens = new List<BaseScreen>();

            var levelSelectPresenter = Object.FindFirstObjectByType<LevelSelectPresenter>(FindObjectsInactive.Include);
            if (levelSelectPresenter != null)
                screens.Add(levelSelectPresenter);
            else
                LoggingSystem.LogError($"{nameof(MenuModuleInstaller)}: no {nameof(LevelSelectPresenter)} found in the scene.");

            var screenManager = new BaseScreenManager(screens);

            Container.Bind<BaseScreenManager>()
                .FromInstance(screenManager);

            Container.Bind<IScreenManager>()
                .FromInstance(screenManager);

            Container.BindInterfacesTo<MenuScreenBootstrapper>()
                .AsSingle()
                .NonLazy();
        }
    }
}

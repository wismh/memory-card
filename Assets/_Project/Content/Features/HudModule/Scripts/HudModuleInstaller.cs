using Project.Features.HudModule;
using UnityEngine;
using Zenject;
namespace _Project.Content.Features.HudModule.Scripts
{
    public class HudModuleInstaller : MonoInstaller
    {
        [SerializeField] private WonPopupPresenter _wonPopupPresenter;
        
        public override void InstallBindings()
        {
            Container.Bind<WonPopupPresenter>()
                .FromInstance(_wonPopupPresenter);
        }
    }
}

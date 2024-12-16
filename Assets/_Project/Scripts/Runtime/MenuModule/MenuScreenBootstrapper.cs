using Cysharp.Threading.Tasks;
using Lumenwake.UIModule;
using Zenject;

namespace Project.Features.MenuModule
{
    public class MenuScreenBootstrapper : IInitializable
    {
        private readonly IScreenManager _screenManager;

        public MenuScreenBootstrapper(IScreenManager screenManager)
        {
            _screenManager = screenManager;
        }

        public void Initialize()
        {
            _screenManager.OpenScreen<LevelSelectPresenter>().Forget();
        }
    }
}

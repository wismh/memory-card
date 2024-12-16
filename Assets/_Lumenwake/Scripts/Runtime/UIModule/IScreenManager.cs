using Cysharp.Threading.Tasks;
using System;

namespace Lumenwake.UIModule
{
    public interface IScreenManager
    {
        UniTask<Result> OpenScreen(Type type);
        UniTask<Result> OpenScreen<T>() where T : BaseScreen;
        UniTask CloseScreen<T>() where T : BaseScreen;
        UniTask CloseAllScreens();
    }
}
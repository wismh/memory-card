using Cysharp.Threading.Tasks;

namespace Lumenwake.UIModule
{
    /// <summary>
    /// Optional hook on a <see cref="BaseScreen"/> when the user selects the tab that is already active.
    /// </summary>
    public interface ITabReselectHandler
    {
        UniTask OnTabReselectedAsync();
    }
}

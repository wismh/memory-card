using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lumenwake.UIModule
{
    public class BaseScreen : MonoBehaviour
    {
        private UniTask<Result> _loadingTask;

        /// <summary>Which independent stack (see <see cref="ScreenLayer"/>) this screen opens into.
        /// Override for anything meant to coexist with a screen in a different layer (e.g. a modal
        /// panel that shouldn't hide the HUD).</summary>
        public virtual ScreenLayer Layer => ScreenLayer.Default;

        public UniTask<Result> LoadScreen()
        {
            if (_loadingTask.Status == UniTaskStatus.Pending)
                return _loadingTask;

            return _loadingTask = LoadInternal();
        }

        protected virtual UniTask<Result> LoadInternal() => 
            UniTask.FromResult(Result.Success);

        public virtual void OnOpen() {}

        public virtual UniTask OnOpenAsync()
        {
            OnOpen();
            return UniTask.CompletedTask;
        }

        public virtual UniTask OnClose() =>
            UniTask.CompletedTask;
    }
}

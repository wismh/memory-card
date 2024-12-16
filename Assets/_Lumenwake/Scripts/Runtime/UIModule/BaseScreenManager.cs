using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lumenwake.UIModule
{
    public class BaseScreenManager : IScreenManager
    {
        private readonly Dictionary<Type, BaseScreen> _allScreens = new();
        private readonly HashSet<BaseScreen> _openingScreens = new();

        // One independent stack per ScreenLayer - opening a screen only hides the previous top of its
        // OWN layer, so e.g. a ScreenLayer.Default HUD stays visible while a ScreenLayer.Overlay panel
        // opens on top of it. See ScreenLayer's doc comment.
        private readonly Dictionary<ScreenLayer, List<BaseScreen>> _layerStacks = new();

        public BaseScreenManager(List<BaseScreen> allScreens)
        {
            foreach (var screen in allScreens)
                _allScreens.Add(screen.GetType(), screen);
        }

        public async UniTask<Result> OpenScreen(Type type)
        {
            if (!_allScreens.TryGetValue(type, out BaseScreen screen))
                return Result.Failure;

            if (!_openingScreens.Add(screen))
                return Result.Success;

            try
            {
                var result = await screen.LoadScreen();
                if (result == Result.Failure)
                    return result;

                List<BaseScreen> stack = GetStack(screen.Layer);

                if (stack.Count > 0)
                    stack[^1].gameObject.SetActive(false);

                int index = stack.IndexOf(screen);
                if (index != -1)
                    stack.RemoveAt(index);

                stack.Add(screen);

                stack[^1].gameObject.SetActive(true);
                await screen.OnOpenAsync();

                return result;
            } finally {
                _openingScreens.Remove(screen);
            }
        }

        public UniTask<Result> OpenScreen<T>() where T : BaseScreen
        {
            return OpenScreen(typeof(T));
        }

        public async UniTask CloseScreen<T>() where T : BaseScreen
        {
            var screen = _allScreens[typeof(T)];
            List<BaseScreen> stack = GetStack(screen.Layer);

            if (!stack.Remove(screen))
                return;

            await screen.OnClose();

            screen.gameObject.SetActive(false);
            if (stack.Count > 0)
                stack[^1].gameObject.SetActive(true);
        }

        public async UniTask CloseAllScreens()
        {
            var screens = _layerStacks.Values.SelectMany(stack => stack).ToList();
            foreach (List<BaseScreen> stack in _layerStacks.Values)
                stack.Clear();

            var tasks = screens
                .Select(async screen =>
                {
                    await screen.OnClose();
                    screen.gameObject.SetActive(false);
                })
                .ToArray();

            await UniTask.WhenAll(tasks);
        }

        public bool HasScreenInStack<T>() where T : BaseScreen
        {
            foreach (List<BaseScreen> stack in _layerStacks.Values)
            {
                foreach (BaseScreen screen in stack)
                {
                    if (screen is T)
                        return true;
                }
            }

            return false;
        }

        private List<BaseScreen> GetStack(ScreenLayer layer)
        {
            if (!_layerStacks.TryGetValue(layer, out List<BaseScreen> stack))
            {
                stack = new List<BaseScreen>();
                _layerStacks[layer] = stack;
            }

            return stack;
        }
    }
}

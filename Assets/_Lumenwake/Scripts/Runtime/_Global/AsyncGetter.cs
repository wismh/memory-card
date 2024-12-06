using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lumenwake
{
    public static class AsyncGetter
    {
        public static UniTask<T> WaitUntilValue<T>(
            Func<T> getter,
            Func<T, bool> predicate,
            Action<Action<T>> subscribe,
            Action<Action<T>> unsubscribe,
            CancellationToken cancellationToken = default)
        {
            var current = getter();
            if (predicate(current))
                return UniTask.FromResult(current);

            var tcs = new UniTaskCompletionSource<T>();

            void Handler(T value)
            {
                if (!predicate(value))
                    return;

                unsubscribe(Handler);
                tcs.TrySetResult(value);
            }

            subscribe(Handler);

            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(() =>
                {
                    unsubscribe(Handler);
                    tcs.TrySetCanceled();
                });
            }

            return tcs.Task;
        }
    }
}
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Core.SaveServiceModule
{
    /// <summary>Persists and restores game state (async load, debounced or immediate save).</summary>
    public interface ISaveService<TState> where TState : class, new()
    {
        TState State { get; }

        /// <summary>Loads save data on the calling thread (for bootstrap / Zenject constructors).</summary>
        TState Load();

        UniTask<TState> LoadAsync(CancellationToken cancellationToken = default);

        void SaveImmediate();

        void RequestSave();
    }
}

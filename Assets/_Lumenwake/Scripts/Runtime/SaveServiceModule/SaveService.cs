using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Core.SaveServiceModule.Migrations;
using Project.Core.SaveServiceModule.Serialization;
using UnityEngine;

namespace Project.Core.SaveServiceModule
{
    public sealed class SaveService<TState> : ISaveService<TState> where TState : class, new()
    {
        private const double DebounceDelayMs = 300;

        private readonly ISerializer _serializer;
        private readonly SaveMigrationRunner _migrationRunner;
        private readonly int _currentVersion;
        private readonly string _filePath;
        private readonly Action<TState, int> _setVersion;
        private readonly Func<TState> _createDefault;

        private TState _runtimeState = new();
        private bool _loaded;

        private readonly object _debounceLock = new();
        private CancellationTokenSource _debounceCts;

        public SaveService(
            ISerializer serializer,
            SaveMigrationRunner migrationRunner,
            int currentVersion,
            Action<TState, int> setVersion,
            string filePath = null,
            Func<TState> createDefault = null)
        {
            _serializer = serializer;
            _migrationRunner = migrationRunner;
            _currentVersion = currentVersion;
            _setVersion = setVersion;
            _createDefault = createDefault ?? (() => new TState());
            _filePath = string.IsNullOrEmpty(filePath)
                ? Path.Combine(Application.persistentDataPath, "save.json")
                : filePath;
        }

        public TState State
        {
            get
            {
                EnsureLoaded();
                return _runtimeState;
            }
        }

        public TState Load()
        {
            if (_loaded)
            {
                return _runtimeState;
            }

            string json = File.Exists(_filePath) ? File.ReadAllText(_filePath) : null;
            ApplyLoadedJson(json);
            return _runtimeState;
        }

        public async UniTask<TState> LoadAsync(CancellationToken cancellationToken = default)
        {
            if (_loaded)
            {
                return _runtimeState;
            }

            string json = null;
            if (File.Exists(_filePath))
            {
                json = await UniTask.RunOnThreadPool(
                    () => File.ReadAllText(_filePath),
                    cancellationToken: cancellationToken);
            }

            await UniTask.SwitchToMainThread(cancellationToken);

            if (_loaded)
            {
                return _runtimeState;
            }

            ApplyLoadedJson(json);
            return _runtimeState;
        }

        private void ApplyLoadedJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                _runtimeState = CreateDefaultState();
            }
            else
            {
                try
                {
                    JObject root = JObject.Parse(json);
                    int declaredVersion = SaveJsonVersion.Read(root);
                    if (declaredVersion > _currentVersion)
                    {
                        throw new InvalidOperationException(
                            $"Save file version {declaredVersion} is newer than supported {_currentVersion}.");
                    }

                    _migrationRunner.RunToCurrent(root);
                    _runtimeState = _serializer.Deserialize<TState>(root.ToString(Formatting.None)) ?? new TState();
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Save load failed; starting new game state. {ex}");
                    _runtimeState = CreateDefaultState();
                }
            }

            _setVersion(_runtimeState, _currentVersion);
            _loaded = true;
        }

        public void SaveImmediate()
        {
            CancelDebouncedSave();
            FlushToDiskBlocking();
        }

        public void RequestSave()
        {
            lock (_debounceLock)
            {
                _debounceCts?.Cancel();
                _debounceCts?.Dispose();
                _debounceCts = new CancellationTokenSource();
                CancellationToken token = _debounceCts.Token;
                DebouncedFlushAsync(token).Forget();
            }
        }

        private TState CreateDefaultState()
        {
            TState state = _createDefault();
            _setVersion(state, _currentVersion);
            return state;
        }

        private void EnsureLoaded()
        {
            if (!_loaded)
            {
                throw new InvalidOperationException("Cannot access save state before Load or LoadAsync has completed.");
            }
        }

        private async UniTaskVoid DebouncedFlushAsync(CancellationToken cancellationToken)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromMilliseconds(DebounceDelayMs), cancellationToken: cancellationToken);
                await FlushToDiskAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void CancelDebouncedSave()
        {
            lock (_debounceLock)
            {
                _debounceCts?.Cancel();
                _debounceCts?.Dispose();
                _debounceCts = null;
            }
        }

        private void FlushToDiskBlocking()
        {
            string json = BuildSaveJson();
            try
            {
                WriteJsonToDisk(json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Save failed: {ex.Message}");
            }
        }

        private async UniTask FlushToDiskAsync(CancellationToken cancellationToken)
        {
            string json = BuildSaveJson();
            try
            {
                await UniTask.RunOnThreadPool(
                    () => WriteJsonToDisk(json),
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Save failed: {ex.Message}");
            }
        }

        private void WriteJsonToDisk(string json)
        {
            string directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(_filePath, json);
        }

        private string BuildSaveJson()
        {
            EnsureLoaded();
            _setVersion(_runtimeState, _currentVersion);
            return _serializer.Serialize(_runtimeState);
        }
    }
}

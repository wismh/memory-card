using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Project.Core.SaveServiceModule.Migrations
{
    public sealed class SaveMigrationRunner
    {
        private readonly IReadOnlyDictionary<int, ISaveMigration> _byFromVersion;
        private readonly int _targetVersion;

        public SaveMigrationRunner(IEnumerable<ISaveMigration> migrations, int targetVersion)
        {
            _targetVersion = targetVersion;
            _byFromVersion = migrations?.ToDictionary(m => m.FromVersion, m => m)
                             ?? new Dictionary<int, ISaveMigration>();
        }

        public void RunToCurrent(JObject root)
        {
            while (true)
            {
                int version = SaveJsonVersion.Read(root);
                if (version >= _targetVersion)
                {
                    break;
                }

                if (!_byFromVersion.TryGetValue(version, out ISaveMigration migration))
                {
                    throw new InvalidOperationException(
                        $"No save migration registered from version {version} (target is {_targetVersion}).");
                }

                migration.Migrate(root);
            }
        }
    }
}

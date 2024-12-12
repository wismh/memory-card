using Newtonsoft.Json.Linq;

namespace Project.Core.SaveServiceModule.Migrations
{
    /// <summary>One forward step in the save schema pipeline; mutates the root JSON document in place.</summary>
    public interface ISaveMigration
    {
        int FromVersion { get; }

        int ToVersion { get; }

        void Migrate(JObject root);
    }
}

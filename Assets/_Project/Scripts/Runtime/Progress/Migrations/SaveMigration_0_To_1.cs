using Newtonsoft.Json.Linq;
using Project.Core.SaveServiceModule.Migrations;

namespace Project.Progress.Migrations
{
    public sealed class SaveMigration_0_To_1 : ISaveMigration
    {
        public int FromVersion => 0;

        public int ToVersion => 1;

        public void Migrate(JObject root)
        {
            root["version"] = 1;
        }
    }
}

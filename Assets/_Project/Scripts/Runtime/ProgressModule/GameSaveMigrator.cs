using System;

namespace Project.Features.ProgressModule
{
    public static class GameSaveMigrator
    {
        public static GameSaveData MigrateToCurrent(GameSaveData data)
        {
            if (data == null)
                return CreateDefault();

            data.completedLevelIds ??= Array.Empty<string>();

            while (data.version < GameSaveVersion.Current)
            {
                switch (data.version)
                {
                    case 0:
                        break;
                    default:
                        data.version = GameSaveVersion.Current;
                        return data;
                }

                data.version++;
            }

            return data;
        }

        public static GameSaveData CreateDefault()
        {
            return new GameSaveData
            {
                version = GameSaveVersion.Current,
                completedLevelIds = Array.Empty<string>()
            };
        }
    }
}

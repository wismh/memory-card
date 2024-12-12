using System;
using System.IO;
using Project.Core.SaveServiceModule;
using Project.Core.SaveServiceModule.Migrations;
using Project.Features.ProgressModule.Migrations;
using UnityEngine;
using Zenject;

namespace Project.Features.ProgressModule
{
    public class ProgressModuleInstaller : Installer<ProgressModuleInstaller>
    {
        public const string SaveFileName = "game_progress.json";

        public override void InstallBindings()
        {
            SaveServiceModuleInstaller.Install(Container);

            Container.Bind<SaveMigrationRunner>()
                .FromInstance(new SaveMigrationRunner(
                    new ISaveMigration[] { new SaveMigration_0_To_1() },
                    GameSaveVersion.Current))
                .AsSingle();

            var savePath = Path.Combine(Application.persistentDataPath, SaveFileName);

            Container.Bind<ISaveService<GameSaveData>>()
                .To<SaveService<GameSaveData>>()
                .AsSingle()
                .WithArguments(
                    GameSaveVersion.Current,
                    (Action<GameSaveData, int>)SetVersion,
                    savePath)
                .NonLazy();

            Container.BindInterfacesTo<LevelProgressService>()
                .AsSingle()
                .NonLazy();

            Container.Bind<IMapAnimationIntent>()
                .To<MapAnimationIntent>()
                .AsSingle();
        }

        private static void SetVersion(GameSaveData state, int version) =>
            state.version = version;
    }
}

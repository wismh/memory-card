namespace Project.Features.ProgressModule
{
    public interface IGameSavePersistence
    {
        bool TryLoad(out GameSaveData data);
        void Save(GameSaveData data);
        void DeleteSave();
    }
}

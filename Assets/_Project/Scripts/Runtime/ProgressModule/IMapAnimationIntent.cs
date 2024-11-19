namespace Project.Features.ProgressModule
{
    public interface IMapAnimationIntent
    {
        void QueueAnimationAfterStageComplete(int completedLevelIndex, int levelCount);

        bool TryConsumeStageCompleteAnimation(out int fromLevelIndex, out int toLevelIndex);
    }
}

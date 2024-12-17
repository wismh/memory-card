using UnityEngine;

namespace Project.Progress
{
    public class MapAnimationIntent : IMapAnimationIntent
    {
        private bool _pending;
        private int _from;
        private int _to;

        public void QueueAnimationAfterStageComplete(int completedLevelIndex, int levelCount)
        {
            if (levelCount <= 0)
                return;

            var last = levelCount - 1;
            _from = Mathf.Clamp(completedLevelIndex, 0, last);
            _to = Mathf.Clamp(completedLevelIndex + 1, 0, last);
            _pending = true;
        }

        public bool TryConsumeStageCompleteAnimation(out int fromLevelIndex, out int toLevelIndex)
        {
            if (!_pending)
            {
                fromLevelIndex = 0;
                toLevelIndex = 0;
                return false;
            }

            _pending = false;
            fromLevelIndex = _from;
            toLevelIndex = _to;
            return true;
        }
    }
}

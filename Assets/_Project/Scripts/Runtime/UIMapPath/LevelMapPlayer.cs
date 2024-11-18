using System.Collections;
using UnityEngine;

namespace Project.Features.UIMapPath
{
    [DisallowMultipleComponent]
    public class LevelMapPlayer : MonoBehaviour
    {
        [SerializeField] private LevelMapPath _path;

        [SerializeField] private RectTransform _playerIcon;

        [Tooltip("If true, final position uses exact level mapping from LevelMapPath (recommended).")]
        [SerializeField] private bool _snapToLevelNodeAtEnd = true;

        [SerializeField] private float _moveDuration = 0.85f;

        [SerializeField] private AnimationCurve _moveEase = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private Coroutine _moveRoutine;
        private RectTransform _playerRect;

        public bool IsMoving { get; private set; }

        private RectTransform PlayerRect
        {
            get
            {
                if (_playerRect == null)
                    ResolvePlayerRect();
                return _playerRect;
            }
        }

        public LevelMapPath Path
        {
            get => _path;
            set => _path = value;
        }

        private void Awake() => ResolvePlayerRect();

        private void ResolvePlayerRect()
        {
            _playerRect = _playerIcon != null
                ? _playerIcon
                : GetComponent<RectTransform>();

            if (_playerRect == null)
                _playerRect = GetComponentInChildren<RectTransform>(true);
        }

        public void SetPositionInstant(int levelIndex)
        {
            if (_path == null || PlayerRect == null)
                return;

            StopMovementInternal();
            ApplyLevelPosition(levelIndex);
        }

        public void MoveToLevel(int levelIndex)
        {
            MoveToLevel(levelIndex, null);
        }

        public void MoveToLevel(int levelIndex, System.Action onComplete)
        {
            if (_path == null || PlayerRect == null)
            {
                onComplete?.Invoke();
                return;
            }

            if (_moveRoutine != null)
                StopCoroutine(_moveRoutine);

            _moveRoutine = StartCoroutine(MoveRoutine(levelIndex, onComplete));
        }

        public void StopMovement()
        {
            StopMovementInternal();
        }

        private void StopMovementInternal()
        {
            if (_moveRoutine != null)
            {
                StopCoroutine(_moveRoutine);
                _moveRoutine = null;
            }

            IsMoving = false;
        }

        private IEnumerator MoveRoutine(int targetLevelIndex, System.Action onComplete)
        {
            IsMoving = true;
            var start = PlayerRect.anchoredPosition;
            var endU = _path.GetLevelNormalizedProgress(targetLevelIndex);
            var endPos = _path.EvaluateAtArcLengthNormalized(endU);

            var startU = ProjectAnchoredToArcU(start);

            if (_snapToLevelNodeAtEnd && Mathf.Approximately(_moveDuration, 0f))
            {
                PlayerRect.anchoredPosition = endPos;
                IsMoving = false;
                onComplete?.Invoke();
                yield break;
            }

            var elapsed = 0f;
            while (elapsed < _moveDuration)
            {
                elapsed += Time.deltaTime;
                var rawT = Mathf.Clamp01(elapsed / _moveDuration);
                var eased = _moveEase != null ? _moveEase.Evaluate(rawT) : rawT;
                var u = Mathf.Lerp(startU, endU, eased);
                PlayerRect.anchoredPosition = _path.EvaluateAtArcLengthNormalized(u);
                yield return null;
            }

            if (_snapToLevelNodeAtEnd)
                PlayerRect.anchoredPosition = endPos;
            else
                PlayerRect.anchoredPosition = _path.EvaluateAtArcLengthNormalized(endU);

            IsMoving = false;
            _moveRoutine = null;
            onComplete?.Invoke();
        }

        private void ApplyLevelPosition(int levelIndex)
        {
            var u = _path.GetLevelNormalizedProgress(levelIndex);
            PlayerRect.anchoredPosition = _path.EvaluateAtArcLengthNormalized(u);
        }

        private float ProjectAnchoredToArcU(Vector2 anchoredPosition)
        {
            var bestU = 0f;
            var bestDist = float.MaxValue;
            const int steps = 64;
            for (var i = 0; i <= steps; i++)
            {
                var u = i / (float)steps;
                var p = _path.EvaluateAtArcLengthNormalized(u);
                var d = (p - anchoredPosition).sqrMagnitude;
                if (d < bestDist)
                {
                    bestDist = d;
                    bestU = u;
                }
            }

            return bestU;
        }
    }
}

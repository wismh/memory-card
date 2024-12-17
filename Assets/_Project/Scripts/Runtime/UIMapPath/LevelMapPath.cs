using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UIMapPath
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class LevelMapPath : MonoBehaviour
    {
        [SerializeField] private List<BezierPathWaypoint> _waypoints = new();

        [Tooltip("Samples per segment for arc-length table (higher = smoother distance mapping).")]
        [Min(4)]
        [SerializeField] private int _samplesPerSegment = 32;

        [Tooltip("Optional: normalized arc-length (0–1) for each level index. If empty, levels are evenly spaced along the path.")]
        [SerializeField] private List<float> _levelNormalizedProgress = new();

        private float[] _arcLengths;
        private Vector2[] _samplePoints;
        private float _totalArcLength;
        private bool _cacheDirty = true;

        public RectTransform Rect => (RectTransform)transform;

        public IReadOnlyList<BezierPathWaypoint> Waypoints => _waypoints;

        public int WaypointCount => _waypoints.Count;

        public float TotalArcLength
        {
            get
            {
                RebuildCacheIfNeeded();
                return _totalArcLength;
            }
        }

        private void OnEnable()
        {
            _cacheDirty = true;
        }

        private void OnValidate()
        {
            _cacheDirty = true;
        }

        public void SetCacheDirty()
        {
            _cacheDirty = true;
        }

        public BezierPathWaypoint GetWaypoint(int index) => _waypoints[index];

        public void SetWaypoint(int index, BezierPathWaypoint waypoint)
        {
            if (index < 0 || index >= _waypoints.Count)
                return;

            _waypoints[index] = waypoint;
            SetCacheDirty();
        }

        public void AddWaypoint(BezierPathWaypoint waypoint)
        {
            _waypoints.Add(waypoint);
            SetCacheDirty();
        }

        public void RemoveLastWaypoint()
        {
            if (_waypoints.Count == 0)
                return;

            _waypoints.RemoveAt(_waypoints.Count - 1);
            SetCacheDirty();
        }

        public float GetLevelNormalizedProgress(int levelIndex)
        {
            if (_levelNormalizedProgress != null && _levelNormalizedProgress.Count > 0)
            {
                if (levelIndex <= 0)
                    return Mathf.Clamp01(_levelNormalizedProgress[0]);

                if (levelIndex >= _levelNormalizedProgress.Count)
                    return Mathf.Clamp01(_levelNormalizedProgress[^1]);

                return Mathf.Clamp01(_levelNormalizedProgress[levelIndex]);
            }

            var count = Mathf.Max(1, _waypoints.Count);

            if (count <= 1)
                return 0f;

            var t = levelIndex / (float)(count - 1);
            return Mathf.Clamp01(t);
        }

        public int LevelProgressCount =>
            _levelNormalizedProgress != null && _levelNormalizedProgress.Count > 0
                ? _levelNormalizedProgress.Count
                : Mathf.Max(0, _waypoints.Count);

        public Vector2 EvaluateAtArcLengthNormalized(float u)
        {
            u = Mathf.Clamp01(u);
            RebuildCacheIfNeeded();

            if (_samplePoints == null || _samplePoints.Length == 0)
                return _waypoints.Count > 0 ? _waypoints[0].position : Vector2.zero;

            if (_totalArcLength <= 0f)
                return _samplePoints[0];

            if (_samplePoints.Length == 1)
                return _samplePoints[0];

            var targetDist = u * _totalArcLength;
            var acc = 0f;
            for (var i = 0; i < _arcLengths.Length; i++)
            {
                var next = acc + _arcLengths[i];
                if (targetDist <= next || i == _arcLengths.Length - 1)
                {
                    var local = targetDist - acc;
                    var segLen = Mathf.Max(1e-6f, _arcLengths[i]);
                    var alpha = Mathf.Clamp01(local / segLen);
                    return Vector2.Lerp(_samplePoints[i], _samplePoints[i + 1], alpha);
                }

                acc = next;
            }

            return _samplePoints[^1];
        }

        public Vector2 EvaluateAtBezierParameter(float globalT)
        {
            globalT = Mathf.Clamp01(globalT);
            var n = _waypoints.Count;
            if (n < 2)
                return n == 1 ? _waypoints[0].position : Vector2.zero;

            var segments = n - 1;
            var f = globalT * segments;
            var seg = Mathf.Min(segments - 1, Mathf.FloorToInt(f));
            var localT = f - seg;
            return EvaluateSegment(seg, localT);
        }

        public Vector2 EvaluateSegment(int segmentIndex, float t)
        {
            t = Mathf.Clamp01(t);
            var n = _waypoints.Count;
            if (n < 2 || segmentIndex < 0 || segmentIndex >= n - 1)
                return n > 0 ? _waypoints[Mathf.Clamp(segmentIndex, 0, n - 1)].position : Vector2.zero;

            var a = _waypoints[segmentIndex];
            var b = _waypoints[segmentIndex + 1];
            var p0 = a.position;
            var p1 = a.position + a.handleOut;
            var p2 = b.position + b.handleIn;
            var p3 = b.position;
            return BezierMath.Cubic(p0, p1, p2, p3, t);
        }

        private void RebuildCacheIfNeeded()
        {
            if (!_cacheDirty)
                return;

            RebuildArcLengthCache();
            _cacheDirty = false;
        }

        private void RebuildArcLengthCache()
        {
            var n = _waypoints.Count;
            if (n < 2)
            {
                _totalArcLength = 0f;
                _arcLengths = Array.Empty<float>();
                _samplePoints = n == 1 ? new[] { _waypoints[0].position } : Array.Empty<Vector2>();
                return;
            }

            var segments = n - 1;
            var samplesPerSeg = Mathf.Max(4, _samplesPerSegment);
            var totalSamples = segments * samplesPerSeg + 1;
            var points = new Vector2[totalSamples];
            var lengths = new float[totalSamples - 1];

            var write = 0;
            for (var s = 0; s < segments; s++)
            {
                for (var i = 0; i < samplesPerSeg; i++)
                {
                    var t = i / (float)samplesPerSeg;
                    points[write++] = EvaluateSegment(s, t);
                }
            }

            points[write] = EvaluateSegment(segments - 1, 1f);

            var total = 0f;
            for (var i = 0; i < lengths.Length; i++)
            {
                var d = Vector2.Distance(points[i], points[i + 1]);
                lengths[i] = d;
                total += d;
            }

            _samplePoints = points;
            _arcLengths = lengths;
            _totalArcLength = total;
        }
    }
}

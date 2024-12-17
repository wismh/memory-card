using UnityEngine;

namespace Project.UIMapPath
{
    public static class BezierMath
    {
        public static Vector2 Cubic(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            var u = 1f - t;
            var uu = u * u;
            var tt = t * t;
            return uu * u * p0
                   + 3f * uu * t * p1
                   + 3f * u * tt * p2
                   + t * tt * p3;
        }

        public static Vector2 CubicTangent(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            var u = 1f - t;
            return 3f * u * u * (p1 - p0)
                   + 6f * u * t * (p2 - p1)
                   + 3f * t * t * (p3 - p2);
        }

        public static float ApproximateCubicLength(
            Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int segments = 24)
        {
            if (segments < 2)
                segments = 2;

            var len = 0f;
            var prev = Cubic(p0, p1, p2, p3, 0f);
            for (var i = 1; i <= segments; i++)
            {
                var t = i / (float)segments;
                var cur = Cubic(p0, p1, p2, p3, t);
                len += Vector2.Distance(prev, cur);
                prev = cur;
            }

            return len;
        }
    }
}

using System;
using UnityEngine;

namespace Project.UIMapPath
{
    [Serializable]
    public class BezierPathWaypoint
    {
        [Tooltip("Anchored position for a UI child that matches this path's anchor setup (typically center).")]
        public Vector2 position;

        [Tooltip("Bezier control toward the previous point (relative to position).")]
        public Vector2 handleIn;

        [Tooltip("Bezier control toward the next point (relative to position).")]
        public Vector2 handleOut;
    }
}

using Project.UIMapPath;
using UnityEditor;
using UnityEngine;

namespace Project.UIMapPath.Editor
{
    public static class LevelMapPathGizmoDrawer
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Active)]
        private static void DrawGizmo(LevelMapPath path, GizmoType type)
        {
            var rt = path.Rect;
            if (rt == null || path.WaypointCount < 2)
                return;

            Gizmos.color = new Color(1f, 0.6f, 0.1f, 0.35f);

            Vector3 prev = rt.TransformPoint(ToLocal3(path.GetWaypoint(0).position));
            for (var s = 0; s < path.WaypointCount - 1; s++)
            {
                for (var k = 1; k <= 16; k++)
                {
                    var t = k / 16f;
                    var p = path.EvaluateSegment(s, t);
                    var world = rt.TransformPoint(ToLocal3(p));
                    Gizmos.DrawLine(prev, world);
                    prev = world;
                }
            }
        }

        private static Vector3 ToLocal3(Vector2 p) => new(p.x, p.y, 0f);
    }
}

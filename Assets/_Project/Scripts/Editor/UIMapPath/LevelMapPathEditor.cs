using Project.Features.UIMapPath;
using UnityEditor;
using UnityEngine;

namespace Project.Features.UIMapPath.Editor
{
    [CustomEditor(typeof(LevelMapPath))]
    public class LevelMapPathEditor : UnityEditor.Editor
    {
        private SerializedProperty _waypoints;
        private SerializedProperty _samplesPerSegment;
        private SerializedProperty _levelNormalizedProgress;

        private void OnEnable()
        {
            _waypoints = serializedObject.FindProperty("_waypoints");
            _samplesPerSegment = serializedObject.FindProperty("_samplesPerSegment");
            _levelNormalizedProgress = serializedObject.FindProperty("_levelNormalizedProgress");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_samplesPerSegment);
            EditorGUILayout.PropertyField(_waypoints, true);
            EditorGUILayout.PropertyField(_levelNormalizedProgress, true);

            EditorGUILayout.HelpBox(
                "Waypoint positions use the same anchoredPosition space as UI children of this RectTransform " +
                "(recommended: anchor middle-center for path and player). Segments are cubic Beziers: " +
                "P0 → (P0+handleOut) and (P3+handleIn) ← P3.",
                MessageType.Info);

            var path = (LevelMapPath)target;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add waypoint"))
            {
                Undo.RecordObject(path, "Add Waypoint");
                var last = path.WaypointCount > 0
                    ? path.GetWaypoint(path.WaypointCount - 1)
                    : new BezierPathWaypoint();
                path.AddWaypoint(new BezierPathWaypoint
                {
                    position = last.position + new Vector2(80f, 0f),
                    handleIn = -new Vector2(40f, 0f),
                    handleOut = new Vector2(40f, 0f)
                });
                EditorUtility.SetDirty(path);
            }

            if (GUILayout.Button("Remove last") && path.WaypointCount > 0)
            {
                Undo.RecordObject(path, "Remove Waypoint");
                path.RemoveLastWaypoint();
                EditorUtility.SetDirty(path);
            }

            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            var path = (LevelMapPath)target;
            var rt = path.Rect;
            if (rt == null || path.WaypointCount < 1)
                return;

            DrawPathPreview(path, rt);

            for (var i = 0; i < path.WaypointCount; i++)
                DrawWaypointHandles(path, rt, i);
        }

        private static void DrawPathPreview(LevelMapPath path, RectTransform rt)
        {
            Handles.color = new Color(1f, 0.85f, 0.2f, 0.9f);
            if (path.WaypointCount < 2)
                return;

            Vector3 prev = rt.TransformPoint(ToLocal3(path.GetWaypoint(0).position));
            for (var s = 0; s < path.WaypointCount - 1; s++)
            {
                for (var k = 1; k <= 24; k++)
                {
                    var t = k / 24f;
                    var p = path.EvaluateSegment(s, t);
                    var world = rt.TransformPoint(ToLocal3(p));
                    Handles.DrawLine(prev, world);
                    prev = world;
                }
            }
        }

        private void DrawWaypointHandles(LevelMapPath path, RectTransform rt, int index)
        {
            var w = path.GetWaypoint(index);
            var pos = w.position;
            var p0 = rt.TransformPoint(ToLocal3(pos));

            EditorGUI.BeginChangeCheck();
            var newP0 = Handles.PositionHandle(p0, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(path, "Move Waypoint");
                var local = rt.InverseTransformPoint(newP0);
                w.position = new Vector2(local.x, local.y);
                path.SetWaypoint(index, w);
                EditorUtility.SetDirty(path);
            }

            var pOut = rt.TransformPoint(ToLocal3(pos + w.handleOut));
            var pIn = rt.TransformPoint(ToLocal3(pos + w.handleIn));

            Handles.color = Color.cyan;
            Handles.DrawDottedLine(p0, pOut, 4f);
            Handles.DrawDottedLine(p0, pIn, 4f);

            EditorGUI.BeginChangeCheck();
            var newOut = Handles.PositionHandle(pOut, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(path, "Move Handle Out");
                var local = rt.InverseTransformPoint(newOut);
                w.handleOut = new Vector2(local.x, local.y) - w.position;
                path.SetWaypoint(index, w);
                EditorUtility.SetDirty(path);
            }

            EditorGUI.BeginChangeCheck();
            var newIn = Handles.PositionHandle(pIn, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(path, "Move Handle In");
                var local = rt.InverseTransformPoint(newIn);
                w.handleIn = new Vector2(local.x, local.y) - w.position;
                path.SetWaypoint(index, w);
                EditorUtility.SetDirty(path);
            }

            Handles.color = Color.white;
            Handles.Label(p0 + Vector3.up * 8f, $" [{index}]");
        }

        private static Vector3 ToLocal3(Vector2 p) => new(p.x, p.y, 0f);
    }
}

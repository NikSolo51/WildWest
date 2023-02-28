using CodeBase.Logic.Spawner;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(PuzzleMarker))]
    public class SpawnMarkerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(PuzzleMarker spawner, GizmoType gizmo)
        {
            Gizmos.color = new Color(0.2745098f,0.2039216f,0.3686275f);
            Gizmos.DrawSphere(spawner.transform.position, 0.5f);
        }
    }
}
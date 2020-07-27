using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Rendering;

namespace Pinwheel.Griffin
{
    [CustomEditor(typeof(GInteractiveGrassAgent))]
    public class GInteractiveGrassAgentInspector : Editor
    {
        private GInteractiveGrassAgent instance;
        private void OnEnable()
        {
            instance = target as GInteractiveGrassAgent;
        }

        public override void OnInspectorGUI()
        {
            instance.Radius = EditorGUILayout.FloatField("Radius", instance.Radius);
        }

        private void OnSceneGUI()
        {
            DrawRadius();
        }

        private void DrawRadius()
        {
            Handles.color = Color.yellow;
            CompareFunction zTest = Handles.zTest;
            Handles.zTest = CompareFunction.LessEqual;
            Handles.DrawWireDisc(instance.transform.position, Vector3.up, instance.Radius);
            Handles.zTest = zTest;
        }
    }
}

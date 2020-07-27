using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Pinwheel.Griffin
{
    [InitializeOnLoad]
    public class GTerrainDataDragAndDrop
    {
        static GTerrainDataDragAndDrop()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
#pragma warning disable 0618
            SceneView.onSceneGUIDelegate += OnSceneViewGUI;
#pragma warning restore 0618
        }

        private static void OnHierarchyGUI(int instanceID, Rect r)
        {
            Event e = Event.current;
            if (e == null)
                return;
            int controlId = EditorGUIUtility.GetControlID(FocusType.Passive);
            if (e.type == EventType.DragUpdated)
            {
                Object[] draggedObjects = DragAndDrop.objectReferences;
                if (draggedObjects.Length == 1 &&
                    draggedObjects[0] is GTerrainData)
                {
                    DragAndDrop.AcceptDrag();
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                    DragAndDrop.activeControlID = controlId;
                    e.Use();
                }
            }
            else if (e.type == EventType.DragPerform)
            {
                Object[] draggedObjects = DragAndDrop.objectReferences;
                if (draggedObjects.Length == 1 &&
                    draggedObjects[0] is GTerrainData)
                {
                    GTerrainData data = draggedObjects[0] as GTerrainData;
                    GCommon.CreateTerrain(data);
                    e.Use();
                }
            }
        }

        public static void OnSceneViewGUI(SceneView sv)
        {
            Event e = Event.current;
            if (e == null)
                return;
            int controlId = EditorGUIUtility.GetControlID(FocusType.Passive, sv.position);
            if (e.type == EventType.DragUpdated)
            {
                Object[] draggedObjects = DragAndDrop.objectReferences;
                if (draggedObjects.Length == 1 &&
                    draggedObjects[0] is GTerrainData)
                {
                    DragAndDrop.AcceptDrag();
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                    DragAndDrop.activeControlID = controlId;
                    e.Use();
                }
            }
            else if (e.type == EventType.DragPerform)
            {
                Object[] draggedObjects = DragAndDrop.objectReferences;
                if (draggedObjects.Length == 1 &&
                    draggedObjects[0] is GTerrainData)
                {
                    GTerrainData data = draggedObjects[0] as GTerrainData;
                    GCommon.CreateTerrain(data);
                    e.Use();
                }
            }
        }
    }
}

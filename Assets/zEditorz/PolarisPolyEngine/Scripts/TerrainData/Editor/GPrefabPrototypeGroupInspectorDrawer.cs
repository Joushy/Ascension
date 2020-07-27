using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Pinwheel.Griffin.PaintTool
{
    public class GPrefabPrototypeGroupInspectorDrawer
    {
        private GPrefabPrototypeGroup instance;
        private GPrefabPrototypeGroupInspectorDrawer(GPrefabPrototypeGroup instance)
        {
            this.instance = instance;
        }

        public static GPrefabPrototypeGroupInspectorDrawer Create(GPrefabPrototypeGroup instance)
        {
            return new GPrefabPrototypeGroupInspectorDrawer(instance);
        }

        public void DrawGUI()
        {
            EditorGUI.BeginChangeCheck();
            DrawPrototypesListGUI();
            DrawAddPrototypeGUI();
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(instance);
            }
        }

        private void DrawPrototypesListGUI()
        {
            instance.Prototypes.RemoveAll(g => g == null);
            CachePrefabPath();
            for (int i = 0; i < instance.Prototypes.Count; ++i)
            {
                GameObject g = instance.Prototypes[i];
                if (g == null)
                    continue;

                string label = g.name;
                string id = "treeprototype" + i + instance.GetInstanceID().ToString();

                int index = i;
                GenericMenu menu = new GenericMenu();
                menu.AddItem(
                    new GUIContent("Remove"),
                    false,
                    () => { ConfirmAndRemovePrototypeAtIndex(index); });

                GEditorCommon.Foldout(label, false, id, () =>
                {
                    DrawPreview(g);
                }, menu);
            }
        }

        private void CachePrefabPath()
        {
            instance.Editor_PrefabAssetPaths.Clear();
            for (int i = 0; i < instance.Prototypes.Count; ++i)
            {
                if (instance.Prototypes[i] == null)
                {
                    instance.Editor_PrefabAssetPaths[i] = string.Empty;
                }
                else
                {
                    instance.Editor_PrefabAssetPaths[i] = AssetDatabase.GetAssetPath(instance.Prototypes[i]);
                }
            }
        }

        private void ConfirmAndRemovePrototypeAtIndex(int index)
        {
            GameObject g = instance.Prototypes[index];
            string label = g.name;
            if (EditorUtility.DisplayDialog(
                "Confirm",
                "Remove " + label,
                "OK", "Cancel"))
            {
                instance.Prototypes.RemoveAt(index);
            }
        }

        private void DrawPreview(GameObject g)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(GEditorCommon.selectionGridTileSizeMedium.y));
            GEditorCommon.DrawPreview(r, g);
        }

        private void DrawAddPrototypeGUI()
        {
            EditorGUILayout.GetControlRect(GUILayout.Height(1));
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(GEditorCommon.objectSelectorDragDropHeight));
            GameObject g = GEditorCommon.ObjectSelectorDragDrop<GameObject>(r, "Drop a Game Object here!", "t:GameObject");
            if (g != null)
            {
                if (!instance.Prototypes.Contains(g))
                {
                    instance.Prototypes.Add(g);
                }
            }
        }
    }
}

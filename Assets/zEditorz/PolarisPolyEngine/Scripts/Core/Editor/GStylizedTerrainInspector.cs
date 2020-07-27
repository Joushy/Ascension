using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

namespace Pinwheel.Griffin
{
    [CustomEditor(typeof(GStylizedTerrain))]
    public class GStylizedTerrainInspector : Editor
    {
        private GStylizedTerrain instance;
        private bool isNeighboringFoldoutExpanded;
        private List<GGenericMenuItem> geometryAdditionalContextAction;
        private List<GGenericMenuItem> foliageAdditionalContextAction;

        private void OnEnable()
        {
            instance = (GStylizedTerrain)target;
            if (instance.TerrainData != null)
                instance.TerrainData.Shading.UpdateMaterials();

            geometryAdditionalContextAction = new List<GGenericMenuItem>();
            geometryAdditionalContextAction.Add(new GGenericMenuItem(
                "Add Height Map Filter",
                false,
                () =>
                {
                    GHeightMapFilter filterComponent = instance.GetComponent<GHeightMapFilter>();
                    if (filterComponent == null)
                    {
                        instance.gameObject.AddComponent<GHeightMapFilter>();
                    }
                }));

            foliageAdditionalContextAction = new List<GGenericMenuItem>();
            foliageAdditionalContextAction.Add(new GGenericMenuItem(
                "Update Trees",
                false,
                () =>
                {
                    if (instance.TerrainData != null)
                    {
                        instance.TerrainData.Foliage.SetTreeRegionDirty(new Rect(0, 0, 1, 1));
                        instance.UpdateTreesPosition(true);
                        instance.TerrainData.Foliage.ClearTreeDirtyRegions();
                        instance.TerrainData.SetDirty(GTerrainData.DirtyFlags.Foliage);
                    }
                }));
            foliageAdditionalContextAction.Add(new GGenericMenuItem(
                "Update Grasses",
                false,
                () =>
                {
                    if (instance.TerrainData != null)
                    {
                        instance.TerrainData.Foliage.SetGrassRegionDirty(new Rect(0, 0, 1, 1));
                        instance.UpdateGrassPatches(-1, true);
                        instance.TerrainData.Foliage.ClearGrassDirtyRegions();
                        instance.TerrainData.SetDirty(GTerrainData.DirtyFlags.Foliage);
                    }
                }));
        }

        public override void OnInspectorGUI()
        {
            if (instance.TerrainData == null)
            {
                DrawNullTerrainDataGUI();
            }
            else
            {
                DrawBasicGUI();
            }
        }

        private void DrawNullTerrainDataGUI()
        {
            instance.TerrainData = EditorGUILayout.ObjectField("Terrain Data", instance.TerrainData, typeof(GTerrainData), false) as GTerrainData;
        }

        private void DrawBasicGUI()
        {
            instance.TerrainData = EditorGUILayout.ObjectField("Terrain Data", instance.TerrainData, typeof(GTerrainData), false) as GTerrainData;
            GTerrainDataInspectorDrawer drawer = GTerrainDataInspectorDrawer.Create(instance.TerrainData, instance);
            drawer.SetGeometryAdditionalContextAction(geometryAdditionalContextAction);
            drawer.SetFoliageAdditionalContextAction(foliageAdditionalContextAction);
            drawer.DrawGUI();
            DrawNeighboringGUI();
        }

        private void DrawNeighboringGUI()
        {
            string label = "Neighboring";
            string id = "neighboring" + instance.GetInstanceID().ToString();

            GenericMenu menu = new GenericMenu();
            menu.AddItem(
                new GUIContent("Reset"),
                false,
                () => { ResetNeighboring(); });
            menu.AddItem(
                new GUIContent("Connect"),
                false,
                () => { GStylizedTerrain.ConnectAdjacentTiles(); });

            isNeighboringFoldoutExpanded = GEditorCommon.Foldout(label, false, id, () =>
             {
                 EditorGUI.BeginChangeCheck();
                 instance.AutoConnect = EditorGUILayout.Toggle("Auto Connect", instance.AutoConnect);
                 instance.GroupId = EditorGUILayout.DelayedIntField("Group Id", instance.GroupId);
                 instance.TopNeighbor = EditorGUILayout.ObjectField("Top Neighbor", instance.TopNeighbor, typeof(GStylizedTerrain), true) as GStylizedTerrain;
                 instance.BottomNeighbor = EditorGUILayout.ObjectField("Bottom Neighbor", instance.BottomNeighbor, typeof(GStylizedTerrain), true) as GStylizedTerrain;
                 instance.LeftNeighbor = EditorGUILayout.ObjectField("Left Neighbor", instance.LeftNeighbor, typeof(GStylizedTerrain), true) as GStylizedTerrain;
                 instance.RightNeighbor = EditorGUILayout.ObjectField("Right Neighbor", instance.RightNeighbor, typeof(GStylizedTerrain), true) as GStylizedTerrain;

                 if (EditorGUI.EndChangeCheck())
                 {
                     if (instance.TopNeighbor != null || instance.BottomNeighbor != null || instance.LeftNeighbor != null || instance.RightNeighbor != null)
                     {
                         GAnalytics.Record(GAnalytics.MULTI_TERRAIN, true);
                     }
                 }
             }, menu);
        }

        public static string GetNeighboringFoldoutID(GStylizedTerrain t)
        {
            string id = "neighboring" + t.GetInstanceID().ToString();
            return id;
        }

        public override bool RequiresConstantRepaint()
        {
            return false;
        }

        private void OnSceneGUI()
        {
            if (instance.TerrainData == null)
                return;
            if (!instance.AutoConnect)
                return;
            if (!isNeighboringFoldoutExpanded)
                return;

            Vector3 terrainSizeXZ = new Vector3(
                instance.TerrainData.Geometry.Width,
                0,
                instance.TerrainData.Geometry.Length);

            if (instance.LeftNeighbor == null)
            {
                Vector3 pos = instance.transform.position + Vector3.left * terrainSizeXZ.x + terrainSizeXZ * 0.5f;
                if (Handles.Button(pos, Quaternion.Euler(90, 0, 0), terrainSizeXZ.z * 0.5f, terrainSizeXZ.z * 0.5f, Handles.RectangleHandleCap) && Event.current.button == 0)
                {
                    GStylizedTerrain t = CreateNeighborTerrain();
                    t.transform.parent = instance.transform.parent;
                    t.transform.position = instance.transform.position + Vector3.left * terrainSizeXZ.x;
                    t.name = string.Format("{0}-{1}", t.name, t.transform.position.ToString());
                    Selection.activeGameObject = t.gameObject;
                    GStylizedTerrain.ConnectAdjacentTiles();
                }
            }
            if (instance.TopNeighbor == null)
            {
                Vector3 pos = instance.transform.position + Vector3.forward * terrainSizeXZ.z + terrainSizeXZ * 0.5f;
                if (Handles.Button(pos, Quaternion.Euler(90, 0, 0), terrainSizeXZ.z * 0.5f, terrainSizeXZ.z * 0.5f, Handles.RectangleHandleCap) && Event.current.button == 0)
                {
                    GStylizedTerrain t = CreateNeighborTerrain();
                    t.transform.parent = instance.transform.parent;
                    t.transform.position = instance.transform.position + Vector3.forward * terrainSizeXZ.z;
                    t.name = string.Format("{0}-{1}", t.name, t.transform.position.ToString());
                    Selection.activeGameObject = t.gameObject;
                    GStylizedTerrain.ConnectAdjacentTiles();
                }
            }
            if (instance.RightNeighbor == null)
            {
                Vector3 pos = instance.transform.position + Vector3.right * terrainSizeXZ.z + terrainSizeXZ * 0.5f;
                if (Handles.Button(pos, Quaternion.Euler(90, 0, 0), terrainSizeXZ.z * 0.5f, terrainSizeXZ.z * 0.5f, Handles.RectangleHandleCap) && Event.current.button == 0)
                {
                    GStylizedTerrain t = CreateNeighborTerrain();
                    t.transform.parent = instance.transform.parent;
                    t.transform.position = instance.transform.position + Vector3.right * terrainSizeXZ.x;
                    t.name = string.Format("{0}-{1}", t.name, t.transform.position.ToString());
                    Selection.activeGameObject = t.gameObject;
                    GStylizedTerrain.ConnectAdjacentTiles();
                }
            }
            if (instance.BottomNeighbor == null)
            {
                Vector3 pos = instance.transform.position + Vector3.back * terrainSizeXZ.z + terrainSizeXZ * 0.5f;
                if (Handles.Button(pos, Quaternion.Euler(90, 0, 0), terrainSizeXZ.z * 0.5f, terrainSizeXZ.z * 0.5f, Handles.RectangleHandleCap) && Event.current.button == 0)
                {
                    GStylizedTerrain t = CreateNeighborTerrain();
                    t.transform.parent = instance.transform.parent;
                    t.transform.position = instance.transform.position + Vector3.back * terrainSizeXZ.z;
                    t.name = string.Format("{0}-{1}", t.name, t.transform.position.ToString());
                    Selection.activeGameObject = t.gameObject;
                    GStylizedTerrain.ConnectAdjacentTiles();
                }
            }
        }

        private GStylizedTerrain CreateNeighborTerrain()
        {
            GStylizedTerrain t = GEditorMenus.CreateStylizedTerrain(instance.TerrainData);
            GEditorCommon.ExpandFoldout(GetNeighboringFoldoutID(t));
            if (instance.TerrainData != null && instance.TerrainData.Shading.CustomMaterial != null)
            {
                t.TerrainData.Shading.CustomMaterial.shader = instance.TerrainData.Shading.CustomMaterial.shader;
                t.TerrainData.Shading.UpdateMaterials();
            }

            return t;
        }

        private void ResetNeighboring()
        {
            instance.AutoConnect = true;
            instance.GroupId = 0;
            instance.TopNeighbor = null;
            instance.BottomNeighbor = null;
            instance.LeftNeighbor = null;
            instance.RightNeighbor = null;
        }
    }
}

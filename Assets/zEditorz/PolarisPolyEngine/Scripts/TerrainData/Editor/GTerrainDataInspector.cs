using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Pinwheel.Griffin
{
    [CustomEditor(typeof(GTerrainData))]
    public class GTerrainDataInspector : Editor
    {
        private GTerrainData instance;

        private void OnEnable()
        {
            instance = (GTerrainData)target;
        }

        public override void OnInspectorGUI()
        {
            GTerrainDataInspectorDrawer.Create(instance).DrawGUI();
        }

        [MenuItem("CONTEXT/GTerrainData/Reset Geometry")]
        private static void ResetGeometry()
        {
            Object o = Selection.activeObject;
            if (o is GTerrainData)
            {
                GTerrainData data = o as GTerrainData;
                data.Geometry.ResetFull();
                AssetDatabase.Refresh();
            }
        }

        [MenuItem("CONTEXT/GTerrainData/Reset Shading")]
        private static void ResetShading()
        {
            Object o = Selection.activeObject;
            if (o is GTerrainData)
            {
                GTerrainData data = o as GTerrainData;
                data.Shading.ResetFull();
                AssetDatabase.Refresh();
            }
        }

        [MenuItem("CONTEXT/GTerrainData/Reset Rendering")]
        private static void ResetRendering()
        {
            Object o = Selection.activeObject;
            if (o is GTerrainData)
            {
                GTerrainData data = o as GTerrainData;
                data.Rendering.ResetFull();
                AssetDatabase.Refresh();
            }
        }

        [MenuItem("CONTEXT/GTerrainData/Reset Foliage")]
        private static void ResetFoliage()
        {
            Object o = Selection.activeObject;
            if (o is GTerrainData)
            {
                GTerrainData data = o as GTerrainData;
                data.Foliage.ResetFull();
                AssetDatabase.Refresh();
            }
        }

        [MenuItem("CONTEXT/GTerrainData/Flush Unused Data", priority = 10000)]
        private static void FlushUnusedData()
        {
            Object o = Selection.activeObject;
            if (o is GTerrainData)
            {
                GTerrainData data = o as GTerrainData;
                string path = AssetDatabase.GetAssetPath(data);
                Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
                for (int i = 0; i < subAssets.Length; ++i)
                {
                    bool willDestroy = false;
                    Object a = subAssets[i];
                    if (a.name.Equals(GGeometry.HEIGHT_MAP_NAME) &&
                        a.GetInstanceID() != data.Geometry.Internal_HeightMap.GetInstanceID())
                    {
                        willDestroy = true;
                    }
                    if (a.name.Equals(GShading.ALBEDO_MAP_NAME) &&
                        a.GetInstanceID() != data.Shading.Internal_AlbedoMap.GetInstanceID())
                    {
                        willDestroy = true;
                    }
                    if (a.name.Equals(GShading.METALLIC_MAP_NAME) &&
                        a.GetInstanceID() != data.Shading.Internal_MetallicMap.GetInstanceID())
                    {
                        willDestroy = true;
                    }
                    if (a.name.Equals(GShading.COLOR_BY_HEIGHT_MAP_NAME) &&
                        a.GetInstanceID() != data.Shading.Internal_ColorByHeightMap.GetInstanceID())
                    {
                        willDestroy = true;
                    }
                    if (a.name.Equals(GShading.COLOR_BY_NORMAL_MAP_NAME) &&
                        a.GetInstanceID() != data.Shading.Internal_ColorByNormalMap.GetInstanceID())
                    {
                        willDestroy = true;
                    }
                    if (a.name.Equals(GShading.COLOR_BLEND_MAP_NAME) &&
                        a.GetInstanceID() != data.Shading.Internal_ColorBlendMap.GetInstanceID())
                    {
                        willDestroy = true;
                    }
                    if (a.name.StartsWith(GShading.SPLAT_CONTROL_MAP_NAME))
                    {
                        willDestroy = true;
                        for (int controlIndex = 0; controlIndex < data.Shading.Internal_SplatControls.Length; ++controlIndex)
                        {
                            if (a.GetInstanceID() == data.Shading.Internal_SplatControls[controlIndex].GetInstanceID())
                            {
                                willDestroy = false;
                            }
                        }
                    }
                    if (willDestroy)
                    {
                        Object.DestroyImmediate(a, true);
                    }
                }

                AssetDatabase.Refresh();
            }
        }

        //[MenuItem("CONTEXT/GTerrainData/Fix Hide Flag")]
        //private static void FixHideFlag()
        //{
        //    Object o = Selection.activeObject;
        //    if (o is GTerrainData)
        //    {
        //        GTerrainData data = o as GTerrainData;
        //        data.Geometry.hideFlags = HideFlags.None;
        //        data.Shading.hideFlags = HideFlags.None; 
        //        data.Rendering.hideFlags = HideFlags.None;
        //        data.Foliage.hideFlags = HideFlags.None;
        //        EditorUtility.SetDirty(data);
        //        AssetDatabase.Refresh();
        //    }
        //}
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Pinwheel.Griffin.DataTool
{
    public class GRawExporter
    {
        public GTerrainData SrcData { get; set; }
        public GBitDepth BitDepth { get; set; }
        public string DataDirectory { get; set; }

        public void Export()
        {
            try
            {
#if UNITY_EDITOR
                GCommonGUI.ProgressBar("Working", "Exporting RAW file...", 1f);
#endif
                if (BitDepth == GBitDepth.Bit8)
                {
                    DoExportRaw8();
                }
                else if (BitDepth == GBitDepth.Bit16)
                {
                    DoExportRaw16();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
#if UNITY_EDITOR
                GCommonGUI.ClearProgressBar();
#endif
            }
        }

        private void DoExportRaw8()
        {
            int rawResolution = SrcData.Geometry.HeightMapResolution + 1;
            byte[] data = new byte[rawResolution * rawResolution];

            Vector2 uv = Vector2.zero;
            Color c = Color.white;
            byte h = 0;
            for (int z = 0; z < rawResolution; ++z)
            {
                for (int x = 0; x < rawResolution; ++x)
                {
                    uv.Set(
                        Mathf.InverseLerp(0, rawResolution - 1, x),
                        1 - Mathf.InverseLerp(0, rawResolution - 1, z));
                    c = SrcData.Geometry.Internal_HeightMap.GetPixelBilinear(uv.x, uv.y);
                    h = (byte)(c.r * byte.MaxValue);
                    data[GUtilities.To1DIndex(x, z, rawResolution)] = h;
                }
            }

            GUtilities.EnsureDirectoryExists(DataDirectory);
            string fileName = string.Format("RAW8_{0}x{0}_{1}.raw", rawResolution, SrcData.Id);
            string path = Path.Combine(DataDirectory, fileName);
            File.WriteAllBytes(path, data);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        private void DoExportRaw16()
        {
            int rawResolution = SrcData.Geometry.HeightMapResolution + 1;
            byte[] data = new byte[rawResolution * rawResolution * sizeof(UInt16)];

            Vector2 uv = Vector2.zero;
            Color c = Color.white;
            UInt16 h = 0;
            int startIndex = 0;
            for (int z = 0; z < rawResolution; ++z)
            {
                for (int x = 0; x < rawResolution; ++x)
                {
                    uv.Set(
                        Mathf.InverseLerp(0, rawResolution - 1, x),
                        1 - Mathf.InverseLerp(0, rawResolution - 1, z));
                    c = SrcData.Geometry.Internal_HeightMap.GetPixelBilinear(uv.x, uv.y);
                    h = (UInt16)(c.r * UInt16.MaxValue);
                    startIndex = 2 * GUtilities.To1DIndex(x, z, rawResolution);
                    Array.Copy(BitConverter.GetBytes(h), 0, data, startIndex, sizeof(UInt16));
                }
            }

            GUtilities.EnsureDirectoryExists(DataDirectory);
            string fileName = string.Format("RAW16_{0}x{0}_{1}.r16", rawResolution, SrcData.Id);
            string path = Path.Combine(DataDirectory, fileName);
            File.WriteAllBytes(path, data);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }
}

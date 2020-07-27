using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pinwheel.Griffin.TextureTool;

namespace Pinwheel.Griffin.TextureTool
{
    public class GHeightMapGenerator : IGTextureGenerator
    {
        public void Generate(RenderTexture targetRt)
        {
            GHeightMapGeneratorParams param = GTextureToolParams.Instance.HeightMap;
            if (param.Terrain == null || param.Terrain.TerrainData == null)
            {
                GCommon.CopyToRT(Texture2D.blackTexture, targetRt);
            }
            else
            {
                if (param.UseRealGeometry)
                {
                    RenderGeometryHeightMap(param.Terrain, targetRt);
                }
                else
                {
                    RenderPixelHeightMap(param.Terrain, targetRt);
                }
            }
        }

        private void RenderGeometryHeightMap(GStylizedTerrain terrain, RenderTexture targetRt)
        {
            GTerrainChunk[] chunks = terrain.Internal_GetChunks();
            RenderTexture.active = targetRt;
            GL.PushMatrix();
            GL.LoadOrtho();
            Material mat = GInternalMaterials.GeometricalHeightMapMaterial;
            mat.SetPass(0);
            GL.Begin(GL.TRIANGLES);

            for (int i = 0; i < chunks.Length; ++i)
            {
                Mesh m = chunks[i].MeshFilterComponent.sharedMesh;
                if (m == null)
                    continue;
                Vector2[] uvs = m.uv;

                for (int j = 0; j < uvs.Length; ++j)
                {
                    GL.Vertex3(uvs[j].x, uvs[j].y, terrain.GetInterpolatedHeightMapSample(uvs[j]).x);
                }
            }

            GL.End();
            GL.PopMatrix();
            RenderTexture.active = null;
        }

        private void RenderPixelHeightMap(GStylizedTerrain terrain, RenderTexture targetRt)
        {
            Texture2D source = terrain.TerrainData.Geometry.Internal_HeightMap;
            RenderTexture.active = targetRt;
            Material mat = GInternalMaterials.ChannelToGrayscaleMaterial;
            mat.SetTexture("_MainTex", source);
            mat.SetInt("_ChannelIndex", 0);
            Graphics.Blit(source, mat);
            RenderTexture.active = null;
        }
    }
}

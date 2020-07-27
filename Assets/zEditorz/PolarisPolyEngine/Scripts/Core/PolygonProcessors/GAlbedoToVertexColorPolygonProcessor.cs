using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pinwheel.Griffin;

namespace Pinwheel.Griffin
{
    [GDisplayName("Albedo To Vertex Color")]
    public class GAlbedoToVertexColorPolygonProcessor : IGPolygonProcessor
    {
        public void Process(GTerrainChunk chunk, ref GPolygon p)
        {
            Texture2D albedo = chunk.Terrain.TerrainData.Shading.Internal_AlbedoMap;
            Color c0 = albedo.GetPixelBilinear(p.Uvs[0].x, p.Uvs[0].y);
            Color c1 = albedo.GetPixelBilinear(p.Uvs[1].x, p.Uvs[1].y);
            Color c2 = albedo.GetPixelBilinear(p.Uvs[2].x, p.Uvs[2].y);

            p.VertexColors.Clear();
            p.VertexColors.Add(c0);
            p.VertexColors.Add(c1);
            p.VertexColors.Add(c2);
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pinwheel.Griffin
{
    public class GPolygon
    {
        private List<Vector3> vertices;
        public List<Vector3> Vertices
        {
            get
            {
                if (vertices == null)
                {
                    vertices = new List<Vector3>(3);
                }
                return vertices;
            }
        }

        private List<Vector2> uvs;
        public List<Vector2> Uvs
        {
            get
            {
                if (uvs == null)
                {
                    uvs = new List<Vector2>(3);
                }
                return uvs;
            }
        }

        private List<Color> vertexColors;
        public List<Color> VertexColors
        {
            get
            {
                if (vertexColors == null)
                {
                    vertexColors = new List<Color>(3);
                }
                return vertexColors;
            }
        }

        private List<int> triangles;
        public List<int> Triangles
        {
            get
            {
                if (triangles == null)
                {
                    triangles = new List<int>(3);
                }
                return triangles;
            }
        }

        public void Clear()
        {
            Vertices.Clear();
            Uvs.Clear();
            VertexColors.Clear();
            Triangles.Clear();
        }
    }
}

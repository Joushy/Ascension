using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pinwheel.Griffin
{
    public interface IGPolygonProcessor
    {
        void Process(GTerrainChunk chunk, ref GPolygon p);
    }
}

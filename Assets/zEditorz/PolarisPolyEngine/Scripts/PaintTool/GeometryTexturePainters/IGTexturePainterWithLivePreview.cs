using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pinwheel.Griffin.PaintTool
{
    public interface IGTexturePainterWithLivePreview
    {
        void Editor_DrawLivePreview(GStylizedTerrain terrain, GTexturePainterArgs args, Camera cam);
    }
}

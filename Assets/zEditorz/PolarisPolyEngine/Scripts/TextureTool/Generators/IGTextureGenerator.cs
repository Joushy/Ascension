using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pinwheel.Griffin.TextureTool
{
    public interface IGTextureGenerator
    {
        void Generate(RenderTexture targetRt);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pinwheel.Griffin.TextureTool
{
    public interface IGTextureFilter
    {
        void Apply(RenderTexture targetRt, GTextureFilterParams param);
    }
}

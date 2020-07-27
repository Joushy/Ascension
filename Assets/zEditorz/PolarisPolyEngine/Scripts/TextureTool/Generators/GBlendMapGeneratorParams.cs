using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pinwheel.Griffin.TextureTool
{
    [System.Serializable]
    public struct GBlendMapGeneratorParams
    {
        [SerializeField]
        private List<GBlendLayer> layers;
        public List<GBlendLayer> Layers
        {
            get
            {
                if (layers==null)
                {
                    layers = new List<GBlendLayer>();
                }
                return layers;
            }
            set
            {
                layers = value;
            }
        }
    }
}

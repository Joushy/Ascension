using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pinwheel.Griffin.PaintTool
{
    [System.Serializable]
    [AddComponentMenu("")]
    public abstract class GSpawnFilter : MonoBehaviour
    {
        [SerializeField]
        private bool ignore;
        public bool Ignore
        {
            get
            {
                return ignore;
            }
            set
            {
                ignore = value;
            }
        }

        public abstract void Apply(ref GSpawnFilterArgs args);
    }
}

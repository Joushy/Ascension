using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Pinwheel.Griffin.HelpTool
{
    [CustomEditor(typeof(GHelpComponent))]
    public class GHelpComponentInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            GHelpToolDrawer.DrawGUI();
        }
    }
}

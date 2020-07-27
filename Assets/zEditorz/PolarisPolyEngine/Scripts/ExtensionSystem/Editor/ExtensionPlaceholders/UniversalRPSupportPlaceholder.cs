#if GRIFFIN && UNITY_EDITOR && !GRIFFIN_URP
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Pinwheel.Griffin.GriffinExtension
{
    public static class UniversalRPSupportPlaceholder
    {
        public static string GetExtensionName()
        {
            return "Polaris V2 - Universal Render Pipeline Support";
        }

        public static string GetPublisherName()
        {
            return "Pinwheel Studio";
        }

        public static string GetDescription()
        {
            return "Adding support for URP.\n" +
                "Requires Unity 2019.3 or above.";
        }

        public static string GetVersion()
        {
            return "v1.0.0";
        }

        public static void OpenSupportLink()
        {
            GEditorCommon.OpenEmailEditor(
                GCommon.SUPPORT_EMAIL,
                "[Polaris V2] URP Support",
                "YOUR_MESSAGE_HERE");
        }

        public static void Button_Download()
        {
            string url = "https://assetstore.unity.com/packages/slug/157785";
            Application.OpenURL(url);
        }
    }
}
#endif

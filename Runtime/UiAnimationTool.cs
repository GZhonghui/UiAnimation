#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;
using UnityEditor;

namespace UiAnimation
{
    public static class UiAnimationTool
    {
        public static void Export(string timelineAssetGuid)
        {
            var timelineAsset = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(timelineAssetGuid));
            if (timelineAsset == null) return;
        }
    }
}

#endif

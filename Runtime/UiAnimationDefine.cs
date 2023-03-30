using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    public class UiAnimationDefine
    {
        public static bool useNativeTimelineAsset { get => true; }

        public static bool enableLua { get => true; }

        public static string name
        {
            get => "UiAnimation";
        }

        public static string extensionName
        {
            get => "uianimation";
        }

        public static string luaExportName
        {
            get => "ExportAnims";
        }

        public static double timelineEps
        {
            get => 1e-2;
        }

        public static string assetPath
        {
            get => System.IO.Path.Combine("Assets", "Ui", "Animation");
        }

        public enum AnimationType
        {
            None = 0,

            RectTransformAnchoredPosition = 1,
            RectTransformSizeDelta = 2,
            RectTransformLocalRotation = 3,
            RectTransformLocalScale = 4,

            UguiImageColor = 101,

            UguiTmpTextFontSize = 201,
            UguiTmpTextColor = 202,
        }
    }
}

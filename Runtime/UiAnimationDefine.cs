using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UiAnimation
{
    public class UiAnimationDefine
    {
        public static string name
        {
            get
            {
                return "UiAnimation";
            }
        }

        public static double timelineEps
        {
            get => 1e-2;
        }

        public static string assetPath
        {
            get
            {
                return System.IO.Path.Combine("Assets", "Ui", "Animation");
            }
        }

        public enum TrackType
        {
            None = 0,

            RectTransformAnchoredPosition = 1,
            RectTransformSizeDelta = 2,
            RectTransformLocalRotation = 3,

            UguiImageColor = 101,

            UguiTmpTextFontSize = 201,
        }
    }
}

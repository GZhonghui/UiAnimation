using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    public class UiAnimationMixerRectTransformAnchoredPosition : UiAnimationMixerBaseVector2
    {
        protected override void ApplyValue(object playerData)
        {
            base.ApplyValue(playerData);

            var rectTransform = playerData as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = m_FinalValue;
            }
        }
    }
}

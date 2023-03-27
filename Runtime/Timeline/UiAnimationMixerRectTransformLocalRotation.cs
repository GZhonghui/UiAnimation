using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    public class UiAnimationMixerRectTransformLocalRotation : UiAnimationMixerBaseFloat
    {
        protected override void ApplyValue(object playerData)
        {
            base.ApplyValue(playerData);

            var rectTransform = playerData as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.localEulerAngles = new Vector3(
                    rectTransform.localEulerAngles.x,
                    rectTransform.localEulerAngles.y,
                    m_FinalValue
                );
            }
        }
    }
}

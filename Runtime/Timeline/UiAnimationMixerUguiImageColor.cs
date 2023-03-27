using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine;

namespace UiAnimation
{
    public class UiAnimationMixerUguiImageColor : UiAnimationMixerBaseVector4
    {
        protected override void ApplyValue(object playerData)
        {
            base.ApplyValue(playerData);

            var image = playerData as Image;
            if (image != null)
            {
                image.color = new Color(
                    m_FinalValue.x, m_FinalValue.y, m_FinalValue.z, m_FinalValue.w
                );
            }
        }
    }
}

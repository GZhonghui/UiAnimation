using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace UiAnimation
{
    public class UiAnimationMixerUguiTmpTextColor : UiAnimationMixerBaseVector4
    {
        protected override void ApplyValue(object playerData)
        {
            base.ApplyValue(playerData);

            var text = playerData as TextMeshProUGUI;
            if (text != null)
            {
                text.color = new Color(
                    m_FinalValue.x, m_FinalValue.y, m_FinalValue.z, m_FinalValue.w
                );
            }
        }
    }
}

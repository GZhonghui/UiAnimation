using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace UiAnimation
{
    public class UiAnimationMixerUguiTmpTextFontSize : UiAnimationMixerBaseFloat
    {
        protected override void ApplyValue(object playerData)
        {
            base.ApplyValue(playerData);

            var text = playerData as TextMeshProUGUI;
            if (text != null)
            {
                text.fontSize = m_FinalValue;
            }
        }
    }
}

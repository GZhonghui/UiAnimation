using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    public class UiAnimationBehaviourBase : PlayableBehaviour
    {
        public UiAnimationStatus m_EndStatus;
        public AnimationCurve m_Curve;
        public double m_Start;
        public double m_End;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
        }
    }
}
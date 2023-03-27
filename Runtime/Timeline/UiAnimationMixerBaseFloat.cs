using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    public class UiAnimationMixerBaseFloat : UiAnimationMixerBase
    {
        protected float m_InitValue;
        protected float m_FinalValue;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            // From Track
            m_InitValue = m_InitStatus.m_UniformValue.x;

            // From Clips
            var lastBehaviour = FindLastFinishedClip(playable);
            if (lastBehaviour != null)
            {
                m_InitValue = lastBehaviour.m_EndStatus.m_UniformValue.x;
            }

            m_FinalValue = m_InitValue;

            // Calc Final Value
            IterateInput(playable);

            // Apply Final Value
            ApplyValue(playerData);
        }

        protected override void ProcessInput(UiAnimationBehaviourBase behaviourBase, float progress)
        {
            base.ProcessInput(behaviourBase, progress);

            var endValue = behaviourBase.m_EndStatus.m_UniformValue.x;

            m_FinalValue = Mathf.LerpUnclamped(m_InitValue, endValue, (float)progress);
        }

        protected override void ApplyValue(object playerData)
        {
            base.ApplyValue(playerData);
        }
    }
}

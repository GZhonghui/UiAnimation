using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    public class UiAnimationMixerBaseVector2 : UiAnimationMixerBase
    {
        protected Vector2 m_InitValue;
        protected Vector2 m_FinalValue;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            // From Track
            m_InitValue = new Vector2(
                m_InitStatus.m_UniformValue.x,
                m_InitStatus.m_UniformValue.y
            );

            // From Clips
            var lastBehaviour = FindLastFinishedClip(playable);
            if (lastBehaviour != null)
            {
                m_InitValue = new Vector2(
                    lastBehaviour.m_EndStatus.m_UniformValue.x,
                    lastBehaviour.m_EndStatus.m_UniformValue.y
                );
            }

            m_FinalValue = new Vector2(m_InitValue.x, m_InitValue.y);

            // Calc Final Value
            IterateInput(playable);

            // Apply Final Value
            ApplyValue(playerData);
        }

        protected override void ProcessInput(UiAnimationBehaviourBase behaviourBase, float progress)
        {
            base.ProcessInput(behaviourBase, progress);

            var endValue = new Vector2(
                behaviourBase.m_EndStatus.m_UniformValue.x,
                behaviourBase.m_EndStatus.m_UniformValue.y
            );

            m_FinalValue = Vector2.LerpUnclamped(m_InitValue, endValue, progress);
        }

        protected override void ApplyValue(object playerData)
        {
            base.ApplyValue(playerData);
        }
    }
}

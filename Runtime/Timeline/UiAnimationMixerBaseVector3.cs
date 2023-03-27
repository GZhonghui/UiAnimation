using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    public class UiAnimationMixerBaseVector3 : UiAnimationMixerBase
    {
        protected Vector3 m_InitValue;
        protected Vector3 m_FinalValue;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            // From Track
            m_InitValue = new Vector3(
                m_InitStatus.m_UniformValue.x,
                m_InitStatus.m_UniformValue.y,
                m_InitStatus.m_UniformValue.z
            );

            // From Clips
            var lastBehaviour = FindLastFinishedClip(playable);
            if (lastBehaviour != null)
            {
                m_InitValue = new Vector3(
                    lastBehaviour.m_EndStatus.m_UniformValue.x,
                    lastBehaviour.m_EndStatus.m_UniformValue.y,
                    lastBehaviour.m_EndStatus.m_UniformValue.z
                );
            }

            m_FinalValue = new Vector3(m_InitValue.x, m_InitValue.y, m_InitValue.z);

            // Calc Final Value
            IterateInput(playable);

            // Apply Final Value
            ApplyValue(playerData);
        }

        protected override void ProcessInput(UiAnimationBehaviourBase behaviourBase, float progress)
        {
            base.ProcessInput(behaviourBase, progress);

            var endValue = new Vector3(
                behaviourBase.m_EndStatus.m_UniformValue.x,
                behaviourBase.m_EndStatus.m_UniformValue.y,
                behaviourBase.m_EndStatus.m_UniformValue.z
            );

            m_FinalValue = Vector3.LerpUnclamped(m_InitValue, endValue, progress);
        }

        protected override void ApplyValue(object playerData)
        {
            base.ApplyValue(playerData);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UiAnimation
{
    public class UiAnimationMixerRectTransformAnchoredPosition : UiAnimationMixerBase
    {
        private Vector2 m_InitValue;
        private Vector2 m_FinalValue;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            var rectTransform = playerData as RectTransform;
            if (rectTransform == null) return;

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

            IterateInput(playable);

            rectTransform.anchoredPosition = m_FinalValue;
        }

        protected override void ProcessInput(UiAnimationBehaviourBase behaviourBase, float progress)
        {
            base.ProcessInput(behaviourBase, progress);

            var endValue = new Vector2(
                behaviourBase.m_EndStatus.m_UniformValue.x,
                behaviourBase.m_EndStatus.m_UniformValue.y
            );

            m_FinalValue = Vector2.Lerp(m_InitValue, endValue, progress);
        }
    }
}

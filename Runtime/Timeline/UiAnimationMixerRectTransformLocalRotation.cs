using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UiAnimation
{
    public class UiAnimationMixerRectTransformLocalRotation : UiAnimationMixerBase
    {
        private float m_InitValue;
        private float m_FinalValue;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            var rectTransform = playerData as RectTransform;
            if (rectTransform == null) return;

            // From Track
            m_InitValue = m_InitStatus.m_UniformValue.x;

            // From Clips
            var lastBehaviour = FindLastFinishedClip(playable);
            if (lastBehaviour != null)
            {
                m_InitValue = lastBehaviour.m_EndStatus.m_UniformValue.x;
            }

            m_FinalValue = m_InitValue;

            IterateInput(playable);

            rectTransform.localEulerAngles = new Vector3(
                rectTransform.localEulerAngles.x,
                rectTransform.localEulerAngles.y,
                m_FinalValue
            );
        }

        protected override void ProcessInput(UiAnimationBehaviourBase behaviourBase, float progress)
        {
            base.ProcessInput(behaviourBase, progress);

            var endValue = behaviourBase.m_EndStatus.m_UniformValue.x;

            m_FinalValue = Mathf.Lerp(m_InitValue, endValue, (float)progress);
        }
    }
}

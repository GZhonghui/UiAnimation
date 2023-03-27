using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    public class UiAnimationMixerBase : PlayableBehaviour
    {
        public UiAnimationStatus m_InitStatus;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
        }

        // Interface
        protected virtual void ProcessInput(UiAnimationBehaviourBase behaviourBase, float progress)
        {

        }

        // Interface
        protected virtual void ApplyValue(object playerData)
        {

        }

        // Tool
        protected void IterateInput(Playable playable)
        {
            var inputCount = playable.GetInputCount();
            for (int i = 0; i < inputCount; i++)
            {
                if (playable.GetInputWeight(i) <= 0) continue;

                var derivedPlayable = (ScriptPlayable<UiAnimationBehaviourBase>)playable.GetInput(i);
                var behaviour = derivedPlayable.GetBehaviour();

                if (behaviour != null)
                {
                    float progress = (float)(derivedPlayable.GetTime() / derivedPlayable.GetDuration());

                    if (behaviour.m_Curve != null)
                    {
                        progress = behaviour.m_Curve.Evaluate(progress);
                    }

                    ProcessInput(behaviour, progress);
                }
            }
        }

        // Tool
        protected UiAnimationBehaviourBase FindLastFinishedClip(Playable playable)
        {
            UiAnimationBehaviourBase result = null;

            var inputCount = playable.GetInputCount();
            double maxEnd = 0.0;

            // Update Init Value
            for (int i = 0; i < inputCount; i++)
            {
                var derivedPlayable = (ScriptPlayable<UiAnimationBehaviourBase>)playable.GetInput(i);
                var behaviour = derivedPlayable.GetBehaviour();

                // Clip is Finished
                if (behaviour != null && playable.GetTime() + UiAnimationDefine.timelineEps >= behaviour.m_End)
                {
                    // Find Last Clip
                    if (behaviour.m_End > maxEnd)
                    {
                        maxEnd = behaviour.m_End;
                        result = behaviour;
                    }
                }
            }

            return result;
        }
    }
}

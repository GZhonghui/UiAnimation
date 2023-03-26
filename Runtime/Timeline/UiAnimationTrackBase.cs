using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    [TrackColor(0, 1, 0)]
    public class UiAnimationTrackBase : TrackAsset
    {
        public UiAnimationStatus m_InitStatus;

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<UiAnimationMixerBase>.Create(graph, inputCount);

            ProcessPlayable(playable);

            return playable;
        }

        protected virtual void ProcessPlayable(Playable playable)
        {
            var basePlayable = (ScriptPlayable<UiAnimationMixerBase>)playable;
            var mixer = basePlayable.GetBehaviour();
            if (mixer != null)
            {
                mixer.m_InitStatus = m_InitStatus;
            }

            foreach (var clip in GetClips())
            {
                var baseClip = clip.asset as UiAnimationClipBase;
                if (baseClip != null)
                {
                    baseClip.m_Start = clip.start;
                    baseClip.m_End = clip.end;
                }
            }
        }

#if UNITY_EDITOR
        // propertyInitStatus saved in UiAnimation
        public virtual void EditorDrawInitValue(UnityEditor.SerializedProperty propertyInitStatus)
        {

        }

        public virtual void EditorLock(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {

        }

        public virtual void EditorReset(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {

        }

        public virtual void EditorSetInitValue(UnityEditor.SerializedProperty propertyInitStatus)
        {
            var uniformValue = propertyInitStatus.FindPropertyRelative("m_UniformValue");
            m_InitStatus.m_UniformValue = new Vector4(
                uniformValue.FindPropertyRelative("x").floatValue,
                uniformValue.FindPropertyRelative("y").floatValue,
                uniformValue.FindPropertyRelative("z").floatValue,
                uniformValue.FindPropertyRelative("w").floatValue
            );
        }
#endif
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(UiAnimationTrackBase))]
    public class UiAnimationTrackBaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            
        }
    }

#endif
}
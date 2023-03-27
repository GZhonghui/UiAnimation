using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;
using DG.Tweening;

#if UNITY_2022_1_OR_NEWER
using Unity.VisualScripting;
#endif

namespace UiAnimation
{
    public class UiAnimationClipBase : PlayableAsset
    {
        public UiAnimationStatus m_EndStatus;
        public AnimationCurve m_Curve;
        public double m_Start;
        public double m_End;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<UiAnimationBehaviourBase>.Create(graph);

            ProcessPlayable(playable);

            return playable;
        }

        protected virtual void ProcessPlayable(Playable playable)
        {
            var basePlayable = (ScriptPlayable<UiAnimationBehaviourBase>)playable;
            var behaviour = basePlayable.GetBehaviour();
            if (behaviour != null)
            {
                behaviour.m_EndStatus = m_EndStatus;
                behaviour.m_Curve = m_Curve;
                behaviour.m_Start = m_Start;
                behaviour.m_End = m_End;
            }
        }

        public virtual Tween CreateTween(UnityEngine.Object target)
        {
            return null;
        }
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UiAnimationClipBase))]
    public class UiAnimationClipBaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var curve =serializedObject.FindProperty("m_Curve");
            curve.animationCurveValue = UnityEditor.EditorGUILayout.CurveField("Curve", curve.animationCurveValue);

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
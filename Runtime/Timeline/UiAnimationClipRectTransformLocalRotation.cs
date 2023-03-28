using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;
using DG.Tweening;

namespace UiAnimation
{
    public class UiAnimationClipRectTransformLocalRotation : UiAnimationClipBase
    {
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<UiAnimationBehaviourRectTransformLocalRotation>.Create(graph);

            ProcessPlayable(playable);

            return playable;
        }

        public override Tween CreateTween(UnityEngine.Object target)
        {
            var rectTransform = target as RectTransform;
            if (rectTransform == null) return null;

            return DOTween.To(
                () => rectTransform.localEulerAngles.z,
                x => rectTransform.localEulerAngles = new Vector3(
                    rectTransform.localEulerAngles.x,
                    rectTransform.localEulerAngles.y,
                    x
                ),
                m_EndStatus.m_UniformValue.x,
                (float)(m_End - m_Start)
            );
        }
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UiAnimationClipRectTransformLocalRotation))]
    public class UiAnimationClipRectTransformLocalRotationEditor : UiAnimationClipBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            var uniformEndValue = serializedObject.FindProperty("m_EndStatus");
            var uniformValue = uniformEndValue.FindPropertyRelative("m_UniformValue");
            var x = uniformValue.FindPropertyRelative("x");

            x.floatValue = UnityEditor.EditorGUILayout.FloatField(
                "End Local Rotation", x.floatValue
            );

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace UiAnimation
{
    public class UiAnimationClipUguiTmpTextFontSize : UiAnimationClipBase
    {
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<UiAnimationBehaviourUguiTmpTextFontSize>.Create(graph);

            ProcessPlayable(playable);

            return playable;
        }

        protected override void ProcessPlayable(Playable playable)
        {
            base.ProcessPlayable(playable);
        }

        public override Tween CreateTween(UnityEngine.Object target)
        {
            var text = target as TextMeshProUGUI;
            if (text == null) return null;

            return DOTween.To(
                () => text.fontSize,
                x => text.fontSize = x,
                m_EndStatus.m_UniformValue.x,
                (float)duration
            ).Pause();
        }
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UiAnimationClipUguiTmpTextFontSize))]
    public class UiAnimationClipUguiTmpTextFontSizeEditor : UiAnimationClipBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            var uniformEndValue = serializedObject.FindProperty("m_EndStatus");
            var uniformValue = uniformEndValue.FindPropertyRelative("m_UniformValue");
            var x = uniformValue.FindPropertyRelative("x");

            x.floatValue = UnityEditor.EditorGUILayout.FloatField(
                "End Font Size",
                x.floatValue
            );

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}

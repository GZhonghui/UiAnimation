using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace UiAnimation
{
    public class UiAnimationClipUguiTmpTextColor : UiAnimationClipBase
    {
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<UiAnimationBehaviourUguiTmpTextColor>.Create(graph);

            ProcessPlayable(playable);

            return playable;
        }

        public override Tween CreateTween(UnityEngine.Object target)
        {
            var text = target as TextMeshProUGUI;
            if (text == null) return null;

            return DOTween.To(
                () => text.color,
                x => text.color = x,
                new Color(
                    m_EndStatus.m_UniformValue.x,
                    m_EndStatus.m_UniformValue.y,
                    m_EndStatus.m_UniformValue.z,
                    m_EndStatus.m_UniformValue.w
                ),
                (float)(m_End - m_Start)
            );
        }
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UiAnimationClipUguiTmpTextColor))]
    public class UiAnimationClipUguiTmpTextColorEditor : UiAnimationClipBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            var uniformEndValue = serializedObject.FindProperty("m_EndStatus");
            var uniformValue = uniformEndValue.FindPropertyRelative("m_UniformValue");
            var x = uniformValue.FindPropertyRelative("x");
            var y = uniformValue.FindPropertyRelative("y");
            var z = uniformValue.FindPropertyRelative("z");
            var w = uniformValue.FindPropertyRelative("w");

            var editorResult = UnityEditor.EditorGUILayout.ColorField(
                "End Text Color",
                new Color(x.floatValue, y.floatValue, z.floatValue, w.floatValue)
            );

            x.floatValue = editorResult.r;
            y.floatValue = editorResult.g;
            z.floatValue = editorResult.b;
            w.floatValue = editorResult.a;

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}


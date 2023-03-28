using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

namespace UiAnimation
{
    public class UiAnimationClipUguiImageColor : UiAnimationClipBase
    {
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<UiAnimationBehaviourUguiImageColor>.Create(graph);

            ProcessPlayable(playable);

            return playable;
        }

        public override Tween CreateTween(UnityEngine.Object target)
        {
            var image = target as Image;
            if (image == null) return null;

            return DOTween.To(
                () => image.color,
                x => image.color = x,
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

    [UnityEditor.CustomEditor(typeof(UiAnimationClipUguiImageColor))]
    public class UiAnimationClipUguiImageColorEditor : UiAnimationClipBaseEditor
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
                "End Image Color",
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

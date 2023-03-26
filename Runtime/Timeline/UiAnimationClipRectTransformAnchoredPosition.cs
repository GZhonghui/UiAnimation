using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    public class UiAnimationClipRectTransformAnchoredPosition : UiAnimationClipBase
    {
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<UiAnimationBehaviourRectTransformAnchoredPosition>.Create(graph);

            ProcessPlayable(playable);

            return playable;
        }

        protected override void ProcessPlayable(Playable playable)
        {
            base.ProcessPlayable(playable);
        }
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UiAnimationClipRectTransformAnchoredPosition))]
    public class UiAnimationClipRectTransformAnchoredPositionEditor : UiAnimationClipBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            var uniformEndValue = serializedObject.FindProperty("m_EndStatus");
            var uniformValue = uniformEndValue.FindPropertyRelative("m_UniformValue");
            var x = uniformValue.FindPropertyRelative("x");
            var y = uniformValue.FindPropertyRelative("y");

            var editorResult = UnityEditor.EditorGUILayout.Vector2Field(
                "End Anchored Position",
                new Vector2(x.floatValue, y.floatValue)
            );

            x.floatValue = editorResult.x;
            y.floatValue = editorResult.y;

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
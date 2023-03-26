using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    [TrackBindingType(typeof(RectTransform))]
    [TrackClipType(typeof(UiAnimationClipRectTransformAnchoredPosition))]
    public class UiAnimationTrackRectTransformAnchoredPosition : UiAnimationTrackBase
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<UiAnimationMixerRectTransformAnchoredPosition>.Create(graph, inputCount);

            ProcessPlayable(playable);

            return playable;
        }

#if UNITY_EDITOR
        public override void EditorDrawInitValue(UnityEditor.SerializedProperty propertyInitStatus)
        {
            var x = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("x");
            var y = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("y");

            var editorResult = UnityEditor.EditorGUILayout.Vector2Field(
                "Init Anchored Position", new Vector2(x.floatValue, y.floatValue)
            );
            x.floatValue = editorResult.x;
            y.floatValue = editorResult.y;
        }

        public override void EditorLock(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {
            var rectTransform = binding as RectTransform;
            var x = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("x");
            var y = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("y");

            if (rectTransform != null)
            {
                x.floatValue = rectTransform.anchoredPosition.x;
                y.floatValue = rectTransform.anchoredPosition.y;
            }
        }

        public override void EditorReset(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {
            var rectTransform = binding as RectTransform;
            var x = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("x");
            var y = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("y");

            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(x.floatValue, y.floatValue);
            }
        }
#endif
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UiAnimationTrackRectTransformAnchoredPosition))]
    public class UiAnimationTrackRectTransformAnchoredPositionEditor : UiAnimationTrackBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var initValue = serializedObject.FindProperty("m_InitStatus");
            var uniformValue = initValue.FindPropertyRelative("m_UniformValue");

            UnityEditor.EditorGUILayout.Vector2Field("Init Anchored Position",
                new Vector2(
                    uniformValue.FindPropertyRelative("x").floatValue,
                    uniformValue.FindPropertyRelative("y").floatValue
                )
            );
        }
    }

#endif
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    [TrackBindingType(typeof(RectTransform))]
    [TrackClipType(typeof(UiAnimationClipRectTransformLocalScale))]
    public class UiAnimationTrackRectTransformLocalScale : UiAnimationTrackBase
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<UiAnimationMixerRectTransformLocalScale>.Create(graph, inputCount);

            ProcessPlayable(playable);

            return playable;
        }

        public override void InitProperty(UnityEngine.Object target)
        {
            base.InitProperty(target);

            var rectTransform = target as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.localScale = new Vector3(
                    m_InitStatus.m_UniformValue.x,
                    m_InitStatus.m_UniformValue.y,
                    1
                );
            }
        }

#if UNITY_EDITOR
        public override void EditorDrawInitValue(UnityEditor.SerializedProperty propertyInitStatus)
        {
            var x = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("x");
            var y = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("y");

            var editorResult = UnityEditor.EditorGUILayout.Vector2Field(
                "Init Local Scale", new Vector2(x.floatValue, y.floatValue)
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
                x.floatValue = rectTransform.localScale.x;
                y.floatValue = rectTransform.localScale.y;
            }
        }

        public override void EditorReset(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {
            var rectTransform = binding as RectTransform;
            var x = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("x");
            var y = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("y");

            if (rectTransform != null)
            {
                rectTransform.localScale = new Vector3(x.floatValue, y.floatValue, 1);
            }
        }
#endif
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UiAnimationTrackRectTransformLocalScale))]
    public class UiAnimationTrackRectTransformLocalScaleEditor : UiAnimationTrackBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var initValue = serializedObject.FindProperty("m_InitStatus");
            var uniformValue = initValue.FindPropertyRelative("m_UniformValue");

            GUI.color = Color.gray;
            UnityEditor.EditorGUILayout.Vector2Field("Init Local Scale",
                new Vector2(
                    uniformValue.FindPropertyRelative("x").floatValue,
                    uniformValue.FindPropertyRelative("y").floatValue
                )
            );
            GUI.color = Color.white;
        }
    }

#endif
}

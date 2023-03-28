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

        public override void InitProperty(UnityEngine.Object target, UiAnimationStatus initStatus)
        {
            base.InitProperty(target, initStatus);

            var rectTransform = target as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.localScale = new Vector3(
                    initStatus.m_UniformValue.x,
                    initStatus.m_UniformValue.y,
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
            if (rectTransform != null)
            {
                var status = new UiAnimationStatus();
                status.m_UniformValue = new Vector4(
                    rectTransform.localScale.x,
                    rectTransform.localScale.y
                );
                status.Serialize(propertyInitStatus);
            }
        }

        public override void EditorReset(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {
            var rectTransform = binding as RectTransform;
            if (rectTransform != null)
            {
                var status = new UiAnimationStatus();
                status.Deserialize(propertyInitStatus);
                rectTransform.localScale = new Vector3(
                    status.m_UniformValue.x,
                    status.m_UniformValue.y,
                    1
                );
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

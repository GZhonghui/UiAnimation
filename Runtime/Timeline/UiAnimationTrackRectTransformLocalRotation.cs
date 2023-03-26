using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    [TrackBindingType(typeof(RectTransform))]
    [TrackClipType(typeof(UiAnimationClipRectTransformLocalRotation))]
    public class UiAnimationTrackRectTransformLocalRotation : UiAnimationTrackBase
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<UiAnimationMixerRectTransformLocalRotation>.Create(graph, inputCount);

            ProcessPlayable(playable);

            return playable;
        }

#if UNITY_EDITOR
        public override void EditorDrawInitValue(UnityEditor.SerializedProperty propertyInitStatus)
        {
            var x = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("x");
            x.floatValue = UnityEditor.EditorGUILayout.FloatField("Init Local Rotation", x.floatValue);
        }

        public override void EditorLock(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {
            var rectTransform = binding as RectTransform;
            var x = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("x");

            if (rectTransform != null)
            {
                x.floatValue = rectTransform.localEulerAngles.z;
            }
        }

        public override void EditorReset(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {
            var rectTransform = binding as RectTransform;
            var x = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("x");

            if (rectTransform != null)
            {
                rectTransform.localEulerAngles = new Vector3(
                    rectTransform.localEulerAngles.x,
                    rectTransform.localEulerAngles.y,
                    x.floatValue
                );
            }
        }
#endif
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UiAnimationTrackRectTransformLocalRotation))]
    public class UiAnimationTrackRectTransformLocalRotationEditor : UiAnimationTrackBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var initValue = serializedObject.FindProperty("m_InitStatus");
            var uniformValue = initValue.FindPropertyRelative("m_UniformValue");

            UnityEditor.EditorGUILayout.FloatField(
                "Init Local Rotation", uniformValue.FindPropertyRelative("x").floatValue
            );
        }
    }

#endif
}

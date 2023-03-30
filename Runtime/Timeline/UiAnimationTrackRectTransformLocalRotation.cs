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

        public override void InitProperty(UnityEngine.Object target, UiAnimationStatus initStatus)
        {
            base.InitProperty(target, initStatus);

            var rectTransform = target as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.localEulerAngles = new Vector3(
                    rectTransform.localEulerAngles.x,
                    rectTransform.localEulerAngles.y,
                    initStatus.m_UniformValue.x
                );
            }
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            var rectTransform = director.GetGenericBinding(this) as RectTransform;
            if (rectTransform != null)
            {
                driver.AddFromName(rectTransform, "m_LocalRotation");
            }
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
            if (rectTransform != null)
            {
                var status = new UiAnimationStatus();
                status.m_UniformValue.x = rectTransform.localEulerAngles.z;
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
                rectTransform.localEulerAngles = new Vector3(
                    rectTransform.localEulerAngles.x,
                    rectTransform.localEulerAngles.y,
                    status.m_UniformValue.x
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

            GUI.color = Color.gray;
            UnityEditor.EditorGUILayout.FloatField(
                "Init Local Rotation", uniformValue.FindPropertyRelative("x").floatValue
            );
            GUI.color = Color.white;
        }
    }

#endif
}

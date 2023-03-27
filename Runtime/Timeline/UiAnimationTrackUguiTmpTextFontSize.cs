using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace UiAnimation
{
    [TrackBindingType(typeof(TextMeshProUGUI))]
    [TrackClipType(typeof(UiAnimationClipUguiTmpTextFontSize))]
    public class UiAnimationTrackUguiTmpTextFontSize : UiAnimationTrackBase
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<UiAnimationMixerUguiTmpTextFontSize>.Create(graph, inputCount);

            ProcessPlayable(playable);

            return playable;
        }

        public override void InitProperty(UnityEngine.Object target)
        {
            base.InitProperty(target);

            var text = target as TextMeshProUGUI;
            if (text != null)
            {
                text.fontSize = m_InitStatus.m_UniformValue.x;
            }
        }

#if UNITY_EDITOR
        public override void EditorDrawInitValue(UnityEditor.SerializedProperty propertyInitStatus)
        {
            var x = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("x");

            x.floatValue = UnityEditor.EditorGUILayout.FloatField(
                "Init Font Size", x.floatValue
            );
        }

        public override void EditorLock(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {
            var text = binding as TextMeshProUGUI;
            var x = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("x");

            if (text != null)
            {
                x.floatValue = text.fontSize;
            }
        }

        public override void EditorReset(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {
            var text = binding as TextMeshProUGUI;
            var x = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("x");

            if (text != null)
            {
                text.fontSize = x.floatValue;
            }
        }
#endif
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UiAnimationTrackUguiTmpTextFontSize))]
    public class UiAnimationTrackUguiTmpTextFontSizeEditor : UiAnimationTrackBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var initValue = serializedObject.FindProperty("m_InitStatus");
            var uniformValue = initValue.FindPropertyRelative("m_UniformValue");

            GUI.color = Color.gray;
            UnityEditor.EditorGUILayout.FloatField("Init Font Size",
                uniformValue.FindPropertyRelative("x").floatValue
            );
            GUI.color = Color.white;
        }
    }

#endif
}
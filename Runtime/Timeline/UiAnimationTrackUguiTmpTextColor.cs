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
    [TrackClipType(typeof(UiAnimationClipUguiTmpTextColor))]
    public class UiAnimationTrackUguiTmpTextColor : UiAnimationTrackBase
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<UiAnimationMixerUguiTmpTextColor>.Create(graph, inputCount);

            ProcessPlayable(playable);

            return playable;
        }

        public override void InitProperty(UnityEngine.Object target, UiAnimationStatus initStatus)
        {
            base.InitProperty(target, initStatus);

            var text = target as TextMeshProUGUI;
            if (text != null)
            {
                text.color = new Color(
                    initStatus.m_UniformValue.x,
                    initStatus.m_UniformValue.y,
                    initStatus.m_UniformValue.z,
                    initStatus.m_UniformValue.w
                );
            }
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            var text = director.GetGenericBinding(this) as TextMeshProUGUI;
            if (text != null)
            {
                driver.AddFromName(text, "m_Color");
            }
        }

#if UNITY_EDITOR
        public override void EditorDrawInitValue(UnityEditor.SerializedProperty propertyInitStatus)
        {
            var x = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("x");
            var y = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("y");
            var z = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("z");
            var w = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("w");

            var editorResult = UnityEditor.EditorGUILayout.ColorField(
                "Init Text Color", new Color(x.floatValue, y.floatValue, z.floatValue, w.floatValue)
            );

            x.floatValue = editorResult.r;
            y.floatValue = editorResult.g;
            z.floatValue = editorResult.b;
            w.floatValue = editorResult.a;
        }

        public override void EditorLock(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {
            var text = binding as TextMeshProUGUI;
            if (text != null)
            {
                var status = new UiAnimationStatus();
                status.m_UniformValue = text.color;
                status.Serialize(propertyInitStatus);
            }
        }

        public override void EditorReset(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {
            var text = binding as TextMeshProUGUI;
            if (text != null)
            {
                var status = new UiAnimationStatus();
                status.Deserialize(propertyInitStatus);
                text.color = status.m_UniformValue;
            }
        }
#endif
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UiAnimationTrackUguiTmpTextColor))]
    public class UiAnimationTrackUguiTmpTextColorEditor : UiAnimationTrackBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var initValue = serializedObject.FindProperty("m_InitStatus");
            var uniformValue = initValue.FindPropertyRelative("m_UniformValue");

            GUI.color = Color.gray;
            UnityEditor.EditorGUILayout.ColorField("Init Text Color",
                new Color(
                    uniformValue.FindPropertyRelative("x").floatValue,
                    uniformValue.FindPropertyRelative("y").floatValue,
                    uniformValue.FindPropertyRelative("z").floatValue,
                    uniformValue.FindPropertyRelative("w").floatValue
                )
            );
            GUI.color = Color.white;
        }
    }

#endif
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine;

namespace UiAnimation
{
    [TrackBindingType(typeof(Image))]
    [TrackClipType(typeof(UiAnimationClipUguiImageColor))]
    public class UiAnimationTrackUguiImageColor : UiAnimationTrackBase
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<UiAnimationMixerUguiImageColor>.Create(graph, inputCount);

            ProcessPlayable(playable);

            return playable;
        }

        public override void InitProperty(UnityEngine.Object target)
        {
            base.InitProperty(target);

            var image = target as Image;
            if (image != null)
            {
                image.color = new Color(
                    m_InitStatus.m_UniformValue.x,
                    m_InitStatus.m_UniformValue.y,
                    m_InitStatus.m_UniformValue.z,
                    m_InitStatus.m_UniformValue.w
                );
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
                "Init Image Color", new Color(x.floatValue, y.floatValue, z.floatValue, w.floatValue)
            );

            x.floatValue = editorResult.r;
            y.floatValue = editorResult.g;
            z.floatValue = editorResult.b;
            w.floatValue = editorResult.a;
        }

        public override void EditorLock(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {
            var image = binding as Image;
            var x = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("x");
            var y = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("y");
            var z = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("z");
            var w = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("w");

            if (image != null)
            {
                x.floatValue = image.color.r;
                y.floatValue = image.color.g;
                z.floatValue = image.color.b;
                w.floatValue = image.color.a;
            }
        }

        public override void EditorReset(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {
            var image = binding as Image;
            var x = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("x");
            var y = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("y");
            var z = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("z");
            var w = propertyInitStatus.FindPropertyRelative("m_UniformValue").FindPropertyRelative("w");

            if (image != null)
            {
                image.color = new Color(x.floatValue, y.floatValue, z.floatValue, w.floatValue);
            }
        }
#endif
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UiAnimationTrackUguiImageColor))]
    public class UiAnimationTrackUguiImageColorEditor : UiAnimationTrackBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var initValue = serializedObject.FindProperty("m_InitStatus");
            var uniformValue = initValue.FindPropertyRelative("m_UniformValue");

            GUI.color = Color.gray;
            UnityEditor.EditorGUILayout.ColorField("Init Image Color",
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
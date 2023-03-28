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

        public override void InitProperty(UnityEngine.Object target, UiAnimationStatus initStatus)
        {
            base.InitProperty(target, initStatus);

            var image = target as Image;
            if (image != null)
            {
                image.color = new Color(
                    initStatus.m_UniformValue.x,
                    initStatus.m_UniformValue.y,
                    initStatus.m_UniformValue.z,
                    initStatus.m_UniformValue.w
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
            if (image != null)
            {
                var status = new UiAnimationStatus();
                status.m_UniformValue = image.color;
                status.Serialize(propertyInitStatus);
            }
        }

        public override void EditorReset(UnityEditor.SerializedProperty propertyInitStatus, UnityEngine.Object binding)
        {
            var image = binding as Image;
            if (image != null)
            {
                var status = new UiAnimationStatus();
                status.Deserialize(propertyInitStatus);
                image.color = status.m_UniformValue;
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
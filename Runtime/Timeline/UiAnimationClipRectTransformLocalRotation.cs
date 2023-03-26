using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    public class UiAnimationClipRectTransformLocalRotation : UiAnimationClipBase
    {
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<UiAnimationBehaviourRectTransformLocalRotation>.Create(graph);

            ProcessPlayable(playable);

            return playable;
        }

        protected override void ProcessPlayable(Playable playable)
        {
            base.ProcessPlayable(playable);
        }
    }

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(UiAnimationClipRectTransformLocalRotation))]
    public class UiAnimationClipRectTransformLocalRotationEditor : UiAnimationClipBaseEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            var uniformEndValue = serializedObject.FindProperty("m_EndStatus");
            var uniformValue = uniformEndValue.FindPropertyRelative("m_UniformValue");
            var x = uniformValue.FindPropertyRelative("x");

            x.floatValue = UnityEditor.EditorGUILayout.FloatField(
                "End Local Rotation", x.floatValue
            );

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}

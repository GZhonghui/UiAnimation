#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;
using UnityEditor;

#if UNITY_2022_1_OR_NEWER
using Unity.VisualScripting;
#endif

namespace UiAnimation
{
    [CustomEditor(typeof(UiAnimation))]
    public class UiAnimationEditor : Editor
    {
        private UiAnimation m_UiAnimation;
        private int m_SelectedIndex = -1;

        public override void OnInspectorGUI()
        {
            m_UiAnimation = target as UiAnimation;

            // Properties
            var propertyInstances = serializedObject.FindProperty("m_Instances");

            GUILayout.BeginVertical("HelpBox");
            EditorGUILayout.HelpBox("UiAnimation Tools", MessageType.Info, true);
            #region Title
            if (GUILayout.Button("Create Animation Instance"))
            {
                AddInstance();
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Stop Preview"))
            {
                StopPreview();
            }
            if (GUILayout.Button("Export All"))
            {

            }
            GUILayout.EndHorizontal();
            #endregion
            GUILayout.EndVertical();

            GUILayout.BeginVertical("HelpBox");
            EditorGUILayout.HelpBox("UiAnimation Instances", MessageType.Info, true);
            #region Instances

            for (int i = 0; i < propertyInstances.arraySize; i++)
            {
                var propertyInstance = propertyInstances.GetArrayElementAtIndex(i);

                GUILayout.BeginHorizontal();
                // Select Button
                GUI.color = (m_SelectedIndex == i) ? Color.cyan : Color.white;
                if (GUILayout.Button(propertyInstance.FindPropertyRelative("m_InstanceName").stringValue))
                {
                    if (m_SelectedIndex == i) m_SelectedIndex = -1;
                    else m_SelectedIndex = i;
                }
                // Remove Button
                GUI.color = new Color(128 / 255.999f, 109 / 255.999f, 158 / 255.999f);
                if (GUILayout.Button("X", GUILayout.Width(32)))
                {
                    RemoveInstance(i);
                    // if (m_SelectedIndex == i)
                    {
                        m_SelectedIndex = -1;
                    }
                }
                GUI.color = Color.white;
                GUILayout.EndHorizontal();

                if (m_SelectedIndex == i)
                {
                    DrawInstance(propertyInstance);
                }
            }

            #endregion
            GUILayout.EndVertical();
        }

        #region Private

        private void AddInstance(string instanceName = "EMPTY")
        {
            serializedObject.Update();

            var propertyInstances = serializedObject.FindProperty("m_Instances");

            var instanceCount = propertyInstances.arraySize;
            propertyInstances.InsertArrayElementAtIndex(instanceCount);
            var propertyAddedInstance = propertyInstances.GetArrayElementAtIndex(instanceCount);

            // Init
            propertyAddedInstance.FindPropertyRelative("m_InstanceName").stringValue = instanceName;
            propertyAddedInstance.FindPropertyRelative("m_AssetName").stringValue = "";
            propertyAddedInstance.FindPropertyRelative("m_AssetPath").stringValue = "";
            propertyAddedInstance.FindPropertyRelative("m_TimelineAsset").objectReferenceValue = null;
            propertyAddedInstance.FindPropertyRelative("m_Bindings").arraySize = 0;

            serializedObject.ApplyModifiedProperties();
        }

        private void RemoveInstance(int index)
        {
            serializedObject.Update();
            var propertyInstances = serializedObject.FindProperty("m_Instances");
            if (propertyInstances.arraySize > index)
            {
                propertyInstances.DeleteArrayElementAtIndex(index);
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawInstance(SerializedProperty propertyInstance)
        {
            if (propertyInstance == null) return;

            serializedObject.Update();

            GUILayout.BeginVertical("GroupBox");

            GUILayout.Label("Instance Editor");

            // Name
            var propertyName = propertyInstance.FindPropertyRelative("m_InstanceName");
            propertyName.stringValue = EditorGUILayout.TextField("Instance Name", propertyName.stringValue);

            // Timeline Asset
            var propertyTimelineAsset = propertyInstance.FindPropertyRelative("m_TimelineAsset");
            propertyTimelineAsset.objectReferenceValue = EditorGUILayout.ObjectField(
                "Timeline Asset",
                propertyTimelineAsset.objectReferenceValue,
                typeof(TimelineAsset),
                false
            );

            #region Tools
            bool allLock = false;
            bool allReset = false;
            GUILayout.BeginHorizontal();
            GUI.color = Color.green;
            allReset = GUILayout.Button("Reset All");
            GUI.color = Color.red;
            allLock = GUILayout.Button("Lock All");
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
            #endregion

            // Timeline Binding
            #region Bindings
            var timelineAsset = propertyTimelineAsset.objectReferenceValue as TimelineAsset;
            if (timelineAsset != null)
            {
                // Read Tracks from Timeline Asset
                var listPlayableBindings = new List<PlayableBinding>();
                foreach (var output in timelineAsset.outputs)
                {
                    var track = output.sourceObject as TrackAsset;
                    if (track != null)
                    {
                        if (track.GetType().IsSubclassOf(typeof(UiAnimationTrackBase))
                            && output.outputTargetType != null)
                        {
                            listPlayableBindings.Add(output);
                        }
                    }
                }

                // Read Saved Bindings
                var propertyBindings = propertyInstance.FindPropertyRelative("m_Bindings");
                var savedBindings = new Dictionary<UnityEngine.Object, UnityEngine.Object>();

                for (int i = 0; i < propertyBindings.arraySize; i += 1)
                {
                    var propertyBinding = propertyBindings.GetArrayElementAtIndex(i);
                    var propertyKey = propertyBinding.FindPropertyRelative("m_Key");
                    var propertyValue = propertyBinding.FindPropertyRelative("m_Value");

                    if (propertyKey.objectReferenceValue != null
                        && propertyValue.objectReferenceValue != null)
                    {
                        savedBindings[propertyKey.objectReferenceValue] = propertyValue.objectReferenceValue;
                    }
                }

                // Resize Saved Bindings
                propertyBindings.arraySize = listPlayableBindings.Count;

                for (int i = 0; i < listPlayableBindings.Count; i += 1)
                {
                    GUILayout.BeginVertical("GroupBox");

                    var output = listPlayableBindings[i];
                    var sourceObject = output.sourceObject;

                    var propertyBinding = propertyBindings.GetArrayElementAtIndex(i);

                    var propertyFileId = propertyBinding.FindPropertyRelative("m_FileId");
                    var propertyKey = propertyBinding.FindPropertyRelative("m_Key");
                    var propertyValue = propertyBinding.FindPropertyRelative("m_Value");
                    var propertyInitStatus = propertyBinding.FindPropertyRelative("m_InitStatus");

                    // File Id
                    AssetDatabase.TryGetGUIDAndLocalFileIdentifier(
                        sourceObject, out string guid, out long fileId
                    );
                    propertyFileId.longValue = fileId;

                    // Key
                    propertyKey.objectReferenceValue = sourceObject;

                    // Value
                    UnityEngine.Object savedTarget = null;
                    if (propertyKey.objectReferenceValue != null
                        && savedBindings.ContainsKey(propertyKey.objectReferenceValue))
                    {
                        savedTarget = savedBindings[propertyKey.objectReferenceValue];
                    }
                    propertyValue.objectReferenceValue = EditorGUILayout.ObjectField(
                        sourceObject.name,
                        savedTarget,
                        output.outputTargetType,
                        true
                    );

                    // Init Value
                    GUILayout.BeginHorizontal();
                    sourceObject.GetType().GetMethod("EditorDrawInitValue").Invoke(
                        sourceObject, new object[] { propertyInitStatus }
                    );
                    GUI.color = Color.green;
                    if (GUILayout.Button("R", GUILayout.Width(24)) || allReset)
                    {
                        sourceObject.GetType().GetMethod("EditorReset").Invoke(
                            sourceObject, new object[] { propertyInitStatus, propertyValue.objectReferenceValue }
                        );
                    }
                    GUI.color = Color.red;
                    if (GUILayout.Button("L", GUILayout.Width(24)) || allLock)
                    {
                        sourceObject.GetType().GetMethod("EditorLock").Invoke(
                            sourceObject, new object[] { propertyInitStatus, propertyValue.objectReferenceValue }
                        );
                    }
                    GUI.color = Color.white;
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                }
            }
            #endregion

            #region Generate
            GUILayout.BeginHorizontal();
            GUI.color = new Color(241 / 255.999f, 147 / 255.999f, 156 / 255.999f);
            if (GUILayout.Button("Build Timeline"))
            {
                BuildPreview(propertyInstance);
            }
            if (GUILayout.Button("Export DoTween"))
            {

            }
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
            #endregion

            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        private void BuildPreview(SerializedProperty propertyInstance)
        {
            #region Process Director
            if (m_UiAnimation.GetComponent<PlayableDirector>() == null)
            {
                m_UiAnimation.gameObject.AddComponent<PlayableDirector>();
            }

            var playDirector = m_UiAnimation.GetComponent<PlayableDirector>();
            playDirector.enabled = true;

            // Disable Auto Play
            var serializedDirector = new SerializedObject(playDirector);
            serializedDirector.Update();
            serializedDirector.FindProperty("m_InitialState").intValue = 0;
            serializedDirector.ApplyModifiedProperties();

            // Set Timeline Asset
            var propertyTimelineAsset = propertyInstance.FindPropertyRelative("m_TimelineAsset");
            if (propertyTimelineAsset != null)
            {
                playDirector.playableAsset = propertyTimelineAsset.objectReferenceValue as TimelineAsset;
            }

            // Set Bindings
            var propertyBindings = propertyInstance.FindPropertyRelative("m_Bindings");
            for (int i = 0; i < propertyBindings.arraySize; i += 1)
            {
                var propertyBinding = propertyBindings.GetArrayElementAtIndex(i);
                var propertyKey = propertyBinding.FindPropertyRelative("m_Key");
                var propertyValue = propertyBinding.FindPropertyRelative("m_Value");
                var propertyInitStatus = propertyBinding.FindPropertyRelative("m_InitStatus");

                if (propertyKey.objectReferenceValue != null)
                {
                    playDirector.SetGenericBinding(
                        propertyKey.objectReferenceValue,
                        propertyValue.objectReferenceValue
                    );

                    var track = propertyKey.objectReferenceValue as UiAnimationTrackBase;
                    if (track != null)
                    {
                        track.EditorSetInitValue(propertyInitStatus);
                    }
                }
            }

            #endregion
        }

        private void StopPreview()
        {
            if (m_UiAnimation.GetComponent<PlayableDirector>() == null)
            {
                m_UiAnimation.gameObject.AddComponent<PlayableDirector>();
            }

            var playDirector = m_UiAnimation.GetComponent<PlayableDirector>();
            playDirector.enabled = false;
            playDirector.playableAsset = null;
        }

        #endregion

        #region Menu Tools
        #endregion
    }

}

#endif
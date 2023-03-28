using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;
using System;
using DG.Tweening;

#if UNITY_2022_1_OR_NEWER
using Unity.VisualScripting;
#endif

namespace UiAnimation
{
    [Serializable]
    public struct UiAnimationStatus
    {
        [SerializeField]
        public Vector4 m_UniformValue;

#if UNITY_EDITOR

        public void Serialize(UnityEditor.SerializedProperty statusProperty)
        {
            if (statusProperty != null)
            {
                statusProperty.FindPropertyRelative("m_UniformValue").vector4Value = m_UniformValue;
            }
        }

        public void Deserialize(UnityEditor.SerializedProperty statusProperty)
        {
            if (statusProperty != null)
            {
                m_UniformValue = statusProperty.FindPropertyRelative("m_UniformValue").vector4Value;
            }
        }

#endif
    }

    [Serializable]
    public struct UiAnimationBinding
    {
        // Key
        [SerializeField]
        public long m_FileId;

        // Key
        [SerializeField]
        public UnityEngine.Object m_Key;

        // Value
        [SerializeField]
        public UnityEngine.Object m_Value;

        // Value
        [SerializeField]
        public bool m_EnableInitStatus;

        // Value
        [SerializeField]
        public UiAnimationStatus m_InitStatus;
    }

    [Serializable]
    public struct UiAnimationInstance
    {
        [SerializeField]
        public string m_InstanceName;

        [SerializeField]
        public string m_AssetName;

        [SerializeField]
        public string m_AssetPath;

        // TODO UiAnimation Asset

        [SerializeField]
        public TimelineAsset m_TimelineAsset;

        [SerializeField]
        public List<UiAnimationBinding> m_Bindings;
    }

    public class UiAnimation : MonoBehaviour
    {
        [SerializeField]
        public List<UiAnimationInstance> m_Instances;

        public bool Play(string instanceName, Action callback = null)
        {
            for (int i = 0; i < m_Instances.Count; i += 1)
            {
                // Find First Matching
                var instance = m_Instances[i];
                if (instance.m_InstanceName != null && instance.m_InstanceName == instanceName)
                {
                    if (instance.m_TimelineAsset != null)
                    {
                        // Play via Timeline Asset
                        PlayTimeline(ref instance, callback);
                        return true;
                    }
                    break;
                }
            }

            return false;
        }

        public bool Rewind(string instanceName)
        {
            return false;
        }

        private void PlayTimeline(ref UiAnimationInstance instance, Action callback)
        {
            var bindingMap = new Dictionary<UnityEngine.Object, UiAnimationBinding>();

            for (int i = 0; i < instance.m_Bindings.Count; i++)
            {
                var binding = instance.m_Bindings[i];
                if (binding.m_Key != null && binding.m_Value != null)
                {
                    bindingMap[binding.m_Key] = binding;
                }
            }

            var sequence = DOTween.Sequence();

            foreach (var output in instance.m_TimelineAsset.outputs)
            {
                var track = output.sourceObject as UiAnimationTrackBase;
                if (track != null && bindingMap.ContainsKey(track))
                {
                    var binding = bindingMap[track];
                    var bindingTarget = binding.m_Value;

                    // Enable Init Value at Runtime
                    if (binding.m_EnableInitStatus)
                    {
                        track.InitProperty(bindingTarget, binding.m_InitStatus);
                    }

                    foreach (var clip in track.GetClips())
                    {
                        var derivedClip = clip.asset as UiAnimationClipBase;
                        if (derivedClip != null)
                        {
                            // Write to Asset
                            derivedClip.m_Start = clip.start;
                            derivedClip.m_End = clip.end;
                            var tween = derivedClip.CreateTween(bindingTarget)
                                .SetDelay((float)clip.start).SetEase(derivedClip.m_Curve).Pause();
                            if (tween != null)
                            {
                                sequence.Join(tween);
                            }
                        }
                    }
                }
            }

            sequence.OnComplete(() =>
            {
                callback?.Invoke();
            });
            sequence.Play();
        }
    }
}

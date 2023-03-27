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
    }

    [Serializable]
    public struct UiAnimationBinding
    {
        [SerializeField]
        public long m_FileId;

        [SerializeField]
        public UnityEngine.Object m_Key;

        [SerializeField]
        public UnityEngine.Object m_Value;

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

        public bool Play(string instanceName)
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
                        PlayTimeline(ref instance);
                        return true;
                    }
                    break;
                }
            }

            return false;
        }

        private void PlayTimeline(ref UiAnimationInstance instance)
        {
            Debug.Log("PlayTimeline");

            var bindingMap = new Dictionary<UnityEngine.Object, UnityEngine.Object>();

            for (int i = 0; i < instance.m_Bindings.Count; i++)
            {
                var binding = instance.m_Bindings[i];
                if (binding.m_Key != null && binding.m_Value != null)
                {
                    bindingMap[binding.m_Key] = binding.m_Value;
                }
            }

            var sequence = DOTween.Sequence();

            foreach (var output in instance.m_TimelineAsset.outputs)
            {
                var track = output.sourceObject as UiAnimationTrackBase;
                if (track != null && bindingMap.ContainsKey(track))
                {
                    var bindingTarget = bindingMap[track];

                    track.InitProperty(bindingTarget);

                    foreach (var clip in track.GetClips())
                    {
                        var derivedClip = clip.asset as UiAnimationClipBase;
                        if (derivedClip != null)
                        {
                            var tween = derivedClip.CreateTween(bindingTarget).SetDelay((float)clip.start);
                            tween.Play();
                            if (tween != null)
                            {
                                sequence.Join(tween);
                            }
                        }
                    }
                }
            }

            Debug.Log("Play");
            // sequence.Play();
        }
    }
}

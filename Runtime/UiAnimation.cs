using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;
using System;

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
            return false;
        }
    }
}

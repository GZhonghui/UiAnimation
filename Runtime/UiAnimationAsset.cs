using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

namespace UiAnimation
{
    public struct UiAnimationJson
    {
        public struct Track
        {
            public struct Clip
            {
                public float m_Start;
                public float m_Duration;
                public UiAnimation m_EndStatus;
            }

            public long m_FileId;
            public UiAnimationStatus m_InitStatus;
            public List<Clip> m_Clips;
        }

        public List<Track> m_Tracks;
    }

    public class UiAnimationAsset
    {

    }
}

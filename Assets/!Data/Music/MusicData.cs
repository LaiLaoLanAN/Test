using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicData", menuName = "Data/Music Data")]
public class MusicData : ScriptableObject
{
    [System.Serializable]
    public class MusicTrack
    {
        public string trackName;
        public AudioClip clip;
        public float volume = 1f;
        public bool loop = true;
    }

    public MusicTrack[] musicTracks;
}

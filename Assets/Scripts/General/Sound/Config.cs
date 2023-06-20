using UnityEngine;

namespace General.Sound
{
    [System.Serializable]
    public  class Config
    {
        public AudioMixerGroup mixerGroup;
        public bool loop;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume;
        [Range(.1f, 3f)]
        public float pitch;
        public AudioSource source;
    }
}
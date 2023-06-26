using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace General.Sound
{
    public class AudioManager : MonoBehaviour
    {
        #region Singleton

        private static AudioManager instance;

        public static AudioManager Instance => instance;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        #endregion

        [SerializeField] AudioMixer audioMixer;

        [Header("Sounds")] [Space(20)] [SerializeField]
        Sound[] sounds;

        [SerializeField] Sound currentThemeSound;

        [Header("Volumes")] [Space(20)] public float masterVol, musicVol, fxVol, uiVol;
        public bool masterVolMute, musicVolMute, fxVolMute, uiVolMute;

        public AudioMixer AudioMixer => audioMixer;
        
        [ContextMenu("Initialize")]
        private void Initialize()
        {
            foreach (var sound in sounds)
            {
                sound.config.source = gameObject.AddComponent<AudioSource>();
                sound.config.source.clip = sound.config.clip;
                sound.config.source.volume = sound.config.volume;
                sound.config.source.pitch = sound.config.pitch;
                sound.config.source.loop = sound.config.loop;
                if (sound.config.mixerGroup.Equals(AudioMixerGroup.FX))
                    sound.config.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("FX")[0];
                else if (sound.config.mixerGroup.Equals(AudioMixerGroup.MUSIC))
                    sound.config.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("MUSIC")[0];
                else if (sound.config.mixerGroup.Equals(AudioMixerGroup.UI))
                    sound.config.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("UI")[0];
            }

            Play("StartSceneTheme");
        }


        #region Muters

        public void MuteGeneral()
        {
            masterVol = 20;
            audioMixer.SetFloat("MasterVolume", -80f);
        }

        public void UnMuteGeneral()
        {
            audioMixer.SetFloat("MasterVolume", masterVol);
            masterVol = 20;
        }

        public void MuteMusic()
        {
            musicVol = -12;
            audioMixer.SetFloat("MUSICVolume", -80f);
        }

        public void UnMuteMusic()
        {
            audioMixer.SetFloat("MUSICVolume", musicVol);
            musicVol = -12;
        }

        public void MuteFx()
        {
            fxVol = -6;
            audioMixer.SetFloat("FXVolume", -80f);
        }

        public void UnMuteFx()
        {
            audioMixer.SetFloat("FXVolume", fxVol);
            fxVol = -6;
        }

        public void MuteUi()
        {
            uiVol = -20;
            audioMixer.SetFloat("UIVolume", -80f);
        }

        public void UnMuteUi()
        {
            audioMixer.SetFloat("UIVolume", uiVol);
            uiVol = -20;
        }

        #endregion

        #region VolumeSetters

        public void SetGeneralVolume(float value)
        {
            if (masterVolMute) return;
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 10 + 20);
        }

        public void SetMusicVolume(float value)
        {
            if (musicVolMute) return;
            audioMixer.SetFloat("MUSICVolume", Mathf.Log10(value) * 10 - 24);
        }

        public void SetFxVolume(float value)
        {
            if (fxVolMute) return;
            audioMixer.SetFloat("FXVolume", Mathf.Log10(value) * 10 - 12);
        }

        public void SetUiVolume(float value)
        {
            if (uiVolMute) return;
            audioMixer.SetFloat("UIVolume", Mathf.Log10(value) * 10 - 20);
        }

        #endregion

        [ContextMenu("PlayMainTheme")]
        private void Test()
        {
            Play("StartSceneTheme");
        }

        public void Play(string name)
        {
            var s = Array.Find(sounds, sound => sound.name == name);
            if (s.config.mixerGroup.Equals(AudioMixerGroup.MUSIC))
                currentThemeSound = s;
            s.config.source.Play();
        }

        public void PlayThemes()
        {
            var names = GetAllThemes();
            var id = Random.Range(0, names.Length);
            var s = Array.Find(sounds, sound => sound.name == names[id]);
            StartCoroutine(Fade(false, currentThemeSound, .5f, 0f));
            currentThemeSound.config.source.enabled = false;
            StartCoroutine(Fade(true, s, 1f, s.config.volume));
            Play(s.name);
        }

        private IEnumerator Fade(bool fadeIn, Sound sound, float duration, float targetVol)
        {
            sound.config.source.enabled = true;
            if (!fadeIn)
            {
                var clip = sound.config.source.clip;
                double sourceLenght = (double)clip.samples / clip.frequency;
                yield return new WaitForSecondsRealtime((float)(sourceLenght - duration));
            }

            var time = 0f;
            var startVol = sound.config.source.volume;
            while (time < duration)
            {
                time += Time.unscaledDeltaTime;
                sound.config.source.volume = Mathf.Lerp(startVol, targetVol, time / duration);
                yield return null;
            }
        }

        public void PlayOneShot(string name)
        {
            var s = Array.Find(sounds, sound => sound.name == name);
            s.config.source.PlayOneShot(s.config.clip);
        }

        public void Pause(string name)
        {
            var s = Array.Find(sounds, sound => sound.name == name);
            s.config.source.Pause();
        }

        public bool IsPlaying(string name)
        {
            var s = Array.Find(sounds, sound => sound.name == name);
            return s.config.source.isPlaying;
        }

        public void PauseAllOtherMusic(string name)
        {
            var s = Array.Find(sounds, sound => sound.name == name);
            foreach (var sound in sounds)
            {
                if (sound.config.mixerGroup != AudioMixerGroup.MUSIC) continue;
                if (sound != s)
                {
                    Pause(sound.name);
                }
            }
        }

        public string[] GetAllThemes()
        {
            var names = new List<string>();
            foreach (var s in sounds)
            {
                if (s.config.mixerGroup.Equals(AudioMixerGroup.MUSIC))
                    if (s.name != "StartSceneTheme")
                        names.Add(s.name);
            }

            return names.ToArray();
        }
    }
}
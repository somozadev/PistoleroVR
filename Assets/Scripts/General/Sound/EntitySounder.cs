using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace General.Sound
{
    public class EntitySounder : MonoBehaviour
    {
        [SerializeField] private Sound[] sounds;

        [SerializeField] private AudioSource[] _distanceBasedAudioSources;
        private void Start()
        {
            int i = 0;
            foreach (var sound in sounds)
            {
                sound.config.source = _distanceBasedAudioSources[i];
                sound.config.source.clip = sound.config.clip;
                sound.config.source.volume = sound.config.volume;
                sound.config.source.pitch = sound.config.pitch;
                sound.config.source.loop = sound.config.loop;
                if (sound.config.mixerGroup.Equals(AudioMixerGroup.FX))
                    sound.config.source.outputAudioMixerGroup =
                        AudioManager.Instance.AudioMixer.FindMatchingGroups("FX")[0];
                else if (sound.config.mixerGroup.Equals(AudioMixerGroup.MUSIC))
                    sound.config.source.outputAudioMixerGroup =
                        AudioManager.Instance.AudioMixer.FindMatchingGroups("MUSIC")[0];
                else if (sound.config.mixerGroup.Equals(AudioMixerGroup.UI))
                    sound.config.source.outputAudioMixerGroup =
                        AudioManager.Instance.AudioMixer.FindMatchingGroups("UI")[0];
                i++;
            }
        }


        public void StartSounding()
        {
            float rndTime = Random.Range(5f, 25f);

            StartCoroutine(WaitToPlay(rndTime));
        }

        private IEnumerator WaitToPlay(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            PlayAughSound();
            StartSounding();
        }

        public void PlayLocalOneShot(string name)
        {
            var s = Array.Find(sounds, sound => sound.name == name);
            s.config.source.PlayOneShot(s.config.clip);
        }

        public void StopSounding()
        {
            StopCoroutine(WaitToPlay(0));
        }

        public void PlayDeadSound()
        {
            PlayLocalOneShot("ZombieDie");
        }

        public void PlayAttackSound()
        {
            int id = Random.Range(4, sounds.Length);
            PlayLocalOneShot(sounds[id].name);
        }

        private void PlayAughSound()
        {
            int id = Random.Range(0, sounds.Length-1);
            PlayLocalOneShot(sounds[id].name);
        }
    }
}
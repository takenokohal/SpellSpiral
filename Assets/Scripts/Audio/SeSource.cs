using System;
using UniRx;
using UnityEngine;

namespace Audio
{
    public class SeSource : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        private const float MinVolume = 0.01f;

        public bool IsFinished { get; private set; }

        private void Update()
        {
            if (audioSource.isPlaying)
                return;

            if (Volume > MinVolume)
                return;

            IsFinished = true;
        }

        public void Play(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            IsFinished = false;
        }

        public void Stop()
        {
            audioSource.Stop();
        }

        public float Volume
        {
            get => audioSource.volume;
            set => audioSource.volume = value;
        }
    }
}
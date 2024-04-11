using System;
using UniRx;
using UnityEngine;

namespace Audio
{
    public class SeSource : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        private readonly Subject<Unit> _onPlayFinish = new();
        public IObservable<Unit> OnPlayFinish => _onPlayFinish.TakeUntilDestroy(this);

        private const float MinVolume = 0.01f;

        private void Update()
        {
            if (audioSource.isPlaying)
                return;

            if (Volume < MinVolume)
                return;

            _onPlayFinish.OnNext(Unit.Default);
        }

        public void Play(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
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
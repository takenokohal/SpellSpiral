using System;
using CriWare;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour, IDisposable
    {
        private static AudioManager _instance;

        private CriAtomExPlayer _criAtomExPlayer;
        private CriAtomExAcb _criAtomExAcb;
        [SerializeField] private bool mute;


        public static bool IsInitialized { get; private set; }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            _instance = this;
            _instance.Init().Forget();
        }


        private async UniTaskVoid Init()
        {
            await UniTask.WaitWhile(() => CriAtom.CueSheetsAreLoading);

            _criAtomExPlayer = new CriAtomExPlayer();
            _criAtomExAcb = CriAtom.GetAcb("CueSheet_0");

            if (mute)
                _criAtomExPlayer.SetVolume(0);

            IsInitialized = true;
        }

        void IDisposable.Dispose()
        {
            _criAtomExPlayer?.Dispose();
        }

        public static void PlaySe(string seName)
        {
            _instance._criAtomExPlayer.SetCue(_instance._criAtomExAcb, seName);
            _instance._criAtomExPlayer.Start();
        }
    }
}
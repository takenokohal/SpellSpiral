using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Pool;

namespace Audio
{
    public class SeManager : MonoBehaviour
    {
        [SerializeField] private SeSource sePrefab;
        [SerializeField] private float allVolume;

        [SerializeField] private List<AudioClip> audioClips;


        private ObjectPool<SeSource> _sePool;

        private readonly List<SeSource> _playingSeList = new();

        private void Start()
        {
            _sePool = new ObjectPool<SeSource>(
                () => Instantiate(sePrefab, transform),
                source => source.gameObject.SetActive(true),
                source => source.gameObject.SetActive(false));
        }

        public SeSource PlaySe(string seName)
        {
            ManageVolume();

            var se = _sePool.Get();
            se.Volume = allVolume;
            se.Play(audioClips.First(value => value.name == seName));

            _playingSeList.Add(se);

            se.OnPlayFinish.Take(1).Subscribe(_ =>
            {
                _sePool.Release(se);
                _playingSeList.Remove(se);
            }).AddTo(this);

            return se;
        }

        private void ManageVolume()
        {
            foreach (var seSource in _playingSeList)
            {
                seSource.Volume /= 2f;
            }
        }
    }
}
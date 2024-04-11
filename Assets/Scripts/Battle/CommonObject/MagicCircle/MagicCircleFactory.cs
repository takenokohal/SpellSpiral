using Audio;
using Cysharp.Threading.Tasks;
using Databases;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace Battle.CommonObject.MagicCircle
{
    public class MagicCircleFactory : MonoBehaviour
    {
        [SerializeField] private MagicCircle prefab;

        [Inject] private readonly CharacterDatabase _characterDatabase;

        private ObjectPool<MagicCircle> _pool;

        private void Start()
        {
            _pool = new ObjectPool<MagicCircle>(
                () => Instantiate(prefab),
                magicCircle => magicCircle.gameObject.SetActive(true),
                magicCircle => magicCircle.gameObject.SetActive(false));
        }

        public async UniTask CreateAndWait(MagicCircleParameters magicCircleParameters)
        {
           AllAudioManager.PlaySe("MagicCircle");

            var magicCircle = _pool.Get();
            var data = _characterDatabase.Find(magicCircleParameters.CharacterKey);
            magicCircle.Init(data.MagicCircleSprite, magicCircleParameters.Color);

            var t = magicCircle.transform;
            t.localScale = Vector3.zero;
            t.DOScale(magicCircleParameters.Size, 0.2f);

            var timeCount = 0f;

            while (timeCount < data.ChantTime)
            {
                t.position = magicCircleParameters.Pos.Invoke();
                timeCount += Time.fixedDeltaTime;

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }

            UniTask.Void(async delegate
            {
                await t.DOScale(0f, 0.2f);
                _pool.Release(magicCircle);
            }, cancellationToken: destroyCancellationToken);
        }
    }
}
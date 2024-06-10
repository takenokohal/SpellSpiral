using Cysharp.Threading.Tasks;
using Databases;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace Battle.CommonObject.Bullet
{
    public class ReadyEffectFactory : MonoBehaviour
    {
        [SerializeField] private ReadyEffect beamEffectPrefab;
        [SerializeField] private ReadyEffect shootEffectPrefab;

        [Inject] private readonly CharacterDatabase _characterDatabase;

        private ObjectPool<ReadyEffect> _beamPool;
        private ObjectPool<ReadyEffect> _shootPool;

        private void Start()
        {
            _beamPool = new ObjectPool<ReadyEffect>(
                (() => Instantiate(beamEffectPrefab)),
                effect => effect.Activate(),
                effect => effect.Close());

            _shootPool = new ObjectPool<ReadyEffect>(
                (() => Instantiate(shootEffectPrefab)),
                effect => effect.Activate(),
                effect => effect.Close());
        }


        public async UniTask BeamCreateAndWait(ReadyEffectParameter readyEffectParameter)
        {
            var effect = _beamPool.Get();
            var data = _characterDatabase.Find(readyEffectParameter.CharacterKey);


            var timeCount = 0f;
            var t = effect.transform;

            while (timeCount < data.ChantTime)
            {
                t.position = readyEffectParameter.Position.Invoke();
                t.rotation = Quaternion.Euler(0, 0, readyEffectParameter.Rotation.Invoke());
                timeCount += Time.fixedDeltaTime;

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }

            _beamPool.Release(effect);
        }

        public async UniTask ShootCreateAndWait(ReadyEffectParameter readyEffectParameter)
        {
            var effect = _shootPool.Get();
            var data = _characterDatabase.Find(readyEffectParameter.CharacterKey);


            var timeCount = 0f;
            var t = effect.transform;

            while (timeCount < data.ChantTime)
            {
                t.position = readyEffectParameter.Position.Invoke();
                t.rotation = Quaternion.Euler(0, 0, readyEffectParameter.Rotation.Invoke());
                timeCount += Time.fixedDeltaTime;

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }

            _shootPool.Release(effect);
        }
    }
}
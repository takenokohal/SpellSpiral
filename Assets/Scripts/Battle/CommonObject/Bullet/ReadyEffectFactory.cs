using Cysharp.Threading.Tasks;
using Databases;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace Battle.CommonObject.Bullet
{
    public class ReadyEffectFactory : MonoBehaviour
    {
        [SerializeField] private ParticleSystem beamEffectPrefab;
        [SerializeField] private ParticleSystem shootEffectPrefab;

        [Inject] private readonly CharacterDatabase _characterDatabase;

        private ObjectPool<ParticleSystem> _beamPool;
        private ObjectPool<ParticleSystem> _shootPool;

        private void Start()
        {
            _beamPool = new ObjectPool<ParticleSystem>(
                (() => Instantiate(beamEffectPrefab)),
                effect => effect.gameObject.SetActive(true),
                effect => effect.gameObject.SetActive(false));

            _shootPool = new ObjectPool<ParticleSystem>(
                (() => Instantiate(shootEffectPrefab)),
                effect => effect.gameObject.SetActive(true),
                effect => effect.gameObject.SetActive(false));
        }


        public async UniTask BeamCreateAndWait(ReadyEffectParameter parameter)
        {
            var effect = _beamPool.Get();
            var data = _characterDatabase.Find(parameter.CharacterKey);


            var timeCount = 0f;
            var t = effect.transform;

            while (timeCount < data.ChantTime)
            {
                t.position = parameter.Position.Invoke();
                t.rotation = Quaternion.Euler(0, 0, parameter.Rotation.Invoke());
                timeCount += Time.fixedDeltaTime;

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }

            _beamPool.Release(effect);
        }

        public async UniTask ShootCreateAndWait(ReadyEffectParameter parameter)
        {
            var effect = _shootPool.Get();
            var data = _characterDatabase.Find(parameter.CharacterKey);


            var timeCount = 0f;
            var t = effect.transform;

            while (timeCount < data.ChantTime)
            {
                t.position = parameter.Position.Invoke();
                t.rotation = Quaternion.Euler(0, 0, parameter.Rotation.Invoke());
                t.localScale = parameter.Size * Vector3.one;
                timeCount += Time.fixedDeltaTime;

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }

            _shootPool.Release(effect);
        }
    }
}
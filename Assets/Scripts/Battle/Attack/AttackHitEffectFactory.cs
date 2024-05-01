using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace Battle.Attack
{
    public class AttackHitEffectFactory : MonoBehaviour
    {
        [SerializeField] private ParticleSystem effectPrefab;

        private ObjectPool<ParticleSystem> _objectPool;

        [SerializeField] private ParticleSystem servantDeadEffectPrefab;
        private ObjectPool<ParticleSystem> _deadEffectPool;

        private void Start()
        {
            _objectPool = new ObjectPool<ParticleSystem>(
                () => Instantiate(effectPrefab),
                target => target.gameObject.SetActive(true),
                target => target.gameObject.SetActive(false));

            _deadEffectPool = new ObjectPool<ParticleSystem>(
                () => Instantiate(servantDeadEffectPrefab),
                target => target.gameObject.SetActive(true),
                target => target.gameObject.SetActive(false));
        }


        public async UniTaskVoid Create(Vector3 position, Quaternion rotation)
        {
            var v = _objectPool.Get();
            var t = v.transform;
            t.position = position;
            t.rotation = rotation;

            await UniTask.Delay(5000, cancellationToken: destroyCancellationToken);

            _objectPool.Release(v);
        }

        public async UniTaskVoid CreateDeadEffect(Vector3 position, Quaternion rotation, float size)
        {
            var v = _deadEffectPool.Get();
            var t = v.transform;
            t.position = position;
            t.rotation = rotation;
            t.localScale = Vector3.one * 0.5f;

            v.Play();

            await UniTask.Delay(5000, cancellationToken: destroyCancellationToken);

            _deadEffectPool.Release(v);
        }
    }
}
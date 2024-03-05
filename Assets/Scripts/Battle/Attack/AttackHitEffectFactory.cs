using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace Battle.Attack
{
    public class AttackHitEffectFactory : MonoBehaviour
    {
        [SerializeField] private ParticleSystem effectPrefab;

        private ObjectPool<ParticleSystem> _objectPool;

        private void Start()
        {
            _objectPool = new ObjectPool<ParticleSystem>(
                () => Instantiate(effectPrefab),
                target => target.gameObject.SetActive(true),
                target => target.gameObject.SetActive(false));
        }


        public async void Create(Vector3 position, Quaternion rotation)
        {
            var v = _objectPool.Get();
            var t = v.transform;
            t.position = position;
            t.rotation = rotation;

            await UniTask.Delay(5000);

            _objectPool.Release(v);
        }
    }
}
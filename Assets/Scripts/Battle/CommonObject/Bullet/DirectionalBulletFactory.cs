/*using UniRx;
using UnityEngine;
using UnityEngine.Pool;

namespace Battle.CommonObject.Bullet
{
    public class DirectionalBulletFactory
    {
        private readonly DirectionalBullet _prefab;

        private ObjectPool<DirectionalBullet> _directionalPool;


        public DirectionalBulletFactory(DirectionalBullet prefab)
        {
            _prefab = prefab;

            Init();
        }

        private void Init()
        {
            _directionalPool = new ObjectPool<DirectionalBullet>(delegate
            {
                var instance = _prefab.CreateFromPrefab();
                instance.OnKill.Subscribe(_ => _directionalPool.Release(instance));
                return instance;
            });
        }

        public void Create(Vector2 pos, Vector2 velocity)
        {
            var v = _directionalPool.Get();
            v.Activate(pos, velocity);
        }
    }
}*/
using UniRx;
using UnityEngine.Pool;

namespace Battle.CommonObject.Bullet
{
    public class HomingBulletFactory
    {
        private readonly HomingBullet _prefab;

        private ObjectPool<HomingBullet> _homingPool;


        public HomingBulletFactory(HomingBullet prefab)
        {
            _prefab = prefab;

            Init();
        }

        private void Init()
        {
            _homingPool = new ObjectPool<HomingBullet>(delegate
            {
                var instance = _prefab.CreateFromPrefab();
                instance.OnKill.Subscribe(_ => _homingPool.Release(instance));
                return instance;
            });
        }

        public void Create(HomingBullet.Parameter parameter)
        {
            var v = _homingPool.Get();
            v.Activate(parameter);
        }
    }
}
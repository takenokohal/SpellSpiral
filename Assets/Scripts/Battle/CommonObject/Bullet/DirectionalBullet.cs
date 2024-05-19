using System;
using System.Threading;
using Audio;
using Battle.Attack;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Battle.CommonObject.Bullet
{
    public class DirectionalBullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private AttackHitController attackHitController;

        private readonly ReactiveProperty<bool> _isDead = new();

        //貫通
        [SerializeField] private bool pierce;


        public bool IsDead
        {
            get => _isDead.Value;
            private set => _isDead.Value = value;
        }

        public IObservable<bool> IsDeadObservable => _isDead.TakeUntilDestroy(this);

        public DirectionalBullet CreateFromPrefab(Vector2 pos, Vector2 velocity, float lifeTime = -1)
        {
            var instance = Instantiate(this);
            instance.Activate(pos, velocity, lifeTime);
            return instance;
        }


        public async UniTaskVoid Kill()
        {
            if (IsDead)
                return;

            _isDead.Value = true;
            rb.velocity /= 2f;

            await transform.DOScale(0, 0.1f).ToUniTask(cancellationToken: destroyCancellationToken);

            Destroy(gameObject);
        }

        private async UniTaskVoid AutoKill()
        {
            await UniTask.Delay(10000, cancellationToken: destroyCancellationToken);
            Kill().Forget();
        }

        private void Activate(Vector2 pos, Vector2 velocity, float lifeTime)
        {
            AllAudioManager.PlaySe("MagicShot");
            gameObject.SetActive(true);
            transform.position = pos;
            rb.velocity = velocity;

            AutoKill().Forget();
            if (!pierce)
                attackHitController.OnAttackHit
                    .Subscribe(_ => { Kill().Forget(); });

            if (lifeTime > 0f)
                LifeTime(lifeTime).Forget();
        }

        private async UniTaskVoid LifeTime(float lifeTime)
        {
            await UniTask.Delay((int)(lifeTime * 1000f), cancellationToken: destroyCancellationToken);

            Kill().Forget();
        }
    }
}
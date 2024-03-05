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

        public bool IsDead
        {
            get => _isDead.Value;
            private set => _isDead.Value = value;
        }

        public IObservable<bool> IsDeadObservable => _isDead.TakeUntilDestroy(this);

        public DirectionalBullet CreateFromPrefab(Vector2 pos, Vector2 velocity)
        {
            var instance = Instantiate(this);
            instance.Activate(pos, velocity);
            return instance;
        }


        public async UniTaskVoid Kill()
        {
            if (IsDead)
                return;

            _isDead.Value = true;

            await transform.DOScale(0, 0.5f);

            Destroy(gameObject);
        }

        private async UniTaskVoid AutoKill()
        {
            await UniTask.Delay(10000);
            Kill().Forget();
        }

        private void Activate(Vector2 pos, Vector2 velocity)
        {
            AudioManager.PlaySe("MagicShot");
            gameObject.SetActive(true);
            transform.position = pos;
            rb.velocity = velocity;

            AutoKill().Forget();
            attackHitController.OnAttackHit
                .Subscribe(_ => { Kill().Forget(); });
        }
    }
}
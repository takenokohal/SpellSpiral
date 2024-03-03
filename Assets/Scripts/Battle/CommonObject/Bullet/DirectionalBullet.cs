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
        private Rigidbody _rb;
        private AttackHitController _attackHitController;

        private readonly Subject<Unit> _onKill = new();
        public IObservable<Unit> OnKill => _onKill.TakeUntilDestroy(this);

        private CancellationTokenSource _autoKillTokenSource;

        public DirectionalBullet CreateFromPrefab()
        {
            var instance = Instantiate(this);
            instance.Init();
            return instance;
        }


        private void Init()
        {
            _rb = GetComponent<Rigidbody>();
            _attackHitController = GetComponent<AttackHitController>();
            
            _attackHitController.OnAttackHit
                .Subscribe(_ => { Kill().Forget(); });
        }

        public async UniTaskVoid Kill()
        {
            _autoKillTokenSource?.Cancel();
            _autoKillTokenSource = null;
            _onKill.OnNext(Unit.Default);

            await transform.DOScale(0, 0.5f);

            Destroy(gameObject);
        }

        private async UniTaskVoid AutoKill()
        {
            await UniTask.Delay(5000, cancellationToken: _autoKillTokenSource.Token);
            Kill().Forget();
        }

        public void Activate(Vector2 pos, Vector2 velocity)
        {
            AudioManager.PlaySe("MagicShot");
            _autoKillTokenSource = new CancellationTokenSource();
            gameObject.SetActive(true);
            transform.position = pos;
            _rb.velocity = velocity;

            AutoKill().Forget();
        }
    }
}
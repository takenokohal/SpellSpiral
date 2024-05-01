using System.Threading;
using Battle.Character.Enemy.Variables.Molle.Servant;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Molle
{
    public class SummonAssassinBoar : BossSequenceBase<MolleState>
    {
        [SerializeField] private AssassinServant boarPrefab;
        [SerializeField] private float moveDuration;

        [SerializeField] private int count;

        [SerializeField] private float magicCircleOffset;
        [SerializeField] private float coolTime;

        [SerializeField] private float recovery;

        [SerializeField] private float xPos;


        private bool _aiming;
        public override MolleState StateKey => MolleState.SummonAssassin;

        protected override async UniTask Sequence()
        {
            _aiming = true;

            var r = Random.Range(0, 2) > 0;
            var to = xPos;
            if (r)
                to *= -1;

            await TweenToUniTask(Parent.Rigidbody.DOMoveX(to, moveDuration));

            for (int i = 0; i < count; i++)
            {
                Shoot(i).Forget();
                await MyDelay(coolTime);
            }

            _aiming = false;
            await MyDelay(recovery);
        }

        private void FixedUpdate()
        {
            if (!_aiming)
                return;

            var rb = Parent.Rigidbody;
            var playerY = PlayerCore.Rigidbody.position.y;
            var position = rb.position;
            rb.position = Vector3.Lerp(position, new Vector3(position.x, playerY),
                moveDuration * Time.fixedDeltaTime);
        }

        private async UniTaskVoid Shoot(int i)
        {
            var tmpPos = CalcPos(i);
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1,
                () => tmpPos));

            ServantFactory.CreateAndInject(boarPrefab, Parent, tmpPos);
        }

        private Vector2 CalcPos(int i)
        {
            return Parent.transform.position +
                   (Vector3)GetDirectionToPlayer() * (1f + i * magicCircleOffset);
        }
    }
}
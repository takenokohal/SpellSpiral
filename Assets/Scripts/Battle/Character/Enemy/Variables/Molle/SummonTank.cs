using Battle.Character.Servant;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Molle
{
    public class SummonTank : BossSequenceBase<MolleState>
    {
        [SerializeField] private ServantBase servantPrefab;

        [SerializeField] private float moveSpeed;

        [SerializeField] private float summonRadius;
        [SerializeField] private int summonCount;
        [SerializeField] private float summonDuration;

        [SerializeField] private float recoveryTime;
        

        public override MolleState StateKey => MolleState.SummonTank;

        protected override async UniTask Sequence()
        {
            var to = Random.insideUnitCircle * new Vector2(5, 2);
            await TweenToUniTask(Parent.Rigidbody.DOMove(to, moveSpeed).SetSpeedBased());

            for (int i = 0; i < summonCount; i++)
            {
                Summon(i).Forget();
                await MyDelay(summonDuration / summonCount);
            }

            await MyDelay(recoveryTime);
        }

        private async UniTaskVoid Summon(int i)
        {
            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(Parent, 1, () => CalcPos(i)));

            CharacterFactory.CreateAndInject(servantPrefab, Parent, CalcPos(i));
        }

        private Vector2 CalcPos(int i)
        {
            var angle = Mathf.PI * 2 / summonCount * i;
            var offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * summonRadius;

            return Parent.transform.position + offset;
        }
    }
}
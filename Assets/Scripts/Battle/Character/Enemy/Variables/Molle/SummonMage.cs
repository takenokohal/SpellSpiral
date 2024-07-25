using Battle.Character.Servant;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Molle
{
    public class SummonMage : BossSequenceBase<MolleState>
    {
        [SerializeField] private string servantPrefab;

        [SerializeField] private Vector2 corner;

        [SerializeField] private float moveDuration;

        [SerializeField] private float recoveryTime;


        public override MolleState StateKey => MolleState.SummonMage;

        protected override async UniTask Sequence()
        {
            var r = await Move();

            Summon(new Vector2(0, 0)).Forget();
            await MyDelay(0.2f);
            Summon(new Vector2(-r.x, 0)).Forget();
            await MyDelay(0.2f);
            Summon(new Vector2(0, -r.y)).Forget();


            await MyDelay(recoveryTime);
        }

        private async UniTask<Vector2Int> Move()
        {
            var rX = Random.Range(0, 2) > 0 ? 1 : -1;
            var rY = Random.Range(0, 2) > 0 ? 1 : -1;
            var r = new Vector2Int(rX, rY);

            var toPos = corner * r;

            await TweenToUniTask(Parent.Rigidbody.DOMove(toPos, moveDuration));

            return r;
        }

        private async UniTaskVoid Summon(Vector2 offset)
        {
            var tmpPos = (Vector2)Parent.Rigidbody.position + offset;
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1,
                () => tmpPos));

            var servant = CharacterFactory.CreateAndInject(servantPrefab, Parent, tmpPos);
        }
    }
}
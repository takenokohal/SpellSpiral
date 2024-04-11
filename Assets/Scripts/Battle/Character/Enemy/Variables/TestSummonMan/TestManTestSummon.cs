using Battle.Attack;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.TestSummonMan
{
    public class TestManTestSummon : BossSequenceBase<TestSummonManController.State>
    {
        [SerializeField] private string servantKey;

        public override TestSummonManController.State StateKey => TestSummonManController.State.Summon;

        protected override async UniTask Sequence()
        {
            Parent.Rigidbody.velocity = Random.insideUnitCircle * 5;
            
            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(CharacterKey,
                    Color.white, 1, () => Parent.transform.position));

        var v=    ServantFactory.Create(servantKey);
        v.transform.position = Parent.transform.position;
        }
    }
}
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class SummonServant : BossSequenceBase<BalteciaState>
    {
        public override BalteciaState StateKey => BalteciaState.SummonServant;

        protected override UniTask Sequence()
        {
            for (int i = 0; i < 2; i++)
            {
                Summon(i).Forget();
            }

            return MyDelay(2f);
        }

        private async UniTaskVoid Summon(int i)
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(
                CharacterKey,
                Color.red, 1,
                () => CalcPos(i)));

            var servant = ServantFactory.Create("MiniDragon");
            servant.transform.position = CalcPos(i);
            servant.transform.SetParent(Parent.transform);
        }

        private Vector2 CalcPos(int i)
        {
            return Parent.transform.position + new Vector3(-Parent.CharacterRotation.Rotation * 2, 1);
        }
    }
}
using System.Linq;
using Battle.Attack;
using Battle.Character;
using Battle.Character.Enemy;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Battle.PlayerSpell.Variables
{
    public class FireBall : SpellBase
    {
        [SerializeField] private DirectionalBullet directionalBullet;

        [SerializeField] private float bulletSpeed;

        [SerializeField] private int howMany;
        [SerializeField] private float magicCircleOffset;
        [SerializeField] private float coolTime;


        protected override async UniTaskVoid Init()
        {
            var target = AllCharacterManager.AllCharacters
                .Where(value=> value.GetOwnerType()== OwnerType.Enemy)
                .OrderBy(value => Vector3.Distance(value.transform.position, PlayerCore.transform.position)).First();
            for (int i = 0; i < howMany; i++)
            {
                Shoot(target, i).Forget();
                await MyDelay(coolTime);
            }

            await MyDelay(1f);
            Destroy(gameObject);
        }

        private async UniTaskVoid Shoot(CharacterBase target, int i)
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(CharacterKey, Color.white, 1,
                () => CalcPos(target, i)));

            var pos = (Vector3)CalcPos(target, i);

            var velocity = (target.transform.position - pos).normalized * bulletSpeed;
            directionalBullet.CreateFromPrefab(pos, velocity);
        }

        private Vector2 CalcPos(CharacterBase target, int i)
        {
            return PlayerCore.transform.position + (Vector3)GetDirectionPlayerToCharacter(target) * (1f + i * magicCircleOffset);
        }
    }
}
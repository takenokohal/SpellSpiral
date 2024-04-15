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
    public class Cracker : SpellBase
    {
        [SerializeField] private DirectionalBullet directionalBullet;

        [SerializeField] private float bulletSpeed;

        [SerializeField] private int howMany;

        [SerializeField] private float arcPerShot = 10f;

        [SerializeField] private float magicCircleOffset;


        protected override async UniTaskVoid Init()
        {
            var target = AllCharacterManager.AllCharacters
                .Where(value => value.GetOwnerType() == OwnerType.Enemy)
                .OrderBy(value => value.CurrentLife)
                .ThenBy(value => Vector3.Distance(value.transform.position, PlayerCore.transform.position)).First();

            var currentArc = 0f;
            for (int i = 0; i < howMany; i++)
            {
                Shoot(target, i, currentArc).Forget();

                currentArc += arcPerShot;
            }


            await MyDelay(1f);
            Destroy(gameObject);
        }

        private async UniTaskVoid Shoot(CharacterBase target, int i, float currentArc)
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(CharacterKey, Color.white, 1,
                () => CalcPos(target, i, currentArc)));

            var pos = (Vector3)CalcPos(target, i, currentArc);

            var velocity = CalcDir(target, i, currentArc) * bulletSpeed;
            directionalBullet.CreateFromPrefab(pos, velocity);
        }

        private Vector2 CalcPos(CharacterBase target, int i, float currentArc)
        {
            var dir = CalcDir(target, i, currentArc);
            return (Vector2)PlayerCore.transform.position + dir * magicCircleOffset;
        }

        private Vector2 CalcDir(CharacterBase target, int i, float currentArc)
        {
            var shootArc = arcPerShot * howMany;
            var dir1 = GetDirectionPlayerToCharacter(target);
            var dir2 = Quaternion.Euler(0, 0, -shootArc / 2f) * dir1;
            var dir3 = Quaternion.Euler(0, 0, currentArc) * dir2;
            return dir3;
        }
    }
}
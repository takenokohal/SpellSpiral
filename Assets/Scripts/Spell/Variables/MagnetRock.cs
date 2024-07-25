using System.Linq;
using Battle.Attack;
using Battle.Character;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Spell.Variables
{
    public class MagnetRock : SpellBase
    {
        [SerializeField] private DirectionalBullet directionalBullet;

        [SerializeField] private float firstSpeed;


        protected override async UniTaskVoid Init()
        {
            var target = AllCharacterManager.AllCharacters
                .Where(value => value.GetOwnerType() == OwnerType.Enemy)
                .OrderBy(value => value.CurrentLife)
                .ThenBy(value =>
                    Vector3.Distance(value.transform.position, PlayerCore.transform.position)).First();

            Shoot(target).Forget();


            await MyDelay(1f);
            Destroy(gameObject);
        }

        private async UniTask Shoot(CharacterBase target)
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(PlayerCore, 2f,
                () => CalcPos(target)));

            directionalBullet.CreateFromPrefab(CalcPos(target), CalcDir(target) * firstSpeed);
        }


        private Vector2 CalcDir(CharacterBase target)
        {
            var v1 = GetDirectionPlayerToCharacter(target);
            return v1;
        }

        private Vector2 CalcPos(CharacterBase target)
        {
            var v3 = CalcDir(target);
            var pos = (Vector2)PlayerCore.transform.position + v3 * 0.5f;
            return pos;
        }
    }
}
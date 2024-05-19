﻿using System.Linq;
using Battle.Character;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.PlayerSpell.Variables
{
    public class GatlingShoot : SpellBase
    {
        [SerializeField] private DirectionalBullet directionalBullet;

        [SerializeField] private float bulletSpeed;

        [SerializeField] private int howMany;
        [SerializeField] private float magicCircleOffset;
        [SerializeField] private float duration;


        protected override async UniTaskVoid Init()
        {
            var target = AllCharacterManager.GetEnemyCharacters()
                .OrderBy(value => value.CurrentLife)
                .ThenBy(value => Vector3.Distance(value.transform.position, PlayerCore.transform.position)).First();
            for (int i = 0; i < howMany; i++)
            {
                Shoot(target, i).Forget();
                await MyDelay(duration / howMany);
            }

            await MyDelay(1f);
            Destroy(gameObject);
        }

        private async UniTaskVoid Shoot(CharacterBase target, int i)
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(PlayerCore, 1,
                () => CalcPos(target, i)));

            var pos = (Vector3)CalcPos(target, i);

            var velocity = (target.transform.position - pos).normalized * bulletSpeed;
            directionalBullet.CreateFromPrefab(pos, velocity);
        }

        private Vector2 CalcPos(CharacterBase target, int i)
        {
            var theta = 2 * Mathf.PI / howMany;
            theta *= i;
            var offset = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta));
            offset *= magicCircleOffset;
            return PlayerCore.transform.position + offset;
        }
    }
}
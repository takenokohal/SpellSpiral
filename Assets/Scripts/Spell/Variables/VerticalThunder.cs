﻿using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Spell.Variables
{
    public class VerticalThunder : SpellBase
    {
        [SerializeField] private SingleHitBeam singleHitBeam;

        protected override async UniTaskVoid Init()
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(PlayerCore, 1f,
                CalcPos));


            singleHitBeam.SetDirection(new Vector2(PlayerCore.CharacterRotation.Rotation, 0));
            singleHitBeam.SetPosition(CalcPos());
            singleHitBeam.Activate(0.2f).Forget();

            await MyDelay(5f);

            Destroy(gameObject);
        }

        private Vector2 CalcPos()
        {
            var pos = PlayerCore.transform.position;

            return pos;
        }
    }
}
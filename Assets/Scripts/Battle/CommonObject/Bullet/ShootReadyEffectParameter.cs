/*using System;
using Battle.Character;
using UnityEngine;

namespace Battle.CommonObject.Bullet
{
    public class ShootReadyEffectParameter
    {
        public string CharacterKey { get; }
        public Func<Vector2> Position { get; }
        public Func<float> Rotation { get; }

        public float Size { get; }

        public ShootReadyEffectParameter(CharacterBase characterBase, Func<Vector2> position, Func<float> rotation,
            float size)
        {
            CharacterKey = characterBase.CharacterKey;
            Position = position;
            Rotation = rotation;
            Size = size;
        }

        public ShootReadyEffectParameter(
            CharacterBase characterBase,
            Func<Vector2> position,
            float size,
            Func<Vector2> direction)
        {
            Position = position;
            Rotation = () => Mathf.Atan2(direction().y, direction().x) * Mathf.Rad2Deg;
            Size = size;
            CharacterKey = characterBase.CharacterKey;
        }
    }
}*/
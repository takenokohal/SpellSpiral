using System;
using Battle.Character;
using Battle.PlayerSpell;
using UnityEngine;

namespace Battle.CommonObject.MagicCircle
{
    public class MagicCircleParameters
    {
        public string CharacterKey { get; }
        public float Size { get; }

        public Func<Vector2> Pos { get; }


        public MagicCircleParameters(CharacterBase character, float size,
            Func<Vector2> pos)
        {
            CharacterKey = character.CharacterKey;
            Size = size;
            Pos = pos;
        }
    }
}
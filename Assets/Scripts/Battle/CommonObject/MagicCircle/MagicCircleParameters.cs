using System;
using Battle.Character;
using UnityEngine;

namespace Battle.CommonObject.MagicCircle
{
    public class MagicCircleParameters
    {
        public CharacterKey CharacterKey { get; }
        public Color Color { get; }
        public float Size { get; }

        public Func<Vector2> Pos { get; }

        public MagicCircleParameters(CharacterKey characterKey, Color color, float size,
            Func<Vector2> pos)
        {
            CharacterKey = characterKey;
            Color = color;
            Size = size;
            Pos = pos;
        }
    }
}
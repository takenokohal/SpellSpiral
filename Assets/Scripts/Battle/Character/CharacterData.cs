using System;
using UnityEngine;

namespace Battle.Character
{
    [Serializable]
    public class CharacterData
    {
        [SerializeField] private string characterKey;

        public string CharacterKey => characterKey;

        [SerializeField] private int life;

        public int Life => life;

        //EditorSetting
        [SerializeField] private Sprite magicCircleSprite;
        public Sprite MagicCircleSprite => magicCircleSprite;

        public float ChantTime => CharacterKey == "Player" ? 0.2f : 1f;
    }
}
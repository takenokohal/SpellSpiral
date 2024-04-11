using System;
using Battle.Attack;
using UnityEngine;

namespace Battle.Character
{
    [Serializable]
    public class CharacterData
    {
        public CharacterData(string characterKey, CharacterType characterType, OwnerType ownerType, int life, Sprite magicCircleSprite)
        {
            this.characterKey = characterKey;
            this.characterType = characterType;
            this.ownerType = ownerType;
            this.life = life;
            this.magicCircleSprite = magicCircleSprite;
        }
        [SerializeField] private string characterKey;

        public string CharacterKey => characterKey;

        [SerializeField] private CharacterType characterType;
        public CharacterType CharacterType => characterType;

        [SerializeField] private OwnerType ownerType;

        public OwnerType OwnerType => ownerType;

        [SerializeField] private int life;

        public int Life => life;

        //EditorSetting
        [SerializeField] private Sprite magicCircleSprite;
        public Sprite MagicCircleSprite => magicCircleSprite;

        public float ChantTime => CharacterKey == "Player" ? 0.2f : 1f;
    }
}
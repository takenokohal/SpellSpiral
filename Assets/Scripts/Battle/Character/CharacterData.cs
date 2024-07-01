using System;
using Battle.Attack;
using UnityEngine;

namespace Battle.Character
{
    [Serializable]
    public class CharacterData
    {
        public CharacterData(string characterKey, string characterNameJp, string characterNameEn,
            CharacterType characterType, string masterName, OwnerType ownerType, int life, CharacterBase characterBase,
            Sprite magicCircleSprite, GameObject stageObject)
        {
            this.characterKey = characterKey;
            this.characterNameJp = characterNameJp;
            this.characterNameEn = characterNameEn;
            this.characterType = characterType;
            this.masterName = masterName;
            this.ownerType = ownerType;
            this.life = life;
            this.characterBase = characterBase;
            this.magicCircleSprite = magicCircleSprite;
            this.stageObject = stageObject;
        }

        [SerializeField] private string characterKey;

        public string CharacterKey => characterKey;

        [SerializeField] private string characterNameJp;

        public string CharacterNameJp => characterNameJp;

        [SerializeField] private string characterNameEn;

        public string CharacterNameEn => characterNameEn;

        [SerializeField] private CharacterType characterType;
        public CharacterType CharacterType => characterType;

        [SerializeField] private string masterName;
        public string MasterName => masterName;

        [SerializeField] private OwnerType ownerType;

        public OwnerType OwnerType => ownerType;

        [SerializeField] private int life;

        public int Life => life;

        [SerializeField] private CharacterBase characterBase;
        public CharacterBase CharacterBase => characterBase;

        [SerializeField] private Sprite magicCircleSprite;
        public Sprite MagicCircleSprite => magicCircleSprite;

        [SerializeField] private GameObject stageObject;

        public GameObject StageObject => stageObject;

        public float ChantTime => CharacterKey == "Player" ? 0.2f : 1f;
    }
}
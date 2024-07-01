using Battle.Character;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NewHomeScene
{
    public class MissionSelectItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Image line;

        
        public CharacterData CharacterData { get; private set; }

        public void Init(CharacterData characterData)
        {
            CharacterData = characterData;
        }
        public string Text
        {
            get => nameText.text;
            set => nameText.text = value;
        }

        public void SetLineSize(float value)
        {
            line.transform.DOScaleX(value, 0.2f);
        }
    }
}
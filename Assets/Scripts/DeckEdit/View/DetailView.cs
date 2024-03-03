using Battle.PlayerSpell;
using Databases;
using DeckEdit.Model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace DeckEdit.View
{
    public class DetailView : MonoBehaviour
    {
        [Inject] private readonly CurrentSelectedSpell _currentSelectedSpell;
        [Inject] private readonly SpellColorPalette _spellColorPalette;

        [SerializeField] private Image spellIconImage;
        [SerializeField] private Image spellColorImage;

        [SerializeField] private TMP_Text spellNameText;
        [SerializeField] private TMP_Text manaCostText;

        [SerializeField] private TMP_Text descriptionText;


        private void Start()
        {
            _currentSelectedSpell.OnSpellChanged().Subscribe(OnSpellChanged);
        }

        private void OnSpellChanged(SpellData spellData)
        {
            spellIconImage.sprite = spellData.SpellIcon;
            spellColorImage.color = _spellColorPalette.GetColor(spellData.SpellAttribute);

            spellNameText.text = spellData.SpellName;
            manaCostText.text = "Cost " + spellData.ManaCost;

            descriptionText.text = spellData.SpellDescription;
        }
    }
}
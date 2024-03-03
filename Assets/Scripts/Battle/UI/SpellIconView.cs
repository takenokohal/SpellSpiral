using System.Collections.Generic;
using Battle.Character.Player;
using Battle.Character.Player.Deck;
using Battle.PlayerSpell;
using Cysharp.Threading.Tasks;
using Databases;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Battle.UI
{
    public class SpellIconView : SerializedMonoBehaviour
    {
        [Inject] private readonly SpellDatabase _spellDatabase;
        [Inject] private readonly SpellColorPalette _spellColorPalette;
        [Inject] private readonly PlayerChant _playerChant;

        [Inject] private readonly BattleDeck _battleDeck;
        [Inject] private readonly PlayerCore _playerCore;

        [OdinSerialize] private readonly Dictionary<SpellSlot, Image> _iconImages = new();
        [OdinSerialize] private readonly Dictionary<SpellSlot, Image> _iconBackGround = new();

        [SerializeField] private Image successAnimation;

        [SerializeField] private TMP_Text deckCount;


        private void Start()
        {
            _playerChant.CurrentSpells.ObserveReplace().Subscribe(value =>
            {
                var data = _spellDatabase.Find(value.NewValue);

                _iconImages[value.Key].sprite = data.SpellIcon;
             //   _iconBackGround[value.Key].color = data.SpellIconColor;
            }).AddTo(this);

            _playerChant.CurrentSpells.ObserveAdd().Subscribe(value =>
            {
                var data = _spellDatabase.Find(value.Value);

                _iconImages[value.Key].sprite = data.SpellIcon;
             //   _iconBackGround[value.Key].color = data.SpellIconColor;
            }).AddTo(this);

            _playerChant.OnChantSuccess.Subscribe(value =>
            {
                UniTask.Void(async delegate
                {
                    var backGround = _iconBackGround[value];
                    successAnimation.transform.position = backGround.transform.position;
                    successAnimation.color = backGround.color;

                    successAnimation.gameObject.SetActive(true);

                    successAnimation.transform.localScale = Vector3.one;
                    successAnimation.transform.DOScale(2.5f, 0.5f);
                    successAnimation.DOFade(1, 0);

                    await successAnimation.DOFade(0, 0.5f);
                    successAnimation.gameObject.SetActive(false);
                });
            }).AddTo(this);

            _battleDeck.OnDraw.Subscribe(_ => { deckCount.text = _battleDeck.GetCount().ToString(); }).AddTo(this);
        }

        private void Update()
        {
            ManageColor();
        }

        private void ManageColor()
        {
            var playerMana = _playerCore.PlayerParameter.Mana;
            foreach (var playerChantCurrentSpell in _playerChant.CurrentSpells)
            {
                var data = _spellDatabase.Find(playerChantCurrentSpell.Value);
                var color = _spellColorPalette.GetColor(data.SpellAttribute);

                Color.RGBToHSV(color, out var h, out var s, out var v);

                if (data.ManaCost > playerMana)
                    color = Color.HSVToRGB(h, s, 0.2f);

                var img = _iconBackGround[playerChantCurrentSpell.Key];
                img.color = Color.Lerp(img.color, color, 0.2f);
            }
            
        }
    }
}
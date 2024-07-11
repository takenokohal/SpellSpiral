using System.Collections.Generic;
using System.Linq;
using Databases;
using DeckEdit.SaveData;
using DeckEdit.View;
using DG.Tweening;
using Others.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

namespace NewDeckEdit.Test
{
    public class CardPoolTest : MonoBehaviour
    {
        [SerializeField] private SpellIconView spellIconPrefab;

        private readonly List<SpellIconView> _instancedIcons = new();
        [Inject] private readonly SpellDatabase _spellDatabase;
        [Inject] private readonly SpellColorPalette _colorPalette;

        [SerializeField] private ScrollRect scrollRect;


        private void Start()
        {
            var list = _spellDatabase.SpellDictionary
                .OrderBy(value => value.Value.SpellAttribute)
                .Select(value => value.Value).ToList();

            foreach (var spellData in list)
            {
                var instance = Instantiate(spellIconPrefab, transform);
                instance.gameObject.SetActive(true);
                _instancedIcons.Add(instance);


                var color = _colorPalette.GetColor(spellData.SpellAttribute);

                instance.SetColor(color);
                instance.SetIcon(spellData.SpellIcon);
            }
        }

        private void Update()
        {
            var up = Keyboard.current.upArrowKey.wasPressedThisFrame;
            var down = Keyboard.current.downArrowKey.wasPressedThisFrame;

            if (up)
                transform.DOLocalMoveY(120, 0.2f).SetRelative();
            
            if (down)
                transform.DOLocalMoveY(-120, 0.2f).SetRelative();
        }
    }
}
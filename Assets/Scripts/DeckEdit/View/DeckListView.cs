using System.Collections.Generic;
using System.Linq;
using Battle.PlayerSpell;
using Databases;
using DeckEdit.Model;
using UniRx;
using UnityEngine;
using VContainer;
using SpellKey = DeckEdit.Model.SpellKey;

namespace DeckEdit.View
{
    public class DeckListView : MonoBehaviour
    {
        [SerializeField] private Transform contentsParent;
        [SerializeField] private DeckListIconView deckListIconViewPrefab;

        [Inject] private readonly DeckList _deckList;
        [Inject] private readonly SpellDatabase _spellDatabase;
        [Inject] private readonly SpellColorPalette _spellColorPalette;
        [Inject] private readonly CurrentSelectedSpell _currentSelectedSpell;

        private readonly Dictionary<SpellKey, DeckListIconView> _iconDictionary = new();
        public IReadOnlyDictionary<SpellKey, DeckListIconView> IconDictionary => _iconDictionary;

        private void Start()
        {
            InitSaveData();

            _deckList.OnAdd.Subscribe(AddIcon);
            _deckList.OnRemove.Subscribe(RemoveIcon);
            _deckList.OnSort.Subscribe(_ => Sort());
        }

        private void InitSaveData()
        {
            foreach (var spellKey in _deckList.CurrentDeckList)
            {
                AddIcon(spellKey);
            }
        }


        private void AddIcon(SpellKey spellKey)
        {
            var data = _spellDatabase.Find(spellKey.Key);
            var color = _spellColorPalette.GetColor(data.SpellAttribute);

            var instance = Instantiate(deckListIconViewPrefab, contentsParent);
            instance.SetColor(color);
            instance.SetIcon(data.SpellIcon);
            instance.OnRightClick.Subscribe(_ => _deckList.Remove(spellKey));
            instance.OnMouseEnter.Subscribe(_ => _currentSelectedSpell.SetSelectData(data));

            _iconDictionary.Add(spellKey, instance);
        }

        private void RemoveIcon(SpellKey spellKey)
        {
            _iconDictionary.Remove(spellKey, out var instance);

            Destroy(instance.gameObject);
        }

        private void Sort()
        {
            var sortedList = _deckList.CurrentDeckList;
            for (var i = 0; i < sortedList.Count; i++)
            {
                var key = sortedList[i];
                _iconDictionary[key].transform.SetSiblingIndex(i);
            }
        }
    }
}
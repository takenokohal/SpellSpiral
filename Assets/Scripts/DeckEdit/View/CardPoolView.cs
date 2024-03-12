using System.Collections.Generic;
using Databases;
using DeckEdit.Model;
using UniRx;
using UnityEngine;
using VContainer;
using SpellKey = DeckEdit.Model.SpellKey;

namespace DeckEdit.View
{
    public class CardPoolView : MonoBehaviour
    {
        [Inject] private readonly CardPool _cardPool;
        [Inject] private readonly SpellDatabase _spellDatabase;
        [Inject] private readonly SpellColorPalette _spellColorPalette;
        [Inject] private readonly DeckList _deckList;
        [Inject] private readonly CurrentSelectedSpell _currentSelectedSpell;

        [SerializeField] private Transform contentsParent;
        [SerializeField] private CardPoolDataView cardPoolDataViewPrefab;


        private readonly List<CardPoolDataView> _instances = new();
        public IReadOnlyList<CardPoolDataView> Instances => _instances;

        private void Start()
        {
            _cardPool.OnChange().Subscribe(OnListChange);
        }

        private void OnListChange(IReadOnlyList<string> list)
        {
            foreach (var cardPoolDataView in _instances)
            {
                Destroy(cardPoolDataView.gameObject);
            }

            _instances.Clear();

            foreach (var s in list)
            {
                CreateDataView(s);
            }
        }

        private void CreateDataView(string key)
        {
            var instance = Instantiate(cardPoolDataViewPrefab, contentsParent);

            var data = _spellDatabase.Find(key);
            instance.SetColor(_spellColorPalette.GetColor(data.SpellAttribute));
            instance.SetIcon(data.SpellIcon);

            instance.OnRightClick.Subscribe(_ => _deckList.Add(new SpellKey(key)));
            instance.OnMouseEnter.Subscribe(_ => _currentSelectedSpell.SetSelectData(data));

            _instances.Add(instance);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Battle.PlayerSpell;
using Databases;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace DeckEdit.Model
{
    public class DeckList : IInitializable ,IDisposable
    {
        [Inject] private readonly SpellDatabase _spellDatabase;
        [Inject] private readonly IDeckSaveDataPresenter _deckSaveDataPresenter;
        private List<SpellKey> _currentDeckList = new();
        public IReadOnlyList<SpellKey> CurrentDeckList => _currentDeckList;

        private readonly Subject<SpellKey> _onAdd = new();
        private readonly Subject<SpellKey> _onRemove = new();
        private readonly Subject<Unit> _onSort = new();

        public IObservable<SpellKey> OnAdd => _onAdd;

        public IObservable<SpellKey> OnRemove => _onRemove;
        public IObservable<Unit> OnSort => _onSort;


        public void Add(SpellKey spellKey)
        {
            if (CurrentDeckList.Count(value => value.Key == spellKey.Key) >= 3)
                return;

            if (CurrentDeckList.Count >= 20)
                return;

            _currentDeckList.Add(spellKey);
            _onAdd.OnNext(spellKey);
            Sort();
        }

        public void Remove(SpellKey spellKey)
        {
            _currentDeckList.Remove(spellKey);
            _onRemove.OnNext(spellKey);
        }

        public void Save()
        {
            _deckSaveDataPresenter.SaveDeck(_currentDeckList.Select(value => value.Key).ToList());
        }

        private void Sort()
        {
            _currentDeckList = _currentDeckList
                .Select(value => (Key: value, data: _spellDatabase.Find(value.Key)))
                .OrderBy(value => value.data.SpellAttribute)
                .ThenBy(value => value.data.ManaCost)
                .ThenBy(value => value.data.SpellKey)
                .Select(value => value.Key)
                .ToList();

            _onSort.OnNext(Unit.Default);
        }

        public void Initialize()
        {
            var saveData = _deckSaveDataPresenter.LoadDeck() ?? new List<string>();

            _currentDeckList.AddRange(saveData.Select(value => new SpellKey(value)));
        }

        public void Dispose()
        {
            _onAdd?.Dispose();
            _onRemove?.Dispose();
            _onSort?.Dispose();
        }
    }
}
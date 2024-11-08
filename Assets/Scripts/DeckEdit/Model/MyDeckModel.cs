﻿using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using DeckEdit.Controller;
using UniRx;
using UnityEngine;
using VContainer;

namespace DeckEdit.Model
{
    public class MyDeckModel : IDisposable
    {
        [Inject] private readonly SpellDatabase _spellDatabase;
        [Inject] private readonly IDeckSaveDataPresenter _deckSaveDataPresenter;
        private List<SpellKey> _currentDeckList = new();
        public IReadOnlyList<SpellKey> CurrentDeckList => _currentDeckList;


        private readonly ReactiveProperty<SpellKey> _currentHighlanderSpell = new();

        public SpellKey CurrentHighlanderSpell
        {
            get => _currentHighlanderSpell.Value;
            set => _currentHighlanderSpell.Value = value;
        }

        public IObservable<SpellKey> HighlanderObservable => _currentHighlanderSpell.Where(value => value != null);


        public const int MaxCount = 20;

        private readonly Subject<Unit> _onUpdate = new();
        public IObservable<Unit> OnUpdate => _onUpdate;

        public bool IsFilled => CurrentDeckList.Count >= MaxCount;

        public bool IsChanged { get; private set; }

        public void Add(SpellKey spellKey)
        {
            if (CurrentDeckList.Count(value => value.Key == spellKey.Key) >= 3)
                return;

            if (IsFilled)
                return;

            _currentDeckList.Add(spellKey);
            Sort();

            _onUpdate.OnNext(Unit.Default);
            IsChanged = true;
        }

        public void Remove(SpellKey spellKey)
        {
            _currentDeckList.Remove(spellKey);

            _onUpdate.OnNext(Unit.Default);
            IsChanged = true;
        }

        public void Clear()
        {
            _currentDeckList.Clear();
            IsChanged = true;
        }

        public void SetDeckData(DeckData deckData)
        {
            _currentDeckList.Clear();
            _currentDeckList.AddRange(
                deckData.normalSpellDeck
                    .OrderBy(value => _spellDatabase.Find(value).SpellAttribute)
                    .Select(value => new SpellKey(value)));

            CurrentHighlanderSpell = new SpellKey(deckData.highlanderSpell);

            _onUpdate.OnNext(Unit.Default);
        }


        public void Save()
        {
            _deckSaveDataPresenter.SaveDeck(new DeckData(_currentDeckList.Select(value => value.Key).ToList(),
                CurrentHighlanderSpell.Key));
            IsChanged = false;
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
        }


        public void Dispose()
        {
            _onUpdate?.Dispose();
        }
    }
}
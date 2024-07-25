using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Others;
using Spell;
using UniRx;
using UnityEngine;
using VContainer;

namespace DeckEdit.Model
{
    public class CardPoolModel : IDisposable
    {
        [Inject] private readonly SpellDatabase _spellDatabase;
        private List<SpellKey> _currentSortedCardPoolList = new();
        public IReadOnlyList<SpellKey> CurrentSortedCardPoolList => _currentSortedCardPoolList;

        private readonly Subject<Unit> _onUpdate = new();
        public IObservable<Unit> OnUpdate => _onUpdate;


        public enum SortType
        {
            Attribute,
            Name,
            ManaCost
        }

        public void ResetAndSort(SortType sortType)
        {
            switch (sortType)
            {
                case SortType.Attribute:
                    SortByAttribute();
                    break;
                case SortType.Name:
                    break;
                case SortType.ManaCost:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortType), sortType, null);
            }

            _onUpdate.OnNext(Unit.Default);
        }

        private void SortByAttribute()
        {
            _currentSortedCardPoolList = _spellDatabase.SpellDictionary
                .Where(value => !value.Value.IsNotImp())
                .Where(value => value.Value.SpellAttribute != SpellAttribute.Highlander)
                .Select(value => value.Value)
                .OrderBy(value => value.SpellAttribute)
                .ThenBy(value => value.ManaCost)
                .ThenBy(value => value.SpellKey)
                .Select(value => new SpellKey(value.SpellKey))
                .ToList();
        }


        public void Dispose()
        {
            _onUpdate?.Dispose();
        }
    }
}
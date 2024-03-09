using System;
using System.Collections.Generic;
using System.Linq;
using Battle.PlayerSpell;
using Databases;
using UniRx;
using VContainer;

namespace DeckEdit.Model
{
    public class CardPool: IDisposable
    {
        [Inject] private readonly SpellDatabase _spellDatabase;

        private readonly ReactiveProperty<IReadOnlyList<string>> _currentCardPool = new();
        public IReadOnlyList<string> CurrentCardPool => _currentCardPool.Value;
        public IObservable<IReadOnlyList<string>> OnChange() => _currentCardPool.Where(value => value != null);

        public void Init()
        {
            _currentCardPool.Value = _spellDatabase.SpellDictionary
                .Where(value=> !value.Value.IsNotImp())
                .OrderBy(value => value.Value.SpellAttribute)
                .Select(value => value.Key).ToList();
        }

        public void Sort()
        {
        }

        public void Dispose()
        {
            _currentCardPool?.Dispose();
        }
    }
}
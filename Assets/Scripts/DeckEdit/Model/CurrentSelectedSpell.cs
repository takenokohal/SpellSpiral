using System;
using Battle.PlayerSpell;
using UniRx;

namespace DeckEdit.Model
{
    public class CurrentSelectedSpell : IDisposable
    {
        private readonly ReactiveProperty<SpellData> _currentSelectedSpellData = new();
        public SpellData SetSelectData(SpellData spellData) => _currentSelectedSpellData.Value = spellData;

        public IObservable<SpellData> OnSpellChanged() => _currentSelectedSpellData.Where(value => value != null);

        public void Dispose()
        {
            _currentSelectedSpellData?.Dispose();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Battle.Character.Player.Deck;
using Databases;
using DeckEdit.Model;
using VContainer;

namespace Test
{
    public class DeckPresenter : BattleDeck.IDeckPresenter
    {
        [Inject] private readonly IDeckSaveDataPresenter _deckSaveDataPresenter;

        public DeckData LoadDeck()
        {
            var deck = _deckSaveDataPresenter.LoadDeck();
            return deck;
        }
        private IEnumerable<string> GetStrings()
        {
            var v = SpellDatabase.LoadOnEditor().SpellDictionary
                .Where(value => !value.Value.IsNotImp())
                .Select(value => value.Key);

            return v;
        }
    }
}
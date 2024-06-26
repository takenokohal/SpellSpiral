using System;
using System.Collections.Generic;

namespace DeckEdit.Model
{
    [Serializable]
    public class DeckData
    {
        public List<string> normalSpellDeck;
        public string highlanderSpell;

        public DeckData(List<string> normalSpellDeck, string highlanderSpell)
        {
            this.normalSpellDeck = normalSpellDeck;
            this.highlanderSpell = highlanderSpell;
        }
    }
}
using System.Collections.Generic;
using DeckEdit.Model;

namespace DeckEdit.SaveData
{
    public static class DefaultDeckData
    {
        public static DeckData GetDefaultDeck()
        {
            return new DeckData(new List<string>()
            {
                "FireShoot",
                "FireShoot",
                "FireShoot",

                "FireCracker",
                "FireCracker",
                "FireCracker",

                "IceMissile",
                "IceMissile",
                "IceMissile",

                "QuickLaser",
                "QuickLaser",

                "Thunder",
                "Explosion",
                "HeavyLaser",

                "SmallHeal",
                "MidHeal",

                "BuffAttack",
                "Duplication",
                "BuffDefense",
                "BuffQuickManaCharge"
            }, "ReflectionMirror");
        }
    }
}
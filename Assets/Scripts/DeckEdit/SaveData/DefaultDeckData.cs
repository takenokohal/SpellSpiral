using System.Collections.Generic;

namespace DeckEdit.SaveData
{
    public static class DefaultDeckData
    {
        public static List<string> GetDefaultDeck()
        {
            return new List<string>()
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
            };
        }
    }
}
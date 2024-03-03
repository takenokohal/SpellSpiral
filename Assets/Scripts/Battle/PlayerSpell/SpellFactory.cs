using Battle.Character.Enemy;
using Battle.Character.Player;
using Battle.Character.Player.Buff;
using Battle.CommonObject.MagicCircle;
using Databases;
using VContainer;

namespace Battle.PlayerSpell
{
    public class SpellFactory
    {
        [Inject] private readonly SpellDatabase _spellDatabase;
        [Inject] private readonly AllEnemyManager _allEnemyManager;
        [Inject] private readonly PlayerCore _playerCore;
        [Inject] private readonly MagicCircleFactory _magicCircleFactory;
        [Inject] private readonly PlayerBuff _playerBuff;

        public void Create(string spellKey)
        {
            var origin = _spellDatabase.Find(spellKey);
            var instance = origin.SpellBase.Construct(
                _playerCore,
                _allEnemyManager,
                _magicCircleFactory,
                _playerBuff,
                origin);
        }
    }
}
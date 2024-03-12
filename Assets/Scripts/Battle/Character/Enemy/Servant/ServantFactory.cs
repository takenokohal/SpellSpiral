using System.Collections.Generic;
using Battle.Character.Player;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using VContainer;

namespace Battle.Character.Enemy.Servant
{
    public class ServantFactory : SerializedMonoBehaviour
    {
        [OdinSerialize] private Dictionary<string, ServantBase> _servantPrefabs;

        [Inject] private readonly PlayerCore _playerCore;
        [Inject] private readonly AllEnemyManager _allEnemyManager;

        public ServantBase Create(string key)
        {
            var instance = Instantiate(_servantPrefabs[key]);
            instance.Init(_playerCore);

            return instance;
        }
    }
}
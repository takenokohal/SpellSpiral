using Battle.Character.Player;
using Sirenix.OdinInspector;
using VContainer;

namespace Battle.Character.Enemy
{
    public class EnemyControllerBase : SerializedMonoBehaviour
    {
        [Inject] private readonly PlayerCore _playerCore;
        protected PlayerCore PlayerCore => _playerCore;
    }
}
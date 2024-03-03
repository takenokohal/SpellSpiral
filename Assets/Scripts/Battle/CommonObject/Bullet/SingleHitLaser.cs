using Battle.Attack;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.CommonObject.Bullet
{
    public class SingleHitLaser : MonoBehaviour
    {
        [SerializeField] private AttackHitController attackHitControllerInChildren;
        [SerializeField] private ParticleSystem effect;


        public class Parameter
        {
            public Vector2 Pos { get; }
            public float Rotation { get; }
            public float ActiveTime { get; }

            public Parameter(Vector2 pos, float rotation, float activeTime)
            {
                Pos = pos;
                Rotation = rotation;
                ActiveTime = activeTime;
            }

            public Parameter(Vector2 pos, Vector2 direction, float activeTime)
            {
                var rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Pos = pos;
                Rotation = rot;
                ActiveTime = activeTime;
            }
        }

        public async UniTaskVoid Activate(Parameter parameter)
        {
            var t = transform;
            t.position = parameter.Pos;
            t.rotation = Quaternion.Euler(0, 0, parameter.Rotation);

            attackHitControllerInChildren.gameObject.SetActive(true);

            effect.Play();

            await UniTask.Delay((int)(parameter.ActiveTime * 1000f), cancellationToken: destroyCancellationToken);

            attackHitControllerInChildren.gameObject.SetActive(false);
        }
    }
}
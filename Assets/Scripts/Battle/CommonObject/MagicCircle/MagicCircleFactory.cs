using Audio;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Databases;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace Battle.CommonObject.MagicCircle
{
    public class MagicCircleFactory : MonoBehaviour
    {
        [SerializeField] private MagicCircle prefab;

        [Inject] private readonly CharacterDatabase _characterDatabase;

        private CinemachineTargetGroup _targetGroup;

        private ObjectPool<MagicCircle> _pool;

        private void Start()
        {
            _targetGroup = FindObjectOfType<CinemachineTargetGroup>();

            _pool = new ObjectPool<MagicCircle>(
                () => Instantiate(prefab));
        }


        public async UniTask CreateAndWait(MagicCircleParameters magicCircleParameters)
        {
            AllAudioManager.PlaySe("MagicCircle");

            var magicCircle = _pool.Get();
            magicCircle.Activate(magicCircleParameters).Forget();
            
            var data = _characterDatabase.Find(magicCircleParameters.CharacterKey);
            magicCircle.Init(data.MagicCircleSprite);

            var t = magicCircle.transform;

            _targetGroup.AddMember(t, 1, 0);

            var timeCount = 0f;

            while (timeCount < data.ChantTime)
            {
                t.position = magicCircleParameters.Pos.Invoke();
                timeCount += Time.fixedDeltaTime;

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }

            _targetGroup.RemoveMember(t);

            await magicCircle.Close();
            
            _pool.Release(magicCircle);
        }
    }
}
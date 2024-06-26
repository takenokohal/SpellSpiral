using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

namespace Battle.CutIn
{
    public class CutInController : MonoBehaviour
    {
        [SerializeField] private PlayableDirector lookRightTimeline;
        [SerializeField] private PlayableDirector lookLeftTimeline;


        [SerializeField] private Transform camerasParent;
        [SerializeField] private Transform particle;
        [SerializeField] private Canvas cutInBackGround;

        private Camera _mainCam;

        private void Start()
        {
            if (Camera.main != null) _mainCam = Camera.main;

            cutInBackGround.worldCamera = _mainCam;
        }

        [Button]
        private void PlayRight(GameObject player)
        {
            CutIn(true, player.transform).Forget();
        }

        [Button]
        private void PlayLeft(GameObject player)
        {
            CutIn(false, player.transform).Forget();
        }

        public async UniTask CutIn(bool lookRight, Transform target)
        {
            var xScale = lookRight ? 1 : -1;
            camerasParent.localScale = new Vector3(xScale, 1, 1);

            particle.localScale = new Vector3(xScale, 1, 1);
            particle.SetParent(_mainCam.transform);
            particle.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            var director = lookRight ? lookRightTimeline : lookLeftTimeline;

            director.Play();

            while (director.state == PlayState.Playing)
            {
                camerasParent.position = target.position;

                await UniTask.Yield(cancellationToken: destroyCancellationToken);
            }
        }
    }
}
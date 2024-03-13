using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using VContainer;

namespace Others
{
    public class IntroController : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Animator playerAnim;

        [SerializeField] private Transform enemyTransform;
        [SerializeField] private Animator enemyAnim;


        [Inject] private readonly GameLoop _gameLoop;


        [SerializeField] private Transform playerFrom;
        [SerializeField] private Transform enemyFrom;

        [SerializeField] private Transform playerTo;
        [SerializeField] private Transform enemyTo;

        [SerializeField] private float introTime;

        [SerializeField] private float recovery;


        [SerializeField] private Transform spellUI;
        [SerializeField] private Transform playerGageUI;
        [SerializeField] private Transform enemyGageUI;

        [SerializeField] private Transform[] walls;
        private static readonly int HorizontalSpeedAnimKey = Animator.StringToHash("HorizontalSpeed");


        private void Start()
        {
            IntroStart().Forget();
        }

        private async UniTaskVoid IntroStart()
        {
            AnimateUI();
            GenerateWall();


            await AnimateCharacters();
            _gameLoop.SendEvent(GameLoop.GameEvent.BattleStart);
        }

        private async UniTask AnimateCharacters()
        {
            playerAnim.SetFloat(HorizontalSpeedAnimKey, 1);
            enemyAnim.SetFloat(HorizontalSpeedAnimKey, 1);

            playerTransform.SetPositionAndRotation(playerFrom.position, playerFrom.rotation);
            enemyTransform.SetPositionAndRotation(enemyFrom.position, enemyFrom.rotation);


            var seq = DOTween.Sequence();
            seq.Append(playerTransform.DOMove(playerTo.position, introTime));
            seq.Join(enemyTransform.DOMove(enemyTo.position, introTime));
            seq.AppendInterval(recovery);

            DOVirtual.Float(
                1,
                0,
                recovery,
                value =>
                {
                    playerAnim.SetFloat(HorizontalSpeedAnimKey, value);
                    enemyAnim.SetFloat(HorizontalSpeedAnimKey, value);
                }).SetDelay(introTime - recovery);
            playerTransform.DORotateQuaternion(playerTo.rotation, recovery).SetDelay(introTime - recovery);
            enemyTransform.DORotateQuaternion(enemyTo.rotation, recovery).SetDelay(introTime - recovery);

            await seq;
        }

        private void AnimateUI()
        {
            var duration = 0.2f;
            spellUI.localScale = Vector3.zero;
            spellUI.DOScale(1, duration);

            playerGageUI.localScale = new Vector3(0, 1);
            playerGageUI.DOScaleX(1, duration);

            enemyGageUI.localScale = new Vector3(0, 1);
            enemyGageUI.DOScaleX(1, duration);
        }

        private void GenerateWall()
        {
            foreach (var wall in walls)
            {
                var to = wall.localScale;

                wall.localScale = Vector3.zero;
                wall.DOScale(to, 0.2f);
            }
        }
    }
}
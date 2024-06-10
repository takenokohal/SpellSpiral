using Battle.Character.Enemy;
using Battle.Character.Player;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Scene;
using UniRx;
using UnityEngine;
using VContainer;

namespace Others
{
    public class IntroController : MonoBehaviour
    {
        [Inject] private readonly PlayerCore _playerCore;
        [Inject] private readonly EnemyBase _enemyBase;
        private Transform PlayerTransform => _playerCore.transform;
        private Animator PlayerAnim => _playerCore.Animator;

        private Transform EnemyTransform => _enemyBase.transform;
        private Animator EnemyAnim => _enemyBase.Animator;


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

        [SerializeField] private bool isDebug;

        private void Start()
        {
            if (isDebug)
                DebugIntro().Forget();
            else
                IntroStart().Forget();
        }

        private async UniTaskVoid DebugIntro()
        {
            PlayerTransform.SetPositionAndRotation(playerTo.position, playerTo.rotation);
            EnemyTransform.SetPositionAndRotation(enemyTo.position, enemyTo.rotation);
            await UniTask.Yield(cancellationToken: destroyCancellationToken);
            _gameLoop.SendEvent(GameLoop.GameEvent.BattleStart);
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
            PlayerAnim.SetFloat(HorizontalSpeedAnimKey, 1);
            EnemyAnim.SetFloat(HorizontalSpeedAnimKey, 1);

            PlayerTransform.SetPositionAndRotation(playerFrom.position, playerFrom.rotation);
            EnemyTransform.SetPositionAndRotation(enemyFrom.position, enemyFrom.rotation);


            var seq = DOTween.Sequence();
            seq.Append(PlayerTransform.DOMove(playerTo.position, introTime));
            seq.Join(EnemyTransform.DOMove(enemyTo.position, introTime));
            seq.AppendInterval(recovery);

            DOVirtual.Float(
                1,
                0,
                recovery,
                value =>
                {
                    PlayerAnim.SetFloat(HorizontalSpeedAnimKey, value);
                    EnemyAnim.SetFloat(HorizontalSpeedAnimKey, value);
                }).SetDelay(introTime - recovery);
            PlayerTransform.DORotateQuaternion(playerTo.rotation, recovery).SetDelay(introTime - recovery);
            EnemyTransform.DORotateQuaternion(enemyTo.rotation, recovery).SetDelay(introTime - recovery);

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
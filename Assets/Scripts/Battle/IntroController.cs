using Battle.Character;
using Battle.Character.Enemy;
using Battle.Character.Player;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace Battle
{
    public class IntroController : MonoBehaviour
    {
        [Inject] private readonly AllCharacterManager _allCharacterManager;
        private PlayerCore PlayerCore => _allCharacterManager.PlayerCore;
        private BossBase BossBase => _allCharacterManager.Boss;
        private Transform PlayerTransform => PlayerCore.transform;
        private Animator PlayerAnim => PlayerCore.Animator;

        private Transform EnemyTransform => BossBase.transform;
        private Animator EnemyAnim => BossBase.Animator;


        [Inject] private readonly BattleLoop _battleLoop;


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
            UniTask.Void(async () =>
            {
                await UniTask.WaitUntil(() => _allCharacterManager.IsPlayerAndBossRegistered());
                
                if (isDebug)
                    DebugIntro().Forget();
                else
                    IntroStart().Forget();
            });

        }


        private async UniTaskVoid DebugIntro()
        {
            PlayerTransform.SetPositionAndRotation(playerTo.position, playerTo.rotation);
            EnemyTransform.SetPositionAndRotation(enemyTo.position, enemyTo.rotation);
            await UniTask.Yield(cancellationToken: destroyCancellationToken);
            _battleLoop.SendEvent(BattleEvent.BattleStart);
        }

        private async UniTaskVoid IntroStart()
        {
            AnimateUI();
            GenerateWall();


            await AnimateCharacters();
            _battleLoop.SendEvent(BattleEvent.BattleStart);
        }

        private async UniTask AnimateCharacters()
        {
            PlayerAnim.SetFloat(HorizontalSpeedAnimKey, 1);
            if (!BossBase.AnimatorIsNull)
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
                    if (!BossBase.AnimatorIsNull)
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
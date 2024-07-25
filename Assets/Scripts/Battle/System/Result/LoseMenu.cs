using System;
using Cysharp.Threading.Tasks;
using DeckEdit.Controller;
using DG.Tweening;
using Others.Dialog;
using Others.Input;
using Others.Scene;
using UnityEngine;
using VContainer;

namespace Battle.System.Result
{
    public class LoseMenu : MonoBehaviour
    {
        private bool _isActive;

        [Inject] private readonly MySceneManager _mySceneManager;


        [Inject] private readonly ChoiceDialog _choiceDialog;

        //  [SerializeField] private VerticalChoiceListView verticalChoiceListView;
        //   [SerializeField] private IconAndTitle.Parameter[] parameters;

        private const string DescriptionKey = "mission_failed";
        private const string RestartKey = "mission_failed_restart";
        private const string HomeKey = "mission_failed_home";
        private const string DeckEditKey = "mission_failed_deck_edit";

        public async UniTaskVoid Activate()
        {
            var result = await _choiceDialog.StartDialog(new ChoiceDialogData(DescriptionKey, new[]
            {
                RestartKey, HomeKey, DeckEditKey
            }));

            switch ((Choice)result)
            {
                case Choice.Restart:
                    _mySceneManager.ChangeSceneAsync("BattleMain").Forget();
                    break;
                case Choice.Home:
                    _mySceneManager.ChangeSceneAsync("Home").Forget();
                    break;
                case Choice.DeckEdit:
                    await DeckEditActiveController.StartDeckEdit();
                    Activate().Forget();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return;
            //   verticalChoiceListView.Initialize(_myInputManager, parameters);
            gameObject.SetActive(true);
            await transform.DOScaleY(0, 0);
            await transform.DOScaleY(1, 0.5f);
            _isActive = true;
            //      verticalChoiceListView.OnSelect.Where(_ => _isActive).Subscribe(OnSelect).AddTo(this);
        }

        private enum Choice
        {
            Restart,
            Home,
            DeckEdit
        }
    }
}
using Cysharp.Threading.Tasks;
using Others.Scene;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace DeckEdit.View
{
    public class BackButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [Inject] private readonly MySceneManager _mySceneManager;

        private void Start()
        {
            button.OnClickAsObservable().TakeUntilDestroy(this).Take(1).Subscribe(_ =>
            {
                _mySceneManager.ChangeSceneAsync(_mySceneManager.PrevSceneName).Forget();
            });
        }
    }
}
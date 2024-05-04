using System.Linq;
using Cysharp.Threading.Tasks;
using Others.Input;
using Others.Scene;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Others
{
    public class TitleScene : LifetimeScope
    {
        private MyInputManager _myInputManager;
        private MySceneManager _mySceneManager;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            _myInputManager = Parent.Container.Resolve<MyInputManager>();
            _mySceneManager = Parent.Container.Resolve<MySceneManager>();

            WaitPressButton().Forget();
        }

        private async UniTaskVoid WaitPressButton()
        {
            await UniTask.WaitUntil(
                () => _myInputManager.UiInput.actions.Any(value => value.WasPressedThisFrame()),
                cancellationToken: destroyCancellationToken);

            _mySceneManager.ChangeSceneAsync("Home").Forget();
        }
    }
}
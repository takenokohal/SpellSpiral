using Cysharp.Threading.Tasks;
using Others.Scene;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Others
{
    public class BattleMainSceneLoader : IInitializable
    {
        [Inject] private readonly MySceneManager _mySceneManager;

        public void Initialize()
        {
            var cam = Camera.main;
            if (cam != null)
                cam.gameObject.SetActive(false);
            _mySceneManager.LoadSceneAdditive("BattleMain").Forget();
        }
    }
}
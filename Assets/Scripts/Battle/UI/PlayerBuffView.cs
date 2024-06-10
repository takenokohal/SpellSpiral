using System.Collections.Generic;
using Battle.Character.Player;
using Battle.Character.Player.Buff;
using Databases;
using UniRx;
using UnityEngine;
using VContainer;

namespace Battle.UI
{
    public class PlayerBuffView : MonoBehaviour
    {
        [Inject] private readonly SpellDatabase _spellDatabase;
        [Inject] private readonly SpellColorPalette _colorPalette;

        [Inject] private readonly PlayerCore _playerCore;
        private PlayerBuff PlayerBuff => _playerCore.PlayerBuff;


        [SerializeField] private PlayerBuffViewChild playerBuffViewChildPrefab;
        [SerializeField] private Transform parent;

        private readonly Dictionary<BuffParameter, PlayerBuffViewChild> _children = new();

        private void Start()
        {
            PlayerBuff.BuffParameters.ObserveAdd()
                .TakeUntilDestroy(this)
                .Subscribe(value => OnAdd(value.Value));

            PlayerBuff.BuffParameters.ObserveRemove()
                .TakeUntilDestroy(this)
                .Subscribe(value => OnRemove(value.Value));
        }

        private void OnAdd(BuffParameter buffParameter)
        {
            var instance = Instantiate(playerBuffViewChildPrefab, parent);

            var spellData = _spellDatabase.Find(buffParameter.SpellKey);
            instance.SetIcon(spellData.SpellIcon);
            instance.SetBackGroundColor(_colorPalette.GetColor(spellData.SpellAttribute));
            instance.SetCircleFill(1);
            _children.Add(buffParameter, instance);
        }

        private void OnRemove(BuffParameter buffParameter)
        {
            var instance = _children[buffParameter];
            Destroy(instance.gameObject);
            _children.Remove(buffParameter);
        }

        private void FixedUpdate()
        {
            foreach (var (key, playerBuffViewChild) in _children)
            {
                var fill = 1f - key.CurrentTime / key.EffectDuration;
                playerBuffViewChild.SetCircleFill(fill ?? 1);
            }
        }
    }
}
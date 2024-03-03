using System.Collections.Generic;
using Battle.Character;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.PlayerSpell.Variables
{
    public class Cyclone : SpellBase
    {
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float duration;

        [SerializeField] private List<Transform> cubes;


        protected override async UniTaskVoid Init()
        {
            foreach (var t in cubes)
            {
                MagicCircleFactory.CreateAndWait(new MagicCircleParameters(CharacterKey.Player, Color.white, 1,
                    () => t.position)).Forget();
            }

            await MyDelay(0.2f);

            foreach (var cube in cubes)
            {
                cube.gameObject.SetActive(true);
            }


            await MyDelay(duration);

            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            transform.position = PlayerCore.Center.position;
            transform.localRotation *= Quaternion.Euler(rotationSpeed * Time.fixedDeltaTime, 0, 0);
        }
    }
}
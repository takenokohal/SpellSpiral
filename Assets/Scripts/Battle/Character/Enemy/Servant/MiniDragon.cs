using Battle.CommonObject.Bullet;
using UnityEngine;

namespace Battle.Character.Enemy.Servant
{
    //固定砲台
    public class MiniDragon : ServantBase
    {
        [SerializeField] private DirectionalBullet directionalBullet;
        [SerializeField] private Transform nozzle;
        
        private void FireAnimationEvent()
        {
            directionalBullet.CreateFromPrefab(nozzle.position, GetDirectionToPlayer());
        }
    }
}
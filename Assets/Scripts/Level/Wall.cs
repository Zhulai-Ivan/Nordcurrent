using Enemies;
using Player.Bullet;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(Collider2D))]
    public class Wall : MonoBehaviour, IBulletDamageable
    {
        public void HandleBulletEnter(Bullet bullet)
        {
           // can destroy walls in future
        }
    }
}
using Player.Bullet;

namespace Enemies
{
    public interface IBulletDamageable
    {
        public void HandleBulletEnter(Bullet bullet);
    }
}
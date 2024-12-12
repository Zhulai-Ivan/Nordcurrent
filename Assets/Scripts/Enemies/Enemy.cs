using Enemy;
using Modules;
using Player.Bullet;
using View;
using Zenject;

namespace Enemies
{
    public class Enemy : BaseView, IBulletDamageable
    {
        private BaseTrack _tracks;
        private BaseBody _body;
        private BaseTower _tower;
        private ViewPool _viewPool;

        [Inject]
        private void InstallBindings(ViewPool viewPool)
        {
            _viewPool = viewPool;
        }

        public void HandleBulletEnter(Bullet bullet)
        {
            _viewPool.Push(bullet);
            // change state to dead
        }
    }
}
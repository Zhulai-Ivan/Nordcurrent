using System;
using Modules;
using Player.Bullet;
using View;

namespace Enemies
{
    public class Enemy : BaseView, IBulletDamageable
    {
        private BaseTrack _tracks;
        private BaseBody _body;
        private BaseTower _tower;

        public event Action Pushed;
        
        public void HandleBulletEnter(Bullet bullet)
        {
            Push();
        }

        public override void Push()
        {
            ViewPool.Push(this);
            Pushed?.Invoke();
        }
    }
}
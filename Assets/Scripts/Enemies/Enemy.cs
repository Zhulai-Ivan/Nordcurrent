using System;
using Modules;
using Player.Bullet;
using States;
using View;
using StateMachine = States.StateMachine;
using Vector2 = UnityEngine.Vector2;

namespace Enemies
{
    public class Enemy : BaseView, IBulletDamageable
    {
        private BaseTrack _tracks;
        private BaseBody _body;
        private BaseTower _tower;
        
        private StateMachine _stateMachine;
        private Vector2 _xLimits;
        private Vector2 _yLimits;

        private float Speed => _tracks.Speed;

        public event Action<Enemy> Pushed;
        public event Action<Enemy> ReadyToSpawn;

        private void Awake()
        {
            _stateMachine = new StateMachine();
        }

        public void SetPositionLimits(Vector2 xLimits, Vector2 yLimits)
        {
            _xLimits = xLimits;
            _yLimits = yLimits;
        }

        public void HandleBulletEnter(Bullet bullet)
        {
            Dead();
        }

        public override void Push()
        {
            ViewPool.Push(this);
            Pushed?.Invoke(this);
        }

        public void Move()
        {
            var state = new MoveState(transform, _xLimits, _yLimits, Speed);
            _stateMachine.SetState(state);
        }

        private void Dead()
        {
            var state = new DeathState(this);
            state.Complete += OnDeathComplete;
            _stateMachine.SetState(state);
        }

        private void OnDeathComplete() =>
            ReadyToSpawn?.Invoke(this);

        public void SetTrack(BaseTrack track)
        {
            _tracks = track;
        }
    }
}
using System;
using Modules;
using Player.Bullet;
using States;
using UnityEngine;
using View;
using StateMachine = States.StateMachine;
using Vector2 = UnityEngine.Vector2;

namespace Enemies
{
    public class Enemy : TankView, IBulletDamageable
    {
        private BaseTrack _tracks;
        
        private StateMachine _stateMachine;
        private Vector2 _xLimits;
        private Vector2 _yLimits;

        private float Speed => _tracks.Speed;

        public event Action<Enemy> Pushed;
        public event Action<Enemy> ReadyToSpawn;

        public override string Name => "Enemy";
        public override ModuleType TrackType => _tracks.Type;

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

        public override void SetTrack(BaseTrack track)
        {
            _tracks = track;
        }
    }
}
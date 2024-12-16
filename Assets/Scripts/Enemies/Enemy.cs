using System;
using Modules;
using Player.Bullet;
using States;
using Unity.VisualScripting;
using UnityEngine;
using View;
using StateMachine = States.StateMachine;

namespace Enemies
{
    public class Enemy : BaseView, IBulletDamageable
    {
        private BaseTrack _tracks;
        private BaseBody _body;
        private BaseTower _tower;
        
        private StateMachine _stateMachine;

        public float Speed => /*.Speed*/ 5f;

        public event Action<Enemy> Pushed;
        public event Action<Enemy> ReadyToSpawn;

        private void Awake()
        {
            _stateMachine = new StateMachine();
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

        public void Move(Vector2 xLimits, Vector2 yLimits)
        {
            var state = new MoveState(transform, xLimits, yLimits, Speed);
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
    }
}
﻿using System;
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

        public event Action Pushed;

        private void Awake()
        {
            _stateMachine = new StateMachine();
        }

        public void HandleBulletEnter(Bullet bullet)
        {
            Push();
        }

        public override void Push()
        {
            ViewPool.Push(this);
            Pushed?.Invoke();
        }

        public void Move(Vector2 xLimits, Vector2 yLimits)
        {
            var state = new MoveState(transform, xLimits, yLimits, Speed);
            _stateMachine.SetState(state);
        }
    }
}
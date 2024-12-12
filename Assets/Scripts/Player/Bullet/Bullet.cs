using System;
using Enemies;
using Enemy;
using UnityEngine;

namespace Player.Bullet
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Bullet : BaseView
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _speed = 6f;

        private Vector2 _direction;

        public Vector2 Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }

        private void FixedUpdate()
        {
            _rigidbody.MovePosition(_rigidbody.position + _speed * Direction * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var damageable = other.gameObject.GetComponent<IBulletDamageable>();
            damageable?.HandleBulletEnter(this);
            Push();
        }
    }
}
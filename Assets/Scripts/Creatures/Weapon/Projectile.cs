using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace PlayPort.Creatures.Weapon
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private int _directon;
        private Rigidbody2D _rigidbody;

        private void Start()
        {
            _directon = transform.lossyScale.x > 0 ? 1 : -1;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var position = _rigidbody.position;
            position.x += _speed * _directon;
            _rigidbody.MovePosition(position);
        }
    }
}

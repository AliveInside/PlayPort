using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayPort.Creatures.Weapon
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] protected float _speed;
        [SerializeField] private bool _invertXScale;
        protected int Direction;
        protected Rigidbody2D Rigidbody;

        
        protected virtual void Start()
        {
            var modifier = _invertXScale ? -1 : 1;
            Direction = modifier * transform.lossyScale.x > 0 ? 1 : -1;
            Rigidbody = GetComponent<Rigidbody2D>();
        }

    }
}

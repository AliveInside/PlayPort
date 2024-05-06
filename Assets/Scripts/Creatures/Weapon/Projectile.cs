using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace PlayPort.Creatures.Weapon
{
    public class Projectile : BaseProjectile
    {
        private void FixedUpdate()
        {
            var position = Rigidbody.position;
            position.x += _speed * Direction;
            Rigidbody.MovePosition(position);
        }
    }
}

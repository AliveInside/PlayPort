using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

namespace PlayPort
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        //private float _direction;
        private Vector2 _direction;
        
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        
        private void Update()
        {
            if (_direction[0] != 0)
            {
                var delta = _direction[0] * _speed * Time.deltaTime;
                var newXPosition = transform.position.x + delta;
                transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
            }
            
            if (_direction[1] != 0)
            {
                var delta = _direction[1] * _speed * Time.deltaTime;
                var newYPosition = transform.position.y + delta;
                transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
            }
        }

        public void Speech()
        {
            Debug.Log("Speeeeech");
        }

    }
}

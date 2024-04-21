using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayPort
{
    public class ElevatorMovement : MonoBehaviour
    {
            [SerializeField] private float _movementSpeed;
            [SerializeField] private bool _isDownMovement;
            [SerializeField] private Transform _upPoint;
            [SerializeField] private Transform _downPoint;
            private bool _isMovementStarted = true;
            private Vector3 _targetPosition;

            private void Update()
            {
                if (_isMovementStarted)
                {
                    if (transform.position.y == _downPoint.position.y)
                    {
                        _isDownMovement = false;
                        EndMovement();
                    }
                    else if (transform.position.y == _upPoint.position.y)
                    {
                        _isDownMovement = true;
                        EndMovement();
                    }

                    _targetPosition = _isDownMovement ? _downPoint.position : _upPoint.position;
                    transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _movementSpeed);
                }
            }

            public void StartMovement()
            {
                _isMovementStarted = true;
            }

            public void EndMovement()
            {
                _isMovementStarted = false;
            }
    }
}

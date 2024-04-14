using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace PlayPort.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onRegeneration;
        [SerializeField] private HealthChangeEvent _onChange;

        public void HealthChange(int changeAmount)
        {
            _health += changeAmount;
            _onChange?.Invoke(_health);

            Debug.Log("Health =" + _health + " changeAmount = " + changeAmount);

            if (changeAmount < 0)
            {
                _onDamage?.Invoke();
            }
            
            if (changeAmount > 0)
            {
                _onRegeneration?.Invoke();
            }

            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {

        }

        public void SetHealth(int health)
        {
            _health = health;
        }
    }
}

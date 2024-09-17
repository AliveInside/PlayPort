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
        private static int _healthMax;

        private void Awake()
        {
            _healthMax = _health;
        }

        public void HealthChange(int changeAmount)
        {
            _health += changeAmount;
            Debug.Log("Health =" + _health + " changeAmount = " + changeAmount);

            if (changeAmount < 0)
            {
                _onDamage?.Invoke();

                if (_health <= 0)
                {
                    Debug.Log("DEATH");
                    _onDie?.Invoke();
                }
            }
            else
            {
                _onRegeneration?.Invoke();
            }
        }
    }
}

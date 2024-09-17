using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayPort.Components
{
    public class HPChangeComponent : MonoBehaviour
    {
        [SerializeField] private int _amount;
        [SerializeField] private bool _isDamage;
        public void HPChange(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            var amount = _amount;

            if (_isDamage)
            {
                amount *= -1;
            }

            healthComponent?.HealthChange(amount);
        }
    }
}

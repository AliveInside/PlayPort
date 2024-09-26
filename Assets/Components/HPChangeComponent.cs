using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayPort.Components
{
    public class HPChangeComponent : MonoBehaviour
    {
        [SerializeField] private int _amount;
        [SerializeField] private TypeOfChange _type;
        public void HPChange(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            var amount = _amount;

            if (_type == TypeOfChange.Damage)
            {
                amount *= -1;
            }

            healthComponent?.HealthChange(amount);
        }

        public enum TypeOfChange
        {
            Heal,
            Damage
        }
    }
}

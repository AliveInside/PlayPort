using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayPort.Components;
using PlayPort.Utils;
using UnityEngine;

namespace PlayPort.Creatures.Mobs
{
    public class TrapModule : MonoBehaviour
    {
        [SerializeField] private List<BaseShootingAI> _traps;
        [SerializeField] private CoolDown _cooldown;

        private int _currentTrap;

        private void Start()
        {
            foreach (var shootingTrap in _traps)
            {
                var hp = shootingTrap.GetComponent<HealthComponent>();
                hp._onDie.AddListener(() => OnTrapDestroyed(shootingTrap));

                shootingTrap.enabled = false;
            }
        }

        private void Update()
        {
            if (_traps.Count == 0)
            {
                enabled = false;
                Destroy(gameObject, 1f);
            }

            var hasTarget = _traps.Any(x => x._vision.IsTouchingLayer);

            if (hasTarget && _cooldown.IsReady)
            {
                _traps[_currentTrap].OnRangeAttack();
                _cooldown.Reset();
                _currentTrap = (int) Mathf.Repeat(_currentTrap + 1, _traps.Count);
            }
        }

        private void OnTrapDestroyed(BaseShootingAI shootingTrap)
        {
            var index = _traps.IndexOf(shootingTrap);

            _traps.Remove(shootingTrap);

            if (index < _currentTrap)
            {
                _currentTrap--;
            }
        }
    }
}

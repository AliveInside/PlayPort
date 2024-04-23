using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayPort.Creatures;

namespace PlayPort.Components
{
    public class PotionEffectComponent : MonoBehaviour
    {
            [SerializeField] private float _buffCoefficient;
            [SerializeField] private string _buffName;
            private Hero _hero;

            private void Start()
            {
                _hero = FindObjectOfType<Hero>();
            }

            public void Buff()
            {
                if (_buffName.Contains("Jump"))
                    _hero.JumpBuff(_buffCoefficient);

                if (_buffName.Contains("Speed"))
                    _hero.SpeedBuff(_buffCoefficient);
            }
    }
}

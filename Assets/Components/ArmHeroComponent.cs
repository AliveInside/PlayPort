using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayPort.Components
{
    public class ArmHeroComponent : MonoBehaviour
    {
        public void ArmHero(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            hero.ArmHero();
        }
    }
}
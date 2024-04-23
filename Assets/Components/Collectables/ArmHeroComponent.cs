using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayPort.Creatures;


namespace PlayPort.Components.Collectables
{
    public class ArmHeroComponent : MonoBehaviour
    {
        public void ArmHero(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            hero.ArmHero();
            hero.SwordsCountChange(1);
        }
    }
}
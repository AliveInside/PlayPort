using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayPort.Components
{
    public class RestartLevelComponent : MonoBehaviour
    {
        private Hero _hero;
        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
        }

        public void Restart()
        {
            _hero.AddCoins(0);
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
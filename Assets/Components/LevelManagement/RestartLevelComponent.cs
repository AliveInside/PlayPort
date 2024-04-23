using System.Collections;
using System.Collections.Generic;
using PlayPort.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayPort.Components
{
    public class RestartLevelComponent : MonoBehaviour
    {
        public void Restart()
        {
            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);
            
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
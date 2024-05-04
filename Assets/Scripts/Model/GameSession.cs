using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayPort.Model.Data;

namespace PlayPort.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;
        // public PlayerData _save;

        // public void Save()
        // {
        //     _save = _data.Clone();
        // }

        // public void LoadLastSave()
        // {
        //     _data = _save.Clone();
        // }

        private void Awake()
        {
            if (IsSessionExist())
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
            }
        }

        private bool IsSessionExist()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var session in sessions)
            {
                if (session != this)
                    return true;
            }

            return false;
        }
    }
}

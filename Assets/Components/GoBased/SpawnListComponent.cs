using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PlayPort.Components
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField]  private SpawnData[] _spawners;

        public void Spawn(string id)
        {
            var spawner = _spawners.FirstOrDefault(element => element.Id == id);
            spawner?.Component.Spawn();

            // // Same
            // foreach (var data in _spawners)
            // {
            //     if (data.Id == id)
            //     {
            //         data.Component.Spawn();
            //         break;
            //     }
            // }
        }

        [Serializable]
        public class SpawnData
        {
            public string Id;
            public SpawnComponent Component;
        }
    }
}

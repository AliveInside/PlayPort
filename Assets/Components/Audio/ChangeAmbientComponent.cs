using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayPort.Components.Audio
{
    public class ChangeAmbientComponent : MonoBehaviour
    {

        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioData[] _sounds;
        private AudioClip _initialSong;
        private bool _isInZone = false;

        private void Start()
        {
            _initialSong = _source.clip;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("AmbientZone"))
            {
                Debug.Log("Its ambient");
                foreach (var sound in _sounds)
                {
                    if (collider == sound.ZoneCollider)
                    {
                        _source.clip = sound.Clip;
                        _source.loop = true;
                        _source.Play();
                        _isInZone = true;
                        break;
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (_isInZone)
            {
                _source.clip = _initialSong;
                _source.Play();
                _isInZone = false;
            }
        }

        [Serializable]
        public class AudioData
        {
            [SerializeField] private AudioClip _clip; 
            [SerializeField] private string _id;
            [SerializeField] private Collider2D _zoneCollider;

            public string Id => _id;
            public AudioClip Clip => _clip;
            public Collider2D ZoneCollider => _zoneCollider;
        }
    }
}

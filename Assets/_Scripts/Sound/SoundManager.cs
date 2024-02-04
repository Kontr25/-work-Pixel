using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static Action DestructSound;
        public static Action RicochetSound;
        public static Action DestroySound;

        [SerializeField] private AudioSource[] _destructSounds;
        [SerializeField] private AudioSource[] _ricochetSound;
        [SerializeField] private AudioSource[] _destroySounds;
        [SerializeField] private AudioSource _throwSounds;
        [SerializeField] private float _destroyDurationValue;

        private Coroutine _destroyRoutine;
        private WaitForSeconds _destroyDuration;

        private int _currentRicochetSound = 0;
        
        private int _currentSoundNumber;

        private void Start()
        {
            _destroyDuration = new WaitForSeconds(_destroyDurationValue);
        }

        private void OnEnable()
        {
            DestructSound += PlayDestructSound;
            RicochetSound += PlayRicochetSound;
            DestroySound += StoneDestroySound;
        }

        private void OnDisable()
        {
            DestructSound -= PlayDestructSound;
            RicochetSound -= PlayRicochetSound;
            DestroySound -= StoneDestroySound;
        }

        private void PlayDestructSound()
        {
            _destructSounds[_currentSoundNumber].Play();

            if (_currentSoundNumber < _destructSounds.Length) _currentSoundNumber++;
            else _currentSoundNumber = 0;
        }

        private void PlayRicochetSound()
        {
            _ricochetSound[_currentRicochetSound].Play();
            
            switch (_currentRicochetSound)
            {
                case 0:
                    _currentRicochetSound = 1;
                    break;
                case 1:
                    _currentRicochetSound = 0;
                    break;
            }
        }

        private void StoneDestroySound()
        {
            if (_destroyRoutine == null)
            {
                _destroyRoutine = StartCoroutine(DestroySoundRoutine());
            }
        }

        private IEnumerator DestroySoundRoutine()
        {
            _throwSounds.Play();
            int soundNumber = Random.Range(0, _destroySounds.Length);
            _destroySounds[soundNumber].Play();
            yield return _destroyDuration;
            _destroyRoutine = null;
        }
    }
}
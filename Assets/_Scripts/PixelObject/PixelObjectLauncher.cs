using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.PixelObject
{
    public class PixelObjectLauncher : MonoBehaviour
    {
        public static Action Fall;
        
        private List<PixelObject> _pixelObjects = new List<PixelObject>();
        [SerializeField] private List<int> _capacityObjects = new List<int>();
        [SerializeField] private int _marginOfError;
        [SerializeField] private Image _progressBar;
        [SerializeField] private float _delayBetweenLaunch;
        [SerializeField] private Level _level;
        [SerializeField] private Transform[] _pointsToSpawn;

        private int _pointNumber = 0;
        private WaitForSeconds _delay;
        private int _currentPixelObjectNumber = 0;
        private float _totalNumberCubes = 0;
        private float _currentNumberCubes = 0;
        private Coroutine _launchRoutine;
        

        private void Start()
        {
            for (int i = 0; i < _level.Objects.Length; i++)
            {
                PixelObject PixObj = Instantiate(_level.PixelObjectPrefab(), _pointsToSpawn[_pointNumber].position, Quaternion.identity);
                _pixelObjects.Add(PixObj);
                _pointNumber++;
            }
            _delay = new WaitForSeconds(_delayBetweenLaunch);
            Invoke(nameof(UpdateCapacityList), 1f);
        }

        private void UpdateCapacityList()
        {
            for (int i = 0; i < _pixelObjects.Count; i++)
            {
                _capacityObjects.Add(_pixelObjects[i].Cubes.Count);
                _totalNumberCubes += _pixelObjects[i].Cubes.Count;
                _currentNumberCubes = _totalNumberCubes;
            }

            StartLaunchObject();
        }

        private void OnEnable()
        {
            Fall += IsCompletelyDestroyed;
        }

        private void OnDisable()
        {
            Fall -= IsCompletelyDestroyed;
        }

        private void IsCompletelyDestroyed()
        {
            _currentNumberCubes--;
            _progressBar.fillAmount = 1 - (1 / _totalNumberCubes * _currentNumberCubes);

            if (_currentNumberCubes < _marginOfError)
            {
                FinishAction.Finish.Invoke(FinishAction.FinishType.Win);
            }
        }

        public void PixelObjectOnStartPosition()
        {
            PixelObject PixObj = _pixelObjects[_currentPixelObjectNumber];
            PixObj.transform.position = transform.position;
            PixObj.RigidbodyPixelObject.isKinematic = false;
            _currentPixelObjectNumber++;
        }

        public void StartLaunchObject()
        {
            if (_launchRoutine == null) _launchRoutine = StartCoroutine(LaunchObjectWithDelay());
        }

        private IEnumerator LaunchObjectWithDelay()
        {
            while (_currentPixelObjectNumber < _pixelObjects.Count)
            {
                PixelObjectOnStartPosition();
                yield return _delay;
            }
            
        }
    }
}
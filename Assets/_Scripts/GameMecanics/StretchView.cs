using System;
using UnityEngine;

namespace GameMecanics
{
    public class StretchView : MonoBehaviour
    {
        [SerializeField] private Transform[] _circles;
        [SerializeField] private AudioSource _strethSound;
        private float _currentDistance = 0;
        private float _distanceBetweenPoints;
        private Vector3 _startPosition;
        private int _currentInteger = 0;

        public Vector3 StartPosition
        {
            get => _startPosition;
            set
            {
                RenderIs(true);
                _startPosition = new Vector3(value.x, value.y, transform.position.z);
            }
            
        }

        public void RenderStretch(Vector3 endPosition)
        {
            endPosition = new Vector3(endPosition.x, endPosition.y, transform.position.z);
            _distanceBetweenPoints = Vector3.Distance(_startPosition, endPosition)/ _circles.Length;
            
            for (int i = 0; i < _circles.Length; i++)
            {
                _currentDistance += _distanceBetweenPoints;
                Vector3 position = Vector3.Lerp(_startPosition, endPosition,
                    _currentDistance / Vector3.Distance(_startPosition, endPosition));
                _circles[i].position = position;
            }

            StretchSound(endPosition);
            _currentDistance = 0;
        }

        private void StretchSound(Vector3 endPosition)
        {
            float distance = Vector3.Distance(_startPosition, endPosition)/2;
            if (Mathf.RoundToInt(distance) != _currentInteger)
            {
                _currentInteger = Mathf.RoundToInt(distance);
                _strethSound.Play();
            }
        }

        public void RenderIs(bool value)
        {
            for (int i = 0; i < _circles.Length; i++)
            {
                _circles[i].gameObject.SetActive(value);
            }
        }
    }
}
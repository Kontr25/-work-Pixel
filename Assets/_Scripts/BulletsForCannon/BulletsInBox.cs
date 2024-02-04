using System;
using _Scripts.Reflected;
using UnityEngine;

namespace _Scripts.BulletsForCannon
{
    public class BulletsInBox : MonoBehaviour
    {
        [Range(10, 35)]
        [SerializeField] private int _bulletsCount;
        [SerializeField] private BulletFromBox[] _bullets;
        [SerializeField] private int _countForCannon;
        [SerializeField] private Obstacle _obstacle;
        
        private int _maxCountForCannon = 0;

        public int MaxCountForCannon
        {
            get
            {
                _maxCountForCannon = _bulletsCount * _countForCannon;
                return _maxCountForCannon;
            }
            set => _maxCountForCannon = value;
        }

        private void Start()
        {
            for (int i = 0; i < _bulletsCount; i++)
            {
                _bullets[i].gameObject.SetActive(true);
                _bullets[i].CountForCannon = _countForCannon;
                _obstacle.BulletsRigidbody.Add(_bullets[i].bulletRigidbody);
            }
        }
    }
}
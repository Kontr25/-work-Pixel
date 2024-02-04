using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.PixelObject
{
    public class PixelCube : MonoBehaviour
    {
        [SerializeField] private GameObject _cube;
        [SerializeField] private Rigidbody _cubeRigidbody;
        [SerializeField] private List<PixelCube> _neighbors = new List<PixelCube>();
        [SerializeField] private bool _isVerified = false;
        private const float _hitDistance = 0.25f;
        private bool _isCanInvokeFallAction = true;
        Ray _ray;
        RaycastHit _hit;

        private PixelObject _mainPixelObject;
        
        private bool _isFallen = false;

        private void Start()
        {
            AddNeigbors();
        }

        public bool IsFallen
        {
            get => _isFallen;
            set => _isFallen = value;
        }

        public PixelObject MainPixelOgject
        {
            get => _mainPixelObject;
            set => _mainPixelObject = value;
        }

        public bool IsVerified
        {
            get => _isVerified;
            set => _isVerified = value;
        }

        public List<PixelCube> Neighbors
        {
            get => _neighbors;
            set => _neighbors = value;
        }

        public bool IsCanInvokeFallAction
        {
            get => _isCanInvokeFallAction;
            set => _isCanInvokeFallAction = value;
        }

        public void StartCheckNeighbors()
        {
            _isVerified = true;
            _mainPixelObject.AddVerifiedPixel(this);
            for (int i = 0; i < _neighbors.Count; i++)
            {
                if (CheckingNeighbors(this) > 0)
                {
                    if (!_neighbors[i]._isVerified)
                    {
                        _neighbors[i].StartCheckNeighbors();
                    }
                };
            }
        }

        public void AddNeigbors()
        {
            _neighbors = new List<PixelCube>();
            if(CheckingRay(transform.forward) != null) _neighbors.Add(CheckingRay(transform.forward));
            if(CheckingRay(-transform.forward) != null) _neighbors.Add(CheckingRay(-transform.forward));
            if(CheckingRay(transform.right) != null) _neighbors.Add(CheckingRay(transform.right));
            if(CheckingRay(-transform.right) != null) _neighbors.Add(CheckingRay(-transform.right));
        }

        private int CheckingNeighbors(PixelCube excludedPixel)
        {
            for (int i = 0; i < _neighbors.Count; i++)
            {
                if (_neighbors.Contains(excludedPixel)) _neighbors.Remove(excludedPixel);
            }
            return _neighbors.Count;
        }

        private PixelCube CheckingRay(Vector3 direction)
        {
            _ray = new Ray(transform.position, direction);
            if (Physics.Raycast(_ray, out  _hit, _hitDistance))
            {
                if (_hit.collider.TryGetComponent(out PixelCube cube))
                {
                    if (cube.IsFallen) return null;
                    else return cube;
                }
            }
            return null;
        }

        public void Fall(Transform bulletPosition)
        {
            if (_isCanInvokeFallAction)
            {
                PixelObjectLauncher.Fall.Invoke();
            }
            for (int i = 0; i < _neighbors.Count; i++)
            {
                _neighbors[i].Neighbors.Remove(this);
            }
            _isFallen = true;
            _mainPixelObject.Cubes.Remove(this);
            _cube.SetActive(true);
            _cube.transform.SetParent(null);
            if (bulletPosition != null)
            {
                var explosionPosition = new Vector3(bulletPosition.position.x, bulletPosition.position.y - 1, bulletPosition.position.z);
                _cubeRigidbody.AddExplosionForce(300f, explosionPosition, 10f);
                _cubeRigidbody.angularVelocity = new Vector3(10, 4, 15);
            }

            if (gameObject.activeInHierarchy)
            {
                Destroy(gameObject);
            }
        }
    }
}
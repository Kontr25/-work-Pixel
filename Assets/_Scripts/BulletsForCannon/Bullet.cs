using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.PixelObject;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.BulletsForCannon
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _bulletRigidbody;
        [SerializeField] private float _shotForce;
        [SerializeField] private PixelObject.PixelObject _targetPixelObject;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material[] _materials;
        [SerializeField] private List<Vector3> _reflectPoint = new List<Vector3>();
        private LineRenderer _lineRenderer;
        private Transform _parent;
        private WaitForSeconds _bulletLifetime;
        private Coroutine _shotRoutine;
        private int _currentPointID = 2;
        private bool _isBulletFly = false;

        public LineRenderer CannonLineRenderer
        {
            get => _lineRenderer;
            set => _lineRenderer = value;
        }

        private void Awake()
        {
            _parent = transform.parent;
            _bulletLifetime = new WaitForSeconds(5f);
        }

        private void Start()
        {
            ChangeColor();
        }

        private void OnEnable()
        {
            _shotRoutine =  StartCoroutine((ShotBullet()));
        }
        
        private void OnDisable()
        {
            if (_shotRoutine != null)
            {
                StopCoroutine(_shotRoutine);
                _shotRoutine = null; 
            }
        }

        private void FixedUpdate()
        {
            if (_isBulletFly)
            {
                _bulletRigidbody.velocity = transform.forward * _shotForce;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PixelCube cube))
            {
                if (!cube.IsFallen)
                {
                    _targetPixelObject = cube.MainPixelOgject;
                    StartCoroutine(FastDisable());
                    cube.Fall(transform);
                }
            }
        }

        public void ChangeColor()
        {
            _meshRenderer.material.color = _materials[Random.Range(0, _materials.Length)].color;
        }
        
        private IEnumerator FastDisable()
        {
            yield return new WaitForSeconds(.2f);
            _targetPixelObject.CheckDestroy();
            gameObject.SetActive(false);
            transform.parent = _parent;
            _bulletRigidbody.velocity = Vector3.zero;
        }

        private IEnumerator ShotBullet()
        {
            AddPoints();
            transform.rotation = _parent.rotation;
            transform.parent = null;
            _isBulletFly = true;
            yield return _bulletLifetime;
            _isBulletFly = false;
            _bulletRigidbody.velocity = Vector3.zero;
            gameObject.SetActive(false);
            transform.parent = _parent;
        }

        private void AddPoints()
        {
            _reflectPoint.Clear();
            _currentPointID = 2;
            
            if(_lineRenderer != null){
                for (int i = 0; i < _lineRenderer.positionCount; i++)
                {
                    _reflectPoint.Add(_lineRenderer.GetPosition(i));
                }
            }
        }
        public void RotateToNextPoint()
        {
            if(_reflectPoint[_currentPointID] != null) {
                Vector3 direction = new Vector3(_reflectPoint[_currentPointID].x,_reflectPoint[_currentPointID].y, transform.position.z);
                transform.LookAt(direction);
                _currentPointID++;
            }
            
        }
    }
}
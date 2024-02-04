using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameMecanics
{
    public class PlayerBall : MonoBehaviour
    {
        [SerializeField] private Rigidbody _bulletRigidbody;
        [SerializeField] private float _shotForce;
        [SerializeField] private TrajectoryRenderer TrajectoryRenderer;
        [SerializeField] private List<Vector3> _reflectPoint = new List<Vector3>();
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private GameObject _cancelingSprite;
        private const float _rayPositionZ = 1.5f;
        private bool _isShot = false;
        [SerializeField] private bool _isBulletFly = false;
        private int _currentPointID = 2;
        private bool _canShot = false;

        [SerializeField] private Transform[] _linePoints;

        private void FixedUpdate()
        {
            if (_isBulletFly)
            {
                _bulletRigidbody.velocity = transform.forward * _shotForce;
            }
            
        }

        

        public bool CanShot
        {
            get => _canShot;
            set
            {
                if (value)
                {
                    _cancelingSprite.SetActive(false);
                    TrajectoryRenderer.StartRenderTrajectory();
                }
                else
                {
                    _cancelingSprite.SetActive(true);
                    TrajectoryRenderer.StopRenderTrajectory();
                }
                _canShot = value;
            }
        }

        public bool IsShot
        {
            get => _isShot;
            set => _isShot = value;
        }

        public bool IsBulletFly
        {
            get => _isBulletFly;
            set => _isBulletFly = value;
        }

        public void OnStart(Vector3 tapPosition)
        {
            if (!_isShot)
            {
                transform.position = new Vector3(tapPosition.x, tapPosition.y, _rayPositionZ);
                gameObject.SetActive(true);
                TrajectoryRenderer.StartRenderTrajectory();
            }
        }

        public void Shot(Vector3 shotDirection)
        {
            if (!_isShot)
            {
                _isShot = true;
                TrajectoryRenderer.StopRenderTrajectory();
                AddPoints();
                _isBulletFly = true;
            }
                
        }
            
        private void AddPoints()
        {
            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                _reflectPoint.Add(_lineRenderer.GetPosition(i));
            }
        }

        public void RotateToNextPoint()
        {
            transform.LookAt(_reflectPoint[_currentPointID]);
            _currentPointID++;
        }

        public void RotateToTarget(Vector3 shotDirection)
        {
            transform.rotation = Quaternion.LookRotation(shotDirection);
        }

        public void CancelShot()
        {
            gameObject.SetActive(false);
            TrajectoryRenderer.StopRenderTrajectory();
        }
    }
}
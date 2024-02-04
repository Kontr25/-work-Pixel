using System.Collections;
using System.Collections.Generic;
using _Scripts.PixelObject;
using _Scripts.Sound;
using GameMecanics;
using UnityEngine;

namespace _Scripts.Slingshot
{
    public class SlingshotBullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _bulletRigidbody;
        [SerializeField] private float _shotForce;
        [SerializeField] private PixelObject.PixelObject _targetPixelObject;
        [SerializeField] private List<Vector3> _reflectPoint = new List<Vector3>();
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;
        [SerializeField] private TrajectoryRenderer _slingshotTrajectoryRenderer;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _divider;
        [SerializeField] private GameObject _cancelingSprite;
        [SerializeField] private float _bulletLifetimeValue;
        [SerializeField] private float _bulletHitDelayValue;
        [SerializeField] private GameObject _mesh;
        [SerializeField] private ParticleSystem _explosionprefab;
        [SerializeField] private Vector3 _explosionScale;
        [SerializeField] private AudioSource _explosionSound;
        [SerializeField] private TrailRenderer _trail;
        [SerializeField] private bool _isGranate;

        private bool _disableStarted = false;
        private Vector3 _defaultScale;
        private bool _canShot = false;
        private Transform _parent;
        private WaitForSeconds _bulletLifetime;
        private WaitForSeconds _bulletHitDelay;
        private Coroutine _shotRoutine;
        private int _currentPointID = 2;
        private bool _isBulletFly = false;
        private bool _isCanRotate = true;
        
        public bool CanShot
        {
            get => _canShot;
            set
            {
                if (value && _cancelingSprite.activeInHierarchy)
                {
                    _cancelingSprite.SetActive(false);
                    _slingshotTrajectoryRenderer.StartRenderTrajectory();
                }
                else if (!value && !_cancelingSprite.activeInHierarchy)
                {
                    _cancelingSprite.SetActive(true);
                    _slingshotTrajectoryRenderer.StopRenderTrajectory();
                }
                _canShot = value;
            }
        }

        public TrajectoryRenderer SlingshotTrajectoryRenderer
        {
            get => _slingshotTrajectoryRenderer;
            set => _slingshotTrajectoryRenderer = value;
        }

        public float ShotForce
        {
            get => _shotForce;
            set => _shotForce = value;
        }

        public bool IsBulletFly => _isBulletFly;

        private void Start()
        {
            _defaultScale = transform.localScale;
            _bulletHitDelay = new WaitForSeconds(_bulletHitDelayValue);
        }

        private void Awake()
        {
            _parent = transform.parent;
            _bulletLifetime = new WaitForSeconds(_bulletLifetimeValue);
        }

        public void Shot()
        {
            if (gameObject.activeInHierarchy)
            {
                _shotRoutine =  StartCoroutine((ShotBullet()));
            }
        }
        
        private void OnDisable()
        {
            if (_shotRoutine != null)
            {
                _mesh.SetActive(true);
                transform.localScale = _defaultScale;
                StopCoroutine(_shotRoutine);
                _shotRoutine = null;
                _isBulletFly = false;
                _bulletRigidbody.velocity = Vector3.zero;
                _slingshotTrajectoryRenderer.StopRenderTrajectory();
                _disableStarted = false;
            }
        }

        private void OnEnable()
        {
            _trail.emitting = true;
        }

        public void ShotForceCalc(Vector3 stretchingPoint)
        {
            float distance = Vector3.Distance(stretchingPoint, transform.position);

            if (distance < _minDistance)
            {
                _shotForce = _minDistance * _divider;
            }
            else if (distance > _maxDistance)
            {
                _shotForce = _maxDistance * _divider;
            }
            else
            {
                _shotForce = distance * _divider;
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
                    SoundManager.DestroySound.Invoke();
                    
                    if(transform.localScale.x > .8f)
                    {
                        transform.localScale = new Vector3(transform.localScale.x - .05f, transform.localScale.y - .05f,
                        transform.localScale.z - .05f);
                        
                    }
                    cube.MainPixelOgject.CheckDestroy();
                    _targetPixelObject = cube.MainPixelOgject;
                    if (!_disableStarted)
                    {
                        cube.MainPixelOgject.PlayDestroySound();
                        _disableStarted = true;
                        StartCoroutine(FastDisableWithDelay());
                    }
                    cube.Fall(transform);
                }
            }
        }
        
        private IEnumerator FastDisableWithDelay()
        {
            yield return _bulletHitDelay;
            if (_isGranate)
            {
                _mesh.SetActive(false);
                _trail.emitting = false;
                transform.localScale = _explosionScale;
                _explosionSound.Play();
                _explosionprefab.Play();
            }
            
            yield return new WaitForSeconds(.2f);
            FastDisable();
            
        }

        public void FastDisable()
        {
            gameObject.SetActive(false);
            transform.parent = _parent;
            _bulletRigidbody.velocity = Vector3.zero;
        }

        private IEnumerator ShotBullet()
        {
            AddPoints();
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
            if(_reflectPoint[_currentPointID] != null && _currentPointID > 0 && _isCanRotate)
            {
                _isCanRotate = false;
                Vector3 direction = new Vector3(_reflectPoint[_currentPointID].x,_reflectPoint[_currentPointID].y, transform.position.z);
                transform.LookAt(direction);
                _currentPointID++;
                Invoke(nameof(CanRotate), .1f);
                SoundManager.RicochetSound.Invoke();
            }
        }

        private void CanRotate()
        {
            _isCanRotate = true;
        }

        public void RotateToTarget(Vector3 shotDirection)
        {
            transform.rotation = Quaternion.LookRotation(shotDirection);
        }

        public void LineRendererSetCollor(Color color)
        {
            _slingshotTrajectoryRenderer.SetLineRendererColor(color);
        }
    }
}
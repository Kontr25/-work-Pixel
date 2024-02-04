using System.Collections;
using _Scripts.BulletsForCannon;
using _Scripts.Pool;
using GameMecanics;
using UnityEngine;

namespace _Scripts.Cannon
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private float _accelerationDelay;
        [SerializeField] private float _shotDelay;
        [SerializeField] private Bullet _bullet;
        [SerializeField] private Transform _shotPoint;
        [SerializeField] private int _pollCapacity;
        [SerializeField] private CannonRotator _cannonRotator;
        [SerializeField] private CannonVisual _cannonVisual;
        [SerializeField] private UnityEngine.Camera _mainCamera;
        [SerializeField] private CannonMagazine cannonMagazine;
        [SerializeField] private AudioSource _shotSound;
        [SerializeField] private CannonTrajectoryRenderer cannonTrajectoryRenderer;
        
        private ObjectPool<Bullet> _pool;
        private Coroutine _shooterRoutine;
        private Coroutine _accelerationShot;
        private WaitForSeconds _delayBetweenShots;

        private bool _isCanShoot = false;
        private bool _shoot;
        
        public bool IsCanShoot
        {
            get => _isCanShoot;
            set => _isCanShoot = value;
        }

        private void Start()
        {
            _pool = new ObjectPool<Bullet>(_bullet, _pollCapacity, _shotPoint)
            {
                AutoExpand = true
            };
            for (int i = 0; i < _pool.PoolList.Count; i++)
            {
                _pool.PoolList[i].CannonLineRenderer = cannonTrajectoryRenderer.CannonLineRenderer;
            }
            
            _delayBetweenShots = new WaitForSeconds(_shotDelay);
        }
        
        private void Update()
        {
            if (_isCanShoot)
            {
                if (Input.GetMouseButtonDown(0))
                    {
                        StartShot();
                    }
        
                    if (Input.GetMouseButton(0))
                    {
                        _cannonRotator.Rotate();
                    }
        
                    if (Input.GetMouseButtonUp(0))
                    {
                        StopShot();
                    }
            }
        }
        public void StartShot()
        {
            _shooterRoutine = StartCoroutine(Shot());
            cannonTrajectoryRenderer.StartRenderTrajectory();
        }
        
        private void StopShot()
        {
            StopCoroutine(_shooterRoutine);
            cannonTrajectoryRenderer.StopRenderTrajectory();
        }
        private IEnumerator Shot()
        {
            while (true)
            {
                yield return _delayBetweenShots;
                CreateBullet();
            }
        }
        
        private void CreateBullet()
        {
            if (cannonMagazine.Bullets > 0)
            {
                _shotSound.Play();
                cannonMagazine.Shot();
                _cannonVisual.Shot();
                var bullet = _pool.GetFreeElement();
                bullet.transform.position = _shotPoint.position;
            }
            else
            {
                _isCanShoot = false;
                StopShot();
                FinishAction.Finish.Invoke(FinishAction.FinishType.Lose);
            }
        }
    }
}
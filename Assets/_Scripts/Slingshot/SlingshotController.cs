using _Scripts.BulletsForCannon;
using _Scripts.EnvironmentElements;
using _Scripts.Pool;
using _Scripts.Tutorial;
using GameMecanics;
using UnityEngine;

namespace _Scripts.Slingshot
{
    public class SlingshotController : MonoBehaviour
    {
        [SerializeField] private LayerMask _layer;
        [SerializeField] private UnityEngine.Camera _mainCamera;
        [SerializeField] private CannonMagazine SlingshotMagazine;
        [SerializeField] private SlingshotBullet _slingshotBullet;
        [SerializeField] private Transform _shotPoint;
        [SerializeField] private int _pollCapacity;
        [SerializeField] private AudioSource _shotSound;
        [SerializeField] private float _bulletZPosition;
        [SerializeField] private StretchView _stretchView;
        [SerializeField] private BulletsCounter _bulletsCounter;
        [SerializeField] private LevelLoader _levelLoader;
        [SerializeField] private NewTutorial _tutorial;

        [Header("Level colors")] 
        [SerializeField] private Color _whiteBlueLevel;
        [SerializeField] private Color _whiteGreenLevel;
        [SerializeField] private Color _whitePinkLevel;

        private SlingshotBullet _currentBullet;
        private ObjectPool<SlingshotBullet> _pool;
        private bool _isCanShoot = false;
        private Vector3 _shotDirection;
        private Ray inputRay;
        RaycastHit inpuHit;
        
        public bool IsCanShoot
        {
            get => _isCanShoot;
            set => _isCanShoot = value;
        }

        private void Start()
        {
            _pool = new ObjectPool<SlingshotBullet>(_slingshotBullet, _pollCapacity, _shotPoint)
            {
                AutoExpand = true
            };
        }

        public void SetLineRendererColors(LevelColor levelColor)
        {
            switch (levelColor)
            {
                    case LevelColor.WhiteBlue:
                        SetLinerendererColor(_whiteBlueLevel);
                        break;
                    case LevelColor.WhiteGreen:
                        SetLinerendererColor(_whiteGreenLevel);
                        break;
                    case LevelColor.WhitePink:
                        SetLinerendererColor(_whitePinkLevel);
                        break;
            }
        }

        private void SetLinerendererColor(Color color)
        {
            for (int i = 0; i < _pool.PoolList.Count; i++)
            {
                _pool.PoolList[i].LineRendererSetCollor(color);
            }
        }

        public void MouseDown()
        {
            if (_levelLoader.CurrentLevel == 0)
            {
                _tutorial.Disable();
            }
            if (_bulletsCounter.BulletCount > 0)
            {
                inputRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.SphereCast(inputRay, .8f, out inpuHit, 1000, _layer))
                {
                    if (inpuHit.collider.TryGetComponent(out Border border))
                    {
                        _isCanShoot = false;
                        if (_currentBullet != null && !_currentBullet.IsBulletFly)
                        {
                            _currentBullet.FastDisable();
                        }
                        _currentBullet = null;
                        CreateBullet(inpuHit.point);
                        _stretchView.StartPosition = inpuHit.point;
                    }
                }
            }
            else
            {
                Invoke(nameof(CheckLose),2f);
            }
        }
        
        public void MouseDrag()
        {
            if (_bulletsCounter.BulletCount > 0)
            {
                inputRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.SphereCast(inputRay, .8f, out inpuHit, 1000, _layer))
                {
                    if (inpuHit.collider.TryGetComponent(out Border border))
                    {
                        Vector3 endPosition = new Vector3(inpuHit.point.x, inpuHit.point.y, _bulletZPosition);
                        _shotDirection = endPosition - _currentBullet.transform.position;
                        float distance = Vector3.Distance(endPosition,
                            new Vector3(_currentBullet.transform.position.x, _currentBullet.transform.position.y,
                                _bulletZPosition));

                        if (distance > 2f)
                        {
                            _isCanShoot = true;
                            _currentBullet.CanShot = true;
                            _currentBullet.RotateToTarget(-_shotDirection);
                            _currentBullet.SlingshotTrajectoryRenderer.VisibleTrajectory();
                            _stretchView.RenderStretch(endPosition);
                            _currentBullet.ShotForceCalc(inpuHit.point);
                        }
                        else
                        {
                            _currentBullet.CanShot = false;
                            _isCanShoot = false;
                            _currentBullet.SlingshotTrajectoryRenderer.CancelingShot(distance);
                        }
                    }
                }
            }
        }
        
        public void MouseUp()
        {
            if (_bulletsCounter.BulletCount > 0 || _levelLoader.CurrentLevel == 0)
            {
                _stretchView.RenderIs(false);
                if (_currentBullet != null && _isCanShoot)
                {
                    if (_levelLoader.CurrentLevel != 0)
                    {
                        _bulletsCounter.Shot();
                    }
                    _currentBullet.Shot();
                    _currentBullet.SlingshotTrajectoryRenderer.StopRenderTrajectory();
                    _isCanShoot = false;
                }
                else
                {
                    _currentBullet.FastDisable();
                }
            }
        }

        private void CheckLose()
        {
            if (_bulletsCounter.BulletCount < 1)
            {
                FinishAction.Finish.Invoke(FinishAction.FinishType.Lose);
            }
        }
        private void CreateBullet(Vector3 inpuHitPoint)
        {
                //SlingshotMagazine.Shot();
                var bullet = _pool.GetFreeElement();
                bullet.transform.position = new Vector3(inpuHitPoint.x, inpuHitPoint.y, _bulletZPosition);
                _currentBullet = bullet;
                _currentBullet.SlingshotTrajectoryRenderer.StartRenderTrajectory();
        }
    }
}
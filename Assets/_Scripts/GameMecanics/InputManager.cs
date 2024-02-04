using _Scripts.EnvironmentElements;
using _Scripts.Reflected;
using _Scripts.Tutorial;
using UnityEngine;

namespace GameMecanics
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private PlayerBall _playerBall;
        [SerializeField] private TrajectoryRenderer _trajectoryRenderer;
        [SerializeField] private TutorialActivator _tutorialActivator;
        [SerializeField] private StretchView _stretchView;

        private Vector3 _shotDirection;
        private Ray inputRay;
        RaycastHit inpuHit;
        private bool _isCanShot = false;

        private void Start()
        {
            if (_playerBall.gameObject.activeInHierarchy)
            {
                _playerBall.IsShot = false;
                _playerBall.IsBulletFly = false;
                _playerBall.gameObject.SetActive(false);
            }
        }

        public Vector3 ShotDirection
        {
            get => _shotDirection;
            set => _shotDirection = value;
        }

        public void MouseDown()
        {
            if (!_playerBall.IsBulletFly)
            {
                inputRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
    
                if (Physics.SphereCast(inputRay,.5f, out inpuHit))
                {
                    if (!inpuHit.collider.TryGetComponent(out Border border))
                    {
                        _isCanShot = false;
                        return;
                    }
                    else
                    {
                        _tutorialActivator.Disable();
                        _playerBall.OnStart(inpuHit.point);
                        _stretchView.StartPosition = new Vector3(_playerBall.transform.position.x, 
                            _playerBall.transform.position.y, 0.5f);
                        _isCanShot = true;
                    }
                }
            }
        }

        public void MouseDrag()
        {
            if (!_playerBall.IsBulletFly)
            {
                if (_isCanShot)
                {
                    inputRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
                        
                    if (Physics.SphereCast(inputRay,.5f, out inpuHit))
                    {
                        if (inpuHit.collider.TryGetComponent(out Border ground) || 
                            inpuHit.collider.TryGetComponent(out ReflectableObject reflectableObject))
                        {
                            Vector3 endPosition = new Vector3(inpuHit.point.x, inpuHit.point.y, 1.5f);
                            _shotDirection = endPosition - _playerBall.transform.position;

                            float distance = Vector3.Distance(endPosition,
                                new Vector3(_playerBall.transform.position.x, _playerBall.transform.position.y, 1.5f));
                                
                            if (distance > 2f)
                            {
                                _stretchView.RenderStretch(endPosition);
                                _playerBall.CanShot = true;
                                _trajectoryRenderer.VisibleTrajectory();
                                _playerBall.RotateToTarget(-_shotDirection);
                            }else
                            {
                                _playerBall.CanShot = false;
                                _trajectoryRenderer.CancelingShot(distance);
                            }
                        }
                    }
                }
            }
        }

        public void MouseUp()
        {
            if (!_playerBall.IsBulletFly)
            {
                _stretchView.RenderIs(false);
                if (_playerBall.CanShot)
                {
                    _isCanShot = false;
                    _playerBall.Shot(_shotDirection);
                }
                else
                {
                    _playerBall.CancelShot();
                }
            }
        }

    }
}

using _Scripts.Pool;
using _Scripts.Slingshot;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Multi
{
    public class Multiplicator : MonoBehaviour
    {
        [SerializeField] private float _multiolicateAngle;
        [SerializeField] private SlingshotBullet _slingshotBullet;
        [SerializeField] private int _pollCapacity;
        [SerializeField] private float _bulletZPosition;
        [SerializeField] private Transform _mesh;
        [SerializeField] private AudioSource _spawnSound;

        private float _currentShotForce;
        private Vector3 _defaultMeshScale;
        private ObjectPool<SlingshotBullet> _pool;
        private float _firstX;
        private float _secondX;
        private float _firstY;
        private float _secondY;
        private SlingshotBullet _firstBullet;
        private SlingshotBullet _secondBullet;

        private void Start()
        {
            _pool = new ObjectPool<SlingshotBullet>(_slingshotBullet, _pollCapacity, transform)
            {
                AutoExpand = true
            };
            _defaultMeshScale = _mesh.localScale;
        }
        
        private SlingshotBullet CreateBullet(Vector3 inpuHitPoint)
        {
            //SlingshotMagazine.Shot();
            var bullet = _pool.GetFreeElement();
            bullet.transform.position = new Vector3(inpuHitPoint.x, inpuHitPoint.y, _bulletZPosition);
            return bullet;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out SlingshotBullet bullet) && bullet != _firstBullet && bullet != _secondBullet)
            {
                Vector3 rotate = bullet.transform.eulerAngles;
                float xAngle = rotate.x;
                float yAngle = rotate.y;
                float zAngle = rotate.z;
                _currentShotForce = bullet.ShotForce;
                bullet.FastDisable();
                Debug.Log("Cyka");
                CreateTwoBullets(xAngle, yAngle, zAngle);
            }
        }

        private void CreateTwoBullets(float xAngle, float yAngle, float zAngle)
        {
            _firstY = yAngle;
            _secondY = yAngle;
            _firstX = xAngle - _multiolicateAngle;
            _secondX = xAngle +_multiolicateAngle;
            
            Quaternion firstRotate = Quaternion.Euler(_firstX, _firstY, zAngle);
            Quaternion secondRotate = Quaternion.Euler(_secondX, _secondY, zAngle);

            _firstBullet = CreateBullet(transform.position);
            _firstBullet.transform.rotation = firstRotate;
            _firstBullet.SlingshotTrajectoryRenderer.StartRenderTrajectory();
            _firstBullet.ShotForce = _currentShotForce;
            
            _secondBullet = CreateBullet(transform.position);
            _secondBullet.transform.rotation = secondRotate;
            _secondBullet.SlingshotTrajectoryRenderer.StartRenderTrajectory();
            _secondBullet.ShotForce = _currentShotForce;

            Invoke(nameof(Shot), .2f);
            
            _mesh.DOScale(_defaultMeshScale * 0.7f, .2f).onComplete = () =>
            {
                _mesh.localScale = _defaultMeshScale * 0.7f;
                _mesh.DOScale(_defaultMeshScale, .2f).onComplete = () =>
                {
                    _mesh.localScale = _defaultMeshScale;
                };
            };
        }

        private void Shot()
        {
            _spawnSound.Play();
            _firstBullet.Shot();
            _secondBullet.Shot();
        }
    }
}
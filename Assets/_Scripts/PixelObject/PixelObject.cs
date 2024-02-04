using System.Collections;
using System.Collections.Generic;
using _Scripts.BulletsForCannon;
using _Scripts.Camera;
using UnityEngine;

namespace _Scripts.PixelObject
{
    public class PixelObject : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private List<PixelCube> _cubes = new List<PixelCube>();
        [SerializeField] private List<PixelCube> _verifiedPixel = new List<PixelCube>();
        [SerializeField] private List<PixelCube> _unVerifiedPixel = new List<PixelCube>();
        [SerializeField] private int _minNumberCube;
        [SerializeField] private bool _firstObject;
        [SerializeField] private bool _isDestroy = false;
        [SerializeField] private bool _isSaw = false;
        [SerializeField] private bool _isBulletContainer;
        [SerializeField] private List<Rigidbody> _bullets;
        [SerializeField] private List<Collider> _bulletsColliders;
        [SerializeField] private BulletsCounter _bulletsCounter;
        [SerializeField] private Collider[] _colliders;
        [SerializeField] private ContainerColor _containerColor;
        [SerializeField] private AudioSource _destroySound;
        
        [Range(1, 8)]
        [SerializeField] private int _bulletCount;
        [SerializeField] private GameObject[] _pixelCubes;

        private Coroutine _checkDestroyRoutine;

        public List<PixelCube> Cubes
        {
            get => _cubes;
            set => _cubes = value;
        }

        public Rigidbody RigidbodyPixelObject
        {
            get => _rigidbody;
            set => _rigidbody = value;
        }

        public bool IsSaw
        {
            get => _isSaw;
            set => _isSaw = value;
        }

        public AudioSource DestroySound
        {
            get => _destroySound;
            set => _destroySound = value;
        }

        public ContainerColor ColorContainer
        {
            get => _containerColor;
            set => _containerColor = value;
        }

        public int BulletCount
        {
            get => _bulletCount;
            set => _bulletCount = value;
        }

        public BulletsCounter Counter
        {
            get => _bulletsCounter;
            set => _bulletsCounter = value;
        }

        private void Awake()
        {
            for (int i = 0; i < _bulletCount; i++)
            {
                _bullets[i].gameObject.SetActive(true);
            }
            if (_isBulletContainer)
            {
                switch (_containerColor)
                {
                    case ContainerColor.Blue:
                        _pixelCubes[0].SetActive(true);
                        break;
                    case ContainerColor.Orange:
                        _pixelCubes[1].SetActive(true);
                        break;
                    case ContainerColor.Green:
                        _pixelCubes[2].SetActive(true);
                        break;
                    case ContainerColor.Pink:
                        _pixelCubes[3].SetActive(true);
                        break;
                    case ContainerColor.White:
                        _pixelCubes[4].SetActive(true);
                        break;
                    case ContainerColor.Yellow:
                        _pixelCubes[5].SetActive(true);
                        break;
                    case ContainerColor.Red:
                        _pixelCubes[6].SetActive(true);
                        break;
                }
            }
            
            if (_firstObject)
            {
                ListUpdate();
            }
        }

        public void PlayDestroySound()
        {
            _destroySound.Play();
        }

        public void CheckDestroy()
        {
            if (_checkDestroyRoutine != null)
            {
                StopCoroutine(_checkDestroyRoutine);
            }
            _checkDestroyRoutine = StartCoroutine(CheckDestroyRoutine());
        }

        private IEnumerator CheckDestroyRoutine()
        {
            Clear();
            if(_cubes.Count > 0) _cubes[0].StartCheckNeighbors();
            for (int i = 0; i < _cubes.Count; i++)
            {
                if (!_cubes[i].IsVerified)
                {
                    _unVerifiedPixel.Add(_cubes[i]);
                }
            }
            
            if (_unVerifiedPixel.Count <= 0)
            {
                for (int i = 0; i < _cubes.Count; i++)
                {
                    _cubes[i].IsVerified = false;
                }
                yield break;
            }
            else
            {
                if (!_isDestroy)
                {
                    Debug.Log("Destroy");
                    if (_isBulletContainer)
                    {
                        for (int i = 0; i < _bulletCount; i++)
                        {
                            _bullets[i].transform.SetParent(null);
                            _bulletsCounter.BulletCount++;
                        }

                        for (int i = 0; i < _colliders.Length; i++)
                        {
                            _colliders[i].enabled = false;
                        }

                        _bulletsCounter.Bullets = new List<Rigidbody>();
                        _bulletsCounter.BulletsColliders = new List<Collider>();
                        for (int i = 0; i < _bullets.Count; i++)
                        {
                            _bulletsCounter.Bullets.Add(_bullets[i]);
                            _bulletsCounter.BulletsColliders.Add(_bulletsColliders[i]);
                        }
                        _bulletsCounter.ExplosionBullet(transform.position);
                    }
                    _isDestroy = true;
                    CreateTwoObjects();
                }
            }
            yield break;
        }

        private void CreateTwoObjects()
        {
            CameraEffects.ShakeKamera.Invoke();
            var newObject1 = EmptyPixelObject(transform.position);
            
            for (int i = 0; i < _unVerifiedPixel.Count; i++)
            {
                if (_unVerifiedPixel[i] != null)
                {
                    _unVerifiedPixel[i].transform.SetParent(newObject1.transform);
                    if(_cubes.Contains(_unVerifiedPixel[i])) _cubes.Remove(_unVerifiedPixel[i]);
                    newObject1.Cubes.Add(_unVerifiedPixel[i]);
                    _unVerifiedPixel[i].MainPixelOgject = newObject1;
                }
            }
            newObject1.Clear();
            
            for (int i = 0; i < _cubes.Count; i++)
            {
                _cubes[i].IsVerified = false;
            }
            var newObject2 = EmptyPixelObject(transform.position);
            newObject2.name = "второй клон";
            for (int i = 0; i < _cubes.Count; i++)
            {
                _cubes[i].transform.SetParent(newObject2.transform);
                newObject2.Cubes.Add(_cubes[i]);
                _cubes[i].MainPixelOgject = newObject2;
            }
            newObject2.Clear();
            
            Destroy(gameObject, .2f);
        }

        private PixelObject EmptyPixelObject(Vector3 position)
        {
            GameObject newObj = new GameObject();
            PixelObject pixelObj = newObj.AddComponent<PixelObject>();
            Rigidbody pixelObjRB = pixelObj.gameObject.AddComponent<Rigidbody>();
            pixelObjRB.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

            pixelObj.DestroySound = newObj.AddComponent<AudioSource>();
            pixelObj.DestroySound.clip = _destroySound.clip;
            newObj.transform.position = position;
            return pixelObj;
        }

        public void Clear()
        {
            for (int i = 0; i < _cubes.Count; i++)
            {
                _cubes[i].IsVerified = false;
            }
        }

        public void ListUpdate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    foreach (Transform Cube in transform.GetChild(i))
                    {
                        if (Cube.TryGetComponent(out PixelCube pixelCube))
                        {
                            if (!_cubes.Contains(pixelCube))
                            {
                                _cubes.Add(pixelCube);
                                pixelCube.MainPixelOgject = this;
                                if (_isSaw)
                                {
                                    pixelCube.IsCanInvokeFallAction = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void FallLastCubes()
        {
            if (_cubes.Count > _minNumberCube)
            {
                return;
            }
            else
            {
                for (int i = 0; i < _cubes.Count; i++)
                {
                    _cubes[i].Fall(transform);
                }
                Invoke(nameof(Victory), 2f);
            }
        }

        private void Victory()
        {
            FinishAction.Finish.Invoke(FinishAction.FinishType.Win);
        }

        public void AddVerifiedPixel(PixelCube pixel)
        {
            if (!_verifiedPixel.Contains(pixel))
            {
                _verifiedPixel.Add(pixel);
            }
        }
    }

    public enum ContainerColor
    {
        Blue,
        Orange,
        Green,
        Pink,
        White,
        Yellow,
        Red
    }
}
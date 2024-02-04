using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.BulletsForCannon
{
    public class BulletsCounter : MonoBehaviour
    {
        [SerializeField] private List<Rigidbody> _bullets = new List<Rigidbody>();
         private List<Collider> _bulletsColliders = new List<Collider>();
        private int _bulletCount = 2;
        
        [SerializeField] private TMP_Text _bulletsCountText;
        [SerializeField] private LevelLoader _levelLoader;
        [SerializeField] private UnityEngine.Camera _mainCamera;
        [SerializeField] private Image _icon;


        private void Start()
        {
            Vector3 newPos = _mainCamera.ScreenToWorldPoint(new Vector3(_icon.transform.position.x,
                _icon.transform.position.y, 15));
            transform.position = newPos;
            Invoke(nameof(UpdateText), .3f);
        }

        private void UpdateText()
        {
            if (_levelLoader.CurrentLevel > 0)
            {
                _bulletsCountText.text = _bulletCount.ToString();
            }
            else
            {
                Infinity();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BulletFromBox bullet))
            {
                _bulletsCountText.text = _bulletCount.ToString();
                Destroy(bullet.gameObject, 1f);
            }
        }

        public List<Rigidbody> Bullets
        {
            get => _bullets;
            set => _bullets = value;
        }

        public int BulletCount
        {
            get => _bulletCount;
            set => _bulletCount = value;
        }

        public List<Collider> BulletsColliders
        {
            get => _bulletsColliders;
            set => _bulletsColliders = value;
        }

        public void ExplosionBullet(Vector3 explosionPosition)
        {
            Debug.Log("LETITTTTTTTTTTTTTTTTTTTTTTTTTTTTTOwjkmvcAAAAAAAAAAAAAAAAAAA");
            StartCoroutine(ExplosionWithDelay(explosionPosition));
        }

        private IEnumerator ExplosionWithDelay(Vector3 explosionPosition)
        {
            for (int i = 0; i < _bullets.Count; i++)
            {
                _bulletsColliders[i].isTrigger = true;
                _bullets[i].useGravity = false;
                _bullets[i].AddExplosionForce(100f,new Vector3(explosionPosition.x, explosionPosition.y, 5f) , 100);
            }
            yield return new WaitForSeconds(.6f);
            for (int i = 0; i < _bullets.Count; i++)
            {
                if (_bullets[i] != null)
                {
                    _bullets[i].isKinematic = true;
                    _bullets[i].DOMove(transform.position, 1f);
                }
            }
        }

        public void Shot()
        {
            _bulletCount--;
            _bulletsCountText.text = _bulletCount.ToString();
        }

        public void Infinity()
        {
            Debug.Log("INFINIIIIIIIIIIITTTTTTTTYYYYYYYYYYYY");
            _bulletsCountText.text = "∞";
        }
    }
}
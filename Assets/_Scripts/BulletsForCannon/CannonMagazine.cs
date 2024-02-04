using _Scripts.Reflected;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.BulletsForCannon
{
    public class CannonMagazine : MonoBehaviour
    {
        [SerializeField] private TMP_Text _bulletCount;
        [SerializeField] private Transform _cannon;
        [SerializeField] private BulletsInBox[] _bulletsInBoxes;
        
        private int _bullets = 0;
        private float _maxBulletCount = 0;
        private Vector3 _defaultScale;

        public int Bullets => _bullets;

        private void Start()
        {
            _defaultScale = _cannon.localScale;
            for (int i = 0; i < _bulletsInBoxes.Length; i++)
            {
                _maxBulletCount += _bulletsInBoxes[i].MaxCountForCannon;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BulletFromBox bullet))
            {
                _bullets += bullet.CountForCannon;
                _bulletCount.text = _bullets.ToString();
                Destroy(bullet.gameObject);
                Punch();
            }
        }

        private void Punch()
        {
            _cannon.DOScale(_defaultScale * 1.3f, .1f).onComplete = () =>
            {
                _cannon.DOScale(_defaultScale, .1f).onComplete = () =>
                {
                    _cannon.localScale = _defaultScale;
                };
            };
        }

        public void Shot()
        {
            _bullets--;
            _bulletCount.text = _bullets.ToString();
        }
    }
}
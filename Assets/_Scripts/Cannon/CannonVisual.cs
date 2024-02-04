using UnityEngine;
using DG.Tweening;

namespace _Scripts.Cannon
{
    public class CannonVisual : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _cannonShotEffect;
        [SerializeField] private float _zJumpPos;

        private Vector3 _defaultScale;
        private Vector3 _defaultPosition;
        private Vector3 _punchScale = new Vector3(1.3f, 1.3f, 0.8f);

        private void Start()
        {
            _defaultScale = transform.localScale;
            _defaultPosition = transform.localPosition;
        }

        public void Shot()
        {
            _cannonShotEffect.Play();
            transform.DOScale(_punchScale, .1f).onComplete = () =>
            {
                transform.DOScale(_defaultScale, 0.2f).onComplete = () =>
                {
                    transform.localScale = _defaultScale;
                };
            };
            
            transform.DOLocalMoveZ(transform.position.z - _zJumpPos, .1f).onComplete = () =>
            {
                transform.DOLocalMoveZ(_defaultPosition.z, .2f);
            };
        }
    }
}
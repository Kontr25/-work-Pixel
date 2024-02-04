using DG.Tweening;
using UnityEngine;

namespace _Scripts.Jumpers
{
    public class CannonMover: EnvironmentObjectMover
    {
        [SerializeField] private Cannon.Cannon _cannon;
        [SerializeField] private Transform _moveTarget;
        [SerializeField] private ParticleSystem _dustEffect;
        
        public override void MoveToPosition()
        {
            transform.DOMove(_moveTarget.position, _moveDuration).onComplete = () =>
            {
                _dustEffect.Play();
            };
            _cannon.IsCanShoot = true;
        }
    }
}
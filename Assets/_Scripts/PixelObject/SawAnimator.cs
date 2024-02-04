using _Scripts.BulletsForCannon;
using UnityEngine;

namespace _Scripts.PixelObject
{
    public class SawAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Bullet bullet))
            {
                _animator.enabled = false;
                enabled = false;
            }
        }
    }
}
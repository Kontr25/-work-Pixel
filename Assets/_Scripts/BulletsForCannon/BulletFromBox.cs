using UnityEngine;

namespace _Scripts.BulletsForCannon
{
    public class BulletFromBox : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        private int _countForCannon = 0;

        public int CountForCannon
        {
            get => _countForCannon;
            set => _countForCannon = value;
        }

        public Rigidbody bulletRigidbody => _rigidbody;

    }
}
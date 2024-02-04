using UnityEngine;

namespace GameMecanics
{
    public class BulletSimulate: MonoBehaviour
    {
        [SerializeField] private Rigidbody _bulletRigidbody;
        [SerializeField] private float _shotForce;

        public void Disable()
        {
            _bulletRigidbody.velocity = Vector3.zero;
        }

        public void Shot(Vector3 inputManagerShotDirection)
        {
            _bulletRigidbody.AddForce(inputManagerShotDirection.normalized * _shotForce, ForceMode.Impulse);
        }
    }
}

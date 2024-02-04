using UnityEngine;

namespace _Scripts.EnvironmentElements
{
    public class PadLock : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] _rigidbodies;

        public void ExplosionPadlock()
        {
            for (int i = 0; i < _rigidbodies.Length; i++)
            {
                _rigidbodies[i].transform.SetParent(null);
                _rigidbodies[i].isKinematic = false;
                _rigidbodies[i].AddExplosionForce(500f, transform.position, 10);
            }
            gameObject.SetActive(false);
        }
    }
}
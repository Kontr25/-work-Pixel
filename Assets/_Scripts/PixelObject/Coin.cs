using DG.Tweening;
using UnityEngine;

namespace _Scripts.PixelObject
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private Rigidbody _coinRigidbody;
        private void Start()
        {
            transform.parent = null;
            transform.DOScale(1f, .5f).onComplete = () =>
            {
                transform.localScale = Vector3.one;
            };
            _coinRigidbody.AddExplosionForce(100f, -transform.forward, 10f);
            Invoke(nameof(Disable),3f);
        }

        private void Disable()
        {
            transform.DOScale(0.1f, .5f).onComplete = () =>
            {
                Destroy(transform.gameObject);
            };
        }
    }
}
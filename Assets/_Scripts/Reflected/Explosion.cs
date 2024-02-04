using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Reflected
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] _parts;

        public void StartExplosion()
        {
            for (int i = 0; i < _parts.Length; i++)
            {
                _parts[i].gameObject.SetActive(true);
                _parts[i].AddExplosionForce(300,transform.position, 10f);
                StartCoroutine(DestroyWithDelay(_parts[i].gameObject));
            }
        }

        private IEnumerator DestroyWithDelay(GameObject part)
        {
            yield return new WaitForSeconds(2f);
            part.transform.DOScale(0.01f, 2f).onComplete = () =>
            {
                Destroy(part);
            };
        }
    }
}
using System.Collections;
using _Scripts.PixelObject;
using _Scripts.Pool;
using UnityEngine;

namespace _Scripts.Shreder
{
    public class Shreder : MonoBehaviour
    {
        [SerializeField] private float _destroyDelayValue;
        [SerializeField] private int _poolCapacity;
        [SerializeField] private Counter _counter;
        [SerializeField] private float _fallenCubeDestroyDelayValue;

        private ObjectPool<Counter> _pool;
        private WaitForSeconds _destroyDelay;
        private WaitForSeconds _fallenCubeDestroyDelay;
        private bool _isGameOver = false;

        private void Start()
        {
            _destroyDelay = new WaitForSeconds(_destroyDelayValue);
            _fallenCubeDestroyDelay = new WaitForSeconds(_fallenCubeDestroyDelayValue);
            _pool = new ObjectPool<Counter>(_counter, _poolCapacity, transform)
            {
                AutoExpand = false
            };

            for (int i = 0; i < _pool.PoolList.Count; i++)
            {
                _pool.PoolList[i].ParentTransform = transform;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PixelCube cube) && !cube.IsFallen)
            {
                StartCoroutine(DestroyWithDelay(cube));
            }
            
            if (other.TryGetComponent(out FallenCube fallenCube) && !fallenCube.DestroyIsStarted)
            {
                fallenCube.DestroyIsStarted = true;
                StartCoroutine(CreateCounter(fallenCube.transform.position));
                fallenCube.DestroyWithDelay(_fallenCubeDestroyDelayValue + 0.1f);
            }
        }

        private IEnumerator DestroyWithDelay(PixelCube cube)
        {
            yield return _destroyDelay;
            if (cube != null)
            {
                cube.Fall(null);
            }
        }
        
        private IEnumerator CreateCounter(Vector3 position)
        {
            yield return _fallenCubeDestroyDelay;
            var counter = _pool.GetFreeElement();
            if (counter != null)
            {
                counter.transform.position = position;
            }
        }
    }
}
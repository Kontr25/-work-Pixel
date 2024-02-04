using UnityEngine;

namespace _Scripts.Shreder
{
    public class Gear : MonoBehaviour
    {
        [SerializeField] private float _zRotationSpeed;
        private void Update()
        {
            transform.Rotate(0,0,_zRotationSpeed * Time.deltaTime);
        }
    }
}
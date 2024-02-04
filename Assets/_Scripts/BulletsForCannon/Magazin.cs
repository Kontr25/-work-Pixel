using UnityEngine;

namespace _Scripts.BulletsForCannon
{
    public class Magazin : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BulletFromBox bulletFromBox))
            {
                bulletFromBox.transform.SetParent(transform);
            }
        }
    }
}
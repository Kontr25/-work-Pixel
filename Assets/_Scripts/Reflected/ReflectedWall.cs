using _Scripts.BulletsForCannon;
using _Scripts.Slingshot;
using UnityEngine;

namespace _Scripts.Reflected
{
    public class ReflectedWall: ReflectableObject
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Bullet bullet))
            {
                bullet.RotateToNextPoint();
            }
            
            if (other.TryGetComponent(out SlingshotBullet slingshotBulletbullet))
            {
                Debug.Log("IHJGVJKHDBKSDJVNLKSDNVLKSDNMV:LSDMV:LS<DV");
                slingshotBulletbullet.RotateToNextPoint();
            }
        }
    }
}
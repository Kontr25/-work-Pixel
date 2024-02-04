using DG.Tweening;
using UnityEngine;

namespace _Scripts.Jumpers
{
    public abstract class EnvironmentObjectMover : MonoBehaviour
    {
        [SerializeField] protected float _moveDuration;

        public virtual void MoveToPosition()
        {
            
        }
    }
}
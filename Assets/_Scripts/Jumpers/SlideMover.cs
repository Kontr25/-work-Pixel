using DG.Tweening;
using UnityEngine;

namespace _Scripts.Jumpers
{
    public class SlideMover: EnvironmentObjectMover
    {
        [SerializeField] private float _delay;
        public override void MoveToPosition()
        {
            Invoke(nameof(Move), _delay);
        }
        private void Move()
         {
             transform.DOLocalMoveZ(transform.position.z + 10, _moveDuration);
         }
    }
}
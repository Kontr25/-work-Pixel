using _Scripts.Jumpers;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.EnvironmentElements
{
    public class ClawMachine : EnvironmentObjectMover
    {
        public override void MoveToPosition()
        {
            transform.DOMoveY(transform.position.y + 15, _moveDuration);
        }
    }
}
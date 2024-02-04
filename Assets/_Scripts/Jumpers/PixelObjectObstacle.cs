using DG.Tweening;

namespace _Scripts.Jumpers
{
    public class PixelObjectObstacle : EnvironmentObjectMover
    {
        public override void MoveToPosition()
        {
            transform.DOLocalMoveZ(0, _moveDuration);
        }
    }
}
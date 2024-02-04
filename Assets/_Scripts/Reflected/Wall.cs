using DG.Tweening;
using GameMecanics;
using UnityEngine;

namespace _Scripts.Reflected
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private float _jumpDuration;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerBall player))
            {
                player.RotateToNextPoint();
            }
        }
        
        public void Move(Vector3 jumpTarget)
        {
            transform.DOMove(jumpTarget, _jumpDuration);
        }
    }
}
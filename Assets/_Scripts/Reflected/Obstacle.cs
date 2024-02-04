using System.Collections.Generic;
using _Scripts.Camera;
using _Scripts.Finish;
using GameMecanics;
using UnityEngine;

namespace _Scripts.Reflected
{
    public class Obstacle: ReflectableObject
    {
        [SerializeField] private GameObject _defauldBlock;
        [SerializeField] private GameObject _destroyedBlock;
        [SerializeField] private Explosion _explosionBox;
        [SerializeField] private GameMechanicsSwitcher gameMechanicsSwitcher;
        [SerializeField] private List<Rigidbody> _bullets = new List<Rigidbody>();

        public List<Rigidbody> BulletsRigidbody
        {
            get => _bullets;
            set => _bullets = value;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerBall player) && IsContacted == false)
            {
                OnContact(player);
                Contact();
            }
        }

        public void Contact()
        {
            IsContacted = true;
            gameMechanicsSwitcher.CheckAllContacts();
        }

        private void OnContact(PlayerBall playerBall)
        {
            DestroyBlock();
            playerBall.RotateToNextPoint();
        }

        public void DestroyBlock()
        {
            CameraEffects.ShakeKamera.Invoke();
            _defauldBlock.SetActive(false);
            _destroyedBlock.SetActive(true);
            _explosionBox.StartExplosion();
            for (int i = 0; i < _bullets.Count; i++)
            {
                _bullets[i].isKinematic = false;
            }
        }
    }
}
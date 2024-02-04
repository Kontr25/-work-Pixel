using _Scripts.Jumpers;
using _Scripts.PixelObject;
using _Scripts.Reflected;
using GameMecanics;
using UnityEngine;

namespace _Scripts.Finish
{
    public class GameMechanicsSwitcher : MonoBehaviour
    {
        
        [SerializeField] private ReflectableObject[] _reflectableObjects;
        [SerializeField] private EnvironmentObjectMover[] _environmentObjects;
        [SerializeField] private Wall[] _walls;
        [SerializeField] private Collider[] _reflectedWall;
        [SerializeField] private PixelObjectLauncher _pixelObjectLauncher;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private GameObject _slingshotInputManager;
        [SerializeField] private StretchView _stretchView;

        public void CheckAllContacts()
        {
            for (int i = 0; i < _reflectableObjects.Length; i++)
            {
                if (!_reflectableObjects[i].IsContacted)
                {
                    return;
                }
            }
            CannonActivate();
        }

        private void CannonActivate()
        {
            _inputManager.gameObject.SetActive(false);
            _slingshotInputManager.SetActive(true);
            _pixelObjectLauncher.StartLaunchObject();
            _stretchView.transform.position =
                new Vector3(_stretchView.transform.position.x, _stretchView.transform.position.y, 4);
            
            for (int i = 0; i < _reflectedWall.Length; i++)
            {
                _reflectedWall[i].enabled = true;
            }
            
            for (int j = 0; j < _environmentObjects.Length; j++)
            {
                _environmentObjects[j].MoveToPosition();
            }

            for (int i = 0; i < _walls.Length; i++)
            {
                Vector3 _jumpTarget;
                if (_walls[i].transform.position.x > 0)
                {
                    _jumpTarget = new Vector3(_walls[i].transform.position.x + 15, _walls[i].transform.position.y,
                        _walls[i].transform.position.z);
                }
                else
                {
                    _jumpTarget = new Vector3(_walls[i].transform.position.x - 15, _walls[i].transform.position.y,
                        _walls[i].transform.position.z);
                }
                _walls[i].Move(_jumpTarget);
            }
        }

        public void CheckLoseAction()
        {
            for (int i = 0; i < _reflectableObjects.Length; i++)
            {
                if (!_reflectableObjects[i].IsContacted)
                {
                    FinishAction.Finish.Invoke(FinishAction.FinishType.Lose);
                }
            }
        }
    }
}
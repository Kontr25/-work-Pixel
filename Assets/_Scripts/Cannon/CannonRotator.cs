using UnityEngine;

namespace _Scripts.Cannon
{
    public class CannonRotator : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _mainCamera;
        [SerializeField] private Transform _lookTarget;
        private Ray _ray;
        private RaycastHit _hit;
        public void Rotate()
        {
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit))
            {
                _lookTarget.position = new Vector3(_hit.point.x, _hit.point.y, transform.position.z);
                transform.LookAt(_lookTarget);
            }
        }
    }
}
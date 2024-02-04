using UnityEngine;

namespace _Scripts.Reflected
{
    public abstract class ReflectableObject : MonoBehaviour
    {
        private bool _isContacted = false;

        public bool IsContacted
        {
            get => _isContacted;
            set => _isContacted = value;
        }
    }
}
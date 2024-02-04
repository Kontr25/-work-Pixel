using UnityEngine;

namespace _Scripts.PixelObject
{
    public class PixelObjectPrefabs : MonoBehaviour
    {
        [SerializeField] private PixelObject[] _prefabs;

        public PixelObject[] Prefabs
        {
            get => _prefabs;
            set => _prefabs = value;
        }
    }
}
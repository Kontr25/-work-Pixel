using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.PixelObject
{
    public class PixelObjectsParent : MonoBehaviour
    {
        [SerializeField] private PixelObject[] _pixelObjects;

        public PixelObject[] PixelObjects
        {
            get => _pixelObjects;
            set => _pixelObjects = value;
        }
    }
}
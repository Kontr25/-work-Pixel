using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.PixelObject
{
    public class PixelGrid : MonoBehaviour
    {
        [SerializeField] private List<PixelCube> _cubes = new List<PixelCube>();
        
        private void ListUpdate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                foreach (Transform Cube in transform.GetChild(i))
                {
                    if (Cube.TryGetComponent(out PixelCube pixelCube))
                    {
                        _cubes.Add(pixelCube);
                    }
                }
            }
        }
    }
}
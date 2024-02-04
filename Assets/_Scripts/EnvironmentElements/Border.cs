using System.Collections.Generic;
using GameMecanics;
using UnityEngine;

namespace _Scripts.EnvironmentElements
{
    public class Border : MonoBehaviour
    {
        [SerializeField] private List<Cannon.Cannon> _cannons = new List<Cannon.Cannon>();

        private void OnTriggerExit(Collider other)
        {
            if(other.TryGetComponent(out PlayerBall bullet))
            {
                
            }
        }
    }
}
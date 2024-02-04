using System;
using _Scripts.Finish;
using GameMecanics;
using UnityEngine;

namespace _Scripts.EnvironmentElements
{
    public class InvisibleBorders : MonoBehaviour
    {
        [SerializeField] private GameMechanicsSwitcher gameMechanicsSwitcher;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerBall playerBall))
            {
                gameMechanicsSwitcher.CheckLoseAction();
            }
        }
    }
}
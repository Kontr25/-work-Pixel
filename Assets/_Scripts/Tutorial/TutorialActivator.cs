using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Tutorial
{
    public class TutorialActivator : MonoBehaviour
    {
        [SerializeField] private List<TutorialHandsMover> _tutorialHandsMovers;


        public void Activate()
        {
            for (int i = 0; i < _tutorialHandsMovers.Count; i++)
            {
                if (_tutorialHandsMovers[i].gameObject.activeInHierarchy)
                {
                    _tutorialHandsMovers[i].ActivateTutorial();
                }
            }
        }
        
        public void Disable()
        {
            for (int i = 0; i < _tutorialHandsMovers.Count; i++)
            {
                if (_tutorialHandsMovers[i].gameObject.activeInHierarchy)
                {
                    _tutorialHandsMovers[i].StopTutorial();
                }
            }
        }
    }
}
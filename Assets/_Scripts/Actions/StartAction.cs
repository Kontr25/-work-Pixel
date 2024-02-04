using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAction : MonoBehaviour
{
    [SerializeField] private StartWindow _startWindow;
    [SerializeField] private List<GameObject> _startableObjects;

    private void OnEnable()
    {
        _startWindow.Clicked += Activate;
    }

    private void OnDisable()
    {
        _startWindow.Clicked -= Activate;
    }

    private void Activate()
    {
        if (_startableObjects.Count > 0)
        {
            foreach (var obj in _startableObjects)
            {
                if (obj.TryGetComponent(out IStartable startable))
                    startable.StartGameAction();
            }
        }
    }
}

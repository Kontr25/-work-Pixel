using System;
using BayatGames.SaveGameFree;
using System.Collections.Generic;
using UnityEngine;

public class FinishAction : MonoBehaviour
{
    public static Action<FinishType> Finish;
    public static bool IsGameOver = false;
    [SerializeField] private List<GameObject> _finishableObjects;
    private bool _victory = false;

    private float _sessionTime = 0f;

    public bool Victory
    {
        get => _victory;
        set => _victory = value;
    }

    private void Start()
    {
        Finish += Activate;
    }
    
    private void OnDestroy()
    {
        Finish -= Activate;
    }
    private void Update()
    {
        _sessionTime += Time.deltaTime;
    }

    public void Activate(FinishType finishType = FinishType.None)
    {
        IsGameOver = true;
        if (_finishableObjects.Count > 0)
        {
            switch (finishType)
            {
                case FinishType.Win:

                    if(!_victory)
                    {
                        _victory = true;
                        foreach (var obj in _finishableObjects)
                    {
                        if (obj.TryGetComponent(out IFinishable finishable))
                            finishable.StartActionOnWin();
                    }              
                    }
                break;

                case FinishType.Lose:

                    foreach (var obj in _finishableObjects)
                    {
                        if (obj.TryGetComponent(out IFinishable finishable))
                            finishable.StartActionOnLose();
                    }
                    break;

                default:
                    break;
            }
        }
    }

    public enum FinishType
    {
        None,
        Win,
        Lose
    }
}
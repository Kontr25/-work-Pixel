using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IStartable
{
    public void StartGameAction()
    {
        Debug.Log("Player activated");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class StartWindow : MonoBehaviour
{
    private Animator _animator;

    public event UnityAction Clicked;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void CloseOnClick()
    {
        _animator.Play("Close");
        Clicked?.Invoke();
    }
}

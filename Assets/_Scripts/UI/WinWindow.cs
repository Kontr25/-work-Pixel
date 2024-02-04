using DG.Tweening;
using UnityEngine;

public class WinWindow : MonoBehaviour, IFinishable
{
    [SerializeField] private Transform[] _endPoint;
    [SerializeField] private GameObject _backGround;
    [SerializeField] private GameObject[] _objectsForDisable;
    [SerializeField] private AudioSource _sound;
    [SerializeField] private ParticleSystem[] _confetti;
    public void StartActionOnWin()
    {
        _sound.Play();
        for (int i = 0; i < _confetti.Length; i++)
        {
            _confetti[i].Play();
        }
        for (int i = 0; i < _objectsForDisable.Length; i++)
        {
            _objectsForDisable[i].SetActive(false);
        }
        _backGround.SetActive(true);
        transform.DOMove(_endPoint[0].position, .3f).onComplete = () =>
        {
            transform.DOMove(_endPoint[1].position, .2f);
        };
    }

    public void StartActionOnLose()
    {
    }
}

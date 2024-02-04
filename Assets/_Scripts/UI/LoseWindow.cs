using DG.Tweening;
using UnityEngine;

public class LoseWindow : MonoBehaviour, IFinishable
{
    [SerializeField] private Transform[] _endPoint;
    [SerializeField] private GameObject _backGround;
    [SerializeField] private GameObject[] _objectsForDisable;
    [SerializeField] private AudioSource _sound;
    public void StartActionOnLose()
    {
        PlayerPrefs.SetInt("Tutorial", 1);
        _sound.Play();
        for (int i = 0; i < _objectsForDisable.Length; i++)
        {
            _objectsForDisable[i].SetActive(false);
        }
        
        _backGround.SetActive(true);
        transform.DOMove(_endPoint[0].position, .3f).onComplete = () =>
        {
            transform.DOMove(_endPoint[1].position, .3f);
        };
    }
        
    public void StartActionOnWin()
    {
    }
}

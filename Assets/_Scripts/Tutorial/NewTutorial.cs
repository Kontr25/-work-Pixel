using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Tutorial
{
    public class NewTutorial : MonoBehaviour
    {
        [SerializeField] private Image _hand;
        [SerializeField] private Image _ball;
        [SerializeField] private Image[] _elements;
        [SerializeField] private Image _wholeObject;
        [SerializeField] private Image _brokenObject;
        [SerializeField] private Transform _handTargetPoint;
        [SerializeField] private Transform _ballTargetPoint;
        [SerializeField] private UnityEngine.Camera _mainCamera;
        [SerializeField] private Transform[] _obstaclesImage;
        [SerializeField] private Transform[] _obstacles;

        private Vector3 _ballDefaultPosition;

        private void Start()
        {
            for (int i = 0; i < _obstaclesImage.Length; i++)
            {
                _obstaclesImage[i].transform.position = _mainCamera.WorldToScreenPoint(_obstacles[i].position);
            }
            
            _ballDefaultPosition = _ball.transform.position;
            StartCoroutine(Tutor());
        }

        private IEnumerator Tutor()
        {
            while (true)
            {
                _wholeObject.DOFade(1, .3f);
                _brokenObject.DOFade(0, .3f);
                _ball.transform.position = _ballDefaultPosition;
                _hand.transform.position = _ball.transform.position;
                _hand.DOFade(1, .5f);
                yield return new WaitForSeconds(.5f);
            
                _hand.transform.DOScale(new Vector3(-.8f, .8f, .8f), .3f);
                for (int i = 0; i < _elements.Length; i++)
                {
                    _elements[i].DOFade(1, .3f);
                }

                _ball.DOFade(1, .3f);
                yield return new WaitForSeconds(.3f);
                
                _hand.transform.DOMove(_handTargetPoint.position, 1f);
                yield return new WaitForSeconds(1.2f);
            
                for (int i = 0; i < _elements.Length; i++)
                {
                    _elements[i].DOFade(0, .2f);
                }
                _hand.transform.DOScale(new Vector3(-1f, 1f, 1f), .2f).onComplete = () =>
                {
                    _hand.DOFade(0, .5f);
                };
                _ball.transform.DOMove(_ballTargetPoint.position, 1f);
                yield return new WaitForSeconds(.5f);
            
                _ball.DOFade(0, .3f);
                _wholeObject.DOFade(0, .3f);
                _brokenObject.DOFade(1, .3f);
                yield return new WaitForSeconds(2f);
            }
        }

        public void Disable()
        {
            StopAllCoroutines();
            transform.DOScale(.01f, .2f).onComplete = () =>
            {
                gameObject.SetActive(false);
            };
        }
    }
}
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Tutorial
{
    public class TutorialHandsMover : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _directionPoint;
        [SerializeField] private Transform _hand;
        [SerializeField] private SpriteRenderer _handDown;
        [SerializeField] private SpriteRenderer _handUp;
        [SerializeField] private float _moveTime;
        [SerializeField] private float _handChangeDelay;
        [SerializeField] private LineRenderer _lineRenderer;

        private WaitForSeconds _delayRepit;
        private WaitForSeconds _delayHandChange;
        private Coroutine _moveRoutine;
        private bool _lineIsRender = false;
        private bool _isTutorial = false;

        private void Start()
        {
            _delayRepit = new WaitForSeconds(_moveTime + _handChangeDelay);
            _delayHandChange = new WaitForSeconds(_handChangeDelay);
            _lineRenderer.SetPosition(0, _player.position);
        }

        private void Update()
        {
            if (_lineIsRender)
            {
                _lineRenderer.SetPosition(1, _hand.position);
            }
        }

        public void ActivateTutorial()
        {
            _isTutorial = true;
            _player.gameObject.SetActive(true);
            _moveRoutine = StartCoroutine(MoveRoutine());
        }

        public void StopTutorial()
        {
            if (_moveRoutine != null)
            {
                StopCoroutine(_moveRoutine);
                _handUp.gameObject.SetActive(false);
                _handDown.gameObject.SetActive(false);
                _lineRenderer.enabled = false;
                _isTutorial = false;
                _player.gameObject.SetActive(false);
            }
        }

        private IEnumerator MoveRoutine()
        {
            while (_isTutorial)
            {
                _hand.position = _player.position;
                _handUp.gameObject.SetActive(true);
                _handUp.DOFade(1, .5f);
                yield return _delayHandChange;
                
                _handUp.gameObject.SetActive(false);
                _handDown.gameObject.SetActive(true);
                yield return _delayHandChange;
                
                _lineRenderer.enabled = true;
                _lineIsRender = true;
                _hand.DOMove(_directionPoint.position, _moveTime).onComplete = () =>
                {
                    if (_isTutorial)
                    {
                        _handUp.gameObject.SetActive(true);
                        _handDown.gameObject.SetActive(false);
                        _lineRenderer.enabled = false;
                        _lineRenderer.SetPosition(1, _player.position);
                        _lineIsRender = false;
                    }
                };
                yield return _delayRepit;
                
                _handUp.DOFade(0, .5f).onComplete = () =>
                {
                    _handUp.gameObject.SetActive(false);
                };
                yield return _delayHandChange;
            }
        }
    }
}
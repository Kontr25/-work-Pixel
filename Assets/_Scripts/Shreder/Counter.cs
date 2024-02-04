using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Shreder
{
    public class Counter : MonoBehaviour
    {
        [SerializeField] private float _flyHeight;
        [SerializeField] private float _flyTime;
        [SerializeField] private float _disableDelayValue;

        private Transform _parentTransform;
        private WaitForSeconds _disableDelay;
        private Coroutine _flyRoutine;
        private Vector3 _defaultScale = Vector3.one;
        private Vector3 _smallScale = new Vector3(0.01f, 0.01f, 0.01f);

        private void Start()
        {
            _disableDelay = new WaitForSeconds(_disableDelayValue);
        }

        public Transform ParentTransform
        {
            get => _parentTransform;
            set => _parentTransform = value;
        }

        private void OnEnable()
        {
            transform.SetParent(null);
            transform.localScale = _smallScale;
            _flyRoutine = StartCoroutine(Fly());
        }

        private IEnumerator Fly()
        {
            yield return new WaitForSeconds(.5f);
            transform.DOScale(_defaultScale, 1f);
            transform.DOLocalMoveY(transform.localPosition.y + _flyHeight, _flyTime);
            yield return _disableDelay;
            transform.DOScale(_smallScale, 1f).onComplete = () =>
            {
                transform.SetParent(_parentTransform);
                transform.localScale = _smallScale;
                if (_flyRoutine != null)
                {
                    StopCoroutine(_flyRoutine);
                }
                gameObject.SetActive(false);
            };

        }
    }
}
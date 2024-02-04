using System.Collections;
using _Scripts.Reflected;
using _Scripts.Sound;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.PixelObject
{
    public class FallenCube : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        private bool _destroyIsStarted = false;

        private Color _defaultColor;

        public bool DestroyIsStarted
        {
            get => _destroyIsStarted;
            set => _destroyIsStarted = value;
        }

        private void Start()
        {
            _defaultColor = _meshRenderer.material.color;
            ConvertColor();
        }

        private void ConvertColor()
        {
            Color dark = new Color(_defaultColor.r / 2, _defaultColor.g / 2, _defaultColor.b / 2);
            _meshRenderer.material.DOColor(dark, 1f);
        }

        public void DestroyWithDelay(float delay)
        {
            StartCoroutine(DestroyCube(delay));
        }

        private IEnumerator DestroyCube(float delay)
        {
            yield return new WaitForSeconds(delay);
            transform.DOScale(0.1f, .5f).onComplete = () =>
            {
                Destroy(gameObject);
            };
        }
    }
}
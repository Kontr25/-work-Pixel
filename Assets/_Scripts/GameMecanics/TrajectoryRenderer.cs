using System;
using _Scripts.Reflected;
using UnityEngine;

namespace GameMecanics
{
    public class TrajectoryRenderer : MonoBehaviour
    {
        [SerializeField] private int _reflections;
        [SerializeField] private float _maxLength;
        [SerializeField] private LayerMask _reflectedLayer;
        [SerializeField] private LayerMask _multiplicator;
        [SerializeField] private LineRenderer _invisibleLineRenderer;
        [SerializeField] private LineRenderer _visibleLineRendererFirst;
        [SerializeField] private LineRenderer _visibleLineRendererSecond;
        [SerializeField] private GameObject _cancelingSprite;
        [SerializeField] private float _distanceDivider;
        private Ray _ray;
        private RaycastHit _hit;
        private Vector3 _direction;
        private bool _isRender = false;

        public LineRenderer CannonLineRenderer
        {
            get => _invisibleLineRenderer;
            set => _invisibleLineRenderer = value;
        }

        private void Update()
        {
            if (_isRender)
            {
                TrajectoryDraw();
            }
        }

        public void StartRenderTrajectory()
        {
            _invisibleLineRenderer.enabled = true;
            _visibleLineRendererFirst.enabled = true;
            _visibleLineRendererSecond.enabled = true;
            _isRender = true;
        }
        
        public void StopRenderTrajectory()
        {
            _isRender = false;
            _invisibleLineRenderer.enabled = false;
            _visibleLineRendererFirst.enabled = false;
            _visibleLineRendererSecond.enabled = false;
        }

        public void VisibleTrajectory()
        {
            if (_isRender)
            {
                _visibleLineRendererFirst.SetPosition(0, transform.position);
                _visibleLineRendererFirst.SetPosition(1, _invisibleLineRenderer.GetPosition(1));
                if (_invisibleLineRenderer.positionCount > 2)
                {
                    Vector3 direction = Vector3.Lerp(_visibleLineRendererFirst.GetPosition(1),_invisibleLineRenderer.GetPosition(2),_distanceDivider
                        / Vector3.Distance(_visibleLineRendererFirst.GetPosition(1),_invisibleLineRenderer.GetPosition(2)) );
                    _visibleLineRendererSecond.SetPosition(0, _visibleLineRendererFirst.GetPosition(1) );
                    _visibleLineRendererSecond.SetPosition(1, direction);
                }
            }
        }

        public void TrajectoryDraw()
        {
            _ray = new Ray(transform.position, transform.forward);

            _invisibleLineRenderer.positionCount = 1;
            _invisibleLineRenderer.SetPosition(0, transform.position);
            float remainingLength = _maxLength;
            
            if (Physics.SphereCast(_ray.origin, 1f, _ray.direction, out _hit, remainingLength, _multiplicator))
            {
                _invisibleLineRenderer.positionCount += 1;
                _invisibleLineRenderer.SetPosition(_invisibleLineRenderer.positionCount - 1, _hit.point);
                if (_visibleLineRendererSecond.enabled)
                {
                    _visibleLineRendererSecond.enabled = false;
                }
                return;
            }

            for (int i = 0; i < _reflections; i++)
            {
                if (Physics.SphereCast(_ray.origin, 1f, _ray.direction, out _hit, remainingLength, _reflectedLayer))
                {
                    if (!_visibleLineRendererSecond.enabled)
                    {
                        _visibleLineRendererSecond.enabled = true;
                    }
                    _invisibleLineRenderer.positionCount += 1;
                    _invisibleLineRenderer.SetPosition(_invisibleLineRenderer.positionCount - 1, _hit.point);
                    remainingLength -= Vector3.Distance(_ray.origin, _hit.point);
                    Vector3 reflectedDirection = Vector3.Reflect(_ray.direction.normalized, _hit.normal.normalized);
                    _ray = new Ray(_hit.point, reflectedDirection);

                    if (_hit.collider.TryGetComponent(out ReflectableObject reflectableObject) ||_hit.collider.TryGetComponent(out Wall wall))
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    _invisibleLineRenderer.positionCount += 1;
                    _invisibleLineRenderer.SetPosition(_invisibleLineRenderer.positionCount - 1, _ray.origin + _ray.direction * remainingLength);
                }
            }
        }

        public void CancelingShot(float distance)
        {
            _cancelingSprite.transform.localScale = new Vector3(1f - distance/2, 1f - distance/2, 1f - distance/2);
        }

        public void SetLineRendererColor(Color color)
        {
            _visibleLineRendererFirst.startColor = color;
            _visibleLineRendererFirst.endColor = color;
            _visibleLineRendererSecond.startColor = color;
            _visibleLineRendererSecond.endColor = new Color(color.r, color.g, color.b, 0/255f);
        }
    }
}
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Camera;
using CodeBase.Services.Update;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.CameraLogic
{
    public class CameraFollow : MonoBehaviour, ILateUpdatable
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothSpeed = 0.125f;
        [SerializeField] private bool enableBounds;
        [SerializeField] private float _minBoundX;
        [SerializeField] private float _maxBoundX;

        private Transform _body;
        private Vector3 _desiredPosition;
        private IUpdateService _updateService;
        private ICameraRaycast _cameraRayCast;

        public void Construct(Transform body, ICameraRaycast cameraRayCast,IUpdateService updateService)
        {
            _body = body;
            _cameraRayCast = cameraRayCast;
            
            _updateService = updateService;
            _updateService.Register(this);
        }

        private void OnDisable()
        {
            _updateService.Unregister(this);
        }

        public void LateUpdateTick()
        {
            if (_body)
            {
                Vector3 lastHitPoint = _cameraRayCast.GetLastHitPoint();
              
                if (lastHitPoint.x > _body.transform.position.x)
                {
                    _desiredPosition = _body.transform.position + _offset;
                }
                else
                {
                    _desiredPosition = _body.transform.position - _offset;
                }

                _desiredPosition.y = transform.position.y;
                _desiredPosition.z = transform.position.z;
                
                if (enableBounds)
                {
                    float newPos;
                    if (transform.position.x >= _maxBoundX)
                    {
                        newPos = transform.position.x + (_desiredPosition.x * _smoothSpeed);
                        if (newPos <= _minBoundX)
                        {
                            return;
                        }
                    }

                    if (transform.position.x <= _minBoundX)
                    {
                        newPos = transform.position.x - (_desiredPosition.x * _smoothSpeed);
                        if (newPos <= _minBoundX)
                        {
                            return;
                        }
                    }
                }
                
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, _desiredPosition, _smoothSpeed);
                transform.position = smoothedPosition;
            }
        }
    }
}
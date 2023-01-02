using CodeBase.Services.Camera;
using CodeBase.Services.Update;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.CameraLogic
{
    public class CameraFollow : MonoBehaviour, ILateUpdatable
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothSpeed = 0.125f;
        [SerializeField] private bool _enableBorders;
        [SerializeField] private float _xLeftBorder;
        [SerializeField] private float _xRightBorder;

        private Transform _body;
        private Vector3 _desiredPosition;
        private Vector3 _previousPosition;

        private float _endPosition;

        private IUpdateService _updateService;
        private ICameraRaycast _cameraRayCast;

        public UnityEvent OnCameraMoving;
        public UnityEvent OnCameraNotMoving;

        public void Construct(Transform body, ICameraRaycast cameraRayCast, IUpdateService updateService)
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

                Vector3 smoothedPosition = Vector3.Lerp(transform.position, _desiredPosition, _smoothSpeed);
                transform.position = smoothedPosition;

                if (transform.position.x == _previousPosition.x)
                {
                    OnCameraNotMoving?.Invoke();
                }
                else
                {
                    OnCameraMoving?.Invoke();
                }

                if (_enableBorders)
                    if (transform.position.x < _xLeftBorder || transform.position.x > _xRightBorder)
                    {
                        transform.position = new Vector3(_previousPosition.x,transform.position.y,transform.position.z);
                    }

                _previousPosition = transform.position;
            }
        }
    }
}
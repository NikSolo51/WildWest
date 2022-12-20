using CodeBase.Infrastructure.Services;
using CodeBase.Services.Input;
using CodeBase.Services.Update;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.CameraLogic
{
    [RequireComponent(typeof(Camera))]
    public class PinchAndZoom : MonoBehaviour, IUpdatable
    {
        [SerializeField] private float _zoomMinBound = 0.1f;
        [SerializeField] private float _zoomMaxBound = 179.9f;
        [SerializeField] private Camera _camera;
        private float _mouseZoomSpeed = 15.0f;
        private float _touchZoomSpeed = 0.1f;
        
        private IInputService _inputService;
        private IUpdateService _updateService;

        public void Construct(IInputService inputService, IUpdateService updateService)
        {
            _inputService = inputService;
            _updateService = updateService;
        }

        private void OnEnable()
        {
            _updateService = AllServices.Container.Single<IUpdateService>();
            _updateService.Register(this);
        }

        private void OnDisable()
        {
            _updateService.Unregister(this);
        }

        public void UpdateTick()
        {
            if (!_camera)
                return;

            if (Input.touchSupported)
            {
                
                if (Input.touchCount == 2)
                {
            
                    Touch tZero = Input.GetTouch(0);
                    Touch tOne = Input.GetTouch(1);
                   
                    Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                    Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                    float oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                    float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                   
                    float deltaDistance = oldTouchDistance - currentTouchDistance;
                    Zoom(deltaDistance, _touchZoomSpeed);
                }
            }
            else
            {
               // float scroll = Input.GetAxis("Mouse ScrollWheel");
                float scroll = _inputService.ScrollAxis;
                Zoom(scroll, _mouseZoomSpeed);
            }

            if (_camera.fieldOfView < _zoomMinBound)
            {
                _camera.fieldOfView = 0.1f;
            }
            else if (_camera.fieldOfView > _zoomMaxBound)
            {
                _camera.fieldOfView = 179.9f;
            }
        }

        void Zoom(float deltaMagnitudeDiff, float speed)
        {
            _camera.fieldOfView += deltaMagnitudeDiff * speed;
            _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, _zoomMinBound, _zoomMaxBound);
        }
    }
}
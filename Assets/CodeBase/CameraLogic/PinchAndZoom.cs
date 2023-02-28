using CodeBase.Infrastructure.Services;
using CodeBase.Services.Camera;
using CodeBase.Services.Update;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    [RequireComponent(typeof(Camera))]
    public class PinchAndZoom : MonoBehaviour, IUpdatable
    {
        [SerializeField] private float _zoomMinBound = 0.1f;
        [SerializeField] private float _zoomMaxBound = 179.9f;
        [SerializeField] private Camera _camera;
        [SerializeField] private bool _inverseScroll = true;
        [SerializeField] private float _mouseZoomSpeed = 15.0f;
        [SerializeField] private float _touchZoomSpeed = 0.1f;
        private float _speed;

        private IUpdateService _updateService;
        private IZoomService _zoomService;

        public void Construct(IUpdateService updateService, IZoomService zoomService)
        {
            _updateService = updateService;
            _zoomService = zoomService;
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
            
            if (!_camera || _zoomService == null)
                return;

            _speed = Input.touchSupported ? _touchZoomSpeed : _mouseZoomSpeed;
            _zoomService.Zoom(_speed,_inverseScroll,_zoomMinBound,_zoomMaxBound);
          
            if (_camera.fieldOfView < _zoomMinBound)
            {
                _camera.fieldOfView = 0.1f;
            }
            else if (_camera.fieldOfView > _zoomMaxBound)
            {
                _camera.fieldOfView = 179.9f;
            }
        }
    }
}


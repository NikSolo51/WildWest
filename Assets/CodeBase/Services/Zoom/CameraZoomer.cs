using CodeBase.Services.Camera;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.Zoom
{
    public abstract class CameraZoomer : IZoomService
    {
        protected UnityEngine.Camera _camera;
        protected IInputService _inputService;
        
        [Inject]
        public void Construct( IInputService inputService)
        {
            _inputService = inputService;
        }

        public void SetupCamera(UnityEngine.Camera camera)
        {
            _camera = camera;
        }

        public abstract void Zoom(float zoomSpeed, bool inverseScroll, float zoomMinBound, float zoomMaxBound);

        protected void CalculateZoom(float deltaMagnitudeDiff, float speed, float zoomMinBound, float zoomMaxBound)
        {
            if(!_camera)
                return;
            _camera.fieldOfView += deltaMagnitudeDiff * speed;
            _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, zoomMinBound, zoomMaxBound);
        }
    }
}
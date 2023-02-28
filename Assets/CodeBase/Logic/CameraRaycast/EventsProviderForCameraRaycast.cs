using CodeBase.Services.Camera;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.CameraRaycast
{
    public class EventsProviderForCameraRaycast : MonoBehaviour
    {
        private ICameraRaycast _cameraRaycast;
        
        [Inject]
        public void Construct(ICameraRaycast cameraRaycast)
        {
            _cameraRaycast = cameraRaycast;
        }

        public void EnableCameraRaycast()
        {
            _cameraRaycast.EnableRayCast();
        }

        public void DisableCameraRaycast()
        {
            _cameraRaycast.DisableRayCast();
        }
    }
}
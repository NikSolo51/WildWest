using CodeBase.Services.Camera;
using UnityEngine;

namespace CodeBase.Logic.CameraRaycast
{
    public class EventsProviderForCameraRaycast : MonoBehaviour
    {
        private ICameraRaycast _cameraRaycast;

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
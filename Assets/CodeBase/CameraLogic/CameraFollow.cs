using CodeBase.Infrastructure.Services;
using CodeBase.Services.Camera;
using CodeBase.Services.Update;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraFollow : MonoBehaviour, ILateUpdatable
    {
        public Vector3 offset;
        public float smoothSpeed = 0.125f;

        private Transform _body;
        private Vector3 desiredPosition;
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
                    desiredPosition = _body.transform.position + offset;
                }
                else
                {
                    desiredPosition = _body.transform.position - offset;
                }

                desiredPosition.y = transform.position.y;
                desiredPosition.z = transform.position.z;

                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                transform.position = smoothedPosition;
            }
        }
    }
}
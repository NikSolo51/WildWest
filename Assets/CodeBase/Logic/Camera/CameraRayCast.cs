using CodeBase.Services.Camera;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase.Logic.Camera
{
    public class CameraRayCast : MonoBehaviour, ICameraRaycast
    {
        public LayerMask _layermask;
        private UnityEngine.Camera _camera;
        private Vector3 lastHitPointPos;
        private bool enableRaycast = true;


        public Vector3 GetLastHitPoint()
        {
            return lastHitPointPos;
        }

        public Collider GetCollider()
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layermask))
            {
                return hit.collider;
            }

            return null;
        }
        
        public Vector3 GetPoint()
        {
            if (!enableRaycast)
                return lastHitPointPos;
            
            if (EventSystem.current.IsPointerOverGameObject()) 
                return lastHitPointPos;
            if (EventSystem.current.currentSelectedGameObject != null)
                return lastHitPointPos;
            
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layermask))
            {
                lastHitPointPos = hit.point;
            }

            return lastHitPointPos;
        }

        public void Construct(UnityEngine.Camera camera)
        {
            _camera = camera;
        }

        public void EnableRayCast()
        {
            enableRaycast = true;
        }

        public void DisableRayCast()
        {
            enableRaycast = false;
        }
    }
}
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Update;
using UnityEngine;

namespace CodeBase.Logic.Parallax
{
    public class Parallax : MonoBehaviour,IUpdatable
    {
        [SerializeField] private Transform followingTarget;
        [SerializeField, Range(0, 1f)] private float parallaxStrength = 0.1f;
        [SerializeField] private bool disableVerticalParallax;
        private Vector3 targetPreviousPosition;
        private IUpdateService _updateService;

        public void Initialize(GameObject camera)
        {
            if (!followingTarget)
                followingTarget = camera.transform;

            targetPreviousPosition = followingTarget.position;
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
            if (followingTarget)
            {
                Vector3 delta = followingTarget.position - targetPreviousPosition;
                if (disableVerticalParallax)
                {
                    delta.y = 0;
                }

                targetPreviousPosition = followingTarget.position;

                transform.position += delta * parallaxStrength;
            }
        }
    }
}
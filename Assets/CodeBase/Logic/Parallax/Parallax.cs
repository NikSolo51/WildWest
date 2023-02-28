using CodeBase.Services.Update;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.Parallax
{
    public class Parallax : MonoBehaviour,IUpdatable
    {
        [SerializeField] private Transform _followingTarget;
        [SerializeField, Range(0, 1f)] private float _parallaxStrength = 0.1f;
        [SerializeField] private bool _disableVerticalParallax;
        private Vector3 _targetPreviousPosition;
        private IUpdateService _updateService;

        [Inject]
        public void Construct(IUpdateService updateService)
        {
            _updateService = updateService;
        }
        // public void Construct(GameObject camera)
        // {
        //     if (!_followingTarget)
        //         _followingTarget = camera.transform;
        //
        //     _targetPreviousPosition = _followingTarget.position;
        // }
        
        private void OnEnable()
        {
            _updateService.Register(this);
        }

        private void OnDisable()
        {
            _updateService.Unregister(this);
        }

        public void UpdateTick()
        {
            if (_followingTarget)
            {
                Vector3 delta = _followingTarget.position - _targetPreviousPosition;
                if (_disableVerticalParallax)
                {
                    delta.y = 0;
                }

                _targetPreviousPosition = _followingTarget.position;

                transform.position += delta * _parallaxStrength;
            }
        }
    }
}
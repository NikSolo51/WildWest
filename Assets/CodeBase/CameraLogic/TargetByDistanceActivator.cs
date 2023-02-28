using CodeBase.Logic.Target;
using CodeBase.Services.Camera;
using CodeBase.Services.Input;
using CodeBase.Services.Update;
using UnityEngine;
using Zenject;

namespace CodeBase.CameraLogic
{
    public class TargetByDistanceActivator : MonoBehaviour,IUpdatable
    {
        private Transform _hero;
        private Target _currentTarget;
        private Target _prevTarget;
        
        private ICameraRaycast _cameraRaycast;
        private IInputService _inputService;
        private IUpdateService _updateService;
        
        [Inject]
        public void Construct(ICameraRaycast cameraRaycast, IInputService inputService,IUpdateService updateService)
        {
            _cameraRaycast = cameraRaycast;
            _inputService = inputService;
            _updateService = updateService;
            _updateService.Register(this);
        }

        private void OnDisable()
        {
            _updateService.Unregister(this);
        }

        public void SetPlayer(Transform hero)
        {
            _hero = hero;
        }
        
        public void UpdateTick()
        {
            if (_inputService != null)
                if (_inputService.IsClickButtonUp())
                {
                    _currentTarget = _cameraRaycast.GetCollider()?.GetComponent<Target>();
                    
                   
                    if (_prevTarget)
                    {
                        if (_currentTarget != _prevTarget)
                        {
                            _prevTarget.TargetUnReached();
                        }
                    }
                }

            if (_currentTarget)
            {
                if (!_currentTarget.IsTargetReached() &&
                    IsThereMinimalDistanceBetweenTargetPosAndPointPos(_currentTarget._activationDistance))
                {
                    _currentTarget.TargetReached();
                    _prevTarget = _currentTarget;
                    _currentTarget = null;
                }
            }
        }
        
        private bool IsThereMinimalDistanceBetweenTargetPosAndPointPos(float minimalDistance)
        {
            if (Vector3.Distance(_hero.transform.position, _currentTarget.transform.position) < minimalDistance)
            {
                return true;
            }

            return false;
        }

      
    }
}
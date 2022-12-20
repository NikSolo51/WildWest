using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Camera;
using CodeBase.Services.Input;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.Update;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
    public class HeroMove : MonoBehaviour, ISavedProgress,IUpdatable
    {
        public NavMeshAgent HeroAgent;
        public float _minimalRunDistnace;
        public float _walkSpeed = 3;
        public float _runSpeed = 4;
        private ICameraRaycast _cameraRayCast;
        private IInputService _movementMovementInputSerivce;
        private IUpdateService _updateService;

        public void Construct(ICameraRaycast cameraRayCast, IInputService inputService)
        {
            _cameraRayCast = cameraRayCast;
            _movementMovementInputSerivce = inputService;
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
            if (_movementMovementInputSerivce != null)
                if (_movementMovementInputSerivce.IsClickButtonUp())
                {
                    Vector3 movementPoint = _cameraRayCast.GetPoint();

                    if (movementPoint.sqrMagnitude > Constants.Epsilon)
                    {
                        float distnace = (movementPoint - transform.position).sqrMagnitude;

                        if (distnace <= _minimalRunDistnace)
                        {
                            HeroAgent.speed = _walkSpeed;
                        }
                        else
                        {
                            HeroAgent.speed = _runSpeed;
                        }
                        
                        HeroAgent.destination = movementPoint;
                        Vector2 rotationDir = new Vector2(movementPoint.x, transform.position.y);
                        HeroAgent.transform.LookAt(rotationDir);
                    }
                }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            //use box collider if you use character controller or your y position will be incorrect
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(),
                transform.position.AsVectorData());
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
            {
                Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
                if (savedPosition != null)
                    Warp(to: savedPosition);
            }
        }

        private void Warp(Vector3Data to)
        {
            HeroAgent.enabled = false;
            transform.position = to.AsUnityVector();
            HeroAgent.enabled = true;
        }

        private static string CurrentLevel() => SceneManager.GetActiveScene().name;
    }
}
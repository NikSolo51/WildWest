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
    public class HeroMove : MonoBehaviour, ISavedProgress, IUpdatable
    {
        [SerializeField] private NavMeshAgent HeroAgent;
        [SerializeField] private float _minimalRunDistnace;
        [SerializeField] private float _walkSpeed = 3;
        [SerializeField] private float _runSpeed = 4;
        private ICameraRaycast _cameraRayCast;
        private IInputService _movementMovementInputSerivce;
        private IUpdateService _updateService;
        private ISaveLoadService _saveLoadService;

        public void Construct(ICameraRaycast cameraRayCast, IInputService inputService,
            ISaveLoadService saveLoadService, IUpdateService updateService)
        {
            _cameraRayCast = cameraRayCast;
            _movementMovementInputSerivce = inputService;
            _saveLoadService = saveLoadService;
            _updateService = updateService;
            _updateService.Register(this);
        }

        private void OnDisable()
        {
            _updateService.Unregister(this);
            _saveLoadService.SaveProgress();
        }

        public void UpdateTick()
        {
            if (_movementMovementInputSerivce != null)
                if (_movementMovementInputSerivce.IsClickButtonUp() ||
                    _movementMovementInputSerivce.IsClickButtonPress())
                    MovingByClick();
        }

        private void MovingByClick()
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
            }

            HeroAgent.destination = movementPoint;
            Vector3 rotationDir = new Vector3(movementPoint.x, transform.position.y, movementPoint.z);
            HeroAgent.transform.LookAt(rotationDir);
        }


        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(),
                transform.position.AsVectorData());
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
            {
                Vector3 savedPosition = progress.WorldData.PositionOnLevel.Position.AsUnityVector();
                
                if (savedPosition != Vector3.zero)
                    Warp(to: savedPosition);
            }
        }

        private void Warp(Vector3 to)
        {
            HeroAgent.enabled = false;
            transform.position = to;
            HeroAgent.enabled = true;
        }

        private static string CurrentLevel() => SceneManager.GetActiveScene().name;
    }
}
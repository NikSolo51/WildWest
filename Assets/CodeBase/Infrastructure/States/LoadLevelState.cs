using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic;
using CodeBase.Logic.CameraRaycast;
using CodeBase.Logic.Parallax;
using CodeBase.Services.Audio;
using CodeBase.Services.Audio.SoundManager;
using CodeBase.Services.Camera;
using CodeBase.Services.Input;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.Services.Update;
using CodeBase.Services.Zoom;
using CodeBase.UI.UIInventory.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayLoadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private IStaticDataService _staticData;
        private IUIItemInventory _uiItemInventory;
        public ISaveLoadService _saveLoadService;
        public LoadLevelState(GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            LoadingCurtain curtain,
            IGameFactory gameFactory,
            IStaticDataService staticData,
            IUIItemInventory uiItemInventory,
            ISaveLoadService saveLoadService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _staticData = staticData;
            _uiItemInventory = uiItemInventory;
            _saveLoadService = saveLoadService;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _gameFactory.CleanUp();
            _gameFactory.WarmUp();
            _uiItemInventory.Clear();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _curtain.Hide();
        }

        private async void OnLoaded()
        {
            await InitGameWorld();
            _saveLoadService.InformProgressReaders();
            _stateMachine.Enter<GameLoopState>();
        }


        private async Task InitGameWorld()
        {
            LevelStaticData levelData = LevelStaticData();

            ISoundService soundManager = await InitializeAudio(levelData.SoundManagerData);
            IInputService inputService = AllServices.Container.Single<IInputService>();
            IUpdateService updateService = AllServices.Container.Single<IUpdateService>();
            ISaveLoadService saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            
            InitUpdateManger(updateService);

            GameObject hero = await CreateHero(levelData,updateService);

            GameObject camera = await InitCamera(levelData);
            ICameraRaycast cameraRaycast =  await InitCameraRaycast(camera,hero.transform);
            CameraShake cameraShake = camera.GetComponent<CameraShake>();
            cameraShake.Construct(updateService);
            
            await InitHero(hero,cameraRaycast,inputService,saveLoadService);
            GameObject hud = await InitHud(hero);
            
            RegisterInventory();
            
            InitPointAndClickSystem(camera, hero,cameraRaycast,inputService);
            await InitPuzzles(levelData, hud,soundManager);
            
            FollowCamera(hero, camera,cameraRaycast,updateService,inputService);
            
            GameObject parallax = await _gameFactory.CreateParallax();
            InitParallax(parallax,camera);
        }

        private void RegisterInventory()
        {
            _saveLoadService.Register(_uiItemInventory);
        }

        private async void InitUpdateManger(IUpdateService updateService)
        {
             GameObject updateManager = await _gameFactory.CreateUpdateManager();
             UpdateManagerProvider updateProvider = updateManager.GetComponent<UpdateManagerProvider>();
             updateProvider.Construct(updateService);
        }

        private async Task<ISoundService> InitializeAudio(SoundManagerData soundManagerData)
        {
            ISoundService soundManager = await _gameFactory.CreateSoundManager(soundManagerData);
            AllServices.Container.RegisterSingle<ISoundService>(soundManager);
            return soundManager;
        }

        private void InitPointAndClickSystem(GameObject camera, GameObject hero,ICameraRaycast cameraRaycast,IInputService inputService)
        {
           TargetByDistanceActivator targetByDistanceActivator = camera.GetComponent<TargetByDistanceActivator>();
            targetByDistanceActivator.Construct(hero.transform, cameraRaycast,
                inputService);
        }

        private async Task<ICameraRaycast> InitCameraRaycast(GameObject camera,Transform heroTransform)
        {
            CameraRayCast cameraRayCast = camera.GetComponent<CameraRayCast>();
            cameraRayCast.Construct(camera.GetComponent<Camera>(),heroTransform);
            RegisterRaycastService(cameraRayCast);

            return cameraRayCast;
        }


        private async Task InitHero(GameObject hero,ICameraRaycast cameraRayCast,IInputService inputService, ISaveLoadService saveLoadService)
        {
            HeroMove heroMove = hero.GetComponent<HeroMove>();
            heroMove.Construct(cameraRayCast,inputService,saveLoadService);
        }

        private async Task<GameObject> CreateHero(LevelStaticData levelData,IUpdateService updateService)
        {
            GameObject hero = await _gameFactory.CreateHero(levelData.InitialHeroPosition,updateService);
            return hero;
        }

        private async Task<GameObject> InitCamera(LevelStaticData levelData)
        {
            GameObject camera = await _gameFactory.CreateCamera(levelData.InitialCameraPosition);
           
            return camera;
        }

        private async Task InitPuzzles(LevelStaticData levelData, GameObject hud,
            ISoundService levelSoundManager)
        {
            for (int i = 0; i < levelData.PuzzleSpawners.Count; i++)
            {
                PuzzleSpawnerData puzzleSpawner = levelData.PuzzleSpawners[i];
                await _gameFactory.CreatePuzzle(puzzleSpawner._position, puzzleSpawner.id, puzzleSpawner.puzzelName,
                    hud.transform,levelSoundManager);
            }
        }

        private void InitParallax(GameObject parallax,GameObject camera)
        {
            Parallax[] parallaxes = parallax.GetComponentsInChildren<Parallax>();
            for (int i = 0; i < parallaxes.Length; i++)
            {
                parallaxes[i].Construct(camera);
            }
        }

        private async Task<GameObject> InitHud(GameObject hero)
        {
            GameObject hud = await _gameFactory.CreateHud();
            return hud;
        }

        private void FollowCamera(GameObject hero, GameObject camera, ICameraRaycast cameraRayCast,IUpdateService updateService,IInputService inputService)
        {
            CameraFollow cameraFollow = camera.GetComponent<CameraFollow>();
            cameraFollow.Construct(hero.transform, cameraRayCast,updateService );
            
            IZoomService zoomService = PinchAndZoomSet(camera,inputService);
            AllServices.Container.RegisterSingle<IZoomService>(zoomService);
            
            PinchAndZoom pinchAndZoom = camera.GetComponent<PinchAndZoom>();
            pinchAndZoom.Construct(updateService,zoomService);
        }

        IZoomService PinchAndZoomSet(GameObject cameraObj,IInputService inputService)
        {
            Camera camera = cameraObj.GetComponent<Camera>();
            if (Input.touchSupported)
            {
                CameraZoomer cameraZoomer = new MobileZoom();
                cameraZoomer.Construct(camera,inputService);
                return cameraZoomer;
            }
            else
            {
                CameraZoomer cameraZoomer = new StandaloneZoom();
                cameraZoomer.Construct(camera,inputService);
                return cameraZoomer;
            }
        }

        private void RegisterRaycastService(ICameraRaycast impl)
        {
            AllServices.Container.RegisterSingle<ICameraRaycast>(impl);
        }

        private LevelStaticData LevelStaticData() => _staticData.ForLevel(SceneManager.GetActiveScene().name);
    }
}
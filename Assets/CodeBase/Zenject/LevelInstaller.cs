using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic.CameraRaycast;
using CodeBase.Logic.Sound;
using CodeBase.Services.Audio;
using CodeBase.Services.Audio.SoundManager;
using CodeBase.Services.Camera;
using CodeBase.Services.StaticData;
using CodeBase.Services.Zoom;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private GameObject _cameraGO;
    [SerializeField] private GameObject _hudGO;
    [SerializeField] private GameObject _playerGO;
    [SerializeField] private SoundManagerAbstract _soundManagerGO;


    public override void InstallBindings()
    {
        Camera camera = _cameraGO.GetComponent<Camera>();
        InitZoomInput(camera);
        InitTargetByDistanceActivator(_cameraGO, _playerGO);
        RegisterAudioService();
        InitHudSettings(_hudGO);
        InitCameraRaycast(camera);
        CameraFollow(_cameraGO, _playerGO);
    }

    private void InitCameraRaycast(Camera camera)
    {
        CameraRayCast cameraRayCast = camera.GetComponent<CameraRayCast>();
        cameraRayCast.SetupPlayer(_playerGO.transform);
        Container.Bind<ICameraRaycast>().FromInstance(cameraRayCast).AsSingle();
    }

    private void InitZoomInput(Camera camera)
    {
        StandaloneZoom standaloneZoom = new StandaloneZoom();
        standaloneZoom.SetupCamera(camera);
        Container.Bind<IZoomService>().To<StandaloneZoom>().AsSingle().NonLazy();
    }

    private void InitHudSettings(GameObject hud)
    {
        SoundSliderController[] soundSliderControllers = hud.GetComponentsInChildren<SoundSliderController>();
        for (int i = 0; i < soundSliderControllers.Length; i++)
        {
            Container.Inject(soundSliderControllers[i]);
        }
    }


    private void CameraFollow(GameObject camera, GameObject player)
    {
        CameraFollow cameraFollow = camera.GetComponent<CameraFollow>();
        cameraFollow.SetupPlayer(player.transform);
    }

    private void InitTargetByDistanceActivator(GameObject camera, GameObject player)
    {
        TargetByDistanceActivator targetByDistanceActivator = camera.GetComponent<TargetByDistanceActivator>();
        targetByDistanceActivator.SetPlayer(player.transform);
    }

    private void RegisterAudioService()
    {
        Container.Bind<ISoundService>().FromInstance(_soundManagerGO).AsSingle().NonLazy();
    }
}
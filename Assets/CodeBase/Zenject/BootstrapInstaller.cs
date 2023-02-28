using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.Logic.CameraRaycast;
using CodeBase.Services.Camera;
using CodeBase.Services.Hud;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomaizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.Services.Update;
using CodeBase.Services.Zoom;
using CodeBase.UI.UIInventory;
using CodeBase.UI.UIInventory.Interfaces;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [Inject]
    private DiContainer _container;
    public override void InstallBindings()
    {
        Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle().NonLazy();
        
        GameStateMachine gameStateMachine = new GameStateMachine();
        Container.Bind<GameStateMachine>().FromInstance(gameStateMachine).AsSingle().NonLazy();
        Container.Bind<IGameStateMachine>().FromInstance(gameStateMachine).AsSingle().NonLazy();

        RegisterAssetProvider();
        RegisterStaticData();

        Container.Bind<IRandomService>().To<RandomService>().AsSingle().NonLazy();

        Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle().NonLazy();

        Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle().NonLazy();
        
        Container.Bind<IUpdateService>().To<UpdateManager>().AsSingle().NonLazy();

        Container.Bind<IGameFactory>().To<GameFactory>().AsSingle().WithArguments(
            Container.Resolve<IAssets>(),
            Container.Resolve<IStaticDataService>(),
            Container.Resolve<ISaveLoadService>(),
            _container
            ).NonLazy();
        
        UIInventory uiInventory = new UIInventory();
        _container.Bind<IUIItemInventory>().FromInstance(uiInventory);
        IHudService hudService = new HudStateService();
        _container.Bind<IHudService>().FromInstance(hudService);
    }

    private void RegisterAssetProvider()
    {
        AssetsProvider assetsProvider = new AssetsProvider();
        assetsProvider.Initialize();
        Container.Bind<IAssets>().To<AssetsProvider>().FromInstance(assetsProvider).AsSingle().NonLazy();
    }

    private void RegisterStaticData()
    {
        StaticDataService StaticData = new StaticDataService();
        StaticData.Initialize();
        Container.Bind<IStaticDataService>().To<StaticDataService>().FromInstance(StaticData).AsSingle().NonLazy();
    }

    private IInputService SetupMovementInputService()
    {
        return new StandaloneInputService();
    }
}
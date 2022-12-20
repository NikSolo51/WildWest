﻿using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Hud;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomaizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.Services.Update;
using CodeBase.UI.UIInventory;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;
        private AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }

        private void RegisterServices()
        {
            _services.RegisterSingle<IInputService>(SetupMovementInputService());
            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            RegisterAssetProvider();
            RegisterStaticData();
            _services.RegisterSingle<IRandomService>(new RandomService());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            SaveLoadService saveLoadService = new SaveLoadService(_services.Single<IPersistentProgressService>());
            _services.RegisterSingle<ISaveLoadService>(saveLoadService);
    
            ItemCombiner itemCombiner = new ItemCombiner();
            itemCombiner.Constructor(_services.Single<IStaticDataService>());
            UIInventory inventory = new UIInventory();
            
            AllServices.Container.RegisterSingle<IUpdateService>(new UpdateManager());
            inventory.Construct(_services.Single<IStaticDataService>(), itemCombiner);
            _services.RegisterSingle<IHudService>(new HudStateService());
            _services.RegisterSingle<IItemCombiner>(itemCombiner);
            _services.RegisterSingle<IUIItemInventory>(inventory);
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>(),
                _services.Single<IStaticDataService>(), _services.Single<IPersistentProgressService>(),
                saveLoadService));
        }

        private void RegisterAssetProvider()
        {
            AssetsProvider assetsProvider = new AssetsProvider();
            assetsProvider.Initialize();
            _services.RegisterSingle<IAssets>(assetsProvider);
        }

        private void RegisterStaticData()
        {
            IStaticDataService StaticData = new StaticDataService();
            StaticData.Initialize();
            _services.RegisterSingle<IStaticDataService>(StaticData);
        }

        public void Exit()
        {
        }

        private static IInputService SetupMovementInputService()
        {
            if (Application.isEditor)
            {
                return new StandaloneInputService();
            }
            else
            {
                return new JoystickMovementInputService();
            }
        }
    }
}
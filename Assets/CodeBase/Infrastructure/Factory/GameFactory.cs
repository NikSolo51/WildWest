using System.Threading.Tasks;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Inventory;
using CodeBase.Logic.Camera;
using CodeBase.Logic.PuzzleHud;
using CodeBase.Logic.Spawner;
using CodeBase.Puzzles;
using CodeBase.Services.Audio;
using CodeBase.Services.Camera;
using CodeBase.Services.Hud;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.Services.Update;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _asset;
        private readonly IStaticDataService _staticData;
        private IPersistentProgressService _progressService;
        private readonly SaveLoadService _saveLoadService;

        private GameObject HeroGameObject { get; set; }

        public GameFactory(IAssets assets,
            IStaticDataService staticData,
            IPersistentProgressService progressService,
            SaveLoadService saveLoadService)
        {
            _asset = assets;
            _staticData = staticData;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public async Task WarmUp()
        {
            await _asset.Load<GameObject>(AssetsAdress.Spawner);
            await _asset.Load<GameObject>(AssetsAdress.Hero);
            await _asset.Load<GameObject>(AssetsAdress.UpdateManager);
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetsAdress.Hud);
            return hud;
        }


        public async Task CreatePuzzle(Vector3 at, string spawnerId, PuzzelName puzzelName, Transform hud,
            ISoundService levelSoundManager)
        {
            PuzzleStaticData puzzleStaticData = _staticData.ForPuzzel(puzzelName);

            GameObject puzzleObjectPrefab = await _asset.Load<GameObject>(puzzleStaticData.PuzzelReference);
            GameObject puzzleHudPrefab = await _asset.Load<GameObject>(puzzleStaticData.PuzzelHudReference);

            GameObject puzzleObject = InstantiateRegistered(puzzleObjectPrefab, at);
            puzzleObject.transform.position = at;

            GameObject puzzleHudObject = InstantiateRegistered(puzzleHudPrefab);
            puzzleHudObject.transform.SetParent(hud, false);


            SoundManagerProvider hudSoundManagerProvider = puzzleHudObject?.GetComponent<SoundManagerProvider>();
            if (hudSoundManagerProvider)
                hudSoundManagerProvider.Construct(levelSoundManager);

            SoundManagerProvider soundManagerProvider = puzzleObject?.GetComponent<SoundManagerProvider>();
            if (soundManagerProvider)
                soundManagerProvider.Construct(levelSoundManager);

            EventsProviderForCameraRaycast eventsProviderForCameraRaycast =
                puzzleObject.GetComponentInChildren<EventsProviderForCameraRaycast>();

            eventsProviderForCameraRaycast.Construct(AllServices.Container.Single<ICameraRaycast>());

            PuzzleHudActivityController puzzleHudActivityController =
                puzzleObject.GetComponentInChildren<PuzzleHudActivityController>();

            puzzleHudActivityController.Construct(puzzleHudObject, AllServices.Container.Single<IHudService>());

            Puzzle puzzle = puzzleObject.GetComponentInChildren<Puzzle>();
            puzzle.Construct(puzzleHudObject);
        }

        public async Task<ISoundService> CreateSoundManager(SoundManagerData soundManagerData)
        {
            SoundManagerStaticData soundManagerManagerStaticData =
                _staticData.ForSoundManager(soundManagerData._soundManagerType);

            if (soundManagerData._soundManagerType == SoundManagerType.Nothing)
                Debug.Log("SoundManager Type is Nothing");

            GameObject soundManagerPrefab = await _asset.Load<GameObject>(soundManagerManagerStaticData.SoundManager);

            GameObject soundManagerObject = InstantiateRegistered(soundManagerPrefab);
            SoundManagerAbstract soundManagerAbstract = soundManagerObject.GetComponent<SoundManagerAbstract>();

            soundManagerAbstract.sounds = soundManagerData._sounds;
            soundManagerAbstract.clips = soundManagerData._clips;

            return soundManagerAbstract;
        }


        public async Task<GameObject> CreateHero(Vector3 at, IUpdateService updateService)
        {
            HeroGameObject = await InstantiateRegisteredAsync(AssetsAdress.Hero, at);
            HeroGameObject.transform.rotation = Quaternion.LookRotation(Vector3.right);

            AnimateAlongAgent animateAlongAgent = HeroGameObject.GetComponent<AnimateAlongAgent>();
            animateAlongAgent.Constructor(updateService);
            return HeroGameObject;
        }

        public async Task<GameObject> CreateUpdateManager()
        {
            GameObject updateManager = await InstantiateRegisteredAsync(AssetsAdress.UpdateManager);
            return updateManager;
        }

        public async Task<GameObject> CreateCamera(Vector3 at)
        {
            GameObject cameraGameObject = await InstantiateAsync(AssetsAdress.Camera, at);
            return cameraGameObject;
        }

        public async Task<GameObject> CreateParallax()
        {
            GameObject parallaxGameObject = await InstantiateAsync(AssetsAdress.Parallax);
            return parallaxGameObject;
        }

        public async Task<GameObject> CreateItem(ItemType typeId, Transform parent)
        {
            ItemStaticData itemData = _staticData.ForItem(typeId);

            GameObject prefab = await _asset.Load<GameObject>(itemData.PrefabReference);

            GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity,
                parent.transform);

            return monster;
        }

        public async Task<GameObject> CreateUIItem(ItemType typeId, Transform parent)
        {
            ItemStaticData itemData = _staticData.ForItem(typeId);

            var validateAddress = Addressables.LoadResourceLocationsAsync(itemData.UIPerefab);
            await validateAddress.Task;

            if (validateAddress.Status == AsyncOperationStatus.Succeeded)
            {
                if (validateAddress.Result.Count > 0)
                {
                    GameObject prefab = await _asset.Load<GameObject>(itemData.UIPerefab);


                    GameObject item = Object.Instantiate(prefab, parent.position, Quaternion.identity,
                        parent.transform);

                    return item;
                }
            }

            return null;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            GameObject gameObject = await _asset.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }


        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await _asset.Instantiate(prefabPath, at: at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await _asset.Instantiate(prefabPath, at: at);
            return gameObject;
        }

        private async Task<GameObject> InstantiateAsync(string prefabPath)
        {
            GameObject gameObject = await _asset.Instantiate(prefabPath);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
            {
                _saveLoadService.ProgressWriters.Add(progressWriter);
            }

            _saveLoadService.ProgressReaders.Add(progressReader);
        }

        public void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _saveLoadService.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }

        public void CleanUp()
        {
            _saveLoadService.ProgressReaders.Clear();
            _saveLoadService.ProgressWriters.Clear();

            _asset.CleanUp();
        }
    }
}
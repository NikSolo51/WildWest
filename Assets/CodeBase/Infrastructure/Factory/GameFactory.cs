using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Inventory;
using CodeBase.Services.Audio;
using CodeBase.Services.Audio.SoundManager;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private IAssets _asset;
        private IStaticDataService _staticData;
        private ISaveLoadService _saveLoadService;
        private DiContainer _container;
        private GameObject HeroGameObject { get; set; }

        public GameFactory(IAssets assets,
            IStaticDataService staticData,
            ISaveLoadService saveLoadService,
            DiContainer container)
        {
            _asset = assets;
            _staticData = staticData;
            _saveLoadService = saveLoadService;
            _container = container;
        }

        public async Task WarmUp()
        {
            await _asset.Load<GameObject>(AssetsAdress.Hero);
            await _asset.Load<GameObject>(AssetsAdress.UpdateManager);
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetsAdress.Hud);
            return hud;
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


                    GameObject item = Object.Instantiate(prefab, prefab.transform.position, Quaternion.identity,
                        parent.parent.parent);
                    
                    return item;
                }
            }

            return null;
        }
      
        
        public async Task<GameObject> CreateHero(Vector3 at)
        {
            HeroGameObject = await InstantiateRegisteredAsync(AssetsAdress.Hero, at);
            HeroGameObject.transform.rotation = Quaternion.LookRotation(Vector3.right);

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
            ISavedProgressReader[] readers = gameObject.GetComponentsInChildren<ISavedProgressReader>();
            for (var index = 0; index < readers.Length; index++)
            {
                ISavedProgressReader progressReader = readers[index];
                _saveLoadService.Register(progressReader);
            }
        }

        public void CleanUp()
        {
            _saveLoadService.CleanUp();
            _asset.CleanUp();
        }
    }
}
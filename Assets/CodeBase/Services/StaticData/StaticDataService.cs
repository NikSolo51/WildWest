using System.Collections.Generic;
using System.Linq;
using CodeBase.Inventory;
using CodeBase.Logic.Spawner;
using CodeBase.Services.Audio.SoundManager;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : MonoBehaviour, IStaticDataService
    {
        private Dictionary<ItemType, ItemStaticData> _items;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<SoundManagerType, SoundManagerStaticData> _soundManagers;
        private Dictionary<ItemType, RecipeStaticData> _recipesDictionary;
        private List<RecipeStaticData> _recipesList;

        public void Initialize()
        {
          
            IList<LevelStaticData> levelStaticData =
                LoadResources<LevelStaticData>("Level");

            _levels = levelStaticData.ToDictionary(x => x.LevelKey, x => x);

            IList<SoundManagerStaticData> soundSystems =
                LoadResources<SoundManagerStaticData>("SoundManager");
            
            _soundManagers = soundSystems.ToDictionary(x => x.SoundManagerType, x => x);

            IList<RecipeStaticData> recipeData = LoadResources<RecipeStaticData>("Recipe");
            _recipesDictionary = recipeData.ToDictionary(x => x.craftItem, x => x);
            _recipesList = _recipesDictionary.Values.ToList();
            
            IList<ItemStaticData> itemsData = LoadResources<ItemStaticData>("Item");
            _items = itemsData.ToDictionary(x => x.itemType, x => x);

        }
        
        private IList<T> LoadResources<T>(string dataName)
        {
            IList<IResourceLocation> resourceLocations =
                Addressables.LoadResourceLocationsAsync(dataName, typeof(T))
                    .WaitForCompletion();
            IList<T> resource =
                Addressables.LoadAssets<T>(resourceLocations, null)
                    .WaitForCompletion();
            return resource;
        }
        

        public ItemStaticData ForItem(ItemType typeId)
        {
            return _items.TryGetValue(typeId, out ItemStaticData staticData)
                ? staticData
                : null;
        }

        public LevelStaticData ForLevel(string sceneKey)
        {
            return _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
                ? staticData
                : null;
        }

        public RecipeStaticData ForRecipe(ItemType recipeType)
        {
            return _recipesDictionary.TryGetValue(recipeType, out RecipeStaticData staticData)
                ? staticData
                : null;
        }

        public SoundManagerStaticData ForSoundManager(SoundManagerType soundManagerType)
        {
            return _soundManagers.TryGetValue(soundManagerType, out SoundManagerStaticData staticData)
                ? staticData
                : null;
        }
        
        public List<RecipeStaticData> GetAllRecipes()
        {
            return _recipesList;
        }
    }
}
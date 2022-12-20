using System.Collections.Generic;
using System.Linq;
using CodeBase.Inventory;
using CodeBase.Logic.Spawner;
using CodeBase.Services.Audio;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<ItemType, ItemStaticData> _items;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<ItemType, RecipeStaticData> _recipesDictionary;
        private Dictionary<PuzzelName, PuzzleStaticData> _puzzels;
        private Dictionary<SoundManagerType, SoundManagerStaticData> _soundManagers;
        private List<RecipeStaticData> _recipes;

        public void Initialize()
        {
            _items = Resources.LoadAll<ItemStaticData>("StaticData/Items").ToDictionary(x => x.itemType, x => x);
            _levels = Resources.LoadAll<LevelStaticData>("StaticData/Levels").ToDictionary(x => x.LevelKey, x => x);
            _recipesDictionary = Resources.LoadAll<RecipeStaticData>("StaticData/Recipes")
                .ToDictionary(x => x.craftItem, x => x);
            _puzzels = Resources.LoadAll<PuzzleStaticData>("StaticData/Puzzles")
                .ToDictionary(x => x.PuzzelName, x => x); 
            _soundManagers = Resources.LoadAll<SoundManagerStaticData>("StaticData/SoundManagers")
                .ToDictionary(x => x.SoundManagerType, x => x);
            
            _recipes = _recipesDictionary.Values.ToList();
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

        public PuzzleStaticData ForPuzzel(PuzzelName puzzelName)
        {
            return _puzzels.TryGetValue(puzzelName, out PuzzleStaticData staticData)
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
            return _recipes;
        }
    }
}
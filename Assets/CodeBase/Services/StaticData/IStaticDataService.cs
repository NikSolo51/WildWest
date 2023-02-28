using System.Collections.Generic;
using CodeBase.Inventory;
using CodeBase.Services.Audio.SoundManager;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService 
    {
        void Initialize();
        LevelStaticData ForLevel(string sceneKey);

        SoundManagerStaticData ForSoundManager(SoundManagerType soundManagerType);
        List<RecipeStaticData> GetAllRecipes();
        ItemStaticData ForItem(ItemType typeId);
        RecipeStaticData ForRecipe(ItemType recipeType);
    }
}
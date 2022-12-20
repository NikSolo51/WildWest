using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Inventory;
using CodeBase.Logic.Spawner;
using CodeBase.Services.Audio;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void Initialize();
        ItemStaticData ForItem(ItemType typeId);
        LevelStaticData ForLevel(string sceneKey);

        List<RecipeStaticData> GetAllRecipes();
        PuzzleStaticData ForPuzzel(PuzzelName puzzelName);
        SoundManagerStaticData ForSoundManager(SoundManagerType soundManagerType);
    }
}
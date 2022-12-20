using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Inventory;
using CodeBase.Services.StaticData;

namespace CodeBase.UI.UIInventory
{
    public interface IItemCombiner : IService
    {
        public void Select(ItemType type);
        public void Clear();
        RecipeStaticData GetCombinedItem();
        List<ItemType> GetSelectedItem();
        List<ItemType> GetUsedItem();
        void DeSelect(ItemType type);
    }
}
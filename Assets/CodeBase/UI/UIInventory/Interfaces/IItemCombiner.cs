using System.Collections.Generic;
using CodeBase.Inventory;
using CodeBase.Services.StaticData;

namespace CodeBase.UI.UIInventory.Interfaces
{
    public interface IItemCombiner 
    {
        public void Select(ItemType type);
        public void Clear();
        RecipeStaticData GetCombinedItem();
        List<ItemType> GetSelectedItem();
        List<ItemType> GetUsedItem();
        void DeSelect(ItemType type);
    }
}
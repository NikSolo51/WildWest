using System.Collections.Generic;
using CodeBase.Inventory;
using CodeBase.Services.StaticData;
using CodeBase.UI.UIInventory.Interfaces;

namespace CodeBase.UI.UIInventory
{
    public class ItemCombiner : IItemCombiner
    {
        private List<ItemType> _selected = new List<ItemType>();
        private List<ItemType> _used = new List<ItemType>();
        private IStaticDataService _staticDataService;

        public void Constructor(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Select(ItemType type)
        {
            if (_selected.Contains(type))
                return;
            _selected.Add(type);
        }
        
        public void DeSelect(ItemType type)
        {
            if (!_selected.Contains(type))
                return;
            _selected.Remove(type);
        }

        public RecipeStaticData GetCombinedItem()
        {
            if (_selected.Count < 1)
                return null;

            RecipeStaticData recipeData = TryCombine();

            return recipeData;
        }

        private RecipeStaticData TryCombine()
        {
            List<RecipeStaticData> recipes = _staticDataService.GetAllRecipes();

            for (int i = 0; i < recipes.Count; i++)
            {
                if (SelectedContainComponents(recipes[i].Components) && recipes[i].Components.Count == _selected.Count)
                {
                    return recipes[i];
                }
            }

            return null;
        }

        private bool SelectedContainComponents(List<ItemType> components)
        {
            _used.Clear();
            for (int i = 0; i < components.Count; i++)
            {
                if (!_selected.Contains(components[i]))
                {
                    _used.Clear();
                    return false;
                }

                _used.Add(components[i]);
            }

            return true;
        }

        public void Clear()
        {
            _selected.Clear();
        }

        public List<ItemType> GetSelectedItem()
        {
            return _selected;
        }

        public List<ItemType> GetUsedItem()
        {
            return _used;
        }
    }
}
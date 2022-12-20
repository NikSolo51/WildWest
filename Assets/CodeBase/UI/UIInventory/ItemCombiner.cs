using System.Collections.Generic;
using CodeBase.Inventory;
using CodeBase.Services.StaticData;

namespace CodeBase.UI.UIInventory
{
    public class ItemCombiner : IItemCombiner
    {
        private List<ItemType> selected = new List<ItemType>();
        private List<ItemType> used = new List<ItemType>();
        private IStaticDataService _staticDataService;

        public void Constructor(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Select(ItemType type)
        {
            if (selected.Contains(type))
                return;
            selected.Add(type);
        }
        
        public void DeSelect(ItemType type)
        {
            if (!selected.Contains(type))
                return;
            selected.Remove(type);
        }


        public RecipeStaticData GetCombinedItem()
        {
            if (selected.Count < 1)
                return null;

            RecipeStaticData recipeData = TryCombine();

            return recipeData;
        }

        private RecipeStaticData TryCombine()
        {
            /// <summary>
            /// Сейчас всё решается перебором, если будет высокая нагрузка, то можно будет
            /// создать систему типа Dictionary<string,Dictionary<ItemRecipeType, RecipeStaticData>>
            /// Где string это название уровня/вагона, к которому привязан предмет, что бы поиск шёл,
            ///лишь в этом конкретно dictionary
            /// </summary>

            List<RecipeStaticData> recipes = _staticDataService.GetAllRecipes();

            for (int i = 0; i < recipes.Count; i++)
            {
                if (SelectedContainComponents(recipes[i].Components) && recipes[i].Components.Count == selected.Count)
                {
                    return recipes[i];
                }
            }

            return null;
        }

        private bool SelectedContainComponents(List<ItemType> components)
        {
            used.Clear();
            for (int i = 0; i < components.Count; i++)
            {
                if (!selected.Contains(components[i]))
                {
                    used.Clear();
                    return false;
                }

                used.Add(components[i]);
            }

            return true;
        }

        public void Clear()
        {
            selected.Clear();
        }

        public List<ItemType> GetSelectedItem()
        {
            return selected;
        }

        public List<ItemType> GetUsedItem()
        {
            return used;
        }
    }
}
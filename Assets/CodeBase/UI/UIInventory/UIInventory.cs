using System.Collections.Generic;
using CodeBase.Inventory;
using CodeBase.Services.StaticData;

namespace CodeBase.UI.UIInventory
{
    public class UIInventory : IUIItemInventory
    {
        public List<UISlot> inventory = new List<UISlot>();
        private int currentSlotIndex;
        private IStaticDataService _staticDataService;
        private IItemCombiner _itemCombiner;
        
        public void Construct(IStaticDataService staticDataService, IItemCombiner itemCombiner)
        {
            _staticDataService = staticDataService;
            _itemCombiner = itemCombiner;
        }

        public void RegisterNewSlot(UISlot slot)
        {
            inventory.Add(slot);
        }

        public void RegisterNewItem(ItemType itemType)
        {
            if (currentSlotIndex + 1 >= inventory.Count)
                return;

            int newIndex = GetNotFilledSlot();

            if (newIndex < 0)
                return;

            ItemStaticData itemData = _staticDataService.ForItem(itemType);
  
            currentSlotIndex = newIndex;
            
            inventory[currentSlotIndex].itemType = itemType;
            inventory[currentSlotIndex]._image.sprite = itemData.Sprite;
            inventory[currentSlotIndex].description = itemData.Description;
            inventory[currentSlotIndex].filled = true;
        }

        private int GetNotFilledSlot()
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (!inventory[i].filled)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Select(ItemType type)
        {
            _itemCombiner.Select(type);
            TryCombineItems();
        }

        public void Deselect(ItemType type)
        {
            _itemCombiner.DeSelect(type);
            TryCombineItems();
        }

        private void TryCombineItems()
        {
            RecipeStaticData data = _itemCombiner.GetCombinedItem();

            if (!data)
                return;

            ClearUsedItems();
            RegisterNewItem(data.craftItem);
        }

        private UISlot GetSlotByType(ItemType type)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemType == type)
                    return inventory[i];
            }

            return null;
        }

        private void ClearUsedItems()
        {
            List<ItemType> used = _itemCombiner.GetUsedItem();
            for (int i = 0; i < inventory.Count; i++)
            {
                for (int j = 0; j < used.Count; j++)
                {
                    if (inventory[i].itemType == used[j])
                    {
                        inventory[i].Clear();
                        currentSlotIndex--;
                    }
                }
            }

            ClearSelected();
        }

        private void ClearSelected()
        {
            _itemCombiner.Clear();
        }

        public void Clear()
        {
            currentSlotIndex = 0;
            inventory.Clear();
        }
    }
}
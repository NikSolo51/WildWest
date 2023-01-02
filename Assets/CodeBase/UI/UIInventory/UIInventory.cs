using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Inventory;
using CodeBase.Services.StaticData;
using CodeBase.UI.UIInventory.Interfaces;

namespace CodeBase.UI.UIInventory
{
    public class UIInventory : IUIItemInventory
    {
        private List<UISlot> _inventory = new List<UISlot>();
        private int _currentSlotIndex;
        private IStaticDataService _staticDataService;
        private IItemCombiner _itemCombiner;
        
        public void Construct(IStaticDataService staticDataService, IItemCombiner itemCombiner)
        {
            _staticDataService = staticDataService;
            _itemCombiner = itemCombiner;
        }

        public void RegisterNewSlot(UISlot slot)
        {
            _inventory.Add(slot);
        }

        public void RegisterNewItem(ItemType itemType)
        {
            if(itemType == ItemType.Nothing)
                return;
            
            if (_currentSlotIndex + 1 >= _inventory.Count)
                return;

            int newIndex = GetNotFilledSlot();

            if (newIndex < 0)
                return;

            ItemStaticData itemData = _staticDataService.ForItem(itemType);
  
            _currentSlotIndex = newIndex;
            
            _inventory[_currentSlotIndex]._itemType = itemType;
            _inventory[_currentSlotIndex]._image.sprite = itemData.Sprite;
            _inventory[_currentSlotIndex].description = itemData.Description;
            _inventory[_currentSlotIndex].filled = true;
        }

        private int GetNotFilledSlot()
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (!_inventory[i].filled)
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
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i]._itemType == type)
                    return _inventory[i];
            }

            return null;
        }

        private void ClearUsedItems()
        {
            List<ItemType> used = _itemCombiner.GetUsedItem();
            for (int i = 0; i < _inventory.Count; i++)
            {
                for (int j = 0; j < used.Count; j++)
                {
                    if (_inventory[i]._itemType == used[j])
                    {
                        _inventory[i].Clear();
                        _currentSlotIndex--;
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
            _currentSlotIndex = 0;
            _inventory.Clear();
        }


        public void LoadProgress(PlayerProgress progress)
        {
            for (int i = 0; i < progress.InventoryData.ItemTypes.Count; i++)
            {
                RegisterNewItem(progress.InventoryData.ItemTypes[i]);
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.InventoryData.ItemTypes.Clear();
            for (int i = 0; i < _inventory.Count; i++)
            {
                progress.InventoryData.ItemTypes.Add(_inventory[i]._itemType);
            }
        }
    }
}
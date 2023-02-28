using CodeBase.Infrastructure.Services;
using CodeBase.Inventory;
using CodeBase.Services.SaveLoad;

namespace CodeBase.UI.UIInventory.Interfaces
{
    public interface IUIItemInventory : IService,ISavedProgress
    {
        void RegisterNewItem(ItemType itemType);
        void RegisterNewSlot(UISlot slot);
        void Select(ItemType type);

        void Clear();
        void Deselect(ItemType type);
    }
}
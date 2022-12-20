using CodeBase.Infrastructure.Services;
using CodeBase.Inventory;

namespace CodeBase.UI.UIInventory
{
    public interface IUIItemInventory : IService
    {
        void RegisterNewItem(ItemType itemType);
        void RegisterNewSlot(UISlot slot);
        void Select(ItemType type);

        void Clear();
        void Deselect(ItemType type);
    }
}
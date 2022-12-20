using CodeBase.Infrastructure.Services;
using CodeBase.UI.UIInventory;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Inventory
{
    public class ItemMarker : MonoBehaviour
    {
        [FormerlySerializedAs("uiItemType")] [FormerlySerializedAs("_itemType")]
        public ItemType itemType;

        private IUIItemInventory _itemInventory;

        private void Awake()
        {
            _itemInventory = AllServices.Container.Single<IUIItemInventory>();
        }

        private void OnTriggerEnter(Collider other)
        {
            //mouse click too
            _itemInventory.RegisterNewItem(itemType);
            Destroy(gameObject);
        }
    }
}
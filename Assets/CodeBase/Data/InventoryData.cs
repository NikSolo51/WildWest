using System;
using System.Collections.Generic;
using CodeBase.Inventory;

namespace CodeBase.Data
{
    [Serializable]
    public class InventoryData
    {
        public List<ItemType> ItemTypes = new List<ItemType>();
    }
}
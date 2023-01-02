using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public InventoryData InventoryData;
        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            InventoryData = new InventoryData();
        }
    }
}
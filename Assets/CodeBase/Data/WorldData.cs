using System;

namespace CodeBase.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public ItemGivers ItemGivers;
        public Targets Targets;
        public WorldData(string initialLevel)
        {
            PositionOnLevel = new PositionOnLevel(initialLevel);
            ItemGivers = new ItemGivers();
            Targets = new Targets();
        }
    }
}
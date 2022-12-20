using System.Collections.Generic;
using CodeBase.Inventory;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Services.StaticData
{
    [CreateAssetMenu(fileName = "RecipeData", menuName = "StaticData/Recipes", order = 0)]
    public class RecipeStaticData : ScriptableObject
    {
        [FormerlySerializedAs("craftUiItem")] [FormerlySerializedAs("CraftItem")]
        public ItemType craftItem;

        public List<ItemType> Components;
    }
}
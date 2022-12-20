using CodeBase.Inventory;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace CodeBase.Services.StaticData
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "StaticData/Item", order = 0)]
    public class ItemStaticData : ScriptableObject
    {
        [FormerlySerializedAs("uiItemType")] [FormerlySerializedAs("ItemType")]
        public ItemType itemType;

        public Sprite Sprite;
        [TextArea] public string Description;
        [Header("For Instantiate UI")] public AssetReferenceGameObject UIPerefab;
        [Header("For Instantiate")] public AssetReferenceGameObject PrefabReference;
    }
}
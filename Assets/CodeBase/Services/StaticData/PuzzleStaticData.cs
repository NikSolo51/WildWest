using CodeBase.Logic.Spawner;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Services.StaticData
{
    [CreateAssetMenu(fileName = "Puzzle", menuName = "StaticData/Puzzle", order = 0)]
    public class PuzzleStaticData : ScriptableObject
    {
        public PuzzelName PuzzelName;
        public AssetReferenceGameObject PuzzelReference;
        public AssetReferenceGameObject PuzzelHudReference;
    }
}
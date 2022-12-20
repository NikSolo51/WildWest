using System.Collections.Generic;
using CodeBase.Services.Audio;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        [Header("Sound Manager Interface [Invisible in inspector]")]
        public string LevelKey;
      
        public SoundManagerData SoundManagerData;
        public List<PuzzleSpawnerData> PuzzleSpawners;

        public Vector3 InitialHeroPosition;
        public Vector3 InitialCameraPosition;
    }
}
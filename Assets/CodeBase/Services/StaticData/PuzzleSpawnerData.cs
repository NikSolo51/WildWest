using System;
using CodeBase.Logic.Spawner;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    [Serializable]
    public class PuzzleSpawnerData
    {
        public PuzzelName puzzelName;
        public Vector3 _position;
        public string id;

        public PuzzleSpawnerData(string id, PuzzelName puzzelName, Vector3 position)
        {
            this.id = id;
            this.puzzelName = puzzelName;
            _position = position;
        }
    }
}
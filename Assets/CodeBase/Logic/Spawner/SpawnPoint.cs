using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Logic.Spawner
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        [FormerlySerializedAs("PuzzleName")] public PuzzelName puzzelName;
        public string Id { get; set; }

        public bool Slain;
        private IGameFactory _factory;

        public void Construct(IGameFactory factory) => _factory = factory;

        public void LoadProgress(PlayerProgress progress)
        {
            // if (progress.KillData.ClearedSpawners.Contains(Id))
            // {
            //     Slain = true;
            // }
            // else
            // {
            //     Spawn();
            // }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            // if (Slain)
            //     progress.KillData.ClearedSpawners.Add(Id);
        }

        private async void Spawn()
        {
            //GameObject monster = await _factory.CreateItem(puzzleName, transform);
        }
    }
}
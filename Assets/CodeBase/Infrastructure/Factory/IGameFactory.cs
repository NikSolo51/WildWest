﻿using System.Threading.Tasks;
using CodeBase.Infrastructure.Services;
using CodeBase.Inventory;
using CodeBase.Logic.Spawner;
using CodeBase.Services.Audio;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        Task<GameObject> CreateUIItem(ItemType typeId, Transform parent);
        Task<GameObject> CreateItem(ItemType typeId, Transform parent);
        Task<GameObject> CreateHero(Vector3 at);
        Task<GameObject> CreateHud();
        Task CreatePuzzle(Vector3 at, string spawnerId, PuzzelName uiItemType, Transform parent,
            ISoundService levelSoundManager);
        void InformProgressReaders();
        void CleanUp();

        Task WarmUp();
        Task<GameObject> CreateCamera(Vector3 at);
        Task<GameObject> CreateParallax();
        Task<ISoundService> CreateSoundManager(SoundManagerData soundManagerData);
        Task<GameObject> CreateUpdateManager();
    }
}
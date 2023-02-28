using System.Threading.Tasks;
using CodeBase.Inventory;
using CodeBase.Services.Audio;
using CodeBase.Services.Audio.SoundManager;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory
    {
        Task<GameObject> CreateHero(Vector3 at);
        Task<GameObject> CreateHud();
        Task<GameObject> CreateCamera(Vector3 at);
        Task<ISoundService> CreateSoundManager(SoundManagerData soundManagerData);
        Task<GameObject> CreateUpdateManager();

        Task WarmUp();
        void CleanUp();
        Task<GameObject> CreateUIItem(ItemType typeId, Transform parent);
    }
}
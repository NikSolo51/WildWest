using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.UI.UIInventory.Interfaces;
using UnityEngine;

namespace CodeBase.Logic
{
    public class StartNewGame : MonoBehaviour
    {
        public void NewGame()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            AllServices.Container.Single<IUIItemInventory>().Clear();
            AllServices.Container.Single<ISaveLoadService>().CleanUp();
            AllServices.Container.Single<IPersistentProgressService>().Progress = new PlayerProgress(Constants.InitialLevel);
            AllServices.Container.Single<IGameStateMachine>().Enter<BootstrapState>();
        }
    }
}
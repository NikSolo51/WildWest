using System;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Inventory;
using CodeBase.Logic;
using CodeBase.Services.SaveLoad;
using CodeBase.UI.UIInventory.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.Puzzles
{
    [RequireComponent(typeof(UniqueId))]
    public class ItemGiver : PuzzleFinisher, ISavedProgress
    {
        public List<Puzzle> Puzzles;
        [SerializeField] private bool giveOneTime = true;
        public List<ItemType> items = new List<ItemType>();
        private IUIItemInventory _itemInventory;
        public override event Action OnFinished;
        public UnityEvent OnFinishedUnityEvent;
        private bool _gave;
        private ISaveLoadService _saveLoadService;
        private string id;

        private void Start()
        {
            _itemInventory = AllServices.Container.Single<IUIItemInventory>();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _saveLoadService.Register(this);
            id = GetComponent<UniqueId>().Id;
      
            if (Puzzles.Count > 0)
                for (int i = 0; i < Puzzles.Count; i++)
                {
                    if (Puzzles[i])
                        Puzzles[i].OnPuzzleSolved += Finish;
                }
        }

        private void OnDestroy()
        {
            if (Puzzles.Count > 0)
                for (int i = 0; i < Puzzles.Count; i++)
                {
                    if (Puzzles[i])
                        Puzzles[i].OnPuzzleSolved -= Finish;
                }
        }

        public override void Finish()
        {
            if (_gave)
                return;

            if (items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    _itemInventory.RegisterNewItem(items[i]);
                }
            }

            OnFinished?.Invoke();
            OnFinishedUnityEvent?.Invoke();

            if (giveOneTime)
                _gave = true;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.WorldData.ItemGivers.usedItemGivers.Contains(id))
            {
                _gave = true;
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_gave)
            {
                if (!progress.WorldData.ItemGivers.usedItemGivers.Contains(id))
                {
                    progress.WorldData.ItemGivers.usedItemGivers.Add(id);
                }
            }
        }
    }
}
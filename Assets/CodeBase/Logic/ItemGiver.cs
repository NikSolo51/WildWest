using System;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Inventory;
using CodeBase.Logic;
using CodeBase.Services.SaveLoad;
using CodeBase.UI.UIInventory.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace CodeBase.Puzzles
{
    [RequireComponent(typeof(UniqueId))]
    public class ItemGiver : PuzzleFinisher, ISavedProgress
    {
        public List<Puzzle> Puzzles;
        [SerializeField] private bool giveOneTime = true;
        public List<ItemType> items = new List<ItemType>();
        public override event Action OnFinished;
        public UnityEvent OnFinishedUnityEvent;
        private bool _gave;
        private string id;
        private IUIItemInventory _itemInventory;
        private ISaveLoadService _saveLoadService;

        [Inject]
        public void Construct(IUIItemInventory itemInventory, ISaveLoadService saveLoadService)
        {
            _itemInventory = itemInventory;
            _saveLoadService = saveLoadService;
            _saveLoadService.Register(this);
        }

        private void Start()
        {
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
using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Inventory;
using CodeBase.UI.UIInventory;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.Puzzles
{
    public class ItemGiver : PuzzleFinisher
    {
        public List<Puzzle> Puzzles;
        [SerializeField] private bool giveOneTime = true;
        public List<ItemType> items = new List<ItemType>();
        private IUIItemInventory _itemInventory;
        public override event Action OnFinished;
        public UnityEvent OnFinishedUnityEvent;
        private bool gave;


        private void Start()
        {
            _itemInventory = AllServices.Container.Single<IUIItemInventory>();
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
            if (gave)
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
                gave = true;
        }
    }
}
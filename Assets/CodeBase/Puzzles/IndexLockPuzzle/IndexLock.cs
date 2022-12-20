using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Logic.CloseHud;
using CodeBase.Logic.IndexCollector;
using CodeBase.Logic.IndexCollector.IndexCollectorUIProvider;
using CodeBase.Logic.PuzzleHud;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.Puzzles.IndexLockPuzzle
{
    public class IndexLock : Puzzle
    {
        public int[] _locksIndexesExpectation;
        public PuzzleHudActivityController _puzzleHudActivityController;
        private ICloseHud _closeHudProvider;
        private ICurrentIndexCollector _currentIndexCollector;
        private IndexCollectorHUDEventProvider _hudEventProvider;
        private int[] currentLocksIndexes;

        public override bool Solved { set; get; }
        public override event Action OnPuzzleSolved;
        public UnityEvent OnPuzzleSolvedUnityEvent;

        public override void Construct(GameObject puzzleHud)
        {
            _closeHudProvider = puzzleHud.GetComponent<ICloseHud>();
            _hudEventProvider = puzzleHud.GetComponent<IndexCollectorHUDEventProvider>();
            _currentIndexCollector = puzzleHud.GetComponent<ICurrentIndexCollector>();
            
            Initialize();
        }

        private void Initialize()
        {
            
            _closeHudProvider.OnCloseHud += _puzzleHudActivityController.DisableHud;
            _closeHudProvider.OnCloseHud += IndexMatch;
            _hudEventProvider.OnCheckIndexMatch += IndexMatch;
            
            currentLocksIndexes = new int[_locksIndexesExpectation.Length];
        }

        private void OnDestroy()
        {
            _closeHudProvider.OnCloseHud -= _puzzleHudActivityController.DisableHud;
            _closeHudProvider.OnCloseHud -= IndexMatch;
            _hudEventProvider.OnCheckIndexMatch -= IndexMatch;
        }

        public void IndexMatch()
        {
            if (!Solved)
            {
                SetCurrentLocksIndexes();
                if (IsIndicesMatched())
                {
                    SolvePuzzle();
                }
            }
        }

        public override void SolvePuzzle()
        {
            OnPuzzleSolved?.Invoke();
            OnPuzzleSolvedUnityEvent?.Invoke();
            _puzzleHudActivityController.StopActivateHud();
            _puzzleHudActivityController.DisableHud();
            Solved = true;
        }

        public bool IsIndicesMatched()
        {
            return Enumerable.SequenceEqual(
                currentLocksIndexes, _locksIndexesExpectation);
        }

        private void SetCurrentLocksIndexes()
        {
            List<ContainCurrentIndex> currentIndicies = _currentIndexCollector.GetCurrentIndicies();
            for (int i = 0; i < currentLocksIndexes.Length; i++)
            {
                currentLocksIndexes[i] = currentIndicies[i].currentIndex;
            }
        }
    }
}
using System;
using UnityEngine;

namespace CodeBase.Puzzles
{
    public abstract class Puzzle : MonoBehaviour
    {
        public abstract void Construct(GameObject puzzleHud);
        public abstract bool Solved { get; set; }
        public abstract void SolvePuzzle();

        public abstract event Action OnPuzzleSolved;
    }
}
using System;
using UnityEngine;

namespace CodeBase.Puzzles
{
    public abstract class PuzzleFinisher : MonoBehaviour
    {
        public abstract void Finish();
        public abstract event Action OnFinished;
    }
}
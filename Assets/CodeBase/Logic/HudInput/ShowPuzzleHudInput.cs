using System;
using UnityEngine;

namespace CodeBase.Logic.HudInput
{
    public abstract class ShowPuzzleHudInput : MonoBehaviour
    {
        public abstract event Action OnShowHud;
    }
}
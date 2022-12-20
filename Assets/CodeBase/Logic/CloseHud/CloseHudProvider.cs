using System;
using UnityEngine;

namespace CodeBase.Logic.CloseHud
{
    public class CloseHudProvider : MonoBehaviour, ICloseHud
    {
        public event Action OnCloseHud;

        public void CloseHud()
        {
            OnCloseHud?.Invoke();
        }
    }
}
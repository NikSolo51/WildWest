using System;
using UnityEngine;

namespace CodeBase.Logic.IndexCollector.IndexCollectorUIProvider
{
    public class IndexCollectorHUDEventProvider : MonoBehaviour
    {
        public event Action OnCheckIndexMatch;

        public void CheckIndexMatch()
        {
            OnCheckIndexMatch?.Invoke();
        }
    }
}
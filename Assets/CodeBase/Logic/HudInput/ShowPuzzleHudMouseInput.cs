using System;
using CodeBase.Logic.Events;

namespace CodeBase.Logic.HudInput
{
    public class ShowPuzzleHudMouseInput : ShowPuzzleHudInput
    {
        public MouseEventProvider _mouseEventProvider;

        public override event Action OnShowHud;

        private void Start()
        {
            _mouseEventProvider.OnMouseDownEvent += ShowInterface;
        }

        private void OnDestroy()
        {
            _mouseEventProvider.OnMouseDownEvent -= ShowInterface;
        }

        private void ShowInterface()
        {
            OnShowHud?.Invoke();
        }
    }
}
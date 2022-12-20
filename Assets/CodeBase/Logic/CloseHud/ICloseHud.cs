using System;

namespace CodeBase.Logic.CloseHud
{
    public interface ICloseHud
    {
        event Action OnCloseHud;
        void CloseHud();
    }
}
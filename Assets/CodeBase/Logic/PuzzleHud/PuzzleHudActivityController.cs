using CodeBase.Services.Hud;
using UnityEngine;

namespace CodeBase.Logic.PuzzleHud
{
    public abstract class PuzzleHudActivityController : MonoBehaviour
    {
        public abstract void Construct(GameObject hud,IHudService hudService);
        public abstract bool HudControllerEnable { get; set; }
        public abstract void EnableHud();
        public abstract void DisableHud();
        public abstract void StopActivateHud();
    }
}
using CodeBase.Services.Hud;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.Logic.PuzzleHud
{
    public class PuzzleHudActivityControllerUnityEvent : PuzzleHudActivityController
    {
        private GameObject _hud;
        public UnityEvent OnEnableInterface;
        public UnityEvent OnDisableInterface;
        public override bool HudControllerEnable { get; set; } = true;

        private bool hudIsActive;
        private IHudService _hudService;

        public override void Construct(GameObject hud,IHudService hudService)
        {
            _hud = hud;
            _hudService = hudService;
        }

        public override void EnableHud()
        {
            if (!HudControllerEnable)
                return;
            if(hudIsActive)
                return;
            if(_hudService.GetState() != HudState.Empty)
                return;
            
            hudIsActive = true;
            
            _hud.SetActive(true);
            _hudService.ChangeState(HudState.PuzzleHudOpen);
            
            OnEnableInterface.Invoke();
        }

        public override void DisableHud()
        {

            hudIsActive = false;
            _hud.SetActive(false);
            
            _hudService.ChangeState(HudState.Empty);

            OnDisableInterface?.Invoke();
        }

        public override void StopActivateHud()
        {
            HudControllerEnable = false;
        }
    }
}
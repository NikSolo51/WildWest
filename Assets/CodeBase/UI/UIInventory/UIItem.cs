using System;
using CodeBase.Services.Hud;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Zenject;

namespace CodeBase.UI.UIInventory
{
    public class UIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool _inObject;
        private IInputService _inputService;
        private IHudService _hudService;
        
        [SerializeField] private UnityEvent OnCloseObjectUnityEvent;
        public event Action OnCloseObject;
        
        [Inject]
        public void Construct(IInputService inputService, IHudService hudService)
        {
            _inputService = inputService;
            _hudService = hudService;
        }
        

        private void Update()
        {
            if (_inputService.IsClickButtonUp() && !_inObject)
            {
                gameObject.SetActive(false);
                _hudService.ChangeState(HudState.Empty);
                OnCloseObjectUnityEvent?.Invoke();
                OnCloseObject?.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _inObject = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _inObject = false;
        }
    }
}
using System;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Hud;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CodeBase.UI.UIInventory
{
    public class UIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool _inObject;
        private IInputService _inputService;
        private IHudService _hudService;
        
        [SerializeField] private UnityEvent OnCloseObjectUnityEvent;
        public event Action OnCloseObject;

        private void Start()
        {
            _inputService = AllServices.Container.Single<IInputService>();
            _hudService = AllServices.Container.Single<IHudService>();
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
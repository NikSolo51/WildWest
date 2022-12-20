using System;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Hud;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace CodeBase.UI.UIInventory
{
    public class UIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool inObject;
        private IInputService _inputService;
        [FormerlySerializedAs("CloseObject")] public UnityEvent closeObjectUnityEvent;
        public event Action OnCloseObject;
        private IHudService _hudService;
        private void Start()
        {
            _inputService = AllServices.Container.Single<IInputService>();
            _hudService = AllServices.Container.Single<IHudService>();
        }

        private void Update()
        {
            if (_inputService.IsClickButtonUp() && !inObject)
            {
                gameObject.SetActive(false);
                _hudService.ChangeState(HudState.Empty);
                closeObjectUnityEvent?.Invoke();
                OnCloseObject?.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            inObject = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inObject = false;
        }
    }
}
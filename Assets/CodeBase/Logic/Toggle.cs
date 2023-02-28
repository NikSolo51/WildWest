using CodeBase.Infrastructure.Services;
using CodeBase.Services.Camera;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CodeBase.Logic
{
    public class Toggle : MonoBehaviour
    {
        [SerializeField] private Image buttonImage;
        private bool toggle;
        public UnityEvent ToggleOn;
        public UnityEvent ToggleOff;

        public void Switch()
        {
            toggle = !toggle;
            if (toggle)
            {
                AllServices.Container.Single<ICameraRaycast>().DisableRayCast();
                ToggleOn?.Invoke();
            }
            else
            {
                AllServices.Container.Single<ICameraRaycast>().EnableRayCast();
                ToggleOff?.Invoke();
            }
        }
    }
}
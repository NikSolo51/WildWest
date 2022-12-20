using UnityEngine;

namespace CodeBase.Services.Update
{
    public class UpdateManagerProvider : MonoBehaviour
    {
        private IUpdateService _updateService;

        public void Construct(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        private void Update()
        {
            if (_updateService != null)
                _updateService.Update();
        }

        private void FixedUpdate()
        {
            if (_updateService != null)
                _updateService.FixedUpdate();
        }

        private void LateUpdate()
        {
            if (_updateService != null)
                _updateService.LateUpdate();
        }
    }
}
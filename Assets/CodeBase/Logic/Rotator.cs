using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Update;
using UnityEngine;

namespace CodeBase.Logic
{
    public class Rotator : MonoBehaviour,IUpdatable
    {
        [SerializeField] private List<GameObject> _objects;
        [SerializeField,Range(-1,1)] private float _speed;
        private IUpdateService _updateService;

        private void OnEnable()
        {
            _updateService = AllServices.Container.Single<IUpdateService>();
            _updateService.Register(this);
        }

        private void OnDisable()
        {
            _updateService.Unregister(this);
        }

        public void UpdateTick()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].transform.RotateAround(Vector3.forward,1 * _speed);
            }
        }
    }
}
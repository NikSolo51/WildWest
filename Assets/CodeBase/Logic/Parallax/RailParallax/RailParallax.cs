using CodeBase.Infrastructure.Services;
using CodeBase.Services.Update;
using UnityEngine;

namespace CodeBase.Logic.Parallax.RailParallax
{
    public class RailParallax : MonoBehaviour,IUpdatable
    { 
        [SerializeField] private GameObject[] _rails;
        [SerializeField] private float _minXPos;
        [SerializeField] private float _maxXPos;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _speed;
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
            for (int i = 0; i < _rails.Length; i++)
            {
                if (_rails[i].transform.position.x <= _minXPos)
                {
                    _rails[i].transform.position = new Vector3(_maxXPos,_rails[i].transform.position.y,_rails[i].transform.position.z);
                }
                _rails[i].transform.position += _offset * _speed;
            }
        }
    }
}
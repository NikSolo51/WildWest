using System.Collections;
using CodeBase.Services.Update;
using UnityEngine;
using Zenject;

namespace CodeBase.CameraLogic
{
    public class CameraShake : MonoBehaviour,ILateUpdatable
    {
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _shakeTime = 8;
        private Vector3 _startPos;
        private bool _shake;
        private IUpdateService _updateService;
        
        [Inject]
        public void Construct(IUpdateService updateService)
        {
            _updateService = updateService;
            _updateService.Register(this);
        }


        private void OnDisable()
        {
            _updateService.Unregister(this);
        }

        public void LateUpdateTick()
        {
            Shake();
        }

        public void Shake()
        {
            if (!_shake)
            {
                _shake = true;
                _startPos = transform.position;
                StartCoroutine(Shaker());
            }
        }

        public void StopShake()
        {
            StopAllCoroutines();
            if (_startPos.y != 0)
                transform.position = new Vector3(transform.position.x, _startPos.y, transform.position.z);
        }

        IEnumerator Shaker()
        {
            float elapsedTime = 0;

            while (elapsedTime < _shakeTime)
            {
                elapsedTime += Time.deltaTime;
                float y = (_startPos.y + _curve.Evaluate(Mathf.PingPong(elapsedTime, _curve[_curve.length - 1].time)));
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
                yield return null;
            }

            transform.position = new Vector3(transform.position.x, _startPos.y, transform.position.z);
            _shake = false;
        }
    }
}
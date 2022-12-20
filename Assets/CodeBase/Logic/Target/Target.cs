using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.Logic.Target
{
    public class Target : MonoBehaviour
    {
        public float _activationDistance = 1;
        public int _reactivateTargetTime = 1;

        public event Action OnTargetReached;
        public UnityEvent OnTargetReachedUnityEvent;
        private bool reached;

        public void TargetReached()
        {
            reached = true;
            
            OnTargetReached?.Invoke();
            OnTargetReachedUnityEvent?.Invoke();

            if (gameObject.activeSelf)
            {
                ReactivateTarget();
            }
        }

        private void ReactivateTarget()
        {
            StartCoroutine(Timer());
        }

        public void TargetUnReached()
        {
            reached = false;
        }

        public bool IsTargetReached()
        {
            return reached;
        }

        IEnumerator Timer()
        {
            int currentTime = _reactivateTargetTime;

            while (currentTime > 0)
            {
                currentTime--;
                yield return new WaitForSeconds(1);
            }
            TargetUnReached();
        }
    }
}
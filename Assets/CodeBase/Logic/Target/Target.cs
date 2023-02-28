using System;
using System.Collections;
using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.Logic.Target
{
    [RequireComponent(typeof(UniqueId))]
    public class Target : MonoBehaviour, ISavedProgress
    {
        public float _activationDistance = 1;
        public int _reactivateTargetTime = 1;

        public event Action OnTargetReached;
        public UnityEvent OnTargetReachedUnityEvent;
        private bool reached;
        private bool deactivated;
        private string _id;
        private ISaveLoadService _saveLoadService;

        private void Start()
        {
            _id = GetComponent<UniqueId>().Id;
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _saveLoadService.Register(this);
        }

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

        public void DeactivateTarget()
        {
            deactivated = true;
            gameObject.SetActive(false);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.WorldData.Targets.deactivatedTarget.Contains(_id))
            {
                DeactivateTarget();
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (deactivated)
            {
                if (!progress.WorldData.Targets.deactivatedTarget.Contains(_id))
                {
                    progress.WorldData.Targets.deactivatedTarget.Add(_id);
                }
            }
        }
    }
}
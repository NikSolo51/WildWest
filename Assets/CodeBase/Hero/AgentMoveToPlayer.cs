using CodeBase.Infrastructure.Services;
using CodeBase.Services.Update;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Hero
{
    public class AgentMoveToPlayer : Follow,IUpdatable
    {
        public NavMeshAgent Agent;
        private Transform _herTransform;
        private IUpdateService _updateService;

        public void Construct(Transform heroTransform)
        {
            _herTransform = heroTransform;
        }
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
            if (_herTransform && Agent.enabled)
                Agent.destination = _herTransform.position;
        }
    }
}
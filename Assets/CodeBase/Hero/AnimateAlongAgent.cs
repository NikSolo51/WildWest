using CodeBase.Infrastructure.Services;
using CodeBase.Services.Update;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(HeroAnimator))]
    public class AnimateAlongAgent : MonoBehaviour,IUpdatable
    {
        private const float MinimalVelocity = 0.1f;

        [SerializeField] private NavMeshAgent Agent;
        [SerializeField] private HeroAnimator Animator;
        private IUpdateService _updateService;

        public void Constructor(IUpdateService updateService)
        {
            _updateService = updateService;
            _updateService.Register(this);
        }

        private void OnDisable()
        {
            _updateService.Unregister(this);
        }
        
        public void UpdateTick()
        {
            if (!Agent || !Animator)
                return;

            if (ShouldMove())
            {
                Animator.Move(Agent.velocity.magnitude);
            }
            else
            {
                Animator.StopMoving(Agent.velocity.magnitude);
            }
        }

        private bool ShouldMove()
        {
            return Agent.velocity.magnitude > MinimalVelocity;
        }
    }
}
using CodeBase.Hero;

namespace CodeBase.Logic.Target
{
    public class PlayerCrossedDistanceTrigger : PlayerCrossedDistance
    {
        public TriggerObserver triggerObserver;
        private bool _isCrossedDistance;

        private void Start()
        {
            triggerObserver.TriggerEnter += CrossedDistance;
            triggerObserver.TriggerExit += OutOfDistance;
        }

        public override void CrossedDistance()
        {
            _isCrossedDistance = true;
        }

        public override void OutOfDistance()
        {
            _isCrossedDistance = false;
        }

        public override bool IsCrossedDistance()
        {
            return _isCrossedDistance;
        }
    }
}
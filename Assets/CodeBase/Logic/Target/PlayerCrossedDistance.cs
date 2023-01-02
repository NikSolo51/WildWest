using UnityEngine;

namespace CodeBase.Logic.Target
{
    public abstract class PlayerCrossedDistance : MonoBehaviour
    {
        public abstract void CrossedDistance();
        public abstract void OutOfDistance();

        public abstract bool IsCrossedDistance();
    }
}
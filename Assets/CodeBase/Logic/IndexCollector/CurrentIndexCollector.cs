using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Logic.IndexCollector
{
    public class CurrentIndexCollector : MonoBehaviour, ICurrentIndexCollector
    {
        public List<ContainCurrentIndex> CurrentIndicies;

        public List<ContainCurrentIndex> GetCurrentIndicies()
        {
            return CurrentIndicies;
        }
    }
}
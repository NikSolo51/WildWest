using System.Collections.Generic;

namespace CodeBase.Logic.IndexCollector
{
    public interface ICurrentIndexCollector
    {
        public List<ContainCurrentIndex> GetCurrentIndicies();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticOptimizer
{
    public interface ICell
    {
        float GetRank(ref IData data);

        ICell NewCell();

        ICell NewCell(ICell cell);
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    /// <summary>
    /// Содержит в себе ключ-значение пары, в которых
    /// ключ - это входные данные
    /// значение - это требуемый вывод
    /// </summary>
    public interface IDataEnumerator : IEnumerator<KeyValuePair<double[,,],double[,,]>>, IEnumerable<KeyValuePair<double[,,], double[,,]>>
    {
        KeyValuePair<double[,,], double[,,]> GetRandom();

        double[,,] Process(Bitmap input);
    }

    
}

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
    public interface IDataEnumerator : IEnumerator<IOPair>, IEnumerable<IOPair>
    {
        IOPair GetRandom(ref Network network);

        /// <summary>
        /// Возращает данные по изображению
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        double[,,] Process(Bitmap input);
    }

    
}

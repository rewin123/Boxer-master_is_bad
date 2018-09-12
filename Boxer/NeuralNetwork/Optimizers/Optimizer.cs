using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public abstract class Optimizer
    {
        public Network network;

        public Optimizer(Network network)
        {
            this.network = network;
        }

        /// <summary>
        /// Подготавливает к работе отимизатор
        /// </summary>
        /// <param name="batch"></param>
        public abstract void Init(int batch);
        
        /// <summary>
        /// Начинает первыю эпоху обучения
        /// </summary>
        /// <param name="data">Входные данные</param>
        /// <param name="batch">Оазмер разовой обучающей сборки</param>
        /// <param name="count">Кол-во обучающих сборок в этой жпохе</param>
        /// <returns>Ошиблку на каждом батче</returns>
        public virtual double[] TrainBatch(IDataEnumerator data, int batch, int count)
        {
            throw new NotImplementedException("Вызван абстрактный класс");
        }
        

        public virtual Optimizer CloneMe(Network network)
        {
            throw new NotImplementedException("Вызван абстрактный класс");
        }

        public virtual double[] TrainBatchContinue(IDataEnumerator data, int batch, int count)
        {
            throw new NotImplementedException("Вызван абстрактный класс");
        }
    }
}

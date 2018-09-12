using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    /// <summary>
    /// Слои работают с 3-х мерными матрицами (chanel, y, x) 
    /// </summary>
    [Serializable]
    public abstract class Layer
    {
        [NonSerialized]
        public Activation func;

        public Size input_size;
        public Size output_size;

        public Layer(Activation activation)
        {
            func = activation;
        }

        public Layer(LayerData data)
        {
            input_size = data.input_size;
            output_size = data.output_size;
        }

        public virtual double[,,] GetOutput(double[,,] input)
        {
            throw new NotImplementedException("Вызван абстрактный класс");
        }

        public virtual Size Compile(Size input_size)
        {
            return input_size;
        }
        
        /// <summary>
        /// Расчитывает ошибку на входе слоя
        /// </summary>
        /// <param name="error_out">Ошибка на выходе слоя</param>
        /// <param name="diff">производная входа</param>
        /// <param name="input">значения входных параметров</param>
        /// <returns></returns>
        public virtual void GetError(double[,,] error_out, double[,,] diff, double[,,] input, double[,,] result)
        {
            throw new NotImplementedException("Вызван абстрактный класс");
        }

        public double[,,] GetError(double[,,] error_out, double[,,] diff, double[,,] input)
        {
            var res = CreateInpuut();
            GetError(error_out, diff, input, res);
            return res;
        }

        public virtual double[,,] GetCachedDiff()
        {
            throw new NotImplementedException("Вызван абстрактный класс");
        }

        public virtual double[,,] GetCachedOutpur()
        {
            throw new NotImplementedException("Вызван абстрактный класс");
        }

        public virtual void ChangeWeights(double[,,] error_out, double[,,] input, double k)
        {
            throw new NotImplementedException("Вызван абстрактный класс");
        }

        public virtual void CreateGradients(int count)
        {
            
        }

        /// <summary>
        /// Записываем градиент в позицию pos
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="error_out"></param>
        /// <param name="input"></param>
        /// <param name="lambda">Параметр L2 регуляризации</param>
        public virtual void WriteG(int pos, double[,,] error_out, double[,,] input, double lambda = 0.01)
        {
            
        }

        public virtual void ActionG(int pos, int to, Func<double,double> func)
        {
            
        }

        public virtual void ActionTwoG(int pos1, int pos2, int to, Func<double,double,double> func)
        {
           
        }

        public virtual void GradWeights(int pos)
        {
            
        }

        public virtual double NormG(int pos)
        {
            return 0;
        }

        public virtual void AddNoise(double width)
        {

        }

        public virtual void NormalizationW()
        {

        }

        public virtual Layer CloneMe()
        {
            throw new NotImplementedException("Я не существую");
        }

        /// <summary>
        /// Возращает выход и производную выхода от функции активации
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="diff"></param>
        public virtual void GetOutputAndDiff(double[,,] input, double[,,] output, double[,,] diff)
        {
            throw new NotImplementedException("Вызван абстрактный класс");
        }

        public IOPair GetOutputAndDiff(double[,,] input)
        {
            var output = CreateOutput();
            var diff = CreateOutput();
            GetOutputAndDiff(input, output, diff);
            return new IOPair(output, diff);
        }

        public virtual bool ITrained
        {
            get
            {
                throw new NotImplementedException("Вызван абстрактный класс Layer");
            }
        }

        public virtual Layer Mirror
        {
            get
            {
                throw new NotImplementedException("Вызван абстрактный класс Layer");
            }
        }

        public virtual LayerData SaveJSON()
        {
            LayerData data = new LayerData();
            data.className = GetType().Name;
            data.activationClass = func.GetType().Name;
            data.activationData = JsonConvert.SerializeObject(func);
            data.input_size = input_size;
            data.output_size = output_size;
            return data;
        }

        public static Layer CreateFromJSON(LayerData data)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes()
                .First(t => t.Name == data.className);

            var act_type = assembly.GetTypes()
                .First(t => t.Name == data.activationClass);


            Layer l = (Layer)Activator.CreateInstance(type, data); //(Layer)Activator.CreateInstance(type, data);
            l.func = (Activation)JsonConvert.DeserializeObject(data.activationData, act_type);
            //l.func = (Activation)Activator.CreateInstance(act_type);

            return l;
        }

        public double[,,] CreateInpuut()
        {
            return doubleArrayExtensions.CreateArray(input_size);
        }

        public double[,,] CreateOutput()
        {
            return doubleArrayExtensions.CreateArray(output_size);
        }

        

        protected static object MagicallyCreateInstance(string className)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var type = assembly.GetTypes()
                .First(t => t.Name == className);

            return Activator.CreateInstance(type);
        }
    }

}

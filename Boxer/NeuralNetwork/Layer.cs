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
    /// Слои работают с 3-х мерными матрицами (channel, y, x) 
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

        public virtual double[,,] GetOutput(ref double[,,] input)
        {
            throw new NotImplementedException("Вызван абстрактный класс");
        }

        /// <summary>
        /// Расчет выхода через небезопасный код
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //public unsafe virtual double* UnsafeOutput(double* input)
        //{
        //    throw new NotImplementedException("Вызван абстрактный класс Network");
        //}

        public virtual Size Compile(Size input_size)
        {
            return input_size;
        }
        

        public virtual double[,,] GetError(double[,,] error_out, double[,,] diff, double[,,] input)
        {
            throw new NotImplementedException("Вызван абстрактный класс");
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

        public virtual void WriteG(int pos, double[,,] error_out, double[,,] input, double k)
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

        public virtual KeyValuePair<double[,,],double[,,]> GetOutputAndDiff(double[,,] input)
        {
            throw new NotImplementedException("Вызван абстрактный класс");
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
            l.func = (Activation)Activator.CreateInstance(act_type);

            return l;
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

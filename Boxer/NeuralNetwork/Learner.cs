using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Callbacks;
using NeuralNetwork.Metrics;

namespace NeuralNetwork
{
    public class Learner
    {
        public IDataEnumerator train_data = null;
        public IDataEnumerator val_data = null;
        public List<ICallback> callbacks = new List<ICallback>();
        public List<IMetric> metrics = new List<IMetric>();
        public Dictionary<string, double> metrics_values = new Dictionary<string, double>();

        public Optimizer optimizer = null;

        public void Learn(int batch,int steps_per_epochs,int epochs = 1)
        {
            optimizer.Init(batch);
            for(int ep = 0;ep < epochs;ep++)
            {
                var err = optimizer.TrainBatchContinue(train_data, batch, steps_per_epochs);
                double err_mean = 0;
                for(int i = 0;i < err.Length;i++)
                {
                    err_mean += err[i];
                }
                err_mean /= err.Length;
                Console.WriteLine("Epoch error: {0}", err_mean);
                metrics_values["err"] = err_mean;
                double[] m_vals = optimizer.network.GetMeanMetrics(train_data, metrics);
                for (int i = 0;i < metrics.Count;i++)
                {
                    Console.WriteLine("Epoch {0}: {1}", metrics[i].Name, m_vals[i]);

                    metrics_values[metrics[i].Name] = m_vals[i];
                }

                if(val_data != null)
                {
                    var val_err = optimizer.network.GetError(val_data);
                    Console.WriteLine("Validation error: {0}", val_err.Key);
                    metrics_values["val_err"] = val_err.Key;
                    m_vals = optimizer.network.GetMeanMetrics(val_data, metrics);
                    for (int i = 0; i < metrics.Count; i++)
                    {
                        Console.WriteLine("Validation {0}: {1}", metrics[i].Name, m_vals[i]);
                        metrics_values["val_" + metrics[i].Name] = m_vals[i];
                    }
                }

                for (int i = 0; i < callbacks.Count; i++)
                    callbacks[i].Action(this);
            }
        }
    }
}

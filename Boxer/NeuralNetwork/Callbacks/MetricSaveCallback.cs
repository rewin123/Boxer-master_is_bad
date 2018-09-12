using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Callbacks
{
    /// <summary>
    /// Сохранякт нейроннуб сеть при уменьшении значения метрики
    /// </summary>
    public class MetricSaveCallback : ICallback
    {
        public string metric = "";
        double metric_val = double.MaxValue;
        public MetricSaveCallback(string metric)
        {
            this.metric = metric;
        }
        public void Action(Learner learner)
        {
            double metric_now = double.MaxValue;
            try
            {
                metric_now = learner.metrics_values[metric];
            }
            catch
            {
                Console.WriteLine("Не могу прочитать метрику {0}", metric);
                return;
            }

            if(metric_val > metric_now)
            {
                metric_val = metric_now;
                File.WriteAllText(metric_val + ".neural", learner.optimizer.network.SaveJSON());
                Console.WriteLine("нейросеть созранена в {0}", metric_val + ".neural");
            }
        }
    }
}

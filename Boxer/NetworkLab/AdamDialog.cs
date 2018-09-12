using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NeuralNetwork;

namespace NetworkLab
{
    public partial class AdamDialog : Form
    {
        public Optimizer opt;
        public AdamDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double k = double.Parse( textBox1.Text);
            double a = double.Parse(textBox1.Text);
            double ro = double.Parse(textBox1.Text);

            opt = new AdamParallel(NetworkData.network, k, a, ro);

            DialogResult = DialogResult.OK;
            Close();
        }

        
    }
}

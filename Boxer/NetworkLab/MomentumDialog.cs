using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkLab
{
    public partial class MomentumDialog : Form
    {
        public Optimizer opt;
        public MomentumDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double k = double.Parse(textBox1.Text);
            double a = double.Parse(textBox2.Text);

            opt = new MomentumParallel(NetworkData.network, a, k);

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

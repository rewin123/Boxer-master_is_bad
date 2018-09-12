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
    public partial class Size_select : Form
    {
        public Size_select()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int w = int.Parse(textBox1.Text);
            int h = int.Parse(textBox2.Text);

            NetworkData.image_size = new Size(w, h);

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

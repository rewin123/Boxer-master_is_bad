using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using BoxerDLL;

namespace Boxer
{
    public partial class SortForm : Form
    {
        string path = "Sorted";
        BitmapEnumerator bits;
        public SortForm()
        {
            InitializeComponent();
            

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Directory.CreateDirectory(path + "\\1");
                Directory.CreateDirectory(path + "\\2");
            }
        }

       
        

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bits = new BitmapEnumerator(openFileDialog1.FileNames);

                pictureBox1.Image = bits.Current;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bits.Current.Save(path + "\\1\\" + bits.SafeName);
            if (!bits.MoveNext())
                Close();
            pictureBox1.Image = bits.Current;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bits.Current.Save(path + "\\2\\" + bits.SafeName);
            if (!bits.MoveNext())
                Close();
            pictureBox1.Image = bits.Current;
        }
    }
}

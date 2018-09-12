using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BoxerDLL;
using System.IO;

namespace Boxer
{
    public partial class SetupBoxCut : Form
    {
        BoxCut2D cutter;
        Bitmap input;
        public SetupBoxCut()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                input = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = input;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cutter = new BoxCut2D(int.Parse(textBox1.Text),
                int.Parse(textBox2.Text),
                float.Parse(textBox3.Text),
                float.Parse(textBox4.Text),
                float.Parse(textBox5.Text),
                float.Parse(textBox6.Text),
                float.Parse(textBox7.Text),
                float.Parse(textBox8.Text));

            pictureBox1.Image = cutter.DrawRectangles(input, new Pen(Color.Green,20));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                Pen p = new Pen(Color.Green, 10);
                string path = "Output";
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                for(int i = 0; i < openFileDialog1.FileNames.Length;i++)
                {
                    Text = i + " from " + openFileDialog1.FileNames.Length;
                    Bitmap map = new Bitmap(openFileDialog1.FileNames[i]);
                    pictureBox1.Image = map;
                    Update();
                    CutterEnumerator bitmaps = cutter.Enumerator(ref map);

                    int index = 0;
                    string name = openFileDialog1.SafeFileNames[i].Split('.')[0];
                    do
                    {
                        Bitmap output = bitmaps.Current;
                        output.Save(path + "/" + name + "_" + index + ".png");
                        output.Dispose();
                        index++;
                    } while (bitmaps.MoveNext());

                    map.Dispose();
                }
            }
        }
    }
}

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

namespace Matrix
{
    public partial class Form1 : Form
    {
        Bitmap image;
        public Form1()
        {
            InitializeComponent();
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                Close();

            image = new Bitmap(openFileDialog1.FileName);

            var form_image = new Bitmap("form_agg.png");
            double[,] matrix = form_image.GetGrey(0, 0, form_image.Width, form_image.Height, 0, form_image.Width);
            

            int m_width = matrix.GetLength(0);
            int m_height = matrix.GetLength(1);

            double sum = 0;
            for(int x = 0;x < m_width;x++)
            {
                for(int y = 0;y < m_height;y++)
                {
                    matrix[x, y] = matrix[x, y] < 0.5 ? -1 : 1;
                    sum += matrix[x, y];
                }
            }

            sum /= m_width * m_height;
            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
                {
                    matrix[x, y] -= sum;
                }
            }

            Vector3[,] map = image.GetRGB();
            double[,] result = new double[image.Width, image.Height];

            int max_width = image.Width - matrix.GetLength(0);
            int max_height = image.Height - matrix.GetLength(1);

            for(int x = 0;x < max_width;x++)
            {
                for(int y = 0;y < max_height;y++)
                {
                    double val = 0;
                    for(int i = 0;i < m_width;i++)
                    {
                        for(int j = 0;j < m_height;j++)
                        {
                            val += map[x + i, y + j].x * matrix[i, j];
                        }
                    }

                    result[x, y] = val > 0 ? val : 0;
                }
            }

            result.RegMaximum();
            pictureBox1.Image = result.GetImage();
            Focus();
        }
    }
}

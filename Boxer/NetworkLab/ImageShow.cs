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
    public partial class ImageShow : Form
    {
        public ImageShow(Bitmap img)
        {
            InitializeComponent();

            pictureBox1.Image = img;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locket
{
    public partial class ImageViewer : Form
    {
        static ImageViewer viewer;


        private ImageViewer()
        {
            InitializeComponent();
        }

        public ImageViewer(Image image)
        {
            InitializeComponent();

            pictureBox1.Image = image;
        }

        private static void Show(Image image, string title)
        {
            if (viewer == null) viewer = new ImageViewer();
            viewer.pictureBox1.Image = image;
            viewer.Text = title;
            viewer.Show();
        }

        private void ImageViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            viewer = null;
        }
    }
}

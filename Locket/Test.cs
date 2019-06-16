using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locket
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += timer_Tick;
            timer.Start();
        }
        Timer timer;
        int value = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = ++value;
            label1.Text = value + "%";
            if(value ==100)timer.Stop();
        }

    }
}




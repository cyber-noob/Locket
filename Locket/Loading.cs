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
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
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
            lblPercent.Text = value + "%";
            if (value == 100)
            {
                timer.Stop();
                this.Close();
            }
        }
    }
}

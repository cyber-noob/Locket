using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locket
{
    public partial class MainForm : Form
    {
        #region Property
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private static MainForm _mainFormAccess;

        public static MainForm MainFormAccess
        {
            get { return MainForm._mainFormAccess; }
        }

        public Panel MainPanelAccess
        {
            get { return this.pnlBody; }
        }

        #endregion

        #region Constructor
        public MainForm()
        {
            InitializeComponent();
            Initialize();
        }
        #endregion

        #region Method
        private void Initialize()
        {
            _mainFormAccess = this;
        }


        private void Select(Button btn)
        {
            foreach (Button _btn in this.pnlMenu.Controls)
            {
                _btn.BackColor = Color.Gray;
            }

            btn.BackColor = Color.FromArgb(50, 50, 50);
        }



        #endregion

        #region Event
        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            this.WindowState = this.WindowState == FormWindowState.Maximized ? this.WindowState = FormWindowState.Normal : FormWindowState.Maximized;
        }

        #endregion

        public void AddControl(UserControl value)
        {
            if (pnlBody.Controls.ContainsKey(value.Name)) return;

            foreach (Control ctrl in pnlBody.Controls)
            {
                ctrl.Visible = false;

                switch (ctrl.Name)
                {
                    case "Videos":
                        pnlBody.Controls.RemoveByKey("Videos");
                        break;

                    case "Images":
                        pnlBody.Controls.RemoveByKey("Images");
                        break;

                    case "Files":
                        pnlBody.Controls.RemoveByKey("Files");
                        break;
                }
            }

            value.Dock = DockStyle.Fill;
            this.pnlBody.Controls.Add(value);
        }

        private void btnVideo_Click(object sender, EventArgs e)
        {
            AddControl(new Videos());
            Select(btnVideo);
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            AddControl(new Images());
            Select(btnImage);
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            AddControl(new Files());
            Select(btnFile);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in this.pnlBody.Controls)
            {
                switch (ctrl.Name)
                {
                    case "Videos":
                        pnlBody.Controls.RemoveByKey("Videos");
                        break;

                    case "Images":
                        pnlBody.Controls.RemoveByKey("Images");
                        break;

                    case "Files":
                        pnlBody.Controls.RemoveByKey("Files");
                        break;
                    default:
                        ctrl.Visible = true;
                        break;
                }

            }

            Select(btnHome);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(SystemData.PATH)) Directory.CreateDirectory(SystemData.PATH);

            Login login = new Login();
            login.ShowDialog(this);

            SystemData.RetrieveFile();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DirectoryInfo info = new DirectoryInfo(SystemData.TEMP);
            foreach (var file in info.GetFiles())
            {
                File.Delete(file.FullName);
            }
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            account.ShowDialog(this);
        }

    }
}

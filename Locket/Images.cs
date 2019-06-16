using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Locket
{
    public partial class Images : UserControl
    {
        #region Property
        #endregion

        #region Constructor
        public Images()
        {
            InitializeComponent();
            Initialize();
        }
        #endregion

        #region Method
        private void Initialize()
        {
            LoadData();
        }

        private void LoadData()
        {
            dataGridView1.Rows.Clear();

            DirectoryInfo info = new DirectoryInfo(SystemData.IMAGE_PATH);
            FileInfo[] filesInfo = info.GetFiles();

            Encryption encrypt = new Encryption();
            foreach (FileInfo _fileInfo in filesInfo)
            {

                string key = _fileInfo.Name;
                string path = _fileInfo.FullName;
                DateTime date = _fileInfo.CreationTime;
                decimal size = Math.Round((decimal)_fileInfo.Length / (decimal)Math.Pow(10, 6), 1);
                string realName = SystemData.FILES[key];

                dataGridView1.Rows.Add(key, realName, size + "MB", date.ToString("d/M/yy h:m tt"));
            }
        }

        private static Image ConvertByteArrayToImage(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            if (data.Length == 0) return null;
            Image img = Image.FromStream(ms);
            ms.Close();
            return img;
        }
        #endregion

        #region Event

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dia = new OpenFileDialog();
            dia.Filter = "png|*.png|jpg|*.jpg|bmp|*.bmp|gif|*.gif";
            dia.Multiselect = true;

            if (dia.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = dia.FileNames;

                Encryption obj = new Encryption();
                foreach (string file in files)
                {
                    obj.Encrypt(file, SystemData.IMAGE_PATH);
                    if (chbDelete.Checked) File.Delete(file);
                }
                SystemData.SaveFile();
                LoadData();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            if (e.ColumnIndex == btnView.Index)
            {
                Encryption encrypt = new Encryption();

                string file = dataGridView1.CurrentRow.Cells[colkey.Index].Value.ToString();
                string realName = SystemData.FILES[file];

                if (!File.Exists(SystemData.TEMP + "\\" + realName))
                {
                    encrypt.Decrypt(file, SystemData.IMAGE_PATH, SystemData.TEMP);
                }

                Image image = (Image)Image.FromFile(SystemData.TEMP + "\\" + realName);

                ImageViewer imageViewer = new ImageViewer(image);
                imageViewer.Text = realName;
                imageViewer.ShowDialog(this);
                image.Dispose();
            }
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0) return;

            var selected = dataGridView1.SelectedRows;

            FolderBrowserDialog dia = new FolderBrowserDialog();
            if (dia.ShowDialog(this) == DialogResult.OK)
            {
                Encryption encrypt = new Encryption();
                foreach (DataGridViewRow _row in selected)
                {
                    string file = _row.Cells[colkey.Index].Value.ToString();
                    encrypt.Decrypt(file, SystemData.IMAGE_PATH, dia.SelectedPath);
                    File.Delete(SystemData.IMAGE_PATH + "\\" + file + SystemData.EXTENSION);
                }
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0) return;
            if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            var selected = dataGridView1.SelectedRows;

            Encryption encrypt = new Encryption();
            foreach (DataGridViewRow _row in selected)
            {
                string file = _row.Cells[colkey.Index].Value.ToString();
                File.Delete(SystemData.IMAGE_PATH + "\\" + file + SystemData.EXTENSION);

            }
            LoadData();
        }
        #endregion


    }
}

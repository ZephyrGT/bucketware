using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace Bucketware.Layouts
{
    public partial class Launcher : Form
    {
        private string path = @"C:\Users\"+Environment.UserName+@"\AppData\Local\Growtopia\Growtopia.exe";
        private string dir = @"C:\Users\" + Environment.UserName + @"\AppData\Local\Growtopia";
        public Launcher()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            
            this.BringToFront();
            try
            {
                //Taskill growtopia if already running...
                Process[] procs = Process.GetProcessesByName("Growtopia");
                foreach (Process p in procs) { p.Kill(); }
            }
            catch
            {
                //Growtopia is not running do nothing
            }
            label3.Text = "GT Location " + path;
            try
            {
                if (!File.Exists("items.dat"))
                {
                    string fileToCopy = dir + @"\cache\items.dat";
                    string destinationDirectory = "";
                    File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy));
                }
                if (!File.Exists("CoreData.txt"))
                {
                    Process.Start("Itemsdecoder.exe");
                }
            }
            catch
            {
                MessageBox.Show("Make sure that Itemsdecoder.exe is in same directery as Bucketware.exe");
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Process.Start(path);
            MainForm mf = new MainForm();
            mf.Show();
            mf.BringToFront();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (!Directory.Exists(dir))
            {
                openFileDialog1.InitialDirectory = "c:\\";
            }
            else
            {
                openFileDialog1.InitialDirectory = dir;
            }
            openFileDialog1.Filter = "Exe files (*.exe)|*.exe";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;
                path = selectedFileName;
                label3.Text = selectedFileName;
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bucketware.Natives;
using GrowbrewProxy;

//This project isnt ready
//You can fork this project but Dont just change this name and publish it as your own

namespace Bucketware.Layouts
{
    public partial class MainForm : Form
    {
        public static bool toggleAutofarmerBool = false;
        public MainForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            BWare.mini = !BWare.mini;
            BWare.minimize(this, 28, 364);
            if (BWare.mini is true)
            {
                this.Opacity = 98;
            }
            else
            {
                this.Opacity = 60;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            guna2Button1.Checked = false;
            guna2Button2.Checked = false;
            guna2Button3.Checked = true;
            guna2Button4.Checked = false;
            guna2Button5.Checked = false;
            guna2Button6.Checked = false;
        }

        private void guna2HScrollBar1_Scroll_1(object sender, ScrollEventArgs e)
        {
            label8.Text = "Pickup range - " + guna2HScrollBar1.Value.ToString();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            guna2Button1.Checked = false;
            guna2Button2.Checked = false;
            guna2Button3.Checked = false;
            guna2Button4.Checked = true;
            guna2Button5.Checked = false;
            guna2Button6.Checked = false;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
            guna2Button1.Checked = false;
            guna2Button2.Checked = false;
            guna2Button3.Checked = false;
            guna2Button4.Checked = false;
            guna2Button5.Checked = true;
            guna2Button6.Checked = false;
            CheckForIllegalCrossThreadCalls = false;
        }
        private void guna2CustomCheckBox5_MouseHover(object sender, EventArgs e)
        {
            BWare.tip(guna2CustomCheckBox5, "Start/Stop Spam");
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Opacity = 98;
            CheckForIllegalCrossThreadCalls = false;
            imports.GetWindowRect(imports.handle, out imports.rect);
            this.Left = imports.rect.left;
            this.Top = imports.rect.top;
            backgroundWorker1.RunWorkerAsync();
            guna2ComboBox1.StartIndex = 1;
            guna2ComboBox2.StartIndex = 1;
            guna2ComboBox3.StartIndex = 0;
            Proxyhelper.loadproxy();
            try
            {
                label45.Text = BWare.dcusername;
                guna2CirclePictureBox2.Load(BWare.dcavatar);
                int leg = label45.Text.Length;
                int x = label45.Location.X;
                int o = x - 7;
                int f = o - leg;
                label45.Location = new Point(f, 311);
            }
            catch
            {

            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            guna2Button1.Checked = true;
            guna2Button2.Checked = false;
            guna2Button3.Checked = false;
            guna2Button4.Checked = false;
            guna2Button5.Checked = false;
            guna2Button6.Checked = false;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            guna2Button1.Checked = false;
            guna2Button2.Checked = true;
            guna2Button3.Checked = false;
            guna2Button4.Checked = false;
            guna2Button5.Checked = false;
            guna2Button6.Checked = false;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                CheckForIllegalCrossThreadCalls = false;
                imports.GetWindowRect(imports.handle, out imports.rect);
                this.Left = imports.rect.left;
                this.Top = imports.rect.top;
                Thread.Sleep(BWare.inter);
            }
        }

        private void guna2CustomCheckBox6_MouseHover(object sender, EventArgs e)
        {
            BWare.tip(guna2CustomCheckBox6, "This disables all what have big chance to give autoban, its good for safe autofarming");
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            try
            {
                Process[] procs = Process.GetProcessesByName("Growtopia");
                foreach (Process p in procs) { p.Kill(); }
            }
            catch
            {

            }
            Application.Exit();
        }

        private void guna2CustomCheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void guna2CustomCheckBox34_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox34.Checked is true)
            {
                BWare.inter = 5;
            }
            else
            {
                BWare.inter = 0;
            }
        }
        private void guna2CustomCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void guna2CustomCheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void guna2CustomCheckBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ipaddrBox.Text != "" && portBox.Text != "")
            {
                Proxyhelper.globalUserData.Growtopia_IP = ipaddrBox.Text;
                Proxyhelper.globalUserData.Growtopia_Port = Convert.ToInt32(portBox.Text);
            }
            Proxyhelper.isHTTPRunning = !Proxyhelper.isHTTPRunning;
            if (Proxyhelper.isHTTPRunning)
            {
                string[] arr = new string[1];
                arr[0] = "http://*:80/";
                HTTPServer.StartHTTP(this, arr);
            }
            else
            {
                HTTPServer.StopHTTP();
            }
            Proxyhelper.LaunchProxy();
        }

        private void ipaddrBox_TextChanged(object sender, EventArgs e)
        {
            Proxyhelper.globalUserData.Growtopia_IP = ipaddrBox.Text;
        }

        private void portBox_TextChanged(object sender, EventArgs e)
        {
            Proxyhelper.globalUserData.Growtopia_Port = Convert.ToInt32(portBox.Text);
        }
    }
}

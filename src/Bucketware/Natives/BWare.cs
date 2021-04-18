using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guna.UI2;
using System.Windows.Forms;
using System.Drawing;
using Bucketware.Layouts;

namespace Bucketware.Natives
{
    class BWare
    {
        public static void tip(Control control, string text)
        {
            Guna.UI2.WinForms.Guna2HtmlToolTip toolTip1 = new Guna.UI2.WinForms.Guna2HtmlToolTip();
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            toolTip1.BackColor = Color.FromArgb(59, 66, 82);
            toolTip1.BorderColor = Color.FromArgb(59, 66, 82);
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(control, text);
        }
        public static bool mini = false;
        public static void minimize(Control control, int h, int f)
        {
            if (mini is false)
            {
                control.Height = h;
            }
            else
            {
                control.Height = f;
            }
        }


        //public static MainForm mf;
        public static int inter;
        public const string WINDOW_NAME = "Growtopia";

        /*public static bool CheckForIllegalCrossThreadCalls { get; private set; }

        public static void setWin()
        {
            CheckForIllegalCrossThreadCalls = false;
            imports.GetWindowRect(imports.handle, out imports.rect);
            mf.Left = imports.rect.left;
            mf.Top = imports.rect.top;
        }
        */
        
    }
}

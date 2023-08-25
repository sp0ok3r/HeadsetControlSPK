using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace HeadsetControlSPK
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        string PROGRAMname = "HeadSetControl SP0OK3R";
        Process process = new Process();

        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();

        private void HeadsetStatus()
        {
            process.StartInfo.FileName = "headsetcontrol-windows.exe";
            process.StartInfo.Arguments = " -b";

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            process.Start();
            string output = process.StandardOutput.ReadToEnd();


            string result = output.Split('\n')[1];
            string bat = Regex.Match(result, @"\d+").Value.ToString();

            int a = int.Parse(bat);
            if ((50 <= a && a <= 100))
            {
                this.notifyIcon1.Icon = Properties.Resources.ssfull;
            }
            else if (20 <= a && a <= 49)
            {
                this.notifyIcon1.Icon = Properties.Resources.ssmid;
            }
            else
            {
                this.notifyIcon1.Icon = Properties.Resources.ssred;
            }
            this.notifyIcon1.Text = PROGRAMname + "\n" + bat + " %";
            process.WaitForExit();

        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            Hide();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            HeadsetStatus();
            Hide();
            t.Interval = 3600000;
            t.Tick += new EventHandler(timer_Tick);
            t.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            HeadsetStatus();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void forceCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HeadsetStatus();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void autoStartToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (autoStartToolStripMenuItem.Checked)
            {
                registryKey.SetValue(PROGRAMname, Application.ExecutablePath);
            }
            else
            {
                registryKey.DeleteValue(PROGRAMname);
            }
        }
    }
}

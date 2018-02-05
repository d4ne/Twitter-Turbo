using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TurboTwitter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bunifuFlatButton1.selected = true;
            single singleUi = new single();
            content.Controls.Add(singleUi);
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            bunifuFlatButton1.selected = true;
            bunifuFlatButton2.selected = false;
            content.Controls.Clear();
            single singleUi = new single();
            content.Controls.Add(singleUi);
        }

        private void bunifuFlatButton2_Click_1(object sender, EventArgs e)
        {
            bunifuFlatButton1.selected = false;
            bunifuFlatButton2.selected = true;
            content.Controls.Clear();
            multi multiUi = new multi();
            content.Controls.Add(multiUi);
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            string currentVersion = "3";
            string htmlCode;

            using (WebClient client = new WebClient())
            {
                htmlCode = client.DownloadString("http://pr0b.com/twitter/currentVersion.php");
            }

            if (currentVersion != htmlCode)
            {
                System.Diagnostics.Process.Start("http://pr0b.com/twitter/download.php");
            }
            else
            {
                MessageBox.Show("Your running the current version.", "Twitter Turbo Info");
            }
        }
    }
}
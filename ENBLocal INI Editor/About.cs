using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace enboost {
    public partial class About:Form {
        public About () {
            InitializeComponent();
        }

        private void About_Load (object sender, EventArgs e) {
            linkLabel1.Links.Add(0, linkLabel1.Text.Length,"http://wiki.step-project.com/Guide:ENBlocal_INI");
            linkLabel2.Links.Add(0, linkLabel1.Text.Length, "http://enbdev.com/download.htm");

            linkLabel1.Parent = pictureBox1;
            linkLabel1.BackColor = Color.Transparent;
            linkLabel2.Parent = pictureBox1;
            linkLabel2.BackColor = Color.Transparent;
            foreach (Control control in Controls) {
                Label label = control as Label;
                if (label != null) {
                    label.Parent = pictureBox1;
                    label.BackColor = Color.Transparent;
                }
            }
        }

        private void linkLabel1_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void linkLabel2_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void button1_Click (object sender, EventArgs e) {
            Close();
        }
    }
}

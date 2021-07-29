using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUPPI
{
    public partial class remoteClient : Form
    {
        public string ips = "";
        public string prts = "";
        public string pps = "";
        public string tms = "";
        public remoteClient()
        {
            InitializeComponent();
        }

        private void lip_Click(object sender, EventArgs e)
        {

        }

        private void okbutton_Click(object sender, EventArgs e)
        {
            ips = iptxt.Text;
            prts = portxt.Text;
            pps = passtxt.Text;
            tms = timeoutText.Text;  
            this.Close(); 
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void remoteClient_Load(object sender, EventArgs e)
        {
            iptxt.Text = ips;
            portxt.Text = prts;
            passtxt.Text = pps;
            timeoutText.Text = tms;  
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace PUPPIGUI
{
    internal partial class PUPPIMapViewForm : Form
    {
        //transmitted to map view to load elements
        internal PUPPICanvas pc;
        internal PUPPIMapViewControl pmvc;
        internal PUPPIMapViewForm()
        {
            InitializeComponent();
        }

        private void PUPPIMapViewForm_Load(object sender, EventArgs e)
        {
            pmvc=new PUPPIMapViewControl(pc);
            ElementHost elo = new ElementHost();
            //elo.Width = 640;
            //elo.Height = 480;
            //elo.Left = 30;
            //elo.Top = 20; 
            elo.Child = pmvc;

            this.tlp1.Controls.Add(elo, 0, 0);
            elo.Dock = DockStyle.Fill;  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}

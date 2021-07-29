using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUPPIGUI
{
    internal partial class PUPPInodeinfo : Form
    {
        internal ListForm lf;
        internal PUPPInodeinfo()
        {
            InitializeComponent();
        }

        private void closeb_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void outputvalues_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void PUPPInodeinfo_Load(object sender, EventArgs e)
        {
            
        }

        private void closeb_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close(); 
        }

        private void closeb_MouseDown(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void inputvalues_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void showContainer_Click(object sender, EventArgs e)
        {
            lf.ShowDialog(); 
        }
    }
}

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
    internal partial class setNodeOutputForm : Form
    {
        internal int iG = 0;
        internal int oG = 0;
        internal int ii = 0;
        internal int oi = 0;
        internal long ml = 0;
        internal setNodeOutputForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void setNodeOutputForm_Load(object sender, EventArgs e)
        {
            nodeInGUIDText.Text = iG.ToString();
            nodeOutGUIDText.Text = oG.ToString();
            nodeOutOutputIndexText.Text = oi.ToString();
            nodeInInputIndexText.Text = ii.ToString();
            maxLoopTextBox.Text = ml.ToString();   
        }

        private void dbut_Click(object sender, EventArgs e)
        {
            try
            {
                iG = Convert.ToInt16(nodeInGUIDText.Text);
                oG = Convert.ToInt16(nodeOutGUIDText.Text);
                oi = Convert.ToInt16(nodeOutOutputIndexText.Text);
                ii = Convert.ToInt16(nodeInInputIndexText.Text);
                ml = Convert.ToInt64(maxLoopTextBox.Text);
            }
            catch
            {

            }
            this.Close(); 
        }
    }
}

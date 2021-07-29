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
    internal partial class PUPPIPickTreeInputForm : Form
    {
        internal PUPPICanvas mycanvas;
        PUPPITreeView ptv;
        //to actually set
        internal bool goforit;
        internal int nodeid = -1;
        internal int outputid = -1;
        internal PUPPIPickTreeInputForm()
        {
            InitializeComponent();
        }
       
        private void PUPPIPickTreeInputForm_Load(object sender, EventArgs e)
        {
            try
            {
                ptv = new PUPPITreeView(mycanvas);
                this.Controls.Add(ptv);
            }
            catch
            {

            }
        }

        private void sinput_Click(object sender, EventArgs e)
        {
            try
            {
                if (ptv.PUPPINodeTree.SelectedNode.Parent.Text == "Outputs")
                {

                    nodeid = Convert.ToInt32(ptv.PUPPINodeTree.SelectedNode.Parent.Parent.Name);
                    outputid = ptv.PUPPINodeTree.SelectedNode.Parent.Nodes.IndexOf(ptv.PUPPINodeTree.SelectedNode);
                    if (nodeid != -1 && outputid != -1)
                    {
                        goforit = true;
                        this.Close();
                        return;
                    }

                }
            }
            catch
            { }
            nodeid = -1;
            outputid = -1; 
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            goforit = false;
            this.Close(); 
        }

     
    }
}

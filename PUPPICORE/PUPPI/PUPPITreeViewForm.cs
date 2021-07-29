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
    
    internal partial class PUPPITreeViewForm : Form
    {
        internal PUPPICanvas mycanvas;
        PUPPITreeView ptv;
        internal PUPPITreeViewForm()
        {
            InitializeComponent();
        }

        private void PUPPITreeViewForm_Load(object sender, EventArgs e)
        {
            try
            {
                ptv = new PUPPITreeView(mycanvas);
                mainLayoutPanel.Controls.Add(ptv, 0, 0);
                ptv.Dock = DockStyle.Fill;  
                //this.Controls.Add(ptv);
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //gets the node from clicking on something
        private TreeNode getclickednode()
        {
            //first we figure out which node selected
            TreeNode pnode = new TreeNode();
            try
            {
                pnode = ptv.PUPPINodeTree.SelectedNode;
            }
            catch { pnode = new TreeNode(); }
            try
            {

                if (ptv.PUPPINodeTree.SelectedNode.Parent.Text == "Inputs" || ptv.PUPPINodeTree.SelectedNode.Parent.Text == "Outputs")
                {
                    pnode = ptv.PUPPINodeTree.SelectedNode.Parent.Parent;

                }
            }
            catch { }
            try
            {
                if (ptv.PUPPINodeTree.SelectedNode.Text == "Children")
                {
                    pnode = ptv.PUPPINodeTree.SelectedNode.Parent;
                }
            }
            catch { }
            try
            {

                if (ptv.PUPPINodeTree.SelectedNode.Text == "Inputs" || ptv.PUPPINodeTree.SelectedNode.Text == "Outputs")
                {
                    pnode = ptv.PUPPINodeTree.SelectedNode.Parent;

                }
            }
            catch { }
            return pnode;
        }

        private void ninfo_Click(object sender, EventArgs e)
        {


            TreeNode pnode = getclickednode();
            try
            {
                //get by node
                if (pnode.Name != "")
                {
                      //  string[] words = pnode.Text.Split(new string[] { " NodeGUID: " }, StringSplitOptions.None);
                        PUPPICanvas.ModViz3DNode tnode = null;
                        mycanvas.stacks.TryGetValue(pnode.Name  , out tnode);
                        if (tnode != null) tnode.showinfo();
                   

                }
            }
            catch
            {

            }
        }

        private void focuson_Click(object sender, EventArgs e)
        {
            TreeNode pnode = getclickednode();
            try
            {
                //get by node
                if (pnode.Name != "")
                {
                    //  string[] words = pnode.Text.Split(new string[] { " NodeGUID: " }, StringSplitOptions.None);
                    PUPPICanvas.ModViz3DNode tnode = null;
                    mycanvas.stacks.TryGetValue(pnode.Name, out tnode);
                    if (tnode != null) tnode.focuscameraon();


                }
            }
            catch
            {

            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
             
            string toFind = searchTextBox.Text;
            int found = 0;
            if (toFind!="")
            {
                ptv.PUPPINodeTree.CollapseAll();
                foreach (TreeNode t in ptv.PUPPINodeTree.Nodes)
                {
                    if (t.Text.Contains(toFind))
                    {
                        t.Expand();
                        found++;
                    }
 
                }
                if (found>0)
                {
                    MessageBox.Show("Found " + found.ToString() + " nodes which have been expanded in the tree view");
  
                }
                else
                {
                    MessageBox.Show("No nodes found with " + toFind);  
                }
            }
            else
            {
                MessageBox.Show("Empty search box");  
            }
        }
    }
}

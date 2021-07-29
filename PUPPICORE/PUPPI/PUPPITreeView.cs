using System;
using System.Collections.Generic;
using System.Collections; 
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUPPIGUI
{
    internal partial class PUPPITreeView : UserControl
    {
        internal PUPPITreeView()
        {
            InitializeComponent();
        }
        internal PUPPITreeView(PUPPICanvas mycanvas)
        {
           

            InitializeComponent();
             //populates the tree
            foreach (PUPPICanvas.ModViz3DNode mynode in mycanvas.stacks.Values)
            {


                //base node
                if (mynode.parent == null)
                {
                    TreeNode treacher=new TreeNode(); 
                        if (mynode.displayname!=""  )
                        {
                            treacher = new TreeNode(mynode.displayname + " NodeGUID: " + mynode.nodeGUID.ToString());
                        }
                        else if (mynode.logical_representation.cleancap!="")
                        {
                            treacher = new TreeNode(mynode.logical_representation.cleancap + " NodeGUID: " + mynode.nodeGUID.ToString());
                        }
                        else
                        {
                            treacher = new TreeNode(mynode.logical_representation.name + " NodeGUID: " + mynode.nodeGUID.ToString());
                        }
                    //will retrieve by name
                        treacher.Name = mynode.nodeGUID.ToString();    
                            TreeNode inps = new TreeNode("Inputs");
                    treacher.Nodes.Add(inps);
                    //create input names
                    foreach (string iname in mynode.getinputnames())
                    {
                        inps.Nodes.Add(iname);
                    }
                    TreeNode onps = new TreeNode("Outputs");
                    treacher.Nodes.Add(onps);
                    //create output names
                    foreach (string oname in mynode.getoutputnames())
                    {
                        onps.Nodes.Add(oname);
                    }
                    //now do the same for all its children
                    List<PUPPICanvas.ModViz3DNode> clist = new List<PUPPICanvas.ModViz3DNode>();
                    TreeNode childs = new TreeNode("Children");
                    treacher.Nodes.Add(childs);
                    mynode.getalllevelschildren(clist);
                    foreach (PUPPICanvas.ModViz3DNode cnode in clist)
                    {
                        TreeNode creacher=new TreeNode(); 
                        if (cnode.displayname != "")
                        {
                            creacher  = new TreeNode(cnode.displayname + " NodeGUID: " + cnode.nodeGUID.ToString());
                        }
                        else if (cnode.logical_representation.cleancap!="")
                        {
                            creacher = new TreeNode(cnode.logical_representation.cleancap + " NodeGUID: " + cnode.nodeGUID.ToString());
                        }
                        else
                        {
                            creacher = new TreeNode(cnode.logical_representation.name + " NodeGUID: " + cnode.nodeGUID.ToString());
                        }
                        creacher.Name = cnode.nodeGUID.ToString();     
                            TreeNode cinps = new TreeNode("Inputs");
                        creacher.Nodes.Add(cinps);
                        //create input names
                        foreach (string iname in cnode.getinputnames())
                        {
                            cinps.Nodes.Add(iname);
                        }
                        TreeNode conps = new TreeNode("Outputs");
                        creacher.Nodes.Add(conps);
                        //create output names
                        foreach (string oname in cnode.getoutputnames())
                        {
                            conps.Nodes.Add(oname);
                        }
                        childs.Nodes.Add(creacher);
                    }


                    this.PUPPINodeTree.Nodes.Add(treacher); 

                }
            }

        }
    }
}

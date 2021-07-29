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
    internal partial class setlistlevelform : Form
    {
        //set variables to use
        internal int numelem=0;
        internal int lstlevel = 0;
       
        internal setlistlevelform(int llevel,bool cansetelems,int curelemnumber)
        {
            lstlevel = llevel;  
            InitializeComponent();
            singleitem.Checked = (llevel == 0);
            listproc.Checked = (llevel == 1);
            textBox1.Enabled = cansetelems;
            textBox1.Text = curelemnumber.ToString();    
            numelem = curelemnumber;  
  

        }

        private void singleitem_CheckedChanged(object sender, EventArgs e)
        {
            if (singleitem.Checked==true  )
            {
                lstlevel = 0;
            }
        }

        private void listproc_CheckedChanged(object sender, EventArgs e)
        {
            if (listproc.Checked ==true )
            {
                lstlevel = 1; 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (textBox1.Enabled ==true )
            //{
            //    try
            //    {
            //        numelem = Convert.ToInt16(textBox1.Text);
            //    }
            //    catch
            //    {
            //        numelem = 0; 
            //    }
            //    if (lstlevel==1 && numelem==0 )
            //    {
            //        numelem = 1; 
            //    }
            //}
            //this.Close(); 
        }

        private void setlistlevelform_MouseDown(object sender, MouseEventArgs e)
        {
           
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBox1.Enabled == true)
            {
                try
                {
                    numelem = Convert.ToInt16(textBox1.Text);
                }
                catch
                {
                    numelem = 0;
                }
                if (lstlevel == 1 && numelem == 0)
                {
                    numelem = 1;
                }
            }
            this.Close(); 
        }

       
    }
}

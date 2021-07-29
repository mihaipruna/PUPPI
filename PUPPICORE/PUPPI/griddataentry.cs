using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;  
using System.Collections;

namespace PUPPI
{
    internal partial class griddataentry : Form
    {
        //whenc cancel, we don't udpate

        internal bool doupdate = false;
        //store stuff in an arraylist of arraylists
        internal ArrayList griddata;
        internal griddataentry(ArrayList existingdata)
        { //populate with what we have so far
            InitializeComponent();
            griddata = new ArrayList();
            if (existingdata != null)
            {
            
                try
                {
                    foreach (ArrayList roww in existingdata )
                    {
                        griddata.Add(new ArrayList() );
                        (griddata[griddata.Count-1  ] as ArrayList ).AddRange(roww); 
                    }
                }
                catch
                {
                    griddata = new ArrayList();
                }
            
            }
            dataGridView1.Rows.Clear(); 
            foreach (ArrayList roww in griddata )
            {
                string[] rdata=new string[roww.Count ];
                dataGridView1.ColumnCount = roww.Count;   
                //make new row
                int i=0;
                foreach (string s in roww)
                {
                    rdata[i]=s;
                    i++;
                }
                dataGridView1.Rows.Add(rdata);   
            }
            if (dataGridView1.Rows.Count==0   )
            {
                dataGridView1.Columns.Add("", "");   
            }
            //rowcount.Text = dataGridView1.Rows.Count.ToString();
            //colcount.Text = dataGridView1.Columns.Count.ToString();      
        }




        private void label1_Click(object sender, EventArgs e)
        {

        }

        //private void setrc_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //change number of rows and columns while trying to rpeserve data
        //        int newrows = 0;
        //        newrows = Convert.ToInt16(rowcount.Text);
        //        int newcols = 0;
        //        newcols = Convert.ToInt16(colcount.Text);
        //        if (newrows < 0) { newrows = 0; rowcount.Text = "0"; }
        //        if (newcols < 0) { newcols = 0; colcount.Text = "0"; }

        //        while (newcols > dataGridView1.Columns.Count)
        //        {
        //            dataGridView1.Columns.Add("","");

        //        }
        //        while (newcols < dataGridView1.Columns.Count)
        //        {
        //            dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);

        //        }
        //            while (newrows > dataGridView1.Rows.Count)
        //            {
        //                dataGridView1.Rows.Add(new DataGridViewRow());

        //            }
        //            while (newrows < dataGridView1.Rows.Count)
        //            {
        //                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);

        //            }
                
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Invalid entry");  
        //    }
        //}

        private void donebutton_Click(object sender, EventArgs e)
        {
            doupdate = true;
            //update grid data
            griddata = new ArrayList();
           foreach (DataGridViewRow row in dataGridView1.Rows) {
               
                     ArrayList grow = new ArrayList();
                     for (int cc = 0; cc < row.Cells.Count;cc++ )
                     {
                         //don't do null cells
                         if (row.Cells[cc].Value != null)
                         {
                             grow.Add(row.Cells[cc].Value.ToString());
                         } 
                     }
                     if (grow.Count > 0)
                     {
                         griddata.Add(grow);
                     }
                }
            if (griddata.Count == 0) doupdate = false;
            this.Close(); 
        }

        private void cancelbutton_Click(object sender, EventArgs e)
        {
            this.Close();  
        }

        private void clearbutton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            griddata.Clear();  
        }

        private void addcol_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Add("", "");   
        }

        private void removecol_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Columns.Count > 0) dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);   
        }
    }
}

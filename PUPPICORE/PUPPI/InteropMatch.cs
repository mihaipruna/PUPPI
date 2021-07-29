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
    public partial class InteropMatch : Form
    {
        //original type matches, if we press cancel
        public List<string> oVTN;
        public List<string> oVTM;
        //after user interaction
        public List<string> dVTN;
        public List<string> dVTM;
        public InteropMatch()
        {
            InitializeComponent();
            oVTN = new List<string>();
            oVTM = new List<string>();
           dVTN = new List<string>();
           dVTM = new List<string>();
        }

        private void addmap_Click(object sender, EventArgs e)
        {
            string vtn="";
            PUPPIFormUtils.formutils.InputBox("Field name input", "Please enter field name in class to convert from", ref vtn);
            vtn = vtn.Replace(" ", "");
            string vtm = "";
            PUPPIFormUtils.formutils.InputBox("Field name input", "Please enter matching field name in class to convert to", ref vtm);
            vtm = vtm.Replace(" ", "");

            if (!checkIL(vtn,vtm) )
            {
                listBox1.Items.Add(vtn + " " + vtm);   
            }
            
        }

        public bool checkIL(string vtn,string vtm)
        {
            for (int i=0;i<listBox1.Items.Count;i++    )
            {
                string svp = listBox1.Items[i].ToString() ;
                char[] spa = { ' ' };
                string[] spav = svp.Split(spa);
                if (spav[0] == vtn || spav[1] == vtm) return true;

            }
            return false;
        }

        private void InteropMatch_Load(object sender, EventArgs e)
        {
            for (int i=0;i<oVTN.Count;i++  )
            {
                string vn = oVTN[i];
                string vm = oVTM[i];
                listBox1.Items.Add(vn + " " + vm);
   
            }
        }

        private void rm_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex>=0  )
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
   
            }
        }

        private void dn_Click(object sender, EventArgs e)
        {
            dVTM.Clear();
            dVTN.Clear();
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string svp = listBox1.Items[i].ToString();
                char[] spa = { ' ' };
                string[] spav = svp.Split(spa);
                dVTN.Add(spav[0]);
                dVTM.Add(spav[1]); 

            }
            this.Close(); 
        }

        private void cl_Click(object sender, EventArgs e)
        {
            dVTM.Clear();
            dVTN.Clear();
            foreach (string s in oVTN)
                dVTN.Add(s);
            foreach (string s in oVTM)
                dVTM.Add(s);
            this.Close(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Click Add Mapping, then enter name of a field from class to convert from, the press Enter, then enter name of field to be set to the same value in class to convert into.");
        }
        
    }
}

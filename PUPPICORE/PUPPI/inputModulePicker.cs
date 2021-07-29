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
    public partial class inputModulePicker : Form
    {
        public string selectedModule = "";
        public List<string> modNames;
        public List<string> modest;
        public inputModulePicker()
        {
            InitializeComponent();
            modNames = new List<string>();
            modest = new List<string>(); 
        }

        private void inputModulePicker_Load(object sender, EventArgs e)
        {
            selectedModule = "";
            foreach (string s in modNames)
            {
                modList.Items.Add(s);
                

            }
            mNumber.Text = modList.Items.Count.ToString()  + " modules found";    
 
        }

        private void aBut_Click(object sender, EventArgs e)
        {
            modList.Items.Clear();
            string mn = fBox.Text;
            if (mn=="")
            {
                foreach (string s in modNames)
                {
                    modList.Items.Add(s);

                }
            }
            else
            {
                foreach (string s in modNames)
                {
                    if (s.Contains(mn))
                    {
                        modList.Items.Add(s);
                    }
                }
            }
            mNumber.Text = modList.Items.Count.ToString() + " modules found";  
        }

        private void useSel_Click(object sender, EventArgs e)
        {
            if (modList.SelectedIndex>=0)
            {
                selectedModule = modList.Items[modList.SelectedIndex].ToString() ;

            }
            else
            {
                selectedModule = "";
 
            }
            this.Close(); 
        }

        private void cBut_Click(object sender, EventArgs e)
        {
            selectedModule = "";
            this.Close();  
        }

        private void mNumber_Click(object sender, EventArgs e)
        {

        }

        private void modList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modList.SelectedIndex!=-1)
                mNumber.Text = modList.Items[modList.SelectedIndex].ToString();  
        }
    }
}

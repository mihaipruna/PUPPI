using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PUPPICADBeta
{
    public partial class setIgnoreMethod : Form
    {
        public setIgnoreMethod()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string entered = "";
            if (PUPPIFormUtils.formutils.InputBox("Please enter a string", "Enter text:", ref entered) != System.Windows.Forms.DialogResult.Cancel)
            {
                
                if (entered!="")
                {
                    if (listBox1.Items.Contains(entered)   )
                    {
                        MessageBox.Show("Filter already defined!");
                    }
                    else
                    {
                        listBox1.Items.Add(entered); 
                    }
                }
                else
                {
                    MessageBox.Show("Empty string!") ; 
                }
            
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ignoreModuleFilter.Clear();
            foreach (string myFilter in listBox1.Items)
            {
                Properties.Settings.Default.ignoreModuleFilter.Add(myFilter);
            }
            Properties.Settings.Default.Save();
            

            this.Close();  
        }

        private void hButt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Set module or toolbar names or partial names to be ignored and not added to the dynamically generated menus. \n Typically names like ToString , GetHashCode , GetType, Equals are used for system functions and can be ignored.");
        }

        private void setIgnoreMethod_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();  
            foreach (string myFilter in Properties.Settings.Default.ignoreModuleFilter)
            {
               listBox1.Items.Add(myFilter);
            }
        }
    }
}

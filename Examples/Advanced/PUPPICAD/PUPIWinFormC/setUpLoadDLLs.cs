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
    public partial class setUpLoadDLLs : Form
    {
        public setUpLoadDLLs()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {

            Properties.Settings.Default.generateModulesFrom.Clear();
            foreach (string dPath in dllListBox.Items  )
            {
                Properties.Settings.Default.generateModulesFrom.Add(dPath);
            }
            Properties.Settings.Default.Save();
         //   System.Windows.MessageBox.Show("Please restart PUPPICAD for changes to take effect.");
            
            this.Close(); 
        }

        private void setUpLoadDLLs_Load(object sender, EventArgs e)
        {
            dllListBox.Items.Clear();  
            foreach (string dPath in Properties.Settings.Default.generateModulesFrom  )
            {
                dllListBox.Items.Add  (dPath);  
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg;
            Nullable<bool> result;
            
            dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "DLL/MTPS Files |*.dll;*.mtps";
            result = dlg.ShowDialog();
            if (result == true)
            {
                if (dllListBox.Items.Contains(dlg.FileName))
                {
                    MessageBox.Show("DLL already loaded");  
                }
                else
                {
                    dllListBox.Items.Add(dlg.FileName);
                }
                
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (dllListBox.SelectedIndex>=0  )
            {
                dllListBox.Items.RemoveAt(dllListBox.SelectedIndex);
            }
        }

        private void hButt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pick which .dll files to create PUPPIModule toolbars from. If a file doesn't load, right click on the file in Windows Explorer and click Unblock. Some dll files are incompatible. Place dependency dll files in same folder."); 
        }

        private void iButton_Click(object sender, EventArgs e)
        {
            setIgnoreMethod sm = new setIgnoreMethod();
            sm.ShowDialog(); 
        }

        private void dllListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

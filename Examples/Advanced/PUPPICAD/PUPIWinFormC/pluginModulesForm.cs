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
    public partial class pluginModulesForm : Form
    {
        public List<string> dllfiles;
        public pluginModulesForm()
        {
            InitializeComponent();
            dllfiles = new List<string>(); 
        }

        private void pluginModulesForm_Load(object sender, EventArgs e)
        {
            foreach (string s in dllfiles )
            {

                pluginFileList.Items.Add(s);   
            }
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Place DLL files containing PUPPIModule classes in the PluginPUPPIModules folder or subfolders.");  
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}

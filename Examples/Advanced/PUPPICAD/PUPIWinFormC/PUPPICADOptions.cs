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
    public partial class PUPPICADOptions : Form
    {

        public List<string> loadedPlugins = new List<string>(); 
        public PUPPICADOptions()
        {
            InitializeComponent();
        }

        private void PUPPICADOptions_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.loadHelixWPF == true)
            {
                
                this.loadWPFHelix.Checked = true;
            }
            else
            {
                
                this.loadWPFHelix.Checked = false;
            }


            if (Properties.Settings.Default.extraButtonsMode == "tablet")
            {

                this.tabletModeRadio.Checked = true;
            }

            if (Properties.Settings.Default.extraButtonsMode == "mouse")
            {
                
                this.mouseControlRadio.Checked = true;
            }

            if (Properties.Settings.Default.extraButtonsMode == "laptop")
            {

                this.laptopModeRadio.Checked = true;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (this.loadWPFHelix.Checked==true  )
            {
                Properties.Settings.Default.loadHelixWPF = true;    
            }
            else
            {
                Properties.Settings.Default.loadHelixWPF = false;    
            }
            

            if (this.tabletModeRadio.Checked==true  )
            {
                Properties.Settings.Default.extraButtonsMode = "tablet";
            }

            if (this.mouseControlRadio.Checked == true)
            {
                Properties.Settings.Default.extraButtonsMode = "mouse";
            }

            if (this.laptopModeRadio.Checked==true)
            {
                Properties.Settings.Default.extraButtonsMode = "laptop";
            }
            Properties.Settings.Default.Save();    
            MessageBox.Show("Please restart PUPPICAD for any changes to take effect.");
            this.Close(); 
        }

        private void runtimePluginsButton_Click(object sender, EventArgs e)
        {
            setUpLoadDLLs formL = new setUpLoadDLLs();
            formL.ShowDialog();
        }

        private void serverSetupButton_Click(object sender, EventArgs e)
        {
            PUPPI.PUPPICADserverSetup ps = new PUPPI.PUPPICADserverSetup();
            ps.ShowDialog();
        }

        private void dllPluginButton_Click(object sender, EventArgs e)
        {
            PUPPICADBeta.pluginModulesForm plf = new pluginModulesForm();
            plf.dllfiles = loadedPlugins;
            plf.ShowDialog(); 
        }
    }
}

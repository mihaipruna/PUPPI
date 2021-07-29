using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;  
using System.Collections; 
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using System.Net.NetworkInformation;
using System.Management;
using System.Management.Instrumentation;
using System.Security.Cryptography;
using System.Security;
using System.Net; 

namespace PUPPIAssemblyCreator
{
  
    public partial class Form1 : Form
    {
        Assembly my_PUPPI_assembly;
        string save_assembly_file_path="";
        String mtps_file_path="";
        string load_DLL_file_path = "";
        string load_DLL_namespace_name = "";
        Type PUPPI_Module;
        List<ArrayList> PUPPI_ModTypes;
        public Form1()
        {
            string vue;
            vue = Environment.GetEnvironmentVariable("PUPILocalID");
            //MessageBox.Show(vue);  
            if (vue == null)
            {
                MessageBox.Show("Not available, invalid or expired trial license.");
                Environment.Exit(3);  
            }
            InitializeComponent();
            try
            {
                my_PUPPI_assembly = Assembly.LoadFrom("PUPPI.dll");
            }
            catch
            {
                my_PUPPI_assembly = null;
            }
            if (my_PUPPI_assembly == null)
            {
                MessageBox.Show("Failed to load PUPPI.dll, needs to be in the same folder as this executable.");
                Environment.Exit(1);  

            }
            PUPPI_Module = my_PUPPI_assembly.GetType("PUPPIModel.PUPPIModule");
            radioButtonMTPS.Checked = true;    
            radioButtonMTPS_CheckedChanged(null, null); 
        }

    

        private void mtps_file_name_Click(object sender, EventArgs e)
        {
            
        }

        private void load_mtps_file_Click(object sender, EventArgs e)
        {
             OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "moduleTypes files (*.mtps)|*.mtps";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                try
                {

                    mtps_file_path = openFileDialog1.FileName;
                    mtps_file_name.Text  = openFileDialog1.FileName;
                    
                }
                catch
                {
                    MessageBox.Show("Error reading from MTPS file");
                    mtps_file_name.Text  = "Error reading last MTPS file";
                    mtps_file_path = ""; 
                }
            }
        }

        private void assembly_dll_file_Click(object sender, EventArgs e)
        {
            
            FolderBrowserDialog saveFileDialog1 = new FolderBrowserDialog();

      
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Directory.GetFiles(saveFileDialog1.SelectedPath).Length>0 )
                {
                    MessageBox.Show("Please select an empty folder for output");
                    return;
                }
                    save_assembly_file_path = saveFileDialog1.SelectedPath;
                    assembly_name_label.Text = saveFileDialog1.SelectedPath; 

               
            }
        }

        private void generate_assembly_Click(object sender, EventArgs e)
        {

            if (radioButtonMTPS.Checked)
            {
                if (save_assembly_file_path == "" || mtps_file_path == "" || ass_name_textbox.Text=="")
                {
                    MessageBox.Show("Missing specifications, make sure you load an MTPS file and also set an assembly path to save to and assembly file name suffix!");
                    return;
                }

            }
            else
            {
                if (save_assembly_file_path == "" || load_DLL_file_path == "" || namespaceTextBox.Text == "" || ass_name_textbox.Text == "")
                {
                    MessageBox.Show("Missing specifications, make sure you load a DLL file and set a namespace name,  and also set an assembly path to save to and assembly file name suffix!");
                    return;
                }
            }
                      char[] sp={'\\'};
            string new_assembly_name = save_assembly_file_path.Split(sp,StringSplitOptions.None  )[save_assembly_file_path.Split(sp,StringSplitOptions.None ).Length - 1].Replace(".dll","") ;
   
            AppDomain currentDom = AppDomain.CurrentDomain;
    //make sure assembly loaded if not load it
    Type PUPPI_MTPS_creator = my_PUPPI_assembly.GetType("PUPPIModel.AutomaticPUPPImodulesCreator");
    List<string> tnames = new List<string>();

    if (radioButtonMTPS.Checked)
    {


        object[] method_params = new object[4];
        method_params[0] = mtps_file_path;

        method_params[1] = null;//tnames;
        method_params[2] = save_assembly_file_path;
        method_params[3] = ass_name_textbox.Text;
        MethodInfo mi = PUPPI_MTPS_creator.GetMethod("exportPUPPImoduleListsFromMTPS");

        object o = mi.Invoke(null, method_params);
        PUPPI_ModTypes = o as List<ArrayList>;
        tnames = method_params[1] as List<string>;
    }
    else
    {
        
        object[] method_params = new object[5];
        method_params[0] = namespaceTextBox.Text  ;

        method_params[1] = null;//tnames;
        method_params[2] = save_assembly_file_path;
        method_params[3] = "";
        method_params[4] = ass_name_textbox.Text;
        MethodInfo mi = PUPPI_MTPS_creator.GetMethod("exportPUPPImoduleListsFromNamespace");

        object o = mi.Invoke(null, method_params);
        PUPPI_ModTypes = o as List<ArrayList>;
        tnames = method_params[1] as List<string>;
   
    }
    MessageBox.Show("Generated " + PUPPI_ModTypes.Count.ToString() + " assemblies from types.");

  

        }

        private void exit_program_Click(object sender, EventArgs e)
        {
            Application.Exit();  
        }

        private void load_mtps_file_Click_1(object sender, EventArgs e)
        {
            load_mtps_file_Click(sender, e);
        }

        private void tableLayoutPanel4_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }

        private void radioButtonMTPS_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMTPS.Checked  )
            {
                load_mtps_file.Enabled = true;
                load_dll_file.Enabled = false;
                namespaceTextBox.Enabled = false;
                if (mtps_file_path == "")
                {
                    mtps_file_name.Text  = "No MTPS file loaded"; 
                }
                else
                {
                    mtps_file_name.Text  = mtps_file_path; 
                }
            }
            else
            {
                load_mtps_file.Enabled = false;
                load_dll_file.Enabled = true;
                namespaceTextBox.Enabled = true;  
                if (load_DLL_file_path=="" )
                {
                    mtps_file_name.Text = "No DLL Assembly loaded";  
                }
                else
                {
                    mtps_file_name.Text = load_DLL_file_path; 
                }
            }
        }

        private void radioButtonNamespace_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMTPS.Checked)
            {
                load_mtps_file.Enabled = true;
                load_dll_file.Enabled = false;
                namespaceTextBox.Enabled = false;
            }
            else
            {
                load_mtps_file.Enabled = false;
                load_dll_file.Enabled = true;
                namespaceTextBox.Enabled = true;
            }
        }

        private void load_dll_file_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "DLL files (*.dll)|*.dll";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                try
                {

                    load_DLL_file_path = openFileDialog1.FileName;
                    mtps_file_name.Text = openFileDialog1.FileName;
                    Assembly aa=Assembly.UnsafeLoadFrom(load_DLL_file_path);
                    if (aa == null) throw new Exception("Null assembly "); 

                }
                catch (Exception exy)
                {
                    MessageBox.Show("Error reading dll assembly: "+exy.ToString() );
                    mtps_file_name.Text = "Error reading last DLL file";
                    load_DLL_file_path  = "";
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
    
}

using System;
using System.Collections; 
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Net;
using PUPPIModel;


namespace PUPPI
{
    internal partial class PUPPIScriptingWindow : Form
    {
        internal List<string> winAssemblyNames;
        internal List<string> winAssemblyPaths;
        internal ArrayList winInputsFromNode;
        internal List<string> winInputNamesFromNode;
        internal ArrayList winResultOutputs;
        internal List<string> winResultOutputNames;
        internal string storedScriptData = "";
        internal string aS = "";

        ////orig data
        //internal List<string> origAPs;
        //internal string origScript;
        //internal bool origCS;
        //internal bool origALN;
        //internal string origSSD;

        //store selection for automatic namespace load
        internal bool autoLoadScriptNamespaces;
        //keep reference to scripting module
        PUPPIModel.PUPPIPremadeModules.LogicModules.PUPPIInteractiveScript pM = null;   
        internal PUPPIScriptingWindow()
        {
            InitializeComponent();
            winInputsFromNode = new ArrayList();
            winResultOutputs = new ArrayList();
            winAssemblyPaths = new List<string>();
            winAssemblyNames = new List<string>();
            winInputNamesFromNode = new List<string>();
            winResultOutputNames = new List<string>();
            csRadioButton.Checked = true;  
            autoLoadScriptNamespaces = true;
            pM = null;
        }

        internal void updateScriptListFromArrayList(ListBox l,IList il)
        {
            l.Items.Clear();  
            foreach (object o in il)
            {
                try
                {
                    l.Items.Add(o.ToString());  
                }
                catch
                {
                    l.Items.Add("null");  
                }
            }
        }

        internal void updateAllScriptLists()
        {
            updateScriptListFromArrayList(loadedDLLList, winAssemblyPaths);
            updateScriptListFromArrayList(inputsArrayList, winInputsFromNode);
            updateScriptListFromArrayList(outputsArrayList, winResultOutputs);

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void PUPPIScriptingWindow_Load(object sender, EventArgs e)
        {
             
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void tesRunButton_Click(object sender, EventArgs e)
        {
           // Console.Clear();
            
            var originalConsoleOut = Console.Out; // preserve the original stream
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);
                updateScriptClassFromWindowData();
                storedScriptData = pM.storedScriptData;
                pM.scriptInputValues.Clear();
                if (pM.usercodeinputs!=null )
                for (int i = 0; i < pM.usercodeinputs.Count; i++)
                {
                    pM.scriptInputValues.Add(pM.usercodeinputs[i]);
                }
                pM.runMyScript(pM.justTheScript);
                aS = pM.aS;
                writer.Flush(); // when you're done, make sure everything is written out

                var myString = writer.GetStringBuilder().ToString();
                consoleTextBox.Text = myString;
                selfUpdateFromScriptClass(pM); 
                updateAllScriptLists();
               
            }
            Console.SetOut(originalConsoleOut); // restore Console.Out
            
              
        }

        private void scriptTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void loadDLLScript_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            Nullable<bool> result = dlg.ShowDialog();
            if (result==true)
            {
                if (winAssemblyPaths.Contains(dlg.FileName ) ==false )
                {
                    winAssemblyPaths.Add(dlg.FileName);
                    updateScriptClassFromWindowData();
                    selfUpdateFromScriptClass(pM); 
                }

            }
        }
        internal void updateScriptClassFromWindowData(bool rinteract=true)
        {
            if (pM == null) return;
            pM.mya = null; 
            pM.autoLoadScriptNamespaces = autoLoadScriptNamespaces;
            pM.assemblyPaths.Clear();  
            pM.assemblyPaths.AddRange(winAssemblyPaths);
            pM.assemblyNames.Clear();
            pM.assemblyNameSpaces.Clear();  
            List<string> fA = new List<string>();  
            foreach (string s in pM.assemblyPaths)
            {
                try
                {
                    string aa =s.Replace(@"\\", @"\");
                    
                    Assembly a = Assembly.LoadFile(aa);
                    pM.assemblyNames.Add(a.GetName().Name);
                    List<string> an = PUPPIModel.PUPPIPremadeModules.LogicModules.PUPPIInteractiveScript.getAssemblyNamespaceNames(a);
                    for (int j = 0; j < an.Count; j++)
                    {
                        if (!pM.assemblyNameSpaces.Contains(an[j]) && an[j].Contains("<")==false && an[j]!="System" && an[j]!="System.Collections") pM.assemblyNameSpaces.Add(an[j]);
                    }
                }
                catch(Exception ex)
                {
                    if (rinteract)
                    MessageBox.Show("Failed to load assembly " + s+"\n\n" + ex.ToString() );
                    fA.Add(s);
                    

                }
            }
            foreach (string f in fA)
            {
                //p.assemblyNames.RemoveAt(p.assemblyPaths.IndexOf(f));  
                pM.assemblyPaths.Remove(f);
                winAssemblyPaths.Remove(f); 
            }
            

            pM.justTheScript = addOutput.Text;
            if (csRadioButton.Checked==true )
            {
                pM.scriptProgrammingLanguage="C#";
            }
            else
            {pM.scriptProgrammingLanguage="VB";}
           
  
        }
        internal void selfUpdateFromScriptClass(PUPPIModel.PUPPIPremadeModules.LogicModules.PUPPIInteractiveScript p)
        {
            pM = p;
            winAssemblyNames.Clear();
            winAssemblyPaths.Clear();
            winResultOutputs.Clear();
            winInputsFromNode.Clear(); 
            winResultOutputNames.Clear();
            winInputNamesFromNode.Clear();   
            autoLoadScriptNamespaces = p.autoLoadScriptNamespaces;
           
           // winAssemblyPaths.AddRange(p.assemblyPaths);
            //foreach (string s in p.assemblyNames )
            //{
            //    winAssemblyNames.Add(s);
            //}

            foreach (string s in p.assemblyPaths)
            {
                winAssemblyPaths.Add(s);
            }
            addOutput.Text = p.justTheScript;
            if (p.scriptProgrammingLanguage =="C#")
            {
                csRadioButton.Checked = true;  
            }
            else
            {
                vbRadioButton.Checked = true;  
            }
            
            
            if (p.usercodeinputs != null)
            {
                for (int ui = 0; ui < p.usercodeinputs.Count; ui++)
                {
                    if (p.usercodeinputs[ui] == null)
                    {
                        if (p.inputnames.Count > ui)
                        {
                            winInputsFromNode.Add(p.inputnames[ui] + " : null");
                        }
                        else
                        {
                            winInputsFromNode.Add("name not set" + " : null");
                        }
                       
                    }
                    else
                    {
                        if (p.inputnames.Count > ui)
                        {
                            winInputsFromNode.Add(p.inputnames[ui] + " : " + p.usercodeinputs[ui].ToString());
                        }
                        else
                        {
                            winInputsFromNode.Add("name not set" + " : " + p.usercodeinputs[ui].ToString());
                        }
                    }
                    if (p.inputnames.Count > ui)
                    {
                        winInputNamesFromNode.Add(p.inputnames[ui]);
                    }
                    else
                    {
                        winInputNamesFromNode.Add("name not set");
                    }
                }
            }


            if (p.usercodeoutputs != null)
            {
                for (int ui = 0; ui <p. usercodeoutputs.Count; ui++)
                {
                    if (p.scriptOutputValues != null && p.scriptOutputValues.Count>ui )
                    {
                        
                        if (p.scriptOutputValues[ui] == null)
                        {
                            if (p.outputnames.Count > ui)
                            {
                                winResultOutputs.Add(p.outputnames[ui] + " : null");
                            }
                            else
                            {
                                winResultOutputs.Add("not set " + " : null");
                            }
                        }
                        else
                        {
                            if (p.outputnames.Count > ui)
                            {
                                winResultOutputs.Add(p.outputnames[ui] + " : " + p.scriptOutputValues[ui].ToString());
                            }
                            else
                            {
                                winResultOutputs.Add("not set" + " : " + p.scriptOutputValues[ui].ToString());
                            }
                        }
                    }
                    else
                    {
                        if (p.outputnames.Count > ui)
                        {
                            winResultOutputs.Add(p.outputnames[ui] + " : null");
                        }
                        else
                        {
                            winResultOutputs.Add("not set " + " : null");
                        }
                    }
                    if (p.outputnames.Count > ui)
                    {
                        winResultOutputNames.Add(p.outputnames[ui]);
                    }
                    else
                    {
                        winResultOutputNames.Add("not set ");
                    }
                }
            }
            updateAllScriptLists();
            autoLoadBox.Checked = autoLoadScriptNamespaces;
        }

        private void csRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            updateScriptClassFromWindowData();
            selfUpdateFromScriptClass(pM);
            updateAllScriptLists();
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            updateScriptClassFromWindowData();
            pM.saveScriptForStorage();
            storedScriptData = pM.storedScriptData;
            this.Close(); 

            
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            //sets it up from saved XML
            pM.setUpMyScript(); 
            this.Close(); 
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"Example,addition operation, when the script is fed 2 inputs.One output is generated and displayed in the console:
C#:                
scriptOutputs[0]=Convert.ToDouble(nodeInputs[0])+Convert.ToDouble(nodeInputs[1]);
Console.WriteLine(scriptOutputs[0].ToString());
VB:
scriptOutputs(0)=Convert.ToDouble(nodeInputs(0))+Convert.ToDouble(nodeInputs(1))
Console.WriteLine(scriptOutputs(0).ToString())

If script uses assemblies, when distributing the script place assemblies in the same folder as the saved PUPPI canvas file containing the script node.
            ");
        }

        private void removeScriptDLL_Click(object sender, EventArgs e)
        {
            try
            {
                winAssemblyPaths.RemoveAt(loadedDLLList.SelectedIndex);
                //loadedDLLList.Items.Remove(loadedDLLList.SelectedItem);  
            }
            catch(Exception exy)
            {
                return;
            }
                updateScriptClassFromWindowData();
                selfUpdateFromScriptClass(pM);
            
        }

        private void loadedDLLList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void codeInConsole_Click(object sender, EventArgs e)
        {
            if (aS != "")
            {
                consoleTextBox.Clear();
                consoleTextBox.Text = aS;
            }
            else
            {
                MessageBox.Show("Press Test Run to generate code");  
            }
        }

        private void inputsArrayList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void autoLoadBox_CheckedChanged(object sender, EventArgs e)
        {
            autoLoadScriptNamespaces = autoLoadBox.Checked;
            updateScriptClassFromWindowData();
            selfUpdateFromScriptClass(pM);
        }

        private void csRadioButton_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void addInputButton_Click(object sender, EventArgs e)
        {
            string dn = "";
            int au = -1;
                    PUPPIFormUtils.formutils.InputBox("Enter number of inputs to script node", "Enter number", ref dn);
                    if (dn != "")
                    {
                        try
                        {
                            au = Convert.ToInt16(dn);   
                        }
                        catch
                        {
                            au = -1;
                        }
                    }
            if (au>=0)
            {
                if (au!=pM.inputs.Count   )
                {

                }
            }
        }

        private void addOutputButton_Click(object sender, EventArgs e)
        {

        }
        internal void updateScriptClassIOFromWindowData()
        {
            pM.inputs.Clear();
            pM.inputnames.Clear();
            pM.outputs.Clear();
            pM.outputnames.Clear();
            for (int i = 0; i < winInputNamesFromNode.Count;i++ )
            {
                PUPPIInParameter ti = new PUPPIInParameter();
                ti.isoptional = true;
                pM.inputs.Add(ti);
                pM.inputnames.Add(winInputNamesFromNode[i]  );

            }
            for (int i = 0; i < winResultOutputNames.Count;i++ )
            {
                pM.outputs.Add("0");
                pM.outputnames.Add(winResultOutputNames[i]);
            }
            pM.resetModuleNumberCalls(); 
  
        }
        private void addInput_Click(object sender, EventArgs e)
        {
           
     
        }

        private void removeInput_Click(object sender, EventArgs e)
        {
            
        }

        private void addoutput1_Click(object sender, EventArgs e)
        {
           
        }

        private void removeOutput_Click(object sender, EventArgs e)
        {
            
        }

        private void addInput_Click_1(object sender, EventArgs e)
        {
            string jl = "";
            DialogResult dl=PUPPIFormUtils.formutils.InputBox("Input name", "Please enter name for new input", ref jl);
            if (dl == DialogResult.Cancel) return;
            if (jl == "") jl = "Input" + winInputsFromNode.Count + 1;
            //winInputNamesFromNode.Add(jl);
            PUPPIInParameter ti = new PUPPIInParameter();
            ti.isoptional = true;
            pM.inputs.Add(ti);
            pM.inputnames.Add(jl);


            updateScriptClassFromWindowData(true);
            //updateScriptClassIOFromWindowData();


            pM.saveScriptForStorage();
            pM.resetModuleNumberCalls();
            pM.forceMyNodeToUpdate();
            this.Close();
        }

        private void removeInput_Click_1(object sender, EventArgs e)
        {
            if (inputsArrayList.Items.Count   > 0)
            {
                try
                {
                    int oii = inputsArrayList.Items.Count - 1;
                    if (pM.inputs[oii].module != null)
                    {
                        MessageBox.Show("Please disconnect input before removing");
                        return;
                    }
                    pM.inputs.RemoveAt(oii);
                    pM.inputnames.RemoveAt(oii);
                    pM.usercodeinputs.RemoveAt(oii);  
                    //inputsArrayList.Items.RemoveAt(inputsArrayList.Items.Count - 1);
                   // winInputNamesFromNode.RemoveAt(winInputNamesFromNode.Count - 1);
                    //winInputsFromNode.RemoveAt(winInputsFromNode.Count - 1);  
                    updateScriptClassFromWindowData(true);
                    // updateScriptClassIOFromWindowData();
                    pM.saveScriptForStorage();
                    pM.resetModuleNumberCalls();
                    pM.forceMyNodeToUpdate();
                    this.Close();

                }
                catch
                {
                    MessageBox.Show("Error removing input");
                }
            }
        }

        private void tableLayoutPanel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void addoutput1_Click_1(object sender, EventArgs e)
        {
            string jl = "";
            DialogResult dl=PUPPIFormUtils.formutils.InputBox("Output name", "Please enter name for new output", ref jl);
            if (dl == DialogResult.Cancel) return;
            if (jl == "") jl = "Output" + winResultOutputNames.Count + 1;
            //winResultOutputNames.Add(jl);
            updateScriptClassFromWindowData(true);
            //updateScriptClassIOFromWindowData();
            pM.outputs.Add("Not Set");
            pM.outputnames.Add(jl);
            pM.saveScriptForStorage();
            pM.resetModuleNumberCalls();
            pM.forceMyNodeToUpdate();
            this.Close();
        }

        private void removeOutput_Click_1(object sender, EventArgs e)
        {
            if (outputsArrayList.Items.Count   > 0)
            {
                try
                {
                    int oii = outputsArrayList.Items.Count - 1;
                    List<PUPPIModule> gm = pM.getDownstreamModules(oii);

                    if (gm.Count > 0)
                    {
                        MessageBox.Show("Please disconnect output before removing");
                        return;
                    }
                    //winResultOutputNames.RemoveAt(outputsArrayList.SelectedIndex);
                    pM.outputs.RemoveAt(oii);
                    pM.outputnames.RemoveAt(oii);
                    pM.usercodeoutputs.RemoveAt(oii);  
                  //  outputsArrayList.Items.RemoveAt(outputsArrayList.Items.Count);
                   // winResultOutputNames.RemoveAt(winResultOutputNames.Count - 1);
                  //  winResultOutputs.RemoveAt(winResultOutputs.Count - 1);
                    updateScriptClassFromWindowData(true);
                    //updateScriptClassIOFromWindowData();
                    pM.saveScriptForStorage();
                    pM.resetModuleNumberCalls();
                    pM.forceMyNodeToUpdate();
                    this.Close();

                }
                catch
                {
                    MessageBox.Show("Error removing output");
                }
            }
        }

        private void renSelInp_Click(object sender, EventArgs e)
        {
            if (inputsArrayList.SelectedIndex >= 0)
            {
                try
                {
                    string jl = "";
                    PUPPIFormUtils.formutils.InputBox("Input name", "Please enter new name for input " + pM.inputnames[inputsArrayList.SelectedIndex], ref jl);
                    if (jl == "") jl = "Input" + pM.inputnames.Count  + 1;
                    pM.inputnames[inputsArrayList.SelectedIndex] = jl;
                    updateScriptClassFromWindowData(true);
                    //updateScriptClassIOFromWindowData();
                    pM.saveScriptForStorage();
                    pM.resetModuleNumberCalls();
                    pM.forceMyNodeToUpdate();
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Error renaming input");  
                }
            }
        }

        private void renLastOut_Click(object sender, EventArgs e)
        {
            if (outputsArrayList.SelectedIndex >= 0)
            {
                try
                {
                    string jl = "";
                    PUPPIFormUtils.formutils.InputBox("Input name", "Please enter new name for output " + pM.outputnames[outputsArrayList.SelectedIndex], ref jl);
                    if (jl == "") jl = "Output" + pM.outputnames.Count + 1;
                    pM.outputnames[outputsArrayList.SelectedIndex] = jl;
                    updateScriptClassFromWindowData(true);
                    //updateScriptClassIOFromWindowData();
                    pM.saveScriptForStorage();
                    pM.resetModuleNumberCalls();
                    pM.forceMyNodeToUpdate();
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Error renaming output");
                }
            }
        }
 
   
            
 
        
    }
}

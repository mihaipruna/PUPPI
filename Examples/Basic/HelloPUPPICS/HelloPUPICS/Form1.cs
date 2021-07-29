using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PUPPIModel; 
//this is required for ArrayList
using System.Collections;

//*******************************************************************************************************************
//PUPPI Hello World Sample for C#
//A basic PUPPI GUI environment for Math operations
//Output can be saved to files
//No Warranty: THE SOFTWARE IS A WORK IN PROGRESS AND IS PROVIDED "AS IS".
//In the PUPPI GUI, you can load examples of visual programs from the Debug/Examples folder.
//http://visualprogramminglanguage.com
//Advanced project samples are available to PUPPI subscribers. Contact us at sales@pupi.co
//*******************************************************************************************************************

namespace HelloPUPPICS
{

    public partial class Form1 : Form
    {
        //code to create an inputbox in C#
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
        //form constructor, this will hold the PUPPI GUI
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //initializes the necessary settings. this is called after programmer makes changes to constants in PUPPIGUISettings
            //this function is required before the canvas is initialized even if no settings were changed
            PUPPIGUI.PUPPIGUISettings.initializeSettings();  
            //creates and adds the PUPPI canvas to one tab page
            PUPPIGUIController.PUPPIProgramCanvas pmc = new PUPPIGUIController.PUPPIProgramCanvas(1000, 460);
            PUPPIGUIController.FormTools.AddPUPPIProgramCanvastoForm(pmc, this, 5, 100);
            //math modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu mathbuttonsmenu = new PUPPIGUIController.PUPPIModuleKeepMenu("Math Functions", 200, 80, 90, 20, 2);
            //add the premade PUPPI Math Modules
            mathbuttonsmenu.AddPUPPIPremadeMathModules();
            //also add functions from the System.Math class
            mathbuttonsmenu.AddMenuButtonList(PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(typeof(System.Math)));
            //add the menu to the form
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(mathbuttonsmenu, this, 0, 20);
            //logic modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu logicmenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("Logic", 60, 80, 50, 20, 1);
            logicmenubuttons.AddPUPPIPremadeLogicModules(0.2, 0.9, 0.5, 1);
            logicmenubuttons.AddMenuButton(new PUPPINOT() as PUPPIModule, 0.2, 0.9, 0.5, 1);
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(logicmenubuttons, this, 200, 20);
            //list modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu listmenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("List Ops", 100, 80, 80, 20, 1);
            listmenubuttons.AddPUPPIPremadeListModules(0.1, 0.9, 0.7, 1);
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(listmenubuttons, this, 260, 20);
            //data input modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu datainputmenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("Input", 90, 80, 70, 20, 1);
            datainputmenubuttons.AddPUPPIPremadeDataInputModules(0.1, 0.9, 0.8, 1);
            //add string input with default colors
            datainputmenubuttons.AddMenuButton(new PUPPIStringInput());   
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(datainputmenubuttons, this, 360, 20);
            //new menu
            PUPPIGUIController.PUPPIModuleKeepMenu outputmenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("Output", 90, 80, 70, 20, 1);
            //add file output module with default colors for button
            outputmenubuttons.AddMenuButton(new PUPPIWriteCSV());
            //add module created automatically from the SimpleConcatStrings method below
            List<Type> methodparmtypes=new List<Type>();
            string typer="";
            //get two string types to generate a parameter list for the SampleStrings method
            methodparmtypes.Add(typer.GetType() );
            methodparmtypes.Add(typer.GetType() );
            
            Type PUPPIscs = PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(SampleStrings)  , "SampleConcatStrings", methodparmtypes);    
            outputmenubuttons.AddMenuButton(System.Activator.CreateInstance(PUPPIscs) as PUPPIModule ); 
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(outputmenubuttons, this, 450, 20);
            
            //file dropdown menu
            PUPPIGUIController.PUPPIDropDownMenu pdmfile = new PUPPIGUIController.PUPPIDropDownMenu(pmc, "File");
            pdmfile.addStandardFileMenuOptions();
            PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(pdmfile, this);
            //edit dropdown menu
            PUPPIGUIController.PUPPIDropDownMenu pdmedit = new PUPPIGUIController.PUPPIDropDownMenu(pmc, "Edit");
            pdmedit.addStandardEditMenuOptions();
            PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(pdmedit, this, 40, 0);
            //view dropdown menu
            PUPPIGUIController.PUPPIDropDownMenu pdmview = new PUPPIGUIController.PUPPIDropDownMenu(pmc, "View");
            pdmview.addStandardViewMenuOptions();
            PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(pdmview, this, 80, 0);
            //help dropdown menu
            PUPPIGUIController.PUPPIDropDownMenu pdmhelp = new PUPPIGUIController.PUPPIDropDownMenu(pmc, "Help");
            pdmhelp.addStandardHelpMenuOptions(Uri.UnescapeDataString(new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath) + @"/PUPPI-user-help.chm");
            PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(pdmhelp, this, 120, 0);
        }
    }
    //custom module: Logical NOT
    public class PUPPINOT : PUPPIModule
    {
        public PUPPINOT()
            //required : call the parent constructor first
            : base()
        {
            //module's name
            name = "NOT";
            //one output defaulted to 0
            outputs.Add(0);
            outputnames.Add("Result");
            //description to show up when hovering over module in menu
            description = "NOT Operation";
            //one input
            inputnames.Add("Input");
            inputs.Add(new PUPPIInParameter());

        }
        //this function controls what the module does when visual program is running
        public override void process_usercode()
        {


            bool b1 = false;

            try
            {
                b1 = Convert.ToBoolean(usercodeinputs[0]);

            }
            catch
            {
                usercodeoutputs[0] = 0;
                return;
            }
            usercodeoutputs[0] = Convert.ToInt16(!b1);




            return;
        }
    }
    //custom module: string input
    public class PUPPIStringInput : PUPPIModule
    {
        //constructor
        public PUPPIStringInput()
            //needs to call base constructor
            : base()
        {
            
            name = "String input";
            outputs.Add(" ");
            outputnames.Add("Value");
        }
        
        //value is set when user double clicks on node
        public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
        {

            string entered = "";
            if (Form1.InputBox("Please enter a string", "Enter text:", ref entered)==DialogResult.OK)  
            usercodeoutputs[0]=entered;     
        
        }


    }
    //custom module: writes number,list of numbers or grid (arraylist of arraylists) to a file
    public class PUPPIWriteCSV : PUPPIModule
    {
        public PUPPIWriteCSV()
            : base()
        {
            name = "Write CSV";
            description = "writes number,list of numbers or grid (arraylist of arraylists) to a comma separated text file";
            //for output, we will display a message 
            outputs.Add("not-run");
            outputnames.Add("Result");
            //multiple types of variables will be accounted for
            inputnames.Add("Item/ArrayList/Grid");
            inputs.Add(new PUPPIInParameter());
            //also the complete file path as string
            inputnames.Add("Output file path");
            inputs.Add(new PUPPIInParameter());
            //we will access inputs and outputs directly
            completeProcessOverride = true;
        }
        //file will get overwritten every time the program runs
        public override void process_usercode()
        {
            
            //if failure of any way, we report it
            //for instance, if not all inputs are connected, etc.
            //with completeprocessoverride enabled we need to do our own error checking
            try
            {
                //get the path. we reference the connected output directly
                string fpath = inputs[1].module.outputs[inputs[1].outParIndex].ToString(); 
                    //check list or grid input
                    if (inputs[0].module.outputs[inputs[0].outParIndex] is ArrayList)
                    {
                        ArrayList orlist = inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList;
                        //2d grid input (Arraylist of arraylists)
                        if (orlist[0] is ArrayList)
                        {
                            //store all rows in a string array
                            int rcount = orlist.Count ;
                            string[] allrows=new string[rcount];
                            for (int rc=0;rc<rcount;rc++)
                            {
                                ArrayList myrow=orlist[rc] as ArrayList;  
                                int ccount = myrow.Count;
                                //write row values separated by comma to array
                                string rowstring="";
                                for (int cc=0;cc<ccount;cc++)
                                {
                                        if (cc!=ccount-1) 
                                        {
                                            rowstring+=myrow[cc].ToString()+",";   
                                        }
                                        else
                                        {
                                            rowstring+=myrow[cc].ToString();
                                        }
                                
                                }
                                allrows[rc]=rowstring;  
                            }

                            System.IO.File.WriteAllLines(fpath, allrows);
                    
                        }
                        //one dimensional generic list
                        else
                        {
                            string singlerow = "";
                            for (int i = 0; i < orlist.Count; i++)
                            {
                                if (i != orlist.Count - 1)
                                {
                                    singlerow += orlist[i].ToString() + ",";  
                                }
                                else
                                {
                                    singlerow += orlist[i].ToString();   
                                }
                            }
                            System.IO.File.WriteAllText(fpath, singlerow);    
                        }
                    }
                    else
                    {
                        //no matter what the input is we convert it to string
                        string text = inputs[0].module.outputs[inputs[0].outParIndex].ToString();    
                            // WriteAllText creates a file, writes the specified string to the file, 
                            // and then closes the file.
                        System.IO.File.WriteAllText(fpath, text);
                    }



                    outputs[0] = "success"; 
            }
            catch
            {
                outputs[0] = "error";
                return;
            }





            return;
        }


    }
    //sample class to show how PUPPI can create Visual Programming Modules from existing methods
    public static class SampleStrings
    {
      
        //simple method concatenating two strings
        public static string SampleConcatStrings(string s1, string s2)
        {
            return s1 + s2;
        }
    }
}
  







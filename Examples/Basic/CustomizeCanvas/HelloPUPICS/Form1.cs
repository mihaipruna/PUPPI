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
//Customizing the appearance of the PUPPI programming canvas
//A basic PUPPI GUI environment with altered graphics settings
//http://visualprogramminglanguage.com
//Advanced  project samples available to PUPPI subscribers. Contact us at sales@pupi.co
//*******************************************************************************************************************



namespace CustomizeCanvas
{

    public partial class Form1 : Form
    {
        //form constructor, this will hold the PUPPI GUI
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //change background color
            PUPPIGUI.PUPPIGUISettings.background_Blue = 0.7;
            //change node color
            PUPPIGUI.PUPPIGUISettings.node_Green = 1;
            //change input color
            PUPPIGUI.PUPPIGUISettings.input_Red = 1;
            //change output color
            PUPPIGUI.PUPPIGUISettings.output_Blue = 1; 
            //wider inputs and outputs to show more text
            PUPPIGUI.PUPPIGUISettings.ioWidth = 0.4;  
            //not showing grid on canvas
            PUPPIGUI.PUPPIGUISettings.showGridOnCanvas = false; 
            //2d canvas
            PUPPIGUI.PUPPIGUISettings.canvasMode = PUPPIGUI.PUPPIGUISettings.CanvasModeEnum.TwoDimensional;    
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
            //text display on custom node
            //empty renderer
            PUPPIGUI.PUPPINodeCustomRenderer myCustomNodeRenderer = new PUPPIGUI.PUPPINodeCustomRenderer("textrender");
            //activate it and apply it to custom PUPPI module
            myCustomNodeRenderer.addRendererToCanvas(pmc);
            myCustomNodeRenderer.applyRendererToModule(typeof(PUPPITextDisplay2D), pmc);
            logicmenubuttons.AddMenuButton(new PUPPITextDisplay2D());  
            
            
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(logicmenubuttons, this, 200, 20);
            //list modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu listmenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("List Ops", 100, 80, 80, 20, 1);
            listmenubuttons.AddPUPPIPremadeListModules(0.1, 0.9, 0.7, 1);
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(listmenubuttons, this, 260, 20);
            //data input modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu datainputmenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("Input", 90, 80, 70, 20, 1);
            datainputmenubuttons.AddPUPPIPremadeDataInputModules(0.1, 0.9, 0.8, 1);
            //add string input with default colors
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(datainputmenubuttons, this, 360, 20);
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

    //display text cell from input string
    public class PUPPITextDisplay2D : PUPPIModule
    {
        public PUPPITextDisplay2D()
            : base()
        {
            name = "Display Text";
            description = "Displays input string on screen";
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("ToDisplay");
            completeProcessOverride = true;
            maxChildren = 0;
        }
        public override void process_usercode()
        {
            string newcap = "";
            if (inputs[0].module != null)
            {
                object os = inputs[0].module.outputs[inputs[0].outParIndex];
                newcap = os.ToString();
            }
            else
            {
                newcap = "null";
            }
            
                PUPPIGUI.PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
                
               
                //instead of updating each component, we simply regenerate since its easier and not much overhead
                pN.clearAll();
            pN.useDefaultCaption = false;
                pN.addTextCell2D(newcap, 1, 0, 0, 0, 0, 0, PUPPIGUI.PUPPIGUISettings.nodeSide, PUPPIGUI.PUPPIGUISettings.nodeSide);  


                
                forceMyNodeToUpdate();
            
        }
    }

   
}
  







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
using System.Windows.Media.Media3D;
using System.Windows.Threading;
//this is required for ArrayList
using System.Collections;
using System.Media;
using PUPPIGUI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

//*******************************************************************************************************************
//sample project for creating a PUPPI HTTP Listeber server and a PUPPI TCP Socket server
//No Warranty: THE SOFTWARE IS A WORK IN PROGRESS AND IS PROVIDED "AS IS".
//see client commands in Getting Started With PUPPI document
//http://visualprogramminglanguage.com
//Advanced client-server project samples available to PUPPI subscribers. Contact us at sales@pupi.co
//*******************************************************************************************************************


namespace TestMTPS
{

    public partial class Form1 : Form
    {

        PUPPIGUI.PUPPICustomRenderer cr;
        PUPPIGUIController.PUPPIProgramCanvas pmc;
        //form constructor, this will hold the PUPPI GUI
        public Form1()
        {
            InitializeComponent();

        }


        private void Form1_Load(object sender, EventArgs e)
        {

            //initializes the necessary settings. this is called after programmer makes changes to constants in PUPPIGUISettings
            //this function is required before the canvas is initialized even if no settings were changed
            PUPPIGUI.PUPPIGUISettings.wireConnectionSize = 2;
            PUPPIGUI.PUPPIGUISettings.drawGestureLine = false;   
       //     PUPPIGUISettings.maxConnPerSpace = 6;
            PUPPIGUI.PUPPIGUISettings.initializeSettings();
            //creates and adds the PUPPI canvas to one tab page
            pmc = new PUPPIGUIController.PUPPIProgramCanvas(1300, 500);
            PUPPIGUIController.FormTools.AddPUPPIProgramCanvastoForm(pmc, this, 5, 100);
            //math modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu mathbuttonsmenu = new PUPPIGUIController.PUPPIModuleKeepMenu("Math Functions", 200, 80, 90, 20, 2, pmc);
            //add the premade PUPPI Math Modules
            mathbuttonsmenu.AddPUPPIPremadeMathModules();
            //also add functions from the System.Math class
            mathbuttonsmenu.AddMenuButtonList(PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(typeof(System.Math)));
            //add the menu to the form
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(mathbuttonsmenu, this, 0, 20);
            //logic modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu logicmenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("Logic", 60, 80, 50, 20, 1, pmc);
            logicmenubuttons.AddPUPPIPremadeLogicModules(0.2, 0.9, 0.5, 1);
            logicmenubuttons.AddPUPPIPremadeDataExchangeModules(0.4, 0.9, 0.5, 1);
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(logicmenubuttons, this, 200, 20);
            //list modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu listmenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("List Ops", 100, 80, 80, 20, 1, pmc);
            listmenubuttons.AddPUPPIPremadeListModules(0.1, 0.9, 0.7, 1);
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(listmenubuttons, this, 260, 20);
            //data input modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu datainputmenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("Input", 90, 80, 70, 20, 1, pmc);
            datainputmenubuttons.AddPUPPIPremadeDataInputModules(0.1, 0.9, 0.8, 1);

            if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
            {
                PUPPINodeCustomRenderer mySliderRenderer = make3DRendererForSlider();
                mySliderRenderer.addRendererToCanvas(pmc);
                mySliderRenderer.applyRendererToModule(typeof(Slider), pmc);
            }
            datainputmenubuttons.AddMenuButton(new Slider(), 1, 0.8, 0.2);


            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(datainputmenubuttons, this, 360, 20);
            //dump other modules
            PUPPIGUIController.PUPPIModuleKeepMenu othermenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("Other", 300, 80, 140, 20, 2, pmc);
            if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
            {
                PUPPINodeCustomRenderer myTextRenderer = makeTextDisplay3DRenderer();
                myTextRenderer.addRendererToCanvas(pmc);
                myTextRenderer.applyRendererToModule(typeof(PUPPITextDisplay), pmc);
            }
            othermenubuttons.AddMenuButton(new PUPPITextDisplay(), 1, 0.8, 0.2);


            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(othermenubuttons, this, 460, 20);


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
            // PUPPIDEBUG.PUPPIDebugger.debugenabled = true;

          string conn = PUPPIServer.PUPPICanvasTCPServer.startServer(pmc, "10.0.0.5",11000, "testpassword", true);   
           // MessageBox.Show(conn);

           conn=PUPPIServer.PUPPICanvasHTTPServer.startServer(pmc, 5309, "testpassword");   
            

        }

        public static PUPPINodeCustomRenderer makeTextDisplay3DRenderer()
        {
            PUPPINodeCustomRenderer textRenderer = new PUPPINodeCustomRenderer("TextDisplayRenderer");
            textRenderer.useDefaultCaption = false;
            textRenderer.addCaption3D("empty", 0, 0, PUPPIGUISettings.nodeHeight * 1 / 4 + PUPPIGUISettings.textRaise / 2, PUPPIGUISettings.nodeSide * 1 / 8, 0, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);
            return textRenderer;
        }


         //makes a custom renderer for the slider node in 3D
        public static PUPPINodeCustomRenderer make3DRendererForSlider()
        {
            PUPPINodeCustomRenderer sliderRenderer = new PUPPINodeCustomRenderer("SliderRenderer");
            //base is a box
            sliderRenderer.addRoundedBox3D(-PUPPIGUISettings.nodeSide / 2, -PUPPIGUISettings.nodeSide / 2, -PUPPIGUISettings.nodeHeight / 2, PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeHeight * 1 / 4, PUPPIGUISettings.nodeSide *0.2,200, 200, 200);

           // sliderRenderer.addRoundedBox3D(-PUPPIGUISettings.nodeSide , -PUPPIGUISettings.nodeSide, -PUPPIGUISettings.nodeHeight / 2, PUPPIGUISettings.nodeSide , PUPPIGUISettings.nodeSide , PUPPIGUISettings.nodeHeight * 1 / 4, PUPPIGUISettings.nodeSide * 0.2, 200, 200, 200);

            
            
            ////add slider support right
            //sliderRenderer.addBox3D(PUPPIGUISettings.nodeSide * 1 / 8, -PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeHeight * 1 / 4, PUPPIGUISettings.nodeSide * 3 / 8, PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeHeight * 1 / 4 + PUPPIGUISettings.textRaise, 255, 255, 255);
            //create illusion of slider channel
            sliderRenderer.addBox3D(-PUPPIGUISettings.nodeSide * 1 / 8, -PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeHeight * 1 / 4, PUPPIGUISettings.nodeSide * 1 / 8, PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeHeight * 1 / 4 + PUPPIGUISettings.textRaise / 2, 150, 150, 150);
            //the slider itself in 3d is a red sphere. initially in the center
            sliderRenderer.addSphere3D(0, 0, PUPPIGUISettings.nodeHeight * 1 / 4, PUPPIGUISettings.nodeSide * 1 / 16, 255, 0, 0);
            //disable default caption
            sliderRenderer.useDefaultCaption = false;
            //now add captions: lower limit, upper limit and value
            //should match the values in default Slider module initialization
            sliderRenderer.addCaption3D("0", PUPPIGUISettings.nodeSide * 1 / 4, -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeSide * 1 / 16, PUPPIGUISettings.nodeHeight * 1 / 4 + PUPPIGUISettings.textRaise / 2, PUPPIGUISettings.nodeSide * 1 / 8, 0, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);
            sliderRenderer.addCaption3D("1", PUPPIGUISettings.nodeSide * 1 / 4, PUPPIGUISettings.nodeSide / 2 - PUPPIGUISettings.nodeSide * 1 / 16, PUPPIGUISettings.nodeHeight * 1 / 4 + PUPPIGUISettings.textRaise / 2, PUPPIGUISettings.nodeSide * 1 / 8, 0, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);
            sliderRenderer.addCaption3D("0.5", PUPPIGUISettings.nodeSide * 1 / 4, 0, PUPPIGUISettings.nodeHeight * 1 / 4 + PUPPIGUISettings.textRaise / 2, PUPPIGUISettings.nodeSide * 1 / 8, 0, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);


            return sliderRenderer;

        }
    
    public class Slider : PUPPIModule
    {
        double resultValue = 0;
        double lowerLimit = 0;
        double upperLimit = 1;
        public Slider()
            : base()
        {

            resultValue = 0.5;
            lowerLimit = 0;
            upperLimit = 1;
            name = "Slider";
            outputs.Add(resultValue);
            outputnames.Add("Result");
            description = "Drag Slider to set value. Set limits by double clicking";
            maxChildren = 0;
            completeDoubleClickOverride = true;
            completeGestureOverride = true;
            completeProcessOverride = true; 
            doubleClickDescription = "Set limits";
            gestureDescription = "Move slider";


        }


        //this function is called when gestures or double clicks are applied to set and validate outputs
        public void updateMe(double rv)
        {

            //scale result to determine position
            double dp = 0;
            dp = (rv - lowerLimit) / (upperLimit - lowerLimit);


            //update the node visually with the new Slider position
            //need to update so that Slider stays within linits of the base box
            //also update captions
            if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
            {
                PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
                //make sure we don't go off base, then the bounding box will update and the box will look displaced
                pN.updateSphere3D(0, 0, -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeSide * 0.126 / 2 + PUPPIGUISettings.nodeSide * dp * (1 - 0.126), PUPPIGUISettings.nodeHeight * 0.25, PUPPIGUISettings.nodeSide * 0.125 / 2, 255, 0, 0);

                pN.updateCaption3D(0, lowerLimit.ToString(), pN.captions3D[0].myCaptionPosition.X, pN.captions3D[0].myCaptionPosition.Y, pN.captions3D[0].myCaptionPosition.Z, pN.captions3D[0].myCaptionTextHeight, 0, 0, 0, pN.captions3D[0].myCaptionDir, pN.captions3D[0].myCaptionOrient);
                pN.updateCaption3D(1, upperLimit.ToString(), pN.captions3D[1].myCaptionPosition.X, pN.captions3D[1].myCaptionPosition.Y, pN.captions3D[1].myCaptionPosition.Z, pN.captions3D[1].myCaptionTextHeight, 0, 0, 0, pN.captions3D[1].myCaptionDir, pN.captions3D[1].myCaptionOrient);
                pN.updateCaption3D(2, rv.ToString(), pN.captions3D[2].myCaptionPosition.X, -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeSide * 0.126 / 2 + PUPPIGUISettings.nodeSide * dp * (1 - 0.126), pN.captions3D[2].myCaptionPosition.Z, pN.captions3D[2].myCaptionTextHeight, 0, 0, 0, pN.captions3D[2].myCaptionDir, pN.captions3D[2].myCaptionOrient);

            }

            forceMyNodeCustomRendererToUpdate();


        }
        //dragging down and up sets the result sets the 
        public override void gestureMe_userCode(double startXRatio, double startYRatio, double startZRatio, double endXRatio, double endYRatio, double endZRatio)
        {
            if (endYRatio < -1) endYRatio = -1;
            if (endYRatio > 1) endYRatio = 1;
            resultValue = Math.Round(lowerLimit + (upperLimit - lowerLimit) * endYRatio, 2);
            //this is to update the node visually when gestureMe_usercode called from other functions or clients
            updateMe(resultValue );
            outputs[0] = resultValue;
        }
        //live updated while dragging
        public override void dragOver_visualUpdate_usercode(double startXRatio, double startYRatio, double startZRatio, double currentXRatio, double currentYRatio, double currentZRatio)
        {
            if (currentYRatio < -1) currentYRatio = -1;
            if (currentYRatio > 1) currentYRatio = 1;
            double rm = Math.Round(lowerLimit + (upperLimit - lowerLimit) * currentYRatio, 2);
            updateMe(rm);

        }
        public override void process_usercode()
        {
            updateMe(resultValue);
            outputs[0] = resultValue;
        }


//save value and limits
        public override string saveSettings()
        {
            return lowerLimit.ToString() + "__" + upperLimit.ToString() + "__" + resultValue.ToString();
        }
        //load them from file qand update node 
        public override void initOnLoad(string savedSettings)
        {
            string[] sep = { "__" };
            string[] vals = savedSettings.Split(sep, StringSplitOptions.None);
            lowerLimit = Math.Round(Convert.ToDouble(vals[0]), 2);
            upperLimit = Math.Round(Convert.ToDouble(vals[1]), 2);
            resultValue = Math.Round(Convert.ToDouble(vals[2]), 2);
        }
        //set the value and the Slider position numerically
        public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
        {
            string LLinputBoxResult = "";
            DialogResult dL = PUPPIFormUtils.formutils.InputBox("Please enter lower limit", "Set Slider Lower Limit", ref LLinputBoxResult);

            string ULinputBoxResult = "";
            DialogResult dU = PUPPIFormUtils.formutils.InputBox("Please enter upper limit", "Set Slider Upper Limit", ref ULinputBoxResult);


            if (dL != DialogResult.Cancel && dU != DialogResult.Cancel)
            {

                //validation
                try
                {
                    lowerLimit = Math.Round(Convert.ToDouble(LLinputBoxResult), 2);
                    upperLimit = Math.Round(Convert.ToDouble(ULinputBoxResult), 2);
                    if (upperLimit == lowerLimit) upperLimit = lowerLimit + 1.0;  
                    if (upperLimit < lowerLimit)
                    {

                        double swapper = lowerLimit;
                        lowerLimit = swapper;
                        upperLimit = swapper;
                    }
                    if (resultValue > upperLimit)
                    {
                        resultValue = upperLimit;
                    }
                    if (resultValue < lowerLimit)
                    {
                        resultValue = lowerLimit;
                    }
                }
                catch
                {
                    //some default values
                    if (resultValue != 0)
                    {
                        lowerLimit = resultValue * 0.5;
                        upperLimit = resultValue * 1.5;
                    }
                    else
                    {
                        lowerLimit = -1;
                        upperLimit = 1;
                    }
                }
                updateMe(resultValue);
            }

        }
     
    }
    //displays a text sent by input
    public class PUPPITextDisplay : PUPPIModule
    {
        public PUPPITextDisplay()
            : base()
        {
            name = "Text AutoSize";
            description = "Displays input string on screen to fit in node footprint";
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
            if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
            {
                PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
                //size according to number of lines
                //int nl = newcap.Split('\n').Length;
                //if (nl == 0) nl = 1;

                pN.updateCaption3D(0, newcap, 0, 0, PUPPIGUISettings.nodeHeight * 1 / 4 + PUPPIGUISettings.textRaise / 2, PUPPIGUISettings.nodeSide * 1.6 / newcap.Length, 0, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);
                //make sure no caption

                forceMyNodeToUpdate();
            }
        }
    }
     
    }

}








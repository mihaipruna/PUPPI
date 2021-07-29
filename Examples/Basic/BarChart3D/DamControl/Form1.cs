using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PUPPIGUI;
using PUPPIModel;
//this is required for ArrayList
using System.Collections;
//************************************************************************************************
//PUPPI Custom Node Appearance and Control Widget creation Sample for C#
//Levers set values on a bar chart displayed in 3D on the programming canvas
//Output can be saved to files
//Also shows tablet mode
//In the PUPPI GUI, you can load examples of visual programs from the Debug\Examples folder.
//http://visualprogramminglanguage.com
//Advanced project samples are available to PUPPI subscribers. Contact us at sales@pupi.co
//************************************************************************************************

namespace PUPPIBarChart3DDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //these help on a tablet
            PUPPIGUISettings.addNodeOnModuleDoubleClick = true;
            PUPPIGUISettings.doubleClickIOConnect = true;  

            PUPPIGUISettings.nodeHeight = 0.25;
            PUPPIGUISettings.ioHeight = 0.1;
            //bigger input and output boxes to easier hit them on a tablet
            PUPPIGUISettings.ioLength = 0.4;
            PUPPIGUISettings.ioMinSpacing = 0.05;
            PUPPIGUISettings.ioWidth = 0.25;  
           //blue-ish background
            PUPPIGUISettings.background_Blue = 1;
            PUPPIGUISettings.background_Red = 0.95;
            PUPPIGUISettings.background_Green = 0.99;
            PUPPIGUISettings.nodeSelected_Red = 1;
            PUPPIGUISettings.tabletMode = true;  
            //thicker connections
            //PUPPIGUISettings.solidConnectionSize = 0.05; 
            PUPPIGUISettings.wireConnectionSize = 3;
            PUPPIGUISettings.windowButtonHeight = 18;  
            //chamfered nodes
            PUPPIGUISettings.chamferCornerDepth = 2; 
            //brownish translucent material for default nodes
            PUPPIGUISettings.node_Green = 0.8;
            PUPPIGUISettings.node_Red = 0.8;
            PUPPIGUISettings.node_Blue = 0.1;  
            PUPPIGUISettings.node_Alpha = 0.9;

            //PUPPIGUISettings.canvasMode = PUPPIGUISettings.CanvasModeEnum.TwoDimensional;     
            //bigger menu font
            PUPPIGUISettings.dropDownMenuFontSize = 16; 
            //initializes the necessary settings. this is called after programmer makes changes to constants in PUPPIGUISettings
            PUPPIGUISettings.initializeSettings();
            //MessageBox.Show("init");  
            //creates and adds the PUPPI canvas to one tab page
            PUPPIGUIController.PUPPIProgramCanvas pmc = new PUPPIGUIController.PUPPIProgramCanvas(975, 570);
            PUPPIGUIController.FormTools.AddPUPPIProgramCanvastoForm(pmc, this, 140, 110);
            //MessageBox.Show("add");
            //add some PUPPI modules
            PUPPIGUIController.PUPPIModuleKeepMenu mathbuttonsmenu = new PUPPIGUIController.PUPPIModuleKeepMenu("Math Functions", 1240, 80, 170, 40,6);
            //add the premade PUPPI Math Modules
            mathbuttonsmenu.AddPUPPIPremadeMathModules();
            //also add cosine
            List<Type> cosParams=new List<Type>();
            cosParams.Add(typeof(double) );
            cosParams.Add(typeof(double) );
            Type cosPUPPIModuleType=PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(Math), "Cos", cosParams);   
            mathbuttonsmenu.AddMenuButton(PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(cosPUPPIModuleType )   );
           //add the menu to the form
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(mathbuttonsmenu, this, 0, 30);

            //list functions added to the left side of the screen
            PUPPIGUIController.PUPPIModuleKeepMenu listbuttonsmenu = new PUPPIGUIController.PUPPIModuleKeepMenu("List Functions", 140, 570, 130, 35, 1);
            listbuttonsmenu.AddPUPPIPremadeListModules();
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(listbuttonsmenu, this, 0, 110);


            //add custom renderer to Lever module
            PUPPINodeCustomRenderer leverRenderer = new PUPPINodeCustomRenderer("LeverRender");
            //the Lever, two cylinders
            //leverRenderer.addSphere3D(0, 0, PUPPIGUISettings.nodeHeight * 1 / 4, PUPPIGUISettings.nodeSide/8, 255, 0, 0);
            leverRenderer.addPipe3D(-PUPPIGUISettings.nodeSide / 8, 0, PUPPIGUISettings.nodeHeight, PUPPIGUISettings.nodeSide / 8, 0, PUPPIGUISettings.nodeHeight,PUPPIGUISettings.nodeHeight * 0.5, 255, 0, 0);
            leverRenderer.addPipe3D(0, 0, 0, 0, 0, PUPPIGUISettings.nodeHeight, PUPPIGUISettings.nodeHeight * 0.5, 255, 0, 0); 
            //base is a box
            leverRenderer.addBox3D(-PUPPIGUISettings.nodeSide / 2, -PUPPIGUISettings.nodeSide / 2, -PUPPIGUISettings.nodeHeight / 2, PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeHeight *1/4, 200, 200, 200);
            //add slider support right
            leverRenderer.addBox3D(PUPPIGUISettings.nodeSide * 1 / 8, -PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeHeight * 1 / 4, PUPPIGUISettings.nodeSide * 3 / 8, PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeHeight * 1 / 4 + PUPPIGUISettings.textRaise, 255, 255, 255); 
            //create illusion of slider channel
            leverRenderer.addBox3D(-PUPPIGUISettings.nodeSide * 1 / 8, -PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeHeight * 1 / 4, PUPPIGUISettings.nodeSide * 1 / 8, PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeHeight * 1 / 4 + PUPPIGUISettings.textRaise / 2, 150, 150, 150); 
            //disable default caption
            leverRenderer.useDefaultCaption = false;
            //add image showing range. image needs to be in same folder as executable
            leverRenderer.addImage3D(@".\ruler.jpg", -PUPPIGUISettings.nodeSide * 2 / 8, 0, PUPPIGUISettings.nodeHeight * 1 / 4+PUPPIGUISettings.textRaise ,  PUPPIGUISettings.nodeSide,PUPPIGUISettings.nodeSide/4 , PUPPICustomRenderer.PUPPIimageDirection.negativeYDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);
            leverRenderer.addRendererToCanvas(pmc);
            leverRenderer.applyRendererToModule(typeof(Lever), pmc);
            //apply blank renderer to Bar Chart module
            PUPPINodeCustomRenderer chartRenderer = new PUPPINodeCustomRenderer("BarChart3DRenderer");
            //needs an object in the renderer to know it is used
            chartRenderer.useDefaultCaption = false;
            chartRenderer.addCaption3D("Bar Plot", 0, -PUPPIGUISettings.nodeSide / 2, 0, PUPPIGUISettings.nodeSide / 3, 0, 0, 255, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);
            chartRenderer.addRendererToCanvas(pmc);  
            chartRenderer.applyRendererToModule(typeof(BarPlot3D), pmc);
            //plot renderer with background elements that won't change
            PUPPINodeCustomRenderer linePlotRenderer = new PUPPINodeCustomRenderer("LinePlot3DRenderer");
            linePlotRenderer.useDefaultCaption = false;
            linePlotRenderer.addCaption3D("Line Plot", 0, -PUPPIGUISettings.nodeSide / 2, 0, PUPPIGUISettings.nodeSide / 3, 0, 0, 255, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);
            // a white background
            linePlotRenderer.addBox3D(-PUPPIGUISettings.nodeSide / 2, -0.001, 0, PUPPIGUISettings.nodeSide / 2, 0.001, 4 * PUPPIGUISettings.nodeSide, 255, 255, 255);
            linePlotRenderer.addRendererToCanvas(pmc);
            linePlotRenderer.applyRendererToModule(typeof(LinePlotVertical3D ),pmc );  
            //menu for widgets (slider and plot in this case)
            PUPPIGUIController.PUPPIModuleKeepMenu widgetmenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("Widgets", 135, 570, 130, 35, 1);
            widgetmenubuttons.AddMenuButton(new BarPlot3D() as PUPPIModule);
            widgetmenubuttons.AddMenuButton(new Lever() as PUPPIModule);
            widgetmenubuttons.AddMenuButton(new LinePlotVertical3D() as PUPPIModule);
            //blank base so we can group our widgets and functions nicely
            widgetmenubuttons.AddMenuButton(new PUPPIModel.PUPPIPremadeModules.DataInputModules.PUPPIBlank() as PUPPIModule);
             PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(widgetmenubuttons, this, 1115, 110);
            //MessageBox.Show("menus");  
            //add small origin under canvas
            pmc.updateCanvasVisualElements(makeTestCanvasStaticVisual());
            //MessageBox.Show("visual");  
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
            PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(pdmhelp, this, 130, 0);
            //MessageBox.Show("dropdowns");  
        }
        //canvas center
        public PUPPICustomRenderer makeTestCanvasStaticVisual()
        {
            PUPPICustomRenderer cr = new PUPPICustomRenderer();
            cr.addSphere3D(0, 0, -0.5, 0.1, 240, 240, 255);
            cr.addBox3D(0, -0.05, -0.55, 0.5, 0.05, -0.45, 255, 200, 200);
            cr.addBox3D(-0.05, 0, -0.55, 0.05, 0.5, -0.45, 200, 255, 200);
            cr.addBox3D(-0.05, -0.05, -0.5, 0.05, 0.05, 0, 200, 200, 255);
            return cr;
        }
        //when the user drags up and down, the slider value updates from 0 to 1
        public class Lever : PUPPIModule
        {
            double resultValue = 0;
            
            public Lever()
                : base()
            {
                completeDoubleClickOverride = true;
                completeProcessOverride = true;
                completeGestureOverride = true; 
                resultValue = -1;
                name = "Lever";
                outputs.Add(resultValue);
                outputnames.Add("Result 0-1");
                description = "Drag Lever or set value between 0-1 by double clicking";
                //can't put anything on top of it because it would not let the slider work
                maxChildren = 0; 



            }
            public override void process_usercode()
            {
                //uninitialized
                if (resultValue==-1)
                {
                    //get from default outputs
                    if (defaultoutputs.Count>0  )
                    {
                        resultValue = Convert.ToDouble(defaultoutputs[0]);  
                    }
                }
                if (resultValue < 0) resultValue = 0;
                if (resultValue > 1) resultValue = 1;
                //update the node visually with the new lever position
                //the top pipe slides back and forth while the connecting pipe goes from the center of the top pipe to the relative origin
                PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
                //update so that lever stays within linits of the base box
                if (-PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeSide * resultValue > -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeHeight * 0.5 && -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeSide * resultValue < PUPPIGUISettings.nodeSide / 2 - PUPPIGUISettings.nodeHeight * 0.5)
                {
                    pN.updatePipe3D(0, -PUPPIGUISettings.nodeSide / 8, -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeSide * resultValue, PUPPIGUISettings.nodeHeight, PUPPIGUISettings.nodeSide / 8, -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeSide * resultValue, PUPPIGUISettings.nodeHeight, PUPPIGUISettings.nodeHeight * 0.5, 255, 0, 0);
                    pN.updatePipe3D(1, 0, 0, 0, 0, -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeSide * resultValue, PUPPIGUISettings.nodeHeight, PUPPIGUISettings.nodeHeight * 0.5, 255, 0, 0);
                }
                else if (-PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeSide * resultValue < -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeHeight * 0.5)
                {
                    pN.updatePipe3D(0, -PUPPIGUISettings.nodeSide / 8, -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeHeight * 0.5, PUPPIGUISettings.nodeHeight, PUPPIGUISettings.nodeSide / 8, -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeHeight * 0.5, PUPPIGUISettings.nodeHeight, PUPPIGUISettings.nodeHeight * 0.5, 255, 0, 0);
                    pN.updatePipe3D(1, 0, 0, 0, 0, -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeHeight * 0.5, PUPPIGUISettings.nodeHeight, PUPPIGUISettings.nodeHeight * 0.5, 255, 0, 0);

                }
                else
                {
                    pN.updatePipe3D(0, -PUPPIGUISettings.nodeSide / 8, PUPPIGUISettings.nodeSide / 2 - PUPPIGUISettings.nodeHeight * 0.5, PUPPIGUISettings.nodeHeight, PUPPIGUISettings.nodeSide / 8, PUPPIGUISettings.nodeSide / 2 -PUPPIGUISettings.nodeHeight * 0.5, PUPPIGUISettings.nodeHeight, PUPPIGUISettings.nodeHeight * 0.5, 255, 0, 0);
                    pN.updatePipe3D(1, 0, 0, 0, 0, PUPPIGUISettings.nodeSide / 2 - PUPPIGUISettings.nodeHeight * 0.5, PUPPIGUISettings.nodeHeight, PUPPIGUISettings.nodeHeight * 0.5, 255, 0, 0);

                }
                forceMyNodeToUpdate(); 
                    //pN.updateSphere3D(0, 0, -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeSide * resultValue, PUPPIGUISettings.nodeHeight * 1 / 4, PUPPIGUISettings.nodeSide/8, 255, 0, 0);
                outputs[0] = resultValue;
               
                
                
            }
            //dragging down and up sets the result sets the 
            public override void gestureMe_userCode(double startXRatio, double startYRatio, double startZRatio, double endXRatio, double endYRatio, double endZRatio)
            {
                resultValue = endYRatio;
                process_usercode(); 
            }
            //set the value and the Lever position numerically
            public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
            {
                string inputBoxResult = "";
                DialogResult d=PUPPIFormUtils.formutils.InputBox("Please enter value between 0-1","Set Lever", ref inputBoxResult);
                if (d!=DialogResult.Cancel )
                {
                   

                    try
                    {
                        resultValue = Convert.ToDouble(inputBoxResult);
                    }
                    catch
                    {

                    }
                    process_usercode(); 
                }
            }
        }
        //displays a bar chart
        //takes a list of numbers and vertical range min and max as input and displays a vertical bar for each entry
        public class BarPlot3D:PUPPIModule
        {
            double yMin = 0;
            double yMax = 1;
           public BarPlot3D()
               :base()
            {
               //vertical range inputs
                PUPPIInParameter yMinInput = new PUPPIInParameter();
                yMinInput.isoptional = true;  
                inputs.Add(yMinInput);
                inputnames.Add("Y Min");
                PUPPIInParameter yMaxInput = new PUPPIInParameter();
                yMaxInput.isoptional = true;
                inputs.Add(yMaxInput);
                inputnames.Add("Y Max");
                //list of umbers
                inputs.Add(new PUPPIInParameter());
                inputnames.Add("Numbers ArrLst");  
                name = "BarPlot";
                description = "Displays a 3D Bar chart of a list of numbers";
               //can't put anything on top of it
                maxChildren = 0; 
                 
            }
            //redraw the chart
           public override void process_usercode()
           {
               if (usercodeinputs[0] != null)
               {
                   yMin = Convert.ToDouble(usercodeinputs[0]);
               }
               if (usercodeinputs[1] != null)
               {
                   yMax = Convert.ToDouble(usercodeinputs[1]);
               }
               if (yMax<yMin)
               {
                   yMin = 0;
                   yMax = 1;
               }
               PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
               //when loading from file, the renderer is not yet initialized
               if (pN != null)
               {
                   ArrayList numInputs = usercodeinputs[2] as ArrayList;

                   //will clear off all the bars and redraw
                   while (pN.boxes3D.Count > 0)
                   {
                       pN.removeBox3D(0);
                   }
                   if (numInputs != null)
                   {
                       double boxWidth = 4 * (PUPPIGUISettings.nodeSide - 2 * PUPPIGUISettings.ioLength) / numInputs.Count;
                       double xPos = -boxWidth * numInputs.Count / 2 + PUPPIGUISettings.ioLength;
                       for (int i = 0; i < numInputs.Count; i++)
                       {
                           double num = Convert.ToDouble(numInputs[i]);
                           //box dimensions and positions

                           double xStart = xPos + boxWidth * i;
                           double boxHeight = 4*Math.Max(Math.Min(yMax - yMin, num - yMin), 0);
                           pN.addBox3D(xStart, -boxWidth / 2, 0, xStart + boxWidth, boxWidth / 2, boxHeight, Convert.ToByte(Math.Min(Convert.ToInt16(Math.Abs(255 * (num - yMin) / (yMax - yMin))), Convert.ToInt16(255))), 0, Convert.ToByte(Math.Min(Convert.ToInt16(Math.Abs((255 * (yMax - num) / (yMax - yMin)))), Convert.ToInt16(255))));
                       }
                   }
                   forceMyNodeToUpdate(); 
               }

           }
        }
        //takes a list of numbers and vertical range min and max as input and displays a line plot in the vertical plane
        public class LinePlotVertical3D : PUPPIModule
        {
            double yMin = 0;
            double yMax = 1;
            public LinePlotVertical3D()
                : base()
            {
                //vertical range inputs
                PUPPIInParameter yMinInput = new PUPPIInParameter();
                yMinInput.isoptional = true;
                inputs.Add(yMinInput);
                inputnames.Add("Y Min");
                PUPPIInParameter yMaxInput = new PUPPIInParameter();
                yMaxInput.isoptional = true;
                inputs.Add(yMaxInput);
                inputnames.Add("Y Max");
                //list of umbers
                inputs.Add(new PUPPIInParameter());
                inputnames.Add("Numbers ArrLst");
                name = "LinePlot";
                description = "Displays a Vertical Plane line plot of numbers";
                //can't put anything on top of it because it would intersect
                maxChildren = 0; 

            }
            //redraw the chart
            public override void process_usercode()
            {
                if (usercodeinputs[0] != null)
                {
                    yMin = Convert.ToDouble(usercodeinputs[0]);
                }
                if (usercodeinputs[1] != null)
                {
                    yMax = Convert.ToDouble(usercodeinputs[1]);
                }
                if (yMax < yMin)
                {
                    yMin = 0;
                    yMax = 1;
                }
                PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
                //when loading from file, the renderer is not yet initialized
                if (pN != null)
                {
                    ArrayList numInputs = usercodeinputs[2] as ArrayList;

                    //will clear off all the pipes and the background and redraw
                    while (pN.pipes3D.Count > 0)
                    {
                        pN.removePipe3D(0);
                    }
                    while (pN.boxes3D.Count>0  )
                    {
                        pN.removeBox3D(0); 
                    }
                    //remove all text but the caption
                    while (pN.captions3D.Count > 1)
                    {
                        pN.removeCaption3D(pN.captions3D.Count-1);
                    }

                    if (numInputs != null)
                    {
                        double xDisplace = 4 * (PUPPIGUISettings.nodeSide - 2 * PUPPIGUISettings.ioLength) / numInputs.Count;
                        double xPos = -xDisplace * numInputs.Count / 2 + PUPPIGUISettings.ioLength;
                        for (int i = 1; i < numInputs.Count; i++)
                        {
                            //previous and current 
                            double pnum = Convert.ToDouble(numInputs[i-1]);
                            double num = Convert.ToDouble(numInputs[i]);
                           double pyPos = 4 * Math.Max(Math.Min(yMax - yMin, pnum - yMin), 0)/(yMax-yMin)  ;
                           double yPos = 4 * Math.Max(Math.Min(yMax - yMin, num - yMin), 0) / (yMax - yMin);
                           
                            pN.addPipe3D(xPos + (i - 1) * xDisplace, 0, pyPos, xPos + i * xDisplace, 0, yPos, 0.01,Convert.ToByte ( Math.Min(Convert.ToInt16(Math.Abs(255 * (num - yMin) / (yMax - yMin))), Convert.ToInt16(255))), 0, Convert.ToByte (Math.Min(Convert.ToInt16(Math.Abs((255 * (yMax - num) / (yMax - yMin)))), Convert.ToInt16(255))));
                        }
                    //white background
                        pN.addBox3D(xPos, -0.001, 0, xPos+numInputs.Count * xDisplace, 0.001, 4 , 255, 255, 255);
                    //plot labels
                        
                        //find extremes and zeros
                        int maxin = numInputs.Count - 1;
                        int minin = 0;
                        int zeroin = numInputs.Count / 2;
                        double maxval = double.MinValue;
                        double minval = double.MaxValue;
                        double zeroval = double.MaxValue; 
                        for (int i = 0; i < numInputs.Count; i++)
                        {
                            double num = Math.Round(Convert.ToDouble(numInputs[i]), 2);
                            if (num>maxval)
                            {
                                maxval = num;
                                maxin = i;
                            }
                            if (num < minval)
                            {
                                minval = num;
                                minin = i;
                            }
                            if (Math.Abs(num)<zeroval )
                            {
                                zeroval = num;
                                zeroin = i;
                            }

                        }

                        for (int i = 0; i < numInputs.Count; i++)
                        {
                            //previous and current 
                           
                            double num = Math.Round( Convert.ToDouble(numInputs[i]),2);
                            double yPos = 4 * Math.Max(Math.Min(yMax - yMin, num - yMin), 0) / (yMax - yMin);
                            if (i==minin || i==maxin || i==zeroin   )
                            pN.addCaption3D("(" + (xPos + i * xDisplace).ToString() + "," + num.ToString() + ")", xPos + i * xDisplace, -0.02, yPos, 0.05, Convert.ToByte(Math.Min(Convert.ToInt16(Math.Abs(255 * (num - yMin) / (yMax - yMin))), Convert.ToInt16(255))), 0, Convert.ToByte (Math.Min(Convert.ToInt16(Math.Abs((255 * (yMax - num) / (yMax - yMin)))), Convert.ToInt16(255))), PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.vertical);
                        }
                       
                    }

                    forceMyNodeToUpdate();
                }

            }
        }
    }
}

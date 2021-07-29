/* StarMath Library
 The MIT License (MIT)

Copyright (c) 2015 DesignEngrLab

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/

/* NCalc
 * 
 * The MIT License (MIT)
Copyright (c) 2011 Sebastien Ros

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

/*Helix Toolkit
 * 
 * The MIT License (MIT)

Copyright (c) 2012 Helix Toolkit contributors

Permission is hereby granted, free of charge, to any person obtaining a
copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * */
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration; //Not so Given.
using PUPPIGUI;
using PUPPIModel;
using HelixToolkit.Wpf;
using System.Reflection;
using PUPPICAD;
using System.Windows.Media.Media3D;
using System.Diagnostics;



namespace PUPPICADBeta
{
    public partial class Form1 : Form
    {
        //two main view controllers
        PUPPIGUIController.PUPPICADView pvc = null;
        PUPPIGUIController.PUPPIProgramCanvas pmc = null;
        Loading splashS;
        public Form1()
        {
            //splash screen
            splashS = new Loading();
            splashS.Show();
            InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //UPIGUISettings.numberSplineSegments = 8;  

            //PUPPIGUISettings.useMultiThreading = false; 
            // PUPPIGUISettings.smoothConnections =false;  
            // PUPPIGUISettings.roundedCornerRadius = 0;
            // PUPPIGUISettings.roundedConnRadius = 0.33;  
            // PUPPIGUISettings.roundedConnRadius = 0;
            // PUPPIGUISettings.connectionRenderMode = PUPPIGUISettings.ConnModeEnum.Solid;
            // PUPPIGUISettings.roundedConnRadius = 0;
            // PUPPIGUISettings.removeOverlaps = false;
            // PUPPIGUISettings.offsetSmallConnections = false;
            // PUPPIGUISettings.connectionFreeCorners = true;  
          //  PUPPIGUISettings.popInputsOutputs = false;  
            PUPPIGUISettings.addNodeOnModuleDoubleClick = true;
           // PUPPIGUISettings.splineConnections = false; 
            // PUPPIGUISettings.connectionFreeCorners = true;
            //PUPPIGUISettings.drawLineWhenConnecting = false;  
           // PUPPIGUISettings.canvasMode = PUPPIGUISettings.CanvasModeEnum.TwoDimensional;
            //PUPPIGUI.PUPPIGUISettings.ioWidth = 0.4;
            //PUPPIGUISettings.offsetSmallConnections = false;  
           //PUPPIGUISettings.connRaise = 0.1;
            PUPPIGUISettings.grid_Alpha = 0.1;
            PUPPIGUISettings.node_Green = 0.9;
            PUPPIGUISettings.node_Blue = 0.8;
            PUPPIGUISettings.node_Red = 0.8;
            PUPPIGUISettings.wireConnectionSize = 2;
            PUPPIGUISettings.maxConnPerSpace = 6;
            //PUPPIGUISettings.displayStatusOnCanvas = false;  
            //PUPPIGUISettings.displayPanButtons = true;  
            PUPPIGUISettings.cameraFarPlaneDistance = 1000;
            PUPPIGUISettings.doubleClickIOConnect = true;
          //  PUPPIGUISettings.splineConnections = false;  
            //   PUPPIGUISettings.tabletMode = true;  
            //PUPPIGUISettings.showGridOnCanvas = false;  
            PUPPIGUISettings.drawGestureLine = false;  
            if (Properties.Settings.Default.extraButtonsMode=="laptop")
            {
                PUPPIGUISettings.displayCameraControlButtons = true; 
            }

            if (Properties.Settings.Default.extraButtonsMode == "tablet")
            {
                PUPPIGUISettings.tabletMode = true;
            }
            //PUPPIGUISettings.displayInfoHover = false; 
            //pre Initialization to make sure the GUI settings are not going to cause errors
            PUPPIGUISettings.initializeSettings();
            //creates and adds the PUPPI canvas to one tab page
            pmc = new PUPPIGUIController.PUPPIProgramCanvas(10, 10);//(1230, 530);
            //add background text as non-interacting visual under nodes . will display "PUPPICAD"
            pmc.updateCanvasVisualElements(canvasRenderer.CanvasRenderHelper.makeTestCanvasStaticVisual());
            PUPPIGUIController.FormTools.AddPUPPIProgramCanvastoForm(pmc, this, 0, 0);
            this.PUPPIcanvaspage.Controls.Add(pmc.myFormControl);
            //autosize
            pmc.myFormControl.Dock = DockStyle.Fill;



            //custom renderer
            PUPPINodeCustomRenderer pcr = makeTestCustomRenderer1();
            pcr.addRendererToCanvas(pmc);
            pcr.applyRendererToModule(typeof(PUPPINOT), pmc);

            //creates and adds a PUPPI cad model veiw to another tab page
            pvc = new PUPPIGUIController.PUPPICADView(1230, 530);
            PUPPIGUIController.FormTools.AddPUPPICADViewToForm(pvc, this, 0, 0);
            //autosize
            pvc.myFormControl.Dock = DockStyle.Fill;
            this.PUPPIcad3dviewpage.Controls.Add(pvc.myFormControl);
            //math modules menu
            //window size on these doesn't matter much because they will auto-size with their parent component (element host) using Dock.Fill
            PUPPIGUIController.PUPPIModuleKeepMenu mathbuttonsmenu = new PUPPIGUIController.PUPPIModuleKeepMenu("Math Functions", 200, 80, 100, 25, 4, pmc,"Basic mathematical operations");
            //add the premade PUPPI Math Modules
            mathbuttonsmenu.AddPUPPIPremadeMathModules();
            //also add functions from the System.Math class
            mathbuttonsmenu.AddMenuButtonList(PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(typeof(System.Math)));
            //add the menu to the form
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(mathbuttonsmenu, this, 0, 20);
            basicMathLayoutPanel.Controls.Add(mathbuttonsmenu.myFormControl, 0, 0);
            //size to fill container
            mathbuttonsmenu.myFormControl.Dock = DockStyle.Fill;
            //StarMath matrix library
            PUPPIGUIController.PUPPIModuleKeepMenu starMathbuttonsmenu = new PUPPIGUIController.PUPPIModuleKeepMenu("StarMath Matrix", 100, 80, 160, 25, 2, pmc, "Matrix operations");
            starMathbuttonsmenu.AddMenuButtonList(PUPPIModel. AutomaticPUPPImodulesCreator.makeTypePUPPImodules(typeof(StarMathLib.StarMath)),0,0.85,0.75);
            //add the menu to the form
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(starMathbuttonsmenu, this, 0, 20);
           
            advancedMathTableLayoutPanel.Controls.Add(starMathbuttonsmenu.myFormControl, 0, 0);
            //size to fill container
            starMathbuttonsmenu.myFormControl.Dock = DockStyle.Fill;
            
            //string expression evaluator from NCalc
            List<Type> expressionConstructorParams = new List<Type>();
            //we want to generate expressions only based on string variables
            expressionConstructorParams.Add(typeof(string));
            PUPPIModule expStringConstructor = PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(PUPPIModel.AutomaticPUPPImodulesCreator.makeConstructorIntoPUPPIModuleType(typeof(NCalc.Expression), expressionConstructorParams));
            //and the method to evaluate an expression
            //no parameters
            List<Type> expressionEvaluateParams = new List<Type>();
            PUPPIModule expressionEvaluate = PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(NCalc.Expression), "Evaluate", expressionEvaluateParams));



            //expression modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu expevalbuttonsmenu = new PUPPIGUIController.PUPPIModuleKeepMenu("Ncalc Expressn", 200, 80, 140, 25, 1, pmc,"Evaluates mathematical expressions supplied as string.Expression string must have variables in square brackets");
            expevalbuttonsmenu.AddMenuButton(expStringConstructor);
            expevalbuttonsmenu.AddMenuButton(expressionEvaluate);
            //add custom module to set parameters
            expevalbuttonsmenu.AddMenuButton(new SetExpressionParameter());
            //and to get parameter value
            expevalbuttonsmenu.AddMenuButton(new GetExpressionParameter());
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(expevalbuttonsmenu, this, 0, 0);
            advancedMathTableLayoutPanel.Controls.Add(expevalbuttonsmenu.myFormControl, 1, 0);
            expevalbuttonsmenu.myFormControl.Dock = DockStyle.Fill;




            //logic modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu logicmenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("Logic", 120, 80, 150, 25, 1, pmc,"Program flow modules");
            logicmenubuttons.AddPUPPIPremadeLogicModules(0.2, 0.9, 0.5, 1);
            //add custom PUPPIModule (see below)
            logicmenubuttons.AddMenuButton(new PUPPINOT() as PUPPIModule, 0.25, 0.9, 0.55, 1);
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(logicmenubuttons, this, 200, 20);
            //add to general Programming layout tab
            generalProgrammingMenuLayout.Controls.Add(logicmenubuttons.myFormControl, 0, 0);
            logicmenubuttons.myFormControl.Dock = DockStyle.Fill;


       

            //data input modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu datainputmenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("Input", 120, 80, 150, 25, 1, pmc,"Data entry modules");
            datainputmenubuttons.AddPUPPIPremadeDataInputModules(0.1, 0.9, 0.8, 1);
            //our custom string input
            datainputmenubuttons.AddMenuButton(new PUPPIStringInput(), 0.85, 1, 0.55);
            datainputmenubuttons.AddMenuButton(new PUPPIStringEvalInput(), 0.80, 1, 0.65);
            datainputmenubuttons.AddMenuButton(new PUPPIFileBrowser(), 0.80, 1, 0.65);

            datainputmenubuttons.AddMenuButton(new PUPPICharInput(), 0.85, 1, 0.55);
            //apply the slider renderer we defined to the slider module
            if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
            {
                PUPPINodeCustomRenderer mySliderRenderer = interactionModules.interactionRendererFunctions.make3DRendererForSlider();
                mySliderRenderer.addRendererToCanvas(pmc);
                mySliderRenderer.applyRendererToModule(typeof(interactionModules.Slider), pmc);
            }
            datainputmenubuttons.AddMenuButton(new interactionModules.Slider(), 1, 0.8, 0.2);

            //apply the integer slider renderer we defined to the slider module
            if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
            {
                //save time, reuse renderer for slider
                PUPPINodeCustomRenderer myIntSliderRenderer = interactionModules.interactionRendererFunctions.make3DRendererForSlider();
                //but need to change name because it is the dictionary key to retrieve it
                myIntSliderRenderer.name = "IntSliderRenderer"; 
                myIntSliderRenderer.addRendererToCanvas(pmc);
                myIntSliderRenderer.applyRendererToModule(typeof(interactionModules.IntSlider), pmc);
            }
            datainputmenubuttons.AddMenuButton(new interactionModules.IntSlider(), 1, 0.8, 0.2);



            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(datainputmenubuttons, this, 360, 20);
            //add to general Programming layout tab
            generalProgrammingMenuLayout.Controls.Add(datainputmenubuttons.myFormControl, 1, 0);
            datainputmenubuttons.myFormControl.Dock = DockStyle.Fill;

            if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
            {
                //regular text display - the one that fits in node
                PUPPINodeCustomRenderer myTextRenderer = canvasRenderer.CanvasRenderHelper.makeTextDisplay3DRenderer();
                myTextRenderer.addRendererToCanvas(pmc);
                myTextRenderer.applyRendererToModule(typeof(canvasRenderer.PUPPITextDisplay), pmc);
                //the fixed size font text display
                PUPPINodeCustomRenderer myTextRendererFS = canvasRenderer.CanvasRenderHelper.makeTextDisplayFS3DRenderer();
                myTextRendererFS.addRendererToCanvas(pmc);
                myTextRendererFS.applyRendererToModule(typeof(canvasRenderer.PUPPITextDisplayFS), pmc);
                //the table display
                PUPPINodeCustomRenderer myTableRenderer = canvasRenderer.CanvasRenderHelper.makeTable3DRenderer();
                myTableRenderer.addRendererToCanvas(pmc);
                myTableRenderer.applyRendererToModule(typeof(canvasRenderer.PUPPIVertTableDisplay ), pmc);
                
                
                //customized text display
                PUPPINodeCustomRenderer myCustomTextRenderer = new PUPPINodeCustomRenderer("CustomTextRenderer");
                myCustomTextRenderer.addRendererToCanvas(pmc);
                myCustomTextRenderer.applyRendererToModule(typeof(canvasRenderer.PUPPITextCustomDisplay), pmc);
                //a renderer for notes the user can place of the canvas
                PUPPINodeCustomRenderer myNoteRenderer = new PUPPINodeCustomRenderer("NoteTextRenderer");
                //this module does not have inputs. as such, when initially loaded, it doesn't process. 
                //the number of objects in the renderer needs to match the number of objects loaded from the renderer state
                //as such, we add a dummy caption to the renderer.
                myNoteRenderer.addCaption3D("Double click to change", 0, 0, 0, 1, 1, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);
                //so that it doesn appear on the note
                myNoteRenderer.useDefaultCaption = false;
                myNoteRenderer.addRendererToCanvas(pmc);
                myNoteRenderer.applyRendererToModule(typeof(canvasRenderer.PUPPINoteDisplay), pmc);

                //render a 3D model of an object directly on canvas instead of using CAD window
                PUPPINodeCustomRenderer modelRenderer = canvasRenderer.CanvasRenderHelper.makeModelRenderer();
                modelRenderer.addRendererToCanvas(pmc);
                modelRenderer.applyRendererToModule(typeof(canvasRenderer.PUPPIObjectDisplay), pmc);
                modelRenderer.applyRendererToModule(typeof(canvasRenderer.PUPPIObjectSizeDisplay), pmc);
                //image display
                PUPPINodeCustomRenderer myImageFileRenderer = new PUPPINodeCustomRenderer("ImageFileRenderer");
                myImageFileRenderer.useDefaultCaption = false;  
                myImageFileRenderer.addRendererToCanvas(pmc);
                myImageFileRenderer.applyRendererToModule(typeof(canvasRenderer.PUPPIImageFileDisplay), pmc);
            }

            //output modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu outputmenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("Output", 140, 80, 150, 25, 1, pmc,"Modules to display results on canvas");
            //render text canvas
            outputmenubuttons.AddMenuButton(new canvasRenderer.PUPPITextDisplay(), 0, 1, 1);
            outputmenubuttons.AddMenuButton(new canvasRenderer.PUPPITextDisplayFS(), 0, 1, 0.9);
            outputmenubuttons.AddMenuButton(new canvasRenderer.PUPPINoteDisplay(), 0, 1, 1);
            outputmenubuttons.AddMenuButton(new canvasRenderer.PUPPITextCustomDisplay(), 0, 1, 1);
            outputmenubuttons.AddMenuButton(new canvasRenderer.PUPPIVertTableDisplay() , 0, 1, 1);
            outputmenubuttons.AddMenuButton(new canvasRenderer.PUPPIImageFileDisplay(), 0.3, 0.9, 1);
    
            //Bar plotting
            //a default empty renderer
            PUPPINodeCustomRenderer barPlotRenderer = new PUPPINodeCustomRenderer("BarChart3DRenderer");
            barPlotRenderer.useDefaultCaption = false;
            barPlotRenderer.addRendererToCanvas(pmc);
            barPlotRenderer.applyRendererToModule(typeof(canvasRenderer.BarGraph3D), pmc);
            outputmenubuttons.AddMenuButton(new canvasRenderer.BarGraph3D(), 0, 0.65, 1);
            //line plot
            PUPPINodeCustomRenderer linePlotRenderer = new PUPPINodeCustomRenderer("LinePlot3DRenderer");
            linePlotRenderer.useDefaultCaption = false;
            linePlotRenderer.addRendererToCanvas(pmc);
            linePlotRenderer.applyRendererToModule(typeof(canvasRenderer.LinePlot3D), pmc);
            outputmenubuttons.AddMenuButton(new canvasRenderer.LinePlot3D(), 0, 0.65, 1);
            //gauge
            PUPPINodeCustomRenderer gaugeRenderer = canvasRenderer.CanvasRenderHelper.makeGauge3DRenderer();
            gaugeRenderer.addRendererToCanvas(pmc);
            gaugeRenderer.applyRendererToModule(typeof(canvasRenderer.PUPPIGauge3D), pmc);
            outputmenubuttons.AddMenuButton(new canvasRenderer.PUPPIGauge3D() , 0.2, 0.65, 1);

            //add critical messages
            //we'll use a step by step workflow here, not the more automatic functions
            List<Type> constructorParams = new List<Type>();
            //the parameterless constructor first (see class definition below
            PUPPIModule msgDefault = PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(PUPPIModel.AutomaticPUPPImodulesCreator.makeConstructorIntoPUPPIModuleType(typeof(AlertMessage), constructorParams));
            //now the constructor with a string
            constructorParams.Add(typeof(string));
            PUPPIModule msgParam = PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(PUPPIModel.AutomaticPUPPImodulesCreator.makeConstructorIntoPUPPIModuleType(typeof(AlertMessage), constructorParams));
            //and the method to display the string as a message
            //list of parameters can also be left null since this method has no parameters
            PUPPIModule displayAlert = PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(AlertMessage), "displayAlert"));
            //now add the modules to the menu
            outputmenubuttons.AddMenuButton(msgDefault);
            outputmenubuttons.AddMenuButton(msgParam);
            outputmenubuttons.AddMenuButton(displayAlert);

            //write to CSV
            outputmenubuttons.AddMenuButton(new customOutputModules.PUPPIWriteToCSV(), 0, 0.9, 1);
            //save bitmap
            outputmenubuttons.AddMenuButton(new customOutputModules.PUPPIBMPToFile(), 0.1, 0.85, 0.95);

            //add to general Programming layout tab
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(outputmenubuttons, this, 360, 20);
            generalProgrammingMenuLayout.Controls.Add(outputmenubuttons.myFormControl, 2, 0);
            outputmenubuttons.myFormControl.Dock = DockStyle.Fill;

            //collection modules
            //list modules menu
            PUPPIGUIController.PUPPIModuleKeepMenu listmenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("", 200, 80, 160, 25, 3, pmc, "1D and 2D array or list functions");
            listmenubuttons.AddPUPPIPremadeListModules(0.1, 0.9, 0.7, 1);
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(listmenubuttons, this, 0, 0);
            //add to Collections Tab
            tabCollections.Controls.Add(listmenubuttons.myFormControl);
            listmenubuttons.myFormControl.Dock = DockStyle.Fill;
            

            //interoperability modules
            PUPPIGUIController.PUPPIModuleKeepMenu dataexchangemenubuttons = new PUPPIGUIController.PUPPIModuleKeepMenu("", 120, 80, 160, 25, 2, pmc,"Data type conversion and data transfer modules");
            dataexchangemenubuttons.AddPUPPIPremadeDataExchangeModules(0.1, 0.9, 0.8, 1);
       
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(dataexchangemenubuttons, this, 0, 0);
            
            
            tabInteroperability.Controls.Add(dataexchangemenubuttons.myFormControl);
            dataexchangemenubuttons.myFormControl.Dock = DockStyle.Fill;

            //string operations
            PUPPIGUIController.PUPPIModuleKeepMenu stringbuttonsmenu = new PUPPIGUIController.PUPPIModuleKeepMenu("", 200, 80, 160, 25, 3, pmc,"Operations on strings" );
            //also add functions from the System.String class
            stringbuttonsmenu.AddMenuButtonList(PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(typeof(System.String)));
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(stringbuttonsmenu, this, 0, 0);
            tabStringMenu.Controls.Add(stringbuttonsmenu.myFormControl);
            stringbuttonsmenu.myFormControl.Dock = DockStyle.Fill;

            //file operations
            PUPPIGUIController.PUPPIModuleKeepMenu filebuttonsmenu = new PUPPIGUIController.PUPPIModuleKeepMenu("", 200, 80, 160, 25, 3, pmc,"Modules for interacting with the file system");
            //also add functions from the System.io.file class
            filebuttonsmenu.AddMenuButtonList(PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(typeof(System.IO.File)));
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(filebuttonsmenu, this, 0, 0);
            tabFileMenu.Controls.Add(filebuttonsmenu.myFormControl);
            filebuttonsmenu.myFormControl.Dock = DockStyle.Fill;


            //menu from Vector3D structure
            //also showing replacement and filter
            List<String> nf = new List<String>();
            nf.Add("Add");
            nf.Add("AngleBetween");
            nf.Add("CrossProduct");
            nf.Add("Divide");
            nf.Add("DotProduct");
            nf.Add("Multiply");
            nf.Add("Negate");
            nf.Add("Normalize");
            nf.Add("Subtract");
          

 


            List<String> rf = new List<String>();
            rf.Add("Add");
            rf.Add("Angle");
            rf.Add("X Prod");
            rf.Add("Divide");
            rf.Add("Dot Prod");
            rf.Add("Multiply");
            rf.Add("Negate");
            rf.Add("Normalize");
            rf.Add("Subtract");
            

            //vector 3d menu
            //remember, you need to instantiate something for the assembly to actually load
            Vector3D vv = new Vector3D();
            
            ArrayList vectormodules = PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(vv.GetType(), false, nf, rf);
            nf = new List<string>();
            //more stuff, getter and setters
            nf.Add("get_X");
            nf.Add("get_Y");
            nf.Add("get_Z");
            nf.Add("set_X");
            nf.Add("set_Y");
            nf.Add("set_Z");
            ArrayList getvectormodules = PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(vv.GetType(), false, nf);
            
            
            PUPPIGUIController.PUPPIModuleKeepMenu pvm = new PUPPIGUIController.PUPPIModuleKeepMenu("Vector3D", 190, 95, 150, 25, 1, pmc,"3D Vector objects and operations");
            pvm.AddMenuButtonList(vectormodules, 0, 0.75, 0.95, 1);
            pvm.AddMenuButtonList(getvectormodules, 0, 0.75, 0.95, 1);

            //convert method to module
            Type projVecPM = PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(PUPPICAD.HelperClasses.utilities), "projectVectorOn",new List<Type>() {typeof(Vector3D),typeof(Vector3D)}  );
            //add to menu
            pvm.AddMenuButton(PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(projVecPM), 0, 1, 0.2, 0.5);

            //convert method to module
            Type rotVecPM = PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(PUPPICAD.HelperClasses.utilities), "rotateVector3D", new List<Type>() { typeof(Vector3D), typeof(Vector3D),typeof(double) });
            //add to menu
            pvm.AddMenuButton(PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(rotVecPM), 0, 1, 0.2, 0.5);   
  
                

            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(pvm, this, 0, 0);
            primitivesLayoutPanel.Controls.Add(pvm.myFormControl, 0, 0);
            pvm.myFormControl.Dock = DockStyle.Fill;

           
            //point 3d menu
            //use one instance to get methods
            Point3D pointtest = new Point3D();
             
           
            //pick only some emthods and replace some names with more readable ones
            List<string> pointmethfilter = new List<string>();
            pointmethfilter.Add("Add");
            pointmethfilter.Add("Multiply");
            pointmethfilter.Add("Subtract");
            pointmethfilter.Add("get_X");
            pointmethfilter.Add("get_Y");
            pointmethfilter.Add("get_Z");
            pointmethfilter.Add("set_X");
            pointmethfilter.Add("set_Y");
            pointmethfilter.Add("set_Z");

            List<string> pointrepfilter = new List<string>();
            pointrepfilter.Add("Add");
            pointrepfilter.Add("Multiply");
            pointrepfilter.Add("Subtract");
            pointrepfilter.Add("get X");
            pointrepfilter.Add("get Y");
            pointrepfilter.Add("get Z");
            pointrepfilter.Add("set X");
            pointrepfilter.Add("set Y");
            pointrepfilter.Add("set Z");



            ArrayList point3dmethods = PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(pointtest.GetType(), false, pointmethfilter, pointrepfilter);
            PUPPIGUIController.PUPPIModuleKeepMenu point3dm = new PUPPIGUIController.PUPPIModuleKeepMenu("Point3D", 90, 95,160, 25, 1, pmc, "3D Point objects and operations");
            point3dm.AddMenuButtonList(point3dmethods, 0.65, 0.8, 0.95, 1);
            //convert method to module
            Type remDupPoPM = PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(PUPPICAD.HelperClasses.utilities), "removeDuplicatePoints", new List<Type>() { typeof(List<Point3D>) });
            //add to menu
            point3dm.AddMenuButton(PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(remDupPoPM    ),0,1,0.2,0.5);
            //change tooltip
            point3dm.ChangeMenuButtonTooltip(PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(remDupPoPM), "Removes duplicate points in a collection of Point3d");  
            
            //convert method to module
            Type distPoPM = PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(PUPPICAD.HelperClasses.utilities), "getDistBetweenPoints", new List<Type>() { typeof(Point3D), typeof(Point3D) });
            //add to menu
            point3dm.AddMenuButton(PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(distPoPM), 0, 1, 0.5, 0.5);
            //convert method to module
            Type betweenPoPM = PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(PUPPICAD.HelperClasses.utilities), "getPointBetweenPoints", new List<Type>() { typeof(Point3D), typeof(Point3D), typeof(double) });
            //add to menu
            point3dm.AddMenuButton(PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(betweenPoPM), 0, 1, 0.5, 0.6);
            //convert method to module
            Type pCentroid = PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(PUPPICAD.HelperClasses.utilities), "getPointsCentroid", new List<Type>() { typeof(IEnumerable) });
            //add to menu
            point3dm.AddMenuButton(PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(pCentroid ), 0, 0.9, 0.6, 0.6);
            //convert method to module
            Type orderCCW = PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(PUPPICAD.HelperClasses.utilities), "orderPointsCounterClockwise", new List<Type>() { typeof(IEnumerable) ,typeof(Vector3D) });
            //add to menu
            point3dm.AddMenuButton(PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(orderCCW), 0, 0.8, 0.7, 0.7);
            //convert method to module
            Type orderCW = PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(PUPPICAD.HelperClasses.utilities), "orderPointsClockwise", new List<Type>() { typeof(IEnumerable), typeof(Vector3D) });
            //add to menu
            point3dm.AddMenuButton(PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(orderCW), 0, 0.8, 0.7, 0.7);
  
            
            //convert method to module
            Type nearestPt = PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(PUPPICAD.HelperClasses.utilities), "getNearestPoint", new List<Type>() { typeof(IEnumerable), typeof(Point3D) });
            //add to menu
            point3dm.AddMenuButton(PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(nearestPt), 0, 0.8, 0.7, 0.7);

            //convert method to module
            Type rotatePt = PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(PUPPICAD.HelperClasses.utilities), "rotatePoint3D", new List<Type>() { typeof(Point3D),typeof(Point3D), typeof(Vector3D),typeof(double) });
            //add to menu
            point3dm.AddMenuButton(PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(rotatePt), 0, 0.8, 0.7, 0.7);

            //convert method to module
            Type normalToPts = PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(PUPPICAD.HelperClasses.utilities), "pointsNormal", new List<Type>() { typeof(Point3D), typeof(Point3D), typeof(Point3D) });
            //add to menu
            point3dm.AddMenuButton(PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(normalToPts), 0, 0.85, 0.75, 0.7);

            
            //add point bounding box dimensions module
            point3dm.AddMenuButton(new PUPPICAD.PUPPIPointCloudBB(), 0.1, 0.7, 0.8, 0.7); 
            
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(point3dm, this, 0, 0);
            primitivesLayoutPanel.Controls.Add(point3dm.myFormControl, 1, 0);
            point3dm.myFormControl.Dock = DockStyle.Fill;

            //in order to create sketches, we also need to be able to make and operate on 2D Points
            //we will use the same filtering as for Point 3D methods
            pointmethfilter.Remove("get_Z");
            pointmethfilter.Remove("set_Z");
            pointrepfilter[0] = "Point2D Add";
            pointrepfilter[1] = "Point2D Mult";
            pointrepfilter[2] = "Point2D Subt";
            pointrepfilter.Remove("get X");
            pointrepfilter.Remove("get Y");
            pointrepfilter.Remove("get Z");
            pointrepfilter.Add("Get Point2D X");
            pointrepfilter.Add("Get Point2D Y");
            pointrepfilter.Remove("set X");
            pointrepfilter.Remove("set Y");
            pointrepfilter.Remove("set Z");
            pointrepfilter.Add("Set Point2D X");
            pointrepfilter.Add("Set Point2D Y");

            ArrayList point2dmodules = PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(typeof(System.Windows.Point), false, pointmethfilter, pointrepfilter);
            PUPPIGUIController.PUPPIModuleKeepMenu point2dm = new PUPPIGUIController.PUPPIModuleKeepMenu("Point 2D", 200, 90, 120, 25, 1, pmc, "2D Point objects and operations");
            point2dm.AddMenuButtonList(point2dmodules, 0.65, 0.8, 0.95, 1);
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(point2dm, this, 0, 0);
            primitivesLayoutPanel.Controls.Add(point2dm.myFormControl, 2, 0);
            point2dm.myFormControl.Dock = DockStyle.Fill;

            //and our custom plane modules
            //take all methods and even the default constructor
            ArrayList allplanemodules = PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(new PUPPICAD.HelperClasses.PUPPIPlane3D().GetType(), true);
            //now get rid of some
            ArrayList planemodules = new ArrayList();
            for (int planeCount = 0; planeCount < allplanemodules.Count; planeCount++)
            {
                Type planeM = allplanemodules[planeCount] as Type;
                if (planeM.Name.Contains("ToString") || planeM.Name.Contains("GetHashCode") || planeM.Name.Contains("GetType"))
                {

                }
                else
                {
                    planemodules.Add(planeM);
                }
            }
            PUPPIGUIController.PUPPIModuleKeepMenu planemenu = new PUPPIGUIController.PUPPIModuleKeepMenu("3D Plane", 190, 130, 160, 25, 1, pmc, "PUPPIPlane3D creation and functions, not the same as Helix Plane3D but can be converted to such");
            planemenu.AddMenuButtonList(planemodules, 0.75, 0.65, 0.85, 0.8);
            //contours
            planemenu.AddMenuButton(new PUPPICAD.PUPPIModelGetContours(), 0.15, 0.95, 0.95);
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(planemenu, this, 0, 0);
            primitivesLayoutPanel.Controls.Add(planemenu.myFormControl, 3, 0);
            planemenu.myFormControl.Dock = DockStyle.Fill;


            //spline menu
            //remember, you need to instantiate something for the assembly to actually load
            PUPPICAD.HelperClasses.PUPPISpline3D ps = new PUPPICAD.HelperClasses.PUPPISpline3D();
            //put everything from the type in the men u
            ArrayList splinemodules = PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(ps.GetType(), true, new List<string>() { "addControlPoint", "insertControlPoint","removeControlPoint", "convertToPoints", "getPointOnSpline", "getSplineApproxLength" ,"convToPolyLine3D" });
            PUPPIGUIController.PUPPIModuleKeepMenu psm = new PUPPIGUIController.PUPPIModuleKeepMenu("3D Spline", 190, 90, 170, 25, 1, pmc, "Bezier spline creation and operations");
            psm.AddMenuButtonList(splinemodules, 0, 0.8, 0.95, 0.8);
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(psm, this, 0, 0);
            curvesLayoutPanel.Controls.Add(psm.myFormControl, 1, 0);   
            //primitivesLayoutPanel.Controls.Add(psm.myFormControl, 3, 0);
            psm.myFormControl.Dock = DockStyle.Fill;
            //polylines. specify which methods to take in addition to constructors
            ArrayList polylinemodules = PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(new PUPPICAD.HelperClasses.PUPPIPolyLine3D().GetType(), false, new List<string>() { "getPoly3DPoints", "reversePolyline3D", "closePolyline3D", "appendPolyline3D", "reorderPoints", "getPolyLineLength", "getPolyLineSegments", "getPointOnPolyLine", "getPolyLinePtRatio", "findNearestPoint", "refactorPolyLine", "trimStartByRatio", "trimEndByRatio", "trimStartByPoint", "trimEndByPoint", "rotatePolyLine", "translatePolyLine", "offsetPolyLine","projectPolyline" });
            PUPPIGUIController.PUPPIModuleKeepMenu pplm = new PUPPIGUIController.PUPPIModuleKeepMenu("3D PolyLine", 190, 120, 170, 25, 1, pmc, "3D PolyLine creation and operations.");
            pplm.AddMenuButtonList(polylinemodules, 0.85, 0.65, 0.75, 0.8);
            //intersection
            pplm.AddMenuButton(new PUPPICAD.PUPPPolylineModelIntersection(), 0.2, 0.9, 0.95); 
            //projection
            pplm.AddMenuButton(new PUPPICAD.PUPPIProjectPolylineOnModel(), 0.2, 0.9, 0.95); 
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(pplm, this, 0, 0);
            //primitivesLayoutPanel.Controls.Add(pplm.myFormControl, 4, 0);
            curvesLayoutPanel.Controls.Add(pplm.myFormControl, 0, 0);
            pplm.myFormControl.Dock = DockStyle.Fill;

            ////helper objects module menu to make 3d objects 
            //2d sketch for extrusions and lofts, with some extra wrappers for commnon shapes
            ArrayList sketchmodules = PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(new PUPPICAD.HelperClasses.PUPPISketch2D().GetType(), false, new List<string>() { "PUPPICircleSketch2D", "PUPPIArcSketch2D", "PUPPICapsuleSketch2D", "PUPPIRectangleSketch2D", "PUPPIEllipseSketch2D", "PUPPIIBeamSketch2D", "getPointList", "offsetSketch", "rotateSketch", "translateSketch" });


            //make a menu to store all this
            PUPPIGUIController.PUPPIModuleKeepMenu skteches2DMenu = new PUPPIGUIController.PUPPIModuleKeepMenu("2D Sketch", 145, 95, 170, 25, 1, pmc, "2D Sketches which can be used to build 3D objects");
            skteches2DMenu.AddMenuButtonList(sketchmodules, 0.85, 0.65, 0.75);

            //convert method to module
            Type sketch2Poly = PUPPIModel.AutomaticPUPPImodulesCreator.makeMethodIntoPUPPIModuleType(typeof(PUPPICAD.HelperClasses.utilities), "sketchToPolyLine3D", new List<Type>() { typeof(PUPPICAD.HelperClasses.PUPPISketch2D), typeof(PUPPICAD.HelperClasses.PUPPIPlane3D), typeof(Vector3D) });
            //add to menu
            skteches2DMenu.AddMenuButton(PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(sketch2Poly), 0, 0.8, 0.7, 0.7);


            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(skteches2DMenu, this, 0, 0);
            //basicCADLayoutPanel.Controls.Add(skteches2DMenu.myFormControl, 2, 0);
            curvesLayoutPanel.Controls.Add(skteches2DMenu.myFormControl, 2, 0);
            skteches2DMenu.myFormControl.Dock = DockStyle.Fill;



         
 



            //transform menu
            //we will only use constructors so no methods
            List<string> emptfilter = new List<string>();
            ScaleTransform3D sc = new ScaleTransform3D();
            TranslateTransform3D tr = new TranslateTransform3D();
            RotateTransform3D ro = new RotateTransform3D();
            ArrayList scaleconstructors = PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(sc.GetType(), false, emptfilter);
            ArrayList rotateconstructors = PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(ro.GetType(), false, emptfilter);
            ArrayList translateconstructors = PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(tr.GetType(), false, emptfilter);
            //make a transformations menu to store transforms
            PUPPIGUIController.PUPPIModuleKeepMenu basic3DEditingMenu = new PUPPIGUIController.PUPPIModuleKeepMenu("Basic Editing", 150, 100, 150, 25, 1, pmc,"Operations for modifying 3D objects");
            //add scale constructors to the menu
            foreach (Type t in scaleconstructors)
            {
                PUPPIModel.PUPPIModule tt = System.Activator.CreateInstance(t) as PUPPIModel.PUPPIModule;
                //only add constructs with 3 parameters
                if (tt.inputs.Count == 3)
                {
                    basic3DEditingMenu.AddMenuButton(tt, 0.85, 0.75, 0.75);
                }
            }
            foreach (Type t in translateconstructors)
            {
                PUPPIModel.PUPPIModule tt = System.Activator.CreateInstance(t) as PUPPIModel.PUPPIModule;
                //only add constructs with 3 parameters
                if (tt.inputs.Count == 3)
                {
                    basic3DEditingMenu.AddMenuButton(tt, 0.95, 0.75, 0.75);
                }
            }
            foreach (Type t in rotateconstructors)
            {
                PUPPIModel.PUPPIModule tt = System.Activator.CreateInstance(t) as PUPPIModel.PUPPIModule;
                //only add constructs with 4 parameters
                if (tt.inputs.Count == 4)
                {
                    basic3DEditingMenu.AddMenuButton(tt, 1, 0.75, 0.75);
                }
            }
            //also add some rotation options for the rotate transform
            ArrayList aarotate3dmodules = PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(new AxisAngleRotation3D().GetType(), false, new List<string>());
            basic3DEditingMenu.AddMenuButtonList(aarotate3dmodules, 1, 0.8, 0.75);
            basic3DEditingMenu.AddMenuButton(new PUPPIApplyTransform (),1,0.9,0.75);
            basic3DEditingMenu.AddMenuButton(new PUPPIHardcodeTransform(), 0.6, 0.8, 0.8);
            basic3DEditingMenu.AddMenuButton(new PUPPIAlignObject(), 1, 0.9, 0.75);
            basic3DEditingMenu.AddMenuButton(new PUPPIResetTransform(), 1, 0.9, 0.75);
            basic3DEditingMenu.AddMenuButton(new PUPPIRefineModelMesh (), 0.7, 0.9, 0.75);

            basic3DEditingMenu.AddMenuButton(new PUPPIChangeColor(), 1, 0.9, 0.7);
            basic3DEditingMenu.AddMenuButton(new PUPPIRecalcNormals(), 0.87, 0.9, 0.7);
            basic3DEditingMenu.AddMenuButton(new PUPPISetTexture(), 0.99, 0.95, 0.75);
            basic3DEditingMenu.AddMenuButton(new PUPPIModelGetPoints(), 0.9, 0.95, 0.75);
            basic3DEditingMenu.AddMenuButton(new PUPPIModelSetPoints(), 0.9, 0.95, 0.75);
            basic3DEditingMenu.AddMenuButton(new PUPPIModelGetDimensions());
            basic3DEditingMenu.AddMenuButton(new PUPPIModelGetArea(), 0.95, 0.95, 0.85);
            basic3DEditingMenu.AddMenuButton(new PUPPIGetModelAsComponents(), 0.8, 0.95, 0.75);
            basic3DEditingMenu.AddMenuButton(new PUPPIGetModelNormal ());
            basic3DEditingMenu.AddMenuButton(new PUPPIModelNearestPoint());
            basic3DEditingMenu.AddMenuButton(new PUPPISliceModel());
            basic3DEditingMenu.AddMenuButton(new PUPPI3DObjectToTextSTL(),0.85,0.9,0.74 );
            basic3DEditingMenu.AddMenuButton(new PUPPI3DObjectFromSTLText() , 0.85, 0.9, 0.74);
            //object sculptor
           
            if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
            {
                PUPPINodeCustomRenderer mySculptorRenterer = new PUPPINodeCustomRenderer("ObjectSculptor");
                mySculptorRenterer.useDefaultCaption = false;
                mySculptorRenterer.addRendererToCanvas(pmc);
                mySculptorRenterer.applyRendererToModule(typeof(interactionModules.ObjectSculptor), pmc);
            }
            basic3DEditingMenu.AddMenuButton(new interactionModules.ObjectSculptor(), 0.9, 0.85, 0.4);

            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(basic3DEditingMenu, this, 0, 0);
            basicCADLayoutPanel.Controls.Add(basic3DEditingMenu.myFormControl, 0, 0);
            basic3DEditingMenu.myFormControl.Dock = DockStyle.Fill;


            //color menu
            //only make colors from rgb and alpha rgb
            List<string> comethfilter = new List<string>();
            comethfilter.Add("FromRgb");
            comethfilter.Add("FromArgb");

            List<string> corepfilter = new List<string>();
            corepfilter.Add("RGBColor");
            corepfilter.Add("AlphaRGBCol");

              
            //make sure we use the right type of Color by specifying the namespace
            ArrayList colormodules = PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(typeof(System.Windows.Media.Color), false, comethfilter, corepfilter);
            PUPPIGUIController.PUPPIModuleKeepMenu PUPPIcolormenu = new PUPPIGUIController.PUPPIModuleKeepMenu("Materials", 120, 95, 120, 25, 1, pmc,"Modules for assigning colors to PUPPI 3D objects and for creating materials for WPF/Helix objects.");
            PUPPIcolormenu.AddMenuButtonList(colormodules, 0.8, 0.8, 0.1, 1);
            //add brushes too. only the constructor with parameters
            PUPPIcolormenu.AddMenuButtonList(PUPPIModel.AutomaticPUPPImodulesCreator.makeTypePUPPImodules(typeof(System.Windows.Media.SolidColorBrush), false, new List<string>()));
            //convenience primary colors
            PUPPIcolormenu.AddMenuButton(new canvasRenderer.PUPPIRedColor(), 1, 0.65, 0.65);
            PUPPIcolormenu.AddMenuButton(new canvasRenderer.PUPPIGreenColor(), 0.65, 1, 0.65);
            PUPPIcolormenu.AddMenuButton(new canvasRenderer.PUPPIBlueColor(), 0.65, 0.65, 1);  
            
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(PUPPIcolormenu, this, 0, 0);
            basicCADLayoutPanel.Controls.Add(PUPPIcolormenu.myFormControl, 1, 0);
            PUPPIcolormenu.myFormControl.Dock = DockStyle.Fill;

         



            //CAD objects menu
            PUPPIGUIController.PUPPIModuleKeepMenu cadmenu = new PUPPIGUIController.PUPPIModuleKeepMenu("CAD Objects", 130, 95, 155, 25, 2, pmc,"3D objects that can be rendered in the CAD window using the 3D Model Access module");
          
            // CAD modules bundled with PUPPI
            cadmenu.AddPUPPICADModules(pvc, 0.95, 0.75, 0.1, 1);
            //create convex hull from points
            //make button semi-transparent
            cadmenu.AddMenuButton(new meshingModules.PUPPICADConvexHull(), 0.65, 0.65, 0.1, 0.5);
            //delaunay
           // cadmenu.AddMenuButton(new meshingModules.PUPPICADDelaunay(), 0.65, 0.65, 0.1, 0.5);

            //get PUPPI modules from the Helix 3d DLL which were selected with the PUPPI Namespace Explorer tool
            //only one module, text 3d 
            List<string> helixTypeNames = new List<string>();
            List<ArrayList> helixModuleLists = PUPPIModel.AutomaticPUPPImodulesCreator.makePUPPImoduleListsFromMTPS(@".\textHelix3D.mtps", out helixTypeNames);

            //store the text creation here
            cadmenu.AddMenuButtonList(helixModuleLists[0], 1, 0, 0);

            //more custom modules
            cadmenu.AddMenuButton(new canvasRenderer.PUPPIObjectDisplay());
            cadmenu.AddMenuButton(new canvasRenderer.PUPPIObjectSizeDisplay());



            //add the menu to the form
            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(cadmenu, this, 0, 0);
            basicCADLayoutPanel.Controls.Add(cadmenu.myFormControl, 2, 0);
            cadmenu.myFormControl.Dock = DockStyle.Fill;


            #region load WPF and Helix#D modules

            if (Properties.Settings.Default.loadHelixWPF)
            {

                //wpf controls menu 
                //menu titles not shown since each tab has title
                PUPPIGUIController.PUPPIModuleKeepMenu aWPFCADMenu = new PUPPIGUIController.PUPPIModuleKeepMenu("", 765, 58, 130, 25, 1, pmc);
                aWPFCADMenu.AddMenuButton(new PUPPICADModel() as PUPPIModule);
                PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(aWPFCADMenu, this, 450 - menuTabControl.Left, 22 - tabPUPPICADBasic.Top);
                aWPFtabPage1.Controls.Add(aWPFCADMenu.myFormControl);
                aWPFCADMenu.myFormControl.Dock = DockStyle.Fill;
                //now make all the types from System.Windows.Media.Media3D PUPPIModules DLL folder
                List<ArrayList> media3Dtypes = new List<ArrayList>();
                List<string> media3Dnames = new List<string>();
                List<string> media3Dfiles = new List<string>();
                //load from DLLs created with the PUPPIAssemblyCreator
                if (Directory.Exists(@".\PUPPICADAdvancedModules\Media3D") && !IsDirectoryEmpty(@".\PUPPICADAdvancedModules\Media3D"))
                    media3Dtypes = PUPPIModel.AutomaticPUPPImodulesCreator.importPUPPIModulesFromFolder(@".\PUPPICADAdvancedModules\Media3D", out media3Dnames, out media3Dfiles);
                ////clean up, get rid of methods related to system operations we don't care about
                //foreach (ArrayList ar in media3Dtypes)
                //{
                //    int arCount = 0;
                //    while (arCount < ar.Count)
                //    {
                //        //PUPPIModule pm = PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(ar[arCount] as Type);
                //        //if (pm.name.Contains("ToString") || pm.name.Contains("Equals") || pm.name.Contains("GetType") || pm.name.Contains("GetHashCode") || pm.name.Contains("GetValue") || pm.name.Contains("SetValue"))
                //        //{
                //        //    ar.RemoveAt(arCount);
                //        //}
                //        //else
                //        //{
                //        //    arCount++;
                //        //}

                //        string testname = (ar[arCount] as Type).Name;
                //        if (testname.Contains("ToString") || testname.Contains("Equals") || testname.Contains("GetType") || testname.Contains("GetHashCode") || testname.Contains("GetValue") || testname.Contains("SetValue"))
                //        {
                //            ar.RemoveAt(arCount);
                //        }
                //        else
                //        {
                //            arCount++;
                //        }
                //    }
                //}
                int i3d = 0;
                foreach (string media3DtypeName in media3Dnames)
                {
                    ArrayList myTypes = media3Dtypes[i3d];
                    if (myTypes.Count > 0)
                    {
                        PUPPIGUIController.PUPPIModuleKeepMenu media3DMenu = new PUPPIGUIController.PUPPIModuleKeepMenu("", 765, 58, 130, 25, 5, pmc);
                        media3DMenu.AddMenuButtonList(myTypes);
                        PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(media3DMenu, this, 450 - menuTabControl.Left, 22 - tabPUPPICADBasic.Top);
                        //just get the 
                        aWPFtabControl.TabPages.Add(media3DtypeName);

                        aWPFtabControl.TabPages[aWPFtabControl.TabPages.Count - 1].Controls.Add(media3DMenu.myFormControl);
                        media3DMenu.myFormControl.Dock = DockStyle.Fill;

                    }
                    i3d++;

                }


                //helix toolkit controls menu 
                //menu titles not shown since each tab has title
                PUPPIGUIController.PUPPIModuleKeepMenu aHelix3DCADMenu = new PUPPIGUIController.PUPPIModuleKeepMenu("", 765, 58, 130, 25, 1, pmc);
                aHelix3DCADMenu.AddMenuButton(new PUPPICADModel() as PUPPIModule);
                PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(aHelix3DCADMenu, this, 450 - menuTabControl.Left, 22 - tabPUPPICADBasic.Top);
                aHelix3Dtabpage1.Controls.Add(aHelix3DCADMenu.myFormControl);
                aHelix3DCADMenu.myFormControl.Dock = DockStyle.Fill;
                //now make all the types from System.Windows.Media.Media3D namespace
                List<ArrayList> Helix3Dtypes = new List<ArrayList>();
                List<string> Helix3Dnames = new List<string>();
                List<string> Helix3Dfiles = new List<string>();
                ////useless types - already deleted from folder
                //List<Type> excludeHelixTypes = new List<Type>();
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.TouchMode));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.CameraMode));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.CameraRotationMode));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.CameraSetting));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.ManipulationEventArgs));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.CategorizedColorAxis));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.ColorAxisPosition));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.ColorAxis));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.RangeColorAxis));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.HelixToolkitException));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.HelixViewport3D));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.InputBindingX));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.CameraController));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.CameraHelper));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.PerspectiveCameraExtension));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.StereoControl));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.StereoHelper));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.StereoView3D));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.AnaglyphView3D));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.WiggleView3D));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.BindableRotateManipulator));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.CombinedManipulator));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.RotateManipulator));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.TranslateManipulator));
                //excludeHelixTypes.Add(typeof(HelixToolkit.Wpf.BindableTranslateManipulator));

                if (Directory.Exists(@".\PUPPICADAdvancedModules\Helix3D") && !IsDirectoryEmpty(@".\PUPPICADAdvancedModules\Helix3D"))
                    Helix3Dtypes = PUPPIModel.AutomaticPUPPImodulesCreator.importPUPPIModulesFromFolder(@".\PUPPICADAdvancedModules\Helix3D", out Helix3Dnames, out Helix3Dfiles);
                ////clean up, get rid of methods related to system operations we don't care about
                //foreach (ArrayList ar in Helix3Dtypes)
                //{
                //    int arCount = 0;
                //    while (arCount < ar.Count)
                //    {
                //        // PUPPIModule pm = PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(ar[arCount] as Type);
                //        string testname = (ar[arCount] as Type).Name;
                //        if (testname.Contains("ToString") || testname.Contains("Equals") || testname.Contains("GetType") || testname.Contains("GetHashCode") || testname.Contains("GetValue") || testname.Contains("SetValue"))
                //        {
                //            ar.RemoveAt(arCount);
                //        }
                //        else
                //        {
                //            arCount++;
                //        }
                //    }
                //}
                i3d = 0;
                foreach (string Helix3DtypeName in Helix3Dnames)
                {
                    ArrayList myTypes = Helix3Dtypes[i3d];
                    //some types are not useful for this
                    bool addThis = true;

                    if (Helix3DtypeName.StartsWith("<")) addThis = false;
                    //if (Helix3DtypeName == "PanHandler") addThis = false;
                    //if (Helix3DtypeName == "RotateHandler") addThis = false;
                    //if (Helix3DtypeName == "ZoomHandler") addThis = false;
                    //if (Helix3DtypeName == "ZoomRectangleHandler") addThis = false;

                    if (myTypes.Count > 0 && addThis)
                    {
                        PUPPIGUIController.PUPPIModuleKeepMenu Helix3DMenu = new PUPPIGUIController.PUPPIModuleKeepMenu("", 765, 58, 130, 25, 5, pmc);
                        Helix3DMenu.AddMenuButtonList(myTypes);
                        PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(Helix3DMenu, this, 450 - menuTabControl.Left, 22 - tabPUPPICADBasic.Top);
                        aHelix3DtabControl.TabPages.Add(Helix3DtypeName);
                        aHelix3DtabControl.TabPages[aHelix3DtabControl.TabPages.Count - 1].Controls.Add(Helix3DMenu.myFormControl);
                        Helix3DMenu.myFormControl.Dock = DockStyle.Fill;

                    }
                    i3d++;

                }

            }
            #endregion


            #region morePUPPIModules

            //supplied PUPPI module DLLs
            //menu titles not shown since each tab has title
            List<ArrayList> otherPUPPIModules = new List<ArrayList>();
            List<string> otherPUPPIModuleNames = new List<string>();

            //to show all the plugin DLLs with valid PUPPIModules
            List<string> loadedFiles = new List<string>(); 

            //load from DLLs 
            if (Directory.Exists(@".\PluginPUPPIModules") && !IsDirectoryEmpty(@".\PluginPUPPIModules"))
            {
                List<string> cloadedFiles = new List<string>(); 
                otherPUPPIModules = PUPPIModel.AutomaticPUPPImodulesCreator.importPUPPIModulesFromFolder(@".\PluginPUPPIModules", out otherPUPPIModuleNames,out cloadedFiles );
                loadedFiles.AddRange(cloadedFiles);  
                int moduleDLLCount = 0;
                foreach (string otherPUPPIModuleName in otherPUPPIModuleNames)
                {
                    ArrayList myTypes = otherPUPPIModules[moduleDLLCount];
                    if (myTypes.Count > 0)
                    {
                       
                        PUPPIGUIController.PUPPIModuleKeepMenu myTypesMenu = new PUPPIGUIController.PUPPIModuleKeepMenu("", 765, 58, 130, 25, 5, pmc);
                        myTypesMenu.AddMenuButtonList(myTypes);
                        PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(myTypesMenu, this, 450 - menuTabControl.Left, 22 - tabPUPPICADBasic.Top);
                        plugTabControl.TabPages.Add(otherPUPPIModuleName);
                        plugTabControl.TabPages[plugTabControl.TabPages.Count - 1].Controls.Add(myTypesMenu.myFormControl);
                        myTypesMenu.myFormControl.Dock = DockStyle.Fill;

                    }
                    moduleDLLCount++;

                }
                //now subfolders
                DirectoryInfo directory = new DirectoryInfo(@".\PluginPUPPIModules");
                DirectoryInfo[] directories = directory.GetDirectories();

                foreach (DirectoryInfo folder in directories)
                {
                   otherPUPPIModules = new List<ArrayList>();
                   otherPUPPIModuleNames = new List<string>();
                   otherPUPPIModules = PUPPIModel.AutomaticPUPPImodulesCreator.importPUPPIModulesFromFolder(folder.FullName, out otherPUPPIModuleNames,out cloadedFiles );
                   loadedFiles.AddRange(cloadedFiles);  
                    moduleDLLCount = 0;

                   plugTabControl.TabPages.Add(folder.Name);

                   TabControl folderTab = new TabControl(); 
                    plugTabControl.TabPages[plugTabControl.TabPages.Count - 1].Controls.Add(folderTab );
                    folderTab.Dock = DockStyle.Fill;
                    folderTab.TabPages.Clear();   
                    
                    foreach (string otherPUPPIModuleName in otherPUPPIModuleNames)
                   {
                       ArrayList myTypes = otherPUPPIModules[moduleDLLCount];
                       if (myTypes.Count > 0)
                       {
                           PUPPIGUIController.PUPPIModuleKeepMenu myTypesMenu = new PUPPIGUIController.PUPPIModuleKeepMenu("", 765, 58, 130, 25, 5, pmc);
                           myTypesMenu.AddMenuButtonList(myTypes);
                           PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(myTypesMenu, this, 450 - menuTabControl.Left, 22 - tabPUPPICADBasic.Top);
                           folderTab.TabPages.Add(otherPUPPIModuleName);
                          folderTab.TabPages[folderTab.TabPages.Count - 1].Controls.Add(myTypesMenu.myFormControl);
                           myTypesMenu.myFormControl.Dock = DockStyle.Fill;

                       }
                       moduleDLLCount++;

                   }
                }
            }


            #endregion

            try
            {
                #region dynamicloadedmodules
                //load dll files saved from form
                List<string> toremovedlls = new List<string>();
                List<string> allstoreddlls = new List<string>();
                string ddllerror = "";

                List<Assembly> loadedAssemblies=new List<Assembly>();  
                //load all other assemblies in every folder to ensure it can get types fromr eferenced assemblies
                //not the most performant way


                List<string> folderNames = new List<string>();
                List<string> loadedAs=new List<string>();

                foreach (string dPath in Properties.Settings.Default.generateModulesFrom)
                {
                    folderNames.Add(Path.GetDirectoryName(dPath));
                    //string[] currentFolderDLLS = System.IO.Directory.GetFiles(Path.GetDirectoryName(dPath), "*.dll");
                    
                    if (dPath.EndsWith("mtps"))
                    {
                        List<string>MTPSNames = new List<string>();
                        List<ArrayList> MTPSModuleLists = PUPPIModel.AutomaticPUPPImodulesCreator.makePUPPImoduleListsFromMTPS(dPath, out MTPSNames);
                        string MTPSTopTab = Path.GetFileNameWithoutExtension(dPath);


                        dllTabControl.TabPages.Add(MTPSTopTab);
                        TabPage newTopTab = dllTabControl.TabPages[dllTabControl.TabPages.Count - 1];
                        //subtabs for each type
                        TabControl typeTabs = new TabControl();
                        typeTabs.Dock = DockStyle.Fill;
                        newTopTab.Controls.Add(typeTabs);
                        typeTabs.TabPages.Clear();
                        int cnter = 0;
                        foreach (string foundTypeName in MTPSNames)
                        {
                            //check through filters
                            ArrayList typeModules = MTPSModuleLists[cnter];

                           

                            if (typeModules.Count == 0)
                            {
                                cnter++;
                                continue;
                            }
                            typeTabs.TabPages.Add(foundTypeName);
                            //generate new menu
                            PUPPIGUIController.PUPPIModuleKeepMenu dynamicallyGeneratedModulesMenu = new PUPPIGUIController.PUPPIModuleKeepMenu("", 765, 58, 130, 25, 5, pmc);
                            //add modules
                           // try
                            //{
                                dynamicallyGeneratedModulesMenu.AddMenuButtonList(typeModules);
                            //}
                            ///catch
                            //{

                            //}
                                //add menu to form
                            PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(dynamicallyGeneratedModulesMenu, this, 450 - menuTabControl.Left, 22 - tabPUPPICADBasic.Top);
                            typeTabs.TabPages[typeTabs.TabPages.Count - 1].Controls.Add(dynamicallyGeneratedModulesMenu.myFormControl);
                            dynamicallyGeneratedModulesMenu.myFormControl.Dock = DockStyle.Fill;
                            cnter++;
                        }
                        //foreach (string ds in currentFolderDLLS)
                        //{
                        //    if (loadedAs.Contains(ds) == false)
                        //    {
                        //        try
                        //        {
                        //            Assembly.UnsafeLoadFrom(ds);
                        //        }
                        //        catch
                        //        {

                        //        }
                        //    }
                        //}
                    
                    }
                    else
                    {
                        Assembly myDLLA = null;
                        try
                        {
                            //  myDLLA = Assembly.LoadFile(dPath);
                            myDLLA = Assembly.UnsafeLoadFrom(dPath);
                            loadedAssemblies.Add(myDLLA);
                            loadedAs.Add(dPath);
                            //foreach (string ds in currentFolderDLLS)
                            //{
                            //    if (loadedAs.Contains(ds) == false)
                            //    {
                            //        try
                            //        {
                            //            Assembly.UnsafeLoadFrom(ds);
                            //        }
                            //        catch
                            //        {

                            //        }
                            //    }
                            //}       


                        }
                        catch (Exception exy)
                        {
                            myDLLA = null;
                            ddllerror = exy.ToString();
                        }





                        if (myDLLA != null)
                        {
                            //big try/catch

                            try
                            {
                                //get namespaces

                                List<string> namespaces = new List<string>();
                                //try
                                //{
                                foreach (var type in myDLLA.GetTypes())
                                {
                                    string ns = type.Namespace;
                                    if (!namespaces.Contains(ns))
                                    {
                                        namespaces.Add(ns);
                                    }

                                }
                                //}
                                //catch
                                //{

                                //}
                                foreach (string myNSName in namespaces)
                                {
                                    List<string> foundTypeNames = new List<string>();
                                    List<ArrayList> foundModules = new List<ArrayList>();
                                    try
                                    {
                                        foundModules = PUPPIModel.AutomaticPUPPImodulesCreator.makeNamespacePUPPImoduleLists(myNSName, true, out foundTypeNames);
                                    }
                                    catch
                                    {

                                    }
                                    ////extra check for now
                                    //foreach (ArrayList arl in foundModules )
                                    //{
                                    //    for (int i = 0; i < arl.Count;i++ )
                                    //    {
                                    //        PUPPIModule ppm = PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(arl[i] as Type); 
                                    //            string sss = ppm.ToString();

                                    //    }
                                    //}


                                    //}
                                    //catch
                                    //{
                                    //    foundModules = new List<ArrayList>();
                                    //    foundTypeNames = new List<string>();  
                                    //}
                                    //create menus
                                    if (foundTypeNames.Count > 0)
                                    {
                                        string newTopTabName = "null";
                                        if (myNSName != null)
                                        {
                                            //get last portion of namespace name
                                            if (myNSName.Contains("."))
                                            {
                                                char[] splitNS = { '.' };
                                                newTopTabName = myNSName.Split(splitNS)[myNSName.Split(splitNS).Length - 1];
                                            }
                                            else
                                            {
                                                newTopTabName = myNSName;
                                            }

                                            //check through filters for namespace name
                                            bool ignoreNamespace = false;
                                            foreach (string ignoreMe in Properties.Settings.Default.ignoreModuleFilter)
                                            {

                                                if (newTopTabName.Contains(ignoreMe))
                                                {
                                                    ignoreNamespace = true;
                                                    break;
                                                }
                                            }

                                            if (ignoreNamespace) continue;

                                            //namespace tab
                                            dllTabControl.TabPages.Add(newTopTabName);
                                            TabPage newTopTab = dllTabControl.TabPages[dllTabControl.TabPages.Count - 1];
                                            //subtabs for each type
                                            TabControl typeTabs = new TabControl();
                                            typeTabs.Dock = DockStyle.Fill;
                                            newTopTab.Controls.Add(typeTabs);
                                            typeTabs.TabPages.Clear();
                                            int cnter = 0;
                                            foreach (string foundTypeName in foundTypeNames)
                                            {
                                                //check through filters
                                                ArrayList typeModules = foundModules[cnter];

                                                foreach (string ignoreMe in Properties.Settings.Default.ignoreModuleFilter)
                                                {

                                                    //check for type name
                                                    if (foundTypeName.Contains(ignoreMe))
                                                    {
                                                        typeModules.Clear();
                                                        break;
                                                    }

                                                    //check for method name
                                                    for (int tCount = 0; tCount < typeModules.Count; tCount++)
                                                    {
                                                        object o = typeModules[tCount];

                                                        if ((o as Type).Name.Contains(ignoreMe))
                                                        {
                                                            typeModules.RemoveAt(tCount);
                                                        }

                                                    }
                                                }

                                                if (typeModules.Count == 0)
                                                {
                                                    cnter++;
                                                    continue;
                                                }
                                                typeTabs.TabPages.Add(foundTypeName);
                                                //generate new menu
                                                PUPPIGUIController.PUPPIModuleKeepMenu dynamicallyGeneratedModulesMenu = new PUPPIGUIController.PUPPIModuleKeepMenu("", 765, 58, 130, 25, 5, pmc);
                                                //add modules
                                                //try
                                                //{
                                                    dynamicallyGeneratedModulesMenu.AddMenuButtonList(typeModules);
                                                //}
                                                //catch
                                                //{

                                                //}
                                                    //add menu to form
                                                PUPPIGUIController.FormTools.AddPUPPIModuleKeepMenutoForm(dynamicallyGeneratedModulesMenu, this, 450 - menuTabControl.Left, 22 - tabPUPPICADBasic.Top);
                                                typeTabs.TabPages[typeTabs.TabPages.Count - 1].Controls.Add(dynamicallyGeneratedModulesMenu.myFormControl);
                                                dynamicallyGeneratedModulesMenu.myFormControl.Dock = DockStyle.Fill;
                                                cnter++;
                                            }
                                        }
                                    }
                                    Path.GetDirectoryName(dPath);
                                
                                }

                            }
                            catch (Exception exy)
                            {
                                myDLLA = null;
                                ddllerror = exy.ToString();
                            }
                            if (myDLLA == null)
                            {
                                //    MessageBox.Show("Error loading assembly " + dPath + ". It will be removed from the list.");
                                toremovedlls.Add(dPath);
                            }
                            else
                            {
                                allstoreddlls.Add(dPath);
                            }

                        }


                    }
                }
                //remove the ones that get errors
                string a2r = "";
                foreach (string removeme in toremovedlls)
                {
                    a2r = a2r + removeme + " ";

                }
                //clear settings and add all valid dlls
                Properties.Settings.Default.generateModulesFrom.Clear();
                foreach (string dPath in allstoreddlls)
                {
                    Properties.Settings.Default.generateModulesFrom.Add(dPath);
                }
                Properties.Settings.Default.Save();

                if (toremovedlls.Count > 0)
                {

                    using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@".\errorlog.txt"))
                    {

                        file.WriteLine(DateTime.Now.ToString());
                        file.WriteLine(ddllerror);
                    }
                    MessageBox.Show("Some dlls could not be converted to PUPPIModules: " + a2r + ". These dlls have been removed from the list and will not be loaded at next restart. PUPPICAD will now close.Please check errorlog.txt");

                    Environment.Exit(4);
                }
                #endregion
            }
            //temporary till PUPPI is changed
            catch (Exception exy)
            {
                try
                {
                    using (System.IO.StreamWriter file =
new System.IO.StreamWriter(@".\errorlog.txt"))
                    {

                        file.WriteLine(DateTime.Now.ToString());
                        file.WriteLine(exy.ToString());
                    }
                    Properties.Settings.Default.generateModulesFrom.Clear();

                    Properties.Settings.Default.Save();
                    MessageBox.Show("Critical error. Clearing list of dll files to load for runtime PUPPIModule creation from settings. Please restart PUPPICAD. See errorlog.txt for details.If error persists, remove any newly added files from the PluginPUPPIModules folder.");
                    Environment.Exit(4);
                }
                catch
                {
                    MessageBox.Show("Critical error. Clearing list of dll files to load for runtime PUPPIModule creation from settings. Please restart PUPPICAD.If error persists, remove any newly added files from the PluginPUPPIModules folder.");
                    Properties.Settings.Default.generateModulesFrom.Clear();

                    Properties.Settings.Default.Save();
                    Environment.Exit(4);
                }
            }
            //file dropdown menu
            PUPPIGUIController.PUPPIDropDownMenu pdmfile = new PUPPIGUIController.PUPPIDropDownMenu(pmc, "File");
            pdmfile.addStandardFileMenuOptions();
            ////add a custom command to file menu to open a form to set up DLLs which are loaded as PUPPI modules
            //setUpLoadDLLs formL = new setUpLoadDLLs();
            //Action changeStuff = () => formL.ShowDialog();
            //pdmfile.addCustomCommandToMenu("Configure Runtime-created PUPPIModules", changeStuff, "Set up the DLL files that are converted to PUPPIModules when starting the program", false);
            //PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(pdmfile, this);
            ////add to layout
            //dropDownLayoutPanel.Controls.Add(pdmfile.myFormControl, 0, 0);


            //add a custom command to file menu to open options
            PUPPICADOptions  formO = new PUPPICADOptions ();
            formO.loadedPlugins = loadedFiles;
            Action runOptions = () => formO.ShowDialog();
            pdmfile.addCustomCommandToMenu("Options", runOptions, "Change preferences. Restart required for changes to take effect", false);
            PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(pdmfile, this);
            //add to layout
            dropDownLayoutPanel.Controls.Add(pdmfile.myFormControl, 0, 0);



            //edit dropdown menu
            PUPPIGUIController.PUPPIDropDownMenu pdmedit = new PUPPIGUIController.PUPPIDropDownMenu(pmc, "Edit");
            pdmedit.addStandardEditMenuOptions();
            PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(pdmedit, this, 40, 0);
            dropDownLayoutPanel.Controls.Add(pdmedit.myFormControl, 1, 0);

            //view dropdown menu
            PUPPIGUIController.PUPPIDropDownMenu pdmview = new PUPPIGUIController.PUPPIDropDownMenu(pmc, "View");
            pdmview.addStandardViewMenuOptions();
            PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(pdmview, this, 80, 0);
            dropDownLayoutPanel.Controls.Add(pdmview.myFormControl, 2, 0);

            //help dropdown menu
            PUPPIGUIController.PUPPIDropDownMenu pdmhelp = new PUPPIGUIController.PUPPIDropDownMenu(pmc, "Help");
            //display video tutorials
            Action videoTutorials = () =>
            {
                ProcessStartInfo webTutors = new ProcessStartInfo("http://pupi.co/index.php/try-pupicad-beta/puppicad-tutorials");
                Process.Start(webTutors);
            };
            pdmhelp.addCustomCommandToMenu("PUPPICAD Tutorials", videoTutorials, "PUPPICAD on YouTube", false);

          

            pdmhelp.addStandardHelpMenuOptions(Uri.UnescapeDataString(new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath) + @"/PUPPI-user-help.chm");
            //display license for NCalc
            Action ncalcLicense = () =>
            {
                try
                {
                    ProcessStartInfo licenses = new ProcessStartInfo(@".\PUPPIBetaLicense.txt");
                    Process.Start(licenses);
                }
                catch
                {
                    MessageBox.Show("Couldn't load file!");  
                }
            };
            pdmhelp.addCustomCommandToMenu("Licenses", ncalcLicense, "License information on libraries used by PUPPICAD.", false);


            //display readme
            Action showReadme = () =>
            {
                try
                {
                    ProcessStartInfo readme = new ProcessStartInfo(@".\readme.txt");
                    Process.Start(readme);
                }
                catch
                {
                    MessageBox.Show("Couldn't load file");
                }
            };
            pdmhelp.addCustomCommandToMenu("PUPPICAD Readme", showReadme, "Extending PUPPICAD, troubleshooting, report issues....", false);


            PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(pdmhelp, this, 120, 0);
            dropDownLayoutPanel.Controls.Add(pdmhelp.myFormControl, 3, 0);

            //completely new menu
            //menu to add prebuilt objects to the canvas
            PUPPIGUIController.PUPPIDropDownMenu constructsHelper = customDropdowns.dropdownCreator.makeCADConstructsMenu(pmc);
            //coordinates not needed really since it's in a table layout panel
            PUPPIGUIController.FormTools.AddPUPPIDropDownMenutoForm(constructsHelper, this);
            dropDownLayoutPanel.Controls.Add(constructsHelper.myFormControl, 4, 0);
            constructsHelper.myFormControl.Dock = DockStyle.Fill;   
            //example of event handling
            //we ask if we should clear the CAD window on new canvas, load canvas and import canvas
            pmc.newCanvasOperationEvent += pmc_newCanvasOperationEvent;
            pmc.loadCanvasOperationEvent += pmc_loadCanvasOperationEvent;
            //hide or show module menus for locked and unlocked canvas
            pmc.lockCanvasOperationEvent += pmc_lockCanvasOperationEvent;
            pmc.unlockCanvasOperationEvent += pmc_unlockCanvasOperationEvent;
            startPUPPICADPartialServer();
            
            
            
            splashS.Close();


            

            //List<Point3D> testPoints = new List<Point3D>();  
            //Point3D p111 = new Point3D(-1, 0, 0);
            //testPoints.Add(p111);
            //Point3D p222 = new Point3D(0, 0, 1);
            //testPoints.Add(p222);
            //Point3D p333 = new Point3D(1, 0, 0);
            //testPoints.Add(p333);
            //PUPPICAD.HelperClasses.PUPPIPolyLine3D pppp = new PUPPICAD.HelperClasses.PUPPIPolyLine3D(testPoints);
            //PUPPICAD.HelperClasses.PUPPIPolyLine3D ppp1 = pppp.refactorPolyLine(5); 

    
        }

        void pmc_unlockCanvasOperationEvent(object sender, EventArgs e)
        {
          //  menuLayoutPanel.RowStyles[1].Height = 50;
            menuLayoutPanel.RowStyles[1].SizeType = SizeType.AutoSize;
            menuLayoutPanel.RowStyles[0].SizeType = SizeType.Absolute;
            menuLayoutPanel.RowStyles[0].Height = 30;
            mainLayoutPanel.RowStyles[0].Height = 30;
            mainLayoutPanel.RowStyles[0].SizeType = SizeType.Percent;
 
            mainLayoutPanel.RowStyles[1].SizeType = SizeType.Percent ;
            mainLayoutPanel.RowStyles[1].Height = 70; 
            this.Refresh(); 
            

        }

        void pmc_lockCanvasOperationEvent(object sender, EventArgs e)
        {

            menuLayoutPanel.RowStyles[1].SizeType = SizeType.Absolute; 
            menuLayoutPanel.RowStyles[1].Height =0;
           menuLayoutPanel.RowStyles[0].SizeType = SizeType.Absolute;
           menuLayoutPanel.RowStyles[0].Height = 30; 
           mainLayoutPanel.RowStyles[0].Height = 32;
           mainLayoutPanel.RowStyles[0].SizeType = SizeType.Absolute;
           mainLayoutPanel.RowStyles[1].SizeType = SizeType.AutoSize;     
           this.Refresh(); 
            
        }

        void pmc_loadCanvasOperationEvent(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Keep CAD Objects?", "About to refresh PUPPI canvas", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                pvc.keepCADWindowObjects();
            }
            else
            {
                pvc.clearCADWindowObjects();
            }
        }
        //we ask if we should clear the CAD window on new canvas, load canvas and import canvas
        void pmc_newCanvasOperationEvent(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Keep CAD Objects?", "About to refresh PUPPI canvas", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                pvc.keepCADWindowObjects();
            }
            else
            {
                pvc.clearCADWindowObjects();
            }

        }

        void startPUPPICADPartialServer()
        {

            int prts = 0;
            string ips = "";
            string pps = "";
            bool isRunning = false;
            string savedSettings = Properties.Settings.Default.serverSettings;
            if (savedSettings != null && savedSettings != String.Empty)
            {
                try
                {
                    int sc1 = 45;
                    int sc2 = 3;
                    string[] seppa = { "_|_|_" };
                    string[] splitta = savedSettings.Split(seppa, StringSplitOptions.None);
                    prts = Convert.ToInt16(splitta[2]);
                    ips = splitta[1];
                    string smps = splitta[0];
                    byte[] bita = new byte[smps.Length / 2];
                    byte[] sbita = Encoding.ASCII.GetBytes(smps).Reverse().ToArray();
                    int j = 0;
                    for (int i = 0; i < sbita.Length - 1; i += 2)
                    {
                        byte b1 = sbita[i];
                        byte b2 = sbita[i + 1];
                        int it1 = b1;
                        if (b2 == 124) it1 += (126 - 32);
                        if (b2 == 125) it1 += 126;
                        if (b2 == 126) it1 += 0;
                        byte balao = Convert.ToByte(it1 - sc1 - sc2);
                        bita[j] = balao;
                        j++;
                    }
                    pps = Encoding.ASCII.GetString(bita);
                    isRunning = Convert.ToBoolean(splitta[3]);
                }
                catch
                {
                    MessageBox.Show("Error reading server settings");  
                    pps = "";
                    ips = "error";
                    prts = 0;
                    isRunning = false;
                }
            }
        if (isRunning )
        {
            //start server
            string conn = "error";
            try
            {
                conn=PUPPIServer.PUPPICanvasTCPServer.startServer(pmc, ips, prts, pps, false);
            }
            catch
            {
                conn = "error";
            }
            if (conn=="error" )
            {
                MessageBox.Show("Failed to start server!");
            }
            else
            {
                this.Text += "...Server running...";
            }

        }
        }


        //makes a custom renderer for nodes - will be used on the NOT module
        //inputs and outputs are also custom
        public PUPPINodeCustomRenderer makeTestCustomRenderer1()
        {
            PUPPINodeCustomRenderer myCustomNodeRenderer = new PUPPINodeCustomRenderer("render1");
            myCustomNodeRenderer.useDefaultCaption = false;
            myCustomNodeRenderer.addCaption3D("Test Caption", 0, 0, 0, 0.5, 255, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.negativeYDirection, PUPPICustomRenderer.PUPPIimageOrientation.vertical);
            //myCustomNodeRenderer.addImage3D(@"C:\scratch\securedownload.png", 1, 1, 1, 2, 1, PUPPICustomRenderer.PUPPIimageDirection.positiveYDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);  
            //pcr.addBox3D(-0.2, -0.2, 0.2, 0.2, 0.2, 0.2, 255, 255, 0);
            myCustomNodeRenderer.addSphere3D(0, -0.05, 0.0, 0.25, 255, 255, 255);
            myCustomNodeRenderer.addSphere3D(-0.1, 0.3, 0.2, 0.05, 0, 0, 255);
            myCustomNodeRenderer.addSphere3D(-0.1, -0.3, 0.2, 0.05, 0, 0, 255);

            myCustomNodeRenderer.addBox3D(-1, -0.01, 0, 1, 0.01, 0.02, 100, 100, 100);
            myCustomNodeRenderer.addBox3D(-0.01, -1, 0, 0.01, 1, 0.02, 100, 100, 100);

            //pcr.addFile3D(@"C:\scratch\wvsmall.obj");
            //myCustomNodeRenderer.acceptsNodesAbove = false;
            //custom renderer for input as well
            PUPPICustomRenderer myCustomInputRenderer = new PUPPICustomRenderer();
            myCustomInputRenderer.useDefaultCaption = false;
            myCustomInputRenderer.addSphere3D(-0.5, 0, 0.15, 0.1, 255, 0, 0);
            myCustomNodeRenderer.addCustomInputRenderer(myCustomInputRenderer);
            //custom renderer for output
            PUPPICustomRenderer myCustomOutputRenderer = new PUPPICustomRenderer();
            myCustomOutputRenderer.addSphere3D(0.5, 0, 0.15, 0.1, 0, 255, 255);
            myCustomOutputRenderer.useDefaultCaption = false;
            myCustomNodeRenderer.addCustomOutputRenderer(myCustomOutputRenderer);
            return myCustomNodeRenderer;
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
                outputs.Add("");
                outputnames.Add("Value");
                //default color override
                setNodeColor(0, 0.5, 1);
            }
            //value is set when user double clicks on node
            //also shows how to do inputs for list processing
            public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
            {


                //get the previous value(s) to display
                string entered = "";
                try
                {
                    if (countListMode == 0)
                    {
                        entered = dblclkpreviousoutputs[0].ToString();
                    }
                    else
                    {
                        ArrayList pvo = dblclkpreviousoutputs[0] as ArrayList;
                        entered = Convert.ToString(pvo[inlist]);
                    }
                }
                catch
                {
                    entered = "";
                }

                if (countListMode == 0)
                {

                    if (PUPPIFormUtils.formutils.InputBox("Please enter a string", "Enter text:", ref entered) == System.Windows.Forms.DialogResult.Cancel)
                    {


                        try
                        {
                            entered = Convert.ToString(dblclkpreviousoutputs[0]);

                        }
                        catch
                        {
                            entered = "";
                        }
                    }
                }
                else
                {
                    if (PUPPIFormUtils.formutils.InputBox("Please enter string " + (inlist + 1).ToString() + "/" + countListMode.ToString(), "Enter text:", ref entered) == System.Windows.Forms.DialogResult.Cancel)
                    {

                        try
                        {
                            ArrayList pvo = dblclkpreviousoutputs[0] as ArrayList;
                            entered = Convert.ToString(pvo[inlist]);
                        }
                        catch
                        {
                            entered = "";
                        }
                    }
                }
                usercodeoutputs[0] = entered;
            }


        }

        //custom module: browse for file,r eturn path
        public class PUPPIFileBrowser : PUPPIModule
        {
            //constructor
            public PUPPIFileBrowser()
                //needs to call base constructor
                : base()
            {

                name = "File Browser";
                outputs.Add("");
                outputnames.Add("File Path");
                //default color override
                setNodeColor(0, 0.5, 0.9);
                completeProcessOverride = true;
                completeDoubleClickOverride = true;
            }
            //value is set when user double clicks on node
            //also shows how to do inputs for list processing
            public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
            {


                //get the previous value(s) to display
                string entered = "";
                try
                {

                    entered = dblclkpreviousoutputs[0].ToString();
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                    if (result == DialogResult.OK) // Test result.
                    {
                        entered = openFileDialog1.FileName;
                    }

                }
                catch
                {
                    entered = "error";
                }

                outputs[0] = entered;


            }
        }

        //custom module: string input evaluated as NCalc expression
        public class PUPPIStringEvalInput : PUPPIModule
        {
            //constructor
            public PUPPIStringEvalInput()
                //needs to call base constructor
                : base()
            {

                name = "String Eval";
                description = "String input evaluated as NCalc expression. Example: Sqrt(4). More at https://ncalc.codeplex.com/";
                //make sure we add something of same type as expected output for when this is loaded to retrieve the entered value properly
                outputs.Add(0.0);
                outputnames.Add("Value");
                
            }
            //value is set when user double clicks on node
            //also shows how to do inputs for list processing
            public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
            {


                //get the previous value(s) to display
                string entered = "";
                try
                {
                    if (countListMode == 0)
                    {
                        entered = dblclkpreviousoutputs[0].ToString();
                    }
                    else
                    {
                        ArrayList pvo = dblclkpreviousoutputs[0] as ArrayList;
                        entered = Convert.ToString(pvo[inlist]);
                    }
                }
                catch
                {
                    entered = "";
                }

                if (countListMode == 0)
                {

                    if (PUPPIFormUtils.formutils.InputBox("Please enter an expression to evaluate.Ex: Sin(0) ", "Enter text:", ref entered) == System.Windows.Forms.DialogResult.Cancel)
                    {


                        try
                        {
                            entered = Convert.ToString(dblclkpreviousoutputs[0]);

                        }
                        catch
                        {
                            entered = "";
                        }
                    }
                }
                else
                {
                    if (PUPPIFormUtils.formutils.InputBox("Please enter string to evaluate " + (inlist + 1).ToString() + "/" + countListMode.ToString(), "Enter text:", ref entered) == System.Windows.Forms.DialogResult.Cancel)
                    {

                        try
                        {
                            ArrayList pvo = dblclkpreviousoutputs[0] as ArrayList;
                            entered = Convert.ToString(pvo[inlist]);
                        }
                        catch
                        {
                            entered = "";
                        }
                    }
                }
                try
                {
                    usercodeoutputs[0] = Convert.ToDouble (new NCalc.Expression(entered).Evaluate());
                }
                catch
                {
                    usercodeoutputs[0] = "error";
                }
            }

           


        }

        //custom module: Char input
        public class PUPPICharInput : PUPPIModule
        {
            //constructor
            public PUPPICharInput()
                //needs to call base constructor
                : base()
            {

                name = "Character input";
                description = "Outputs variable of type char, useful for Advanced String operations.";
                outputs.Add(' ');
                outputnames.Add("Value");
                //default color override
                setNodeColor(0, 0.6, 1);
            }
            //value is set when user double clicks on node
            //also shows how to do inputs for list processing
            public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
            {


                //get the previous value(s) to display
                char entered = ' ';
                try
                {
                    if (countListMode == 0)
                    {
                        entered = Convert.ToChar (dblclkpreviousoutputs[0]);
                    }
                    else
                    {
                        ArrayList pvo = dblclkpreviousoutputs[0] as ArrayList;
                        entered = Convert.ToChar(pvo[inlist]);
                    }
                }
                catch
                {
                    entered = ' ';
                }
                string en = "";
                if (countListMode == 0)
                {

                    if (PUPPIFormUtils.formutils.InputBox("Please enter a character", "Enter text:", ref en) == System.Windows.Forms.DialogResult.Cancel)
                    {


                        try
                        {
                            entered = Convert.ToChar(dblclkpreviousoutputs[0]);

                        }
                        catch
                        {
                            entered = ' ';
                        }
                    }
                    if (en.Length >0 )
                    entered = en[0];
                }
                else
                {
                    
                    if (PUPPIFormUtils.formutils.InputBox("Please enter Character " + (inlist + 1).ToString() + "/" + countListMode.ToString(), "Enter text:", ref en) == System.Windows.Forms.DialogResult.Cancel)
                    {

                        try
                        {
                            ArrayList pvo = dblclkpreviousoutputs[0] as ArrayList;
                            entered = Convert.ToChar(pvo[inlist]);
                        }
                        catch
                        {
                            entered = ' ';
                        }
                    }
                    if (en.Length > 0)
                        entered = en[0];
                }
                usercodeoutputs[0] = entered;
            }


        }

        //creates a custom PUPPIModule from scratch
        public class PUPPINOT : PUPPIModule
        {
            public PUPPINOT()
                : base()
            {
                name = "NOT";
                outputs.Add(0);
                outputnames.Add("Result");
                description = "NOT Operation";
                inputnames.Add("Input");

                inputs.Add(new PUPPIInParameter());

            }
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

            public override void gestureMe_userCode(double startXRatio, double startYRatio, double startZRatio, double endXRatio, double endYRatio, double endZRatio)
            {
                MessageBox.Show("Dragged " + startXRatio.ToString() + " " + startYRatio.ToString() + " " + startZRatio.ToString() + " to " + endXRatio.ToString() + " " + endYRatio.ToString() + " " + endZRatio.ToString());
            }
            public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
            {
                MessageBox.Show("double clicked " + clickXRatio.ToString() + " " + clickYRatio.ToString() + " " + clickZRatio.ToString());
            }
        }

        //helper functions for Ncalc expressions
        public class SetExpressionParameter : PUPPIModule
        {
            public SetExpressionParameter()
                : base()
            {
                name = "SetExpParam";
                cleancap = "Set Expr Param";
                description = "Add a new parameter or set a value to an existing parameter. Parameter names must be enclose in [] in expression string but not here.";
                inputs.Add(new PUPPIInParameter());
                inputnames.Add("Expression");
                inputs.Add(new PUPPIInParameter());
                inputnames.Add("Par. Name");
                inputs.Add(new PUPPIInParameter());
                inputnames.Add("Par. Val");
                outputs.Add(null);
                outputnames.Add("Modified Exp.");

            }
            public override void process_usercode()
            {
                try
                {
                    NCalc.Expression ex = usercodeinputs[0] as NCalc.Expression;
                    if (ex == null)
                    {
                        usercodeoutputs[0] = "Null exp.";
                        return;
                    }
                    string s = usercodeinputs[1].ToString();
                    if (s == "")
                    {
                        usercodeoutputs[0] = "Invalid par. name.";
                        return;
                    }
                    s = s.Replace(" ", "");
                    //get rid of brackets if used
                    s = s.Replace("[", "");
                    s = s.Replace("]", "");

                    double d = Convert.ToDouble(usercodeinputs[2]);
                    //clone expression
                    NCalc.Expression nex = PUPPIModel.Utilities.CloneObject(ex) as NCalc.Expression;
                    //foreach (KeyValuePair<string,object>kv in ex.Parameters  )
                    //{
                    //    nex.Parameters.Add(kv.Key, kv.Value);
                    //}
                    if (nex.Parameters.ContainsKey(s))
                    {
                        nex.Parameters[s] = d;
                    }
                    else
                    {
                        nex.Parameters.Add(s, d);
                    }
                    usercodeoutputs[0] = nex;
                }
                catch
                {
                    usercodeoutputs[0] = "error";
                }
            }
        }

        //helper functions for Ncalc expressions
        public class GetExpressionParameter : PUPPIModule
        {
            public GetExpressionParameter()
                : base()
            {
                name = "GetExpParam";
                cleancap = "Get Expr Param";
                description = " Get a value of an existing parameter. Parameter names must be enclose in [] in expression string but not here.";
                inputs.Add(new PUPPIInParameter());
                inputnames.Add("Expression");
                inputs.Add(new PUPPIInParameter());
                inputnames.Add("Par. Name");
                outputs.Add(null);
                outputnames.Add("Value");

            }
            public override void process_usercode()
            {
                try
                {
                    NCalc.Expression ex = usercodeinputs[0] as NCalc.Expression;
                    if (ex == null)
                    {
                        usercodeoutputs[0] = "Null exp.";
                        return;
                    }
                    string s = usercodeinputs[1].ToString();
                    if (s == "")
                    {
                        usercodeoutputs[0] = "Invalid par. name.";
                        return;
                    }

                    s = s.Replace(" ", "");
                    //get rid of brackets if used
                    s = s.Replace("[", "");
                    s = s.Replace("]", "");
                    if (ex.Parameters.ContainsKey(s))
                    {
                        usercodeoutputs[0] = ex.Parameters[s];
                    }
                    else
                    {
                        usercodeoutputs[0] = "Not found";
                    }

                }
                catch
                {
                    usercodeoutputs[0] = "error";
                }
            }
        }

        //a sample class to convert into PUPPI modules
        //displays alerts

        public class AlertMessage
        {
            public string myAlertMessage = "";
            public AlertMessage()
            {
                myAlertMessage = "Press OK to Continue";

            }
            public AlertMessage(string alertMsg)
            {
                myAlertMessage = alertMsg;
            }
            public void displayAlert()
            {
                MessageBox.Show(myAlertMessage);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (PUPPIDEBUG.PUPPIDebugger.debugenabled == true)
            {
                PUPPIDEBUG.debuglog dede = new PUPPIDEBUG.debuglog();
                dede.writelog(PUPPIDEBUG.PUPPIDebugger.logstr);
                dede.Show();
            }
            else
            {
                Application.Exit();
            }
        }

        private void debugcheck_CheckedChanged(object sender, EventArgs e)
        {
            if (debugcheck.Checked == true)
            {
                PUPPIDEBUG.PUPPIDebugger.debugenabled = true;
            }
            else
            {
                PUPPIDEBUG.PUPPIDebugger.debugenabled = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://www.youtube.com/watch?v=sABtS8uj8Hw&list=PLwvRxDSJB8uvEjnbA9HL3Y1UGpf-NYT8q");
            Process.Start(sInfo);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        //loads an example program
        private void myFirstProgram_Click(object sender, EventArgs e)
        {
            const string message =
      "Are you sure that you would like to load example?";
            const string caption = "Unsaved programs will be lost";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);


            if (result == DialogResult.Yes)
            {
                pmc.openCanvasCommand(@".\Examples\myfirstprogram.xml");
            }
        }
        //prompt user if closing form from x button
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            const string message =
      "Are you sure that you want to exit PUPPICAD? Unsaved changes will be lost.";
            const string caption = "Program Exiting";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                // cancel the closure of the form.
                e.Cancel = true;
            }

        }

        //check folder empty
        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private void basicCADLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

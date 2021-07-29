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
//samples of modules and renderers to create custom interactions on the canvas
namespace interactionModules
{
    //function to determine bounding box of a model so it can be scaled and centered
    public static class interactionRendererFunctions
    {
        //makes a custom renderer for the slider node in 3D
        public static PUPPINodeCustomRenderer make3DRendererForSlider()
        {
            PUPPINodeCustomRenderer sliderRenderer = new PUPPINodeCustomRenderer("SliderRenderer");
            //base is a box
            sliderRenderer.addBox3D(-PUPPIGUISettings.nodeSide / 2, -PUPPIGUISettings.nodeSide / 2, -PUPPIGUISettings.nodeHeight / 2, PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeHeight * 1 / 4, 200, 200, 200);
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
            //this is in case we drag outside , to make sure position of slider remains consistent with result
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
                outputs[0] = resultValue;
            }

        }
       
    }

    public class IntSlider : PUPPIModule
    {
        int resultValue = 0;
        int lowerLimit = 0;
        int upperLimit = 10;
        public IntSlider()
            : base()
        {

            resultValue = 5;
            lowerLimit = 0;
            upperLimit = 10;
            name = "Integer Slider";
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
        public void updateMe(int rv)
        {

            //scale result to determine position
            double dp = 0;
            dp = (double)(rv - lowerLimit) / (double)(upperLimit - lowerLimit);

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
            //else
            //  pN.updateCircle2D(0, 0, -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.nodeSide * dp, PUPPIGUISettings.nodeSide * 0.125, 255, 0, 0, 1); 
            outputs[0] = resultValue;
            forceMyNodeToUpdate();


        }
        //dragging down and up sets the result sets the 
        public override void gestureMe_userCode(double startXRatio, double startYRatio, double startZRatio, double endXRatio, double endYRatio, double endZRatio)
        {
            if (endYRatio < -1) endYRatio = -1;
            if (endYRatio > 1) endYRatio = 1;
            resultValue = Convert.ToInt16 (Math.Round(lowerLimit + (upperLimit - lowerLimit) * endYRatio, 0));
            updateMe(resultValue);
        }

        public override void dragOver_visualUpdate_usercode(double startXRatio, double startYRatio, double startZRatio, double currentXRatio, double currentYRatio, double currentZRatio)
        {
            if (currentYRatio < -1) currentYRatio = -1;
            if (currentYRatio > 1) currentYRatio = 1;
            int rv = Convert.ToInt16(Math.Round(lowerLimit + (upperLimit - lowerLimit) * currentYRatio, 0));
            updateMe(rv);
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
            lowerLimit = Convert.ToInt16 (Math.Round(Convert.ToDouble(vals[0]),0));
            upperLimit = Convert.ToInt16 (Math.Round(Convert.ToDouble(vals[1]), 0));
            resultValue = Convert.ToInt16 (Math.Round(Convert.ToDouble(vals[2]), 0));
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
                    lowerLimit = Convert.ToInt16 (Math.Round(Convert.ToDouble(LLinputBoxResult), 0));
                    upperLimit = Convert.ToInt16 (Math.Round(Convert.ToDouble(ULinputBoxResult), 0));
                   if (lowerLimit==upperLimit  )
                   {
                       upperLimit = lowerLimit + 10;  
                   }
                    if (upperLimit < lowerLimit)
                    {

                        int swapper = lowerLimit;
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
                        lowerLimit = resultValue -10;
                        upperLimit = resultValue +10;
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
        public override void process_usercode()
        {
            updateMe(resultValue);
        }
    }

    public class ObjectSculptor : PUPPIModule
    {
        List<Point3D> storedPoints;
        double moveRadius = 0;
        ModelVisual3D todi = null;
        bool displayDeformer = false;
        string mAxis = "";
        double sizeMe;
        ModelVisual3D tempmod=null;
        public ObjectSculptor() : base()
        {
            completeGestureOverride = true;
            completeProcessOverride = true;
            completeDoubleClickOverride = true;
            maxChildren = 0;
            name = "Sculptor";
            description = "Shape Model3D or ModelVisual3D by dragging over. Returns changed ModelVisual3D";
            gestureDescription = "Updates model points";
            inputnames.Add("3D Object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Display Size");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Axis X/Y/Z");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Deform Radius");
            inputs.Add(new PUPPIInParameter());
            outputs.Add(null);
            outputnames.Add("Modified Object");
            storedPoints = new List<Point3D>();
            moveRadius = 0;
            todi = null;
            sizeMe = PUPPIGUISettings.nodeSide;
            tempmod = null; 
            
            
        }
        public override void process_usercode()
        {
            moveRadius = 0; 
            mAxis = "";
            object moodel = inputs[0].module.outputs[inputs[0].outParIndex];
            ModelVisual3D mV = null;
            List<Point3D> inputMPoints = new List<Point3D>();
            if (moodel is ModelVisual3D)
            {
               ModelVisual3D ml=moodel as ModelVisual3D;

               mV = PUPPICAD.HelperClasses.utilities.fullCloneModelVisual3D(ml);   
                
            }
            else if (moodel is Model3D)
            {
                Model3D mo=moodel as Model3D; 
                mV = new ModelVisual3D();
                mV.Content = mo.Clone(); 
            }

            try
            {
                mAxis = inputs[2].module.outputs[inputs[2].outParIndex].ToString() ;
            }
            catch
            {
                mAxis = "";
            }
            try
            {
                moveRadius=Convert.ToDouble(inputs[3].module.outputs[inputs[3].outParIndex])  ; 
            }
            catch
            {
                moveRadius = 0; 
            }
             sizeMe= Convert.ToDouble(inputs[1].module.outputs[inputs[1].outParIndex]);
            if (sizeMe <= 0)
            {
                outputs[0] = "invalid display size";
                return;
            }
            //check to see if same # of points
            inputMPoints = PUPPICAD.HelperClasses.utilities.get3DObjectPoints(mV);
            if (inputMPoints.Count!=storedPoints.Count    )
            {
                storedPoints.Clear();
                storedPoints.AddRange(inputMPoints);  
            }
           todi = PUPPICAD.HelperClasses.utilities.set3DObjectPoints(mV, storedPoints);
           outputs[0] = todi;
           displayDeformer = false; 
           if (todi != null) displayMe(todi,new Point3D());
           tempmod = PUPPICAD.HelperClasses.utilities.fullCloneModelVisual3D(todi); 
        }
        public override void gestureMe_userCode(double startXRatio, double startYRatio, double startZRatio, double endXRatio, double endYRatio, double endZRatio)
        {
           if (todi!=null && storedPoints.Count>0  && moveRadius>0   )
           {
               Rect3D bb = canvasRenderer.CanvasRenderHelper.getModelBoundingBox(todi);
               double nstx;
               double nsty;
               double nstz;
               double nsix;
               double nsiy;
               double nsiz;
               getMyNodeBoundingBox(out nstx,out nsty,out nstz,out nsix,out nsiy,out nsiz);
               //since height is not standard
               Point3D originalFocus = new Point3D(bb.X + bb.SizeX * startXRatio, bb.Y + bb.SizeY * startYRatio, bb.Z +   startZRatio*PUPPIGUISettings.nodeHeight * bb.SizeX/nsix  );
               Point3D endFocus = new Point3D(bb.X + bb.SizeX * endXRatio, bb.Y + bb.SizeY * endYRatio, bb.Z + endZRatio * PUPPIGUISettings.nodeHeight * bb.SizeX / nsix);

               storedPoints = moveMyPoints(originalFocus, endFocus);
               todi = PUPPICAD.HelperClasses.utilities.set3DObjectPoints(todi, storedPoints);
               outputs[0] = todi;
               displayDeformer = false;
               if (todi != null) displayMe(todi,new Point3D() ); 
              
           }
        }
        internal List<Point3D> moveMyPoints(Point3D originalFocus, Point3D endFocus)
        {
            List<Point3D> nsp = new List<Point3D>();
            foreach (Point3D pi in storedPoints)
            {
                Point3D p = pi;
                double distof = p.DistanceTo(originalFocus);
                if (distof < moveRadius)
                {
                    if (mAxis.ToLower() == "x")
                    {
                        double maxMoveDist = endFocus.X - originalFocus.X;

                        p.X += maxMoveDist - distof / moveRadius * maxMoveDist;

                    }
                    if (mAxis.ToLower() == "y")
                    {
                        double maxMoveDist = endFocus.Y - originalFocus.Y;

                        p.Y += maxMoveDist - distof / moveRadius * maxMoveDist;

                    }
                    if (mAxis.ToLower() == "z")
                    {
                        double maxMoveDist = endFocus.Z - originalFocus.Z;

                        p.Z += maxMoveDist - distof / moveRadius * maxMoveDist;

                    }
                }
                nsp.Add(p);
            }
            return nsp;
        }
        public override void dragOver_visualUpdate_usercode(double startXRatio, double startYRatio, double startZRatio, double currentXRatio, double currentYRatio, double currentZRatio)
        {
            displayDeformer = true; 
            if (PUPPIGUISettings.canvasMode==PUPPIGUISettings.CanvasModeEnum.ThreeDimensional   )
            {
                double nstx;
                double nsty;
                double nstz;
                double nsix;
                double nsiy;
                double nsiz;
                getMyNodeBoundingBox(out nstx, out nsty, out nstz, out nsix, out nsiy, out nsiz);
                Rect3D bb = canvasRenderer.CanvasRenderHelper.getModelBoundingBox(todi);
                Point3D originalFocus = new Point3D(bb.X + bb.SizeX * startXRatio, bb.Y + bb.SizeY * startYRatio, bb.Z + startZRatio * PUPPIGUISettings.nodeHeight * bb.SizeX / nsix);
                Point3D endFocus = new Point3D(bb.X + bb.SizeX * currentXRatio, bb.Y + bb.SizeY * currentYRatio, bb.Z + currentZRatio * PUPPIGUISettings.nodeHeight * bb.SizeX / nsix);

                List<Point3D> tempoints  = moveMyPoints(originalFocus, endFocus);
                if (tempmod != null)
                {
                    ModelVisual3D tmod = PUPPICAD.HelperClasses.utilities.set3DObjectPoints(tempmod, tempoints);
                    displayMe(tmod, endFocus, true);
                }
            }
        }
        //public void updateMe()
        //{
        //    if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
        //    {
        //        displayMe(true,cu) 
                
        //    }
        //}
        public void displayMe(ModelVisual3D myModel,Point3D markerpos,bool showMarker=false  )
        {
            Rect3D bbox = new Rect3D();
          
            if ( inputs[1].module != null)
            {

                //one object

                ModelVisual3D mv = PUPPICAD.HelperClasses.utilities.fullCloneModelVisual3D( myModel);
               
                bbox=canvasRenderer.CanvasRenderHelper.getModelBoundingBox(mv);    



                if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
                {
                    PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
                    //remove all models
                    pN.modelVisuals3D.Clear();
                    //marker sphere
                    if (showMarker==true )
                    {
                       // Rect3D mbb = canvasRenderer.CanvasRenderHelper.getModelBoundingBox(mv);
                        //make the sphere
                        double rd = moveRadius;
                      //  Point3D pos=new Point3D(mbb.X+mbb.SizeX*xRatio,mbb.Y+mbb.SizeY*yRatio ,mbb.Z+mbb.SizeZ*zRatio );
                        mv.Children.Add(PUPPICAD.HelperClasses.utilities.makeSphere(markerpos, rd, System.Windows.Media.Color.FromArgb(100,255,0,0    )));

                    }
                    //for planar surfaces
                    double hraise = 0;
                    if (bbox.SizeZ < 0.1) hraise = PUPPIGUISettings.nodeHeight;  
                    pN.add3DModelFitRectBounds(mv, sizeMe, sizeMe, sizeMe,hraise  );
                    forceMyNodeToUpdate();
                }
            }
        }


    }
}


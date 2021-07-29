using PUPPIGUI;
using PUPPIModel;
using HelixToolkit.Wpf;
using System.Reflection;
using PUPPICAD;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
//modules and renderers to display custom graphics directly on the canvas
namespace canvasRenderer
{
    //function to determine bounding box of a model so it can be scaled and centered
    public static class CanvasRenderHelper
    {
        public static Rect3D getModelBoundingBox(ModelVisual3D m)
        {
            Rect3D bb = new Rect3D();
            if (m.Content != null) bb.Union(m.Content.Bounds);
            for (int i = 0; i < m.Children.Count; i++)
            {
                bb.Union(getModelBoundingBox(m.Children[i] as ModelVisual3D));
            }
            return bb;
        }
        //makes a custom renderer to display an image under the objects
        public static PUPPICustomRenderer makeTestCanvasStaticVisual()
        {
            PUPPICustomRenderer cr = new PUPPICustomRenderer();
            cr.addCaption3D("PUPPICAD", 0, 0, -1, 10, 240, 240, 240, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);
            cr.addSphere3D(0, 0, -0.5, 0.1, 240, 240, 255);
            cr.addBox3D(0, -0.05, -0.55, 0.5, 0.05, -0.45, 255, 200, 200);
            cr.addBox3D(-0.05, 0, -0.55, 0.05, 0.5, -0.45, 200, 255, 200);
            cr.addBox3D(-0.05, -0.05, -0.5, 0.05, 0.05, 0, 200, 200, 255);
            return cr;
        }

        //displays what is in the input so it can be seen bigger
        public static PUPPINodeCustomRenderer makeTextDisplay3DRenderer()
        {
            PUPPINodeCustomRenderer textRenderer = new PUPPINodeCustomRenderer("TextDisplayRenderer");
            textRenderer.useDefaultCaption = false;
            textRenderer.addCaption3D("empty", 0, 0, PUPPIGUISettings.nodeHeight * 1 / 4 + PUPPIGUISettings.textRaise / 2, PUPPIGUISettings.nodeSide * 1 / 8, 0, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);
            return textRenderer;
        }
        //renderer for the text with set font height
        public static PUPPINodeCustomRenderer makeTextDisplayFS3DRenderer()
        {
            PUPPINodeCustomRenderer textRenderer = new PUPPINodeCustomRenderer("TextDisplayFSRenderer");
            textRenderer.useDefaultCaption = false;
            textRenderer.addCaption3D("empty", 0, 0, PUPPIGUISettings.nodeHeight * 1 / 4 + PUPPIGUISettings.textRaise / 2, 0.5, 0, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);
            return textRenderer;
        }
        //renderer for the table
        public static PUPPINodeCustomRenderer makeTable3DRenderer()
        {
            PUPPINodeCustomRenderer textRenderer = new PUPPINodeCustomRenderer("VerticalTableRenderer");
            textRenderer.useDefaultCaption = false;
            textRenderer.addCaption3D("empty", 0, 0, PUPPIGUISettings.nodeHeight * 1 / 4 + PUPPIGUISettings.textRaise / 2, 0.5, 0, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);
            return textRenderer;
        }
        //to display CAD objects on nodes
        public static PUPPINodeCustomRenderer makeModelRenderer()
        {
            PUPPINodeCustomRenderer modelRenderer = new PUPPINodeCustomRenderer("ModelRenderer");
            modelRenderer.useDefaultCaption = false;
            return modelRenderer;
        }
        public static PUPPINodeCustomRenderer makeGauge3DRenderer()
        {
            PUPPINodeCustomRenderer modelRenderer = new PUPPINodeCustomRenderer("GaugeRenderer");
            modelRenderer.useDefaultCaption = false;
            //circle
            modelRenderer.addPipe3D(0, -PUPPIGUISettings.nodeSide / 16, PUPPIGUISettings.nodeSide / 2, 0, PUPPIGUISettings.nodeSide / 16, PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeSide, 10, 10, 10);
           
            return modelRenderer;
        }
    }
    //displays one or more 3D objects coming as inputs stacked on canvas
    public class PUPPIObjectDisplay : PUPPIModule
    {

        public PUPPIObjectDisplay()
            : base()
        {
            name = "Display Model(s)";
            description = "Displays 3D object(s) On Canvas fitting withing standard node footprint";
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("ToDisplay");
            completeProcessOverride = true;
            maxChildren = 0;
        }
        public override void process_usercode()
        {
            Rect3D bbox = new Rect3D();
            string newcap = "";
            int count = 0;
            List<Rect3D> bbxes = new List<Rect3D>();
            List<ModelVisual3D> addModels = new List<ModelVisual3D>();
            if (inputs[0].module != null)
            {

                //one object

                ModelVisual3D mv = null;
                if (inputs[0].module.outputs[inputs[0].outParIndex] is ModelVisual3D)
                {
                    mv = inputs[0].module.outputs[inputs[0].outParIndex] as ModelVisual3D;

                    count = 1;
                }
                else if (inputs[0].module.outputs[inputs[0].outParIndex] is Model3D)
                {

                    mv = new System.Windows.Media.Media3D.ModelVisual3D();
                    Model3D m = inputs[0].module.outputs[inputs[0].outParIndex] as Model3D;
                    mv.Content = m;
                    count = 1;


                }


                 //   or multiple
                else
                {
                    mv = new ModelVisual3D();
                    ArrayList linput = inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList;

                    if (linput != null)
                    {
                        count = linput.Count;
                        double curheight = 0;
                        //adds all the elements to list
                        for (int i = 0; i < linput.Count; i++)
                        {
                            if (linput[i] is ModelVisual3D)
                            {
                                ModelVisual3D mvi = linput[i] as ModelVisual3D;
                                addModels.Add(mvi);

                            }
                            else if (linput[i] is Model3D)
                            {
                                System.Windows.Media.Media3D.ModelVisual3D mvi = new System.Windows.Media.Media3D.ModelVisual3D();
                                Model3D m = linput[i] as Model3D;
                                mvi.Content = m;
                                addModels.Add(mvi);


                            }
                        }

                    }
                }

                if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
                {
                    PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
                    pN.recalcNormsOnRender = true;
                    //remove all models
                    pN.modelVisuals3D.Clear();
                    pN.wireBoxes3D.Clear();
                    if (count == 1)
                    {
                        //single model, also make bounding box
                        pN.addModelVisual3D(mv, PUPPIGUISettings.ModelFitInNodeMode.Fit);
                        Rect3D makeBoxRect = CanvasRenderHelper.getModelBoundingBox(pN.modelVisuals3D[0]);

                        pN.addWireBox3D(makeBoxRect.X - makeBoxRect.SizeX / 2, makeBoxRect.Y - makeBoxRect.SizeY / 2, makeBoxRect.Z - makeBoxRect.SizeZ / 2, makeBoxRect.SizeX, makeBoxRect.SizeY, makeBoxRect.SizeZ, 255, 0, 0);
                    }
                    else if (count > 1)
                    {
                        //this function automatically stackas models with bounding boxes within node footprint
                        pN.addStacked3DModels(addModels);

                    }

                    forceMyNodeToUpdate();
                }
            }
        }
    }

    //displays one or more 3D objects coming as inputs stacked on canvas
    public class PUPPIObjectSizeDisplay : PUPPIModule
    {

        public PUPPIObjectSizeDisplay()
            : base()
        {
            name = "Disp. Model Size";
            description = "Displays a 3D model on canvas to fit within specified square footprint dimension.Takes ModelVisual3D or Model3D objects";
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("ToDisplay");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Side Length");
            completeProcessOverride = true;
            maxChildren = 0;
        }
        public override void process_usercode()
        {
            Rect3D bbox = new Rect3D();
            string newcap = "";
            int count = 0;
            List<Rect3D> bbxes = new List<Rect3D>();
            List<ModelVisual3D> addModels = new List<ModelVisual3D>();
            if (inputs[0].module != null && inputs[1].module != null)
            {

                //one object

                ModelVisual3D mv = null;
                if (inputs[0].module.outputs[inputs[0].outParIndex] is ModelVisual3D)
                {
                    mv = inputs[0].module.outputs[inputs[0].outParIndex] as ModelVisual3D;

                    count = 1;
                }
                else if (inputs[0].module.outputs[inputs[0].outParIndex] is Model3D)
                {

                    mv = new System.Windows.Media.Media3D.ModelVisual3D();
                    Model3D m = inputs[0].module.outputs[inputs[0].outParIndex] as Model3D;
                    mv.Content = m;
                    count = 1;


                }

                double sizeMe = Convert.ToDouble(inputs[1].module.outputs[inputs[1].outParIndex]);
                if (sizeMe > 0) count = 1;




                if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional && count == 1)
                {
                    PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
                    pN.recalcNormsOnRender = true;
                    //remove all models
                    pN.modelVisuals3D.Clear();
                    pN.wireBoxes3D.Clear();
                    //single model, also make bounding box
                    pN.add3DModelFitBox(mv, sizeMe, sizeMe, Double.MaxValue);
                    forceMyNodeToUpdate();
                }
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


    //displays image from file
    public class PUPPIImageFileDisplay : PUPPIModule
    {
        string tmpfile = "";
        public PUPPIImageFileDisplay()
            : base()
        {
            name = "Image File";
            description = "Displays image from file path in input on screen to fit in node footprint";
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("FilePath");
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
                if (System.IO.File.Exists(newcap) == false) return;
            }
            else
            {
                newcap = "null";
                return;
            }
            if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
            {

                ////workaround for now for image cache
                //if (File.Exists(tmpfile))
                //{
                //    File.Delete(tmpfile);
                //}
                //tmpfile = System.IO.Path.GetTempPath() + @"\" + DateTime.Now.Second.ToString() + "_" + getGUID().ToString() + Path.GetFileNameWithoutExtension(newcap);


                //File.Copy(newcap, tmpfile);
                PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
                //make sure no caption
                pN.images3D.Clear();
               // pN.addImage3D(tmpfile, 0, 0, PUPPIGUISettings.connRaise, PUPPIGUISettings.nodeSide, PUPPIGUISettings.nodeSide, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);
                pN.addImage3D(newcap, 0, 0, PUPPIGUISettings.connRaise, PUPPIGUISettings.nodeSide, PUPPIGUISettings.nodeSide, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);

                forceMyNodeToUpdate();
            }
        }
    }

    //displays a text sent by input
    public class PUPPITextDisplayFS : PUPPIModule
    {
        public PUPPITextDisplayFS()
            : base()
        {
            name = "Text Fixed Font Size";
            description = "Displays input string on screen to at font height 0.5";
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


                pN.updateCaption3D(0, newcap, 0, 0, PUPPIGUISettings.nodeHeight * 1 / 4 + PUPPIGUISettings.textRaise / 2, 0.5, 0, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);


                forceMyNodeToUpdate();
            }
        }
    }

    //displays strings in table vertically
    public class PUPPIVertTableDisplay : PUPPIModule
    {
        public PUPPIVertTableDisplay()
            : base()
        {
            name = "Vertical Table";
            description = "Displays PUPPI Grid or 2D array or list or single value as a table";
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Grid/2D Array");
            PUPPIInParameter pinp = new PUPPIInParameter();
            pinp.isoptional = true;
            inputs.Add(pinp);
            inputnames.Add("Cell Hr Sz");
            pinp = new PUPPIInParameter();
            pinp.isoptional = true;
            inputs.Add(pinp);
            inputnames.Add("Cell Vrt Sz");
            pinp = new PUPPIInParameter();
            pinp.isoptional = true;
            inputs.Add(pinp);
            inputnames.Add("Heading");
            completeProcessOverride = true;
            maxChildren = 0;
        }
        public override void process_usercode()
        {
            ArrayList rows = new ArrayList();
            List<string> myrow = new List<string>();
            double cl = 0.5;
            if (inputs[1].module != null)
            {
                try
                {
                    cl = Convert.ToDouble(inputs[1].module.outputs[inputs[1].outParIndex]);
                }
                catch
                {
                    cl = 0.5;
                }
            }
            if (cl <= 0) cl = 0.5;
            double ch = 0.1;
            if (inputs[2].module != null)
            {
                try
                {
                    ch = Convert.ToDouble(inputs[2].module.outputs[inputs[2].outParIndex]);
                }
                catch
                {
                    ch = 0.1;
                }
            }
            if (ch <= 0) cl = 0.1;
            //top heading or title
            string ah = "";
            if (inputs[3].module != null)
            {
                ah = inputs[3].module.outputs[inputs[3].outParIndex].ToString();
            }
            try
            {

                string newcap = "";
                if (inputs[0].module != null)
                {
                    object os = inputs[0].module.outputs[inputs[0].outParIndex];
                    if (os.GetType().IsArray)
                    {
                        Array aos = os as Array;
                        rows = new ArrayList();
                        if (aos.Rank == 1)
                        {
                            int lb = aos.GetLowerBound(0);
                            int ub = aos.GetUpperBound(0);
                            myrow = new List<string>();
                            for (int ic = lb; ic <= ub; ic++)
                            {
                                object myv = aos.GetValue(ic);
                                myrow.Add(myv.ToString());
                            }
                            rows.Add(myrow);
                        }
                        else if (aos.Rank == 2)
                        {
                            int lb1 = aos.GetLowerBound(0);
                            int ub1 = aos.GetUpperBound(0);
                            int lb2 = aos.GetLowerBound(1);
                            int ub2 = aos.GetUpperBound(1);


                            for (int ic = lb1; ic <= ub1; ic++)
                            {
                                myrow = new List<string>();
                                for (int jc = lb2; jc <= ub2; jc++)
                                {
                                    object myv = aos.GetValue(ic, jc);
                                    myrow.Add(myv.ToString());
                                }
                                rows.Add(myrow);
                            }
                        }
                        else
                        {
                            rows = new ArrayList();
                            myrow = new List<string>();
                            myrow.Add("Invalid array rank.");
                            rows.Add(myrow);
                        }
                    }
                    else
                    {
                        if (os is IEnumerable)
                        {
                            ArrayList ay = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(os);
                            for (int ayi = 0; ayi < ay.Count; ayi++)
                            {
                                List<string> thisrow = new List<string>();
                                object osy = ay[ayi];
                                if (osy is IEnumerable && osy.GetType() != typeof(string))
                                {
                                    ArrayList ray = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(osy);
                                    for (int rayi = 0; rayi < ray.Count; rayi++)
                                    {
                                        thisrow.Add(ray[rayi].ToString());
                                    }
                                }
                                else
                                {
                                    thisrow.Add(osy.ToString());
                                }

                                rows.Add(thisrow);
                            }

                        }
                        else
                        {
                            List<string> thisrow = new List<string>();
                            thisrow.Add(os.ToString());
                            rows.Add(thisrow);

                        }
                    }

                }
                else
                {
                    rows = new ArrayList();
                    myrow = new List<string>();
                    myrow.Add("null input");
                    rows.Add(myrow);

                }
            }
            catch
            {
                rows = new ArrayList();
                myrow = new List<string>();
                myrow.Add("error");
                rows.Add(myrow);

            }
            //figure out maximum string length
            //int maxlength = 0;
            //for (int i = 0; i < rows.Count; i++)
            //{
            //    List<string> thisrow = rows[i] as List<string>;
            //    for (int j=0;j<thisrow.Count;j++  )
            //    {
            //        string thiscellitem = thisrow[j];
            //        if (thiscellitem.Length > maxlength) maxlength = thiscellitem.Length ;  
            //    }
            //}



            //string sample = "";
            //for (int sc = 0; sc < maxlength;sc++ )
            //{
            //    sample = sample + " ";
            //}
            ////figure out cell dimensions
            //TextVisual3D t = new TextVisual3D();
            //t.Position = new Point3D(0, 0, 0);  
            //t.Text = sample;
            //t.TextDirection = new Vector3D(1, 0, 0);
            //t.UpDirection = new Vector3D(0, 0, 1);
            //t.FontSize = fs;
            //double ch = t.Content.Bounds.SizeZ;
            //double cl= t.Content.Bounds.SizeX;
            int rowcells = 0;
            if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
            {
                PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
                pN.clearAll();
                for (int i = 0; i < rows.Count; i++)
                {
                    List<string> thisrow = rows[i] as List<string>;
                    rowcells = thisrow.Count;
                    double putz = ch * (rows.Count - i - 1) + ch / 2;
                    for (int j = 0; j < thisrow.Count; j++)
                    {
                        double putx = -thisrow.Count * 0.5 * cl + j * cl + 0.5 * cl;
                        // string padded=thisrow[j].PadRight(maxlength)  ; 
                        //try
                        //{
                        //    padded = padded.PadLeft(padded.Length+Convert.ToInt16 ((maxlength - padded.Length )*0.5),' ');
                        //}
                        //catch{
                        //    padded=thisrow[j];
                        //}
                        //string np = padded;
                        // try
                        //{
                        //    padded = padded.PadRight(padded.Length + Convert.ToInt16((maxlength - thisrow[j].Length) * 0.5), ' ');
                        //}
                        //catch{
                        //    padded = np;
                        //}
                        // padded = padded.PadLeft(maxlength);  
                        pN.addTextCell3D(thisrow[j], putx, 0, putz, 0, 0, 0, cl, ch);
                    }
                }

                if (ah != "")
                {
                    double putz = ch * (rows.Count) + ch / 2;
                    pN.addTextCell3D(ah, 0, 0, putz, 0, 0, 0, rowcells * cl, ch, 240, 240, 255);
                }

                forceMyNodeToUpdate();
            }
        }
    }


    //displays a text sent by input
    public class PUPPITextCustomDisplay : PUPPIModule
    {
        public PUPPITextCustomDisplay()
            : base()
        {
            name = "Custom Text Display";
            description = "Displays input string on screen. Use +X , -X, +Y , -Y for Direction and H or V for Orientation";
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("ToDisplay");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Direction");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Orientation");
            //optional inputs
            PUPPIInParameter textColorInput = new PUPPIInParameter();
            textColorInput.isoptional = true;
            inputs.Add(textColorInput);
            inputnames.Add("Text Color");
            PUPPIInParameter backColorInput = new PUPPIInParameter();
            backColorInput.isoptional = true;
            inputs.Add(backColorInput);
            inputnames.Add("Bck. Color");
            //handle all processing in process_usercode
            completeProcessOverride = true;
            //don't allow any nodes on top
            maxChildren = 0;
        }
        public override void process_usercode()
        {
            //validate text and orientations
            string newcap = "";
            if (inputs[0].module != null)
            {
                try
                {
                    object os = inputs[0].module.outputs[inputs[0].outParIndex];
                    newcap = os.ToString();
                }
                catch
                {
                    newcap = "";
                }
            }
            else
            {
                newcap = "";
            }
            string dir = "";
            if (inputs[1].module != null)
            {
                try
                {
                    object os = inputs[1].module.outputs[inputs[1].outParIndex];
                    dir = os.ToString();
                }
                catch
                {
                    dir = "";
                }
            }
            else
            {
                dir = "";
            }
            string ori = "";
            if (inputs[2].module != null)
            {
                try
                {
                    object os = inputs[2].module.outputs[inputs[2].outParIndex];
                    ori = os.ToString();
                }
                catch
                {
                    ori = "";
                }
            }
            else
            {
                ori = "";
            }
            if (newcap == "" || dir == "" || ori == "") return;
            dir = dir.Replace(" ", "");
            ori = ori.Replace(" ", "");
            //colors
            Color textColor = Color.FromRgb(0, 0, 0);
            if (inputs[3].module != null)
            {
                try
                {
                    object os = inputs[3].module.outputs[inputs[3].outParIndex];
                    textColor = (Color)os;
                }
                catch
                {
                    textColor = Color.FromRgb(0, 0, 0);
                }
            }
            else
            {
                textColor = Color.FromRgb(0, 0, 0);
            }

            Color backColor = Color.FromRgb(255, 255, 255);
            if (inputs[4].module != null)
            {
                try
                {
                    object os = inputs[4].module.outputs[inputs[4].outParIndex];
                    backColor = (Color)os;
                }
                catch
                {
                    backColor = Color.FromRgb(255, 255, 255);
                }
            }
            else
            {
                backColor = Color.FromRgb(255, 255, 255);
            }

            if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
            {
                PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
                //make sure no caption
                pN.useDefaultCaption = false;
                //clear everything and regen
                pN.clearAll();
                //text orientation settings - convert to right type
                PUPPICustomRenderer.PUPPIimageDirection textDir = PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection;

                if (dir == "-X")
                {
                    textDir = PUPPICustomRenderer.PUPPIimageDirection.negativeXDirection;
                }
                if (dir == "+Y")
                {
                    textDir = PUPPICustomRenderer.PUPPIimageDirection.positiveYDirection;
                }
                if (dir == "-Y")
                {
                    textDir = PUPPICustomRenderer.PUPPIimageDirection.negativeYDirection;
                }
                PUPPICustomRenderer.PUPPIimageOrientation textOrient = PUPPICustomRenderer.PUPPIimageOrientation.horizontal;
                if (ori == "V")
                {
                    textOrient = PUPPICustomRenderer.PUPPIimageOrientation.vertical;
                }
                pN.addTextCell3D(newcap, 0, 0, 0, textColor.R, textColor.G, textColor.B, PUPPIGUISettings.nodeSide, PUPPIGUISettings.nodeHeight, backColor.R, backColor.G, backColor.B, textDir, textOrient);

                forceMyNodeToUpdate();
            }
        }
    }

    //displays a text entered by user
    public class PUPPINoteDisplay : PUPPIModule
    {
        //the text gets saved
        string noteText = "Double click to change";
        double textSize = 1.0;
        public PUPPINoteDisplay()
            : base()
        {
            name = "Note on Canvas";
            description = "Displays entered text on screen. Double click to change text";
            doubleClickDescription = "Set text and size";
            completeProcessOverride = true;
            completeDoubleClickOverride = true;
            //nothing can be on top
            maxChildren = 0;
            setNodeSolidBoundary(false);


        }
        public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
        {
            string newcap = "";
            string ts = "";
            double textSize = 0;
            if (PUPPIFormUtils.formutils.InputBox("Please enter a string", "Enter text:", ref newcap) == System.Windows.Forms.DialogResult.Cancel)
            {
                newcap = "";
            }
            if (PUPPIFormUtils.formutils.InputBox("Please enter text size", "Enter number:", ref ts) == System.Windows.Forms.DialogResult.Cancel)
            {
                ts = "";
            }
            try
            {
                textSize = Convert.ToDouble(ts);
            }
            catch
            {
                textSize = 0;
            }

            if (newcap != "" && textSize > 0) setDisplayText(newcap, textSize);
            

        }



        public void setDisplayText(string text, double tSize)
        {
            setNodeSolidBoundary(false);

            noteText = text;
            textSize = tSize;
            if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
            {
                PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
                //need to check for null renderer since we are calling this from constructor
                if (pN != null)
                {


                    //fresh renderer every time
                    pN.clearAll();
                    pN.addCaption3D(text, 0, 0, PUPPIGUISettings.nodeHeight * 1 / 4 + PUPPIGUISettings.textRaise / 2, textSize, 0, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.horizontal);
                    forceMyNodeToUpdate();
                }
            }
        }
        //load and display text when loading file
        public override void initOnLoad(string savedSettings)
        {
            //load text and text size
            char[] splitter = { '_' };
            noteText = savedSettings.Split(splitter)[0];
            textSize = Convert.ToDouble(savedSettings.Split(splitter)[1]);
        }
        //save text and size when saving canvas
        public override string saveSettings()
        {
            return noteText + "_" + textSize.ToString();
        }
        //upon loading the processing function will ensure the visual representation gets updated
        public override void process_usercode()
        {
            setDisplayText(noteText, textSize);
        }

    }

    //takes a list of numbers and bar width as input and displays a vertical bar for each entry
    public class BarGraph3D : PUPPIModule
    {

        public BarGraph3D()
            : base()
        {
            //vertical range inputs
            name = "Bar Graph";
            description = "Displays a 3D Bar chart of a list of numbers.Scales to node footprint dimensions, use Bar Width for controlling apsect ratio.";
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Values");
            PUPPIInParameter bw = new PUPPIInParameter();
            bw.isoptional = true;
            inputs.Add(bw);
            inputnames.Add("Bar Width");
            //can't put anything on top of it
            maxChildren = 0;
            completeProcessOverride = true;

        }
        //redraw the chart
        public override void process_usercode()
        {

            PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
            //when loading from file, the renderer is not yet initialized
            if (pN != null)
            {

                object os = inputs[0].module.outputs[inputs[0].outParIndex];
                ArrayList barNumbers = new ArrayList();
                if (os == null) return;
                if (os is ICollection)
                    barNumbers = new ArrayList(os as ICollection);
                else if (os is IEnumerable)
                    barNumbers = PUPPIModule.makeMeAnArrayList(os as IEnumerable);
                else return;

                object inputBoxWidth = null;
                //can also be done by checking if module is null
                try
                {
                    inputBoxWidth = inputs[1].module.outputs[inputs[1].outParIndex];
                }
                catch
                {
                    inputBoxWidth = null;
                }
                //will clear off all components and redraw
                pN.clearAll();
                if (barNumbers.Count > 0)
                {
                    //use this for scaling
                    double defaultboxWidth = 4 * (PUPPIGUISettings.nodeSide - 2 * PUPPIGUISettings.ioLength) / barNumbers.Count;
                    double boxWidth = defaultboxWidth;
                    if (inputBoxWidth != null)
                        boxWidth = Convert.ToDouble(inputBoxWidth);
                    if (boxWidth == 0) boxWidth = defaultboxWidth;
                    double xPos = -defaultboxWidth * barNumbers.Count / 2 + PUPPIGUISettings.ioLength;
                    double yMin = Double.MaxValue;
                    double yMax = double.MinValue;
                    //finding minimum and maximum for color scale and legend
                    for (int i = 0; i < barNumbers.Count; i++)
                    {
                        double num = Convert.ToDouble(barNumbers[i]);
                        if (num > yMax) yMax = num;
                        if (num < yMin) yMin = num;

                    }
                    for (int i = 0; i < barNumbers.Count; i++)
                    {
                        double num = Convert.ToDouble(barNumbers[i]);
                        //box dimensions and positions

                        double xStart = xPos + defaultboxWidth * i;
                        double boxHeight = num * defaultboxWidth / boxWidth;
                        pN.addBox3D(xStart, -defaultboxWidth / 2, 0, xStart + defaultboxWidth, defaultboxWidth / 2, boxHeight, Convert.ToByte(Math.Min(Convert.ToInt16(Math.Abs(255 * (num - yMin) / (yMax - yMin))), Convert.ToInt16(255))), 0, Convert.ToByte(Math.Min(Convert.ToInt16(Math.Abs((255 * (yMax - num) / (yMax - yMin)))), Convert.ToInt16(255))));
                    }
                    if (yMax > 0)
                        //maximum value text in the middle, up
                        pN.addCaption3D(yMax.ToString(), 0, 0, yMax * defaultboxWidth / boxWidth + 0.4, 0.2, 0, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.vertical);
                    if (yMin < 0)
                        //minimum value text in the middle, down
                        pN.addCaption3D(yMin.ToString(), 0, 0, yMin * defaultboxWidth / boxWidth - 0.4, 0.2, 0, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.vertical);

                }
                forceMyNodeToUpdate();
            }

        }
    }
    //takes a list of x numbers and a list of y numbers and an optional x/y scale factor and plots lines between points in 3D
    public class LinePlot3D : PUPPIModule
    {

        public LinePlot3D()
            : base()
        {


            inputs.Add(new PUPPIInParameter());
            inputnames.Add("X val coll");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Y val coll");


            //plot will fit in node footprint but the height can be adjusted by setting the y/x scale. default value 1
            PUPPIInParameter scale = new PUPPIInParameter();
            scale.isoptional = true;
            inputs.Add(scale);
            inputnames.Add("Y/X scale");
            name = "Line Plot";
            description = "Displays a Vertical Plane line plot of supplied X and Y values";
            //can't put anything on top of it because it would intersect
            maxChildren = 0;
            completeProcessOverride = true;

        }
        //redraw the chart
        public override void process_usercode()
        {

            //get x values
            object osx = inputs[0].module.outputs[inputs[0].outParIndex];
            ArrayList xNumbers = new ArrayList();
            if (osx == null) return;
            if (osx is ICollection)
                xNumbers = new ArrayList(osx as ICollection);
            else if (osx is IEnumerable)
                xNumbers = PUPPIModule.makeMeAnArrayList(osx as IEnumerable);
            else return;
            //get y values
            object osy = inputs[1].module.outputs[inputs[1].outParIndex];
            ArrayList yNumbers = new ArrayList();
            if (osy == null) return;
            if (osy is ICollection)
                yNumbers = new ArrayList(osy as ICollection);
            else if (osx is IEnumerable)
                yNumbers = PUPPIModule.makeMeAnArrayList(osy as IEnumerable);
            else return;

            if (xNumbers.Count < 1 || yNumbers.Count < 1 || xNumbers.Count != yNumbers.Count) return;

            //scale (optional)
            double yxscale = 1.0;
            object osc = null;
            if (inputs[2].module != null)
            {
                osc = inputs[2].module.outputs[inputs[2].outParIndex];
            }
            if (osc != null)
                try
                {
                    yxscale = Convert.ToDouble(osc);
                }
                catch
                {
                    yxscale = 1.0;
                }


            //get maximum and minimum x values to set up overall scaling to fit inside node. also do conversion to doubles same loop
            double maxX = double.MinValue;
            double minX = double.MaxValue;
            double maxY = double.MinValue;
            double minY = double.MaxValue;
            List<double> xVals = new List<double>();
            List<double> yVals = new List<double>();
            for (int i = 0; i < xNumbers.Count; i++)
            {
                double xi = Convert.ToDouble(xNumbers[i]);
                xVals.Add(xi);
                double yi = Convert.ToDouble(yNumbers[i]);
                yVals.Add(yi);
                if (xi > maxX) maxX = xi;
                if (xi < minX) minX = xi;
                if (yi > maxY) maxY = yi;
                if (yi < minY) minY = yi;
            }
            double xDelta = maxX - minX;
            if (xDelta == 0) return;
            double overallScaleFactor = (PUPPIGUISettings.nodeSide - 2 * PUPPIGUISettings.ioLength) / xDelta;

            PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
            //when loading from file, the renderer is not yet initialized
            if (pN != null)
            {

                //remove everything from renderer. will regenerate.
                pN.clearAll();


                double textSize = PUPPIGUISettings.nodeSide / xVals.Count * overallScaleFactor / 2;

                //first caption - smallest values
                string lowerLeftCaption = "(" + Math.Truncate(100 * minX) / 100 + "," + Math.Truncate(100 * minY) / 100 + ")";

                double xPosP = -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.ioLength + textSize / 10 * lowerLeftCaption.Length;
                double yPosP = textSize / 2;
                pN.addCaption3D(lowerLeftCaption, xPosP, -PUPPIGUISettings.nodeSide * 0.01, yPosP, textSize, 255, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.vertical);

                //first caption - largest values
                string upperRightCaption = "(" + Math.Truncate(100 * maxX) / 100 + "," + Math.Truncate(100 * maxY) / 100 + ")";

                xPosP = PUPPIGUISettings.nodeSide / 2 - PUPPIGUISettings.ioLength - textSize / 10 * upperRightCaption.Length;
                yPosP = (maxY - minY) * overallScaleFactor * yxscale;
                pN.addCaption3D(upperRightCaption, xPosP, -PUPPIGUISettings.nodeSide * 0.01, yPosP, textSize, 255, 0, 0, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.vertical);



                for (int i = 1; i < xVals.Count; i++)
                {

                    //previous and current x and y used to draw line

                    xPosP = -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.ioLength + (xVals[i - 1] - minX) * overallScaleFactor;
                    yPosP = (yVals[i - 1] - minY) * overallScaleFactor * yxscale;

                    double xPosC = -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.ioLength + (xVals[i] - minX) * overallScaleFactor;
                    double yPosC = (yVals[i] - minY) * overallScaleFactor * yxscale;
                    pN.addPipe3D(xPosP, 0, yPosP, xPosC, 0, yPosC, PUPPIGUISettings.nodeSide * 0.01, 0, 0, 0);
                }
                //white background
                pN.addBox3D(-PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.ioLength / 2, -PUPPIGUISettings.nodeSide * 0.0005, 0, PUPPIGUISettings.nodeSide / 2 - PUPPIGUISettings.ioLength / 2, PUPPIGUISettings.nodeSide * 0.0005, (maxY - minY) * overallScaleFactor * yxscale + PUPPIGUISettings.nodeHeight, 255, 255, 255);
                //plot axes if intersecting the lines
                if (minX < 0 && maxX > 0)
                {
                    //Y axis
                    double xPosYaxis = -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.ioLength + (0 - minX) * overallScaleFactor;
                    double yPosYaxisBottom = (0) * overallScaleFactor * yxscale;
                    double yPosYaxisTop = (maxY - minY) * overallScaleFactor * yxscale;
                    pN.addPipe3D(xPosYaxis, 0, yPosYaxisBottom, xPosYaxis, 0, yPosYaxisTop, PUPPIGUISettings.nodeSide * 0.02, 255, 0, 0);
                }
                if (minY < 0 && maxY > 0)
                {
                    //X axis
                    double yPosXaxis = (0 - minY) * overallScaleFactor * yxscale;
                    double xPosXaxisLeft = -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.ioLength + (0) * overallScaleFactor;
                    double xPosXaxisRight = -PUPPIGUISettings.nodeSide / 2 + PUPPIGUISettings.ioLength + (maxX - minX) * overallScaleFactor;

                    pN.addPipe3D(xPosXaxisLeft, 0, yPosXaxis, xPosXaxisRight, 0, yPosXaxis, PUPPIGUISettings.nodeSide * 0.02, 0, 0, 255);
                }






            }

            forceMyNodeToUpdate();
        }


    }


    //displays a gauge with custom caption and limits
    public class PUPPIGauge3D : PUPPIModule
    {
        double lowerLimit = 0;
        double upperLimit = 1;
        string caption = "Gauge";
        public PUPPIGauge3D()
            : base()
        {
            name = "Gauge";
            description = "Number rendered on needle gauge";
            doubleClickDescription = "Set caption and limits"; 
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Number");
            completeDoubleClickOverride = true;
            completeProcessOverride = true;
        }
        public override string saveSettings()
        {
            return lowerLimit.ToString() + "__" + upperLimit.ToString() + "__" + caption;
        }
        //load them from file qand update node 
        public override void initOnLoad(string savedSettings)
        {
            string[] sep = { "__" };
            string[] vals = savedSettings.Split(sep, StringSplitOptions.None);


            lowerLimit = Convert.ToInt32(vals[0]);
            upperLimit = Convert.ToInt32(vals[1]);
            caption = vals[2];
        }
        //set the value and the Slider position numerically
        public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
        {

            string newcap = caption;
            DialogResult dC = PUPPIFormUtils.formutils.InputBox("Please enter caption", "Set caption", ref newcap);

            if (dC != DialogResult.Cancel)
                caption = newcap;


            string LLinputBoxResult = "";
            DialogResult dL = PUPPIFormUtils.formutils.InputBox("Please enter lower limit", "Set Gauge Lower Limit", ref LLinputBoxResult);

            string ULinputBoxResult = "";
            DialogResult dU = PUPPIFormUtils.formutils.InputBox("Please enter upper limit", "Set Gauge Upper Limit", ref ULinputBoxResult);




            if (dL != DialogResult.Cancel && dU != DialogResult.Cancel)
            {

                //validation
                try
                {
                    lowerLimit = Convert.ToInt32(LLinputBoxResult);
                    upperLimit = Convert.ToInt32(ULinputBoxResult);
                    if (upperLimit == lowerLimit) upperLimit = lowerLimit + 1;
                    if (upperLimit < lowerLimit)
                    {

                        double swapper = lowerLimit;
                        lowerLimit = swapper;
                        upperLimit = swapper;
                    }

                }
                catch
                {

                }

            }

        }
        public override void process_usercode()
        {
            PUPPINodeCustomRenderer pN = getNodeCustomRenderer();
            if (pN != null)
            {
                try
                {

                    double inp = Convert.ToDouble(inputs[0].module.outputs[inputs[0].outParIndex]);


                    //when loading from file, the renderer is not yet initialized
                    if (PUPPIGUISettings.canvasMode == PUPPIGUISettings.CanvasModeEnum.ThreeDimensional)
                    {
                        pN.triangles3D.Clear();
                        pN.captions3D.Clear();
                        pN.addCaption3D(caption, 0, -PUPPIGUISettings.nodeSide / 16 - PUPPIGUISettings.textRaise, PUPPIGUISettings.nodeSide *3/8, PUPPIGUISettings.nodeSide / 8, 255, 255, 255, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.vertical);
                        pN.addCaption3D(Math.Round(lowerLimit, PUPPIGUISettings.showDigits).ToString(), -PUPPIGUISettings.nodeSide *5/16, -PUPPIGUISettings.nodeSide / 16 - PUPPIGUISettings.textRaise, PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeSide / 8, 255, 255, 255, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.vertical);
                        pN.addCaption3D(Math.Round(upperLimit, PUPPIGUISettings.showDigits).ToString(), PUPPIGUISettings.nodeSide *5/ 16, -PUPPIGUISettings.nodeSide / 16 - PUPPIGUISettings.textRaise, PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeSide / 8, 255, 255, 255, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.vertical);
                        pN.addCaption3D(Math.Round((upperLimit+lowerLimit)/2 , PUPPIGUISettings.showDigits).ToString(),0, -PUPPIGUISettings.nodeSide / 16 - PUPPIGUISettings.textRaise, PUPPIGUISettings.nodeSide *3.5/4, PUPPIGUISettings.nodeSide / 8, 255, 255, 255, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.vertical);
                        pN.addCaption3D(Math.Round(lowerLimit +(upperLimit-lowerLimit )  / 4, PUPPIGUISettings.showDigits).ToString(), -PUPPIGUISettings.nodeSide / 4, -PUPPIGUISettings.nodeSide / 16 - PUPPIGUISettings.textRaise, PUPPIGUISettings.nodeSide*3 / 4, PUPPIGUISettings.nodeSide / 8, 255, 255, 255, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.vertical);
                        pN.addCaption3D(Math.Round(lowerLimit +(upperLimit -lowerLimit) *3/ 4, PUPPIGUISettings.showDigits).ToString(), PUPPIGUISettings.nodeSide / 4, -PUPPIGUISettings.nodeSide / 16 - PUPPIGUISettings.textRaise, PUPPIGUISettings.nodeSide*3 / 4, PUPPIGUISettings.nodeSide / 8, 255, 255, 255, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.vertical);

                        double ratio=0;
                        if (inp>upperLimit )
                        {
                            ratio = 1;
                        }
                        else if (inp>lowerLimit )
                        {
                            ratio = (inp - lowerLimit) / (upperLimit - lowerLimit);
                        }
                        //these coords are in the plane of the gauge
                        //90 degrees needle range
                        double uX = -Math.Cos(Math.PI / 2 * ratio);
                        double uY = Math.Sin(Math.PI / 2 * ratio);
                        double needleRadius = PUPPIGUISettings.nodeSide / 4 * Math.Sqrt(2);
                        //rotate coordinates - 45 degrees
                        double rX = uX * Math.Cos(-Math.PI / 4) - uY * Math.Sin(-Math.PI / 4);
                        double rY = uY * Math.Cos(-Math.PI / 4) + uX * Math.Sin(-Math.PI / 4);
                        //needle origin
                        double oX = 0;
                        double oY = PUPPIGUISettings.nodeSide / 4;
                        //transformed coords of needle tip, still in plane of the gauge
                        double ntX = oX + rX * needleRadius;
                        double ntY = oY + rY * needleRadius;
                        //3d point coordinates
                        //making needle from a filled triangle
                        double p1x = -PUPPIGUISettings.nodeSide / 32;
                        double p1y = -PUPPIGUISettings.nodeSide / 16 - 2 * PUPPIGUISettings.textRaise;
                        double p1z = PUPPIGUISettings.nodeSide / 4;
                        double p2x = PUPPIGUISettings.nodeSide / 32;
                        double p2y = -PUPPIGUISettings.nodeSide / 16 - 2 * PUPPIGUISettings.textRaise;
                        double p2z = PUPPIGUISettings.nodeSide / 4;
                        //the tip
                        double p3x = ntX;
                        double p3y = -PUPPIGUISettings.nodeSide / 16 - 2 * PUPPIGUISettings.textRaise;
                        double p3z = ntY;
                        pN.addTriangle3D(p1x, p1y, p1z, p2x, p2y, p2z, p3x, p3y, p3z, 255, 255, 255); 

                    }
                }
                catch
                {
                    pN.triangles3D.Clear();
                    pN.captions3D.Clear();
                    pN.addCaption3D("error", 0, -PUPPIGUISettings.nodeSide / 16 - PUPPIGUISettings.textRaise, PUPPIGUISettings.nodeSide / 2, PUPPIGUISettings.nodeSide / 8, 255, 255, 255, PUPPICustomRenderer.PUPPIimageDirection.positiveXDirection, PUPPICustomRenderer.PUPPIimageOrientation.vertical);

                }
                forceMyNodeToUpdate();
            }
        }

    }


    //convenience module to output red color to assign to objects
    public class PUPPIRedColor : PUPPIModule
    {
        public PUPPIRedColor()
            : base()
        {
            description = "Convenience module to output red color to assign to objects";
            name = "Color Red";
            outputs.Add(Colors.Red);
            outputnames.Add("Color");
        }
    }

    //convenience module to output green color to assign to objects
    public class PUPPIGreenColor : PUPPIModule
    {
        public PUPPIGreenColor()
            : base()
        {
            description = "Convenience module to output green color to assign to objects";
            name = "Color Green";
            outputs.Add(Colors.Green);
            outputnames.Add("Color");
        }
    }

    //convenience module to output blue color to assign to objects
    public class PUPPIBlueColor : PUPPIModule
    {
        public PUPPIBlueColor()
            : base()
        {
            description = "Convenience module to output blue color to assign to objects";
            name = "Color Blue";
            outputs.Add(Colors.Blue);
            outputnames.Add("Color");
        }
    }

}
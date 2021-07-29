using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using PUPPIGUI;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.ObjectModel;
using PUPPIModel;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Concurrent;
namespace PUPPICAD
{
    /// <summary>
    /// The <see cref="PUPPICAD"/> namespace contains classes for creating and displaying 3D objects in a dedicated 3D model window.
    /// </summary>

    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }

    internal class PUPPI3DView : Window
    {
        internal HelixViewport3D hv;
        Frame frame3d;
        PerspectiveCamera bmycamera;
        //top and bottom lights
        internal ModelVisual3D tlv3d;
        internal ModelVisual3D dlv3d;
        //side lights
        internal ModelVisual3D flv3d;
        internal ModelVisual3D blv3d;
        internal ModelVisual3D llv3d;
        internal ModelVisual3D rlv3d;
        internal int th = 0;
        internal int tw = 0;

        System.Windows.Controls.MenuItem exportscene;
        System.Windows.Controls.MenuItem zoomextents;
        System.Windows.Controls.MenuItem toggleCSA;
        System.Windows.Controls.MenuItem addObj;


        internal CoordinateSystemVisual3D cv;
        internal Grid canvasview;

        internal event EventHandler clicked3DW;
        internal event EventHandler dropped3DW;
        internal SynchronizedCollection<String> commandQueue;
        internal ConcurrentDictionary<string, Model3D> serverObjects;

        //so clients can retrieve their models
        internal static SynchronizedCollection<String> modelGUIDs;
        internal static int modelGUIDCount = 0;


        internal List<string> commandQueueResults;
        //thread safe stuff for server
        internal static string currentXMLRepServer;
        internal static string currentCADStatusServer;
        internal void saveReponThread()
        {


            PUPPI3DView.currentXMLRepServer = saveCADRepresentationToXML();


        }

       

        internal string readMyCADStatus()
        {
            string rep = "";
            if (hv.Children.Contains(cv))
            {
                rep += "Object count: " + (hv.Children.Count - 7).ToString() + "\n";
                rep += "Coordinate arrows visible";
            }
            else
            {
                rep += "Object count: " + (hv.Children.Count - 6).ToString() + "\n";
                rep += "Coordinate arrows invisible";
            }
            return rep;
        }
        //called when server started
        internal void doAnInitialServerUpdate()
        {
            if (PUPPIruntimesettings.PUPPICADTCPServerIsRunning || PUPPIruntimesettings.PUPPICADHTTPServerIsRunning)
            {
                PUPPI3DView.currentCADStatusServer = readMyCADStatus();
                saveReponThread();


            }
        }

        internal string saveCADRepresentationToXML()
        {



            return "";
        }

        internal void runStringCanvasCommands()
        {
            ////multiple commands, block processing then run
            //if (commandQueue.Count > 1)
            //{
            PUPPIruntimesettings.processyesno = false;
            bool needsrerun = false;
            if (commandQueue.Count > 0) needsrerun = true;
            if (needsrerun) commandQueueResults.Clear();

            while (commandQueue.Count > 0)
            {
                string myc = commandQueue[0];
                if (commandQueue.Count > 0)
                    commandQueue.RemoveAt(0);
                string sss = interpretMyTextualCommand(myc);
                commandQueueResults.Add(sss);

            }





        }

        internal CombinedManipulator tramo;
        internal void AddCombinedManipulator(ModelVisual3D mv, bool canRotate, Point3D placementPosition,PUPPIGUISettings.CADWindowManipulatorPosition positioning = PUPPIGUISettings.CADWindowManipulatorPosition.BoxCorner)
        {
            if (hv.Children.Contains(mv as Visual3D) == false) return;
            if (hv.Children.Contains(tramo as Visual3D)) hv.Children.Remove(tramo as Visual3D);
            tramo = new CombinedManipulator();
            tramo.CanRotateX = canRotate;
            tramo.CanRotateY = canRotate;
            tramo.CanRotateZ = canRotate;
            //Rect3D trabo = tramo.getBBRecursive();

            double scaleFactor = PUPPIGUISettings.CADViewManipulatorSize / 1.0;
            
            
            hv.Children.Add(tramo);
            tramo.Bind(mv);
           
            
            Rect3D bbx = mv.getBBRecursive();
            //tramo.Color = Colors.Red;
           
           tramo.Position = bbx.rectcenter();
            if (positioning == PUPPIGUISettings.CADWindowManipulatorPosition.BoxCorner)
            {

                tramo.Offset = new Vector3D(bbx.SizeX * 0.5, bbx.SizeY * 0.5, bbx.SizeZ * 0.5);
            }
            else if (positioning == PUPPIGUISettings.CADWindowManipulatorPosition.Center)
            {
                tramo.Offset = new Vector3D(0, 0, 0);
            }
            else
            {
                //ModelVisual3D mc = mv.cloneVisualAndChildren();
                ModelVisual3D mmc=PUPPICAD.HelperClasses.utilities.hardcodeTransforms(mv);
                Rect3D tbb = mmc.getBBRecursive();
                Vector3D transported=tbb.rectcenter()-bbx.rectcenter();
                Point3D newplace = placementPosition - transported;
                tramo.Offset = newplace - bbx.rectcenter();
                tramo.Offset = tramo.Offset + new Vector3D(PUPPIGUISettings.CADManipExactDisplacement, PUPPIGUISettings.CADManipExactDisplacement, PUPPIGUISettings.CADManipExactDisplacement);
               
            }
            //tramo. = bbx.Size.X*0.1;
            //tramo.Direction = new Vector3D(1, 0, 0);
            tramo.Pivot = bbx.rectcenter();
            //tramo.Diameter = bbx.SizeX * 0.1;
            tramo.SetName("ObjTranslateManipulator");
           
            //tramo.
           

        }

        internal Rect3D getsfebbox()
        {
            Rect3D sfbb = new Rect3D();
            //minimal
            Rect3D mbb = new Rect3D(-1, -1, -1, 2, 2, 2);
            foreach (Visual3D vv in hv.Children)
            {
                //no lights
                if (vv is ModelVisual3D)
                {
                    ModelVisual3D v = vv as ModelVisual3D;
                    //no lights
                    if (v != tramo && v != tlv3d && v != dlv3d && v != flv3d && v != blv3d && v != llv3d && v != rlv3d && v != cv)
                    {
                        Rect3D nbb = Rect3D.Empty;
                        try
                        {
                            nbb = v.getBBRecursive();
                        }
                        catch
                        {
                            nbb = Rect3D.Empty;
                        }
                       sfbb.Union(nbb);
                    }
                }
            }
            sfbb.Union(mbb);
            return sfbb;
        }

        // public void addBox3D(double xStart, double yStart, double zStart, double xEnd, double yEnd, double zEnd, byte red, byte green, byte blue)
        internal string interpretMyTextualCommand(string myCommand)
        {
            string[] sep = { "_|_" };
            string[] myInput = myCommand.Split(sep, StringSplitOptions.None);
            try
            {
                if (myInput[0].ToLower() == "addbox3d")
                {
                    double xStart = Convert.ToDouble(myInput[1]);
                    double yStart = Convert.ToDouble(myInput[2]);
                    double zStart = Convert.ToDouble(myInput[3]);
                    double xEnd = Convert.ToDouble(myInput[4]);
                    double yEnd = Convert.ToDouble(myInput[5]);
                    double zEnd = Convert.ToDouble(myInput[6]);

                    byte red = Convert.ToByte(myInput[7]);
                    byte green = Convert.ToByte(myInput[8]);
                    byte blue = Convert.ToByte(myInput[9]);



                    PUPPIGUI.PUPPICustomRenderer pcr = new PUPPIGUI.PUPPICustomRenderer();
                    pcr.addBox3D(xStart, yStart, zStart, xEnd, yEnd, zEnd, red, green, blue);

                    pcr.forceRender();
                    if (pcr.model3D.Children.Count > 0)
                    {
                        ModelVisual3D nc = (pcr.model3D.Children[0] as ModelVisual3D).cloneVisualAndChildren();

                        int cnter = 0;
                        nc.SetName(modelGUIDs[modelGUIDCount]);
                        if (nc.Content != null && nc.Content is GeometryModel3D)
                        {

                            // MeshGeometry3D mgg=(nc.Content as GeometryModel3D).Geometry as MeshGeometry3D;
                            //mgg.Freeze();
                            Model3D mgg = nc.Content as Model3D;
                            mgg.Freeze();
                            serverObjects.TryAdd(modelGUIDs[modelGUIDCount] + "--" + cnter.ToString(), mgg);
                            cnter++;
                        }
                        foreach (ModelVisual3D mnc in nc.Children)
                        {
                            if (mnc.Content != null)
                            {
                                // MeshGeometry3D mgg = (mnc.Content as GeometryModel3D).Geometry as MeshGeometry3D;
                                Model3D mgg = mnc.Content as Model3D;
                                mgg.Freeze();
                                serverObjects.TryAdd(modelGUIDs[modelGUIDCount] + "--" + cnter.ToString(), mgg);
                                cnter++;
                            }
                        }
                        modelGUIDCount++;

                        hv.Children.Add(nc);
                        Rect3D bbox = getsfebbox();
                        redolighting(bbox);

                        return nc.GetName();
                    }
                    else
                        return "failure";
                }
                // public void addSphere3D(double xCenter, double yCenter, double zCenter, double radius, byte red, byte green, byte blue)
                if (myInput[0].ToLower() == "addsphere3d")
                {
                    double xCenter = Convert.ToDouble(myInput[1]);
                    double yCenter = Convert.ToDouble(myInput[2]);
                    double zCenter = Convert.ToDouble(myInput[3]);
                    double radius = Convert.ToDouble(myInput[4]);


                    byte red = Convert.ToByte(myInput[5]);
                    byte green = Convert.ToByte(myInput[6]);
                    byte blue = Convert.ToByte(myInput[7]);



                    PUPPIGUI.PUPPICustomRenderer pcr = new PUPPIGUI.PUPPICustomRenderer();
                    pcr.addSphere3D(xCenter, yCenter, zCenter, radius, red, green, blue);

                    pcr.forceRender();
                    if (pcr.model3D.Children.Count > 0)
                    {
                        ModelVisual3D nc = (pcr.model3D.Children[0] as ModelVisual3D).cloneVisualAndChildren();
                        int cnter = 0;
                        nc.SetName(modelGUIDs[modelGUIDCount]);
                        if (nc.Content != null && nc.Content is GeometryModel3D)
                        {

                            Model3D mgg = nc.Content;
                            mgg.Freeze();
                            serverObjects.TryAdd(modelGUIDs[modelGUIDCount] + "--" + cnter.ToString(), mgg);
                            cnter++;
                        }
                        foreach (ModelVisual3D mnc in nc.Children)
                        {
                            if (mnc.Content != null)
                            {
                                Model3D mgg = mnc.Content;
                                mgg.Freeze();
                                serverObjects.TryAdd(modelGUIDs[modelGUIDCount] + "--" + cnter.ToString(), mgg);
                                cnter++;
                            }
                        }
                        modelGUIDCount++;

                        hv.Children.Add(nc);
                        Rect3D bbox = getsfebbox();
                        redolighting(bbox);

                        return nc.GetName();
                    }
                    else
                        return "failure";
                }
                if (myInput[0].ToLower() == "addcadfile")
                {
                    string mypath = myInput[1];




                    PUPPIGUI.PUPPICustomRenderer pcr = new PUPPIGUI.PUPPICustomRenderer();
                    pcr.addFile3D(mypath);



                    pcr.forceRender();
                    if (pcr.model3D.Children.Count > 0)
                    {
                        ModelVisual3D nc = (pcr.model3D.Children[0] as ModelVisual3D).cloneVisualAndChildren();
                        int cnter = 0;
                        nc.SetName(modelGUIDs[modelGUIDCount]);
                        if (nc.Content != null && nc.Content is GeometryModel3D)
                        {

                            Model3D mgg = nc.Content;
                            mgg.Freeze();
                            serverObjects.TryAdd(modelGUIDs[modelGUIDCount] + "--" + cnter.ToString(), mgg);
                            cnter++;
                        }
                        foreach (ModelVisual3D mnc in nc.Children)
                        {
                            if (mnc.Content != null)
                            {
                                Model3D mgg = mnc.Content;
                                mgg.Freeze();
                                serverObjects.TryAdd(modelGUIDs[modelGUIDCount] + "--" + cnter.ToString(), mgg);
                                cnter++;
                            }
                        }
                        modelGUIDCount++;

                        hv.Children.Add(nc);
                        Rect3D bbox = getsfebbox();
                        redolighting(bbox);

                        return nc.GetName();
                    }
                    else
                        return "failure";
                }
                //      public void addPipe3D(double xStart, double yStart, double zStart, double xEnd, double yEnd, double zEnd, double diameter, byte red, byte green, byte blue)
                if (myInput[0].ToLower() == "removeobject")
                {
                    int rindex = -1;

                    for (int counter = 0; counter < hv.Children.Count; counter++)
                    {
                        Visual3D mv = hv.Children[counter];
                        if (mv.GetName() == myInput[1])
                        {
                            rindex = counter;
                            break;
                        }
                    }
                    if (rindex != -1)
                    {
                        hv.Children.RemoveAt(rindex);
                        List<string> remkeys = new List<string>();
                        foreach (string sss in serverObjects.Keys)
                        {
                            if (sss.Contains(myInput[1]))
                            {
                                remkeys.Add(sss);
                            }
                        }
                        foreach (string sss in remkeys)
                        {
                            Model3D ml = null;
                            serverObjects.TryRemove(sss, out ml);
                        }
                        Rect3D bbox = getsfebbox();
                        redolighting(bbox);
                        return "removed";
                    }
                    else
                    {
                        return "notfound";
                    }

                }
                if (myInput[0].ToLower() == "addpipe3d")
                {
                    double xStart = Convert.ToDouble(myInput[1]);
                    double yStart = Convert.ToDouble(myInput[2]);
                    double zStart = Convert.ToDouble(myInput[3]);
                    double xEnd = Convert.ToDouble(myInput[4]);
                    double yEnd = Convert.ToDouble(myInput[5]);
                    double zEnd = Convert.ToDouble(myInput[6]);
                    double diameter = Convert.ToDouble(myInput[7]);
                    byte red = Convert.ToByte(myInput[8]);
                    byte green = Convert.ToByte(myInput[9]);
                    byte blue = Convert.ToByte(myInput[10]);



                    PUPPIGUI.PUPPICustomRenderer pcr = new PUPPIGUI.PUPPICustomRenderer();
                    pcr.addPipe3D(xStart, yStart, zStart, xEnd, yEnd, zEnd, diameter, red, green, blue);

                    pcr.forceRender();
                    if (pcr.model3D.Children.Count > 0)
                    {
                        ModelVisual3D nc = (pcr.model3D.Children[0] as ModelVisual3D).cloneVisualAndChildren();

                        int cnter = 0;
                        nc.SetName(modelGUIDs[modelGUIDCount]);
                        if (nc.Content != null && nc.Content is GeometryModel3D)
                        {

                            Model3D mgg = nc.Content;
                            mgg.Freeze();
                            serverObjects.TryAdd(modelGUIDs[modelGUIDCount] + "--" + cnter.ToString(), mgg);
                            cnter++;
                        }
                        foreach (ModelVisual3D mnc in nc.Children)
                        {
                            if (mnc.Content != null)
                            {
                                Model3D mgg = mnc.Content;
                                mgg.Freeze();
                                serverObjects.TryAdd(modelGUIDs[modelGUIDCount] + "--" + cnter.ToString(), mgg);
                                cnter++;
                            }
                        }
                        modelGUIDCount++;

                        hv.Children.Add(nc);
                        Rect3D bbox = getsfebbox();
                        redolighting(bbox);

                        return nc.GetName();
                    }
                    else
                        return "failure";
                }
                if (myInput[0].ToLower() == "addstlstring")
                {

                    ModelVisual3D mv = PUPPICAD.HelperClasses.utilities.ReadSTLFromString(myInput[1], myInput[2]);
                    hv.Children.Add(mv);
                    Rect3D bbox = getsfebbox();
                    redolighting(bbox);
                }

                return "Unrecognized command";
            }
            catch (Exception exy)
            {
                return "Error";
            }
        }


        //holds the logic
        //internal PUPPIProgram program;
        internal PUPPI3DView(double left, double top, double width, double height)
        {



            //initialize the grid

            canvasview = new Grid();
            //   canvasview.Margin = new Thickness(left, top, 0, 0);
            canvasview.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            canvasview.VerticalAlignment = VerticalAlignment.Stretch;

            //canvasview.Height = height;
            //canvasview.Width = width;


            tw = (int)width;
            th = (int)height;

            //3d view
            hv = new HelixViewport3D();
            hv.AllowDrop = true;
            hv.Background = new LinearGradientBrush(Color.FromRgb(20, 20, 240), Color.FromRgb(255, 255, 255), 90);
            hv.ShowCoordinateSystem = true;
            hv.ShowViewCube = true;
            cv = new CoordinateSystemVisual3D();
            hv.Children.Add(cv);
            ////Lighting
            tlv3d = new ModelVisual3D();
            dlv3d = new ModelVisual3D();
            flv3d = new ModelVisual3D();
            blv3d = new ModelVisual3D();
            rlv3d = new ModelVisual3D();
            llv3d = new ModelVisual3D();
            redolighting();
            hv.Children.Add(tlv3d);
            hv.Children.Add(dlv3d);
            hv.Children.Add(flv3d);
            hv.Children.Add(blv3d);
            hv.Children.Add(rlv3d);
            hv.Children.Add(llv3d);
            //DefaultLights dl = new DefaultLights();
            //hv.Children.Add(dl);
            //Show it all
            frame3d = new Frame();
            frame3d.AllowDrop = true;

            frame3d.Content = hv;
            //frame3d.ClipToBounds = true;
            //frame3d.Width = width;
            //frame3d.Height = height;


            frame3d.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;


            frame3d.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;

            frame3d.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;


            frame3d.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;



            canvasview.Children.Add(frame3d);
            //frame3d.Margin = new Thickness(0,0,0,0) ;  
            hv.AllowDrop = true;

            hv.BorderBrush = Brushes.LightBlue;
            hv.BorderThickness = new Thickness(2);

            System.Windows.Controls.DockPanel d = new System.Windows.Controls.DockPanel();


            if (PUPPIGUISettings.CADWindowsShowExportButton)
            {
                exportscene = new System.Windows.Controls.MenuItem();
                exportscene.Header = "Export Geometry";
                exportscene.Background = Brushes.LightBlue;
                exportscene.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                exportscene.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                exportscene.Click += exportscene_Click;



                // this.canvasview.Children.Add(exportscene);
                d.Children.Add(exportscene);
            }

            if (PUPPIGUISettings.CADWindowsShowZEButton)
            {

                zoomextents = new System.Windows.Controls.MenuItem();
                zoomextents.Header = "Zoom Extents";
                zoomextents.Background = Brushes.LightBlue;

                zoomextents.Click += zoomextents_Click;
                // zoomextents.Margin=new Thickness() 
                zoomextents.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                zoomextents.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                //zoomextents.MaxWidth = 100; 
                d.Children.Add(zoomextents);
                //   zoomextents.Margin = new Thickness(210 , 0, 0  , 0);
                //  this.canvasview.Children.Add(zoomextents);
                //this.canvasview.Children.Add(d);
            }

            if (PUPPIGUISettings.CADWindowsShowCoordArrowsButton)
            {
                toggleCSA = new System.Windows.Controls.MenuItem();
                toggleCSA.Header = "Toggle Coord. Arrows";
                toggleCSA.Background = Brushes.LightBlue;

                toggleCSA.Click += toggleCSA_Click;
                // zoomextents.Margin=new Thickness() 
                toggleCSA.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                toggleCSA.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                //zoomextents.MaxWidth = 100; 
                d.Children.Add(toggleCSA);

            }

            if (PUPPIGUISettings.CADWindowsShowAddObjButton)
            {
                addObj = new System.Windows.Controls.MenuItem();
                addObj.Header = "Add Object";
                addObj.Background = Brushes.LightBlue;

                addObj.Click += addObj_Click;
                // zoomextents.Margin=new Thickness() 
                addObj.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                addObj.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                //zoomextents.MaxWidth = 100; 
                d.Children.Add(addObj);

            }
            //   zoomextents.Margin = new Thickness(210 , 0, 0  , 0);
            //  this.canvasview.Children.Add(zoomextents);
            this.canvasview.Children.Add(d);
            frame3d.MouseDown += PUPPI3DView_MouseDown;
         //   frame3d.DragOver += hv_DragOver;
            commandQueue = new SynchronizedCollection<string>();
            commandQueueResults = new List<string>();
            serverObjects = new ConcurrentDictionary<string, Model3D>();
            somewhereProcessingStopped += (sender, args) => { runStringCanvasCommands(); };

            if (modelGUIDs == null)
            {
                modelGUIDs = new SynchronizedCollection<string>();
                for (int gc = 0; gc < PUPPIGUISettings.CADMaxGUIDs; gc++)
                {
                    modelGUIDs.Add(Guid.NewGuid().ToString());
                }
            }


            Thread backgroundExeMonitor = new Thread(new ThreadStart(this.backgroundMonitorCommands));
            backgroundExeMonitor.IsBackground = true;
            backgroundExeMonitor.Name = "CSCO";
            backgroundExeMonitor.Start();
        }

        void hv_DragOver(object sender, System.Windows.DragEventArgs e)
        {
            int i=3;
        }

       

        void frame3d_Drop(object sender, System.Windows.DragEventArgs e)
        {
            string sev = "";
            var p = e.GetPosition(hv);

            Vector3D n;
            Point3D pos;
            var source = FS(p, out n, out pos);

            if (source != null && source.Visual != null)
            {
                if (source.Visual is ModelVisual3D == false) return;


                int index = -1;
                bool found = false;
                for (int i = 0; i < hv.Children.Count; i++)
                {

                    if ((hv.Children[i] as Visual3D) is ModelVisual3D == false) continue;
                    ModelVisual3D v = hv.Children[i] as ModelVisual3D;
                    //no lights
                    if (v != tramo && v != tlv3d && v != dlv3d && v != flv3d && v != blv3d && v != llv3d && v != rlv3d && v != cv)
                    {
                        index++;
                        try
                        {
                            if (v == source.Visual) //|| v.Children.Contains(source.Visual) || (source.Visual as ModelVisual3D).Children.Contains(v))
                            {
                                found = true;
                                sev = source.Visual.GetName() + "_|_";
                                break;
                            }
                            else
                            {
                                ModelVisual3D vs = source.Visual as ModelVisual3D;
                                vs = VisualTreeHelper.GetParent(vs) as ModelVisual3D;
                                while (vs != null && vs != v)
                                {
                                    vs = VisualTreeHelper.GetParent(vs) as ModelVisual3D;
                                }
                                if (vs != null)
                                {
                                    found = true;
                                    sev = vs.GetName() + "_|_";
                                    break;
                                }
                            }
                        }
                        catch (Exception exy)
                        {

                        }

                    }
                }
                if (found) sev += index.ToString() + "_|_";
            }
            sev += "{" + pos.X.ToString() + "," + pos.Y.ToString() + "," + pos.Z.ToString() + "}";
            if (sev != null && sev != "")
            {

               
                
                    sev = "DROP"  + "_|_" + sev;
                
            }
            if (dropped3DW != null && sev != null && sev != "") dropped3DW(sev, EventArgs.Empty);
        }
        private static readonly PropertyInfo Visual3DModelPropertyInfo = typeof(Visual3D).GetProperty("Visual3DModel", BindingFlags.Instance | BindingFlags.NonPublic);

        void addObj_Click(object sender, RoutedEventArgs e)
        {
            //just import for now

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            string fp = "";
            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                fp = dlg.FileName;
            }
            else return;
            try
            {
                FileModelVisual3D fv = new FileModelVisual3D();
                fv.Source = fp;
                ModelVisual3D gg = new ModelVisual3D();
                if (fv.Source != null)
                {

                    Model3DGroup ga = Visual3DModelPropertyInfo.GetValue(fv, null) as Model3DGroup;
                    foreach (Model3D m in ga.Children)
                    {

                        ModelVisual3D g = new ModelVisual3D();
                        //default material
                        GeometryModel3D mg = m as GeometryModel3D;

                        if (mg.Material == null)
                        {
                            mg.Material = new DiffuseMaterial(Brushes.White);
                            g.Content = mg;
                        }
                        else
                        {
                            g.Content = m;
                        }


                        gg.Children.Add(g);
                    }

                }
                if (gg.Children.Count > 0)
                {
                    ModelVisual3D ml = gg.cloneVisualAndChildren();
                    // ml.SetName(modelGUIDs[modelGUIDCount]);
                    hv.Children.Add(ml);
                    //modelGUIDCount++;
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Error loading model");
            }
        }


        internal EventHandler somewhereProcessingStopped;




        void backgroundMonitorCommands()
        {
            int pstatus = 0;
            if (PUPPIServer.PUPPICADTCPServer.NumberClientThreads + PUPPIServer.PUPPICADHTTPServer.NumberClientThreads > 0)
                pstatus = 1;

            while (true)
            {
                long curstatus = PUPPIServer.PUPPICADTCPServer.NumberClientThreads + PUPPIServer.PUPPICADHTTPServer.NumberClientThreads;
                if (curstatus > 0)
                {
                    curstatus = 1;
                }
                else
                {
                    curstatus = 0;
                }


                if (pstatus == 1 && curstatus == 0)
                {
                    if (somewhereProcessingStopped != null)
                    {

                        Dispatcher.BeginInvoke(new EventHandler(somewhereProcessingStopped), this, null);
                        //  somewhereProcessingStopped(this, null);
                        //});
                    }

                }
                pstatus = (int)curstatus;
            }
        }

        Viewport3DHelper.HitResult FS(Point p, out Vector3D normal, out Point3D pos)
        {

            var hits = Viewport3DHelper.FindHits(hv.Viewport, p);

            foreach (var h in hits)
            {
                pos = h.Position;
                normal = h.Normal;
                return h;//.Model;
            }


            pos = new Point3D();
            normal = new Vector3D();
            return null;
        }
        void PUPPI3DView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string sev = "";
            var p = e.GetPosition(hv);

            Vector3D n;
            Point3D pos;
            var source = FS(p, out n, out pos);

            if (source != null && source.Visual != null)
            {
                if (source.Visual is ModelVisual3D == false) return;
               

                int index = -1;
                bool found = false;
                for (int i = 0; i < hv.Children.Count; i++)
                {

                    if ((hv.Children[i] as Visual3D) is ModelVisual3D == false) continue;
                    ModelVisual3D v = hv.Children[i] as ModelVisual3D;
                    //no lights
                    if (v!=tramo && v != tlv3d && v != dlv3d && v != flv3d && v != blv3d && v != llv3d && v != rlv3d && v != cv)
                    {
                        index++;
                        try
                        {
                            if (v == source.Visual) //|| v.Children.Contains(source.Visual) || (source.Visual as ModelVisual3D).Children.Contains(v))
                            {
                                found = true;
                                sev = source.Visual.GetName() + "_|_";
                                break;
                            }
                            else
                            {
                                ModelVisual3D vs=source.Visual as ModelVisual3D;
                                vs=VisualTreeHelper.GetParent(vs) as ModelVisual3D;
                                while (vs!=null && vs!=v)
                                {
                                    vs = VisualTreeHelper.GetParent(vs) as ModelVisual3D;
                                }
                                if (vs!=null)
                                {
                                    found = true;
                                    sev = vs.GetName() + "_|_";
                                    break;
                                }
                            }
                        }
                        catch (Exception exy)
                        {

                        }

                    }
                }
                if (found) sev += index.ToString() + "_|_";
            }
            sev += "{" + pos.X.ToString() + "," + pos.Y.ToString() + "," + pos.Z.ToString() + "}";
            if (sev != null && sev != "")
            {
                
                if (e.ChangedButton == MouseButton.Right)
                {
                    sev = "RB"+e.ClickCount+"_|_" + sev;
                }
                else if (e.ChangedButton == MouseButton.Left)
                {
                    sev = "LB" + e.ClickCount + "_|_" + sev;
                }
                else
                {
                    sev = "MB" + e.ClickCount + "_|_" + sev;
                }
            }
            if (clicked3DW != null && sev != null && sev != "") clicked3DW(sev, EventArgs.Empty);
        }

        void toggleCSA_Click(object sender, RoutedEventArgs e)
        {
            if (hv.Children.Contains(cv))
            {
                hv.Children.Remove(cv);
            }
            else
            {
                hv.Children.Add(cv);
            }
        }
        //this redoes the lighting based on bounding box
        internal void redolighting(Rect3D bbox = new Rect3D())
        {   //top bot

            double diag=Math.Max(bbox.getDiagonal(),1.0);

            PointLight tlight = new PointLight(Colors.White, new Point3D(bbox.Location.X + bbox.SizeX / 2, bbox.Location.Y + bbox.SizeY / 2, 10 + bbox.SizeZ + bbox.Location.Z + bbox.SizeZ));
           // tlight.Range = diag;
            //tlight.Color = System.Windows.Media.Color.FromRgb(255, 255, 255);
            tlight.ConstantAttenuation = Math.Pow( 1.0 / Math.Max(bbox.SizeZ, 1.0),2); // bbox.SizeZ / 3;

            tlv3d.Content = tlight;

            PointLight dlight = new PointLight(Colors.White, new Point3D(bbox.Location.X + bbox.SizeX / 2, bbox.Location.Y + bbox.SizeY / 2, -10 - bbox.SizeZ + bbox.Location.Z));
            //dlight.Range = diag;
            //dlight.Color = System.Windows.Media.Color.FromRgb(255, 255, 255);
            dlight.ConstantAttenuation = Math.Pow( 1.0 / Math.Max(bbox.SizeZ, 1.0),2);
            dlv3d.Content = dlight;
            //front back

            PointLight flight = new PointLight(Colors.White, new Point3D(bbox.Location.X - 10 - bbox.SizeX, bbox.Location.Y + bbox.SizeY / 2, bbox.Location.Z + bbox.SizeZ / 2));
            flight.ConstantAttenuation = Math.Pow( 1.0 / Math.Max(bbox.SizeX, 1.0),2);
            //flight.Color = System.Windows.Media.Color.FromRgb(255, 255, 255);
            //flight.Range = diag;
            flv3d.Content = flight;

            PointLight blight = new PointLight(Colors.White, new Point3D(bbox.Location.X + bbox.SizeX + 10 + bbox.SizeX, bbox.Location.Y + bbox.SizeY / 2, bbox.Location.Z + bbox.SizeZ / 2));
            blight.ConstantAttenuation = Math.Pow( 1.0 / Math.Max(bbox.SizeX, 1.0),2);
            //blight.Color = System.Windows.Media.Color.FromRgb(255, 255, 255);
            //blight.Range = diag;
            blv3d.Content = blight;
            //right and left
            PointLight rlight = new PointLight(Colors.White, new Point3D(bbox.Location.X + bbox.SizeX / 2, bbox.Location.Y - 10 - bbox.SizeY, bbox.Location.Z + bbox.SizeZ / 2));
            rlight.ConstantAttenuation =Math.Pow(  1.0 / Math.Max(bbox.SizeY, 1.0),2);
            //rlight.Color = System.Windows.Media.Color.FromRgb(255, 255, 255);
            //rlight.Range = diag;
            rlv3d.Content = rlight;

            PointLight llight = new PointLight(Colors.White, new Point3D(bbox.Location.X + bbox.SizeX / 2, bbox.Location.Y + bbox.SizeY + 10 + bbox.SizeY, bbox.Location.Z + bbox.SizeZ / 2));
            llight.ConstantAttenuation =Math.Pow( 1.0 / Math.Max(bbox.SizeY, 1.0),2);
            //llight.Color = System.Windows.Media.Color.FromRgb(255, 255, 255);
            //llight.Range =diag;
            llv3d.Content = llight;

        }
        void zoomextents_Click(object sender, RoutedEventArgs e)
        {
            hv.ZoomExtents(500);
        }

        void exportscene_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog d = new Microsoft.Win32.SaveFileDialog();

            d.Filter = Exporters.Filter + "|STL ASCII Files (*.stl)|*.stl";
            d.DefaultExt = Exporters.DefaultExtension;
            Nullable<bool> result = d.ShowDialog();

            if (result == true)
            {

                if (hv.Children.Contains(tramo)) hv.Children.Remove(tramo);
                if (d.FileName.Contains(".STL") || d.FileName.Contains(".stl"))
                {
                    //special export to stl
                    HelperClasses.helperfunctions.exportSTLA(hv.Viewport, d.FileName);
                }
                else
                {
                    try
                    {
                        Viewport3DHelper.Export(hv.Viewport, d.FileName);
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("Export error - possible bug in Helix Toolkit: objz file format not supported");
                    }
                }
            }


        }
    }
    /// <summary>
    /// PUPPI Module to connect to an existing PUPPICAD View and place 3D Objects.
    /// </summary>
    public class PUPPICADModel : PUPPIModule
    {

        internal static object savedsource = null;
        //this loads the source reference
        //will input a list of objects added to the helix viewport
        /// <summary>
        /// Constructor for the Module that allows a PUPPI program to output to a PUPPI CAD View.
        /// </summary>
        /// <param name="myPUPPICADview">An instance of PUPPICADView class</param>
        public PUPPICADModel(object myPUPPICADview)
            : base()
        {
            name = "3D Model Access";
            description = "Renders a 3D object or a list of objects in CAD Window.";
            outputs.Add(myPUPPICADview);
            savedsource = myPUPPICADview;
            outputnames.Add("Model");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("3D Obj/List");
            completeProcessOverride = true;
            canBeInContainer = false;
            //we can only drag one down
            unique = true;
        }
        public PUPPICADModel()
            : base()
        {
            name = "3D Model Access";
            outputs.Add(savedsource);
            description = "Output one or more 3D objects to CAD window";
            outputnames.Add("Model");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("3D Obj/List");
            completeProcessOverride = true;
            canBeInContainer = false;
            //we can only drag one down
            unique = true;
        }
        public override void process_usercode()
        {



            try
            {

                //clear window
                //we will actually connect to the publicly available view

                PUPPIGUIController.PUPPICADView pgv = savedsource as PUPPIGUIController.PUPPICADView;
                PUPPI3DView mycad = pgv.p3dview;

                HelixViewport3D hhh = mycad.hv;
                hhh.Children.Clear();
                //hhh.Children.Add(mycad.cv);
                Rect3D bbox = new Rect3D();

                if (inputs[0].module != null)
                {
                    //one object
                    if (inputs[0].module.outputs[inputs[0].outParIndex] is ModelVisual3D)
                    {
                        ModelVisual3D mv = inputs[0].module.outputs[inputs[0].outParIndex] as ModelVisual3D;
                        ModelVisual3D mvc = mv.cloneMyVisual();
                        //mvc.SetName(PUPPI3DView.modelGUIDs[PUPPI3DView.modelGUIDCount]);
                        //PUPPI3DView.modelGUIDCount++;
                        //mvc.RecalVisNo();
                        hhh.Children.Add(mvc);
                       
                        //  mycad.redolighting(mv.Content.Bounds );
                        bbox = Rect3D.Union(bbox, mv.getBBRecursive());
                        mycad.redolighting(bbox);
                    }
                    else if (inputs[0].module.outputs[inputs[0].outParIndex] is Model3D)
                    {

                        System.Windows.Media.Media3D.ModelVisual3D mvc = new System.Windows.Media.Media3D.ModelVisual3D();
                        Model3D m = (inputs[0].module.outputs[inputs[0].outParIndex] as Model3D).Clone();


                        mvc.Content = m;


                        //mvc.SetName(PUPPI3DView.modelGUIDs[PUPPI3DView.modelGUIDCount]);
                        //PUPPI3DView.modelGUIDCount++;
                      //mvc.RecalVisNo();
                        hhh.Children.Add(mvc);
                        bbox = Rect3D.Union(bbox, mvc.getBBRecursive());
                        mycad.redolighting(bbox);

                    }


                        //or multiple
                    else if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                    {
                        ArrayList linput = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]) as ArrayList;

                        if (linput != null)
                        {

                            //adds all the elements to the window
                            for (int i = 0; i < linput.Count; i++)
                            {
                                if (linput[i] is ModelVisual3D)
                                {
                                    object ooo = linput[i];
                                    ModelVisual3D mv = ooo as ModelVisual3D;

                                    ModelVisual3D mvc = mv.cloneMyVisual();
                                    // mvc.SetName(PUPPI3DView.modelGUIDs[PUPPI3DView.modelGUIDCount]);
                                    //PUPPI3DView.modelGUIDCount++;
                                 // mvc.RecalVisNo();
                                    hhh.Children.Add(mvc);
                                    
                                    bbox = Rect3D.Union(bbox, mv.getBBRecursive());
                                }
                                else if (linput[i] is Model3D)
                                {
                                    System.Windows.Media.Media3D.ModelVisual3D mvc = new System.Windows.Media.Media3D.ModelVisual3D();
                                    Model3D m = (linput[i] as Model3D).Clone();
                                    mvc.Content = m;
                                  // mvc.RecalVisNo();
                                    //mvc.SetName(PUPPI3DView.modelGUIDs[PUPPI3DView.modelGUIDCount]);
                                    //PUPPI3DView.modelGUIDCount++;
                                    hhh.Children.Add(mvc);

                                    bbox = Rect3D.Union(bbox, mvc.getBBRecursive());
                                }
                            }
                            //redo lights

                            mycad.redolighting(bbox);
                        }
                    }
                    else
                    {
                        outputs[0] = "Invalid input data";
                        return;
                    }
                }

                hhh.Children.Add(mycad.tlv3d);
                hhh.Children.Add(mycad.dlv3d);
                hhh.Children.Add(mycad.flv3d);
                hhh.Children.Add(mycad.blv3d);
                hhh.Children.Add(mycad.rlv3d);
                hhh.Children.Add(mycad.llv3d);

                if (pgv.savedCADWinObjects != null)
                {
                    foreach (ModelVisual3D vb in pgv.savedCADWinObjects)
                    {

                        //vb.SetName(PUPPI3DView.modelGUIDs[PUPPI3DView.modelGUIDCount]);
                        //PUPPI3DView.modelGUIDCount++;

                        hhh.Children.Add(vb);
                        bbox = Rect3D.Union(bbox, vb.getBBRecursive());
                        mycad.redolighting(bbox);
                    }
                }

                ModelVisual3D ccv = mycad.cv.Children[0] as ModelVisual3D;
                // ccv.
                //scale down the coord system
                if (ccv.Content.Bounds.getDiagonal() > bbox.getDiagonal() / 2 && bbox.getDiagonal() > 0)
                {
                    ScaleTransform3D sc = new ScaleTransform3D(bbox.getDiagonal() / 2 / ccv.Content.Bounds.getDiagonal(), bbox.getDiagonal() / 2 / ccv.Content.Bounds.getDiagonal(), bbox.getDiagonal() / 2 / ccv.Content.Bounds.getDiagonal());
                    mycad.cv.Transform = sc;
                }
                hhh.Children.Add(mycad.cv);
                hhh.ZoomExtents();
            }
            catch
            {
                outputs[0] = "error";
                return;
            }





            return;
        }
    }
    /// <summary>
    /// Returns list of ModelVisual3D objects which are in CAD Window
    /// </summary>
    public class PUPPIGetCADViewContents : PUPPIModule
    {
        internal static object savedsource = null;
        //this loads the source reference
        //will output a list of objects added to the helix viewport
        /// <summary>
        /// Constructor for the Module that allows a PUPPI program to output contents of  PUPPI CAD View.
        /// </summary>
        /// <param name="myPUPPICADview">An instance of PUPPICADView class</param>
        public PUPPIGetCADViewContents(object myPUPPICADview)
            : base()
        {
            name = "Obj in CAD View";
            description = "Gets list of objects in CAD view.";
            outputs.Add(null);
            savedsource = myPUPPICADview;
            outputnames.Add("Obj. List");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Dummy Input");
            completeProcessOverride = true;
            canBeInContainer = false;
            //we can only drag one down
            unique = true;

        }
        public PUPPIGetCADViewContents()
            : base()
        {
            name = "Obj in CAD View";
            description = "Gets list of objects in CAD view.";
            outputs.Add(null);

            outputnames.Add("Obj. List");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Dummy Input");
            completeProcessOverride = true;
            canBeInContainer = false;
            //we can only drag one down
            unique = true;

        }
        public override void process_usercode()
        {

            if (savedsource == null)
            {
                outputs[0] = "null CAD";
                return;
            }



            PUPPIGUIController.PUPPICADView pgv = savedsource as PUPPIGUIController.PUPPICADView;

            if (pgv == null || pgv.p3dview == null)
            {
                outputs[0] = "null CAD";
                return;
            }

            PUPPI3DView mycad = pgv.p3dview;

            HelixViewport3D hhh = mycad.hv;
            List<ModelVisual3D> objs = new List<ModelVisual3D>();
            foreach (object o in hhh.Children)
            {
                if (o is ModelVisual3D)
                {
                    if (o == mycad.tlv3d || o == mycad.dlv3d || o == mycad.flv3d || o == mycad.blv3d || o == mycad.llv3d || o == mycad.rlv3d || o == mycad.cv) continue;

                    ModelVisual3D mo = o as ModelVisual3D;

                    ModelVisual3D moc = mo.cloneVisualAndChildren();
                    objs.Add(moc);
                }
            }
            outputs[0] = objs;
        }
    }

    /// <summary>
    /// PUPPI Module to connect to write a 3D Model to a file
    /// </summary>
    public class PUPPI3DModel2File : PUPPIModule
    {

        public PUPPI3DModel2File()
            : base()
        {
            name = "3D to STL";

            description = "Writes one or more 3D models to an ASCII STL file. Returns 1 or 0.";
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("3D Obj/List");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("File Path");
            outputs.Add(0);
            outputnames.Add("Operation result");
        }
        public override void process_usercode()
        {
            Rect3D bbox = new Rect3D();
            ModelVisual3D mv = new ModelVisual3D();

            //one object
            if (usercodeinputs[0] is ModelVisual3D)
            {
                mv = usercodeinputs[0] as ModelVisual3D;
                //bbox = Rect3D.Union(bbox, mv.getBBRecursive());

            }
            else if (usercodeinputs[0] is Model3D)
            {

                Model3D m = usercodeinputs[0] as Model3D;
                mv.Content = m;
                // bbox = Rect3D.Union(bbox, mv.getBBRecursive());
            }
            //or multiple
            else if (usercodeinputs[0] is ICollection || usercodeinputs[0] is IEnumerable)
            {
                ArrayList linput = PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[0]) as ArrayList;

                if (linput != null)
                {



                    //adds all the elements to the window
                    for (int i = 0; i < linput.Count; i++)
                    {
                        if (linput[i] is ModelVisual3D)
                        {
                            ModelVisual3D mvi = linput[i] as ModelVisual3D;
                            mv.addFlatVisualAndChildren(mvi);
                            //bbox = Rect3D.Union(bbox, mv.getBBRecursive());
                        }
                        else if (linput[i] is Model3D)
                        {
                            System.Windows.Media.Media3D.ModelVisual3D mvi = new System.Windows.Media.Media3D.ModelVisual3D();
                            Model3D m = linput[i] as Model3D;
                            mvi.Content = m;
                            mv.addFlatVisualAndChildren(mvi);
                            //bbox = Rect3D.Union(bbox, mv.getBBRecursive());
                        }
                    }

                }
            }
            else
            {
                usercodeoutputs[0] = "Invalid input";
                return;
            }


            string filename = usercodeinputs[1].ToString();
            ArrayList lines = new ArrayList();
            HelperClasses.helperfunctions.writegeomSTLrecursivelyWT(mv, lines, "PUPPICADexport", new Transform3DGroup());
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
            {
                foreach (string line in lines)
                {


                    file.WriteLine(line);

                }
            }
            usercodeoutputs[0] = "success";
        }

    }

    /// <summary>
    /// Combines models supplied as a list into a single model. Set fit by specifying X Y Z axes or R for reset transforms or anything else including null to simply put models at their coordinates.";
    /// </summary>
    public class PUPPICombine3DModels : PUPPIModule
    {
        public PUPPICombine3DModels()
            : base()
        {
            name = "Combine 3D Models";
            description = "Combines models supplied as a list into a single model. Set fit by specifying X Y Z axes or R for reset transforms or anything else including null to simply put models at their coordinates.";
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("3D Obj/List");
            PUPPIInParameter align = new PUPPIInParameter();
            align.isoptional = true;
            inputs.Add(align);
            inputnames.Add("Align (X/Y/Z/R/N");
            outputs.Add("nothing");
            outputnames.Add("ModelVisual3D");

        }
        public override void process_usercode()
        {

            Rect3D bbox = new Rect3D();
            ModelVisual3D mv = new ModelVisual3D();
            List<ModelVisual3D> modelVisuals = new List<ModelVisual3D>();
            //one object
            if (usercodeinputs[0] is ModelVisual3D)
            {
                mv = (usercodeinputs[0] as ModelVisual3D).cloneMyVisual();
                //bbox = Rect3D.Union(bbox, mv.getBBRecursive());
                modelVisuals.Add(mv);

            }
            else if (usercodeinputs[0] is Model3D)
            {

                Model3D m = (usercodeinputs[0] as Model3D).Clone();
                mv.Content = m;
                // bbox = Rect3D.Union(bbox, mv.getBBRecursive());
                modelVisuals.Add(mv);
            }
            //or multiple
            else if (usercodeinputs[0] is ICollection || usercodeinputs[0] is IEnumerable)
            {
                ArrayList linput = PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[0]) as ArrayList;


                if (linput != null)
                {



                    //adds all the elements to the window
                    for (int i = 0; i < linput.Count; i++)
                    {
                        if (linput[i] is ModelVisual3D)
                        {
                            ModelVisual3D mvi = (linput[i] as ModelVisual3D).cloneMyVisual();
                            modelVisuals.Add(mvi);
                            //bbox = Rect3D.Union(bbox, mv.getBBRecursive());
                        }
                        else if (linput[i] is Model3D)
                        {
                            System.Windows.Media.Media3D.ModelVisual3D mvi = new System.Windows.Media.Media3D.ModelVisual3D();
                            Model3D m = (linput[i] as Model3D).Clone();
                            mvi.Content = m;
                            modelVisuals.Add(mvi);
                            //bbox = Rect3D.Union(bbox, mv.getBBRecursive());
                        }
                    }

                }
            }
            else
            {
                usercodeoutputs[0] = "Invalid input";
                return;
            }
            string alignMode = "noAlign";
            if (usercodeinputs[1] != null)
            {
                alignMode = usercodeinputs[1].ToString();
            }




            ModelVisual3D toAdd = new ModelVisual3D();
            List<Rect3D> bbxes = new List<Rect3D>();
            for (int i = 0; i < modelVisuals.Count; i++)
            {
                bbxes.Add(modelVisuals[i].getBBRecursive());
            }
            Rect3D largestBB = new Rect3D(0, 0, 0, 0, 0, 0);
            for (int i = 0; i < bbxes.Count; i++)
            {
                if (bbxes[i].getDiagonal() > largestBB.getDiagonal())
                {
                    largestBB = bbxes[i];
                }
            }
            if (largestBB.IsEmpty)
            {
                usercodeoutputs[0] = "Empty model";
                return;
            }


            mv = new ModelVisual3D();
            double curheight = 0;
            for (int i = 0; i < modelVisuals.Count; i++)
            {
                Rect3D currentBB = new Rect3D(-largestBB.SizeX / 2, -largestBB.SizeY / 2, curheight, largestBB.SizeX, largestBB.SizeY, largestBB.SizeZ);
                ModelVisual3D bv = new ModelVisual3D();
                Rect3D mbb = bbxes[i];
                if (alignMode == "X" || alignMode == "x")
                {
                    double xcenter = i * largestBB.SizeX;
                    double ycenter = 0;
                    double zcenter = 0;
                    double deltax = xcenter - mbb.rectcenter().X;
                    double deltay = ycenter - mbb.rectcenter().Y;
                    double deltaz = zcenter - mbb.rectcenter().Z;
                    bv = modelVisuals[i].translateVisualAndChildren(deltax, deltay, deltaz);
                    mv.addFlatVisualAndChildren(bv);

                }
                else if (alignMode == "Y" || alignMode == "y")
                {
                    double xcenter = 0;
                    double ycenter = i * largestBB.SizeY;
                    double zcenter = 0;
                    double deltax = xcenter - mbb.rectcenter().X;
                    double deltay = ycenter - mbb.rectcenter().Y;
                    double deltaz = zcenter - mbb.rectcenter().Z;
                    bv = modelVisuals[i].translateVisualAndChildren(deltax, deltay, deltaz);
                    mv.addFlatVisualAndChildren(bv);
                }
                else if (alignMode == "Z" || alignMode == "z")
                {
                    double xcenter = 0;
                    double ycenter = 0;
                    double zcenter = i * largestBB.SizeZ;
                    double deltax = xcenter - mbb.rectcenter().X;
                    double deltay = ycenter - mbb.rectcenter().Y;
                    double deltaz = zcenter - mbb.rectcenter().Z;
                    bv = modelVisuals[i].translateVisualAndChildren(deltax, deltay, deltaz);
                    mv.addFlatVisualAndChildren(bv);
                }
                else if (alignMode == "R" || alignMode == "r")
                {
                    double deltax = 0;
                    double deltay = 0;
                    double deltaz = 0;
                    bv = modelVisuals[i].translateVisualAndChildren(deltax, deltay, deltaz);
                    mv.addFlatVisualAndChildren(bv);
                }

                else
                {


                    mv.Children.Add(modelVisuals[i]);
                }








            }
            usercodeoutputs[0] = mv;
        }
    }
    public class PUPPIGetModelAsComponents : PUPPIModule
    {
        public PUPPIGetModelAsComponents()
            : base()
        {
            name = "Explode Model";
            description = "Gets the content Model3D and recursively the children Model3D into a list";
            outputs.Add(null);
            outputnames.Add("3D Model List");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("ModelVisual3D");


        }

        public override void process_usercode()
        {
            ModelVisual3D mv = usercodeinputs[0] as ModelVisual3D;
            List<GeometryModel3D> gg = new List<GeometryModel3D>();
            mv.getAllGeometryChildren(gg);
            usercodeoutputs[0] = gg;
        }
    }


    /// <summary>
    /// will create a cube by specifying a point for origin and a vector for diagonal
    ///optional transform
    /// </summary>
    public class PUPPIBox_Point_VectorDiag : PUPPIModule
    {
        public PUPPIBox_Point_VectorDiag()
            : base()
        {
            name = "Box";
            outputs.Add(new ModelVisual3D());

            outputnames.Add("3D Object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Orig(Pt3D)");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Diag(V3D)");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Color");
            //also any transofrms
            PUPPIInParameter ti = new PUPPIInParameter();
            ti.isoptional = true;
            inputs.Add(ti);
            inputnames.Add("Transf./List");
        }
        public override void process_usercode()
        {

            try
            {
                var meshBuilder = new MeshBuilder(false, false);
                Point3D corigin = (Point3D)(usercodeinputs[0]);
                Vector3D vdiag = (Vector3D)(usercodeinputs[1]);
                //if a list of transforms is presented
                Transform3DGroup groupo = new Transform3DGroup();
                if (usercodeinputs[3] != null)
                {
                    if (usercodeinputs[3] is ICollection || usercodeinputs[3] is IEnumerable)
                    {
                        ArrayList ui = PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[3]) as ArrayList;
                        for (int ti = 0; ti < ui.Count; ti++)
                        {
                            groupo.Children.Add(ui[ti] as Transform3D);
                        }
                    }//or just one transform
                    else
                    {
                        groupo.Children.Add(usercodeinputs[3] as Transform3D);
                    }
                }
                Color colo = (Color)(usercodeinputs[2]);
                corigin.Offset(vdiag.X / 2, vdiag.Y / 2, vdiag.Z / 2);
                meshBuilder.AddBox(corigin, vdiag.X, vdiag.Y, vdiag.Z);
                var mesh = meshBuilder.ToMesh(true);
                GeometryModel3D newModel = new GeometryModel3D();
                newModel.Geometry = mesh;

                newModel.Material = new DiffuseMaterial(new SolidColorBrush(colo));
                newModel.Transform = groupo;
                ModelVisual3D model = new ModelVisual3D();
                model.Content = newModel;



                usercodeoutputs[0] = model;
            }
            catch
            {
                usercodeoutputs[0] = new ModelVisual3D();
            }




        }
    }
    /// <summary>
    /// Renders a point in 3D space
    /// </summary>
    public class PUPPIPoint3DObject : PUPPIModule
    {
        public PUPPIPoint3DObject()
            : base()
        {
            name = "3D Point Object";
            outputs.Add("not rendered");
            description = "Renders a point in 3D space.";
            outputnames.Add("Point Object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Point3D");

        }
        public override void process_usercode()
        {

            try
            {
                Point3D p = (Point3D)usercodeinputs[0];
                PointsVisual3D pv = new PointsVisual3D();
                Point3DCollection pc = new Point3DCollection();
                pc.Add(p);
                pv.Points = pc;
                pv.Size = 3;
                pv.Color = Color.FromRgb(0, 0, 0);
                usercodeoutputs[0] = pv;


            }
            catch
            {
                usercodeoutputs[0] = "error";
            }
        }
    }

    /// <summary>
    /// will load a 3D model from a file. Supported file formats: .3ds .obj .lwo .stl .off
    /// </summary>
    public class PUPPICAD_File_Load : PUPPIModule
    {
        private static readonly PropertyInfo Visual3DModelPropertyInfo = typeof(Visual3D).GetProperty("Visual3DModel", BindingFlags.Instance | BindingFlags.NonPublic);

        public PUPPICAD_File_Load()
            : base()
        {
            name = "CAD File";
            outputs.Add(new ModelVisual3D());
            description = "Loads 3D model from file .Supported file formats: .3ds .obj .lwo .stl .off";
            outputnames.Add("3D Object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("File Path");
            //also any transofrms
            PUPPIInParameter ti = new PUPPIInParameter();
            ti.isoptional = true;
            inputs.Add(ti);
            inputnames.Add("Transf./List");
        }
        public override void process_usercode()
        {

            try
            {
                string fp = usercodeinputs[0] as string;
                FileModelVisual3D fv = new FileModelVisual3D();
                fv.Source = fp;
                ModelVisual3D gg = new ModelVisual3D();
                if (fv.Source != null)
                {

                    Model3DGroup ga = Visual3DModelPropertyInfo.GetValue(fv, null) as Model3DGroup;
                    foreach (Model3D m in ga.Children)
                    {

                        ModelVisual3D g = new ModelVisual3D();
                        //default material
                        GeometryModel3D mg = m as GeometryModel3D;

                        if (mg.Material == null)
                        {
                            mg.Material = new DiffuseMaterial(Brushes.White);
                            g.Content = mg;
                        }
                        else
                        {
                            g.Content = m;
                        }


                        gg.Children.Add(g);
                    }

                }
                //if a list of transforms is presented
                Transform3DGroup groupo = new Transform3DGroup();
                if (usercodeinputs[1] != null)
                {
                    if (usercodeinputs[1] is ICollection || usercodeinputs[1] is IEnumerable)
                    {
                        ArrayList ui = PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[1]) as ArrayList;
                        for (int ti = 0; ti < ui.Count; ti++)
                        {
                            Transform3D lao = (ui[ti] as Transform3D).Clone();
                            groupo.Children.Add(lao);
                        }
                    }//or just one transform
                    else
                    {
                        Transform3D palao = (usercodeinputs[1] as Transform3D).Clone();
                        groupo.Children.Add(palao);
                    }
                }
                if (fv.Transform != null)
                {
                    gg.Transform = HelperClasses.helperfunctions.transformCombiner(fv.Transform, groupo);
                }
                else
                {
                    gg.Transform = groupo;
                }

                usercodeoutputs[0] = gg;
            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = "Error: " + exy.ToString();
            }

        }
    }

    /// <summary>
    /// will create a sphere by specifying a point for center and a number for radius.
    ///optional transform
    /// </summary>
    public class PUPPISphere_Point_Rad : PUPPIModule
    {
        public PUPPISphere_Point_Rad()
            : base()
        {
            name = "Sphere";
            outputs.Add(new ModelVisual3D());
            description = "Generates a renderable sphere";
            outputnames.Add("3D Object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Center(Pt3D)");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Radius");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Color");
            //also any transofrms
            PUPPIInParameter ti = new PUPPIInParameter();
            ti.isoptional = true;
            inputs.Add(ti);
            inputnames.Add("Transf./List");
        }
        public override void process_usercode()
        {

            try
            {
                var meshBuilder = new MeshBuilder(false, false);
                Point3D corigin = (Point3D)(usercodeinputs[0]);
                double radius = Convert.ToDouble(usercodeinputs[1]);
                //if a list of transforms is presented
                Transform3DGroup groupo = new Transform3DGroup();
                if (usercodeinputs[3] != null)
                {
                    if (usercodeinputs[3] is ICollection || usercodeinputs[3] is IEnumerable)
                    {
                        ArrayList ui = PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[3]) as ArrayList;
                        for (int ti = 0; ti < ui.Count; ti++)
                        {
                            groupo.Children.Add(ui[ti] as Transform3D);
                        }
                    }//or just one transform
                    else
                    {
                        groupo.Children.Add(usercodeinputs[3] as Transform3D);
                    }
                }
                Color colo = (Color)usercodeinputs[2];

                meshBuilder.AddSphere(corigin, radius);
                var mesh = meshBuilder.ToMesh(true);
                GeometryModel3D newModel = new GeometryModel3D();
                newModel.Geometry = mesh;

                newModel.Material = new DiffuseMaterial(new SolidColorBrush(colo));
                newModel.BackMaterial = new DiffuseMaterial(new SolidColorBrush(colo));
                newModel.Transform = groupo;
                ModelVisual3D model = new ModelVisual3D();
                model.Content = newModel;



                usercodeoutputs[0] = model;
            }
            catch
            {
                usercodeoutputs[0] = new ModelVisual3D();
            }




        }
    }
    /// <summary>
    /// will create a cylinder by specifying a point for base center, a vector for axis, and a number for radius.
    ///optional transform
    /// </summary>
    public class PUPPICyl_Point_VectorAxis_Rad : PUPPIModule
    {
        public PUPPICyl_Point_VectorAxis_Rad()
            : base()
        {
            name = "Cylinder";
            outputs.Add(new ModelVisual3D());
            description = "Generates a renderable cylinder";
            outputnames.Add("3D Object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Orig(Pt3D)");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Axis(V3D)");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Radius");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Color");
            //also any transofrms
            PUPPIInParameter ti = new PUPPIInParameter();
            ti.isoptional = true;
            inputs.Add(ti);
            inputnames.Add("Transf./List");

        }
        public override void process_usercode()
        {

            try
            {
                var meshBuilder = new MeshBuilder(false, false);
                Point3D corigin = (Point3D)(usercodeinputs[0]);
                Vector3D vdiag = (Vector3D)(usercodeinputs[1]);
                //for now, user does conversion
                double radius = Convert.ToDouble(usercodeinputs[2]);
                //if a list of transforms is presented
                Transform3DGroup groupo = new Transform3DGroup();
                if (usercodeinputs[4] != null)
                {
                    if (usercodeinputs[4] is ICollection || usercodeinputs[4] is IEnumerable)
                    {
                        ArrayList ui = PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[4]) as ArrayList;
                        for (int ti = 0; ti < ui.Count; ti++)
                        {
                            groupo.Children.Add(ui[ti] as Transform3D);
                        }
                    }//or just one transform
                    else
                    {
                        groupo.Children.Add(usercodeinputs[4] as Transform3D);
                    }
                }
                Color colo = (Color)usercodeinputs[3];
                Point3D endpoint = new Point3D(corigin.X, corigin.Y, corigin.Z);
                endpoint.Offset(vdiag.X, vdiag.Y, vdiag.Z);

                meshBuilder.AddPipe(corigin, endpoint, radius * 2, 0, 16);
                var mesh = meshBuilder.ToMesh(true);
                GeometryModel3D newModel = new GeometryModel3D();
                newModel.Geometry = mesh;

                newModel.Material = new DiffuseMaterial(new SolidColorBrush(colo));
                newModel.Transform = groupo;
                ModelVisual3D model = new ModelVisual3D();
                model.Content = newModel;



                usercodeoutputs[0] = model;
            }
            catch
            {
                usercodeoutputs[0] = new ModelVisual3D();
            }




        }
    }


    /// <summary>
    /// will create a truncated cone by specifying a point for base center, a vector for axis, and the two radii.
    ///optional transform
    /// </summary>
    public class PUPPITruncone_Point_VectorAxis_Rad : PUPPIModule
    {
        public PUPPITruncone_Point_VectorAxis_Rad()
            : base()
        {
            name = "(Truncated)Cone";
            outputs.Add(new ModelVisual3D());
            description = "Generates a renderable truncated cone. Set top radius to 0 to make it not truncated";
            outputnames.Add("3D Object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Orig(Pt3D)");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Axis(V3D)");

            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Base Rad.");


            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Top Rad.");

            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Color");
            //also any transofrms
            PUPPIInParameter ti = new PUPPIInParameter();
            ti.isoptional = true;
            inputs.Add(ti);
            inputnames.Add("Transf./List");

        }
        public override void process_usercode()
        {

            try
            {
                var meshBuilder = new MeshBuilder(false, false);
                Point3D corigin = (Point3D)(usercodeinputs[0]);
                Vector3D vdiag = (Vector3D)(usercodeinputs[1]);
                //for now, user does conversion
                double radius1 = Convert.ToDouble(usercodeinputs[2]);
                double radius2 = Convert.ToDouble(usercodeinputs[3]);
                //if a list of transforms is presented
                Transform3DGroup groupo = new Transform3DGroup();
                if (usercodeinputs[5] != null)
                {
                    if (usercodeinputs[5] is ICollection || usercodeinputs[5] is IEnumerable)
                    {
                        ArrayList ui = PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[5]) as ArrayList;
                        for (int ti = 0; ti < ui.Count; ti++)
                        {
                            groupo.Children.Add(ui[ti] as Transform3D);
                        }
                    }//or just one transform
                    else
                    {
                        groupo.Children.Add(usercodeinputs[5] as Transform3D);
                    }
                }
                Color colo = (Color)usercodeinputs[4];
                Point3D endpoint = new Point3D(corigin.X, corigin.Y, corigin.Z);
                endpoint.Offset(vdiag.X, vdiag.Y, vdiag.Z);

                double height = vdiag.Length;
                if (height == 0)
                {
                    usercodeoutputs[0] = "invalid height";
                    return;
                }

                if (radius1 <= 0)
                {
                    usercodeoutputs[0] = "invalid base radius";
                    return;
                }
                if (radius2 <= 0)
                {
                    meshBuilder.AddCone(corigin, endpoint, radius1, true, 16);
                }
                else
                {
                    meshBuilder.AddCone(corigin, vdiag, radius1, radius2, height, true, true, 16);
                }


                //   meshBuilder.AddPipe(corigin, endpoint, radius * 2, 0, 16);
                var mesh = meshBuilder.ToMesh(true);
                GeometryModel3D newModel = new GeometryModel3D();
                newModel.Geometry = mesh;

                newModel.Material = new DiffuseMaterial(new SolidColorBrush(colo));
                newModel.Transform = groupo;
                ModelVisual3D model = new ModelVisual3D();
                model.Content = newModel;



                usercodeoutputs[0] = model;
            }
            catch
            {
                usercodeoutputs[0] = new ModelVisual3D();
            }




        }
    }


    /// <summary>
    /// Generates a filled polygon from PUPPIPolyline3D or PUPPIPolylineObject points which should be on the same plane
    /// </summary>
    public class PUPPIFillPolyline : PUPPIModule
    {
        public PUPPIFillPolyline()
            : base()
        {
            name = "Fill Polyline";
            description = "Generates a filled polygon from PUPPIPolyline3D or PUPPIPolylineObject points which should be on the same plane.Polyline automatically closed.";
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("3D Polyline");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Color");
            outputs.Add(null);
            outputnames.Add("ModelVisual3D");
        }

        public override void process_usercode()
        {

            List<Point3D> mypoints = new List<Point3D>();
            if (usercodeinputs[0] is HelperClasses.PUPPIPolyLine3D)
            {
                HelperClasses.PUPPIPolyLine3D myp = usercodeinputs[0] as HelperClasses.PUPPIPolyLine3D;
                mypoints = new List<Point3D>(myp.points3dlist);
            }
            else if (usercodeinputs[0].GetType() == typeof(LinesVisual3D))
            {

                LinesVisual3D lv = usercodeinputs[0] as LinesVisual3D;

                mypoints = new List<Point3D>(lv.Points);
            }
            else
            {
                usercodeoutputs[0] = "Invalid input";
                return;
            }
            HelperClasses.helperfunctions.pointsList3dPrune(mypoints, PUPPIGUISettings.geomWeldToler);
            if (mypoints.Count < 3)
            {
                usercodeoutputs[0] = "Nor enough points";
                return;
            }
            Point3D cento = HelperClasses.utilities.getPointsCentroid(mypoints);
            Vector3D mynor = HelperClasses.utilities.pointsNormal(cento, mypoints[0], mypoints[1]);
            if (mynor.Length == 0)
            {
                mynor = new Vector3D(0, 0, 1);
            }


            MeshBuilder meshBuilder = new MeshBuilder(false, false);

            List<int> indices = new List<int>();
            for (int i = 0; i < mypoints.Count; i++)
            {
                meshBuilder.AddNode(mypoints[i], mynor, new Point(0, 0));
                indices.Add(i);
            }


            meshBuilder.AddPolygonByCuttingEars(indices);
            //  meshBuilder.AddPolygon(mypoints);


            var mesh = meshBuilder.ToMesh(true);

            GeometryModel3D polygonsModel = new GeometryModel3D();

            polygonsModel.Geometry = mesh;


            Color colo = (Color)usercodeinputs[1];


            polygonsModel.Material = new DiffuseMaterial(new SolidColorBrush(colo));
            //back material so we don't worry about normals for now
            polygonsModel.BackMaterial = new DiffuseMaterial(new SolidColorBrush(colo));
            ModelVisual3D model = new ModelVisual3D();
            model.Content = polygonsModel;

            usercodeoutputs[0] = model;
        }
    }


    /// <summary>
    ///  Generates a surface from PUPPIPolyline3D or PUPPIPolylineObject.Polyline automatically closed.
    /// </summary>
    public class PUPPISurfaceCap : PUPPIModule
    {
        public PUPPISurfaceCap()
            : base()
        {
            name = "Surface Cap";
            description = "Generates a rounded cap surface from PUPPIPolyline3D or PUPPIPolylineObject/Sketch Object.Polyline automatically closed.";
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("3D Polyline");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Tip Vector3D");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Number Segments");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Color");


            outputs.Add(null);
            outputnames.Add("ModelVisual3D");
        }

        public override void process_usercode()
        {
            List<Point3D> mypoints = new List<Point3D>();
            if (usercodeinputs[0] is HelperClasses.PUPPIPolyLine3D)
            {
                HelperClasses.PUPPIPolyLine3D myp = usercodeinputs[0] as HelperClasses.PUPPIPolyLine3D;
                mypoints = new List<Point3D>(myp.points3dlist);
            }
            else if (usercodeinputs[0].GetType() == typeof(LinesVisual3D))
            {

                LinesVisual3D lv = usercodeinputs[0] as LinesVisual3D;

                mypoints = new List<Point3D>(lv.Points);
            }
            else
            {
                usercodeoutputs[0] = "Invalid Polyline input";
                return;
            }
            HelperClasses.helperfunctions.pointsList3dPrune(mypoints, PUPPIGUISettings.geomWeldToler);
            if (mypoints.Count < 3)
            {
                usercodeoutputs[0] = "Not enough points";
                return;

            }
            Vector3D dirvec = new Vector3D();
            if (usercodeinputs[1] is Vector3D)
            {
                dirvec = (Vector3D)usercodeinputs[1];
            }
            else
            {
                usercodeoutputs[0] = "Invalid Vector3D input";
                return;
            }
            int ns = 0;
            ns = Convert.ToInt16(usercodeinputs[2]);
            if (ns < 1)
            {
                usercodeoutputs[0] = "Invalid number segments";
                return;
            }

            MeshBuilder meshBuilder = new MeshBuilder(false, false);

            List<Point3D> prevPoints = new List<Point3D>();
            Point3D baseCentroid = HelperClasses.helperfunctions.getCentroid(mypoints);
            Point3D tip = baseCentroid + dirvec;
            for (int i = 1; i <= ns; i++)
            {

                if (i == 1) prevPoints = new List<Point3D>(mypoints);
                if (i == ns)
                {

                    for (int j = 0; j < prevPoints.Count - 1; j++)
                    {
                        meshBuilder.AddTriangle(prevPoints[j], prevPoints[j + 1], tip);
                    }
                    meshBuilder.AddTriangle(prevPoints[prevPoints.Count - 1], prevPoints[0], tip);
                }
                else
                {
                    double myratio = (double)i / (double)ns * (double)i / (double)ns;
                    Vector3D dispStage = dirvec * (double)i / (double)ns;
                    List<Point3D> newPoints = new List<Point3D>();
                    for (int j = 0; j < mypoints.Count; j++)
                    {
                        Point3D plao = baseCentroid + (mypoints[j] - baseCentroid) * (1 - myratio) + dispStage;
                        newPoints.Add(plao);
                    }
                    for (int j = 0; j < newPoints.Count - 1; j++)
                    {
                        meshBuilder.AddTriangle(prevPoints[j], prevPoints[j + 1], newPoints[j + 1]);
                        meshBuilder.AddTriangle(prevPoints[j], newPoints[j + 1], newPoints[j]);

                    }
                    meshBuilder.AddTriangle(prevPoints[prevPoints.Count - 1], prevPoints[0], newPoints[0]);
                    meshBuilder.AddTriangle(prevPoints[prevPoints.Count - 1], newPoints[0], newPoints[prevPoints.Count - 1]);
                    prevPoints = new List<Point3D>(newPoints);
                }
            }



            var mesh = meshBuilder.ToMesh(true);
            // mesh.
            ////check normals
            // for (int i=0;i<mesh.Positions.Count;i++ )
            // {
            //     Vector3D bc = baseCentroid - mesh.Positions[i];
            //     Vector3D pn = mesh.Normals[i];
            //     if (Vector3D.AngleBetween(pn,bc)>=0 && Vector3D.AngleBetween(pn,bc)<180   )
            //     {
            //         pn.Negate();
            //         mesh.Normals[i] = pn; 
            //     }
            // }

            GeometryModel3D polygonsModel = new GeometryModel3D();

            polygonsModel.Geometry = mesh;


            Color colo = (Color)usercodeinputs[3];


            polygonsModel.Material = new DiffuseMaterial(new SolidColorBrush(colo));
            //back material so we don't worry about normals for now
            polygonsModel.BackMaterial = new DiffuseMaterial(new SolidColorBrush(colo));
            ModelVisual3D model = new ModelVisual3D();
            model.Content = polygonsModel;

            usercodeoutputs[0] = model;
        }
    }


    /// <summary>
    /// will create a closed loft from a collection of two or more PUPPI 3D Sketches (PUPPISketchObject) or 3DPolylineObjects. Loft closed and capped automatically.
    ///optional transform
    /// </summary>
    public class PUPPILoft : PUPPIModule
    {
        public PUPPILoft()
            : base()
        {
            name = "Closed Loft";
            outputs.Add(new ModelVisual3D());
            description = "Generates a renderable closed / capped loft object from a list of 3DSketch or planar 3DPolyline objects. A transform or list of transforms can also be applied";
            outputnames.Add("Loft");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("3DSktch/Poly Obj Lst");

            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Color");
            //also any transofrms
            PUPPIInParameter ti = new PUPPIInParameter();
            ti.isoptional = true;
            inputs.Add(ti);
            inputnames.Add("Transf./List");
        }
        public override void process_usercode()
        {


            try
            {


                ArrayList sketches = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[0]);
                if (sketches.Count == 0)
                {
                    usercodeoutputs[0] = "no sections";
                    return;
                }
                //if a list of transforms is presented
                Transform3DGroup groupo = new Transform3DGroup();
                if (usercodeinputs[2] != null)
                {
                    if (usercodeinputs[2] is ICollection || usercodeinputs[2] is IEnumerable)
                    {
                        ArrayList ui = PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[2]) as ArrayList;
                        for (int ti = 0; ti < ui.Count; ti++)
                        {

                            groupo.Children.Add(ui[ti] as Transform3D);
                        }
                    }//or just one transform
                    else
                    {
                        groupo.Children.Add(usercodeinputs[2] as Transform3D);
                    }
                }
                Color colo = (Color)(usercodeinputs[1]);
                List<Point3DCollection> plist = new List<Point3DCollection>();
                //start and end caps
                List<Point3D> startpoints = new List<Point3D>();
                List<Point3D> endpoints = new List<Point3D>();
                //allpoints to get bbox
                List<Point3D> allpoints = new List<Point3D>();
                int ii = 0;
                //works with PUPPIPolyline3D also
                if (sketches[0].GetType() == typeof(LinesVisual3D))
                {

                    foreach (LinesVisual3D lv in sketches)
                    {

                        List<Point3D> curpoints = lv.Points as List<Point3D>;
                        if (ii == 0) startpoints.AddRange(curpoints);
                        if (ii == sketches.Count - 1) endpoints.AddRange(curpoints);
                        allpoints.AddRange(curpoints);
                        //Point3DCollectionConverter cc = new Point3DCollectionConverter();
                        Point3DCollection pcl = new Point3DCollection(curpoints);
                        //to close, make sure start and end same
                        if (pcl[0].DistanceTo(pcl[pcl.Count - 1]) > PUPPIGUISettings.geomWeldToler)
                            pcl.Add(new Point3D(pcl[0].X, pcl[0].Y, pcl[0].Z));
                        plist.Add(pcl);
                        ii++;
                    }
                }
                else if (sketches[0].GetType() == typeof(HelperClasses.PUPPIPolyLine3D))
                {
                    foreach (HelperClasses.PUPPIPolyLine3D pao in sketches)
                    {
                        List<Point3D> curpoints = pao.points3dlist as List<Point3D>;
                        if (ii == 0) startpoints.AddRange(curpoints);
                        if (ii == sketches.Count - 1) endpoints.AddRange(curpoints);
                        allpoints.AddRange(curpoints);
                        //Point3DCollectionConverter cc = new Point3DCollectionConverter();
                        Point3DCollection pcl = new Point3DCollection(curpoints);
                        //to close, make sure start and end same
                        if (pcl[0].DistanceTo(pcl[pcl.Count - 1]) > PUPPIGUISettings.geomWeldToler)
                            pcl.Add(new Point3D(pcl[0].X, pcl[0].Y, pcl[0].Z));
                        plist.Add(pcl);
                        ii++;
                    }
                }
                else
                {
                    usercodeoutputs[0] = "Invalid data";
                    return;
                }
                //test same number points
                bool needsRefactoring = false;
                int maxPts = 0;
                foreach (Point3DCollection pll in plist)
                {
                    if (maxPts == 0) maxPts = pll.Count;
                    if (maxPts != pll.Count) needsRefactoring = true;
                    if (pll.Count > maxPts) maxPts = pll.Count;
                }
                if (needsRefactoring)
                {
                    for (int pi = 0; pi < plist.Count; pi++)
                    {
                        Point3DCollection remakeme = plist[pi];
                        if (remakeme.Count != maxPts)
                        {
                            List<Point3D> mpts = remakeme.ToList<Point3D>();
                            HelperClasses.PUPPISpline3D ps = new HelperClasses.PUPPISpline3D(mpts);
                            List<Point3D> newpoints = ps.convertToPoints(maxPts);
                            plist[pi] = new Point3DCollection(newpoints);
                        }
                    }
                }


                List<Vector3DCollection> vlist = new List<Vector3DCollection>();

                List<PointCollection> poilist = new List<PointCollection>();

                MeshGeometry3D meshme = new MeshGeometry3D();
                //meshme.TextureCoordinates.Add(new Point(0, 1));
                //meshme.TextureCoordinates.Add(new Point(1, 1));
                //meshme.TextureCoordinates.Add(new Point(0, 0));
                //meshme.TextureCoordinates.Add(new Point(1, 0));

                HelperClasses.helperfunctions.AddLoft(meshme, plist);

                GeometryModel3D newModel = new GeometryModel3D();
                newModel.Geometry = meshme;



                newModel.Material = new DiffuseMaterial(new SolidColorBrush(colo));
                newModel.BackMaterial = newModel.Material;
                newModel.Transform = groupo;
                ModelVisual3D model = new ModelVisual3D();
                model.Content = newModel;

                //make polygons for start and end points
                //firs, prune
                Rect3D lbb = HelperClasses.helperfunctions.pCloudBBox(allpoints);
                double reftoler = Math.Min(Math.Min(Math.Min(lbb.SizeX, lbb.SizeY), lbb.SizeZ), Math.Min(Math.Min(lbb.SizeX, lbb.SizeY), lbb.SizeZ)) * PUPPIGUISettings.geomWeldToler;
                HelperClasses.helperfunctions.pointsList3dPrune(startpoints, reftoler);
                HelperClasses.helperfunctions.pointsList3dPrune(endpoints, reftoler);
                MeshBuilder meshBuilders = new MeshBuilder(false, false);
                MeshBuilder meshBuildere = new MeshBuilder(false, false);
                //making them separate
                meshBuilders.AddPolygon(startpoints);
                meshBuildere.AddPolygon(endpoints);

                var meshs = meshBuilders.ToMesh(true);
                var meshe = meshBuildere.ToMesh(true);
                GeometryModel3D polygonsModels = new GeometryModel3D();
                GeometryModel3D polygonsModele = new GeometryModel3D();

                polygonsModels.Geometry = meshs;
                polygonsModele.Geometry = meshe;
                polygonsModels.Transform = groupo;
                polygonsModele.Transform = groupo;
                ModelVisual3D pvs = new ModelVisual3D();
                ModelVisual3D pve = new ModelVisual3D();
                pvs.Content = polygonsModels;
                pve.Content = polygonsModele;


                polygonsModels.Material = new DiffuseMaterial(new SolidColorBrush(colo));
                polygonsModele.Material = new DiffuseMaterial(new SolidColorBrush(colo));
                //back material so we don't worry about normals for now
                polygonsModels.BackMaterial = newModel.Material;
                polygonsModele.BackMaterial = newModel.Material;
                model.Children.Add(pvs);
                model.Children.Add(pve);
                //  model.Transform = groupo; 

                usercodeoutputs[0] = model;
            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = "error";

            }





        }
    }
    /// <summary>
    /// Returns the bounding box of a list of Point3D objects as values for start and size on 3 axes
    /// </summary>
    public class PUPPIPointCloudBB : PUPPIModule
    {

        public PUPPIPointCloudBB()
            : base()
        {
            name = "Point Cloud BB";
            description = "Returns the bounding box of a list of Point3D objects as values for start and size on 3 axes";
            outputs.Add(0);
            outputnames.Add("XStart");
            outputs.Add(0);
            outputnames.Add("YStart");
            outputs.Add(0);
            outputnames.Add("ZStart");

            outputs.Add(0);
            outputnames.Add("XSize");
            outputs.Add(0);
            outputnames.Add("YSize");
            outputs.Add(0);
            outputnames.Add("ZSize");

            inputnames.Add("Point Coll.");
            inputs.Add(new PUPPIInParameter());
        }

        public override void process_usercode()
        {
            if (usercodeinputs[0] is IEnumerable)
            {
                ArrayList mpts = makeCollOrEnumIntoArrayList(usercodeinputs[0]);
                double minZ = Double.MaxValue;
                double maxZ = Double.MinValue;
                double minY = Double.MaxValue;
                double maxY = Double.MinValue;
                double minX = Double.MaxValue;
                double maxX = Double.MinValue;
                for (int i = 0; i < mpts.Count; i++)
                {
                    Point3D thisPt = (Point3D)mpts[i];
                    if (thisPt.X < minX) minX = thisPt.X;
                    if (thisPt.Y < minY) minY = thisPt.Y;
                    if (thisPt.Z < minZ) minZ = thisPt.Z;

                    if (thisPt.X > maxX) maxX = thisPt.X;
                    if (thisPt.Y > maxY) maxY = thisPt.Y;
                    if (thisPt.Z > maxZ) maxZ = thisPt.Z;


                }
                usercodeoutputs[0] = minX;
                usercodeoutputs[1] = minY;
                usercodeoutputs[2] = minZ;

                usercodeoutputs[3] = maxX - minX;
                usercodeoutputs[4] = maxY - minY;
                usercodeoutputs[5] = maxZ - minZ;
            }
            else
            {
                usercodeoutputs[0] = "data not list";
            }
        }
    }


    /// <summary>
    /// will create an open loft from a collection of two or more PUPPI  3DPolylineObjects. 
    ///optional transform
    /// </summary>
    public class PUPPISurfaceLoft : PUPPIModule
    {
        public PUPPISurfaceLoft()
            : base()
        {
            name = "Surface Loft";
            outputs.Add(new ModelVisual3D());
            description = "Generates a surface loft of planar 3DPolyline objects. A transform or list of transforms can also be applied.";
            outputnames.Add("Loft");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("3DPoly Obj Lst");

            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Color");
            //also any transofrms
            PUPPIInParameter ti = new PUPPIInParameter();
            ti.isoptional = true;
            inputs.Add(ti);
            inputnames.Add("Transf./List");
        }
        public override void process_usercode()
        {


            try
            {


                ArrayList sketches = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[0]);
                if (sketches.Count == 0)
                {
                    usercodeoutputs[0] = "no sections";
                    return;
                }
                //if a list of transforms is presented
                Transform3DGroup groupo = new Transform3DGroup();
                if (usercodeinputs[2] != null)
                {
                    if (usercodeinputs[2] is ICollection || usercodeinputs[2] is IEnumerable)
                    {
                        ArrayList ui = PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[2]) as ArrayList;
                        for (int ti = 0; ti < ui.Count; ti++)
                        {

                            groupo.Children.Add(ui[ti] as Transform3D);
                        }
                    }//or just one transform
                    else
                    {
                        groupo.Children.Add(usercodeinputs[2] as Transform3D);
                    }
                }
                Color colo = (Color)(usercodeinputs[1]);
                List<Point3DCollection> plist = new List<Point3DCollection>();

                //allpoints to get bbox
                //  List<Point3D> allpoints = new List<Point3D>();
                int ii = 0;

                //works with PUPPIPolyline3D also
                if (sketches[0].GetType() == typeof(LinesVisual3D))
                {
                    foreach (LinesVisual3D lv in sketches)
                    {

                        List<Point3D> curpoints = lv.Points as List<Point3D>;

                        //  allpoints.AddRange(curpoints);
                        Point3DCollectionConverter cc = new Point3DCollectionConverter();
                        Point3DCollection pcl = new Point3DCollection(curpoints);
                        plist.Add(pcl);
                        ii++;
                    }

                }
                else if (sketches[0].GetType() == typeof(HelperClasses.PUPPIPolyLine3D))
                {
                    foreach (HelperClasses.PUPPIPolyLine3D pao in sketches)
                    {
                        List<Point3D> curpoints = pao.points3dlist as List<Point3D>;
                        //  allpoints.AddRange(curpoints);
                        Point3DCollectionConverter cc = new Point3DCollectionConverter();
                        Point3DCollection pcl = new Point3DCollection(curpoints);
                        plist.Add(pcl);
                        ii++;
                    }
                }
                else
                {
                    usercodeoutputs[0] = "Invalid data";
                    return;
                }


                //test same number points
                bool needsRefactoring = false;
                int maxPts = 0;
                foreach (Point3DCollection pll in plist)
                {
                    if (maxPts == 0) maxPts = pll.Count;
                    if (maxPts != pll.Count) needsRefactoring = true;
                    if (pll.Count > maxPts) maxPts = pll.Count;
                }
                if (needsRefactoring)
                {
                    for (int pi = 0; pi < plist.Count; pi++)
                    {
                        Point3DCollection remakeme = plist[pi];
                        if (remakeme.Count != maxPts)
                        {
                            List<Point3D> mpts = remakeme.ToList<Point3D>();
                            HelperClasses.PUPPISpline3D ps = new HelperClasses.PUPPISpline3D(mpts);
                            List<Point3D> newpoints = ps.convertToPoints(maxPts);
                            plist[pi] = new Point3DCollection(newpoints);
                        }
                    }
                }
                List<Vector3DCollection> vlist = new List<Vector3DCollection>();

                List<PointCollection> poilist = new List<PointCollection>();

                MeshGeometry3D meshme = new MeshGeometry3D();
                HelperClasses.helperfunctions.AddLoft(meshme, plist);

                GeometryModel3D newModel = new GeometryModel3D();
                newModel.Geometry = meshme;

                newModel.Material = new DiffuseMaterial(new SolidColorBrush(colo));
                newModel.BackMaterial = newModel.Material;
                newModel.Transform = groupo;
                ModelVisual3D model = new ModelVisual3D();
                model.Content = newModel;

                ////make polygons for start and end points
                ////firs, prune
                //Rect3D lbb = HelperClasses.helperfunctions.pCloudBBox(allpoints);
                //double reftoler = Math.Min(Math.Min(Math.Min(lbb.SizeX, lbb.SizeY), lbb.SizeZ), Math.Min(Math.Min(lbb.SizeX, lbb.SizeY), lbb.SizeZ)) * PUPPIGUISettings.geomWeldToler;


                usercodeoutputs[0] = model;
            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = "error";

            }





        }
    }


    /// <summary>
    /// will create an extrusion from a PUPPI 2D Sketch (PUPPISketch2D), an origin point and a vector for extrusion axis, capping ends
    ///optional transform
    /// </summary>
    public class PUPPIExtrude_Point_VectorAxis_Shape : PUPPIModule
    {
        public PUPPIExtrude_Point_VectorAxis_Shape()
            : base()
        {
            name = "Capped Extrusion";
            outputs.Add(new ModelVisual3D());
            description = "Generates a renderable capped extrusion based on a 2D sketch, a polyline for path and a vector for sketch X axis.";
            outputnames.Add("3D object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("PUPPIPolyline3D");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("X Axis(V3D)");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("2D Sketch Obj.");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Color");
            //also any transofrms
            PUPPIInParameter ti = new PUPPIInParameter();
            ti.isoptional = true;
            inputs.Add(ti);
            inputnames.Add("Transf./List");
        }
        public override void process_usercode()
        {


            try
            {
                //  <helix:ExtrudedVisual3D Path="1 0 -0.45 1 0 0.45" Section="0 0 0.45 0 0 0.45" Fill="Blue" Visible="{Binding IsChecked, ElementName=ExtrudedVisible}"/>

                HelperClasses.PUPPIPolyLine3D pupo = (HelperClasses.PUPPIPolyLine3D)(usercodeinputs[0]);
                Vector3D xaxis = (Vector3D)(usercodeinputs[1]);
                HelperClasses.PUPPISketch2D sketchy = (HelperClasses.PUPPISketch2D)(usercodeinputs[2]);
                //if a list of transforms is presented
                Transform3DGroup groupo = new Transform3DGroup();
                if (usercodeinputs[4] != null)
                {
                    if (usercodeinputs[4] is ICollection || usercodeinputs[4] is IEnumerable)
                    {
                        ArrayList ui = PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[4]) as ArrayList;
                        for (int ti = 0; ti < ui.Count; ti++)
                        {
                            groupo.Children.Add(ui[ti] as Transform3D);
                        }
                    }//or just one transform
                    else
                    {
                        groupo.Children.Add(usercodeinputs[4] as Transform3D);
                    }
                }
                Color colo = (Color)usercodeinputs[3];



                ExtrudedVisual3D extru = new ExtrudedVisual3D();

                System.Windows.Media.PointCollection collpo = new PointCollection();
                foreach (Point popo in sketchy.points2dlist)
                {
                    collpo.Add(popo);
                }

                extru.Section = collpo;

                extru.Transform = groupo;
                extru.Material = new DiffuseMaterial(new SolidColorBrush(colo));
                extru.IsSectionClosed = true;

                Point3DCollection collpa = new Point3DCollection();
                foreach (Point3D po3 in pupo.points3dlist)
                {
                    collpa.Add(po3);
                }
                extru.Path = collpa;
                extru.IsPathClosed = false;

                extru.Fill = new SolidColorBrush(colo);
                //and now cap the extrusion

                ////create a new coordinate system
                Point3D startplaneor = pupo.points3dlist[0];
                //outside normal
                Vector3D startplaneno = Point3D.Subtract(pupo.points3dlist[1], pupo.points3dlist[0]);
                //go in a bit

                startplaneor.Offset(startplaneno.X * PUPPIGUISettings.geomCapToler, startplaneno.Y * PUPPIGUISettings.geomCapToler, startplaneno.Z * PUPPIGUISettings.geomCapToler);

                //get the x axis projected onto the plane
                Vector3D startplanex = Vector3D.Subtract(xaxis, xaxis.project(startplaneno));
                extru.SectionXAxis = startplanex;// xaxis; 
                MeshGeometry3D mg = extru.Model.Geometry as MeshGeometry3D;
                //for merging points, we use a reference dimension
                //this is the smallest dimension we find in our object
                Rect skbb = sketchy.GetBounds();
                double refdim = Math.Min(Math.Min(Math.Min(mg.Bounds.SizeX, Math.Min(mg.Bounds.SizeY, mg.Bounds.SizeZ)), skbb.Size.Width), skbb.Size.Height);
                double reftoler = PUPPIGUISettings.geomWeldToler;
                if (refdim > 0) { reftoler = refdim * PUPPIGUISettings.geomWeldToler; }


                startplaneno.Normalize();
                List<Point3D> startpoints = MeshGeometryHelper.GetContourSegments(mg as MeshGeometry3D, startplaneor, startplaneno).ToList();
                //eliminate duplicates
                HelperClasses.helperfunctions.pointsList3dPrune(startpoints, reftoler);
                HelperClasses.helperfunctions.reorder3Dpoints(startpoints, startplaneno);
                //get the vertices in original mesh
                List<Point3D> myverts = mg.Positions.ToList<Point3D>();
                //make sure we get vertices from mesh
                HelperClasses.helperfunctions.matchVertices(startpoints, myverts, PUPPIGUISettings.geomCapToler * 1.01);




                //now cap end
                int pc = pupo.points3dlist.Count;
                startplaneor = pupo.points3dlist[pc - 1];
                //outside normal
                startplaneno = Point3D.Subtract(pupo.points3dlist[pc - 2], pupo.points3dlist[pc - 1]);
                //go in a bit

                startplaneor.Offset(startplaneno.X * PUPPIGUISettings.geomCapToler, startplaneno.Y * PUPPIGUISettings.geomCapToler, startplaneno.Z * PUPPIGUISettings.geomCapToler);
                startplaneno.Normalize();



                List<Point3D> endpoints = MeshGeometryHelper.GetContourSegments(mg as MeshGeometry3D, startplaneor, startplaneno).ToList();
                HelperClasses.helperfunctions.pointsList3dPrune(endpoints, reftoler);
                HelperClasses.helperfunctions.reorder3Dpoints(endpoints, startplaneno);
                HelperClasses.helperfunctions.matchVertices(endpoints, myverts, PUPPIGUISettings.geomCapToler * 1.01);




                MeshBuilder meshBuilder = new MeshBuilder(false, false);

                meshBuilder.AddPolygon(startpoints);
                meshBuilder.AddPolygon(endpoints);

                var mesh = meshBuilder.ToMesh(true);
                GeometryModel3D newModel = new GeometryModel3D();
                newModel.Geometry = mesh;

                newModel.Material = new DiffuseMaterial(new SolidColorBrush(colo));
                //back material so we don't worry about normals for now
                newModel.BackMaterial = newModel.Material;
                //    newModel.Transform = groupo;
                ModelVisual3D model = new ModelVisual3D();
                model.Content = newModel;
                extru.Children.Add(model);

                usercodeoutputs[0] = extru;
                //   cgp.Children.Add(extru);  




            }
            catch
            {
                usercodeoutputs[0] = new ModelVisual3D();
            }




        }
    }

    /// <summary>
    /// Supplied a Model3D or ModelVisual3D object and a PUPPIPolyline3D or PUPPIPolyline object, returns a list of intersection points
    /// </summary>
    public class PUPPPolylineModelIntersection : PUPPIModule
    {
        public PUPPPolylineModelIntersection()
            : base()
        {
            name = "Polyline Model Intersect";

            description = "Supplied a Model3D or ModelVisual3D object and a PUPPIPolyline3D or PUPPIPolyline object, returns a list of intersection points";
            inputnames.Add("3D object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("PUPPIPolyline3D");
            inputs.Add(new PUPPIInParameter());
            outputs.Add("not set");
            outputnames.Add("Point3D List");

        }
        public override void process_usercode()
        {

            List<Point3D> iPoints = new List<Point3D>();
            try
            {


                HelperClasses.PUPPIPolyLine3D pupo;
                if (usercodeinputs[1].GetType() == typeof(LinesVisual3D))
                {
                    LinesVisual3D ll = usercodeinputs[1] as LinesVisual3D;
                    pupo = new HelperClasses.PUPPIPolyLine3D(ll.Points);
                }
                else
                {
                    pupo = (HelperClasses.PUPPIPolyLine3D)(usercodeinputs[1]);
                }


                List<GeometryModel3D> mlist = new List<GeometryModel3D>();
                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D mvi = usercodeinputs[0] as ModelVisual3D;
                    //if (mvi.Content != null)
                    //{
                    //    GeometryModel3D ggg = mvi.Content as GeometryModel3D;
                    //    mlist.Add(ggg);
                    //}
                    List<GeometryModel3D> mclist = new List<GeometryModel3D>();
                    mvi.getAllGeometryChildren(mclist);
                    mlist.AddRange(mclist);


                }
                else if (usercodeinputs[0] is Model3D)
                {

                    mlist.Add(usercodeinputs[0] as GeometryModel3D);

                }
                List<Point3D> ppoints = pupo.getPoly3DPoints();
                foreach (GeometryModel3D gao in mlist)
                {
                    MeshGeometry3D me = gao.Geometry as MeshGeometry3D;

                    //triangles
                    for (int trindex = 0; trindex < me.TriangleIndices.Count; trindex += 3)
                    {
                        Point3D p1 = me.Positions[me.TriangleIndices[trindex]];
                        Point3D p2 = me.Positions[me.TriangleIndices[trindex + 1]];
                        Point3D p3 = me.Positions[me.TriangleIndices[trindex + 2]];

                        Vector3D v0 = p3 - p1;
                        Vector3D v1 = p2 - p1;
                        Vector3D v3 = p3 - p2;
                        //to go past degenerate triangles
                        if (v0.Length > PUPPIGUISettings.geomWeldToler && v1.Length > PUPPIGUISettings.geomWeldToler && v3.Length > PUPPIGUISettings.geomWeldToler)
                        {




                            try
                            {
                                //barycentric method
                                HelperClasses.PUPPIPlane3D pplane = new HelperClasses.PUPPIPlane3D(p1, p2, p3);
                                //first plane intersection
                                List<Point3D> planepoints = pplane.intersectionPoints(pupo);
                                //now triangle intersection
                                for (int pindex = 0; pindex < planepoints.Count; pindex++)
                                {
                                    Vector3D v2 = planepoints[pindex] - p1;
                                    double dot00 = Vector3D.DotProduct(v0, v0);
                                    double dot01 = Vector3D.DotProduct(v0, v1);
                                    double dot02 = Vector3D.DotProduct(v0, v2);
                                    double dot11 = Vector3D.DotProduct(v1, v1);
                                    double dot12 = Vector3D.DotProduct(v1, v2);

                                    double invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
                                    double u = (dot11 * dot02 - dot01 * dot12) * invDenom;
                                    double v = (dot00 * dot12 - dot01 * dot02) * invDenom;


                                    if ((u >= 0) && (v >= 0) && (u + v < 1))
                                    {
                                        iPoints.Add(planepoints[pindex]);
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                }




            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = exy.ToString();
                return;
            }

            HelperClasses.helperfunctions.pointsList3dPrune(iPoints, PUPPIGUISettings.geomWeldToler);
            usercodeoutputs[0] = iPoints;

        }
    }

    /// <summary>
    /// Supplied a Model3D or ModelVisual3D object, returns a wireframe representation
    /// </summary>
    public class PUPPIWireframeRepresentation : PUPPIModule
    {
        struct pointPair
        {
            public Point3D p1;
            public Point3D p2;
        }
        public PUPPIWireframeRepresentation()
            : base()
        {
            name = "Wireframe Display";

            description = "Supplied a Model3D or ModelVisual3D object, returns a wireframe representation";
            inputnames.Add("3D object");
            inputs.Add(new PUPPIInParameter());
            outputs.Add("not set");
            outputnames.Add("Wireframe");

        }
        public override void process_usercode()
        {


            try
            {


                Transform3D maintransform = null;
                List<Transform3D> childtransforms = new List<Transform3D>();

                List<GeometryModel3D> mlist = new List<GeometryModel3D>();
                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D mvi = usercodeinputs[0] as ModelVisual3D;
                    maintransform = mvi.Transform;
                    List<GeometryModel3D> mclist = new List<GeometryModel3D>();
                    mvi.getAllGeometryChildren(mclist);
                    mlist.AddRange(mclist);
                    foreach (GeometryModel3D gm in mclist)
                    {
                        childtransforms.Add(gm.Transform);
                    }

                }
                else if (usercodeinputs[0] is Model3D)
                {
                    maintransform = Transform3D.Identity;
                    mlist.Add(usercodeinputs[0] as GeometryModel3D);
                    childtransforms.Add((usercodeinputs[0] as GeometryModel3D).Transform);

                }
                if (mlist.Count == 0) { usercodeoutputs[0] = "no model"; return; }
                ModelVisual3D mvv = new ModelVisual3D();
                List<pointPair> palo = new List<pointPair>();
                foreach (GeometryModel3D gao in mlist)
                {
                    MeshGeometry3D me = gao.Geometry as MeshGeometry3D;

                    //triangles
                    for (int trindex = 0; trindex < me.TriangleIndices.Count; trindex += 3)
                    {
                        Point3D p1 = me.Positions[me.TriangleIndices[trindex]];
                        Point3D p2 = me.Positions[me.TriangleIndices[trindex + 1]];
                        Point3D p3 = me.Positions[me.TriangleIndices[trindex + 2]];

                        Vector3D v0 = p3 - p1;
                        Vector3D v1 = p2 - p1;
                        Vector3D v3 = p3 - p2;
                        //to go past degenerate triangles
                        if (v0.Length > PUPPIGUISettings.geomWeldToler && v1.Length > PUPPIGUISettings.geomWeldToler && v3.Length > PUPPIGUISettings.geomWeldToler)
                        {




                            try
                            {
                                pointPair pao = new pointPair();
                                bool foundpao = false;
                                LinesVisual3D llv = new LinesVisual3D();
                                foreach (pointPair palao in palo)
                                {
                                    if ((palao.p1.DistanceTo(p1) < PUPPIGUISettings.geomWeldToler && (palao.p2.DistanceTo(p2) < PUPPIGUISettings.geomWeldToler)) || (palao.p2.DistanceTo(p1) < PUPPIGUISettings.geomWeldToler && (palao.p1.DistanceTo(p2) < PUPPIGUISettings.geomWeldToler)))
                                    {
                                        foundpao = true;
                                        break;
                                    }
                                }
                                if (!foundpao)
                                {
                                    llv = new LinesVisual3D();
                                    llv.Color = Colors.Black;
                                    llv.Points.Add(p1);
                                    llv.Points.Add(p2);
                                    mvv.Children.Add(llv);
                                    pao = new pointPair();
                                    pao.p1 = p1;
                                    pao.p2 = p2;
                                    palo.Add(pao);
                                }

                                foundpao = false;
                                foreach (pointPair palao in palo)
                                {
                                    if ((palao.p1.DistanceTo(p2) < PUPPIGUISettings.geomWeldToler && (palao.p2.DistanceTo(p3) < PUPPIGUISettings.geomWeldToler)) || (palao.p2.DistanceTo(p2) < PUPPIGUISettings.geomWeldToler && (palao.p1.DistanceTo(p3) < PUPPIGUISettings.geomWeldToler)))
                                    {
                                        foundpao = true;
                                        break;
                                    }
                                }
                                if (!foundpao)
                                {
                                    llv = new LinesVisual3D();
                                    llv.Points.Add(p2);
                                    llv.Points.Add(p3);
                                    pao = new pointPair();
                                    pao.p1 = p2;
                                    pao.p2 = p3;
                                    palo.Add(pao);
                                    mvv.Children.Add(llv);
                                }

                                foundpao = false;
                                foreach (pointPair palao in palo)
                                {
                                    if ((palao.p1.DistanceTo(p3) < PUPPIGUISettings.geomWeldToler && (palao.p2.DistanceTo(p1) < PUPPIGUISettings.geomWeldToler)) || (palao.p2.DistanceTo(p3) < PUPPIGUISettings.geomWeldToler && (palao.p1.DistanceTo(p1) < PUPPIGUISettings.geomWeldToler)))
                                    {
                                        foundpao = true;
                                        break;
                                    }
                                }

                                if (!foundpao)
                                {
                                    llv = new LinesVisual3D();
                                    llv.Points.Add(p3);
                                    llv.Points.Add(p1);
                                    pao = new pointPair();
                                    pao.p1 = p3;
                                    pao.p2 = p1;
                                    palo.Add(pao);
                                    mvv.Children.Add(llv);
                                }

                            }
                            catch
                            {

                            }
                        }
                    }
                }
                if (maintransform != null)
                    mvv.Transform = maintransform.Clone();
                usercodeoutputs[0] = mvv;




            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = exy.ToString();
                return;
            }



        }
    }


    public class PUPPIProjectPolylineOnModel : PUPPIModule
    {
        public PUPPIProjectPolylineOnModel()
            : base()
        {
            name = "Proj PLine on Model";
            description = "Projects a PolyLine3D or PolyLine3D object on a 3D Model";
            inputnames.Add("3D object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("PLine/Obj");
            inputs.Add(new PUPPIInParameter());
            outputs.Add("not set");
            outputnames.Add("PUPPIPolyline3D");
        }
        public override void process_usercode()
        {

            try
            {



                List<Point3D> mypoints = new List<Point3D>();
                if (usercodeinputs[1] is HelperClasses.PUPPIPolyLine3D)
                {
                    HelperClasses.PUPPIPolyLine3D myp = usercodeinputs[1] as HelperClasses.PUPPIPolyLine3D;
                    mypoints = new List<Point3D>(myp.points3dlist);
                }
                else if (usercodeinputs[1].GetType() == typeof(LinesVisual3D))
                {

                    LinesVisual3D lv = usercodeinputs[1] as LinesVisual3D;

                    mypoints = new List<Point3D>(lv.Points);
                }
                else
                {
                    usercodeoutputs[0] = "Invalid input";
                    return;
                }

                List<GeometryModel3D> mlist = new List<GeometryModel3D>();
                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D mvi = usercodeinputs[0] as ModelVisual3D;

                    List<GeometryModel3D> mclist = new List<GeometryModel3D>();
                    mvi.getAllGeometryChildren(mclist);
                    mlist.AddRange(mclist);


                }
                else if (usercodeinputs[0] is Model3D)
                {

                    mlist.Add(usercodeinputs[0] as GeometryModel3D);

                }

                List<Point3D> projPts = new List<Point3D>();
                foreach (Point3D pl in mypoints)
                {
                    projPts.Add(HelperClasses.utilities.getNearestPointOnModel(pl, mlist));
                }

                usercodeoutputs[0] = new HelperClasses.PUPPIPolyLine3D(projPts);






            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = exy.ToString();
                return;
            }



        }
    }


    /// <summary>
    /// Gets the nearest point on a 3D Object
    /// </summary>
    public class PUPPIModelNearestPoint : PUPPIModule
    {
        public PUPPIModelNearestPoint()
            : base()
        {
            name = "Nearest Point";

            description = "Gets the nearest point on a 3D Object";
            inputnames.Add("3D object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Point3D");
            inputs.Add(new PUPPIInParameter());
            outputs.Add("not set");
            outputnames.Add("Point3D");

        }
        public override void process_usercode()
        {

            try
            {



                Point3D nPt = (Point3D)usercodeinputs[1];

                List<GeometryModel3D> mlist = new List<GeometryModel3D>();
                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D mvi = usercodeinputs[0] as ModelVisual3D;

                    List<GeometryModel3D> mclist = new List<GeometryModel3D>();
                    mvi.getAllGeometryChildren(mclist);
                    mlist.AddRange(mclist);


                }
                else if (usercodeinputs[0] is Model3D)
                {

                    mlist.Add(usercodeinputs[0] as GeometryModel3D);

                }
                ////find closest triangle to point so we don't recalculate all normals
                //int closestModel = -1;
                //int closestPointIndex = -1;

                //int cindex = 0;
                //foreach (GeometryModel3D gao in mlist)
                //{

                //    MeshGeometry3D me = gao.Geometry as MeshGeometry3D;


                //    //triangles
                //    for (int pindex = 0; pindex < me.Positions.Count; pindex++)
                //    {
                //        Point3D p1 = me.Positions[pindex];

                //        if (p1.DistanceTo(nPt) < myDist)
                //        {
                //            myDist = p1.DistanceTo(nPt);
                //            closestModel = cindex;
                //            closestPointIndex = pindex;

                //        }

                //    }
                //    cindex++;
                //}

                usercodeoutputs[0] = HelperClasses.utilities.getNearestPointOnModel(nPt, mlist);

                //if (mydist != Double.MaxValue)
                //{
                //    //GeometryModel3D gao = mlist[closestModel];
                //    //MeshGeometry3D me = gao.Geometry as MeshGeometry3D;
                //    //Point3D pepe = me.Positions[closestPointIndex];
                //    usercodeoutputs[0] = pi;

                //}
                //else
                //{
                //    usercodeoutputs[0] = "no model";
                //}




            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = exy.ToString();
                return;
            }



        }
    }

    public class PUPPIPlanarMesh : PUPPIModule
    {
        //  public List<Point3D> uniquepts; 
        public PUPPIPlanarMesh()
            : base()
        {
            name = "Planar Mesh";
            outputs.Add(new ModelVisual3D());
            outputnames.Add("ModelVisual3D");
            description = "Mesh based on square size and count,X axis and PUPPIPlane3D or Plane3D";
            inputnames.Add("Plane");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("X Axis");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("X Count");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Y Count");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Square Side");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Color");
            inputs.Add(new PUPPIInParameter());

        }
        public override void process_usercode()
        {
            try
            {
                Plane3D paa = null;
                try
                {
                    paa = ((HelperClasses.PUPPIPlane3D)usercodeinputs[0]).theplane;
                }
                catch
                {
                    paa = null;
                }

                if (paa == null)
                {
                    try
                    {
                        paa = (Plane3D)usercodeinputs[0];
                    }
                    catch
                    {
                        paa = null;
                    }
                }
                if (paa == null)
                {
                    usercodeoutputs[0] = "Invalid plane";
                    return;
                }
                Vector3D xaxis = new Vector3D(1, 0, 0);
                try
                {
                    xaxis = (Vector3D)usercodeinputs[1];
                }
                catch
                {
                    usercodeoutputs[0] = "Invalid X Axis";
                    return;
                }

                int xcount = 0;
                try
                {
                    xcount = Convert.ToInt16(usercodeinputs[2]);
                }
                catch
                {
                    usercodeoutputs[0] = "Invalid X Count";
                    return;
                }
                if (xcount <= 0)
                {
                    usercodeoutputs[0] = "Invalid X Count";
                    return;
                }

                int ycount = 0;
                try
                {
                    ycount = Convert.ToInt16(usercodeinputs[3]);
                }
                catch
                {
                    usercodeoutputs[0] = "Invalid Y Count";
                    return;
                }
                if (ycount <= 0)
                {
                    usercodeoutputs[0] = "Invalid Y Count";
                    return;
                }

                double side = 0;
                try
                {
                    side = Convert.ToDouble(usercodeinputs[4]);
                }
                catch
                {
                    usercodeoutputs[0] = "Invalid Square Side";
                    return;
                }
                if (side <= 0)
                {
                    usercodeoutputs[0] = "Invalid Square Side";
                    return;
                }

                Color pcolor;
                try
                {
                    pcolor = (System.Windows.Media.Color)(usercodeinputs[5]);
                }
                catch
                {
                    usercodeoutputs[0] = "Invalid Color";
                    return;
                }







                var pts = new Point3D[xcount + 1, ycount + 1];
                double deltacx = (double)(xcount) * side * 0.5;
                double deltacy = (double)(ycount) * side * 0.5;
                for (int i = 0; i < xcount + 1; i++)
                    for (int j = 0; j < ycount + 1; j++)
                    {
                        Point cp = new Point(i * side - deltacx, j * side - deltacy);

                        pts[i, j] = HelperClasses.helperfunctions.moveskpointtoplane(cp, paa, xaxis);


                    }

                var mb = new MeshBuilder(false, true);
                mb.AddRectangularMesh(pts, null, false, false);
                MeshGeometry3D Mesh = mb.ToMesh();
                GeometryModel3D m = new GeometryModel3D();
                m.Geometry = Mesh;
                m.Material = new DiffuseMaterial(new SolidColorBrush(pcolor));
                m.BackMaterial = new DiffuseMaterial(new SolidColorBrush(pcolor));
                ModelVisual3D mv = new ModelVisual3D();
                mv.Content = m;
                usercodeoutputs[0] = mv;

            }
            catch
            {
                usercodeoutputs[0] = "error";
                //  usercodeoutputs[1] = "error"; 
            }
        }
    }

    /// <summary>
    /// Gets the normal vector of the mesh of the 3D object input nearest to the point model input.
    /// </summary>
    public class PUPPIGetModelNormal : PUPPIModule
    {
        public PUPPIGetModelNormal()
            : base()
        {
            name = "Get Model Normal";

            description = "Gets the normal vector of the mesh of the 3D object input nearest to the point model input.";
            inputnames.Add("3D object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Point3D");
            inputs.Add(new PUPPIInParameter());
            outputs.Add("not set");
            outputnames.Add("Normal Vector3D");

        }
        public override void process_usercode()
        {


            try
            {



                Point3D nPt = (Point3D)usercodeinputs[1];

                List<GeometryModel3D> mlist = new List<GeometryModel3D>();
                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D mvi = usercodeinputs[0] as ModelVisual3D;

                    List<GeometryModel3D> mclist = new List<GeometryModel3D>();
                    mvi.getAllGeometryChildren(mclist);
                    mlist.AddRange(mclist);


                }
                else if (usercodeinputs[0] is Model3D)
                {

                    mlist.Add(usercodeinputs[0] as GeometryModel3D);

                }
                //find closest triangle to point so we don't recalculate all normals
                int closestModel = -1;
                int closestPointIndex = -1;
                double myDist = double.MaxValue;
                int cindex = 0;
                foreach (GeometryModel3D gao in mlist)
                {

                    MeshGeometry3D me = gao.Geometry as MeshGeometry3D;


                    //triangles
                    for (int pindex = 0; pindex < me.Positions.Count; pindex++)
                    {
                        Point3D p1 = me.Positions[pindex];

                        if (p1.DistanceTo(nPt) < myDist)
                        {
                            myDist = p1.DistanceTo(nPt);
                            closestModel = cindex;
                            closestPointIndex = pindex;

                        }

                    }
                    cindex++;
                }

                if (closestModel != -1 && closestPointIndex != -1)
                {
                    GeometryModel3D gao = mlist[closestModel];
                    MeshGeometry3D me = gao.Geometry as MeshGeometry3D;
                    Vector3DCollection ve = MeshGeometryHelper.CalculateNormals(me);
                    Vector3D veve = ve[closestPointIndex];
                    veve.Normalize();
                    usercodeoutputs[0] = veve;

                }
                else
                {
                    usercodeoutputs[0] = "no model";
                }




            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = exy.ToString();
                return;
            }



        }
    }



    /// <summary>
    /// will create an extrusion from a PUPPI 2D Sketch (PUPPISketch2D), an origin point and a vector for extrusion axis. no capping
    ///optional transform
    /// </summary>
    public class PUPPIOpenExtrude_Point_VectorAxis_Shape : PUPPIModule
    {
        public PUPPIOpenExtrude_Point_VectorAxis_Shape()
            : base()
        {
            name = "Uncapped Extrusion";
            outputs.Add(new ModelVisual3D());
            description = "Generates a renderable uncapped  extrusion based on a 2D sketch, a polyline for path and a vector for sketch X axis.";
            outputnames.Add("3D object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("PUPPIPolyline3D");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("X Axis(V3D)");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("2D Sketch Obj.");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Color");
            //also any transofrms
            PUPPIInParameter ti = new PUPPIInParameter();
            ti.isoptional = true;
            inputs.Add(ti);
            inputnames.Add("Transf./List");
            //if path is closed
            PUPPIInParameter pc = new PUPPIInParameter();
            pc.isoptional = true;
            inputs.Add(pc);
            inputnames.Add("Path Closed 1/0");

        }
        public override void process_usercode()
        {


            try
            {
                //  <helix:ExtrudedVisual3D Path="1 0 -0.45 1 0 0.45" Section="0 0 0.45 0 0 0.45" Fill="Blue" Visible="{Binding IsChecked, ElementName=ExtrudedVisible}"/>

                HelperClasses.PUPPIPolyLine3D pupo = (HelperClasses.PUPPIPolyLine3D)(usercodeinputs[0]);
                Vector3D xaxis = (Vector3D)(usercodeinputs[1]);
                HelperClasses.PUPPISketch2D sketchy = (HelperClasses.PUPPISketch2D)(usercodeinputs[2]);
                //if a list of transforms is presented
                Transform3DGroup groupo = new Transform3DGroup();
                if (usercodeinputs[4] != null)
                {
                    if (usercodeinputs[4] is ICollection || usercodeinputs[4] is IEnumerable)
                    {
                        ArrayList ui = PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[4]) as ArrayList;
                        for (int ti = 0; ti < ui.Count; ti++)
                        {
                            groupo.Children.Add(ui[ti] as Transform3D);
                        }
                    }//or just one transform
                    else
                    {
                        groupo.Children.Add(usercodeinputs[4] as Transform3D);
                    }
                }
                Color colo = (Color)usercodeinputs[3];



                ExtrudedVisual3D extru = new ExtrudedVisual3D();

                System.Windows.Media.PointCollection collpo = new PointCollection();
                foreach (Point popo in sketchy.points2dlist)
                {
                    collpo.Add(popo);
                }

                extru.Section = collpo;

                extru.Transform = groupo;
                extru.Material = new DiffuseMaterial(new SolidColorBrush(colo));
                extru.IsSectionClosed = true;

                Point3DCollection collpa = new Point3DCollection();
                foreach (Point3D po3 in pupo.points3dlist)
                {
                    collpa.Add(po3);
                }
                extru.Path = collpa;


                bool pf = false;
                if (usercodeinputs[5] != null)
                {
                    pf = Convert.ToBoolean(usercodeinputs[5]);
                }
                extru.IsPathClosed = pf;

                extru.Fill = new SolidColorBrush(colo);



                usercodeoutputs[0] = extru;





            }
            catch
            {
                usercodeoutputs[0] = new ModelVisual3D();
            }




        }
    }


    public class PUPPICloneAndPlace : PUPPIModule
    {
        public PUPPICloneAndPlace()
            : base()
        {
            name = "Clone Place Object";
            description = "Supplied ModelVisual3D or Model3D and list of Point3D, it returns ModelVisual3D with input object cloned at point positions";
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("3D Object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Point List");
            outputs.Add(null);
            outputnames.Add("Combined Object");
        }
        public override void process_usercode()
        {
            try
            {
                ModelVisual3D mv = new ModelVisual3D();
                if (usercodeinputs[0] is ModelVisual3D)
                {
                    mv = usercodeinputs[0] as ModelVisual3D;


                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D m = usercodeinputs[0] as Model3D;
                    Model3D mao = m.Clone();
                    mv.Content = mao;


                }

                else
                {
                    usercodeoutputs[0] = "Invalid object input type.";
                    return;
                }
                Point3D mc = mv.getBBRecursive().rectcenter();


                ArrayList apt = makeCollOrEnumIntoArrayList(usercodeinputs[1]);
                ModelVisual3D allm = new ModelVisual3D();
                for (int ii = 0; ii < apt.Count; ii++)
                {
                    Point3D nc = (Point3D)apt[ii];

                    allm.Children.Add((HelperClasses.utilities.fullCloneModelVisual3D(mv).translateVisualAndChildren(nc.X - mc.X, nc.Y - mc.Y, nc.Z - mc.Z)));
                }
                allm.addAllChildrenFlatRecursive(new List<ModelVisual3D>());
                usercodeoutputs[0] = allm;
            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = "Processing error: " + exy.ToString(); ;
                return;
            }
        }
    }

    /// <summary>
    /// 3D visual rep of polyline,a collection of 3D points
    /// </summary>
    public class PUPPIPlineObject : PUPPIModule
    {
        public PUPPIPlineObject()
            : base()
        {
            name = "3D Polyline Object ";
            description = "Renderable 3D Polyline taking a PUPPIPolyline3D as input";
            outputs.Add(new ModelVisual3D());
            outputnames.Add("3D object");
            inputnames.Add("PUPPIPolyline3D");
            inputs.Add(new PUPPIInParameter());
        }
        public override void process_usercode()
        {
            try
            {
                HelperClasses.PUPPIPolyLine3D PUPPIpo = (HelperClasses.PUPPIPolyLine3D)usercodeinputs[0];
                LinesVisual3D ll = new LinesVisual3D();
                ll.Color = Colors.Black;
                ll.Points.Clear();
                if (PUPPIpo.points3dlist.Count >= 2)
                {
                    for (int i = 0; i < PUPPIpo.points3dlist.Count - 1; i++)
                    {

                        ll.Points.Add(PUPPIpo.points3dlist[i]);
                        ll.Points.Add(PUPPIpo.points3dlist[i + 1]);

                    }

                    usercodeoutputs[0] = ll;
                }
            }
            catch
            {
                usercodeoutputs[0] = "error";
            }
        }
    }
    /// <summary>
    /// 3D visual rep of a closed sketch,a collection of 3D points.
    /// created with a 2D sketch ,a plane, and an x axis vector
    /// </summary>
    public class PUPPISketchObject : PUPPIModule
    {
        //  public List<Point3D> uniquepts; 
        public PUPPISketchObject()
            : base()
        {
            name = "3D Sketch Object";
            outputs.Add(new ModelVisual3D());
            outputnames.Add("3D Sketch");
            description = "Generates a renderable sketch in 3D coordinates";
            inputnames.Add("2D Sketch");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Plane");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("X axis");
            inputs.Add(new PUPPIInParameter());
            //so we can use points;
            // uniquepts=new List<Point3D>();  
        }
        public override void process_usercode()
        {
            try
            {
                Plane3D paa = null;
                try
                {
                    paa = ((HelperClasses.PUPPIPlane3D)usercodeinputs[1]).theplane;
                }
                catch
                {
                    paa = null;
                }

                if (paa == null)
                {
                    try
                    {
                        paa = (Plane3D)usercodeinputs[1];
                    }
                    catch
                    {
                        paa = null;
                    }
                }
                if (paa == null)
                {
                    usercodeoutputs[0] = "Invalid plane";
                    return;
                }
                HelperClasses.PUPPISketch2D PUPPIpo = (HelperClasses.PUPPISketch2D)(usercodeinputs[0]);
                Vector3D xaxis = (Vector3D)usercodeinputs[2];
                LinesVisual3D ll = new LinesVisual3D();
                //   uniquepts = new List<Point3D>();   
                ll.Color = Colors.Black;
                List<Point3D> s3dplist = new List<Point3D>();
                foreach (Point po in PUPPIpo.points2dlist)
                {
                    Point3D po3d = HelperClasses.helperfunctions.moveskpointtoplane(po, paa, xaxis);
                    ll.Points.Add(po3d);
                    //intermediate points to make lines
                    if (PUPPIpo.points2dlist.IndexOf(po) != 0 && PUPPIpo.points2dlist.IndexOf(po) != PUPPIpo.points2dlist.Count)
                    {
                        ll.Points.Add(po3d);
                    }
                    //  uniquepts.Add(po3d);  
                }
                //to close sketch
                Point3D firstpoint = HelperClasses.helperfunctions.moveskpointtoplane(PUPPIpo.points2dlist[0], paa, xaxis); //new Point3D(PUPPIpo.points2dlist[0].X, PUPPIpo.points2dlist[0].Y, 0);
                ll.Points.Add(firstpoint);

                usercodeoutputs[0] = ll;
                // usercodeoutputs[1] = uniquepts; 
            }
            catch
            {
                usercodeoutputs[0] = "error";
                //  usercodeoutputs[1] = "error"; 
            }
        }
    }


    /// <summary>
    /// 3D visual rep of a a plane based on PUPPIPlane3D or Plane3D
    /// </summary>
    public class PUPPIPlaneObject : PUPPIModule
    {
        //  public List<Point3D> uniquepts; 
        public PUPPIPlaneObject()
            : base()
        {
            name = "Show Plane";
            outputs.Add(new ModelVisual3D());
            outputnames.Add("Plane Representation");
            description = "3D visual rep of a a plane based on PUPPIPlane3D or Plane3D";
            inputnames.Add("Plane");
            inputs.Add(new PUPPIInParameter());

        }
        public override void process_usercode()
        {
            try
            {
                Plane3D paa = null;
                try
                {
                    paa = ((HelperClasses.PUPPIPlane3D)usercodeinputs[0]).theplane;
                }
                catch
                {
                    paa = null;
                }

                if (paa == null)
                {
                    try
                    {
                        paa = (Plane3D)usercodeinputs[0];
                    }
                    catch
                    {
                        paa = null;
                    }
                }
                if (paa == null)
                {
                    usercodeoutputs[0] = "Invalid plane";
                    return;
                }
                List<Point> drawPts = new List<Point>();
                drawPts.Add(new Point(-1, -1));
                drawPts.Add(new Point(1, -1));
                drawPts.Add(new Point(1, 1));
                drawPts.Add(new Point(-1, 1));

                Vector3D xaxis = new Vector3D(1, 0, 0);
                LinesVisual3D ll = new LinesVisual3D();
                //   uniquepts = new List<Point3D>();   
                ll.Color = Colors.Green;
                List<Point3D> s3dplist = new List<Point3D>();
                foreach (Point po in drawPts)
                {
                    Point3D po3d = HelperClasses.helperfunctions.moveskpointtoplane(po, paa, xaxis);
                    ll.Points.Add(po3d);
                    //intermediate points to make lines
                    if (drawPts.IndexOf(po) != 0 && drawPts.IndexOf(po) != drawPts.Count)
                    {
                        ll.Points.Add(po3d);
                    }
                    //  uniquepts.Add(po3d);  
                }
                //to close sketch
                Point3D firstpoint = HelperClasses.helperfunctions.moveskpointtoplane(drawPts[0], paa, xaxis); //new Point3D(PUPPIpo.points2dlist[0].X, PUPPIpo.points2dlist[0].Y, 0);
                ll.Points.Add(firstpoint);

                usercodeoutputs[0] = ll;
                // usercodeoutputs[1] = uniquepts; 
            }
            catch
            {
                usercodeoutputs[0] = "error";
                //  usercodeoutputs[1] = "error"; 
            }
        }
    }


    /// <summary>
    /// Gets a point collection of the mesh positions of a Model3D or ModelVisual3D object. Children ignored.
    /// </summary>
    public class PUPPIModelGetPoints : PUPPIModule
    {
        public PUPPIModelGetPoints()
            : base()
        {
            name = "Get 3D Model Points";
            description = "Returns a list of points of a Model3D or ModelVisual3D object.Children ignored.";
            outputs.Add(new ArrayList());
            outputnames.Add("Points List");

            inputnames.Add("Model");
            inputs.Add(new PUPPIInParameter());

        }
        public override void process_usercode()
        {
            usercodeoutputs[0] = null;
            try
            {
                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D m = usercodeinputs[0] as ModelVisual3D;
                    usercodeoutputs[0] = m.getMy3DVisualModelPositionsAsPoints();
                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D m = usercodeinputs[0] as Model3D;
                    usercodeoutputs[0] = m.getMy3DModelPositionsAsPoints();
                }
            }
            catch
            {
                usercodeoutputs[0] = "error";
            }
        }

    }

    /// <summary>
    /// Gets area of a Model3D or ModelVisual3D object.
    /// </summary>
    public class PUPPIModelGetArea : PUPPIModule
    {
        public PUPPIModelGetArea()
            : base()
        {
            name = "Get 3D Model Area";
            description = "Gets area of a Model3D or ModelVisual3D object.";
            outputs.Add(0);
            outputnames.Add("Area");

            inputnames.Add("Model");
            inputs.Add(new PUPPIInParameter());

        }
        public override void process_usercode()
        {
            usercodeoutputs[0] = null;
            try
            {
                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D m = usercodeinputs[0] as ModelVisual3D;
                    usercodeoutputs[0] = HelperClasses.helperfunctions.getArearecursivelyWT(m, new Transform3DGroup());
                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D m = usercodeinputs[0] as Model3D;
                    ModelVisual3D mv = new ModelVisual3D();
                    mv.Content = m.Clone();
                    usercodeoutputs[0] = HelperClasses.helperfunctions.getArearecursivelyWT(mv, new Transform3DGroup());
                }
            }
            catch
            {
                usercodeoutputs[0] = "error";
            }
        }

    }


    /// <summary>
    /// Converts model and children to STL format and returns list of strings 
    /// </summary>
    public class PUPPI3DObjectToTextSTL : PUPPIModule
    {
        public PUPPI3DObjectToTextSTL()
            : base()
        {
            name = "3D Model To STL Text";
            description = "Converts model and children to STL format and returns list of strings. Useful for sending models between computers. ";
            outputs.Add(null);
            outputnames.Add("String List");

            inputnames.Add("Model");
            inputs.Add(new PUPPIInParameter());

        }
        public override void process_usercode()
        {
            usercodeoutputs[0] = null;
            try
            {
                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D m = usercodeinputs[0] as ModelVisual3D;
                    string name = "noname";
                    try
                    {
                        name = m.GetName();
                        if (name == "" || name == string.Empty) name = "noname";
                    }
                    catch
                    {
                        name = "noname";
                    }
                    ArrayList linesA = new ArrayList();
                    HelperClasses.helperfunctions.writegeomSTLrecursivelyWT(m, linesA, name, new Transform3DGroup());
                    List<string> lines = new List<string>();
                    for (int i = 0; i < linesA.Count; i++)
                    {
                        string line = linesA[i].ToString();
                        lines.Add(line);
                    }
                    usercodeoutputs[0] = lines;
                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D m = usercodeinputs[0] as Model3D;
                    ModelVisual3D mv = new ModelVisual3D();
                    mv.Content = m;
                    ArrayList linesA = new ArrayList();
                    HelperClasses.helperfunctions.writegeomSTLrecursivelyWT(mv, linesA, "noname", new Transform3DGroup());
                    List<string> lines = new List<string>();
                    for (int i = 0; i < linesA.Count; i++)
                    {
                        string line = linesA[i].ToString();
                        lines.Add(line);
                    }
                    usercodeoutputs[0] = lines;
                }
            }
            catch
            {
                usercodeoutputs[0] = "error";
            }
        }

    }
    /// <summary>
    /// Converts STL text supplied as list of lines into 3D model
    /// </summary>

    public class PUPPI3DObjectFromSTLText : PUPPIModel.PUPPIModule
    {
        public PUPPI3DObjectFromSTLText()
            : base()
        {
            name = "3D Model from STL Text";
            description = "Converts STL text supplied as list of lines into 3D model. ";
            outputs.Add(null);
            inputnames.Add("String List");

            outputnames.Add("Model");
            inputs.Add(new PUPPIInParameter());

        }
        public override void process_usercode()
        {
            ArrayList linput = new ArrayList();
            if (usercodeinputs[0] is ICollection)
                linput = new ArrayList(usercodeinputs[0] as ICollection);
            else if (usercodeinputs[0] is IEnumerable)
                linput = PUPPIModule.makeMeAnArrayList(usercodeinputs[0] as IEnumerable);
            var list = linput.Cast<string>().ToList();
            usercodeoutputs[0] = HelperClasses.helperfunctions.readgeomSTLstring(list);

        }
    }

    /// <summary>
    /// Gets the contours of a plane intersecting a model and children as a list of lists of points (one list per child)
    /// </summary>
    public class PUPPIModelGetContours : PUPPIModule
    {
        public PUPPIModelGetContours()
            : base()
        {
            name = "Get 3D Model Contours";
            description = "Finds contours based on a PUPPIPlane or Plane3D, on a Model3D or ModelVisual3D, including children, and returns a list of lists of points.";
            outputs.Add(new ArrayList());
            outputnames.Add("Point3D Lists");
            inputnames.Add("Model");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Plane");
            inputs.Add(new PUPPIInParameter());
        }
        public override void process_usercode()
        {
            Plane3D thePlaner = null;
            if (usercodeinputs[1] is HelperClasses.PUPPIPlane3D)
            {
                thePlaner = (usercodeinputs[1] as HelperClasses.PUPPIPlane3D).theplane;
            }
            else if (usercodeinputs[1] is Plane3D)
            {
                thePlaner = usercodeinputs[1] as Plane3D;
            }

            if (thePlaner == null)
            {
                usercodeoutputs[0] = "Invalid Plane";
                return;

            }
            List<List<Point3D>> cpts = new List<List<Point3D>>();
            if (usercodeinputs[0] is Model3D)
            {
                Model3D mao = usercodeinputs[0] as Model3D;
                GeometryModel3D gao = mao as GeometryModel3D;
                if (gao != null)
                {

                    var segments = MeshGeometryHelper.GetContourSegments(gao.Geometry as MeshGeometry3D, thePlaner.Position, thePlaner.Normal).ToList();
                    //List<Point3D> mycpts = new List<Point3D>();
                    foreach (var contour in MeshGeometryHelper.CombineSegments(segments, PUPPIGUISettings.geomWeldToler).ToList())
                    {
                        if (contour.Count > 2)
                        {
                            List<Point3D> mycpts = new List<Point3D>();
                            mycpts.AddRange(contour);
                            Point3D sp = (Point3D)contour[0];
                            Point3D ep = (Point3D)contour[contour.Count - 1];
                            bool isclosed = false;
                            if (sp.DistanceTo(ep) < PUPPIGUISettings.geomWeldToler)
                            {
                                isclosed = true;
                            }
                            HelperClasses.helperfunctions.pointsList3dPrune(mycpts, PUPPIGUISettings.geomWeldToler);
                            if (isclosed) mycpts.Add(ep);
                            cpts.Add(mycpts);
                            //break;
                        }
                    }
                    // cpts.Add(mycpts); 
                }

            }
            else if (usercodeinputs[0] is ModelVisual3D)
            {
                ModelVisual3D mvao = usercodeinputs[0] as ModelVisual3D;
                List<GeometryModel3D> lg = new List<GeometryModel3D>();
                mvao.getAllGeometryChildren(lg);
                foreach (GeometryModel3D mao in lg)
                {
                    var segments = MeshGeometryHelper.GetContourSegments(mao.Geometry as MeshGeometry3D, thePlaner.Position, thePlaner.Normal).ToList();
                    //List<Point3D> mycpts = new List<Point3D>();
                    foreach (var contour in MeshGeometryHelper.CombineSegments(segments, PUPPIGUISettings.geomWeldToler).ToList())
                    {
                        if (contour.Count > 2)
                        {
                            List<Point3D> mycpts = new List<Point3D>();
                            mycpts.AddRange(contour);
                            Point3D sp = (Point3D)contour[0];
                            Point3D ep = (Point3D)contour[contour.Count - 1];
                            bool isclosed = false;
                            if (sp.DistanceTo(ep) < PUPPIGUISettings.geomWeldToler)
                            {
                                isclosed = true;
                            }
                            HelperClasses.helperfunctions.pointsList3dPrune(mycpts, PUPPIGUISettings.geomWeldToler);
                            if (isclosed) mycpts.Add(ep);
                            cpts.Add(mycpts);
                            //break;
                        }
                    }
                    //cpts.Add(mycpts);
                }
            }
            else if (usercodeinputs[0] is Visual3D)
            {
                Visual3D vao = usercodeinputs[0] as Visual3D;
                ModelVisual3D mvao = vao as ModelVisual3D;
                if (mvao != null)
                {
                    List<GeometryModel3D> lg = new List<GeometryModel3D>();
                    mvao.getAllGeometryChildren(lg);
                    foreach (GeometryModel3D mao in lg)
                    {
                        var segments = MeshGeometryHelper.GetContourSegments(mao.Geometry as MeshGeometry3D, thePlaner.Position, thePlaner.Normal).ToList();
                        foreach (var contour in MeshGeometryHelper.CombineSegments(segments, PUPPIGUISettings.geomWeldToler).ToList())
                        {
                            if (contour.Count > 2)
                            {
                                List<Point3D> mycpts = new List<Point3D>();
                                mycpts.AddRange(contour);
                                Point3D sp = (Point3D)contour[0];
                                Point3D ep = (Point3D)contour[contour.Count - 1];
                                bool isclosed = false;
                                if (sp.DistanceTo(ep) < PUPPIGUISettings.geomWeldToler)
                                {
                                    isclosed = true;
                                }
                                HelperClasses.helperfunctions.pointsList3dPrune(mycpts, PUPPIGUISettings.geomWeldToler);
                                if (isclosed) mycpts.Add(ep);
                                cpts.Add(mycpts);
                                // break;
                            }
                        }
                    }
                }
                else
                {
                    usercodeoutputs[0] = "Invalid Feature";
                }
            }
            else
            {
                usercodeoutputs[0] = "Invalid Feature";

            }
            usercodeoutputs[0] = cpts;
        }
    }

    public class PUPPIModelGetDimensions : PUPPIModule
    {
        public PUPPIModelGetDimensions()
            : base()
        {
            name = "Get 3D Model Dims";
            description = "Gets the start position and dimensions of Model3D or ModelVisual3D and children bounding box";
            outputs.Add(0);
            outputnames.Add("XStart");
            outputs.Add(0);
            outputnames.Add("YStart");
            outputs.Add(0);
            outputnames.Add("ZStart");

            outputs.Add(0);
            outputnames.Add("XSize");
            outputs.Add(0);
            outputnames.Add("YSize");
            outputs.Add(0);
            outputnames.Add("ZSize");

            inputnames.Add("Model");
            inputs.Add(new PUPPIInParameter());


        }
        public override void process_usercode()
        {

            try
            {
                Rect3D ml;
                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D m = usercodeinputs[0] as ModelVisual3D;
                    ml = m.getBBRecursive();


                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D m = usercodeinputs[0] as Model3D;
                    ml = m.Bounds;
                }
                else
                {
                    usercodeoutputs[0] = "Invalid input";
                    return;
                }
                if (!ml.IsEmpty)
                {
                    usercodeoutputs[0] = ml.Location.X;
                    usercodeoutputs[1] = ml.Location.Y;
                    usercodeoutputs[2] = ml.Location.Z;

                    usercodeoutputs[3] = ml.SizeX;
                    usercodeoutputs[4] = ml.SizeY;
                    usercodeoutputs[5] = ml.SizeZ;

                }
            }
            catch
            {
                usercodeoutputs[0] = "error";
            }
        }
    }

    /// <summary>
    /// Sets the mesh positions of a Model3D or ModelVisual3D object from collection of Point3D. Children ignored.
    /// </summary>
    public class PUPPIModelSetPoints : PUPPIModule
    {
        public PUPPIModelSetPoints()
            : base()
        {
            name = "Sets 3D Model Points";
            description = "Sets the mesh positions of a Model3D or ModelVisual3D object from collection of Point3D. Children ignored.";
            outputs.Add("not run");
            outputnames.Add("UpdatedModel");

            inputnames.Add("Model");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Points Collection");
            inputs.Add(new PUPPIInParameter());

        }
        public override void process_usercode()
        {
            usercodeoutputs[0] = null;
            try
            {
                ArrayList linput = new ArrayList();
                if (usercodeinputs[1] is ICollection)
                    linput = new ArrayList(usercodeinputs[1] as ICollection);
                else if (usercodeinputs[1] is IEnumerable)
                    linput = PUPPIModule.makeMeAnArrayList(usercodeinputs[1] as IEnumerable);
                else
                {
                    usercodeoutputs[0] = "Invalid arguments";
                    return;
                }
                List<Point3D> plist = linput.Cast<Point3D>().ToList();
                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D m = usercodeinputs[0] as ModelVisual3D;
                    ModelVisual3D mc = m.cloneMyVisual();
                    usercodeoutputs[0] = mc.setMy3DVisualModelPositionsAsPoints(plist);
                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D m = usercodeinputs[0] as Model3D;
                    Model3D mc = m.Clone();
                    usercodeoutputs[0] = mc.setMy3DModelPositionsAsPoints(plist);
                }
            }
            catch
            {
                usercodeoutputs[0] = "error";
            }
        }

    }
    /// <summary>
    /// Applies one or a list of transforms to a 3D object, which is cloned
    /// </summary>
    public class PUPPIApplyTransform : PUPPIModule
    {
        public PUPPIApplyTransform()
            : base()
        {
            name = "Apply Transform(s)";
            description = "Applies one or a list of transforms to a 3D object, which is cloned";
            inputnames.Add("3D Object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Transf/List");
            inputs.Add(new PUPPIInParameter());
            outputnames.Add("3D Object");
            outputs.Add(null);

        }
        public override void process_usercode()
        {
            Transform3DGroup groupo = new Transform3DGroup();
            try
            {

                if (usercodeinputs[1] is ICollection || usercodeinputs[1] is IEnumerable)
                {
                    ArrayList ui = PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[1]) as ArrayList;
                    for (int ti = 0; ti < ui.Count; ti++)
                    {
                        Transform3D plao = (ui[ti] as Transform3D).Clone();
                        groupo.Children.Add(plao);
                    }
                }//or just one transform
                else
                {
                    Transform3D clao = (usercodeinputs[1] as Transform3D).Clone();
                    groupo.Children.Add(clao);
                }
            }
            catch
            {
                usercodeoutputs[0] = "Invalid Transform";
                return;
            }

            try
            {

                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D m = usercodeinputs[0] as ModelVisual3D;
                    ModelVisual3D mao = m.cloneMyVisual();
                    if (m.Transform != null)
                    {
                        mao.Transform = HelperClasses.helperfunctions.transformCombiner(m.Transform, groupo);
                    }
                    else
                    {
                        mao.Transform = groupo;
                    }
                    usercodeoutputs[0] = mao;

                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D m = usercodeinputs[0] as Model3D;
                    Model3D mao = m.Clone();
                    if (m.Transform != null)
                    {
                        mao.Transform = HelperClasses.helperfunctions.transformCombiner(m.Transform, groupo);
                    }
                    else
                    {
                        mao.Transform = groupo;
                    }
                    usercodeoutputs[0] = mao;
                }

                else
                {
                    usercodeoutputs[0] = "Invalid object input type.";
                    return;
                }
            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = "Object input error: " + exy.ToString(); ;
                return;
            }



        }
    }

    /// <summary>
    /// Hardcodes the transforms of a 3D object to its points. Object is cloned
    /// </summary>
    public class PUPPIHardcodeTransform : PUPPIModule
    {
        public PUPPIHardcodeTransform()
            : base()
        {
            name = "Hardcode Transform(s)";
            description = "Hardcodes the transforms of a 3D object to its points. Object is cloned";
            inputnames.Add("3D Object");
            inputs.Add(new PUPPIInParameter());
            outputnames.Add("3D Object");
            outputs.Add(null);

        }
        public override void process_usercode()
        {


            try
            {

                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D m = usercodeinputs[0] as ModelVisual3D;
                    ModelVisual3D mao = HelperClasses.helperfunctions.htr(m, new Transform3DGroup());

                    usercodeoutputs[0] = mao;

                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D m = usercodeinputs[0] as Model3D;
                    Model3D mao = m.Clone();
                    ModelVisual3D malao = new ModelVisual3D();
                    malao.Content = mao;

                    usercodeoutputs[0] = HelperClasses.helperfunctions.htr(malao, new Transform3DGroup());
                }

                else
                {
                    usercodeoutputs[0] = "Invalid object input type.";
                    return;
                }
            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = "Object input error: " + exy.ToString(); ;
                return;
            }



        }
    }

    public class PUPPIRefineModelMesh : PUPPIModule
    {
        public PUPPIRefineModelMesh()
            : base()
        {
            name = "Refine Object";
            description = "Refines ModelVisual3D or Model3D through loop subdivision - experimental, doesn't work well on every geometry";
            inputnames.Add("Object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Number Subdivides");
            inputs.Add(new PUPPIInParameter());
            outputnames.Add("Refined");
            outputs.Add(null);
        }
        public override void process_usercode()
        {

            try
            {

                int numberSub = Convert.ToInt16(usercodeinputs[1]);
                if (numberSub <= 0)
                {
                    usercodeoutputs[0] = "Invalid number subdivisions";
                    return;
                }
                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D m = usercodeinputs[0] as ModelVisual3D;

                    usercodeoutputs[0] = HelperClasses.utilities.subdivideSurface(m, numberSub);

                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D m = usercodeinputs[0] as Model3D;

                    usercodeoutputs[0] = HelperClasses.utilities.subdivideSurface(m, numberSub);
                }

                else
                {
                    usercodeoutputs[0] = "Invalid object input type.";
                    return;
                }
            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = "Processing error:" + exy.ToString(); ;
                return;
            }



        }
    }
    /// <summary>
    /// Resets transform to identity
    /// </summary>
    public class PUPPIResetTransform : PUPPIModule
    {
        public PUPPIResetTransform()
            : base()
        {
            name = "Reset Transform(s)";
            description = "Resets transform to identity";
            inputnames.Add("3D Object");
            inputs.Add(new PUPPIInParameter());
            outputnames.Add("3D Object");
            outputs.Add(null);

        }
        public override void process_usercode()
        {

            try
            {

                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D m = usercodeinputs[0] as ModelVisual3D;
                    ModelVisual3D mao = m.cloneMyVisual();
                    mao.Transform = Transform3D.Identity;
                    usercodeoutputs[0] = mao;

                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D m = usercodeinputs[0] as Model3D;
                    Model3D mao = m.Clone();
                    mao.Transform = Transform3D.Identity;
                    usercodeoutputs[0] = mao;
                }

                else
                {
                    usercodeoutputs[0] = "Invalid object input type.";
                    return;
                }
            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = "Object input error: " + exy.ToString(); ;
                return;
            }



        }
    }



    /// <summary>
    /// Aligns a 3D object with transforms based on supplied new origin position and XY axes.
    /// </summary>
    public class PUPPIAlignObject : PUPPIModule
    {
        public PUPPIAlignObject()
            : base()
        {
            name = "Align Object";
            description = " Aligns a 3D object using transforms based on supplied new origin position and XY axes.";
            inputnames.Add("3D Object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("New Origin");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("New X Axis");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("New Y Axis");
            inputs.Add(new PUPPIInParameter());
            outputnames.Add("Displaced 3D Object");
            outputs.Add(null);

        }
        public override void process_usercode()
        {


            try
            {
                Point3D pos = (Point3D)usercodeinputs[1];

                Vector3D xaxis = (Vector3D)usercodeinputs[2];
                if (xaxis.Length == 0)
                {
                    usercodeoutputs[0] = "Invalid X Axis";
                    return;
                }

                Vector3D yaxis = (Vector3D)usercodeinputs[3];
                if (yaxis.Length == 0)
                {
                    usercodeoutputs[0] = "Invalid Y Axis";
                    return;
                }

                if (Vector3D.DotProduct(xaxis, yaxis) != 0)
                {
                    usercodeoutputs[0] = "Invalid Axis Combination";
                    return;
                }
                Vector3D oxaxis = new Vector3D(1, 0, 0);
                Vector3D oyaxis = new Vector3D(0, 1, 0);
                Transform3DGroup tg = new Transform3DGroup();

                AxisAngleRotation3D xrot = new AxisAngleRotation3D(Vector3D.CrossProduct(oxaxis, xaxis), Vector3D.AngleBetween(oxaxis, xaxis));
                RotateTransform3D rax = new RotateTransform3D(xrot);
                Vector3D noyaxis = Vector3D.Multiply(oyaxis, rax.Value);

                AxisAngleRotation3D yrot = new AxisAngleRotation3D(Vector3D.CrossProduct(noyaxis, yaxis), Vector3D.AngleBetween(noyaxis, yaxis));

                tg.Children.Add(rax);
                tg.Children.Add(new RotateTransform3D(yrot));
                tg.Children.Add(new TranslateTransform3D(new Vector3D(pos.X, pos.Y, pos.Z)));

                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D m = usercodeinputs[0] as ModelVisual3D;
                    ModelVisual3D mao = m.cloneMyVisual();
                    mao.Transform = HelperClasses.helperfunctions.transformCombiner(mao.Transform, tg);
                    usercodeoutputs[0] = mao;

                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D m = usercodeinputs[0] as Model3D;
                    Model3D mao = m.Clone();
                    mao.Transform = HelperClasses.helperfunctions.transformCombiner(mao.Transform, tg);
                    usercodeoutputs[0] = mao;
                }

                else
                {
                    usercodeoutputs[0] = "Invalid object input type.";
                    return;
                }
            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = "Object input error: " + exy.ToString(); ;
                return;
            }



        }
    }


    public class PUPPISliceModel : PUPPIModule
    {
        public PUPPISliceModel()
            : base()
        {
            name = "Slice Object";
            description = "Slices a 3D model based on a cutting plane or a list of cutting planes.Caps ends of models if input is 1";
            inputnames.Add("3D Object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Plane/List");
            inputs.Add(new PUPPIInParameter());
            PUPPIInParameter pi = new PUPPIInParameter();
            pi.isoptional = true;
            inputnames.Add("Cap 1/0");
            inputs.Add(pi);
            outputnames.Add("3D Object Lst.");
            outputs.Add(null);
        }
        public override void process_usercode()
        {
            try
            {
                bool bb = false;
                if (usercodeinputs[2] != null)
                {
                    try
                    {
                        bb = Convert.ToBoolean(usercodeinputs[2]);
                    }
                    catch
                    {
                        bb = false;
                    }
                }
                List<HelperClasses.PUPPIPlane3D> plist = new List<HelperClasses.PUPPIPlane3D>();
                if (usercodeinputs[1] is IEnumerable)
                {
                    ArrayList aa = makeCollOrEnumIntoArrayList(usercodeinputs[1]);
                    for (int pa = 0; pa < aa.Count; pa++)
                    {
                        if (aa[pa] is HelperClasses.PUPPIPlane3D)
                        {
                            plist.Add(aa[pa] as HelperClasses.PUPPIPlane3D);
                        }
                    }
                }
                else if (usercodeinputs[1] is HelperClasses.PUPPIPlane3D)
                {
                    plist.Add(usercodeinputs[1] as HelperClasses.PUPPIPlane3D);
                }
                if (plist.Count == 0)
                {
                    usercodeoutputs[0] = "No valid planes";
                    return;
                }
                if (usercodeinputs[0] is Visual3D)
                {
                    ModelVisual3D mv = usercodeinputs[0] as ModelVisual3D;
                    if (mv != null)
                    {


                        List<ModelVisual3D> cutmodels = HelperClasses.helperfunctions.cutMVByPlanes(mv, plist);
                        //fill
                        if (bb)
                        {
                            int cutmoi = 0;

                            foreach (ModelVisual3D slice in cutmodels)
                            {

                                List<GeometryModel3D> lg = new List<GeometryModel3D>();
                                try
                                {
                                    slice.getAllGeometryChildren(lg);
                                }
                                catch
                                {
                                    lg = new List<GeometryModel3D>();
                                }
                                string namer = "";
                                try
                                {
                                    namer = slice.GetName();
                                }
                                catch
                                {
                                    namer = "";

                                }
                                if (namer == "" || namer == null)
                                    namer = "SliceModelV_" + cutmoi.ToString();
                                int cutGi = 0;
                                foreach (GeometryModel3D mao in lg)
                                {

                                    Material makemat = mao.Material;
                                    foreach (HelperClasses.PUPPIPlane3D cp in plist)
                                    {

                                        //displace to make sure we cut
                                        List<Point3D> displaneso = new List<Point3D>();
                                        Vector3D normalizedN = cp.thenormal;
                                        normalizedN.Normalize();
                                        displaneso.Add(cp.theorigin + normalizedN * PUPPIGUISettings.geomCapToler);
                                        displaneso.Add(cp.theorigin - normalizedN * PUPPIGUISettings.geomCapToler);

                                        foreach (Point3D ppp in displaneso)
                                        {

                                            List<List<Point3D>> contours = HelperClasses.helperfunctions.getM3DConts(mao as Model3D, ppp, cp.thenormal);
                                            foreach (List<Point3D> onec in contours)
                                            {
                                                if (onec.Count > 2)
                                                {
                                                    Point3D sp = (Point3D)onec[0];
                                                    Point3D ep = (Point3D)onec[onec.Count - 1];
                                                    bool isclosed = false;
                                                    if (sp.DistanceTo(ep) < PUPPIGUISettings.geomWeldToler)
                                                    {
                                                        isclosed = true;
                                                    }
                                                    HelperClasses.helperfunctions.pointsList3dPrune(onec, PUPPIGUISettings.geomWeldToler);
                                                    if (isclosed)
                                                    {
                                                        onec.Add(ep);
                                                        if (makemat == null)
                                                        {
                                                            makemat = new DiffuseMaterial(new SolidColorBrush(Colors.White));
                                                        }
                                                        ModelVisual3D myPlane = HelperClasses.helperfunctions.pillFolly(onec, makemat);
                                                        myPlane.SetName("PS_" + namer + cutGi.ToString());
                                                        slice.Children.Add(myPlane);
                                                    }
                                                    else
                                                    {

                                                    }
                                                }
                                                cutGi++;
                                            }
                                            cutGi++;
                                        }
                                        cutGi++;
                                    }

                                }
                                cutmoi++;
                            }
                        }


                        usercodeoutputs[0] = cutmodels; //HelperClasses.helperfunctions.cutMVByPlanes(mv, plist);//, bb, new List<ModelVisual3D>());
                        return;
                    }
                    else
                    {
                        usercodeoutputs[0] = "Invalid model";
                        return;
                    }
                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D mv = usercodeinputs[0] as Model3D;
                    if (mv != null)
                    {
                        //no filling here, we'll see
                        usercodeoutputs[0] = HelperClasses.helperfunctions.cutMByPlanes(mv, plist);
                        return;
                    }
                    else
                    {
                        usercodeoutputs[0] = "Invalid model";
                        return;
                    }

                }
                else
                {
                    usercodeoutputs[0] = "Invalid object type";
                }
            }

            catch (Exception exy)
            {
                usercodeoutputs[0] = exy.ToString();
            }
        }
    }

    /// <summary>
    /// Changes color of a 3D object, which is cloned
    /// </summary>
    public class PUPPIChangeColor : PUPPIModule
    {
        public PUPPIChangeColor()
            : base()
        {
            name = "Change Color";
            description = "Changes color of a 3D object, which is cloned";
            inputnames.Add("3D Object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Color");
            inputs.Add(new PUPPIInParameter());
            outputnames.Add("3D Object");
            outputs.Add(null);

        }
        public override void process_usercode()
        {

            Color colo = (Color)usercodeinputs[1];

            try
            {

                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D m = usercodeinputs[0] as ModelVisual3D;
                    ModelVisual3D mao = m.cloneMyVisual();
                    mao.colorVisualAndChildren(colo);
                    usercodeoutputs[0] = mao;

                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D m = usercodeinputs[0] as Model3D;

                    Model3D mao = m.Clone();

                    GeometryModel3D gg = mao as GeometryModel3D;
                    if (gg != null) gg.Material = new DiffuseMaterial(new SolidColorBrush(colo));
                    usercodeoutputs[0] = gg;
                }

                else
                {
                    usercodeoutputs[0] = "Invalid object input type.";
                    return;
                }
            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = "Object input error: " + exy.ToString(); ;
                return;
            }



        }
    }


    /// <summary>
    /// Recalculates normals of 3D object for render quality improvement
    /// </summary>
    public class PUPPIRecalcNormals : PUPPIModule
    {
        public PUPPIRecalcNormals()
            : base()
        {
            name = "Recalculate Normals";
            description = "Recalculates normals of a 3D object, which is cloned";
            inputnames.Add("3D Object");
            
            inputs.Add(new PUPPIInParameter());
            outputnames.Add("3D Object");
            outputs.Add(null);

        }
        public override void process_usercode()
        {

            

            try
            {

                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D m = usercodeinputs[0] as ModelVisual3D;
                    ModelVisual3D mao = m.cloneMyVisual();
                    mao.RecalVisNo();
                    usercodeoutputs[0] = mao;

                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D m = usercodeinputs[0] as Model3D;

                    Model3D mao = m.Clone();

                    GeometryModel3D gg = mao as GeometryModel3D;
                    MeshGeometry3D ms = gg.Geometry.Clone() as MeshGeometry3D;
                    Vector3DCollection nn = MeshGeometryHelper.CalculateNormals(ms);
                    ms.Normals = nn;
                    gg.Geometry = ms;
                    usercodeoutputs[0] = gg;
                }

                else
                {
                    usercodeoutputs[0] = "Invalid object input type.";
                    return;
                }
            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = "Object input error: " + exy.ToString(); ;
                return;
            }



        }
    }

    /// <summary>
    /// Sets texture on a 3D object from a file.3D object is cloned
    /// </summary>
    public class PUPPISetTexture : PUPPIModule
    {
        public PUPPISetTexture()
            : base()
        {
            name = "Set Texture";
            description = "Sets texture on a 3D object from a file.3D object is cloned";
            inputnames.Add("3D Object");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Image File Path");
            inputs.Add(new PUPPIInParameter());
            outputnames.Add("3D Object");
            outputs.Add(null);

        }
        public override void process_usercode()
        {

            try
            {
                string filepath = usercodeinputs[1].ToString();
                if (System.IO.File.Exists(filepath) == false)
                {
                    usercodeoutputs[0] = "Image file does not exist";
                    return;
                }
                Material mat = MaterialHelper.CreateImageMaterial(filepath);
                if (mat == null)
                {
                    usercodeoutputs[0] = "Invalid image file";
                    return;
                }


                if (usercodeinputs[0] is ModelVisual3D)
                {
                    ModelVisual3D m = usercodeinputs[0] as ModelVisual3D;
                    ModelVisual3D mao = m.cloneMyVisual();
                    mao.materialVisualAndChildren(mat);
                    usercodeoutputs[0] = mao;

                }
                else if (usercodeinputs[0] is Model3D)
                {
                    Model3D m = usercodeinputs[0] as Model3D;

                    Model3D mao = m.Clone();

                    GeometryModel3D gg = mao as GeometryModel3D;
                    if (gg != null) gg.Material = mat;
                    usercodeoutputs[0] = gg;
                }

                else
                {
                    usercodeoutputs[0] = "Invalid object input type.";
                    return;
                }
            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = "Object input error: " + exy.ToString(); ;
                return;
            }



        }
    }

    namespace HelperClasses
    {
        /// <summary>
        /// 2D closed sketch,a collection of 2D points.
        /// Used for making extrusions. Cannot be added to CAD model directly.
        /// </summary>
        public class PUPPISketch2D
        {
            public List<Point> points2dlist;
            public PUPPISketch2D()
            {
                points2dlist = new List<Point>();
            }
            //giving it an array of points
            public PUPPISketch2D(ArrayList myp2ds)
            {
                points2dlist = new List<Point>();
                try
                {
                    for (int i = 0; i < myp2ds.Count; i++)
                    {

                        Point pp = (Point)(myp2ds[i]);
                        points2dlist.Add(pp);
                    }
                }
                catch
                {

                }
            }
            //gets the point list
            public ArrayList getPointList()
            {
                ArrayList mypoints = new ArrayList();
                foreach (Point p in points2dlist)
                {

                    mypoints.Add(p);
                }
                return mypoints;

            }

            public Rect GetBounds()
            {
                double maxx = double.MinValue;
                double maxy = double.MinValue;
                double minx = double.MaxValue;
                double miny = double.MaxValue;
                int pc = points2dlist.Count;
                for (int i = 0; i < pc; i++)
                {
                    if (points2dlist[i].X > maxx) maxx = points2dlist[i].X;
                    if (points2dlist[i].X < minx) minx = points2dlist[i].X;
                    if (points2dlist[i].Y > maxy) maxy = points2dlist[i].Y;
                    if (points2dlist[i].Y < miny) miny = points2dlist[i].Y;

                }
                return new Rect(minx, miny, maxx - minx, maxy - miny);
            }
            //makes sketch as a circle
            public static PUPPISketch2D PUPPICircleSketch2D(Point ccenter, double cradius, int nsegments)
            {
                ArrayList points2dlist = new ArrayList();
                try
                {
                    double overseg = 1.0 / Convert.ToDouble(nsegments);
                    for (int i = 0; i < nsegments; i++)
                    {

                        double angle = Math.PI * 2 * overseg * i;
                        Point pp = new Point(ccenter.X + cradius * Math.Cos(angle), ccenter.Y + cradius * Math.Sin(angle));
                        points2dlist.Add(pp);
                    }
                    return new PUPPISketch2D(points2dlist);
                }
                catch
                {
                    return null;
                }
            }
            //makes sketch as a circle
            public static PUPPISketch2D PUPPIArcSketch2D(Point ccenter, double cradius, int nsegments, double startAngleRad, double endAngleRad)
            {
                ArrayList points2dlist = new ArrayList();
                try
                {
                    double overseg = 1.0 / Convert.ToDouble(nsegments);
                    //points is 1+number segments
                    for (int i = 0; i <= nsegments; i++)
                    {

                        double angle = startAngleRad + (endAngleRad - startAngleRad) * overseg * i;
                        Point pp = new Point(ccenter.X + cradius * Math.Cos(angle), ccenter.Y + cradius * Math.Sin(angle));
                        points2dlist.Add(pp);
                    }
                    return new PUPPISketch2D(points2dlist);
                }
                catch
                {
                    return null;
                }
            }

            //makes a 2d capsule
            //counterclockwise
            public static PUPPISketch2D PUPPICapsuleSketch2D(Point ccenter, double height, double width, int narcsegments)
            {
                ArrayList points2dlist = new ArrayList();
                try
                {
                    double overseg = 1.0 / Convert.ToDouble(narcsegments);
                    //points is 1+number segments
                    for (int i = 0; i <= narcsegments; i++)
                    {

                        double angle = -Math.PI * 0.5 + Math.PI * overseg * i;
                        Point pp = new Point(ccenter.X + width * 0.5 + height * 0.5 * Math.Cos(angle), ccenter.Y + height * 0.5 * Math.Sin(angle));
                        points2dlist.Add(pp);
                    }
                    for (int i = 0; i <= narcsegments; i++)
                    {

                        double angle = Math.PI * 0.5 + Math.PI * overseg * i;
                        Point pp = new Point(ccenter.X - width * 0.5 + height * 0.5 * Math.Cos(angle), ccenter.Y + height * 0.5 * Math.Sin(angle));
                        points2dlist.Add(pp);
                    }

                    return new PUPPISketch2D(points2dlist);
                }
                catch
                {
                    return null;
                }
            }

            //makes sketch as a rectangle
            public static PUPPISketch2D PUPPIRectangleSketch2D(Point ccenter, double width, double height)
            {
                ArrayList points2dlist = new ArrayList();
                try
                {


                    points2dlist.Add(new Point(ccenter.X - width / 2, ccenter.Y - height / 2));
                    points2dlist.Add(new Point(ccenter.X + width / 2, ccenter.Y - height / 2));
                    points2dlist.Add(new Point(ccenter.X + width / 2, ccenter.Y + height / 2));
                    points2dlist.Add(new Point(ccenter.X - width / 2, ccenter.Y + height / 2));

                    return new PUPPISketch2D(points2dlist);
                }
                catch
                {
                    return null;
                }
            }
            //makes sketch as an ellipse
            public static PUPPISketch2D PUPPIEllipseSketch2D(Point ccenter, double majoraxis, double minoraxis, int nsegments)
            {
                ArrayList points2dlist = new ArrayList();
                try
                {
                    double overseg = 1.0 / Convert.ToDouble(nsegments);
                    for (int i = 0; i < nsegments; i++)
                    {

                        double angle = Math.PI * 2 * overseg * i;
                        Point pp = new Point(ccenter.X + majoraxis * Math.Cos(angle), ccenter.Y + minoraxis * Math.Sin(angle));
                        points2dlist.Add(pp);
                    }
                    return new PUPPISketch2D(points2dlist);
                }
                catch
                {
                    return null;
                }
            }

            public static PUPPISketch2D PUPPIIBeamSketch2D(Point center, double width, double height, double flangeThickness, double webThickness)
            {
                ArrayList points2dlist = new ArrayList();
                try
                {

                    Vector pp = new Vector(-webThickness / 2, height / 2 - flangeThickness);
                    points2dlist.Add(pp + center);

                    pp = new Vector(-width / 2, height / 2 - flangeThickness);
                    points2dlist.Add(pp + center);

                    pp = new Vector(-width / 2, height / 2);
                    points2dlist.Add(pp + center);

                    pp = new Vector(width / 2, height / 2);
                    points2dlist.Add(pp + center);

                    pp = new Vector(width / 2, height / 2 - flangeThickness);
                    points2dlist.Add(pp + center);


                    pp = new Vector(webThickness / 2, height / 2 - flangeThickness);
                    points2dlist.Add(pp + center);

                    pp = new Vector(webThickness / 2, -height / 2 + flangeThickness);
                    points2dlist.Add(pp + center);


                    pp = new Vector(width / 2, -height / 2 + flangeThickness);
                    points2dlist.Add(pp + center);


                    pp = new Vector(width / 2, -height / 2);
                    points2dlist.Add(pp + center);


                    pp = new Vector(-width / 2, -height / 2);
                    points2dlist.Add(pp + center);

                    pp = new Vector(-width / 2, -height / 2 + flangeThickness);
                    points2dlist.Add(pp + center);

                    pp = new Vector(-webThickness / 2, -height / 2 + flangeThickness);
                    points2dlist.Add(pp + center);

                    return new PUPPISketch2D(points2dlist);


                }
                catch
                {
                    return null;
                }
            }

            public static PUPPISketch2D offsetSketch(PUPPISketch2D originalSketch, double offset)
            {
                PUPPISketch2D newSketch = new PUPPISketch2D();
                ArrayList mpt = originalSketch.getPointList();
                ArrayList opt = new ArrayList();
                double xi = 0;
                double yi = 0;
                if (mpt.Count > 0)
                {
                    for (int i = 0; i < mpt.Count; i++)
                    {
                        Point mpti = (Point)mpt[i];
                        xi += mpti.X;
                        yi += mpti.Y;

                    }
                    xi = xi / mpt.Count;
                    yi = yi / mpt.Count;
                    Point ai = new Point(xi, yi);
                    for (int i = 0; i < mpt.Count; i++)
                    {
                        Point mpti = (Point)mpt[i];
                        Vector vai = Point.Subtract(mpti, ai);
                        if (vai.Length > 0)
                        {
                            double dl = vai.Length + offset;
                            vai.Normalize();
                            vai = vai * dl;
                            mpti = ai + vai;
                        }
                        opt.Add(mpti);

                    }
                    newSketch = new PUPPISketch2D(opt);

                }
                return newSketch;
            }

            public static PUPPISketch2D rotateSketch(PUPPISketch2D originalSketch, double angleDeg, Point center)
            {
                PUPPISketch2D newSketch = new PUPPISketch2D();
                ArrayList mpt = originalSketch.getPointList();
                ArrayList opt = new ArrayList();
                double xi = 0;
                double yi = 0;
                if (mpt.Count > 0)
                {

                    for (int i = 0; i < mpt.Count; i++)
                    {
                        Point mpti = (Point)mpt[i];
                        Vector vai = Point.Subtract(mpti, center);
                        if (vai.Length > 0)
                        {
                            double newX = vai.X * Math.Cos(angleDeg * Math.PI / 180) - vai.Y * Math.Sin(angleDeg * Math.PI / 180);
                            double newY = vai.Y * Math.Cos(angleDeg * Math.PI / 180) + vai.X * Math.Sin(angleDeg * Math.PI / 180);
                            mpti = center + new Vector(newX, newY);
                        }
                        opt.Add(mpti);

                    }
                    newSketch = new PUPPISketch2D(opt);

                }
                return newSketch;
            }

            public static PUPPISketch2D translateSketch(PUPPISketch2D originalSketch, double moveX, double moveY)
            {
                PUPPISketch2D newSketch = new PUPPISketch2D();
                ArrayList mpt = originalSketch.getPointList();
                ArrayList opt = new ArrayList();
                Vector moveme = new Vector(moveX, moveY);
                if (mpt.Count > 0)
                {

                    for (int i = 0; i < mpt.Count; i++)
                    {
                        Point mpti = (Point)mpt[i];
                        mpti = mpti + moveme;
                        opt.Add(mpti);

                    }
                    newSketch = new PUPPISketch2D(opt);

                }
                return newSketch;
            }



        }


        /// <summary>
        ///3D Polyline,a collection of 3D points.
        /// Cannot be added to CAD model directly,make a PUPPI POlyline object for that.
        /// </summary>
        public class PUPPIPolyLine3D
        {
            public List<Point3D> points3dlist;
            public PUPPIPolyLine3D()
            {
                points3dlist = new List<Point3D>();
            }
            //giving it an array of points
            public PUPPIPolyLine3D(ArrayList myp3ds)
            {
                points3dlist = new List<Point3D>();
                try
                {
                    for (int i = 0; i < myp3ds.Count; i++)
                    {

                        Point3D pp = (Point3D)(myp3ds[i]);
                        points3dlist.Add(pp);
                    }
                }
                catch
                {

                }


            }
            //giving it generic
            public PUPPIPolyLine3D(IEnumerable anyPoint3DCollection)
            {
                ArrayList makeme = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(anyPoint3DCollection);
                points3dlist = new List<Point3D>();
                try
                {
                    for (int i = 0; i < makeme.Count; i++)
                    {

                        Point3D pp = (Point3D)(makeme[i]);
                        points3dlist.Add(pp);
                    }
                }
                catch
                {

                }


            }

            public PUPPIPolyLine3D(Point3D startPoint, IEnumerable vectors)
            {
                ArrayList mp = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(vectors);
                points3dlist = new List<Point3D>();
                points3dlist.Add(startPoint);
                Point3D nextPoint = startPoint;
                for (int i = 0; i < mp.Count; i++)
                {
                    nextPoint = nextPoint + (Vector3D)mp[i];
                    points3dlist.Add(nextPoint);
                }


            }
            public double getPolyLineLength()
            {
                double le = 0;
                List<Point3D> plist = points3dlist;
                for (int i = 0; i < plist.Count - 1; i++)
                {
                    Vector3D lv = plist[i + 1] - plist[i];
                    le = le + lv.Length;
                }

                return le;
            }


            public Point3D getPointOnPolyLine(double lengthRatio)
            {
                Point3D myPoint = new Point3D();

                if (points3dlist.Count > 1)
                {

                    if (lengthRatio >= 0 && lengthRatio <= 1)
                    {
                        double wholength = getPolyLineLength();
                        double whereget = wholength * lengthRatio;
                        double clength = 0;
                        for (int i = 0; i < points3dlist.Count - 1; i++)
                        {
                            double cseg = utilities.getDistBetweenPoints(points3dlist[i], points3dlist[i + 1]);
                            if (clength + cseg < whereget)
                            {
                                clength += cseg;
                            }
                            else if (cseg > 0)
                            {
                                double diff = whereget - clength;
                                double pratio = diff / cseg;
                                return utilities.getPointBetweenPoints(points3dlist[i], points3dlist[i + 1], pratio);
                            }
                        }
                    }

                }
                else
                {

                }
                return myPoint;
            }

            public PUPPIPolyLine3D refactorPolyLine(int newNumberPoints)
            {
                PUPPIPolyLine3D pl = null;
                if (newNumberPoints < 2) return null;
                double getL = getPolyLineLength();
                if (getL <= 0) return null;
                if (points3dlist.Count < 2) return null;
                List<Point3D> newPlist = new List<Point3D>();
                for (int i = 0; i < newNumberPoints; i++)
                {
                    double thisr = (double)i / (double)(newNumberPoints - 1); // * (double)getL;/
                    if (thisr < 0) thisr = 0;
                    if (thisr > 1) thisr = 1;
                    newPlist.Add(getPointOnPolyLine(thisr));

                }
                if (newPlist.Count > 0) return new PUPPIPolyLine3D(newPlist);
                return null; ;
            }

            public List<PUPPIPolyLine3D> getPolyLineSegments()
            {
                List<PUPPIPolyLine3D> segs = new List<PUPPIPolyLine3D>();
                if (points3dlist.Count > 1)
                {
                    for (int i = 0; i < points3dlist.Count - 1; i++)
                    {
                        List<Point3D> these = new List<Point3D>();
                        these.Add(points3dlist[i]);
                        these.Add(points3dlist[i + 1]);
                        segs.Add(new PUPPIPolyLine3D(these));
                    }
                }
                return segs;
            }


            public double getPolyLinePtRatio(int zeroBasedIndex)
            {
                if (zeroBasedIndex < 0 || zeroBasedIndex >= points3dlist.Count) return -1;

                if (zeroBasedIndex == 0) return 0;
                if (zeroBasedIndex == points3dlist.Count - 1) return 1;
                double plenght = getPolyLineLength();
                if (plenght == 0) return 0;
                if (points3dlist.Count > 1)
                {

                    double sofar = 0;
                    for (int i = 0; i < zeroBasedIndex; i++)
                    {
                        double cseg = utilities.getDistBetweenPoints(points3dlist[i], points3dlist[i + 1]);
                        sofar += cseg;

                    }
                    return sofar / plenght;

                }
                else return 0;
            }


            public PUPPIPolyLine3D rotatePolyLine(Point3D center, Vector3D axis, double angleDegrees)
            {

                List<Point3D> nmp = new List<Point3D>();
                foreach (Point3D mp in points3dlist)
                {
                    Point3D mnmp = utilities.rotatePoint3D(mp, center, axis, angleDegrees);
                    nmp.Add(mnmp);
                }
                return new PUPPIPolyLine3D(nmp);
            }

            public PUPPIPolyLine3D translatePolyLine(Vector3D translation)
            {
                List<Point3D> nmp = new List<Point3D>();
                foreach (Point3D mp in points3dlist)
                {
                    Point3D mnmp = mp + translation;
                    nmp.Add(mnmp);
                }
                return new PUPPIPolyLine3D(nmp);
            }

            public PUPPIPolyLine3D offsetPolyLine(double offset)
            {
                Point3D centroid = helperfunctions.getCentroid(points3dlist);
                List<Point3D> nmp = new List<Point3D>();
                foreach (Point3D mp in points3dlist)
                {
                    Vector3D vl = mp - centroid;
                    Point3D mnmp = mp;

                    if (vl.Length > 0)
                    {
                        mnmp = centroid + vl * (vl.Length + offset) / (vl.Length);
                    }
                    nmp.Add(mnmp);
                }
                return new PUPPIPolyLine3D(nmp);
            }

            public PUPPIPolyLine3D trimStartByRatio(double lengthRatio)
            {
                PUPPIPolyLine3D pl = null;
                if (lengthRatio < 0 || lengthRatio > 1) return null;
                double getL = getPolyLineLength();
                if (getL <= 0) return null;
                if (points3dlist.Count < 2) return null;
                List<Point3D> newPlist = new List<Point3D>();
                double tratio = 0;
                for (int i = 0; i < points3dlist.Count; i++)
                {

                    double sratio = getPolyLinePtRatio(i);



                    if (sratio >= lengthRatio)
                    {
                        newPlist.Add(points3dlist[i]);



                    }
                    else if (i < points3dlist.Count - 1)
                    {
                        double eratio = getPolyLinePtRatio(i + 1);


                        if (sratio < lengthRatio && eratio > lengthRatio)
                        {
                            Point3D btnP = utilities.getPointBetweenPoints(points3dlist[i], points3dlist[i + 1], (lengthRatio - sratio) / (eratio - sratio));
                            newPlist.Add(btnP);

                        }
                    }
                }
                if (newPlist.Count > 0) return new PUPPIPolyLine3D(newPlist);
                return null;
            }

            public PUPPIPolyLine3D trimEndByRatio(double lengthRatio)
            {
                PUPPIPolyLine3D pl = null;
                if (lengthRatio < 0 || lengthRatio > 1) return null;
                double getL = getPolyLineLength();
                if (getL <= 0) return null;
                if (points3dlist.Count < 2) return null;
                List<Point3D> newPlist = new List<Point3D>();
                double tratio = 0;
                for (int i = 0; i < points3dlist.Count; i++)
                {
                    double sratio = getPolyLinePtRatio(i);
                    if (sratio <= lengthRatio)
                    {
                        newPlist.Add(points3dlist[i]);

                    }
                    if (i < points3dlist.Count - 1)
                    {
                        double eratio = getPolyLinePtRatio(i + 1);

                        if (sratio < lengthRatio && eratio > lengthRatio)
                        {

                            Point3D btnP = utilities.getPointBetweenPoints(points3dlist[i], points3dlist[i + 1], (lengthRatio - sratio) / (eratio - sratio));
                            newPlist.Add(btnP);
                            break;

                        }
                    }
                }
                if (newPlist.Count > 0) return new PUPPIPolyLine3D(newPlist);
                return null; ;
            }

            public PUPPIPolyLine3D trimStartByPoint(Point3D trimPoint)
            {

                Point3D trueTrimPoint = findNearestPoint(trimPoint);



                if (points3dlist.Count < 2) return null;
                List<Point3D> newPlist = new List<Point3D>();
                bool beenfound = false;
                for (int i = 0; i < points3dlist.Count; i++)
                {

                    if (beenfound == false)
                    {
                        if (i < points3dlist.Count - 1)
                        {
                            if (helperfunctions.isBetweenPts(points3dlist[i], points3dlist[i + 1], trueTrimPoint))
                            {
                                if (points3dlist[i].DistanceTo(trueTrimPoint) < PUPPIGUISettings.geomWeldToler)
                                {
                                    newPlist.Add(points3dlist[i]);
                                    beenfound = true;
                                }
                                else if (points3dlist[i + 1].DistanceTo(trueTrimPoint) < PUPPIGUISettings.geomWeldToler)
                                {
                                    newPlist.Add(points3dlist[i + 1]);
                                    beenfound = true;
                                    i++;
                                }
                                else
                                {
                                    newPlist.Add(trueTrimPoint);
                                    beenfound = true;
                                }
                            }
                        }
                        else
                        {
                            if (points3dlist[i].DistanceTo(trueTrimPoint) < PUPPIGUISettings.geomWeldToler)
                            {
                                newPlist.Add(points3dlist[i]);
                            }
                        }
                    }
                    else
                    {
                        newPlist.Add(points3dlist[i]);
                    }


                }
                if (newPlist.Count > 0) return new PUPPIPolyLine3D(newPlist);
                return null;
            }

            public PUPPIPolyLine3D trimEndByPoint(Point3D trimPoint)
            {

                Point3D trueTrimPoint = findNearestPoint(trimPoint);




                if (points3dlist.Count < 2) return null;
                List<Point3D> newPlist = new List<Point3D>();

                for (int i = 0; i < points3dlist.Count; i++)
                {


                    if (i < points3dlist.Count - 1)
                    {
                        if (helperfunctions.isBetweenPts(points3dlist[i], points3dlist[i + 1], trueTrimPoint))
                        {
                            if (points3dlist[i].DistanceTo(trueTrimPoint) < PUPPIGUISettings.geomWeldToler)
                            {
                                newPlist.Add(points3dlist[i]);
                                break;

                            }
                            else if (points3dlist[i + 1].DistanceTo(trueTrimPoint) < PUPPIGUISettings.geomWeldToler)
                            {
                                newPlist.Add(points3dlist[i]);
                                newPlist.Add(points3dlist[i + 1]);

                                break;
                            }
                            else
                            {
                                newPlist.Add(points3dlist[i]);
                                newPlist.Add(trueTrimPoint);
                                break;

                            }
                        }
                        else
                        {

                            newPlist.Add(points3dlist[i]);

                        }
                    }
                    else
                    {

                        newPlist.Add(points3dlist[i]);

                    }



                }
                if (newPlist.Count > 0) return new PUPPIPolyLine3D(newPlist);
                return null;
            }

            /// <summary>
            /// Gets the nearest point to argument, located on polyline.
            /// </summary>
            /// <param name="myPoint">A Point3D</param>
            /// <returns>Point3D</returns>
            public Point3D findNearestPoint(Point3D myPoint)
            {
                int findex = -1;
                double md = double.MaxValue;
                for (int i = 0; i < points3dlist.Count; i++)
                {

                    if (md > myPoint.DistanceTo(points3dlist[i]))
                    {
                        md = myPoint.DistanceTo(points3dlist[i]);
                        findex = i;
                    }
                }
                if (findex == -1) return new Point3D();
                if (findex == 0)
                {
                    Point3D fp1 = helperfunctions.closestOnLineBetween(points3dlist[findex], points3dlist[findex + 1], myPoint);
                    if (helperfunctions.isBetweenPts(points3dlist[findex], points3dlist[findex + 1], fp1))
                    {
                        return fp1;
                    }
                    else
                    {
                        return points3dlist[findex];
                    }
                }
                else if (findex == points3dlist.Count - 1)
                {
                    Point3D fp1 = helperfunctions.closestOnLineBetween(points3dlist[findex], points3dlist[findex - 1], myPoint);
                    if (helperfunctions.isBetweenPts(points3dlist[findex], points3dlist[findex - 1], fp1))
                    {
                        return fp1;
                    }
                    else
                    {
                        return points3dlist[findex];
                    }
                }
                else
                {
                    Point3D fp1 = helperfunctions.closestOnLineBetween(points3dlist[findex], points3dlist[findex + 1], myPoint);
                    if (helperfunctions.isBetweenPts(points3dlist[findex], points3dlist[findex + 1], fp1))
                    {

                    }
                    else
                    {
                        fp1 = points3dlist[findex];
                    }

                    Point3D fp2 = helperfunctions.closestOnLineBetween(points3dlist[findex], points3dlist[findex - 1], myPoint);
                    if (helperfunctions.isBetweenPts(points3dlist[findex], points3dlist[findex - 1], fp2))
                    {

                    }
                    else
                    {
                        fp2 = points3dlist[findex];
                    }

                    if (myPoint.DistanceTo(fp1) < myPoint.DistanceTo(fp2))
                    {
                        return fp1;
                    }
                    else
                    {
                        return fp2;
                    }
                }


            }



            public void reorderPoints(Point3D nearStart)
            {
                //get rid of duplicates
                helperfunctions.pointsList3dPrune(points3dlist, PUPPIGUISettings.geomWeldToler);
                int findex = -1;
                double dfound = Double.MaxValue;
                for (int i = 0; i < points3dlist.Count; i++)
                {
                    Point3D thisP = points3dlist[i];
                    double dc = thisP.DistanceTo(nearStart);
                    if (dc < dfound)
                    {
                        dfound = dc;
                        findex = i;
                    }
                }

                List<Point3D> neworder = new List<Point3D>();
                for (int i = findex; i < points3dlist.Count; i++)
                {
                    neworder.Add(points3dlist[i]);
                }
                for (int i = 0; i < findex; i++)
                {
                    neworder.Add(points3dlist[i]);
                }

                points3dlist = neworder;
                closePolyline3D();

            }

            public List<Point3D> getPoly3DPoints()
            {
                return points3dlist;
            }

            public void reversePolyline3D()
            {
                points3dlist.Reverse();
            }

            public void closePolyline3D()
            {
                if (points3dlist.Count > 2)
                {
                    Point3D sp = points3dlist[0];
                    Point3D ep = points3dlist[points3dlist.Count - 1];
                    if (sp.DistanceTo(ep) > PUPPIGUISettings.geomWeldToler)
                    {
                        points3dlist.Add(sp);
                    }
                }
            }
            public PUPPIPolyLine3D appendPolyline3D(PUPPIPolyLine3D otherPolyline)
            {
                List<Point3D> myNewPolyPts = new List<Point3D>();
                if (points3dlist.Count > 0 && otherPolyline.points3dlist.Count > 0)
                {

                    for (int i = 0; i < points3dlist.Count; i++)
                    {
                        myNewPolyPts.Add(points3dlist[i]);
                    }
                    Point3D sp = otherPolyline.points3dlist[0];
                    Point3D ep = points3dlist[points3dlist.Count - 1];
                    if (sp.DistanceTo(ep) > PUPPIGUISettings.geomWeldToler)
                    {
                        myNewPolyPts.Add(sp);
                    }
                    for (int i = 1; i < otherPolyline.points3dlist.Count; i++)
                    {
                        myNewPolyPts.Add(otherPolyline.points3dlist[i]);
                    }
                }
                else if (points3dlist.Count > 0)
                {
                    for (int i = 0; i < points3dlist.Count; i++)
                    {
                        myNewPolyPts.Add(points3dlist[i]);
                    }
                }
                else if (otherPolyline.points3dlist.Count > 0)
                {
                    for (int i = 0; i < otherPolyline.points3dlist.Count; i++)
                    {
                        myNewPolyPts.Add(otherPolyline.points3dlist[i]);
                    }
                }
                PUPPIPolyLine3D repo = new PUPPIPolyLine3D(myNewPolyPts);
                return repo;
            }
            public PUPPIPolyLine3D projectPolyline(PUPPIPlane3D projPlane)
            {
                if (projPlane == null) return null;
                List<Point3D> npl = new List<Point3D>();
                foreach (Point3D mp in points3dlist)
                {
                    object lp = projPlane.projectPoint(mp);
                    if (lp != null)
                    {
                        npl.Add((Point3D)lp);
                    }
                }
                if (npl.Count > 0)
                {
                    return new PUPPIPolyLine3D(npl);
                }
                return null;
            }
            //public List<Point3D> intersectPolyLine(PUPPIPolyLine3D otherPolyLine, PUPPIPlane3D projPlane)
            //{
            //    List<Point3D> pintersect = new List<Point3D>();
            //    return pintersect;
            //}
        }
        /// <summary>
        /// A plane object, based on origin 3D point and normal 3d vector. Cannot be added to 3D model directly.
        /// used to make 3D sketches. can convert to Helix Plane3D
        /// </summary>
        public class PUPPIPlane3D
        {
            public Plane3D theplane { get; set; }
            public Point3D theorigin { get; set; }
            public Vector3D thenormal { get; set; }
            //default
            public PUPPIPlane3D()
            {
                theorigin = new Point3D(0, 0, 0);
                thenormal = new Vector3D(0, 0, 1);
                theplane = new Plane3D(theorigin, thenormal);
            }

            public PUPPIPlane3D(Point3D origin, Vector3D normal)
            {
                theplane = new Plane3D(origin, normal);
                theorigin = origin;
                thenormal = normal;


            }


            public PUPPIPlane3D(Point3D p1, Point3D p2, Point3D p3)
            {
                theorigin = p1;
                Vector3D v1 = p2 - p1;
                Vector3D v2 = p3 - p1;
                thenormal = Vector3D.CrossProduct(v1, v2);
                theplane = new Plane3D(theorigin, thenormal);


            }

            //to make it compatible to WPF/Helix
            public Plane3D ConvToHelixPlane3D()
            {
                return theplane;
            }
            //returns intersection points between polyline segments and plane
            public List<Point3D> intersectionPoints(PUPPIPolyLine3D myPolyline)
            {
                List<Point3D> pPts = myPolyline.points3dlist;
                List<Point3D> myPoints = new List<Point3D>();
                Plane3D myPlane = theplane;
                for (int i = 0; i < pPts.Count - 1; i++)
                {
                    object myP = myPlane.LineIntersection(pPts[i], pPts[i + 1]);
                    
                    if (myP != null)
                    {
                        Point3D pao = (Point3D)myP;
                        if (pao.DistanceTo(pPts[i]) <= pPts[i].DistanceTo(pPts[i + 1]) && pao.DistanceTo(pPts[i+1]) <= pPts[i].DistanceTo(pPts[i + 1]))
                        myPoints.Add(pao);
                    }
                }


                return myPoints;
            }

            public Point3D? projectPoint(Point3D toProject)
            {
                //make long line
                double bigNumber = toProject.DistanceTo(this.theorigin) * 2;   // Double.MaxValue * 0.5;
                Vector3D normie = thenormal;
                normie.Normalize();
                Point3D onextreme = toProject - normie * bigNumber;
                Point3D otherextreme = toProject + normie * bigNumber;
                object tpla = theplane.LineIntersection(onextreme, otherextreme);
                if (tpla == null)
                {
                    return null;
                }
                else
                {

                    Point3D pao = (Point3D)tpla;
                    // if (Math.Abs (pao.X) >= bigNumber || Math.Abs(pao.Y) >= bigNumber || Math.Abs(pao.Z )>= bigNumber) return null;
                    return pao;
                }
            }

            public double pointMinDistance(Point3D measureFrom)
            {
                object proj = projectPoint(measureFrom);
                if (proj == null) return double.MinValue;
                return measureFrom.DistanceTo((Point3D)proj);
            }

        }




        /// <summary>
        /// Bezier spline creation and operations
        /// </summary>
        public class PUPPISpline3D
        {
            List<Point3D> splinePoints;
            internal double splineLength = 0;
            public PUPPISpline3D()
            {
                splinePoints = new List<Point3D>();
            }
            public PUPPISpline3D(IEnumerable controlPoints)
            {
                ArrayList mp = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(controlPoints);
                splinePoints = mp.Cast<Point3D>().ToList<Point3D>();

            }
            public PUPPISpline3D(Point3D startPoint, IEnumerable vectors)
            {
                ArrayList mp = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(vectors);
                splinePoints = new List<Point3D>();
                splinePoints.Add(startPoint);
                Point3D nextPoint = startPoint;
                for (int i = 0; i < mp.Count; i++)
                {
                    nextPoint = nextPoint + (Vector3D)mp[i];
                    splinePoints.Add(nextPoint);
                }


            }

            public void addControlPoint(Point3D p)
            {
                splinePoints.Add(p);
            }
            public void insertControlPoint(Point3D p, int index)
            {
                if (index >= 0 && index < splinePoints.Count)
                    splinePoints.Insert(index, p);

            }

            public void removeControlPoint(int index)
            {
                if (index >= 0 && index < splinePoints.Count)
                    splinePoints.RemoveAt(index);

            }

            public List<Point3D> convertToPoints(int numberPoints)
            {

                List<Point3D> discretized = new List<Point3D>();
                double ratioStep = 1.0 / (double)numberPoints;
                if (numberPoints > 1)
                {
                    for (int i = 0; i < numberPoints; i++)
                    {
                        double cRatio = (double)i * 1.0 / (double)(numberPoints - 1);
                        discretized.Add(getPointOnSpline(cRatio));
                    }
                    splineLength = 0;
                    for (int i = 0; i < discretized.Count - 1; i++)
                    {
                        Vector3D lv = discretized[i + 1] - discretized[i];
                        splineLength = splineLength + lv.Length;
                    }
                }

                return discretized;
            }

            public PUPPIPolyLine3D convToPolyLine3D(int numberPoints)
            {

                List<Point3D> discretized = new List<Point3D>();
                double ratioStep = 1.0 / (double)numberPoints;
                if (numberPoints > 1)
                {
                    for (int i = 0; i < numberPoints; i++)
                    {
                        double cRatio = (double)i * 1.0 / (double)(numberPoints - 1);
                        discretized.Add(getPointOnSpline(cRatio));
                    }
                    splineLength = 0;
                    for (int i = 0; i < discretized.Count - 1; i++)
                    {
                        Vector3D lv = discretized[i + 1] - discretized[i];
                        splineLength = splineLength + lv.Length;
                    }
                }

                return new PUPPIPolyLine3D(discretized);
            }


            public Point3D getPointOnSpline(double lengthRatio)
            {
                Point3D myPoint = new Point3D();
                List<Point3D> tempoints = splinePoints.Select(item => (Point3D)item.DeepClone()).ToList();
                if (splinePoints.Count > 1)
                {

                    if (lengthRatio >= 0 && lengthRatio <= 1)
                    {
                        int i = tempoints.Count - 1;
                        while (i > 0)
                        {
                            for (int k = 0; k < i; k++)
                            {
                                tempoints[k] = tempoints[k] + lengthRatio * (tempoints[k + 1] - tempoints[k]);
                            }
                            i--;
                        }
                    }
                    myPoint = tempoints[0];
                }
                else
                {
                    myPoint = tempoints[0];
                }
                return myPoint;
            }

            public double getSplineApproxLength(int numberPoints)
            {
                double le = 0;
                List<Point3D> plist = convertToPoints(numberPoints);
                for (int i = 0; i < plist.Count - 1; i++)
                {
                    Vector3D lv = plist[i + 1] - plist[i];
                    le = le + lv.Length;
                }
                splineLength = le;
                return le;
            }

        }

        

        /// <summary>
        /// Various methods to manipulate 3D objects
        /// </summary>
        public static class utilities
        {
            /// <summary>
            /// Gets the center of a bounding box defined as Rect3D struct
            /// </summary>
            /// <param name="bbox">Rect3D</param>
            /// <returns>Point3D location or Double.Minvalue coords if empty</returns>
            public static Point3D  GetBoundingBoxCenter(Rect3D bbox)
            {
                if (bbox.IsEmpty) return new Point3D(Double.MinValue, double.MinValue, double.MinValue);
                return bbox.rectcenter();
            }

            /// <summary>
            /// Refines the mesh of a Model3D
            /// </summary>
            /// <param name="model">Model3D</param>
            /// <param name="subdivideTimes">Number of times to subdivide</param>
            /// <returns>Subdivided Model3D</returns>
            public static Model3D subdivideSurface(Model3D model, int subdivideTimes)
            {
                Model3D ml = model.Clone();
                GeometryModel3D gm = ml as GeometryModel3D;
                if (gm == null) return null;

                MeshGeometry3D mg = gm.Geometry as MeshGeometry3D;
                if (mg == null) return null;

                LoopSubdivision l = new LoopSubdivision(mg);
                l.Scheme = SubdivisionScheme.Loop;
                l.Subdivide(subdivideTimes);

                gm.Geometry = l.ToMeshGeometry3D();
                //test
                // gm.BackMaterial = gm.Material; 
                return gm as Model3D;
            }
            //public static ModelVisual3D MakeColoredMesh(ModelVisual3D originalModel,List<double> x,List<double> y,List<double> z,List<Color> pointCols)
            //{

            //}
            //public static ModelVisual3D MakeStreamlines(List<double> x,List<double> y,List<double> z,List<double> index, List<double> U)
            //{

            //}

            /// <summary>
            /// Recursively refines the mesh of a ModelVisual3D and children
            /// </summary>
            /// <param name="model">ModelVisual3D</param>
            /// <param name="subdivideTimes">Number of times to subdivide</param>
            /// <returns>Subdivided ModelVisual3D</returns>
            public static ModelVisual3D subdivideSurface(ModelVisual3D model, int subdivideTimes)
            {
                ModelVisual3D subdivided = fullCloneModelVisual3D(model);
                subdivided.Children.Clear();
                if (model.Content != null)
                {
                    Model3D mll = model.Content;
                    mll = subdivideSurface(mll, subdivideTimes);
                    if (mll != null) subdivided.Content = mll;
                }
                foreach (ModelVisual3D mc in model.Children)
                {
                    subdivided.Children.Add(subdivideSurface(mc, subdivideTimes));
                }
                return subdivided;
            }

            /// <summary>
            /// Returns bounding box of object including children
            /// </summary>
            /// <param name="myModel">ModelVisual3D</param>
            /// <returns>Rect3D structure</returns>
            public static Rect3D GetObjectBoundingBox(ModelVisual3D myModel)
            {
                if (myModel == null) return Rect3D.Empty;
                return myModel.getBBRecursive();
            }

            /// <summary>
            /// Creates a sphere 3D object
            /// </summary>
            /// <param name="center">Point3D for center of sphere</param>
            /// <param name="radius">double for radius</param>
            /// <param name="color">System.Windows.Media.Color - not System.Drawing.Color</param>
            /// <returns>ModelVisual3D of sphere</returns>
            public static ModelVisual3D makeSphere(Point3D center, double radius, Color color)
            {

                MeshBuilder meshBuilder = new MeshBuilder();
                meshBuilder.AddSphere(center, radius);
                var mesh = meshBuilder.ToMesh(true);
                GeometryModel3D newModel = new GeometryModel3D();
                newModel.Geometry = mesh;

                newModel.Material = new DiffuseMaterial(new SolidColorBrush(color));
                newModel.BackMaterial = new DiffuseMaterial(new SolidColorBrush(color));

                ModelVisual3D model = new ModelVisual3D();
                model.Content = newModel;



                return model;
            }
            /// <summary>
            /// Makes a pipe
            /// </summary>
            /// <param name="startPoint">Point3D</param>
            /// <param name="endPoint">Point3D</param>
            /// <param name="innerDiameter">double</param>
            /// <param name="outerDiameter">double</param>
            /// <param name="color">System.Windies.Media.Color</param>
            /// <param name="sides">number of sides to render</param>
            /// <returns>ModelVisual3D</returns>
            public static  ModelVisual3D MakePipe(Point3D startPoint, Point3D endPoint, double innerDiameter, double outerDiameter, Color color,int sides)
        {

            ModelVisual3D bv = new ModelVisual3D();
            var meshBuilder = new MeshBuilder(false, false);

            meshBuilder.AddPipe(startPoint, endPoint, innerDiameter,outerDiameter,sides);
            var mesh = meshBuilder.ToMesh(true);
            GeometryModel3D newModel = new GeometryModel3D();
            newModel.Geometry = mesh;
            newModel.Material = new DiffuseMaterial(new SolidColorBrush(color));
            newModel.BackMaterial = new DiffuseMaterial(new SolidColorBrush(color));
                bv.Content = newModel;
            return bv;
        }

            /// <summary>
            /// Creates a box 3D object
            /// </summary>
            /// <param name="center">Point3D for center of box</param>
            /// <param name="xSize">total size x direction</param>
            /// <param name="ySize">total size y direction</param>
            /// <param name="zSize">total size z direction</param>
            /// <param name="color">System.Windows.Media.Color - not System.Drawing.Color</param>
            /// <returns>ModelVisual3D of box</returns>
            public static ModelVisual3D makeBox(Point3D center, double xSize, double ySize, double zSize, Color color)
            {

                MeshBuilder meshBuilder = new MeshBuilder();
                meshBuilder.AddBox(center, xSize, ySize, zSize);
                var mesh = meshBuilder.ToMesh(true);
                GeometryModel3D newModel = new GeometryModel3D();
                newModel.Geometry = mesh;

                newModel.Material = new DiffuseMaterial(new SolidColorBrush(color));
                newModel.BackMaterial = new DiffuseMaterial(new SolidColorBrush(color));

                ModelVisual3D model = new ModelVisual3D();
                model.Content = newModel;



                return model;
            }

            /// <summary>
            /// Gets point positions of Model3D object
            /// </summary>
            /// <param name="m">Model3D</param>
            /// <returns>list of Point3D</returns>
            public static List<Point3D> get3DObjectPoints(Model3D m)
            {
                return m.getMy3DModelPositionsAsPoints();
            }
            /// <summary>
            /// Gets point positions of ModelVisual3D object
            /// </summary>
            /// <param name="m">ModelVisual3D</param>
            /// <returns>list of Point3D</returns>
            public static List<Point3D> get3DObjectPoints(ModelVisual3D m)
            {
                return m.getMy3DVisualModelPositionsAsPoints();
            }
            /// <summary>
            /// Sets point positions of Model3D object
            /// </summary>
            /// <param name="m">Model3D</param>
            /// <param name="points">list of Point3D, must match existing point count</param>
            /// <returns>updated Model3D</returns>
            public static Model3D set3DObjectPoints(Model3D m, List<Point3D> points)
            {
                return m.setMy3DModelPositionsAsPoints(points);
            }
            /// <summary>
            /// Sets point positions of ModelVisual3D object
            /// </summary>
            /// <param name="m">ModelVisual3D</param>
            /// <param name="points">List of Point3D, must match existing point count</param>
            /// <returns>updated ModelVisual3D</returns>
            public static ModelVisual3D set3DObjectPoints(ModelVisual3D m, List<Point3D> points)
            {
                return m.setMy3DVisualModelPositionsAsPoints(points);
            }
            /// <summary>
            /// Exports visual custom renderer to image file.Visual should not belong to a viewport (clone it first).
            /// </summary>
            /// <param name="mv">ModelVisual3D</param>
            /// <param name="direction">Z only for now</param>
            /// <param name="name">file root name</param>
            /// <returns>image file path</returns>
            public static string renderModelVisual3D(ModelVisual3D mv, string direction, string name)
            {
                string ret = "nothing";



                try
                {


                    string myPath = "";
                    bool cand = true;
                    if (System.IO.File.Exists(System.IO.Path.GetTempPath() + name + PUPPIGUISettings.NCRImageSaveExtension))
                    {

                        try
                        {
                            System.IO.File.Delete(System.IO.Path.GetTempPath() + name + PUPPIGUISettings.NCRImageSaveExtension);
                        }
                        catch
                        {
                            ret = "cannotdelete";
                            return ret;
                        }
                    }
                    myPath = System.IO.Path.GetTempPath() + name + PUPPIGUISettings.NCRImageSaveExtension;

                    //ThreadStart userDelegate = delegate()
                    //{
                    //    //var vp = new Viewport3D { Camera = CameraHelper.CreateDefaultCamera(), Width = 1280, Height = 720 };
                    //    //vp.Children.Add(new DefaultLights());
                    //    //vp.Children.Add(nmv);

                    //    //BitmapExporter e = new BitmapExporter(myPath); 
                    //    //e.Export(vp);

                    ModelVisual3D mvf = mv;//.cloneMyVisual();
                    //reset overall transform
                    mvf.Transform = Transform3D.Identity;
                    HelixViewport3D useh = new HelixViewport3D();
                    Rect3D rr = mvf.getBBRecursive();
                    int iWidth = Convert.ToInt16((double)PUPPIGUISettings.nodeImagePixelPerUnit * rr.SizeX);
                    int iHeight = Convert.ToInt16((double)PUPPIGUISettings.nodeImagePixelPerUnit * rr.SizeY);

                    //camera position

                    double dist = rr.SizeX / 2 / Math.Tan(Math.PI / 6);

                    useh.Camera = new OrthographicCamera(new Point3D(rr.X + rr.SizeX / 2, rr.Y + rr.SizeY / 2, rr.Z + rr.SizeZ + dist), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), rr.SizeX);
                    //useh.Camera.NearPlaneDistance = 0;

                    useh.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    useh.ShowCoordinateSystem = false;
                    useh.ShowViewCube = false;
                    useh.Children.Add(new DefaultLights());
                    useh.Children.Add(mvf);
                    HelixToolkit.Wpf.Viewport3DHelper.ResizeAndArrange(useh.Viewport, iWidth, iHeight);
                    //useh.ZoomExtents();
                    useh.Export(myPath);
                    // };

                    //Camera c=new PerspectiveCamera(new Point3D(0,0,100),new Vector3D(0,0,-1), new Vector3D(0,1,0),60  );  
                    //BitmapExporter bl = new BitmapExporter(myPath);
                    //var vp = new Viewport3D { Camera = c , Width = 200, Height = 200 };
                    //vp.Children.Add(new DefaultLights());

                    //vp.Children.Add(mv);
                    //HelixToolkit.Wpf.Viewport3DHelper.ResizeAndArrange(vp, 1280, 720);
                    //CameraHelper.ZoomExtents( 
                    ////HelixToolkit.Wpf.Viewport3DHelper.Export 
                    //bl.Export(vp); 
                    //Thread thread = new Thread(
                    // delegate()
                    // {

                    //     userDelegate.Invoke();
                    // });
                    //thread.SetApartmentState(ApartmentState.STA);

                    //thread.Start();
                    //thread.Join();
                    ret = System.IO.Path.GetTempPath() + name + PUPPIGUISettings.NCRImageSaveExtension;
                    if (!System.IO.File.Exists(ret)) ret = "failed";


                    return ret;

                    //RenderTargetBitmap bmp = new RenderTargetBitmap(200, 200, 96, 96, PixelFormats.Pbgra32);
                    //PngBitmapEncoder png = new PngBitmapEncoder();
                    //Viewport3D myViewport3d = new Viewport3D();
                    //PerspectiveCamera pc = new PerspectiveCamera();
                    //pc.Position = new Point3D(0, 0, 10);
                    //pc.LookDirection = new Vector3D(0, 0, -1);
                    //myViewport3d.Camera = pc;  
                    //myViewport3d.Children.Add(mv);

                    //myViewport3d.Measure(new Size(200, 200));
                    //myViewport3d.Arrange(new Rect(0, 0, 200, 200));
                    //myViewport3d.InvalidateVisual();

                    //bmp.Render(myViewport3d);


                }
                catch (Exception exy)
                {
                    return "error";
                }
            }

            /// <summary>
            /// Exports model to OBJ file(s).Visual should not belong to a viewport (clone it first).
            /// </summary>
            /// <param name="mv">ModelVisual3D</param>
            /// <param name="name">file root name</param>
            /// <returns>OBJ folder path</returns>
            public static string saveModelVisual3DAsOBJ(ModelVisual3D mv, string name)
            {




                string ret = "nothing";



                try
                {


                    string myPath = "";

                    if (System.IO.Directory.Exists(System.IO.Path.GetTempPath() + name + "folder"))
                    {

                        try
                        {
                            System.IO.Directory.Delete(System.IO.Path.GetTempPath() + name + "folder", true);
                        }
                        catch
                        {
                            ret = "cannotdelete";
                            return ret;
                        }
                    }
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetTempPath() + name + "folder");
                    myPath = System.IO.Path.GetTempPath() + name + "folder\\" + name + ".obj";


                    ModelVisual3D mvf = mv;
                    //reset overall transform
                    mvf.Transform = Transform3D.Identity;
                    HelixViewport3D useh = new HelixViewport3D();

                    //  Rect3D rr = mvf.getBBRecursive();



                    useh.ShowCoordinateSystem = false;
                    useh.ShowViewCube = false;

                    useh.Children.Add(mvf);

                    useh.Export(myPath);

                    ret = System.IO.Path.GetTempPath() + name + "folder";
                    if (!System.IO.File.Exists(ret)) ret = "failed";


                    return ret;




                }
                catch (Exception exy)
                {
                    return "error";
                }
            }


            /// <summary>
            /// Exports model to binary STL file.Returns file path if successful or error message
            /// </summary>
            /// <param name="mv">ModelVisual3D</param>
            /// <param name="fname">file name</param>
            public static string saveModelVisual3DAsSTLB(ModelVisual3D mv, string fname)
            {








                try
                {


                    byte[] stu = helperfunctions.writegeomSTLrecursivelyBS(mv.cloneVisualAndChildren());
                    System.IO.File.WriteAllBytes(fname, stu);
                    return fname;



                }
                catch (Exception exy)
                {
                    return "error";
                }
            }

            /// <summary>
            /// Gets the nearest point to argument on a list of 3D models
            /// </summary>
            /// <param name="nPt">a Point3D</param>
            /// <param name="geoModels">list of GeometryModel3D</param>
            /// <returns>nearest Point or original point if not found</returns>
            public static Point3D getNearestPointOnModel(Point3D nPt, List<GeometryModel3D> geoModels)
            {
                Point3D pi = new Point3D(Double.MinValue, double.MinValue, double.MinValue);
                double mydist = Double.MaxValue;
                foreach (GeometryModel3D gao in geoModels)
                {
                    MeshGeometry3D me = gao.Geometry as MeshGeometry3D;

                    //triangles
                    for (int trindex = 0; trindex < me.TriangleIndices.Count; trindex += 3)
                    {
                        Point3D p1 = me.Positions[me.TriangleIndices[trindex]];
                        Point3D p2 = me.Positions[me.TriangleIndices[trindex + 1]];
                        Point3D p3 = me.Positions[me.TriangleIndices[trindex + 2]];
                        if (p1.DistanceTo(nPt) < mydist)
                        {
                            pi = p1;
                            mydist = p1.DistanceTo(nPt);
                        }
                        if (p2.DistanceTo(nPt) < mydist)
                        {
                            pi = p2;
                            mydist = p2.DistanceTo(nPt);
                        }
                        if (p3.DistanceTo(nPt) < mydist)
                        {
                            pi = p3;
                            mydist = p3.DistanceTo(nPt);
                        }
                        Vector3D v0 = p3 - p1;
                        Vector3D v1 = p2 - p1;
                        Vector3D v3 = p3 - p2;
                        //to go past degenerate triangles
                        if (v0.Length > PUPPIGUISettings.geomWeldToler && v1.Length > PUPPIGUISettings.geomWeldToler && v3.Length > PUPPIGUISettings.geomWeldToler)
                        {




                            try
                            {
                                //barycentric method
                                HelperClasses.PUPPIPlane3D pplane = new HelperClasses.PUPPIPlane3D(p1, p2, p3);
                                //plane projection
                                object projpo = pplane.projectPoint(nPt);
                                if (projpo != null)
                                {
                                    Point3D projp = (Point3D)projpo;
                                    Vector3D v2 = projp - p1;
                                    double dot00 = Vector3D.DotProduct(v0, v0);
                                    double dot01 = Vector3D.DotProduct(v0, v1);
                                    double dot02 = Vector3D.DotProduct(v0, v2);
                                    double dot11 = Vector3D.DotProduct(v1, v1);
                                    double dot12 = Vector3D.DotProduct(v1, v2);

                                    double invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
                                    double u = (dot11 * dot02 - dot01 * dot12) * invDenom;
                                    double v = (dot00 * dot12 - dot01 * dot02) * invDenom;


                                    if ((u >= 0) && (v >= 0) && (u + v < 1))
                                    {
                                        //projection in
                                        if (projp.DistanceTo(nPt) < mydist)
                                        {
                                            pi = projp;
                                            mydist = projp.DistanceTo(nPt);
                                        }

                                    }
                                }

                            }
                            catch
                            {

                            }
                        }
                    }
                }
                if (pi.X != Double.MinValue && pi.Y != Double.MinValue && pi.Z != Double.MinValue && mydist != Double.MaxValue)
                {
                    return pi;
                }
                else
                {
                    return nPt;
                }
            }

            /// <summary>
            /// Converts a 2D sketch to a 3D Polyline
            /// </summary>
            /// <param name="sketch">PUPPISketch2D</param>
            /// <param name="plane">PUPPIPlane3D</param>
            /// <param name="xAxis">Vector3D</param>
            /// <returns>PUPPIPolyline3D</returns>
            public static PUPPIPolyLine3D sketchToPolyLine3D(PUPPISketch2D sketch, PUPPIPlane3D plane, Vector3D xAxis)
            {
                List<Point3D> npts = new List<Point3D>();
                ArrayList aa = sketch.getPointList();
                for (int i = 0; i < aa.Count; i++)
                {
                    Point p = (Point)aa[i];
                    npts.Add(helperfunctions.moveskpointtoplane(p, plane.theplane, xAxis));
                }
                return new PUPPIPolyLine3D(npts);

            }
            /// <summary>
            /// Gets the normal of three noncollinear points.RHR & clockwise ordering assumed
            /// </summary>
            /// <param name="point1">First Point3D</param>
            /// <param name="point2">Second Point3D</param>
            /// <param name="point3">Third Point3D</param>
            /// <returns>Normal Vector3D</returns>
            public static Vector3D pointsNormal(Point3D point1, Point3D point2, Point3D point3)
            {
                Vector3D v1 = point1 - point3;
                Vector3D v2 = point2 - point3;
                Vector3D v3 = Vector3D.CrossProduct(v1, v2);
                v3.Normalize();
                return v3;
            }

            /// <summary>
            /// Gets the centroid point between points provided as collection
            /// </summary>
            /// <param name="pointsList">Collection with Point3D objects</param>
            /// <returns>Point3D</returns>
            public static Point3D getPointsCentroid(IEnumerable pointsList)
            {
                ArrayList aa = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(pointsList);
                var plist = aa.Cast<Point3D>().ToList();
                return helperfunctions.getCentroid(plist);

            }

            /// <summary>
            /// Gets the nearest point from pointsList to myPoint
            /// </summary>
            /// <param name="pointsList">collection of Point3D</param>
            /// <param name="myPoint">another Point3D</param>
            /// <returns>nearest Point3D</returns>
            public static Point3D getNearestPoint(IEnumerable pointsList, Point3D myPoint)
            {
                ArrayList aa = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(pointsList);
                var plist = aa.Cast<Point3D>().ToList();
                Point3D ret = new Point3D(double.MinValue, double.MinValue, double.MinValue);
                foreach (Point3D ppp in plist)
                {
                    if (ppp.DistanceTo(myPoint) < ret.DistanceTo(myPoint))
                    {
                        ret = ppp;
                    }
                }
                return ret;

            }

            /// <summary>
            /// Rotates a 3D vector in 3D space
            /// </summary>
            /// <param name="toRotate">Vector3D to rotate</param>
            /// <param name="axis">Rotation axis Vector3D</param>
            /// <param name="angleDegrees">Rotation angle in degrees</param>
            /// <returns>Rotated vector</returns>
            public static Vector3D rotateVector3D(Vector3D toRotate, Vector3D axis, double angleDegrees)
            {

                Vector3D rotated = new Vector3D();

                Matrix3D m = Matrix3D.Identity;
                Quaternion q = new Quaternion(axis, angleDegrees);
                m.Rotate(q);

                rotated = m.Transform(toRotate);


                return rotated;
            }
            /// <summary>
            /// Rotates a Point3D around a center/ axis
            /// </summary>
            /// <param name="toRotate">Point3D to rotate</param>
            /// <param name="rotationCenter">Point3D to rotate around</param>
            /// <param name="axis">Vector3D rotation axis</param>
            /// <param name="angleDegrees">rotation angle in degrees</param>
            /// <returns>rotated Point3D</returns>
            public static Point3D rotatePoint3D(Point3D toRotate, Point3D rotationCenter, Vector3D axis, double angleDegrees)
            {
                Vector3D rV = toRotate - rotationCenter;
                Vector3D arV = rotateVector3D(rV, axis, angleDegrees);
                return rotationCenter + arV;

            }

            /// <summary>
            /// returns a point between or outside on the line between point1 and point2 
            /// </summary>
            /// <param name="point1">first point</param>
            /// <param name="point2">second point</param>
            /// <param name="ratio">ratio of distance between point1 and point2. can also be negative or greater than 1 </param>
            /// <returns>Point3D</returns>
            public static Point3D getPointBetweenPoints(Point3D point1, Point3D point2, double ratio)
            {
                double dist = point1.DistanceTo(point2);
                Vector3D vo = point2 - point1;
                vo.Normalize();
                return point1 + vo * ratio * dist;

            }

            /// <summary>
            /// Reorders points counterclockwise. Assumes they are roughly in the same plane perpendicular to normal vector
            /// </summary>
            /// <param name="myPoints">Collection of Point3D</param>
            /// <param name="normal">Normal to plane to order points in</param>
            /// <returns>List with reordered points</returns>
            public static List<Point3D> orderPointsCounterClockwise(IEnumerable myPoints, Vector3D normal)
            {
                List<Point3D> retlist = new List<Point3D>();
                ArrayList makeme = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(myPoints);

                try
                {
                    for (int i = 0; i < makeme.Count; i++)
                    {

                        Point3D pp = (Point3D)(makeme[i]);
                        retlist.Add(pp);
                    }
                }
                catch
                {

                }

                PUPPICAD.HelperClasses.helperfunctions.reorder3Dpoints(retlist, normal);
                return retlist;
            }


            /// <summary>
            /// Reorders points clockwise. Assumes they are roughly in the same plane perpendicular to normal vector
            /// </summary>
            /// <param name="myPoints">Collection of Point3D</param>
            /// <param name="normal">Normal to plane to order points in</param>
            /// <returns>List with reordered points</returns>
            public static List<Point3D> orderPointsClockwise(IEnumerable myPoints, Vector3D normal)
            {
                List<Point3D> retlist = new List<Point3D>();
                ArrayList makeme = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(myPoints);

                try
                {
                    for (int i = 0; i < makeme.Count; i++)
                    {

                        Point3D pp = (Point3D)(makeme[i]);
                        retlist.Add(pp);
                    }
                }
                catch
                {

                }

                PUPPICAD.HelperClasses.helperfunctions.reorder3Dpoints(retlist, normal);
                retlist.Reverse();
                return retlist;
            }
            /// <summary>
            /// Clones a 3d model including children (recursive) and transforms.
            /// </summary>
            /// <param name="originalModel">ModelVisual3D</param>
            /// <returns>clone ModelVisual3D</returns>
            public static ModelVisual3D fullCloneModelVisual3D(ModelVisual3D originalModel)
            {

                ModelVisual3D newModel = new ModelVisual3D();
                newModel = originalModel.cloneVisualAndChildren();
                return newModel;
            }


            /// <summary>
            /// Moves a model by creating a copy at displaced position. No transforms involved.
            /// </summary>
            /// <param name="originalModel">ModelVisual3D object</param>
            /// <param name="deltax">X offset</param>
            /// <param name="deltay">Y offset</param>
            /// <param name="deltaz">Z offset</param>
            /// <returns>Displaced ModelVisual3D object</returns>
            public static ModelVisual3D moveModelVisual3D(ModelVisual3D originalModel, double deltax, double deltay, double deltaz)
            {
                ModelVisual3D newModel = new ModelVisual3D();
                newModel = originalModel.translateVisualAndChildren(deltax, deltay, deltaz);
                return newModel;
            }
            /// <summary>
            /// Gets rid of duplicate points
            /// </summary>
            /// <param name="toprune">List of Point3D which gets edited</param>
            /// <returns>Updated points list</returns>
            public static List<Point3D> removeDuplicatePoints(List<Point3D> pointsList)
            {
                List<Point3D> np = new List<Point3D>(pointsList);
                helperfunctions.pointsList3dPrune(np, PUPPIGUISettings.geomWeldToler);
                return np;
            }
            /// <summary>
            /// Calculates the distance between two 3D points
            /// </summary>
            /// <param name="p1">first Point3D</param>
            /// <param name="p2">second Point3D</param>
            /// <returns>distance</returns>
            public static double getDistBetweenPoints(Point3D p1, Point3D p2)
            {
                return p1.DistanceTo(p2);
            }
            /// <summary>
            /// Projects one vector on another in 3D
            /// </summary>
            /// <param name="toProject">Vector3D to project</param>
            /// <param name="projectOn">Vector3D projected on</param>
            /// <returns>Projection Vector</returns>
            public static Vector3D projectVectorOn(Vector3D toProject, Vector3D projectOn)
            {
                return toProject.project(projectOn);
            }
            /// <summary>
            /// Combines two transforms
            /// </summary>
            /// <param name="T1">Transform3D or Transform3DGroup</param>
            /// <param name="T2">Transform3D or Transform3DGroup</param>
            /// <returns>Transform3DGroup</returns>
            public static Transform3DGroup combineTransforms(object T1, object T2)
            {
                return helperfunctions.transformCombiner(T1, T2);
            }
            /// <summary>
            /// Encodes the model's and children's transforms into their point positions.
            /// </summary>
            /// <param name="mv">ModelVisual3D</param>
            /// <returns>ModelVisual3D</returns>
            public static ModelVisual3D hardcodeTransforms(ModelVisual3D mv)
            {

                return helperfunctions.htr(mv, new Transform3DGroup());
            }
            /// <summary>
            /// Generates a ModelVisual3D from STL string representations
            /// </summary>
            /// <param name="STLString">ASCII STL</param>
            /// <param name="separator">line separator</param>
            /// <returns>ModelVisual3D</returns>
            public static ModelVisual3D ReadSTLFromString(string STLString, string separator)
            {
                string[] sep = new string[1];
                sep[0] = separator;
                string[] myLines = STLString.Split(sep, StringSplitOptions.None);
                return helperfunctions.readgeomSTLstring(myLines.ToList<string>());

            }
            /// <summary>
            /// Changes color of ModelVisual3D and children
            /// </summary>
            /// <param name="origmodel">ModelVisual3D</param>
            /// <param name="redRatio">0..1</param>
            /// <param name="greenRatio">0..1</param>
            /// <param name="blueRatio">0..1</param>
            /// <param name="alphaRatio">0..1</param>
            /// <returns>new ModelVisual3D</returns>
            public static ModelVisual3D SetObjectColor(ModelVisual3D origmodel, double redRatio, double greenRatio, double blueRatio, double alphaRatio)
            {
                redRatio = Math.Max(Math.Min(redRatio, 1.0), 0.0);
                greenRatio = Math.Max(Math.Min(greenRatio, 1.0), 0.0);
                blueRatio = Math.Max(Math.Min(blueRatio, 1.0), 0.0);
                alphaRatio = Math.Max(Math.Min(alphaRatio, 1.0), 0.0);
                Color cc = Color.FromArgb((byte)(alphaRatio * 255), (byte)(redRatio * 255), (byte)(greenRatio * 255), (byte)(blueRatio * 255));
                ModelVisual3D malao = origmodel.cloneVisualAndChildren();
                malao.colorVisualAndChildren(cc);
                return malao;
            }

        }

        //transforms between our regular coordinate system and another orthogonal defined by two vectors, for x and y

        //internal class CoordinateTransform
        //{
        //   internal  MatrixTransform3D whattodo;
        //    internal CoordinateTransform(Vector3D u,Vector3D v,Point3D neworigin)
        //    {
        //        Vector3D w = Vector3D.CrossProduct(u, v);
        //        Matrix3D mat = new Matrix3D(u.X, u.Y, u.Z, 0, v.X, v.Y, v.Z, 0, w.X, w.Y, w.Z, 0,neworigin.X,neworigin.Y,neworigin.Z,1);
        //        whattodo = new MatrixTransform3D(mat);
        //    }

        //}

        internal class helperfunctions
        {


            //internal static Model3D colormymesh(Model3D origmesh, List<double> x, List<double> y, List<double> z, List<Color> pointCols)
            //{
            //    GeometryModel3D gmesh = origmesh.Clone() as GeometryModel3D;
            //    if (gmesh == null) return origmesh.Clone();
            //    MeshGeometry3D myg = gmesh.Geometry as MeshGeometry3D;
            //    if (myg == null) return gmesh as Model3D;

            //}

            internal static void renderModelVisual3DThread(List<Model3D> frozenModels, string direction, string name)
            {




                try
                {


                    string myPath = "";
                    bool cand = true;
                    if (System.IO.File.Exists(System.IO.Path.GetTempPath() + name + PUPPIGUISettings.NCRImageSaveExtension))
                    {

                        try
                        {
                            System.IO.File.Delete(System.IO.Path.GetTempPath() + name + PUPPIGUISettings.NCRImageSaveExtension);
                        }
                        catch
                        {

                            return;
                        }
                    }
                    myPath = System.IO.Path.GetTempPath() + name + PUPPIGUISettings.NCRImageSaveExtension;



                    ModelVisual3D mvf = new ModelVisual3D();
                    foreach (Model3D mf in frozenModels)
                    {
                        ModelVisual3D mcf = new ModelVisual3D();
                        mcf.Content = mf;
                        mvf.Children.Add(mcf);
                    }
                    HelixViewport3D useh = new HelixViewport3D();
                    Rect3D rr = mvf.getBBRecursive();
                    int iWidth = Convert.ToInt16((double)PUPPIGUISettings.nodeImagePixelPerUnit * rr.SizeX);
                    int iHeight = Convert.ToInt16((double)PUPPIGUISettings.nodeImagePixelPerUnit * rr.SizeY);

                    //camera position

                    double dist = rr.SizeX / 2 / Math.Tan(Math.PI / 6);

                    useh.Camera = new OrthographicCamera(new Point3D(rr.X + rr.SizeX / 2, rr.Y + rr.SizeY / 2, rr.Z + rr.SizeZ + dist), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), rr.SizeX);
                    //useh.Camera.NearPlaneDistance = 0;

                    useh.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    useh.ShowCoordinateSystem = false;
                    useh.ShowViewCube = false;
                    useh.Children.Add(new DefaultLights());
                    useh.Children.Add(mvf);
                    HelixToolkit.Wpf.Viewport3DHelper.ResizeAndArrange(useh.Viewport, iWidth, iHeight);
                    //useh.ZoomExtents();
                    useh.Export(myPath);

                    string ret = System.IO.Path.GetTempPath() + name + PUPPIGUISettings.NCRImageSaveExtension;




                }
                catch (Exception exy)
                {
                    return;
                }
            }

            internal static ModelVisual3D htr(ModelVisual3D mv3d, Transform3DGroup tg)
            {
                ModelVisual3D nmv = new ModelVisual3D();
                if (mv3d.Transform != null)
                    tg.Children.Add(mv3d.Transform);
                try
                {
                    if (mv3d.Content != null)
                    {
                        Transform3DGroup ats = tg.Clone();
                        GeometryModel3D newModel = mv3d.Content.Clone() as GeometryModel3D;

                        if (newModel != null)
                        {
                            Matrix3D tm;
                            if (newModel.Transform != null)
                            {
                                ats.Children.Add(newModel.Transform);

                            }
                            tm = ats.Value;

                            MeshGeometry3D mtd = newModel.Geometry as MeshGeometry3D;
                            if (mtd != null)
                            {

                                Point3DCollection npos = new Point3DCollection();
                                for (int tri = 0; tri < mtd.Positions.Count; tri++)
                                {

                                    Point3D pos1 = mtd.Positions[tri] * tm;
                                    npos.Add(pos1);


                                }
                                mtd.Positions = npos;
                            }
                            newModel.Geometry = mtd;
                            nmv.Content = newModel;

                        }
                    }
                    foreach (ModelVisual3D mv1 in mv3d.Children)
                    {
                        nmv.Children.Add(htr(mv1, tg));
                    }
                }
                catch
                {
                    return new ModelVisual3D();
                }
                return nmv;
            }

            internal static ModelVisual3D pillFolly(List<Point3D> mypoints, Material mat)
            {
                HelperClasses.helperfunctions.pointsList3dPrune(mypoints, PUPPIGUISettings.geomWeldToler);

                MeshBuilder meshBuilder = new MeshBuilder(false, false);

                //meshBuilder.AddPolygon(mypoints);


                //var mesh = meshBuilder.ToMesh(true);


                if (mypoints.Count < 3)
                {

                    return null;
                }
                Point3D cento = HelperClasses.utilities.getPointsCentroid(mypoints);
                Vector3D mynor = HelperClasses.utilities.pointsNormal(cento, mypoints[0], mypoints[1]);
                if (mynor.Length == 0)
                {
                    mynor = new Vector3D(0, 0, 1);
                }




                List<int> indices = new List<int>();
                for (int i = 0; i < mypoints.Count; i++)
                {
                    meshBuilder.AddNode(mypoints[i], mynor, new Point(0, 0));
                    indices.Add(i);
                }


                meshBuilder.AddPolygonByCuttingEars(indices);

                var mesh = meshBuilder.ToMesh();
                GeometryModel3D polygonsModel = new GeometryModel3D();

                polygonsModel.Geometry = mesh;

                polygonsModel.Material = mat;
                //back material so we don't worry about normals for now
                polygonsModel.BackMaterial = mat;
                ModelVisual3D model = new ModelVisual3D();
                model.Content = polygonsModel;
                return model;
            }

            internal static List<Point3D> getM3DContSingle(Model3D mao, Point3D position, Vector3D normal)
            {
                normal.Normalize();
                List<Point3D> cpoints = new List<Point3D>();
                GeometryModel3D gao = mao as GeometryModel3D;
                if (gao != null)
                {

                    var segments = MeshGeometryHelper.GetContourSegments(gao.Geometry as MeshGeometry3D, position, normal).ToList();
                    //List<Point3D> mycpts = new List<Point3D>();
                    foreach (var contour in MeshGeometryHelper.CombineSegments(segments, PUPPIGUISettings.geomWeldToler).ToList())
                    {

                        cpoints.AddRange(contour);
                        break;

                    }

                }
                return cpoints;
            }

            internal static List<List<Point3D>> getM3DConts(Model3D mao, Point3D position, Vector3D normal)
            {
                List<List<Point3D>> cpoints = new List<List<Point3D>>();
                normal.Normalize();
                GeometryModel3D gao = mao as GeometryModel3D;
                if (gao != null)
                {

                    var segments = MeshGeometryHelper.GetContourSegments(gao.Geometry as MeshGeometry3D, position, normal).ToList();
                    //List<Point3D> mycpts = new List<Point3D>();
                    foreach (var contour in MeshGeometryHelper.CombineSegments(segments, PUPPIGUISettings.geomWeldToler).ToList())
                    {


                        List<Point3D> mp = new List<Point3D>(contour);
                        cpoints.Add(mp);
                        //break;

                    }

                }
                return cpoints;
            }


            internal static Point3D closestOnLineBetween(Point3D a, Point3D b, Point3D p)
            {

                Vector3D ap = p - a;
                Vector3D ab = b - a;
                Point3D result = a + Vector3D.DotProduct(ap, ab) / Vector3D.DotProduct(ab, ab) * ab;
                return result;
            }


            internal static bool isBetweenPts(Point3D a, Point3D b, Point3D p)
            {
                return Math.Abs(a.DistanceTo(b) - a.DistanceTo(p) - b.DistanceTo(p)) < PUPPIGUISettings.geomWeldToler;
            }

            //export contents of view to ascii STL
            internal static bool exportSTLA(Viewport3D h, string filename)
            {   //write all the lines
                ArrayList lines = new ArrayList();
                foreach (ModelVisual3D mv3d in h.Children)
                {

                    if (mv3d.Content != null)
                    {
                        if (mv3d.Content.GetType() != typeof(PointLight))
                        {
                            writegeomSTLrecursivelyWT(mv3d, lines, "solid_" + h.Children.IndexOf(mv3d).ToString(), new Transform3DGroup());
                        }
                    }
                    else
                    {
                        writegeomSTLrecursivelyWT(mv3d, lines, "solid_" + h.Children.IndexOf(mv3d).ToString(), new Transform3DGroup());
                    }
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
                {
                    foreach (string line in lines)
                    {


                        file.WriteLine(line);

                    }
                }
                return false;
            }

            internal static byte[] writegeomSTLrecursivelyBS(ModelVisual3D mv3d)
            {
                List<byte> stuff = new List<byte>();
                byte[] Header = new byte[80];

                int sc = 0;
                int tc = 0;
                writegeomSTLrecursivelyB(mv3d, stuff, ref sc, ref tc);

                stuff.InsertRange(0, BitConverter.GetBytes(tc));
                stuff.InsertRange(0, Header);
                return stuff.ToArray();
            }

            internal static void writegeomSTLrecursivelyB(ModelVisual3D mv3d, List<byte> stuff, ref int solidcount, ref int tricount)
            {
                try
                {

                    GeometryModel3D newModel = mv3d.Content as GeometryModel3D;

                    if (newModel != null)
                    {
                        MeshGeometry3D mtd = newModel.Geometry as MeshGeometry3D;
                        if (mtd != null)
                        {

                            int nt = (int)(mtd.TriangleIndices.Count / 3);

                            for (int tri = 0; tri < mtd.TriangleIndices.Count / 3; tri++)
                            {

                                Point3D pos1 = mtd.Positions[mtd.TriangleIndices[tri * 3]];

                                Point3D pos2 = mtd.Positions[mtd.TriangleIndices[tri * 3 + 1]];
                                Point3D pos3 = mtd.Positions[mtd.TriangleIndices[tri * 3 + 2]];
                                //get the normal to point outwards that is to  nearest corner
                                Vector3D rawnormal = Vector3D.CrossProduct(Point3D.Subtract(pos1, pos2), Point3D.Subtract(pos3, pos2));
                                Point3D faceavg = (new Point3D((pos1.X + pos2.X + pos3.X) / 3, (pos1.Y + pos2.Y + pos3.Y) / 3, (pos1.Z + pos2.Z + pos3.Z) / 3));
                                Vector3D outward = Point3D.Subtract(faceavg.nearestcorner(mtd.Bounds), faceavg);
                                Vector3D outnormal = outward.project(rawnormal);
                                outnormal.Normalize();
                                if (outnormal.X.ToString() == "NaN") outnormal.X = 1;
                                if (outnormal.Y.ToString() == "NaN") outnormal.Y = 1;
                                if (outnormal.Z.ToString() == "NaN") outnormal.Z = 1;
                                float[] fnb = new float[3];
                                fnb[0] = (float)outnormal.X;
                                fnb[1] = (float)outnormal.Y;
                                fnb[2] = (float)outnormal.Z;
                                float[] pos1b = new float[3];
                                pos1b[0] = (float)pos1.X;
                                pos1b[1] = (float)pos1.Y;
                                pos1b[2] = (float)pos1.Z;

                                float[] pos2b = new float[3];
                                pos2b[0] = (float)pos2.X;
                                pos2b[1] = (float)pos2.Y;
                                pos2b[2] = (float)pos2.Z;

                                float[] pos3b = new float[3];
                                pos3b[0] = (float)pos3.X;
                                pos3b[1] = (float)pos3.Y;
                                pos3b[2] = (float)pos3.Z;

                                stuff.AddRange(BitConverter.GetBytes(fnb[0]));
                                stuff.AddRange(BitConverter.GetBytes(fnb[1]));
                                stuff.AddRange(BitConverter.GetBytes(fnb[2]));

                                stuff.AddRange(BitConverter.GetBytes(pos1b[0]));
                                stuff.AddRange(BitConverter.GetBytes(pos1b[1]));
                                stuff.AddRange(BitConverter.GetBytes(pos1b[2]));

                                stuff.AddRange(BitConverter.GetBytes(pos2b[0]));
                                stuff.AddRange(BitConverter.GetBytes(pos2b[1]));
                                stuff.AddRange(BitConverter.GetBytes(pos2b[2]));

                                stuff.AddRange(BitConverter.GetBytes(pos3b[0]));
                                stuff.AddRange(BitConverter.GetBytes(pos3b[1]));
                                stuff.AddRange(BitConverter.GetBytes(pos3b[2]));

                                UInt16 bc = (UInt16)solidcount;
                                stuff.AddRange(BitConverter.GetBytes(bc));

                            }


                            solidcount++;
                            tricount += (int)nt;

                        }
                    }
                    foreach (ModelVisual3D ml in mv3d.Children)
                    {
                        writegeomSTLrecursivelyB(ml, stuff, ref solidcount, ref tricount);
                    }
                }
                catch
                {

                }


            }

            //writes content and children recursively in string line array
            internal static void writegeomSTLrecursively(ModelVisual3D mv3d, ArrayList lines, string solname)
            {
                //first try without children
                try
                {

                    GeometryModel3D newModel = mv3d.Content as GeometryModel3D;

                    if (newModel != null)
                    {
                        MeshGeometry3D mtd = newModel.Geometry as MeshGeometry3D;
                        if (mtd != null)
                        {
                            lines.Add("solid " + solname);
                            //write the mesh

                            for (int tri = 0; tri < mtd.TriangleIndices.Count / 3; tri++)
                            {

                                Point3D pos1 = mtd.Positions[mtd.TriangleIndices[tri * 3]];

                                Point3D pos2 = mtd.Positions[mtd.TriangleIndices[tri * 3 + 1]];
                                Point3D pos3 = mtd.Positions[mtd.TriangleIndices[tri * 3 + 2]];
                                //get the normal to point outwards that is to  nearest corner
                                Vector3D rawnormal = Vector3D.CrossProduct(Point3D.Subtract(pos1, pos2), Point3D.Subtract(pos3, pos2));
                                Point3D faceavg = (new Point3D((pos1.X + pos2.X + pos3.X) / 3, (pos1.Y + pos2.Y + pos3.Y) / 3, (pos1.Z + pos2.Z + pos3.Z) / 3));
                                Vector3D outward = Point3D.Subtract(faceavg.nearestcorner(mtd.Bounds), faceavg);
                                Vector3D outnormal = outward.project(rawnormal);
                                outnormal.Normalize();
                                if (outnormal.X.ToString() == "NaN") outnormal.X = 1;
                                if (outnormal.Y.ToString() == "NaN") outnormal.Y = 1;
                                if (outnormal.Z.ToString() == "NaN") outnormal.Z = 1;
                                lines.Add("facet normal " + outnormal.X.ToString() + " " + outnormal.Y.ToString() + " " + outnormal.Z.ToString());
                                lines.Add("    outer loop");
                                lines.Add("        vertex " + pos1.X.ToString() + " " + pos1.Y.ToString() + " " + pos1.Z.ToString());
                                lines.Add("        vertex " + pos2.X.ToString() + " " + pos2.Y.ToString() + " " + pos2.Z.ToString());
                                lines.Add("        vertex " + pos3.X.ToString() + " " + pos3.Y.ToString() + " " + pos3.Z.ToString());
                                lines.Add("    endloop");
                                lines.Add("endfacet");

                            }
                            lines.Add("endsolid " + solname);
                        }

                    }
                    foreach (ModelVisual3D mv1 in mv3d.Children)
                    {
                        writegeomSTLrecursively(mv1, lines, solname + mv3d.Children.IndexOf(mv1));
                    }
                }
                catch
                {

                }

            }

            internal static ModelVisual3D readgeomSTLstring(List<string> lines)
            {
                ModelVisual3D remade = new ModelVisual3D();
                int ic = 0;
                while (ic < lines.Count)
                {
                    if (lines[ic].ToLower().Contains("solid "))
                    {

                        Point3DCollection pos = new Point3DCollection();
                        Int32Collection tris = new Int32Collection();
                        string sname = lines[ic].Replace("solid ", "").Replace(" ", "");
                        SolidColorBrush mySoBro = Brushes.Gray;
                        try
                        {
                            if (sname.Contains("[Color]"))
                            {
                                string[] splitter = { "[Color]" };
                                string[] splitted = sname.Split(splitter, StringSplitOptions.None);
                                sname = splitted[0];
                                string[] csplitter = { "__" };
                                string[] csplitted = splitted[1].Split(csplitter, StringSplitOptions.None);
                                byte R = Convert.ToByte(csplitted[0]);
                                byte G = Convert.ToByte(csplitted[1]);
                                byte B = Convert.ToByte(csplitted[2]);
                                byte A = Convert.ToByte(csplitted[3]);
                                mySoBro = new SolidColorBrush(Color.FromArgb(A, R, G, B));

                            }
                        }
                        catch
                        {
                            mySoBro = Brushes.Gray;
                        }
                        while (lines[ic].ToLower().Contains("endsolid") == false)
                        {
                            if (lines[ic].ToLower().Contains("facet normal "))
                            {
                                string fnsl = lines[ic].ToLower().Replace("facet normal", "");
                                while (fnsl.StartsWith(" "))
                                {
                                    fnsl = fnsl.Remove(0, 1);
                                }
                                string[] spasear = { " " };
                                string[] normals = fnsl.Split(spasear, StringSplitOptions.None);
                                double nx = 1.0;
                                double ny = 1.0;
                                double nz = 1.0;
                                try
                                {
                                    nx = Convert.ToDouble(normals[0]);
                                    ny = Convert.ToDouble(normals[1]);
                                    nz = Convert.ToDouble(normals[2]);
                                }
                                catch
                                {
                                    nx = 1.0;
                                    ny = 1.0;
                                    nz = 1.0;
                                }
                                Vector3D normal = new Vector3D(nx, ny, nz);
                                ic += 2;

                                string v1sl = lines[ic].ToLower().Replace("vertex", "");
                                while (v1sl.StartsWith(" "))
                                {
                                    v1sl = v1sl.Remove(0, 1);
                                }

                                string[] v1s = v1sl.Split(spasear, StringSplitOptions.None);
                                double v1x = 1.0;
                                double v1y = 1.0;
                                double v1z = 1.0;
                                try
                                {
                                    v1x = Convert.ToDouble(v1s[0]);
                                    v1y = Convert.ToDouble(v1s[1]);
                                    v1z = Convert.ToDouble(v1s[2]);
                                }
                                catch
                                {
                                    v1x = 1.0;
                                    v1y = 1.0;
                                    v1z = 1.0;
                                }
                                Point3D vertex1 = new Point3D(v1x, v1y, v1z);
                                int index1 = helperfunctions.pointsFinder(vertex1, pos.ToList());
                                if (index1 == -1)
                                {
                                    pos.Add(vertex1);
                                    index1 = pos.Count - 1;
                                }

                                ic++;
                                string v2sl = lines[ic].ToLower().Replace("vertex", "");
                                while (v2sl.StartsWith(" "))
                                {
                                    v2sl = v2sl.Remove(0, 1);
                                }

                                string[] v2s = v2sl.Split(spasear, StringSplitOptions.None);
                                double v2x = 1.0;
                                double v2y = 1.0;
                                double v2z = 1.0;
                                try
                                {
                                    v2x = Convert.ToDouble(v2s[0]);
                                    v2y = Convert.ToDouble(v2s[1]);
                                    v2z = Convert.ToDouble(v2s[2]);
                                }
                                catch
                                {
                                    v2x = 1.0;
                                    v2y = 1.0;
                                    v2z = 1.0;
                                }
                                Point3D vertex2 = new Point3D(v2x, v2y, v2z);
                                int index2 = helperfunctions.pointsFinder(vertex2, pos.ToList());
                                if (index2 == -1)
                                {
                                    pos.Add(vertex2);
                                    index2 = pos.Count - 1;
                                }
                                ic++;
                                string v3sl = lines[ic].ToLower().Replace("vertex", "");
                                while (v3sl.StartsWith(" "))
                                {
                                    v3sl = v3sl.Remove(0, 1);
                                }

                                string[] v3s = v3sl.Split(spasear, StringSplitOptions.None);
                                double v3x = 1.0;
                                double v3y = 1.0;
                                double v3z = 1.0;
                                try
                                {
                                    v3x = Convert.ToDouble(v3s[0]);
                                    v3y = Convert.ToDouble(v3s[1]);
                                    v3z = Convert.ToDouble(v3s[2]);
                                }
                                catch
                                {
                                    v3x = 1.0;
                                    v3y = 1.0;
                                    v3z = 1.0;
                                }
                                Point3D vertex3 = new Point3D(v3x, v3y, v3z);
                                int index3 = helperfunctions.pointsFinder(vertex3, pos.ToList());
                                if (index3 == -1)
                                {
                                    pos.Add(vertex3);
                                    index3 = pos.Count - 1;
                                }
                                tris.Add(index1);
                                tris.Add(index2);
                                tris.Add(index3);


                                ic += 2;

                            }
                            else
                            {
                                ic++;
                            }
                        }
                        if (pos.Count >= 3)
                        {
                            MeshGeometry3D mg = new MeshGeometry3D();
                            mg.TriangleIndices = tris;
                            mg.Positions = pos;
                            GeometryModel3D gm = new GeometryModel3D();
                            gm.Geometry = mg;
                            gm.Material = new DiffuseMaterial(mySoBro);
                            gm.BackMaterial = gm.Material;
                            ModelVisual3D mv3d = new ModelVisual3D();
                            mv3d.Content = gm;
                            remade.Children.Add(mv3d);
                        }
                    }
                    ic++;
                }
                if (remade.Children.Count == 0) return null;
                return remade;
            }


            internal static ModelVisual3D readgeomSTLB(byte[] conte)
            {
                ModelVisual3D remade = new ModelVisual3D();
                int position = 80;
                int numtris = (int)Convert.ToUInt32(BitConverter.ToInt32(conte, position));
                position += 4;


                Point3DCollection pos = new Point3DCollection();
                Int32Collection tris = new Int32Collection();
                string sname = "binary";
                SolidColorBrush mySoBro = Brushes.Gray;


                for (int j = 0; j < numtris; j++)
                {




                    double nx = (double)BitConverter.ToSingle(conte, position);
                    position += 4;
                    double ny = (double)BitConverter.ToSingle(conte, position);
                    position += 4;
                    double nz = (double)BitConverter.ToSingle(conte, position);
                    position += 4;

                    Vector3D normal = new Vector3D(nx, ny, nz);

                    double v1x = (double)BitConverter.ToSingle(conte, position);
                    position += 4;
                    double v1y = (double)BitConverter.ToSingle(conte, position);
                    position += 4;
                    double v1z = (double)BitConverter.ToSingle(conte, position);
                    position += 4;

                    Point3D vertex1 = new Point3D(v1x, v1y, v1z);


                    int index1 = helperfunctions.pointsFinder(vertex1, pos.ToList());
                    if (index1 == -1)
                    {
                        pos.Add(vertex1);
                        index1 = pos.Count - 1;
                    }


                    double v2x = (double)BitConverter.ToSingle(conte, position);
                    position += 4;
                    double v2y = (double)BitConverter.ToSingle(conte, position);
                    position += 4;
                    double v2z = (double)BitConverter.ToSingle(conte, position);
                    position += 4;


                    Point3D vertex2 = new Point3D(v2x, v2y, v2z);
                    int index2 = helperfunctions.pointsFinder(vertex2, pos.ToList());
                    if (index2 == -1)
                    {
                        pos.Add(vertex2);
                        index2 = pos.Count - 1;
                    }

                    double v3x = (double)BitConverter.ToSingle(conte, position);
                    position += 4;
                    double v3y = (double)BitConverter.ToSingle(conte, position);
                    position += 4;
                    double v3z = (double)BitConverter.ToSingle(conte, position);
                    position += 4;

                    Point3D vertex3 = new Point3D(v3x, v3y, v3z);
                    int index3 = helperfunctions.pointsFinder(vertex3, pos.ToList());
                    if (index3 == -1)
                    {
                        pos.Add(vertex3);
                        index3 = pos.Count - 1;
                    }
                    tris.Add(index1);
                    tris.Add(index2);
                    tris.Add(index3);



                }
                if (pos.Count >= 3)
                {
                    MeshGeometry3D mg = new MeshGeometry3D();
                    mg.TriangleIndices = tris;
                    mg.Positions = pos;
                    GeometryModel3D gm = new GeometryModel3D();
                    gm.Geometry = mg;
                    gm.Material = new DiffuseMaterial(mySoBro);
                    gm.BackMaterial = gm.Material;
                    ModelVisual3D mv3d = new ModelVisual3D();
                    mv3d.Content = gm;
                    remade.Children.Add(mv3d);
                }



                if (remade.Children.Count == 0) return null;
                return remade;
            }

            //try with transform
            internal static void writegeomSTLrecursivelyWT(ModelVisual3D mv3d, ArrayList lines, string solname, Transform3DGroup tg)
            {
                if (mv3d.Transform != null)
                    tg.Children.Add(mv3d.Transform);
                try
                {
                    Transform3DGroup ats = tg.Clone();
                    GeometryModel3D newModel = mv3d.Content as GeometryModel3D;
                    //tries to extract color info
                    string colorString = "";
                    try
                    {
                        if (newModel.Material is DiffuseMaterial)
                        {
                            DiffuseMaterial dm = newModel.Material as DiffuseMaterial;
                            Brush bru = dm.Brush;
                            Color co;
                            if (bru is SolidColorBrush)
                            {
                                SolidColorBrush sobru = bru as SolidColorBrush;
                                co = sobru.Color;
                            }
                            else
                            {
                                co = dm.Color;
                            }
                            colorString = "[Color]" + co.R.ToString() + "__" + co.G.ToString() + "__" + co.B.ToString() + "__" + co.A.ToString();

                        }
                        else if (newModel.Material is SpecularMaterial)
                        {
                            SpecularMaterial dm = newModel.Material as SpecularMaterial;
                            Brush bru = dm.Brush;
                            Color co;
                            if (bru is SolidColorBrush)
                            {
                                SolidColorBrush sobru = bru as SolidColorBrush;
                                co = sobru.Color;
                            }
                            else
                            {
                                co = dm.Color;
                            }
                            colorString = "[Color]" + co.R.ToString() + "__" + co.G.ToString() + "__" + co.B.ToString() + "__" + co.A.ToString();

                        }
                        else if (newModel.Material is EmissiveMaterial)
                        {
                            EmissiveMaterial dm = newModel.Material as EmissiveMaterial;
                            Brush bru = dm.Brush;
                            Color co;
                            if (bru is SolidColorBrush)
                            {
                                SolidColorBrush sobru = bru as SolidColorBrush;
                                co = sobru.Color;
                            }
                            else
                            {
                                co = dm.Color;
                            }
                            colorString = "[Color]" + co.R.ToString() + "__" + co.G.ToString() + "__" + co.B.ToString() + "__" + co.A.ToString();

                        }
                        else
                        {
                            //gray
                            colorString = "[Color]" + "200__200__200__255";
                        }
                    }
                    catch
                    {
                        colorString = "";
                    }
                    if (newModel != null)
                    {
                        Matrix3D tm;
                        if (newModel.Transform != null)
                        {
                            ats.Children.Add(newModel.Transform);

                        }
                        tm = ats.Value;

                        MeshGeometry3D mtd = newModel.Geometry as MeshGeometry3D;
                        if (mtd != null)
                        {
                            lines.Add("solid " + solname + colorString);
                            //write the mesh

                            for (int tri = 0; tri < mtd.TriangleIndices.Count / 3; tri++)
                            {

                                Point3D pos1 = mtd.Positions[mtd.TriangleIndices[tri * 3]] * tm;

                                Point3D pos2 = mtd.Positions[mtd.TriangleIndices[tri * 3 + 1]] * tm;
                                Point3D pos3 = mtd.Positions[mtd.TriangleIndices[tri * 3 + 2]] * tm;
                                //get the normal to point outwards that is to  nearest corner
                                Vector3D rawnormal = Vector3D.CrossProduct(Point3D.Subtract(pos1, pos2), Point3D.Subtract(pos3, pos2));
                                Point3D faceavg = (new Point3D((pos1.X + pos2.X + pos3.X) / 3, (pos1.Y + pos2.Y + pos3.Y) / 3, (pos1.Z + pos2.Z + pos3.Z) / 3));
                                Vector3D outward = Point3D.Subtract(faceavg.nearestcorner(mtd.Bounds), faceavg);
                                Vector3D outnormal = outward.project(rawnormal);
                                outnormal.Normalize();
                                if (outnormal.X.ToString() == "NaN") outnormal.X = 1;
                                if (outnormal.Y.ToString() == "NaN") outnormal.Y = 1;
                                if (outnormal.Z.ToString() == "NaN") outnormal.Z = 1;
                                lines.Add("facet normal " + outnormal.X.ToString() + " " + outnormal.Y.ToString() + " " + outnormal.Z.ToString());
                                lines.Add("    outer loop");
                                lines.Add("        vertex " + pos1.X.ToString() + " " + pos1.Y.ToString() + " " + pos1.Z.ToString());
                                lines.Add("        vertex " + pos2.X.ToString() + " " + pos2.Y.ToString() + " " + pos2.Z.ToString());
                                lines.Add("        vertex " + pos3.X.ToString() + " " + pos3.Y.ToString() + " " + pos3.Z.ToString());
                                lines.Add("    endloop");
                                lines.Add("endfacet");

                            }
                            lines.Add("endsolid " + solname + colorString);
                        }

                    }
                    foreach (ModelVisual3D mv1 in mv3d.Children)
                    {
                        writegeomSTLrecursivelyWT(mv1, lines, solname + mv3d.Children.IndexOf(mv1), tg);
                    }
                }
                catch (Exception exy)
                {

                }

            }

            //try with transform
            internal static double getArearecursivelyWT(ModelVisual3D mv3d, Transform3DGroup tg)
            {

                double myArea = 0;
                if (mv3d.Transform != null)
                    tg.Children.Add(mv3d.Transform);
                try
                {
                    Transform3DGroup ats = tg.Clone();
                    GeometryModel3D newModel = mv3d.Content as GeometryModel3D;

                    if (newModel != null)
                    {
                        Matrix3D tm;
                        if (newModel.Transform != null)
                        {
                            ats.Children.Add(newModel.Transform);

                        }
                        tm = ats.Value;

                        MeshGeometry3D mtd = newModel.Geometry as MeshGeometry3D;
                        if (mtd != null)
                        {

                            for (int tri = 0; tri < mtd.TriangleIndices.Count / 3; tri++)
                            {

                                Point3D pos1 = mtd.Positions[mtd.TriangleIndices[tri * 3]] * tm;

                                Point3D pos2 = mtd.Positions[mtd.TriangleIndices[tri * 3 + 1]] * tm;
                                Point3D pos3 = mtd.Positions[mtd.TriangleIndices[tri * 3 + 2]] * tm;


                                Point3D basePro = closestOnLineBetween(pos2, pos3, pos1);
                                Vector3D tribase = pos3 - pos2;
                                Vector3D trih = pos1 - basePro;
                                myArea += 0.5 * tribase.Length * trih.Length;

                            }

                        }

                    }
                    foreach (ModelVisual3D mv1 in mv3d.Children)
                    {
                        myArea += getArearecursivelyWT(mv1, tg);
                    }
                }
                catch
                {
                    return 0;
                }
                return myArea;
            }



            //simplified loft
            internal static void AddLoft(MeshGeometry3D mesh,
            IList<Point3DCollection> positionsList
                // IList<Vector3DCollection> normalList,
                // IList<PointCollection> textureCoordinateList
           )
            {
                // disconnect the collections while we change...
                var positions = mesh.Positions;
                var normals = mesh.Normals;
                var textureCoordinates = mesh.TextureCoordinates;
                var triangleIndices = mesh.TriangleIndices;
                mesh.Positions = null;
                mesh.Normals = null;
                mesh.TextureCoordinates = null;
                mesh.TriangleIndices = null;

                int index0 = positions.Count;
                int n = -1;
                for (int i = 0; i < positionsList.Count; i++)
                {
                    var pc = positionsList[i];

                    // check that all curves have same number of points
                    if (n == -1)
                        n = pc.Count;
                    if (pc.Count != n)
                        throw new InvalidOperationException("All curves should have the same number of points");

                    // add the points
                    foreach (var p in pc)
                        positions.Add(p);

                    //// add normals 
                    //if (normalList != null)
                    //{
                    //    var nc = normalList[i];
                    //    foreach (var normal in nc) normals.Add(normal);
                    //}

                    //// add texcoords
                    //if (textureCoordinateList != null)
                    //{
                    //    var tc = textureCoordinateList[i];
                    //    foreach (var t in tc) textureCoordinates.Add(t);
                    //}
                }

                for (int i = 0; i + 1 < positionsList.Count; i++)
                {
                    for (int j = 0; j + 1 < n; j++)
                    {
                        int i0 = index0 + i * n + j;
                        int i1 = i0 + n;
                        int i2 = i1 + 1;
                        int i3 = i0 + 1;
                        triangleIndices.Add(i0);
                        triangleIndices.Add(i1);
                        triangleIndices.Add(i2);

                        triangleIndices.Add(i2);
                        triangleIndices.Add(i3);
                        triangleIndices.Add(i0);
                    }
                }

                mesh.Positions = positions;
                mesh.Normals = normals;
                mesh.TextureCoordinates = textureCoordinates;
                mesh.TriangleIndices = triangleIndices;
            }


            internal static int pointsFinder(Point3D toFind, List<Point3D> findin)
            {
                int index = 0;
                foreach (Point3D isit in findin)
                {
                    if (toFind.DistanceTo(isit) < PUPPIGUISettings.geomWeldToler)
                    {
                        return index;
                    }
                    index++;
                }
                return -1;
            }


            //gets rid of duplicate points



            internal static void pointsList3dPrune(List<Point3D> toprune, double disttoler)
            {
                //int index = 0;
                //first pass, will get rid of about half
                //while (index<toprune.Count-1 )
                //{
                //    if (toprune[index].DistanceTo(toprune[index+1] )<disttoler  )
                //    {
                //        toprune.RemoveAt(index + 1);  
                //    }
                //    else
                //    {
                //        index = index + 1;
                //    }
                //}
                //second pass
                //only keep till they start repeating

                //     Point3D p1 = toprune[0];
                // int i2=-1;
                //     for (i2 = 1; i2 < toprune.Count; i2++)
                //     {
                //         Point3D p2 = toprune[i2];
                //         if (p1.DistanceTo(p2) < disttoler)
                //         {

                //             break;
                //         }
                //     }
                //    //remove from i2
                //while (i2<toprune.Count && i2!=-1  )
                //{
                //    toprune.RemoveAt(i2);  
                //}



                List<Point3D> cleared = new List<Point3D>();
                for (int i1 = 0; i1 < toprune.Count; i1++)
                {
                    Point3D p1 = toprune[i1];
                    bool foundit = false;
                    for (int i2 = 0; i2 < cleared.Count; i2++)
                    {
                        Point3D p2 = cleared[i2];
                        if (p1.DistanceTo(p2) < disttoler)
                        {
                            foundit = true;
                            break;
                        }
                    }
                    if (foundit == false)
                    {
                        cleared.Add(p1);
                    }
                }
                if (cleared.Count > 2)
                {
                    toprune.Clear();
                    toprune.AddRange(cleared);
                }

            }
            //matches vertices from the first list to nearest on the second list or within tolerance
            internal static void matchVertices(List<Point3D> matched, List<Point3D> matchto, double fittoler)
            {

                for (int i1 = 0; i1 < matched.Count; i1++)
                {
                    double matchdist = double.MaxValue;
                    Point3D p1 = matched[i1];
                    int findex = -1;
                    for (int i2 = 0; i2 < matchto.Count; i2++)
                    {
                        Point3D p2 = matchto[i2];
                        if (p1.DistanceTo(p2) <= fittoler)
                        {
                            findex = i2;
                            break;
                        }
                        else
                        {
                            if (p1.DistanceTo(p2) <= matchdist)
                            {
                                matchdist = p1.DistanceTo(p2);
                                findex = i2;
                            }
                        }
                    }
                    if (findex != -1)
                    {
                        matched[i1] = new Point3D(matchto[findex].X, matchto[findex].Y, matchto[findex].Z);
                    }
                }
            }
            //reorders the points cclockwise, assumes they are roughly in same plane
            internal static void reorder3Dpoints(List<Point3D> listofpoints, Vector3D normal)
            {
                if (listofpoints.Count == 0) return;
                Point3D centroid = getCentroid(listofpoints);
                bool outoforder = true;
                Vector3D xaxis = Point3D.Subtract(listofpoints[0], centroid);
                while (outoforder)
                {
                    outoforder = false;
                    for (int i1 = 0; i1 < listofpoints.Count - 1; i1++)
                    {
                        Vector3D ax1 = Point3D.Subtract(listofpoints[i1], centroid);
                        for (int i2 = i1 + 1; i2 < listofpoints.Count; i2++)
                        {
                            Vector3D ax2 = Point3D.Subtract(listofpoints[i2], centroid);
                            if (ax1.anglewithradNormal(xaxis, normal) > ax2.anglewithradNormal(xaxis, normal))
                            //if (Vector3D.AngleBetween (xaxis,ax1) > Vector3D.AngleBetween(xaxis,ax2  ))
                            {
                                listofpoints.Swap<Point3D>(i1, i2);
                                outoforder = true;
                                break;
                            }
                        }
                        if (outoforder == true) break;
                    }
                }
            }
            //gets the centroid of a series of points
            internal static Point3D getCentroid(List<Point3D> listofpoints)
            {
                Point3D centroid = new Point3D();
                if (listofpoints.Count > 0)
                {
                    double sumx = 0;
                    double sumy = 0;
                    double sumz = 0;
                    for (int i = 0; i < listofpoints.Count; i++)
                    {
                        sumx += listofpoints[i].X / listofpoints.Count;
                        sumy += listofpoints[i].Y / listofpoints.Count;
                        sumz += listofpoints[i].Z / listofpoints.Count;
                    }
                    centroid = new Point3D(sumx, sumy, sumz);
                }
                return centroid;
            }
            //converts a 2d point to a plane
            internal static Point3D moveskpointtoplane(Point p2d, Plane3D plane3d, Vector3D xaxis)
            {
                Vector3D regx = Vector3D.Subtract(xaxis, xaxis.project(plane3d.Normal));// new Vector3D(1, 0, 0);
                Vector3D regy = Vector3D.CrossProduct(regx, plane3d.Normal); //new Vector3D(0, 1, 0);
                Vector3D regz = plane3d.Normal; //new Vector3D(0, 0, 1);
                //need to make sure x axis on plane
                // Vector3D startplanex = Vector3D.Subtract( xaxis,xaxis.project(plane3d.Normal))   ;
                //  Vector3D startplaney = Vector3D.CrossProduct(plane3d.Normal, startplanex);
                //   Vector3D startplaneno = plane3d.Normal;

                Vector3D startplanex = new Vector3D(1, 0, 0);// Vector3D.Subtract(xaxis, xaxis.project(plane3d.Normal));
                Vector3D startplaney = new Vector3D(0, 1, 0); //Vector3D.CrossProduct(plane3d.Normal, startplanex);
                Vector3D startplaneno = new Vector3D(0, 0, 1);// plane3d.Normal;  

                Point3D popo3d = new Point3D(p2d.X, p2d.Y, 0);

                //  double rotx = Math.Cos(Vector3D.AngleBetween(regx, startplanex) / 180 * Math.PI) * popo3d.X + Math.Cos(Vector3D.AngleBetween(regy, startplanex) / 180 * Math.PI) * popo3d.Y + Math.Cos(Vector3D.AngleBetween(regz, startplanex) / 180 * Math.PI) * popo3d.Z;
                //   double roty = Math.Cos(Vector3D.AngleBetween(regx, startplaney) / 180 * Math.PI) * popo3d.X + Math.Cos(Vector3D.AngleBetween(regy, startplaney) / 180 * Math.PI) * popo3d.Y + Math.Cos(Vector3D.AngleBetween(regz, startplaney) / 180 * Math.PI) * popo3d.Z;
                //  double rotz = Math.Cos(Vector3D.AngleBetween(regx, startplaneno) / 180 * Math.PI) * popo3d.X + Math.Cos(Vector3D.AngleBetween(regy, startplaneno) / 180 * Math.PI) * popo3d.Y + Math.Cos(Vector3D.AngleBetween(regz, startplaneno) / 180 * Math.PI) * popo3d.Z;
                //need to do it backwards

                //the matrix


                double rotx = Math.Cos(Vector3D.AngleBetween(regx, startplanex) / 180 * Math.PI) * popo3d.X + Math.Cos(Vector3D.AngleBetween(regy, startplanex) / 180 * Math.PI) * popo3d.Y + Math.Cos(Vector3D.AngleBetween(regz, startplanex) / 180 * Math.PI) * popo3d.Z;
                double roty = Math.Cos(Vector3D.AngleBetween(regx, startplaney) / 180 * Math.PI) * popo3d.X + Math.Cos(Vector3D.AngleBetween(regy, startplaney) / 180 * Math.PI) * popo3d.Y + Math.Cos(Vector3D.AngleBetween(regz, startplaney) / 180 * Math.PI) * popo3d.Z;
                double rotz = Math.Cos(Vector3D.AngleBetween(regx, startplaneno) / 180 * Math.PI) * popo3d.X + Math.Cos(Vector3D.AngleBetween(regy, startplaneno) / 180 * Math.PI) * popo3d.Y + Math.Cos(Vector3D.AngleBetween(regz, startplaneno) / 180 * Math.PI) * popo3d.Z;

                popo3d = new Point3D(rotx, roty, rotz);
                //translate assuming centered at origin vefore
                popo3d.Offset(plane3d.Position.X, plane3d.Position.Y, plane3d.Position.Z);
                return popo3d;
            }

            internal static Transform3DGroup transformCombiner(object t1, object t2)
            {
                Transform3DGroup tao = new Transform3DGroup();
                if (t1 != null && t1 is Transform3D)
                {
                    Transform3D tt1 = t1 as Transform3D;
                    tao.Children.Add(tt1.Clone());
                }
                else if (t1 != null && t1 is Transform3DGroup)
                {
                    Transform3DGroup t1g = t1 as Transform3DGroup;
                    foreach (Transform3D talao in t1g.Children)
                    {

                        tao.Children.Add(talao.Clone());
                    }
                }
                if (t2 != null && t2 is Transform3D)
                {
                    Transform3D tt2 = t2 as Transform3D;
                    tao.Children.Add(tt2.Clone());
                }
                else if (t2 != null && t2 is Transform3DGroup)
                {
                    Transform3DGroup t2g = t2 as Transform3DGroup;
                    foreach (Transform3D talao in t2g.Children)
                    {

                        tao.Children.Add(talao.Clone());
                    }
                }
                return tao;

            }

            //gets the bounding box of a list of 3d points
            internal static Rect3D pCloudBBox(List<Point3D> pcloud)
            {
                Rect3D bb = new Rect3D();
                double maxx = double.MinValue;
                double maxy = double.MinValue;
                double maxz = double.MinValue;
                double minx = double.MaxValue;
                double miny = double.MaxValue;
                double minz = double.MaxValue;
                int pc = pcloud.Count;
                for (int i = 0; i < pc; i++)
                {
                    if (pcloud[i].X > maxx) maxx = pcloud[i].X;
                    if (pcloud[i].X < minx) minx = pcloud[i].X;

                    if (pcloud[i].Y > maxy) maxy = pcloud[i].Y;
                    if (pcloud[i].Y < miny) miny = pcloud[i].Y;

                    if (pcloud[i].Z > maxz) maxz = pcloud[i].Z;
                    if (pcloud[i].Z < minz) minz = pcloud[i].Z;

                }
                bb = new Rect3D(minx, miny, minz, maxx - minx, maxy - miny, maxz - minz);
                return bb;
            }
            //internal static List<ModelVisual3D> cutMVInHalf(ModelVisual3D toCut, Point3D p, Vector3D n, bool fillCuts, List<ModelVisual3D> ignorePlanes)//, List<ModelVisual3D> firstHalf, List<ModelVisual3D> secondHalf  )
            //{
            //    List<ModelVisual3D> twoHalves = new List<ModelVisual3D>();
            //    List<Model3D> contentHalves = new List<Model3D>();
            //    List<ModelVisual3D> firstHalves = new List<ModelVisual3D>();
            //    List<ModelVisual3D> secondHalves = new List<ModelVisual3D>();
            //    if (toCut != null)
            //    {
            //        ModelVisual3D myPlane = null;
            //        List<ModelVisual3D> myPlanes = new List<ModelVisual3D>();
            //        Model3D myc = toCut.Content;
            //        GeometryModel3D gc = myc as GeometryModel3D;
            //        Material makemat = null;
            //        if (gc != null)
            //        {
            //            makemat = gc.Material;
            //        }

            //        if (myc != null)
            //        {


            //            contentHalves = cutMInHalf(myc, p, n);
            //            if (fillCuts)
            //            {
            //                List<List<Point3D>> conthurs = getM3DConts(myc, p, n);
            //                int ii = 0;
            //                foreach (List<Point3D> conthur in conthurs)
            //                    if (conthur.Count > 2)
            //                    {
            //                        //closed contour
            //                        if (conthur[0].DistanceTo(conthur[conthur.Count - 1]) < PUPPIGUISettings.geomWeldToler)
            //                        {
            //                            if (makemat == null)
            //                            {
            //                                makemat = new DiffuseMaterial(new SolidColorBrush(Colors.White));
            //                            }
            //                            myPlane = pillFolly(conthur, makemat);
            //                            myPlane.SetName("PS_" + ii.ToString() + "_" + toCut.GetName());
            //                            myPlanes.Add(myPlane);
            //                            ii++;
            //                        }
            //                    }

            //            }

            //        }
            //        ModelVisual3D half1 = new ModelVisual3D();

            //        ModelVisual3D half2 = new ModelVisual3D();
            //        if (contentHalves.Count > 0)
            //        {
            //            half1.Content = contentHalves[0];
            //            half1.SetName(toCut.GetName() + "_S1");
            //            if (fillCuts)
            //                foreach (ModelVisual3D mpp in myPlanes)
            //                {
            //                    if (mpp != null && mpp.Content.Bounds.IsEmpty == false)
            //                    {
            //                        ModelVisual3D pignore = mpp.cloneMyVisual();
            //                        half1.Children.Add(pignore);
            //                        ignorePlanes.Add(pignore);
            //                    }
            //                }

            //        }
            //        if (contentHalves.Count > 1)
            //        {
            //            half2.Content = contentHalves[1];
            //            half2.SetName(toCut.GetName() + "_S2");
            //            if (fillCuts)
            //            {
            //                foreach (ModelVisual3D mpp in myPlanes)
            //                {
            //                    if (mpp != null && mpp.Content.Bounds.IsEmpty == false)
            //                    {
            //                        ModelVisual3D pignore = mpp.cloneMyVisual();
            //                        half2.Children.Add(pignore);
            //                        ignorePlanes.Add(pignore);
            //                    }
            //                }
            //            }

            //        }
            //        foreach (ModelVisual3D tocc in toCut.Children)
            //        {
            //            if (ignorePlanes.Contains(tocc) == false)
            //            {
            //                List<ModelVisual3D> cchalves = cutMVInHalf(tocc, p, n, fillCuts, ignorePlanes);
            //                if (cchalves.Count > 0)
            //                    firstHalves.Add(cchalves[0]);
            //                if (cchalves.Count > 1)
            //                    secondHalves.Add(cchalves[1]);
            //            }
            //            //check which side
            //            else
            //            {

            //                Point3D pcenter = tocc.Content.Bounds.rectcenter();
            //                Vector3D plav = pcenter - p;
            //                if (Vector3D.AngleBetween(plav, n) < 90 && Vector3D.AngleBetween(plav, n) > -90)
            //                {
            //                    firstHalves.Add(tocc.cloneMyVisual());
            //                }
            //                else
            //                {
            //                    secondHalves.Add(tocc.cloneMyVisual());
            //                }
            //            }

            //        }


            //        foreach (ModelVisual3D chalf1 in firstHalves)
            //        {
            //            half1.Children.Add(chalf1);
            //        }
            //        foreach (ModelVisual3D chalf2 in secondHalves)
            //        {
            //            half2.Children.Add(chalf2);
            //        }
            //        if (half1.Content != null || half1.Children.Count > 0) twoHalves.Add(half1);
            //        if (half2.Content != null || half2.Children.Count > 0) twoHalves.Add(half2);
            //    }
            //    return twoHalves;
            //}
            internal static List<ModelVisual3D> cutMVInHalf(ModelVisual3D toCut, Point3D p, Vector3D n)
            {
                List<ModelVisual3D> twoHalves = new List<ModelVisual3D>();
                List<Model3D> contentHalves = new List<Model3D>();
                List<ModelVisual3D> firstHalves = new List<ModelVisual3D>();
                List<ModelVisual3D> secondHalves = new List<ModelVisual3D>();
                if (toCut != null)
                {
                    ModelVisual3D myPlane = null;
                    List<ModelVisual3D> myPlanes = new List<ModelVisual3D>();
                    Model3D myc = toCut.Content;
                    GeometryModel3D gc = myc as GeometryModel3D;
                    Material makemat = null;
                    if (gc != null)
                    {
                        makemat = gc.Material;
                    }

                    if (myc != null)
                    {


                        contentHalves = cutMInHalf(myc, p, n);


                    }
                    ModelVisual3D half1 = new ModelVisual3D();

                    ModelVisual3D half2 = new ModelVisual3D();
                    if (contentHalves.Count > 0)
                    {
                        half1.Content = contentHalves[0];
                        half1.SetName(toCut.GetName() + "_S1");


                    }
                    if (contentHalves.Count > 1)
                    {
                        half2.Content = contentHalves[1];
                        half2.SetName(toCut.GetName() + "_S2");


                    }
                    foreach (ModelVisual3D tocc in toCut.Children)
                    {

                        List<ModelVisual3D> cchalves = cutMVInHalf(tocc, p, n);
                        if (cchalves.Count > 0)
                            firstHalves.Add(cchalves[0]);
                        if (cchalves.Count > 1)
                            secondHalves.Add(cchalves[1]);


                    }


                    foreach (ModelVisual3D chalf1 in firstHalves)
                    {
                        half1.Children.Add(chalf1);
                    }
                    foreach (ModelVisual3D chalf2 in secondHalves)
                    {
                        half2.Children.Add(chalf2);
                    }
                    if (half1.Content != null || half1.Children.Count > 0) twoHalves.Add(half1);
                    if (half2.Content != null || half2.Children.Count > 0) twoHalves.Add(half2);
                }
                return twoHalves;
            }


            internal static List<Model3D> cutMInHalf(Model3D toCut, Point3D p, Vector3D n)
            {

                List<Model3D> halves = new List<Model3D>();
                GeometryModel3D gm = toCut as GeometryModel3D;
                if (gm != null)
                {


                    MeshGeometry3D mgplus = MeshGeometryHelper.Cut(gm.Geometry as MeshGeometry3D, p, n);
                    n.Negate();
                    MeshGeometry3D mgminus = MeshGeometryHelper.Cut(gm.Geometry as MeshGeometry3D, p, n);

                    if (mgplus != null && mgplus.Bounds.IsEmpty == false)
                    {
                        Model3D malaoplus = toCut.Clone();
                        GeometryModel3D gmalaoplus = malaoplus as GeometryModel3D;

                        MeshBuilder mbi = new MeshBuilder();

                        gmalaoplus.Geometry = mgplus;

                        halves.Add(malaoplus);
                    }
                    else
                    {
                        halves.Add(null);
                    }
                    if (mgminus != null && mgminus.Bounds.IsEmpty == false)
                    {
                        Model3D malaominus = toCut.Clone();
                        GeometryModel3D gmalaominus = malaominus as GeometryModel3D;
                        gmalaominus.Geometry = mgminus;
                        halves.Add(malaominus);
                    }
                    else
                    {
                        halves.Add(null);
                    }

                }
                return halves;

            }

            internal static List<Model3D> cutMByPlanes(Model3D toCut, List<PUPPIPlane3D> myPlanes)
            {
                List<Model3D> rlist = new List<Model3D>();
                if (myPlanes.Count > 0)
                {
                    List<Model3D> rrlist = new List<Model3D>();
                    rrlist = cutMInHalf(toCut, myPlanes[0].theorigin, myPlanes[0].thenormal);

                    if (myPlanes.Count > 1)
                    {
                        List<PUPPIPlane3D> otherP = new List<PUPPIPlane3D>(myPlanes);
                        otherP.RemoveAt(0);
                        foreach (Model3D mlao in rrlist)
                        {
                            if (mlao != null)
                            {
                                rlist.AddRange(cutMByPlanes(mlao, otherP));
                            }
                        }
                    }
                    else
                    {
                        foreach (Model3D mlao in rrlist)
                        {
                            if (mlao != null)
                            {
                                rlist.Add(mlao);
                            }
                        }
                    }
                }
                return rlist;
            }

            internal static List<ModelVisual3D> cutMVByPlanes(ModelVisual3D toCut, List<PUPPIPlane3D> myPlanes)//, bool fillCuts, List<ModelVisual3D> ignorePlanes)
            {
                List<ModelVisual3D> rlist = new List<ModelVisual3D>();
                if (myPlanes.Count > 0)
                {
                    List<ModelVisual3D> rrlist = new List<ModelVisual3D>();
                    rrlist = cutMVInHalf(toCut, myPlanes[0].theorigin, myPlanes[0].thenormal);//, fillCuts, ignorePlanes);

                    if (myPlanes.Count > 1)
                    {
                        List<PUPPIPlane3D> otherP = new List<PUPPIPlane3D>(myPlanes);
                        otherP.RemoveAt(0);
                        foreach (ModelVisual3D mlao in rrlist)
                        {
                            if (mlao != null)
                            {
                                rlist.AddRange(cutMVByPlanes(mlao, otherP));//, fillCuts, ignorePlanes));
                            }
                        }
                    }
                    else
                    {
                        foreach (ModelVisual3D mlao in rrlist)
                        {
                            if (mlao != null)
                            {
                                //get cut plane contour
                                //PUPPIPlane3D mp = myPlanes[0]; 
                                //if (mlao.Content!=null )
                                //{

                                //}
                                rlist.Add(mlao);
                            }
                        }
                    }
                }
                return rlist;
            }
            //internal static List<Point3D> getMContours(Model3D mao, Point3D p, Vector3D n)
            //{
            //    List<Point3D> cpts = new List<Point3D>();
            //    GeometryModel3D gmao = mao as GeometryModel3D;
            //    if (gmao != null)
            //    {
            //        var segments = MeshGeometryHelper.GetContourSegments(gmao.Geometry as MeshGeometry3D, p, n).ToList();

            //        foreach (var contour in MeshGeometryHelper.CombineSegments(segments, PUPPIGUISettings.geomWeldToler).ToList())
            //        {
            //            if (contour.Count > 2)
            //            {
            //                List<Point3D> mycpts = new List<Point3D>();
            //                mycpts.AddRange(contour);
            //                Point3D sp = (Point3D)contour[0];
            //                Point3D ep = (Point3D)contour[contour.Count - 1];
            //                bool isclosed = false;
            //                if (sp.DistanceTo(ep) < PUPPIGUISettings.geomWeldToler)
            //                {
            //                    isclosed = true;
            //                }
            //                HelperClasses.helperfunctions.pointsList3dPrune(mycpts, PUPPIGUISettings.geomWeldToler);
            //                if (isclosed) mycpts.Add(ep);
            //                cpts = mycpts;
            //                break;
            //            }
            //        }
            //    }
            //    return cpts;
            //}
        }
        //extensions we might need
        //internal static class modelingExtensions
        //{
        //    //method to retrieve Point2d x and y -nneded
        //    internal static double getPoint2DX(this Point po)
        //    {
        //        return po.X;
        //    }
        //    internal static double getPoint2DY(this Point po)
        //    {
        //        return po.Y;
        //    }
        //}





    }
}

#region oldtransformcode
//and for the other end of the extrusion
//List<Point3D> endpoints = new List<Point3D>();
//startplaneor = pupo.points3dlist[pupo.points3dlist.Count - 1];
//startplaneno = Point3D.Subtract(pupo.points3dlist[pupo.points3dlist.Count - 1], pupo.points3dlist[pupo.points3dlist.Count - 2]);
//startplanex = Vector3D.Subtract(xaxis, xaxis.project(startplaneno));
//trao = new TranslateTransform3D(startplaneor.ToVector3D());
//roz = new AxisAngleRotation3D(startplaneno, Vector3D.AngleBetween(new Vector3D(1, 0, 0), startplanex));
//traroz = new RotateTransform3D(roz);
//rox = new AxisAngleRotation3D(startplanex, Vector3D.AngleBetween(new Vector3D(0, 0, 1), startplaneno));
//trarox = new RotateTransform3D(rox);
//tg = new Transform3DGroup();
//tg.Children.Add(trarox);
//tg.Children.Add(traroz);
//tg.Children.Add(trao);
//foreach (Point popo in sketchy.points2dlist)
//{
//    Point3D popo3d = new Point3D(popo.X, popo.Y, 0);
//    Point3D tranplaneo = tg.Transform(popo3d);
//    endpoints.Insert(0,tranplaneo);
//}

////the regular x axis projected onto plane
//    Vector3D regxaxis = Vector3D.Subtract(new Vector3D(1, 0, 0), new Vector3D(1, 0, 0).project(startplaneno));  
//    AxisAngleRotation3D axa=new AxisAngleRotation3D(startplaneno,xaxo.anglewithrad(regxaxis)*180/Math.PI) ;
//    RotateTransform3D roro = new RotateTransform3D(axa);
//    roro.CenterX = startplaneor.X;
//    roro.CenterY = startplaneor.Y;
//    roro.CenterZ = startplaneor.Z;
////need to rotate the points by the angle of the normal with the vertical z+
//    AxisAngleRotation3D norma = new AxisAngleRotation3D(Vector3D.CrossProduct (new Vector3D(0,0,1),startplaneno)  , startplaneno.anglewithrad(new Vector3D(0,0,1))*180/Math.PI   );
//    RotateTransform3D rono = new RotateTransform3D(norma);
//    rono.CenterX = 0;//startplaneor.X;
//    rono.CenterY = 0;// startplaneor.Y;
//    rono.CenterZ = 0;// startplaneor.Z;
//foreach (Point popo in sketchy.points2dlist  )
//{
//    Point3D popo3d = new Point3D(popo.X, popo.Y, 0);
//    popo3d = rono.Transform(popo3d);   
//    Vector3D ortopo = Point3D.Subtract(popo3d, startplaneor);
//    Vector3D projecto = ortopo.project(startplaneno);
//    Point3D planeo = Point3D.Subtract(popo3d, projecto);
//     //but these need to be rotated to fit the x axis
//    Point3D tranplaneo = roro.Transform(planeo);


//    startpoints.Add(tranplaneo);  


//}
//  var meshBuilder = new MeshBuilder(false, false);
//  Point3D corigin = (Point3D)(usercodeinputs[0]) ;
//  Vector3D vdiag = (Vector3D)(usercodeinputs[1]);
//  HelperClasses.PUPPISketch2D sketchy = (HelperClasses.PUPPISketch2D)(usercodeinputs[2]);   
//  //if a list of transforms is presented
//  Transform3DGroup groupo = new Transform3DGroup();
//  if (usercodeinputs[4] is ArrayList)
//  {
//      ArrayList ui = usercodeinputs[4] as ArrayList;
//      for (int ti = 0; ti < ui.Count; ti++)
//      {
//          groupo.Children.Add(ui[ti] as Transform3D);
//      }
//  }//or just one transform
//  else
//  {
//      groupo.Children.Add(usercodeinputs[4] as Transform3D);
//  }
//  Color colo = (Color)usercodeinputs[3];
//  Point3D endpoint = new Point3D(corigin.X, corigin.Y, corigin.Z);
//  endpoint.Offset(vdiag.X, vdiag.Y, vdiag.Z);
//  Vector3D xaxis = Vector3D.CrossProduct(vdiag, new Vector3D(0, 0, 1));

//  meshBuilder.AddExtrudedGeometry(sketchy.points2dlist, xaxis, corigin, endpoint);
//// meshBuilder.AddPolygon()  
//  var mesh = meshBuilder.ToMesh(true);
//  GeometryModel3D newModel = new GeometryModel3D();
//  newModel.Geometry = mesh;

//  newModel.Material = new DiffuseMaterial(new SolidColorBrush(colo));
//  newModel.Transform = groupo;
//  ModelVisual3D model = new ModelVisual3D();
//  model.Content = newModel;


//////for test also do points on ground
//List<Point3D> startpointsground = new List<Point3D>();
//foreach (Point popo in sketchy.points2dlist)
//{
//    Point3D popo3d = new Point3D(popo.X, popo.Y, 0);
//    startpointsground.Add(popo3d);
//}

//  usercodeoutputs[0] = model;

//simply an array of points
//internal class PUPPIPolyLine3D
//{
//    internal List<Point3D> points3dlist;
//    internal PUPPIPolyLine3D()
//    {
//       points3dlist = new List<Point3D>();
//    }
//    //giving it an array of points
//    internal PUPPIPolyLine3D(object myp3d)
//    {
//        List<Point3D> po3= new List<Point3D>();
//        ArrayList myp3ds= new ArrayList();
//       if (myp3d.GetType()==typeof(List )  )
//       {
//           try
//           {
//               po3 = myp3d as List<Point3D>;
//           }
//           catch
//           {
//               po3 = new List<Point3D>();
//           }
//       }
//           else if (myp3d.GetType()==typeof(ArrayList)  )
//           {
//            try
//            {
//                myp3ds = myp3d as ArrayList;
//            }
//            catch
//            {
//                myp3ds = new ArrayList(); 
//            }
//           }
//        }

//    }

//  CuttingPlaneGroup cgp = new CuttingPlaneGroup();
//   cgp.CuttingPlanes.Add(new Plane3D(startplaneor, startplaneno));




//HelperClasses.CoordinateTransform coo = new HelperClasses.CoordinateTransform(startplanex, Vector3D.CrossProduct(startplanex, startplaneno), startplaneor);
//foreach (Point popo in sketchy.points2dlist)
//{
//    Point3D popo3d = new Point3D(popo.X, popo.Y, 0);
//    //rotated coords per Rutherfor Aris page 9
//    //double rotx = Math.Cos(Vector3D.AngleBetween(regx, startplanex) / 180 * Math.PI) * popo3d.X + Math.Cos(Vector3D.AngleBetween(regy, startplanex) / 180 * Math.PI) * popo3d.Y + Math.Cos(Vector3D.AngleBetween(regz, startplanex) / 180 * Math.PI) * popo3d.Z;
//    //double roty = Math.Cos(Vector3D.AngleBetween(regx, startplaney) / 180 * Math.PI) * popo3d.X + Math.Cos(Vector3D.AngleBetween(regy, startplaney) / 180 * Math.PI) * popo3d.Y + Math.Cos(Vector3D.AngleBetween(regz, startplaney) / 180 * Math.PI) * popo3d.Z;
//    //double rotz = Math.Cos(Vector3D.AngleBetween(regx, startplaneno) / 180 * Math.PI) * popo3d.X + Math.Cos(Vector3D.AngleBetween(regy, startplaneno) / 180 * Math.PI) * popo3d.Y + Math.Cos(Vector3D.AngleBetween(regz, startplaneno) / 180 * Math.PI) * popo3d.Z;

//    // double rotx = Math.Cos(regx.anglewithrad(  startplanex) ) * popo3d.X + Math.Cos(regy.anglewithrad ( startplanex) ) * popo3d.Y + Math.Cos(regz.anglewithrad (startplanex)) * popo3d.Z;
//    //double roty = Math.Cos(regx.anglewithrad (startplaney) ) * popo3d.X + Math.Cos(regy.anglewithrad (startplaney) ) * popo3d.Y + Math.Cos(regz.anglewithrad ( startplaney) ) * popo3d.Z;
//    //double rotz = Math.Cos(regx.anglewithrad ( startplaneno) ) * popo3d.X + Math.Cos(regy.anglewithrad ( startplaneno) ) * popo3d.Y + Math.Cos(regz.anglewithrad (startplaneno) ) * popo3d.Z;

//    //double rotx = Math.Cos(startplanex.anglewithrad(regx)) * popo3d.X + Math.Cos(startplanex.anglewithrad(regy)) * popo3d.Y + Math.Cos(startplanex.anglewithrad(regz)) * popo3d.Z;
//    //double roty = Math.Cos(startplaney.anglewithrad(regx)) * popo3d.X + Math.Cos(startplaney.anglewithrad(regy)) * popo3d.Y + Math.Cos(startplaney.anglewithrad(regz)) * popo3d.Z;
//    //double rotz = Math.Cos(startplaneno.anglewithrad(regx)) * popo3d.X + Math.Cos(startplaneno.anglewithrad(regy)) * popo3d.Y + Math.Cos(startplaneno.anglewithrad(regz)) * popo3d.Z;


//    //move to new origin
//    //Point3D tranplaneo = new Point3D(startplaneor.X + rotx, startplaneor.Y + roty, startplaneor.Z + rotz);
//    //Point3D tranplaneo = tg.Transform(popo3d);
//    // Point3D tranplaneo = coo.whattodo .Transform(popo3d);
//    startpoints.Add(tranplaneo);
//}



//gets ends to cap extrusion
//internal class MeshGeometryHelper2
//{
//    /// <summary>
//    /// Create a 64-bit key from two 32-bit indices
//    /// </summary>
//    private static UInt64 CreateKey(UInt32 i0, UInt32 i1)
//    {
//        return ((UInt64)i0 << 32) + i1;
//    }

//    /// <summary>
//    /// Extract two 32-bit indices from the 64-bit key
//    /// </summary>
//    private static void ReverseKey(UInt64 key, out UInt32 i0, out UInt32 i1)
//    {
//        i0 = (UInt32)(key >> 32);
//        i1 = (UInt32)((key << 32) >> 32);
//    }
//    internal static List<int> FindBorderEdges(MeshGeometry3D mesh)
//    {
//        var dict = new Dictionary<ulong, int>();

//        for (int i = 0; i < mesh.TriangleIndices.Count / 3; i++)
//        {
//            int i0 = i * 3;
//            for (int j = 0; j < 3; j++)
//            {
//                int index0 = mesh.TriangleIndices[i0 + j];
//                int index1 = mesh.TriangleIndices[i0 + (j + 1) % 3];
//                int minIndex = Math.Min(index0, index1);
//                int maxIndex = Math.Max(index1, index0);
//                ulong key = CreateKey((UInt32)minIndex, (UInt32)maxIndex);
//                if (dict.ContainsKey(key))
//                {
//                    dict[key] = dict[key] + 1;
//                }
//                else
//                {
//                    dict.Add(key, 1);
//                }
//            }
//        }

//        var edges = new List<int>();
//        foreach (var kvp in dict)
//        {
//            // find edges only used by 1 triangle
//            if (kvp.Value == 1)
//            {
//                uint i0, i1;
//                ReverseKey(kvp.Key, out i0, out i1);
//                edges.Add((int)i0);
//                edges.Add((int)i1);
//            }
//        }

//        var borderPoints = new List<Point>();
//        mesh.Positions.ToList().ForEach(p => borderPoints.Add(new Point(p.X, p.Y)));

//        var result = OrganizeEdges(edges, borderPoints);
//        return result;
//    }


//    private static List<int> OrganizeEdges(List<int> edges, List<Point> positions)
//    {
//        var visited = new Dictionary<int, bool>();
//        var edgeList = new List<int>();
//        var resultList = new List<int>();
//        var nextIndex = -1;
//        while (resultList.Count < edges.Count)
//        {
//            if (nextIndex < 0)
//            {
//                for (int i = 0; i < edges.Count; i += 2)
//                {
//                    if (!visited.ContainsKey(i))
//                    {
//                        nextIndex = edges[i];
//                        break;
//                    }
//                }
//            }

//            for (int i = 0; i < edges.Count; i += 2)
//            {
//                if (visited.ContainsKey(i))
//                    continue;

//                int j = i + 1;

//                int k = -1;
//                if (edges[i] == nextIndex)
//                    k = j;
//                else if (edges[j] == nextIndex)
//                    k = i;

//                if (k >= 0)
//                {
//                    var edge = edges[k];
//                    visited[i] = true;
//                    edgeList.Add(nextIndex);
//                    edgeList.Add(edge);
//                    nextIndex = edge;
//                    i = 0;
//                }
//            }

//            // calculate winding order - then add to final result.
//            var borderPoints = new List<Point>();
//            edgeList.ForEach(ei => borderPoints.Add(positions[ei]));
//            var winding = CalculateWindingOrder(borderPoints);
//            if (winding > 0)
//                edgeList.Reverse();

//            resultList.AddRange(edgeList);
//            edgeList = new List<int>();
//            nextIndex = -1;
//        }

//        return resultList;
//    }


//    internal static MeshGeometry3D Extrude(MeshGeometry3D surface, double z)
//    {
//        var borderIndexes = MeshGeometryHelper2.FindBorderEdges(surface);
//        var borderPoints = new List<Point3D>();
//        borderIndexes.ToList().ForEach(bi => borderPoints.Add(surface.Positions[bi]));

//        var topPoints = borderPoints.ToList();
//        var botPoints = borderPoints.Select(p => new Point3D(p.X, p.Y, p.Z + z)).ToList();

//        var allPoints = new List<Point3D>();
//        var allIndexes = new List<int>();
//        var allNormals = new List<Vector3D>();

//        // sides.
//        allPoints.AddRange(topPoints);
//        allPoints.AddRange(botPoints);

//        for (int i = 0; i < topPoints.Count; i += 2)
//        {
//            int j = (i + 1) % topPoints.Count;

//            allIndexes.Add(i);
//            allIndexes.Add(j);
//            allIndexes.Add(topPoints.Count + j);
//            allIndexes.Add(topPoints.Count + j);
//            allIndexes.Add(topPoints.Count + i);
//            allIndexes.Add(i);

//            var a = allPoints[i].ToVector3D();
//            var b = allPoints[j].ToVector3D();
//            var c = allPoints[topPoints.Count + j].ToVector3D();
//            Vector3D vs1 = Vector3D.Subtract(b, a);
//            Vector3D vs2 = Vector3D.Subtract(c, a);
//            Vector3D vs3 = Vector3D.CrossProduct(vs1, vs2);
//            vs3.Normalize();

//            // var n0 = b.Subtract(a).Crss(c.Subtract(a)).Unit();
//            var n0 = vs3;
//            allNormals.Add(n0);
//            allNormals.Add(n0);
//            allNormals.Add(n0);
//            allNormals.Add(n0);
//        }

//        var surfaceNormals = new List<Vector3D>();
//        if (surface.Normals == null)
//            surface.TriangleIndices.ToList().ForEach(i => surfaceNormals.Add(new Vector3D(0, 0, 1)));

//        // top
//        var count = allPoints.Count;
//        var topSurfacePoints = surface.Positions.Select(p => new Point3D(p.X, p.Y, p.Z + z)).ToList();
//        var topNormals = surfaceNormals.ToList();
//        allPoints.AddRange(topSurfacePoints);
//        allNormals.AddRange(topNormals);
//        var topSurfaceIndexes = surface.TriangleIndices.Select(i => count + i).ToList();
//        AddTriangleIndexes(topSurfaceIndexes, allIndexes, false);

//        // bottom
//        count = allPoints.Count;
//        var botSurfacePoints = surface.Positions.ToList();
//        var botNormals = surfaceNormals.Select(n => n.Flip()).ToList();
//        allPoints.AddRange(botSurfacePoints);
//        allNormals.AddRange(botNormals);
//        var botSurfaceIndexes = surface.TriangleIndices.Select(i => count + i).ToList();
//        AddTriangleIndexes(botSurfaceIndexes, allIndexes, true);

//        var mesh = new Mesh3D(allPoints, allIndexes);
//        var meshGeom = mesh.ToMeshGeometry3D();
//        meshGeom.Normals = new Vector3DCollection(allNormals);

//        if (z < 0)
//            ReverseWinding(meshGeom);

//        var simple = Simplify(meshGeom, 0.01);
//        return simple;
//    }
//    internal static MeshGeometry3D Simplify(MeshGeometry3D mesh, double eps)
//    {
//        // Find common positions
//        var dict = new Dictionary<int, int>(); // map position index to first occurence of same position
//        for (int i = 0; i < mesh.Positions.Count; i++)
//        {
//            for (int j = i + 1; j < mesh.Positions.Count; j++)
//            {
//                if (dict.ContainsKey(j))
//                    continue;

//                double l2 = (mesh.Positions[i] - mesh.Positions[j]).LengthSquared;
//                if (l2 < eps)
//                {
//                    dict.Add(j, i);
//                }
//            }
//        }

//        var p = new Point3DCollection();
//        var ti = new Int32Collection();

//        // create new positions array
//        var newIndex = new Dictionary<int, int>(); // map old index to new index
//        for (int i = 0; i < mesh.Positions.Count; i++)
//        {
//            if (!dict.ContainsKey(i))
//            {
//                newIndex.Add(i, p.Count);
//                p.Add(mesh.Positions[i]);
//            }
//        }

//        // Update triangle indices
//        for (int i = 0; i < mesh.TriangleIndices.Count; i++)
//        {
//            int index = mesh.TriangleIndices[i];
//            int j;
//            if (dict.TryGetValue(index, out j))
//            {
//                ti.Add(newIndex[j]);
//            }
//            else
//            {
//                ti.Add(newIndex[index]);
//            }
//        }

//        var result = new MeshGeometry3D();
//        result.Positions = p;
//        result.TriangleIndices = ti;
//        return result;
//    }
//    private static void AddTriangleIndexes(List<int> triangleIndices, List<int> allIndexes, bool reverseWindingOrder)
//    {
//        for (int i = 0; i < triangleIndices.Count; i += 3)
//        {
//            var i0 = triangleIndices[i + 0];
//            var i1 = triangleIndices[i + 1];
//            var i2 = triangleIndices[i + 2];
//            if (reverseWindingOrder)
//                allIndexes.AddRange(new[] { i2, i1, i0 });
//            else
//                allIndexes.AddRange(new[] { i0, i1, i2 });
//        }
//    }

//    internal static void ReverseWinding(MeshGeometry3D mesh)
//    {
//        var indices = mesh.TriangleIndices.ToList();
//        var flippedIndices = new List<int>();
//        AddTriangleIndexes(indices, flippedIndices, true);
//        mesh.TriangleIndices = new System.Windows.Media.Int32Collection(flippedIndices);
//    }

//    /// <summary>
//    /// returns 1 for CW, -1 for CCW, 0 for unknown.
//    /// </summary>
//    internal static int CalculateWindingOrder(IList<Point> points)
//    {
//        // the sign of the 'area' of the polygon is all we are interested in.
//        var area = CalculateSignedArea(points);
//        if (area < 0.0)
//            return 1;
//        else if (area > 0.0)
//            return -1;
//        return 0; // error condition - not even verts to calculate, non-simple poly, etc.
//    }

//    internal static double CalculateSignedArea(IList<Point> points)
//    {
//        double area = 0.0;
//        for (int i = 0; i < points.Count; i++)
//        {
//            int j = (i + 1) % points.Count;
//            area += points[i].X * points[j].Y;
//            area -= points[i].Y * points[j].X;
//        }
//        area /= 2.0;

//        return area;
//    }
//}
#endregion

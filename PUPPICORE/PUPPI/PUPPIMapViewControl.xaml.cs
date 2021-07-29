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
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;   
//2d view for maps
namespace PUPPIGUI
{
   
      internal partial class PUPPIMapViewControl : UserControl
    {
        
        internal partial class PUPPIMapCanvas : Window
        {
            internal Grid canvasview;
            internal Frame frame2d;
            internal HelixViewport3D hv;
            internal PUPPIMapCanvas(PUPPICanvas myc)
            {
                hv = new HelixViewport3D(); 
                canvasview = new Grid();

                //canvasview.Margin = new Thickness(0, 0, 0, 0);

                //canvasview.Height = 640;
                //canvasview.Width = 480;


                canvasview.HorizontalAlignment = HorizontalAlignment.Stretch;
                canvasview.VerticalAlignment = VerticalAlignment.Stretch;


                frame2d = new Frame();
                frame2d.AllowDrop = false;
               
                frame2d.Content = hv;
                //frame2d.ClipToBounds = true;
                //frame2d.Width = 640;
                //frame2d.Height = 480;


                frame2d.HorizontalAlignment = HorizontalAlignment.Stretch;


                frame2d.HorizontalContentAlignment = HorizontalAlignment.Stretch;

                frame2d.VerticalAlignment = VerticalAlignment.Stretch;


                frame2d.VerticalContentAlignment = VerticalAlignment.Stretch;  


                //make sure helix viewport camera only looks from the top
                hv.ShowViewCube = false;
                MouseGesture nogesture=new MouseGesture();
                nogesture.MouseAction=MouseAction.None;    
                hv.RotateGesture = nogesture;
                hv.ChangeLookAtGesture = nogesture;
                hv.BackViewGesture = nogesture;
                hv.BottomViewGesture = nogesture;
                hv.ChangeFieldOfViewGesture = nogesture;
                hv.FrontViewGesture = nogesture;
                hv.LeftViewGesture = nogesture;
                hv.OrthographicToggleGesture = nogesture;
                hv.ResetCameraGesture = nogesture;
                hv.RotateGesture2 = nogesture;
                hv.TopViewGesture = nogesture;

                //add pertinent stuff from the original canvas
                ModelVisual3D newgrid = new ModelVisual3D();
                newgrid.Content = myc.gv3d.Content.Clone();     
                hv.Children.Add(newgrid);  
                //set camera to look from above
                hv.PreviewMouseWheel+=hv_PreviewMouseWheel;  

                //hv.CameraChanged += hv_CameraChanged;
                hv.Camera.UpDirection = new Vector3D(0, 1, 0);
                hv.Camera.Position = new Point3D(0, 0, 10);
                hv.Camera.LookDirection = new Vector3D(0, 0, -1); 
                hv.ZoomExtents();
                hv.Background = new SolidColorBrush(Colors.White);
                DefaultLights dl = new DefaultLights();
                hv.Children.Add(dl);
                canvasview.Children.Add(frame2d);
                drawbases(myc);
                drawuniqueconns(myc); 
                
            }

            //makes sure the pan doesn't get messed up when we zoom in or out
            private void hv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
            {

                if (hv.Camera.LookDirection.Length >= 800 && e.Delta < 0)
                {
                    Vector3D vava = hv.Camera.LookDirection;
                    vava = Vector3D.Multiply(800 / vava.Length, vava);
                    hv.Camera.LookDirection = vava;
                }

                if (hv.Camera.LookDirection.Length <= 5 && e.Delta > 0)
                {
                    Vector3D vava = hv.Camera.LookDirection;
                    vava.Normalize();
                    vava = Vector3D.Multiply(5, vava);
                    hv.Camera.LookDirection = vava;
                }
            }




            void hv_CameraChanged(object sender, RoutedEventArgs e)
            {
               // hv.Camera.UpDirection = new Vector3D(0, 1, 0);
               //  
            }
            //draws rectangles for bases
            internal void drawbases(PUPPICanvas mycanvas)
            {
                foreach (PUPPICanvas.ModViz3DNode mv in mycanvas.stacks.Values)
                {

                    if (mv.parent == null)
                    {
                        Point3D nlocation = new Point3D(mv.boundingBox.X, mv.boundingBox.Y, 0);// new Point3D(mv.myself.Content.Bounds.Location.X, mv.myself.Content.Bounds.Location.Y, 0);
                        Point3D blocation = new Point3D(nlocation.X, nlocation.Y, nlocation.Z);
                        Size3D bsize = new Size3D(mv.boundingBox.SizeX, mv.boundingBox.SizeY, PUPPIGUISettings.connRaise);     // new Size3D(mv.myself.Content.Bounds.SizeX, mv.myself.Content.Bounds.SizeY, PUPPIGUISettings.connRaise);
                        var meshBuilder = new MeshBuilder(false, false);
                        blocation.Offset(bsize.X * 0.5, bsize.Y * 0.5, bsize.Z * 0.5);
                        meshBuilder.AddBox(blocation, bsize.X, bsize.Y, bsize.Z);
                        var mesh = meshBuilder.ToMesh(true);
                        GeometryModel3D newModel = new GeometryModel3D();
                        newModel.Geometry = mesh;
                        if (mv.nodeUpdatedMaterial == null)
                        {
                            newModel.Material = mycanvas.node_default_mat;// (mv.myself.Content as GeometryModel3D).Material;      
                        }
                        else
                        {
                            newModel.Material = mv.nodeUpdatedMaterial;   
                        }
                        ModelVisual3D model = new ModelVisual3D();
                        model.Content = newModel;
                        TextVisual3D caption = new TextVisual3D();
                        caption.UpDirection = new Vector3D(0, 1, 0);
                        //if (logical_representation.name != "")
                        //{

                        //so it fits
                        caption.Height = PUPPIGUISettings.nodeSpacing;// (myself.Content.Bounds.SizeY / (double)logical_representation.name.Length);
                        if (mv.caption != null)
                        {
                            caption.Text = mv.caption.Text;
                        }
                        else
                        {
                            caption.Text = "Custom Caption";
                        }
                        List<PUPPICanvas.ModViz3DNode> allchilds=new List<PUPPICanvas.ModViz3DNode>() ;
  
                        mv.getalllevelschildren(allchilds );
                        if (allchilds.Count > 0)
                        {
                            caption.Text += "\n" +" + " + "\n" + allchilds.Count.ToString() +"\n"+ "child nodes";     
                        }
                        caption.Height = bsize.Y;   
                       
                        
                        if (caption.Content.Bounds.SizeX > mv.boundingBox.SizeX - PUPPIGUISettings.ioWidth && mv.boundingBox.SizeX - PUPPIGUISettings.ioWidth > 0)
                        {
                            caption.Height /= caption.Content.Bounds.SizeX / (mv.boundingBox.SizeX - PUPPIGUISettings.ioWidth);
                        }
                        //}
                        if (caption.Height > mv.boundingBox.SizeY)
                        {
                            caption.Height = mv.boundingBox.SizeY;
                        }
                        caption.Position = new Point3D(nlocation.X + bsize.X * 0.5, nlocation.Y + bsize.Y*0.5, nlocation.Z  + bsize.Z   + PUPPIGUISettings.textRaise); 
                        

                        hv.Children.Add(model);
                        hv.Children.Add(caption);  
                    }
                }
            }
            //draws simplified connections
            internal void drawuniqueconns(PUPPICanvas mycanvas)
            {
                //list of unique stack connections
                List<PUPPICanvas.ModViz3DConn> uconns = new List<PUPPICanvas.ModViz3DConn>();    
                foreach (PUPPICanvas.ModViz3DConn mvcc in mycanvas.connpaths.Values    )
                {

                    bool found = false;
                    PUPPICanvas.ModViz3DNode sbase = mvcc.sourcenode.getroot();
                    PUPPICanvas.ModViz3DNode dbase = mvcc.destnode.getroot();

                    foreach (PUPPICanvas.ModViz3DConn checkon in uconns )
                    {
                        PUPPICanvas.ModViz3DNode csbase = checkon.sourcenode.getroot();
                        PUPPICanvas.ModViz3DNode cdbase = checkon.destnode.getroot();
                        if (csbase==sbase && cdbase==dbase || csbase==cdbase)
                        {
                            found = true;
                            break;
                        }
                       
                    }
                    if (sbase == dbase) found = true;
                    if (found == false)
                    {
                        uconns.Add(mvcc);
                    }
                }
                //now draw the unique connections between bases
                foreach (PUPPICanvas.ModViz3DConn mvcc in uconns  )
                {
                    var meshBuilder = new MeshBuilder(false, false);

                    //GeometryModel3D gm = mvcc.myself.Content as GeometryModel3D;
                    ////flatten the geometry
                    //ScaleTransform3D scdown = new ScaleTransform3D(1, 1, 0.0001);
                    //TranslateTransform3D mdown = new TranslateTransform3D(0, 0, -gm.Bounds.Location.Z); 
                    //ModelVisual3D flaconn = new ModelVisual3D();
                    //flaconn.Content = gm.Clone();
                    //Transform3DGroup tg = new Transform3DGroup();
                    //tg.Children.Add(mdown);
                    //tg.Children.Add(scdown);
                    //flaconn.Transform = tg;
                    //hv.Children.Add(flaconn);  
                    
                    //better way to generate clearer maps
                    Point3D spoint=mvcc.sourcenode.myself.Content.Bounds.rectcenter();
                    spoint.Z = mvcc.sourcenode.myself.Content.Bounds.SizeZ+PUPPIGUISettings.textRaise*2 ;
                    Point3D epoint = new Point3D();
                    
                    //for (int i=0;i<mvcc.basePoints.Count;i++  )
                    //{
                    //    Point3D pla = mvcc.basePoints[i];
                    //    epoint = new Point3D(pla.X, pla.Y, PUPPIGUISettings.connRaise);
                    //    //smooth
                    //    if (i<mvcc.basePoints.Count-1   )
                    //    {
                    //        Point3D pla1 = mvcc.basePoints[i+1];
                    //        Point3D epoint1 = new Point3D(pla1.X, pla1.Y, PUPPIGUISettings.connRaise);
                    //        if (epoint1.DistanceTo(spoint )<PUPPIGUISettings.nodeSide    )
                    //        {
                    //            epoint = new Point3D((epoint.X + epoint1.X) * 0.5, (epoint.Y + epoint1.Y) * 0.5, PUPPIGUISettings.connRaise);
                    //            i++;
  
                    //        }

                    //    }


                    //    //    epoint.Z = 0; 
                    //    //    if (cc.ishorizontal )
                    //    //    {
                    //    //        epoint.Offset(0, cc.centeroffset, 0);  
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        epoint.Offset(cc.centeroffset,0,0 )  ;
                    //    //    }
                    //    ////split angld into two
                    //    //    if (spoint.X != epoint.X && spoint.Y != epoint.Y)
                    //    //    {
                    //    //        Point3D middie = new Point3D(spoint.X, epoint.Y, 0);
                    //    //        meshBuilder.AddPipe(spoint, middie, 0, PUPPIGUISettings.solidConnectionSize, PUPPIGUISettings.pipeSides);
                    //    //        meshBuilder.AddArrow(middie, epoint, PUPPIGUISettings.solidConnectionSize, PUPPIGUISettings.arrowTip);

                    //    //    }
                    //    //    else
                    //    //    {
                    //            meshBuilder.AddArrow(spoint, epoint, PUPPIGUISettings.solidConnectionSize, PUPPIGUISettings.arrowTip);
                    //       // }
                    //        spoint=new Point3D(epoint.X,epoint.Y,epoint.Z  )  ;
                    //        i++;
                    
                    
                    //}

                    //final point going to the box
                    epoint = mvcc.destnode.myself.Content.Bounds.rectcenter();
                    epoint.Z = mvcc.destnode.myself.Content.Bounds.SizeZ + PUPPIGUISettings.textRaise * 2;
                    meshBuilder.AddArrow(spoint, epoint, PUPPIGUISettings.solidConnectionSize, PUPPIGUISettings.arrowTip);
                    var mesh = meshBuilder.ToMesh(true);
                     GeometryModel3D gm=new GeometryModel3D(); 
                    gm.Geometry = mesh;
                    Color ccm=ColorHelper.HsvToColor(mvcc.cindex, mvcc.colsaturation, mvcc.colvallue);
                    ccm=ccm.ChangeAlpha(100);
                    Brush bru = new SolidColorBrush(ccm);
                    gm.Material = new DiffuseMaterial(bru); //MaterialHelper.CreateMaterial(ccm);
                    
                    //flatten the geometry
                    ScaleTransform3D scdown = new ScaleTransform3D(1, 1, 0.0001);
                   // TranslateTransform3D mdown = new TranslateTransform3D(0, 0, -gm.Bounds.Location.Z);
                    ModelVisual3D flaconn = new ModelVisual3D();
                    flaconn.Content = gm;
                    
                    //Transform3DGroup tg = new Transform3DGroup();
                    //tg.Children.Add(mdown);
                    //tg.Children.Add(scdown);
                   // flaconn.Transform = tg;
                    hv.Children.Add(flaconn);  
                }
               
            }

        }
        PUPPIMapCanvas pmc;
        internal PUPPIMapViewControl(PUPPICanvas myc)
        {
            InitializeComponent();
            pmc = new PUPPIMapCanvas(myc); 
            this.PUPPIMapViewCanvas.Children.Add(pmc.canvasview);
  
        }
        
    }
}

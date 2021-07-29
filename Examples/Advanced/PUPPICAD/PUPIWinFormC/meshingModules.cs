

/*
MIConvexHull
*The MIT License (MIT)

Copyright (c) 2010 David Sehnal, Matthew Campbell

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
SOFTWARE.
*/


using PUPPIGUI;
using PUPPIModel;
using HelixToolkit.Wpf;
using System.Reflection;
using PUPPICAD;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace meshingModules
{
   //compute convex hull from points
    //uses MIConvexHull
     public class PUPPICADConvexHull : PUPPIModule
    {
         public PUPPICADConvexHull()
            : base()
        {
            name = "Convex Hull";
            outputs.Add(new ModelVisual3D());
            description = "Compute convex hull ModelVisual3D object from points. A transform or list of transforms can also be applied.";
            outputnames.Add("Model");
            inputs.Add(new PUPPIInParameter());
            inputnames.Add("Point3D List");

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


                ArrayList pointsA = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[0]);
                List<Point3D> points = pointsA.Cast<Point3D>().ToList();
                //get rid of duplicates
                
                

               points=PUPPICAD.HelperClasses.utilities.removeDuplicatePoints(points); 
                if (points.Count == 0)
                {
                    usercodeoutputs[0] = "no points";
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
                //color to render
                Color colo = (Color)(usercodeinputs[1]);
                //convert Point3D list to list of MIConvexHull Vertex
                List<VertexHull> vertices=new List<VertexHull>();  
                for (int i=0;i<points.Count;i++ )
                {

                    Point3D thisPoint=(Point3D)points[i];
                    VertexHull ve=new VertexHull();
                   double[] myCoordinates; 
                    myCoordinates = new double[3] ;
                    myCoordinates[0]=thisPoint.X;
                    myCoordinates[1]=thisPoint.Y;
                    myCoordinates[2]=thisPoint.Z;
                    ve.coordinates=myCoordinates;  
                    vertices.Add(ve);  
   
                }

                //compute hull
                MIConvexHull.ConvexHull myHull = new MIConvexHull.ConvexHull(vertices);
                List<MIConvexHull.IFaceConvHull> gotFaces = new List<MIConvexHull.IFaceConvHull>();
                List<MIConvexHull.IVertexConvHull> gotVertices = myHull.FindConvexHull(out gotFaces);
                
               
                //generate model
                var CVPoints = new Point3DCollection();
                foreach (var chV in gotVertices )
                {
                    CVPoints.Add(new Point3D(chV.coordinates[0],chV.coordinates[1],chV.coordinates[2]));
                   
                }

                var faceTris = new Int32Collection();
                foreach (var f in gotFaces)
                {
                    // The vertices are stored in clockwise order.
                    faceTris.Add(gotVertices.IndexOf(f.vertices[0]));
                    faceTris.Add(gotVertices.IndexOf(f.vertices[1]));
                    faceTris.Add(gotVertices.IndexOf(f.vertices[2]));
                }
                var meshme = new MeshGeometry3D
                {
                    Positions = CVPoints,
                    TriangleIndices = faceTris
                };

               

                        
               

                GeometryModel3D newModel = new GeometryModel3D();
                newModel.Geometry = meshme;

                newModel.Material = new DiffuseMaterial(new SolidColorBrush(colo));
                newModel.BackMaterial = newModel.Material;
                newModel.Transform = groupo;
                ModelVisual3D model = new ModelVisual3D();
                model.Content = newModel;

              

                usercodeoutputs[0] = model;
            }
            catch (Exception exy)
            {
                usercodeoutputs[0] = "error: " +exy.ToString();

            }





        }
    }

     public class PUPPICADDelaunay : PUPPIModule
     {
         public PUPPICADDelaunay()
             : base()
         {
             name = "Delaunay";
             description = "Computes Delaunay tringulation from points. Returns a list of PUPPIPolyLine3D , one for each triangle";
             outputs.Add(null);
             outputnames.Add("PUPPIPolyLine3D list");
             inputs.Add(new PUPPIInParameter());
             inputnames.Add("Point3D List");

         }
         public override void process_usercode()
         {


             try
             {


                 ArrayList pointsA = PUPPIModel.PUPPIModule.makeCollOrEnumIntoArrayList(usercodeinputs[0]);
                 List<Point3D> points = pointsA.Cast<Point3D>().ToList();
                 //get rid of duplicates



                 points = PUPPICAD.HelperClasses.utilities.removeDuplicatePoints(points);
                 if (points.Count == 0)
                 {
                     usercodeoutputs[0] = "no points";
                     return;
                 }
                 
                 
                 //convert Point3D list to list of MIConvexHull Vertex
                 List<VertexHull> vertices = new List<VertexHull>();
                 for (int i = 0; i < points.Count; i++)
                 {

                     Point3D thisPoint = (Point3D)points[i];
                     VertexHull ve = new VertexHull();
                     double[] myCoordinates;
                     myCoordinates = new double[3];
                     myCoordinates[0] = thisPoint.X;
                     myCoordinates[1] = thisPoint.Y;
                     myCoordinates[2] = 0;// thisPoint.Z;
                     ve.coordinates = myCoordinates;
                     vertices.Add(ve);

                 }

                 

                 //compute hull
                 MIConvexHull.ConvexHull myHull = new MIConvexHull.ConvexHull(vertices);
                 //calculate Delaunay   
                 List<MIConvexHull.IFaceConvHull> myDelaunay = myHull.FindDelaunayTriangulation();
                 //convert to list of Polylines
                 List<PUPPICAD.HelperClasses.PUPPIPolyLine3D> listpolys = new List<PUPPICAD.HelperClasses.PUPPIPolyLine3D>();     

                 foreach (MIConvexHull.IFaceConvHull mhf in myDelaunay )
                 {
                     List<Point3D> myPoints = new List<Point3D>();  
                     Point3D p1=new Point3D(mhf.vertices[0].coordinates[0], mhf.vertices[0].coordinates[1],mhf.vertices[0].coordinates[2]);
                     myPoints.Add(p1);  
                     Point3D p2 = new Point3D(mhf.vertices[1].coordinates[0], mhf.vertices[1].coordinates[1], mhf.vertices[1].coordinates[2]);
                     myPoints.Add(p2);  
                     Point3D p3 = new Point3D(mhf.vertices[2].coordinates[0], mhf.vertices[2].coordinates[1], mhf.vertices[2].coordinates[2]);
                     myPoints.Add(p3);  
                     PUPPICAD.HelperClasses.PUPPIPolyLine3D myPoly = new PUPPICAD.HelperClasses.PUPPIPolyLine3D(myPoints );
                     listpolys.Add(myPoly);  
                 }
                 usercodeoutputs[0] = listpolys;  



                
             }
             catch (Exception exy)
             {
                 usercodeoutputs[0] = "error: " + exy.ToString();

             }





         }
     }


    //bare minimum class to build the hull
    public class VertexHull: MIConvexHull.IVertexConvHull
    {
        public double[] coordinates { get; set; }
    }

    //public class FaceHull: MIConvexHull.IFaceConvHull
    //{
    //    public double[] normal { get; set; }
    //    public VertexHull[] vertices { get; set; }
    //}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PUPPIGUIController;


namespace customDropdowns
{
    //storing emthods to add more dropdown menus to the main form
    public static class dropdownCreator
    {
        //loading premade objects onto the canvas
        public static PUPPIDropDownMenu makeCADConstructsMenu(PUPPIProgramCanvas myCanvas)
        {
            PUPPIGUIController.PUPPIDropDownMenu cadObjects = new PUPPIDropDownMenu(myCanvas, "Quick Build");

            Action loadPoint2D = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\point2d-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Point (2D)", loadPoint2D, "Adds a Point to the canvas with all required inputs satisfied.It can be used to build other objects.Double click to change numeric inputs.", false);


            Action loadPoint3D = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\point3d-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Point3D", loadPoint3D, "Adds a Point3D to the canvas with all required inputs satisfied.It can be used to build other objects.Double click to change numeric inputs.", false);


            Action loadVector3D = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\vector3d-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Vector3D", loadVector3D, "Adds a Vector3D to the canvas with all required inputs satisfied.It can be used to build other objects.Double click to change numeric inputs.", false);

            Action loadLine3D = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\line3d-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Line(3D)", loadLine3D, "Adds a Line (2 point PUPPIPolyline3D) to the canvas with all required inputs satisfied.It can be used to build other objects.Double click to change numeric inputs.", false);

            
            
            Action loadPlane = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\plane-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Plane", loadPlane, "Adds a PUPPIPlane3D to the canvas with all required inputs satisfied.It can be used to build other objects.Double click to change numeric inputs.", false);


            Action loadRectangleSketch = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\rectanglesketch2d-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Rectangle Sketch", loadRectangleSketch, "Adds a Rectangle Sketch to the canvas with all required inputs satisfied.It can be used to build other objects.Double click to change numeric inputs.", false);

            Action loadCircleSketch = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\circlesketch2d-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Circle Sketch", loadCircleSketch, "Adds a Circle Sketch to the canvas with all required inputs satisfied.It can be used to build other objects.Double click to change numeric inputs.", false);

            Action loadArcSketch = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\arcsketch2d-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Arc Sketch", loadArcSketch, "Adds an Arc Sketch to the canvas with all required inputs satisfied.It can be used to build other objects.Double click to change numeric inputs.", false);

            Action loadCapsuleSketch = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\capsulesketch2d-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Capsule Sketch", loadCapsuleSketch, "Adds a Capsule Sketch to the canvas with all required inputs satisfied.It can be used to build other objects.Double click to change numeric inputs.", false);

            Action loadEllipseSketch = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\ellipsesketch2d-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Ellipse Sketch", loadEllipseSketch, "Adds an Ellipse Sketch to the canvas with all required inputs satisfied.It can be used to build other objects.Double click to change numeric inputs.", false);


            Action loadPointObject = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\pointobject-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Point Object", loadPointObject, "Adds a renderable Point to the canvas with all required inputs satisfied.Double click to change numeric inputs. Connect to 3D Model Access node to render in CAD Window.", false);

            Action loadRectangle3D = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\rectanglesketch3d-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add 3D Rectangle Object", loadRectangle3D, "Adds a renderable Rectangle to the canvas with all required inputs satisfied.Double click to change numeric inputs. Connect to 3D Model Access node to render in CAD Window.", false);




            Action loadCircle3D = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\circlesketch3d-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add 3D Circle Object", loadCircle3D, "Adds a renderable Circle to the canvas with all required inputs satisfied.Double click to change numeric inputs. Connect to 3D Model Access node to render in CAD Window.", false);

            Action loadPlanarMesh = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\planarmesh-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Planar Mesh Object", loadPlanarMesh, "Adds a renderable Planar Mesh to the canvas with all required inputs satisfied.Double click to change numeric inputs. Connect to 3D Model Access node to render in CAD Window.", false);

            
            Action loadSphere = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\sphere-construct.xml"); 
            };
            cadObjects.addCustomCommandToMenu("Add Sphere Object", loadSphere, "Adds a Sphere to the canvas with all required inputs satisfied.Double click to change numeric inputs. Connect to 3D Model Access node to render in CAD Window.", false);

            Action loadBox = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\box-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Box Object", loadBox, "Adds a Box to the canvas with all required inputs satisfied.Double click to change numeric inputs. Connect to 3D Model Access node to render in CAD Window.", false);

           
            Action loadCylinder = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\cylinder-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Cylinder Object", loadCylinder, "Adds a Cylinder to the canvas with all required inputs satisfied.Double click to change numeric inputs. Connect to 3D Model Access node to render in CAD Window.", false);

            Action loadCone = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\cone-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Cone Object", loadCone, "Adds a Cone to the canvas with all required inputs satisfied.Double click to change numeric inputs. Connect to 3D Model Access node to render in CAD Window.", false);


            Action loadColor = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\color-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Color", loadColor, "Adds a Alpha RGB Color node to the canvas with all required inputs satisfied.It can be used to build other objects.Double click to change numeric inputs. ", false);


            Action loadTranslate = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\translate-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Translate Transform", loadTranslate, "Adds a Translate Transform 3D node to the canvas with all required inputs satisfied.It can be used to build other objects.Double click to change numeric inputs. ", false);

            Action loadRotate = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\rotate-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Rotate Transform", loadRotate, "Adds a Rotate Transform 3D node to the canvas with all required inputs satisfied.It can be used to build other objects.Double click to change numeric inputs. ", false);

            Action loadScale = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\scale-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add Scale Transform", loadScale, "Adds a Scale Transform 3D node to the canvas with all required inputs satisfied.It can be used to build other objects.Double click to change numeric inputs. ", false);

            Action loadForLoop = () =>
            {
                myCanvas.importCanvasCommand(@".\Constructs\forloop-construct.xml");
            };
            cadObjects.addCustomCommandToMenu("Add For Loop", loadForLoop, "Adds a For List node to the canvas with all required inputs satisfied.Double click to change numeric inputs. ", false);


            return cadObjects;
        }
    }
}

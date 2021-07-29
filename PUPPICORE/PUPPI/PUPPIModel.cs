using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PUPPIFormUtils;
//to make new classes
using System.Reflection;
using System.Reflection.Emit;
using PUPPIDEBUG;
using PUPPIGUI;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PUPPIModel
{
    /// <summary>
    /// The <see cref="PUPPIModel"/> namespace contains classes for creating and modifying PUPPI Visual Programming Module logic units.
    /// </summary>

    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }


    /// <summary>
    /// Inheritable class for creating Visual Programming modules.
    /// </summary>
    public class PUPPIModule
    {



        // internal bool isconditional = false;
        //to check if any other modules are processing
        internal static long concurrentProcesses = 0;
        //public override bool Equals(object obj)
        //{
        //    // If parameter is null return false.
        //    if (obj == null)
        //    {
        //        return false;
        //    }

        //    // If parameter cannot be cast to Point return false.
        //    PUPPIModule p = obj as PUPPIModule;
        //    if ((System.Object)p == null)
        //    {
        //        return false;
        //    }

        //    // Return true if the fields match:
        //    return (name==p.name) && (inputnames.SequenceEqual(p.inputnames)) && (outputnames.SequenceEqual(p.outputnames)) && (description==p.description)      ;
        //}

        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

            dllName = dllName.Replace(".", "_");

            if (dllName.EndsWith("_resources")) return null;
            string an = Assembly.GetExecutingAssembly().GetName().Name;
            //System.Windows.Forms.MessageBox.Show(an);  
            //  System.Windows.Forms.MessageBox.Show(an);
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(an + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

            byte[] bytes = (byte[])rm.GetObject(dllName);

            if (bytes != null && bytes.Length > 0) return System.Reflection.Assembly.Load(bytes); else return null;
        }
        /// <summary>
        /// Gets a description of the PUPPIModule in string format.
        /// </summary>
        /// <returns>A description of the PUPPIModule in string format</returns>
        public override string ToString()
        {
            try
            {

                string rv = name;
                if (inputs.Count > 0)
                {
                    rv += "_|_[Inputs:]{";
                    for (int i = 0; i < inputs.Count; i++)
                    {
                        object oa = getInputValue(i);
                        if (oa != null)
                        {
                            rv += inputnames[i] + ":|:" + oa.ToString();
                        }
                        else
                        {
                            rv += inputnames[i] + ":|:" + "null";
                        }
                        if (i < inputs.Count - 1) rv += ",";
                    }
                    rv += "}";
                }
                if (outputs.Count > 0)
                {
                    rv += "_|_[Outputs:]{";
                    for (int i = 0; i < outputs.Count; i++)
                    {
                        if (outputs[i] != null)
                            rv += outputnames[i] + ":|:" + outputs[i].ToString();
                        else rv += outputnames[i] + ":|:" + "null";
                        if (i < outputs.Count - 1) rv += ",";
                    }
                    rv += "}";
                }
                return rv;
            }
            catch (Exception exy)
            {
                return "Object not proprtly initialized:\n" + exy.ToString();
            }
        }
        /// <summary>
        /// Returns a node's geometry. This allows module processing and node renderer updates to vary on node's position and size.
        /// </summary>
        /// <param name="xStart">output parameter, start X</param>
        /// <param name="yStart">output parameter, start Y</param>
        /// <param name="zStart">output parameter, start Z</param>
        /// <param name="xSize">output parameter, size X</param>
        /// <param name="ySize">output parameter, size Y</param>
        /// <param name="zSize">output parameter, size Z</param>
        /// <returns>If node bounding box was retrieved</returns>
        public bool getMyNodeBoundingBox(out double xStart, out double yStart, out double zStart, out double xSize, out double ySize, out double zSize)
        {
            xStart = 0;
            yStart = 0;
            zStart = 0;

            xSize = 0;
            ySize = 0;
            zSize = 0;
            if (useMyPUPPIcanvas != null)
            {
                return useMyPUPPIcanvas.getANodeBoundingBoxByGUID(GUID.ToString(), out xStart, out yStart, out zStart, out xSize, out ySize, out zSize);
            }
            return false;
        }

        ///// <summary>
        ///// Tries to load PUPPI dll from current folder.
        ///// </summary>
        //public void selfLoad()
        //{
        //    if (al == false)
        //    {
        //        string codeBase = Assembly.GetExecutingAssembly().CodeBase;
        //        UriBuilder uri = new UriBuilder(codeBase);
        //        string path = Uri.UnescapeDataString(uri.Path);
        //        try
        //        {
        //            Assembly a=Assembly.LoadFrom(System.IO.Path.GetDirectoryName(path) + "\\PUPPI.dll");
        //           if (a!=null)
        //            al = true;
        //        }
        //        catch
        //        {

        //        }
        //    }
        //}


        /// <summary>
        /// Converts an a ICollection or IEnumberable o  to an ArrayList to use in th PUPPI standard way
        /// </summary>
        /// <param name="object">An object implmenting the IColelction or IEnumerable interface</param>
        /// <returns>an ArrayList or null if argument not compatible</returns>
        public static ArrayList makeCollOrEnumIntoArrayList(object ooo)
        {
            ArrayList st = new ArrayList();
            if (ooo is ICollection)
                st = new ArrayList(ooo as ICollection);
            else if (ooo is IEnumerable)
                st = PUPPIModule.makeMeAnArrayList(ooo as IEnumerable);
            else
                return null;
            return st;
        }


        /// <summary>
        /// Converts an enumerable to an ArrayList to use in th PUPPI standard way
        /// </summary>
        /// <param name="gsomeEnumerable">An object implmenting the IEnumerable interface</param>
        /// <returns>an ArrayList</returns>
        public static ArrayList makeMeAnArrayList(IEnumerable someEnumerable)
        {
            ArrayList st = new ArrayList();
            foreach (object gee in someEnumerable)
            {
                st.Add(gee);
            }
            return st;
        }
        //when generated dlls if needs to load PUPPI
        internal static bool al = false;
        //settings on load / save . make sure to clear it
        //internal static string myNodeCustomSavedSettings="";

        internal static PUPPICanvas useMyPUPPIcanvas = null;
        internal static PUPPIGUIController.PUPPIProgramCanvas useMyPUPPIcontroller = null;
        ///// <summary>
        ///// Gives complete access to the canvas controller, if any defined.
        ///// </summary>
        //public static PUPPIGUIController.PUPPIProgramCanvas GUIController = null;

        /// <summary>
        /// Programatically updates the static (non-interacting) elements placed on the canvas.
        /// </summary>
        /// <param name="cRenderer">a PUPPI Custom Renderer object or null</param>
        public void updateCanvasVisualElements(PUPPICustomRenderer cRenderer)
        {
            if (useMyPUPPIcanvas != null)
            {
                useMyPUPPIcanvas.staticVisuals = cRenderer;
                useMyPUPPIcanvas.renderStaticVisual();
            }
        }
        /// <summary>
        /// If the module is part of a file loaded into the canvas, it returns file name and path
        /// </summary>
        /// <returns>File path or empty string</returns>
        public string getModuleFileNamePath()
        {
            if (useMyPUPPIcanvas != null)
                return useMyPUPPIcanvas.fileCurLdd;
            else
                return "";
        }

        ///// <summary>
        ///// gets a module by GUID
        ///// </summary>
        ///// <param name="guid">The Node GUID as a string</param>
        ///// <returns>a PUPPIModule</returns>
        //public static PUPPIModule getModuleByGUID(string guid)
        //{
        //    ca
        //}
        /// <summary>
        /// returns the module GUID
        /// </summary>
        /// <returns></returns>
        public int getGUID()
        {
            return GUID;
        }

        internal int GUID { get; set; }
        /// <summary>
        /// name of the module. This will be displayed on node unless caption changed. Can only include letters and digits.
        /// </summary>
        public string name;
        /// <summary>
        /// description to help the user understand what it does
        /// </summary>
        public string description = "";
        /// <summary>
        /// Some modules perform operations on objects outside the PUPPI Canvas and cannot work from within containers. Set this to true to prevent them from being added to a container.
        /// </summary>
        public bool canBeInContainer = true;

        /// <summary>
        /// A cleaned up default caption
        /// </summary>
        public string cleancap = "";
        /// <summary>
        /// This is used to pass any info we want when we process. Shows up in node information.
        /// </summary>
        public string anymessage = "";

        /// <summary>
        /// These are inputs in the format PUPPIInParameter. They reference the outputs of connected modules.
        /// When completeProcessOverride flag is true the process_usercode works with these and the developer
        /// has to account for inputs passed as Lists and extract elements in process_usercode.
        /// </summary>
        public List<PUPPIInParameter> inputs;
        /// <summary>
        /// Values for outputs, can be any type including generics. Type conversion should be handling in process_usercode
        /// For nodes without inputs, in order to properly save outputs changed by gesture or double click, set the value of the output in the constructor to something matching the type of the output set by the double click.
        ///  When completeProcessOverride flag is true the process_usercode function works with the values from this array
        /// </summary>
        public ArrayList outputs;

        /// <summary>
        /// These are the inputs  available to process_usercode method is completeProcessOverride flag is false.
        /// Can be any type including generics. Type conversion should be handled in process_usercode
        /// List processing is done automatically when completeProcessOverride is false.
        /// </summary>
        public ArrayList usercodeinputs;
        /// <summary>
        /// These are the outputs  available to process_usercode method is completeProcessOverride flag is false.
        /// Can be any type including generics. Type conversion should be handled in process_usercode
        /// </summary>
        public ArrayList usercodeoutputs;
        /// <summary>
        /// Arraylist of default outputs when no inputs are present. Has to match the number of outputs.
        /// </summary>
        public ArrayList defaultoutputs;
        /// <summary>
        /// In case we want to revert to previous output values during doubleclick_usercode, for instance for cancel.
        /// </summary>
        public ArrayList dblclkpreviousoutputs;
        /// <summary>
        /// Names of outputs as will be displayed in the visual programming canvas.
        /// Has to match the number of outputs.
        /// </summary>
        public List<string> outputnames;
        /// <summary>
        /// Names of inputs as will be displayed in the visual programming canvas.
        /// Has to match the number of inputs.
        /// </summary>
        public List<string> inputnames;

        internal List<methodparm> methodparms;
        /// <summary>
        /// When a PUPPI module is generated from an existing method, this will store the method name.
        /// </summary>
        public string methodname = "";

        internal protected bool isconstructor = false;
        internal protected bool isparameterlessconstructor = false;
        internal protected enum method_types { REGULAR_METHOD, GETTER_METHOD, SETTER_METHOD };
        internal protected bool moduleisenumeration = false;
        internal protected method_types method_type = method_types.REGULAR_METHOD;
        //calling type name
        internal protected string ctn = "";
        /// <summary>
        /// Sets a color override for the node using this module.use unsetNodeColor() to restore to default color. Red is reserved for errors so it will reset when sucessfully processing.
        /// </summary>
        /// <param name="redColor">0-1</param>
        /// <param name="greenColor">0-1</param>
        /// <param name="blueColor">0-1</param>
        public void setNodeColor(double redColor, double greenColor, double blueColor)
        {
            moduleNodeColorRed = Math.Max(Math.Min(redColor, 1), 0);
            moduleNodeColorGreen = Math.Max(Math.Min(greenColor, 1), 0);
            moduleNodeColorBlue = Math.Max(Math.Min(blueColor, 1), 0);
            moduleNodeColorOverride = true;
            if (useMyPUPPIcanvas != null)
            {
                if (useMyPUPPIcanvas.stacks.ContainsKey(GUID.ToString()))
                {
                    useMyPUPPIcanvas.stacks[GUID.ToString()].setMyPUPPINodeUpdatedMaterialColor(moduleNodeColorRed, moduleNodeColorGreen, moduleNodeColorBlue, 1);
                    forceMyNodeToUpdate();
                }
            }

        }

        /// <summary>
        /// Sets the solidity of the node. If a node is solid it reacts normally to the canvas rearranging, otherwise it is ignored and can intersect other nodes.
        /// </summary>
        /// <param name="isSolid">boolean  value</param>
        public void setNodeSolidBoundary(bool isSolid)
        {
            if (useMyPUPPIcanvas != null)
            {
                if (useMyPUPPIcanvas.stacks.ContainsKey(GUID.ToString()))
                {
                    useMyPUPPIcanvas.stacks[GUID.ToString()].isSolid = isSolid;
                }
            }
        }

        /// <summary>
        /// Restore node color to default
        /// </summary>
        public void unSetNodeColor()
        {
            moduleNodeColorOverride = false;
            if (useMyPUPPIcanvas != null)
            {
                if (useMyPUPPIcanvas.stacks.ContainsKey(GUID.ToString()))
                {
                    useMyPUPPIcanvas.stacks[GUID.ToString()].nodeUpdatedMaterial = null;
                    forceMyNodeToUpdate();
                }
            }

        }
        internal bool moduleNodeColorOverride = false;
        internal double moduleNodeColorRed = 0;
        internal double moduleNodeColorGreen = 0;
        internal double moduleNodeColorBlue = 0;

        //how many times this has been called. used for now just to set defaults for outputs.
        internal long numbercalls = 0;
        //process everything as an array
        internal bool listprocess = false;
        /// <summary>
        /// Number of elements in a list, if node set for list processing. 0 means no list (single element processing).
        /// </summary>
        public int countListMode = 0;
        /// <summary>
        /// Position in list of modules when doing list processing.
        /// </summary>
        public int inlist = 0;
        /// <summary>
        /// If this is set to true, the process_usercode function has complete control of how the module handles inputs and outputs when running.
        /// When set to true, the developer has to work directly with the inputs and outputs arraylists.
        /// </summary>
        public bool completeProcessOverride = false;
        /// <summary>
        /// If this is set to true, the doubleClickMe_userCode function has complete control of how the module handles inputs and outputs when the end user double clicks on it from the GUI.
        /// When set to true, the developer has to work directly with the inputs and outputs arraylists.
        /// </summary>
        public bool completeDoubleClickOverride = false;
        /// <summary>
        /// If this is set to true, the gestureMe_userCode function has complete control of how the module handles inputs and outputs when the end user drags over a node from the GUI.
        /// When set to true, the developer has to work directly with the inputs and outputs arraylists.
        /// </summary>
        public bool completeGestureOverride = false;
        /// <summary>
        /// If this is set to true, only one module of this type can exist on the canvas.
        /// </summary>
        public bool unique = false;
        //if we want to fire it back up for loops for example
        //bool doreupstream = false;
        //which input to send back through
        //int doreupstreamiv = 0;
        //which input to set
        //int doreupstreamuiv = 0;
        //the value
        // object doreupstreamv = null;
        //this updates input of another object that this object has changed
        internal event processhandler prochan;
        internal delegate void processhandler();
        //(object sender,  ProcessEventArgs data);
        //number of nodes upstream left to process. this way we don't end up with processing a node a bunch of times
        internal long upstreamleft = 0;
        /// <summary>
        /// Sets the maximum number of children the node can have. Set it to -1 to follow defaults in PUPPIGUISettings.
        /// </summary>
        public int maxChildren = -1;
        /// <summary>
        /// Sets the maximum number of outgoing connections the node can have. Set it to less than 0 to allow infinte number.
        /// </summary>
        public int maxOutgoingConnections = -1;



        /// <summary>
        /// Default base constructor for the PUPPIModule partial class
        /// </summary>
        public PUPPIModule()
        {
            inputs = new List<PUPPIInParameter>();
            outputs = new ArrayList();
            defaultoutputs = new ArrayList();
            dblclkpreviousoutputs = new ArrayList();
            outputnames = new List<string>();

            //need to keep track of how many things are connected for good visualization offsets
            //outputconnnumbers=new List<int>();
            inputnames = new List<string>();
            methodparms = new List<methodparm>();
            GUID = -1;

        }
        /// <summary>
        /// Gets the number of nodes connected to output
        /// </summary>
        /// <param name="outputIndex">valid output index (zero based)</param>
        /// <returns>Number of nodes connected to output</returns>
        public int getNumberOutboundConnections(int outputIndex)
        {
            try
            {
                if (outputIndex >= 0 && outputIndex < outputs.Count)
                {
                    if (useMyPUPPIcanvas != null)
                    {
                        if (useMyPUPPIcanvas.stacks.ContainsKey(GUID.ToString()))
                        {
                            return useMyPUPPIcanvas.stacks[GUID.ToString()].vizoutputs[outputIndex].outboundconns.Count;
                        }
                    }
                    return 0;
                }
            }
            catch
            {

            }
            return 0;
        }
        //gets the number of mandatory inputs
        internal int mandinputs()
        {
            int rr = 0;
            for (int i = 0; i < inputs.Count; i++)
            {
                if (inputs[i].isoptional == false) rr++;
            }
            return rr;
        }
        /// <summary>
        /// Forces graphics update with optional movement of a module's node representation. 
        /// Can be called for instance at the end of process_usercode or gesture_usercode for nodes with custom renderer
        /// </summary>
        /// <param name="moveX">displacement on x axis,defaults to 0</param>
        /// <param name="moveY">displacement on y axis, defaults to 0</param>
        public void forceMyNodeToUpdate(double moveX = 0, double moveY = 0)
        {
            if (useMyPUPPIcanvas != null)
            {
                useMyPUPPIcanvas.forceNodeMoveUpdate(GUID.ToString(), moveX, moveY);
            }
        }

        /// <summary>
        /// Forces lightwaeight graphics update of node with custom renderer
        /// Can be called for instance from the dragOver_visualUpdate_usercode function to update node appearance in real time as user drags over node.
        /// </summary>
        /// <param name="moveX">displacement on x axis,defaults to 0</param>
        /// <param name="moveY">displacement on y axis, defaults to 0</param>
        public void forceMyNodeCustomRendererToUpdate()
        {
            if (useMyPUPPIcanvas != null)
            {
                useMyPUPPIcanvas.forceNodeCustomRendererUpdate(GUID.ToString());
            }
        }

        //resets the number calls to 1 to speed up processing when all connections regenrated

        //this does the checks so the user doesn't ahve to bother with them
        //full overwrite means the user code handles all the inputs and outputs directly
        internal void process()
        {
            concurrentProcesses++;
            try
            {


                //doreupstream = false;
                //set default values
                if (numbercalls == 0)
                {
                    for (int ocounter = 0; ocounter < outputs.Count; ocounter++)
                    {
                        defaultoutputs.Add(outputs[ocounter]);
                        //outputconnnumbers.Add(0); 
                    }

                }
                numbercalls++;
                //if loading a whole file,we process only after all inputs have been processed
                //if (numbercalls < inputs.Count && runtimesettings.fullregen == true) return;
                if (PUPPIDebugger.debugenabled)
                {
                    PUPPIDebugger.log("begin " + name + utils.StringConstants.processdebuglog + GUID.ToString() + ".Call# " + numbercalls.ToString());
                }


                {
                    if (completeProcessOverride == false)
                    {





                        //checks all inputs to make sure they are not null
                        for (int i = 0; i < inputs.Count; i++)
                        {
                            if ((inputs[i] == null || inputs[i].module == null) && inputs[i].isoptional == false)
                            {
                                //clears output to default

                                setdefaultoutputs();

                                concurrentProcesses--;
                                return;
                            }

                        }
                        //checks if lists are defined well
                        //only if we have inputs
                        if (inputs.Count > 0)
                        {
                            countListMode = 0;
                            if (listprocess == true)
                            {

                                for (int ii = 0; ii < inputs.Count; ii++)
                                {
                                    if (inputs[ii].module != null && inputs[ii].module.outputs[inputs[ii].outParIndex] != null && !inputs[ii].inputAutomaticListMode)
                                    {
                                        //check if array to figure out the count
                                        int alm = 0;
                                        if (inputs[ii].module.outputs[inputs[ii].outParIndex] is ICollection)

                                            alm = (inputs[ii].module.outputs[inputs[ii].outParIndex] as ICollection).Count;
                                        else if (inputs[ii].module.outputs[inputs[ii].outParIndex] is IEnumerable)
                                            //slower
                                            alm = PUPPIModule.makeMeAnArrayList(inputs[ii].module.outputs[inputs[ii].outParIndex] as IEnumerable).Count;

                                        if (alm == 0 || (countListMode != 0 && alm != countListMode))
                                        {
                                            setdefaultoutputs();
                                            concurrentProcesses--;
                                            return;
                                        }
                                        else
                                        {
                                            countListMode = alm;
                                        }
                                        //}
                                    }
                                    //not fully connected
                                    else if (inputs[ii].isoptional == false && !inputs[ii].inputAutomaticListMode)
                                    {

                                        setdefaultoutputs();

                                        concurrentProcesses--;
                                        return;
                                    }
                                }



                            }
                        }

                        //process override as arrays
                        if (listprocess == true)
                        {

                            if (numbercalls > PUPPIGUISettings.stackOverflowGuard * countListMode && PUPPIGUISettings.stackOverflowGuard * countListMode > 0)
                            {
                                PUPPIruntimesettings.processyesno = false;
                                MessageBox.Show("Stack Overflow Guard Activated. Program Aborted!");
                                concurrentProcesses--;
                                return;

                            }
                            //only run this if we have inputs
                            if (inputs.Count > 0)
                            {
                                //reset outputs to blank arrays
                                for (int oo = 0; oo < outputs.Count; oo++)
                                {
                                    outputs[oo] = new ArrayList();
                                }
                                for (int lcount = 0; lcount < countListMode; lcount++)
                                {
                                    //reset the inputs for user code so we can run the same function multiple times
                                    inlist = lcount;
                                    usercodeinputs = new ArrayList();
                                    for (int ii = 0; ii < inputs.Count; ii++)
                                    {

                                        if (inputs[ii].isoptional == true)
                                        {
                                            try
                                            {
                                                //clones input
                                                if (inputs[ii].inputAutomaticListMode)
                                                {
                                                    usercodeinputs.Add(inputs[ii].module.outputs[inputs[ii].outParIndex]);
                                                }
                                                else
                                                {
                                                    ArrayList arra = new ArrayList();
                                                    if (inputs[ii].module.outputs[inputs[ii].outParIndex] is ICollection)
                                                        arra = new ArrayList(inputs[ii].module.outputs[inputs[ii].outParIndex] as ICollection);
                                                    else if (inputs[ii].module.outputs[inputs[ii].outParIndex] is IEnumerable)
                                                        arra = PUPPIModule.makeMeAnArrayList(inputs[ii].module.outputs[inputs[ii].outParIndex] as IEnumerable);
                                                    usercodeinputs.Add(arra[lcount]);
                                                }
                                            }
                                            catch
                                            {
                                                usercodeinputs.Add(null);
                                            }
                                        }
                                        //onky try catch if it's optional so we don't lose performance cause we already checked
                                        else
                                        {
                                            //clones input
                                            if (inputs[ii].inputAutomaticListMode)
                                            {
                                                usercodeinputs.Add(inputs[ii].module.outputs[inputs[ii].outParIndex]);
                                            }
                                            else
                                            {
                                                ArrayList arra = new ArrayList();
                                                if (inputs[ii].module.outputs[inputs[ii].outParIndex] is ICollection)
                                                    arra = new ArrayList(inputs[ii].module.outputs[inputs[ii].outParIndex] as ICollection);
                                                else if (inputs[ii].module.outputs[inputs[ii].outParIndex] is IEnumerable)
                                                    arra = PUPPIModule.makeMeAnArrayList(inputs[ii].module.outputs[inputs[ii].outParIndex] as IEnumerable);
                                                usercodeinputs.Add(arra[lcount]);
                                            }
                                        }
                                    }
                                    //set user code outputs to default values
                                    //these are used by the user changed processing code so that they don't worry
                                    //about list mode
                                    usercodeoutputs = new ArrayList();
                                    if (defaultoutputs.Count == outputs.Count)
                                    {
                                        for (int oo = 0; oo < outputs.Count; oo++)
                                        {
                                            usercodeoutputs.Add(defaultoutputs[oo]);
                                        }
                                    }
                                    else
                                    {
                                        for (int oo = 0; oo < outputs.Count; oo++)
                                        {
                                            usercodeoutputs.Add("Not set");
                                        }
                                    }
                                    //user defined code or class code
                                    if (isconstructor == true)
                                    {
                                        construct_me();
                                    }
                                    else
                                    {
                                        if (methodparms.Count == 0)
                                        {
                                            process_usercode();
                                        }
                                        else
                                        {

                                            method_me();

                                        }
                                    }
                                    for (int oo = 0; oo < outputs.Count; oo++)
                                    {
                                        (outputs[oo] as ArrayList).Add(usercodeoutputs[oo]);
                                    }

                                }
                            }

                        }
                        else
                        {
                            if (numbercalls > PUPPIGUISettings.stackOverflowGuard && PUPPIGUISettings.stackOverflowGuard > 0)
                            {
                                PUPPIruntimesettings.processyesno = false;
                                MessageBox.Show("Stack Overflow Guard Activated. Program Aborted!");
                                concurrentProcesses--;
                                return;

                            }
                            if (inputs.Count > 0)
                            {
                                //reset the inputs for user code so we can run the same function multiple times
                                usercodeinputs = new ArrayList();
                                for (int ii = 0; ii < inputs.Count; ii++)
                                {

                                    if (inputs[ii].isoptional == false)
                                    {
                                        usercodeinputs.Add(inputs[ii].module.outputs[inputs[ii].outParIndex]);
                                    }
                                    //for optional we use try-catch to add null if errors
                                    else
                                    {
                                        try
                                        {
                                            usercodeinputs.Add(inputs[ii].module.outputs[inputs[ii].outParIndex]);
                                        }
                                        catch
                                        {
                                            usercodeinputs.Add(null);
                                        }
                                    }
                                }
                                //set user code outputs to default values
                                usercodeoutputs = new ArrayList();
                                if (defaultoutputs.Count == outputs.Count)
                                {
                                    for (int oo = 0; oo < outputs.Count; oo++)
                                    {
                                        usercodeoutputs.Add(defaultoutputs[oo]);
                                    }
                                }
                                else
                                {
                                    for (int oo = 0; oo < outputs.Count; oo++)
                                    {
                                        usercodeoutputs.Add("Not set");
                                    }
                                }
                                //user defined code or class code
                                if (isconstructor == true)
                                {
                                    construct_me();
                                }
                                else
                                {
                                    if (methodparms.Count == 0)
                                    {
                                        process_usercode();
                                    }
                                    else
                                    {

                                        method_me();

                                    }
                                }
                                for (int oo = 0; oo < outputs.Count; oo++)
                                {
                                    outputs[oo] = (usercodeoutputs[oo]);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (numbercalls > PUPPIGUISettings.stackOverflowGuard && PUPPIGUISettings.stackOverflowGuard > 0)
                        {
                            PUPPIruntimesettings.processyesno = false;
                            MessageBox.Show("Stack Overflow Guard Activated. Program Aborted!");
                            concurrentProcesses--;
                            return;

                        }
                        process_usercode();
                    }
                }
                if (PUPPIDebugger.debugenabled)
                {
                    PUPPIDebugger.log("end " + name + utils.StringConstants.processdebuglog + GUID.ToString() + ".Call# " + numbercalls.ToString());
                }
                //   concurrentProcesses--; 
                if (false)//(doreupstream == true)
                {
                    //if (completeProcessOverride==false )
                    //{
                    //    throw new Exception("Upstream reprocess only works from nodes with complete process override"); 
                    //}
                    //else
                    //{
                    //    //upstream proc
                    //    if (doreupstreamiv<inputs.Count  )
                    //    {
                    //        if (inputs[doreupstreamiv ]!=null && inputs[doreupstreamiv ].module!=null )
                    //        {
                    //            if (inputs[doreupstreamiv].module.inputs.Count > doreupstreamuiv)
                    //            {
                    //                PUPPIInParameter pi = inputs[doreupstreamiv].module.inputs[doreupstreamuiv];
                    //                if (pi!=null && pi.module!=null )
                    //                {
                    //                    pi.module.outputs[pi.outParIndex] = doreupstreamv; 
                    //                }
                    //                inputs[doreupstreamiv].module.doIprocess();
                    //            }
                    //        }
                    //    }
                    //}

                    //PUPPIModel.PUPPIPremadeModules.LogicModules.PUPPIConditional pppc = this as PUPPIModel.PUPPIPremadeModules.LogicModules.PUPPIConditional;
                    //if (pppc.doprocess)

                    //    useMyPUPPIcanvas.setMyNodeOnCanvasInput(pppc.ngi.ToString(), pppc.ii, pppc.setme);

                }
                else
                    if (prochan != null && !haltProcess)
                    {
                        prochan();
                    }
                //clear errors
                if (moduleNodeColorRed == 1 && moduleNodeColorBlue == 0 && moduleNodeColorGreen == 0 && moduleNodeColorOverride == true) unSetNodeColor();
            }
            catch (Exception exy)
            {
                if (PUPPIDebugger.debugenabled)
                {
                    PUPPIDebugger.log("Node processing error: " + exy.ToString());
                }
                for (int oo = 0; oo < outputs.Count; oo++)
                {
                    outputs[oo] = "Processing Error";
                }
                setNodeColor(1, 0, 0);

            }
            concurrentProcesses--;
            //if (PUPPIModel.PUPPIModule.useMyPUPPIcanvas   != null)
            //{
            //    PUPPIModel.PUPPIModule.useMyPUPPIcanvas.lastChanged = DateTime.Now;

            //    if (PUPPIruntimesettings.PUPPICanvasTCPServerIsRunning)
            //    {
            //        PUPPICanvas.currentCanvasStatusServer = PUPPIModel.PUPPIModule.useMyPUPPIcanvas.readMyCanvasStatus();
            //        PUPPICanvas.currentXMLRepServer = PUPPIModel.PUPPIModule.useMyPUPPIcanvas.saveCanvasRepresentationToXML();
            //    }
            //}
        }
        internal bool haltProcess = false;
        internal void addCanvasCommand(string commandAndArguments)
        {
            if (useMyPUPPIcanvas != null)
            {
                useMyPUPPIcanvas.commandQueue.Add(commandAndArguments);
            }
        }

        /// <summary>
        /// Can be called to make an independent node/module process inputs provided as argument.
        /// </summary>
        /// <param name="procInputs">ArrayList of inputs, must match in count the inputs of this module</param>
        /// <param name="procListMode">If true, each input is treated as a list</param>
        public void processExplicit(ArrayList procInputs, bool procListMode = false)
        {
            concurrentProcesses++;
            try
            {
                try
                {
                    // System.Windows.Forms.MessageBox.Show("removing event ");
                    AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                }
                catch { } //System.Windows.Forms.MessageBox.Show("aDDING event ");
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            //doreupstream = false;
            //set default values
            if (numbercalls == 0)
            {
                for (int ocounter = 0; ocounter < outputs.Count; ocounter++)
                {
                    defaultoutputs.Add(outputs[ocounter]);
                    //outputconnnumbers.Add(0); 
                }

            }
            numbercalls++;

            if (completeProcessOverride == false)
            {

                if (procInputs.Count != inputs.Count) throw new Exception("Input count mismatch");
                //checks all inputs to make sure they are not null
                for (int i = 0; i < inputs.Count; i++)
                {
                    if (procInputs[i] == null && inputs[i].isoptional == false)
                    {
                        //clears output to default

                        setdefaultoutputs();

                        concurrentProcesses--;
                        return;
                    }

                }
                //checks if lists are defined well
                //only if we have inputs
                if (inputs.Count > 0)
                {
                    countListMode = 0;
                    if (listprocess == true)
                    {

                        for (int ii = 0; ii < inputs.Count; ii++)
                        {
                            if (procInputs[ii] != null && !inputs[ii].inputAutomaticListMode)
                            {
                                //check if array to figure out the count
                                int alm = 0;
                                if (procInputs[ii] is ICollection)

                                    alm = (procInputs[ii] as ICollection).Count;
                                else if (procInputs[ii] is IEnumerable)
                                    //slower
                                    alm = PUPPIModule.makeMeAnArrayList(procInputs[ii] as IEnumerable).Count;

                                if (alm == 0 || (countListMode != 0 && alm != countListMode))
                                {
                                    setdefaultoutputs();
                                    return;
                                }
                                else
                                {
                                    countListMode = alm;
                                }
                                //}
                            }
                            //not fully connected
                            else if (inputs[ii].isoptional == false && !inputs[ii].inputAutomaticListMode)
                            {

                                setdefaultoutputs();

                                concurrentProcesses--;
                                return;
                            }
                        }



                    }
                }

                //process override as arrays
                if (listprocess == true)
                {

                    if (numbercalls > PUPPIGUISettings.stackOverflowGuard * countListMode && PUPPIGUISettings.stackOverflowGuard * countListMode > 0)
                    {
                        PUPPIruntimesettings.processyesno = false;
                        MessageBox.Show("Stack Overflow Guard Activated. Program Aborted!");
                        concurrentProcesses--;
                        return;

                    }
                    //only run this if we have inputs
                    if (inputs.Count > 0)
                    {
                        //reset outputs to blank arrays
                        for (int oo = 0; oo < outputs.Count; oo++)
                        {
                            outputs[oo] = new ArrayList();
                        }
                        for (int lcount = 0; lcount < countListMode; lcount++)
                        {
                            //reset the inputs for user code so we can run the same function multiple times
                            usercodeinputs = new ArrayList();
                            for (int ii = 0; ii < inputs.Count; ii++)
                            {

                                if (inputs[ii].isoptional == true)
                                {
                                    try
                                    {
                                        //clones input
                                        if (inputs[ii].inputAutomaticListMode)
                                        {
                                            usercodeinputs.Add(procInputs[ii]);
                                        }
                                        else
                                        {
                                            ArrayList arra = new ArrayList();
                                            if (procInputs[ii] is ICollection)
                                                arra = new ArrayList(procInputs[ii] as ICollection);
                                            else if (procInputs[ii] is IEnumerable)
                                                arra = PUPPIModule.makeMeAnArrayList(procInputs[ii] as IEnumerable);
                                            usercodeinputs.Add(arra[lcount]);
                                        }
                                    }
                                    catch
                                    {
                                        usercodeinputs.Add(null);
                                    }
                                }
                                //onky try catch if it's optional so we don't lose performance cause we already checked
                                else
                                {
                                    //clones input
                                    if (inputs[ii].inputAutomaticListMode)
                                    {
                                        usercodeinputs.Add(procInputs[ii]);
                                    }
                                    else
                                    {
                                        ArrayList arra = new ArrayList();
                                        if (procInputs[ii] is ICollection)
                                            arra = new ArrayList(procInputs[ii] as ICollection);
                                        else if (procInputs[ii] is IEnumerable)
                                            arra = PUPPIModule.makeMeAnArrayList(procInputs[ii] as IEnumerable);
                                        usercodeinputs.Add(arra[lcount]);
                                    }
                                }
                            }
                            //set user code outputs to default values
                            //these are used by the user changed processing code so that they don't worry
                            //about list mode
                            usercodeoutputs = new ArrayList();
                            for (int oo = 0; oo < outputs.Count; oo++)
                            {
                                usercodeoutputs.Add(defaultoutputs[oo]);
                            }
                            //user defined code or class code
                            if (isconstructor == true)
                            {
                                construct_me();
                            }
                            else
                            {
                                if (methodparms.Count == 0)
                                {
                                    process_usercode();
                                }
                                else
                                {

                                    method_me();

                                }
                            }
                            for (int oo = 0; oo < outputs.Count; oo++)
                            {
                                (outputs[oo] as ArrayList).Add(usercodeoutputs[oo]);
                            }

                        }
                    }
                }
                else
                {
                    if (numbercalls > PUPPIGUISettings.stackOverflowGuard && PUPPIGUISettings.stackOverflowGuard > 0)
                    {
                        PUPPIruntimesettings.processyesno = false;
                        MessageBox.Show("Stack Overflow Guard Activated. Program Aborted!");
                        concurrentProcesses--;
                        return;

                    }
                    if (inputs.Count > 0)
                    {
                        //reset the inputs for user code so we can run the same function multiple times
                        usercodeinputs = new ArrayList();
                        for (int ii = 0; ii < inputs.Count; ii++)
                        {

                            if (inputs[ii].isoptional == false)
                            {
                                usercodeinputs.Add(procInputs[ii]);
                            }
                            //for optional we use try-catch to add null if errors
                            else
                            {
                                try
                                {
                                    usercodeinputs.Add(procInputs[ii]);
                                }
                                catch
                                {
                                    usercodeinputs.Add(null);
                                }
                            }
                        }
                        //set user code outputs to default values
                        usercodeoutputs = new ArrayList();
                        for (int oo = 0; oo < outputs.Count; oo++)
                        {
                            usercodeoutputs.Add(defaultoutputs[oo]);
                        }
                        //user defined code or class code
                        if (isconstructor == true)
                        {
                            construct_me();
                        }
                        else
                        {
                            if (methodparms.Count == 0)
                            {
                                process_usercode();
                            }
                            else
                            {

                                method_me();

                            }
                        }
                        for (int oo = 0; oo < outputs.Count; oo++)
                        {
                            outputs[oo] = (usercodeoutputs[oo]);
                        }
                    }
                }
            }
            else
            {
                if (numbercalls > PUPPIGUISettings.stackOverflowGuard && PUPPIGUISettings.stackOverflowGuard > 0)
                {
                    PUPPIruntimesettings.processyesno = false;
                    MessageBox.Show("Stack Overflow Guard Activated. Program Aborted!");
                    concurrentProcesses--;
                    return;

                }
                process_usercode();
            }

            if (false)//(doreupstream == true)
            {

            }
            else
                if (prochan != null)
                {
                    prochan();
                }
            concurrentProcesses--;
        }

        /// <summary>
        /// Can set the caption of the node using the current module.
        /// </summary>
        /// <param name="captionTextx">The text to set the caption to</param>
        /// <param name="captionIndex">Modules with custom renderers can have multiple captions so the index is used. For regular nodes the index is disregarded.</param>
        public void setNodeCaption(string captionText, int captionIndex = 0)
        {
            if (useMyPUPPIcanvas != null)
                useMyPUPPIcanvas.updateNodeCaptionProgrammatically(captionText, captionIndex, GUID.ToString());


        }
        /// <summary>
        /// Causes the node upstream of the input index to reprocess with specified value on specified input.
        /// Can be used for loops. Caution, you might end up in an infinite loop.
        /// Only works if complete process override enabled.
        /// </summary>
        /// <param name="inputIndex">A valid input index.</param>
        /// <param name="upstreamInputIndex">A valid input index for upstream node</param>
        /// <param name="upstreamValue">The value to set the upstream's node input to.</param>
        //public void sendUpstreamSignal(int inputIndex,int upstreamInputIndex, object upstreamValue)
        //{
        //    if (inputIndex>=inputs.Count )
        //    {
        //        throw new Exception("Invalid input index for upstream process!");
        //    }
        //    else
        //    {
        //        doreupstreamiv = inputIndex;
        //        doreupstreamuiv = upstreamInputIndex;
        //        doreupstreamv = upstreamValue;  
        //        doreupstream = true;
        //    }
        //}

        //sets outputs to default values
        private void setdefaultoutputs()
        {
            if (PUPPIDebugger.debugenabled)
            {
                PUPPIDebugger.log(utils.StringConstants.setdefaultoutputsdebuglog + name + ".Node GUID " + GUID.ToString());
            }
            for (int ocounter = 0; ocounter < outputs.Count; ocounter++)
            {
                if (ocounter < defaultoutputs.Count)
                {
                    outputs[ocounter] = defaultoutputs[ocounter];
                }
            }
            //so we update inputs and outputs with default values
            //and visually
            if (prochan != null)
            {
                prochan();
            }

        }
        /// <summary>
        /// gets the modules downstream in a tree node format
        /// </summary>
        /// <returns>a TreeNode with this node being the root</returns>
        public TreeNode getDownstreamTree()
        {
            TreeNode tn = new TreeNode();
            tn.Name = GUID.ToString();
            tn.Text = name;
            for (int i = 0; i < outputs.Count; i++)
            {
                List<PUPPIModule> pl = getDownstreamModules(i);
                foreach (PUPPIModule p in pl)
                {
                    tn.Nodes.Add(p.getDownstreamTree());
                }
            }
            return tn;
        }

        /// <summary>
        /// gets the upstream modules in a tree node format
        /// </summary>
        /// <returns>a TreeNode with this node being the root</returns>
        public TreeNode getUpstreamTree()
        {
            TreeNode tn = new TreeNode();
            tn.Name = GUID.ToString();
            tn.Text = name;
            for (int i = 0; i < inputs.Count; i++)
            {
                PUPPIModule pl = getUpstreamModule(i);
                if (pl != null)
                    tn.Nodes.Add(pl.getUpstreamTree());

            }
            return tn;
        }

        /// <summary>
        /// This method is overriden to customize the behavior of the module when running a visual program.
        /// This function is not called if the node has no inputs, so make sure to set usercode_outputs outputs through gestures or double clicks.
        /// </summary>
        public virtual void process_usercode()
        {
            //read from usercode inputs and outputs to usercode outputs
            //list levels handled automatically, so ideally this handles one input and one output value per input and output

        }
        //this gets put in the process_usercode if it's a class
        /// <summary>
        /// Gets the custom renderer of the canvas representation of this PUPPIModule, in order to change node appearance at runtime.
        /// </summary>
        /// <returns>the PUPPINodeCustomRenderer associated to the node, or null if the node does not have a custom renderer (rendered as box)</returns>
        public PUPPINodeCustomRenderer getNodeCustomRenderer()
        {
            if (useMyPUPPIcanvas != null)
            {
                return useMyPUPPIcanvas.getNodeCustomRendererByID(GUID.ToString());
            }
            return null;
        }

        internal bool connect_input(PUPPIInParameter pinp, int inputindex, bool msg, bool checkOnCanvas = true)
        {


            if (pinp != null && inputindex >= 0 && inputs.Count > inputindex)
            {


                bool circ = false;

                //if (PMsubModules.Count > 0)
                //{

                //    return true;
                //}
                //else
                //{

                if (pinp.module != null)
                {
                    circ = check_ancestry(this, pinp.module);
                }
                if (circ == true)
                {
                    if (msg) System.Windows.Forms.MessageBox.Show("Circular logic,cannot connect!");
                    return false;
                }
                if (inputs[inputindex] != null)
                {
                    if (inputs[inputindex].module != null)
                    {
                        return false;
                    }
                }
                //keep the optionality
                bool isopt = inputs[inputindex].isoptional;
                inputs[inputindex] = pinp;
                inputs[inputindex].isoptional = isopt;
                //pinp.module.outputconnnumbers[pinp.outParIndex]++;

                if (checkOnCanvas)
                {
                    try
                    {

                        if (useMyPUPPIcanvas != null)
                        {
                            PUPPICanvas.ModViz3DNode mv = useMyPUPPIcanvas.stacks[inputs[inputindex].module.GUID.ToString()];
                            //get outbound node
                            int re = 0;
                            // int di=Math.DivRem (mv.number_outbound_connections,PUPPIGUISettings.maxConnPerSpace ,out re  );
                            //need to fore redraw
                            mv.number_outbound_connections++;
                            //if (di>0 && re==0)
                            //{
                            // //   useMyPUPPIcanvas.initconn_matrices();    
                            //}

                        }

                    }
                    catch (Exception exy)
                    {
                        if (PUPPIDebugger.debugenabled)
                        {
                            PUPPIDebugger.log(" Module GUID " + GUID.ToString() + "Not found on canvas " + inputindex.ToString() + ":\n" + exy.ToString());
                        }
                        return false;
                    }
                }
                connectevent(pinp);
                //useMyPUPPIcanvas.stacks[GUID.ToString()].number_outbound_connections++;
                return true;
                //}
            }

            return false;
        }
        //this is just the event
        internal void connectevent(PUPPIInParameter pinp)
        {
            pinp.inchan += doIprocess;
        }
        //we can't just process every time, we need to make sure we don't just have to transmit  further upstream
        internal void doIprocess()
        {
            if (PUPPIGUI.PUPPIruntimesettings.processyesno == true)
            {
                //in run program mode, to make it faster, will only process if all nodes upstream have been processed
                if (upstreamleft > 0)
                {
                    upstreamleft--;
                }
                if (upstreamleft == 0)
                    
                    process();
            }

        }
        //this way we set up upstream
        internal void userdoIprocess()
        {
            if (PUPPIGUI.PUPPIruntimesettings.processyesno == true)
            {
                resetUSLD();
                increaseUSLD(new List<PUPPIInParameter>());
            }
            doIprocess();
        }
        //increase upstreamleft by one recursively for nodes downstream
        //make sure each input only increased once
        internal void increaseUSLD(List<PUPPIInParameter> pl)
        {
            for (int i = 0; i < outputs.Count; i++)
                foreach (PUPPIModule p in getDownstreamModules(i))
                {
                    bool finput = false;
                    for (int ii = 0; ii < p.inputs.Count; ii++)
                    {
                        if (p.inputs[ii].module == this)
                        {
                            //not already added
                            if (pl.Contains(p.inputs[ii]) == false)
                            {

                                p.upstreamleft++;
                                pl.Add(p.inputs[ii]);
                                finput = true;

                            }

                        }


                    }
                    if (finput) p.increaseUSLD(pl);

                }
        }


        //uses the list no canvas
        internal void increaseUSLDList(List<PUPPIInParameter> pl, List<PUPPIModule> pml)
        {
            for (int i = 0; i < outputs.Count; i++)
                foreach (PUPPIModule p in getDownstreamModulesFromList(i, pml))
                {
                    bool finput = false;
                    for (int ii = 0; ii < p.inputs.Count; ii++)
                    {
                        if (p.inputs[ii].module == this)
                        {
                            //not already added
                            if (pl.Contains(p.inputs[ii]) == false)
                            {

                                p.upstreamleft++;
                                pl.Add(p.inputs[ii]);
                                finput = true;

                            }

                        }


                    }
                    if (finput) p.increaseUSLDList(pl, pml);

                }
        }

        internal void resetUSLD()
        {
            for (int i = 0; i < outputs.Count; i++)
                foreach (PUPPIModule p in getDownstreamModules(i))
                {

                    p.upstreamleft = 0;
                    p.resetUSLD();

                }
        }
        //no canvas
        internal void resetUSLDList(List<PUPPIModule> plist)
        {
            for (int i = 0; i < outputs.Count; i++)
                foreach (PUPPIModule p in getDownstreamModulesFromList(i, plist))
                {

                    p.upstreamleft = 0;
                    p.resetUSLDList(plist);

                }
        }

        //called from outside to process in new thread not working
        internal void newthreadprocess()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(
          new System.Threading.WaitCallback(procthread));
        }

        //gets a ranking

        //try multithread for processing-not working
        private void procthread(Object stateInfo)
        {

            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Send,
                                      (ThreadStart)delegate() { process(); });
        }

        //disconencts from another output

        internal bool disconnect_input(int inputindex, bool checkOnCanvas = true)
        {
            if (inputindex >= 0 && inputs.Count > inputindex)
            {
                if (inputs[inputindex] != null)
                {


                    //remove handler
                    try
                    {
                        inputs[inputindex].inchan -= doIprocess;
                    }
                    catch
                    {

                    }
                    if (checkOnCanvas)
                    {
                        try
                        {
                            if (useMyPUPPIcanvas != null)
                            {
                                PUPPICanvas.ModViz3DNode mv = useMyPUPPIcanvas.stacks[inputs[inputindex].module.GUID.ToString()];
                                //get outbound node
                                int re = 0;
                                //int di = Math.DivRem(mv.number_outbound_connections, PUPPIGUISettings.maxConnPerSpace, out re);
                                //need to fore redraw
                                if (mv.number_outbound_connections > 0)
                                    mv.number_outbound_connections--;
                                //if (di > 0 && re == 0)
                                //{
                                //   // useMyPUPPIcanvas.initconn_matrices();
                                //}
                            }
                        }
                        catch (Exception exy)
                        {
                            if (PUPPIDebugger.debugenabled)
                            {
                                PUPPIDebugger.log(" Module GUID " + GUID.ToString() + "Not found on canvas " + inputindex.ToString() + ":\n" + exy.ToString());
                            }
                            return false;
                        }
                    }

                    inputs[inputindex].module = null;

                    //process so that outputs update

                    doIprocess();
                    return true;
                }
            }
            return false;
        }
        //checks ancestry to ensure we don't have a circular reference
        internal bool check_ancestry(PUPPIModule lastone, PUPPIModule nextmodule)
        {
            bool circularity = false;
            if (nextmodule == lastone)
            {
                circularity = true;
            }
            else
            {
                for (int i = 0; i < nextmodule.inputs.Count; i++)
                {
                    PUPPIInParameter pinp = nextmodule.inputs[i];
                    if (pinp != null)
                    {
                        PUPPIModule pm = pinp.module;
                        if (pm != null)
                        {
                            circularity = check_ancestry(lastone, pm);
                        }
                        if (circularity == true) break;
                    }
                }
            }

            return circularity;
        }
        /// <summary>
        /// Sets the module number of calls made to zero. Used when number of inputs or outputs changes, when module first called for proccessing it generates the default outputs from current outputs.
        /// </summary>
        public void resetModuleNumberCalls()
        {
            numbercalls = 0;
        }
        /// <summary>
        /// If set to true, when double clicking, the doubleClickMe_userCode is called only once even if module in list mode with multiple inputs.
        /// </summary>
        public bool oneTimeDoubleClickListInteraction = false;


        //what happens when we double click on this!
        internal void doubleclickme(double clickXRatio, double clickYRatio, double clickZRatio)
        {
            bool stopDBU = false;
            if (useMyPUPPIcanvas != null)
                useMyPUPPIcanvas.resetNumberCallsOnInteraction();
            //prepare inputs and outputs for setting
            dblclkpreviousoutputs.Clear();
            for (int oi = 0; oi < outputs.Count; oi++)
            {
                dblclkpreviousoutputs.Add(outputs[oi] as object);
            }
            if (completeDoubleClickOverride == false)
            {
                if (listprocess == true)
                {
                    if (countListMode == 0)
                    {
                        //have user input number of elements if setting list
                        string celements = "0";
                        formutils.InputBox("Please enter number of elements for list mode", "", ref celements);
                        countListMode = Convert.ToInt32(celements);
                        if (countListMode <= 0)
                        {
                            countListMode = 1;
                        }
                    }
                    //reset outputs to blank arrays
                    for (int oo = 0; oo < outputs.Count; oo++)
                    {
                        outputs[oo] = new ArrayList();
                    }
                    for (int lcount = 0; lcount < countListMode; lcount++)
                    {

                        //set user code outputs to default values
                        //these are used by the user changed processing code so that they don't worry
                        //about list mode
                        usercodeoutputs = new ArrayList();
                        for (int oo = 0; oo < outputs.Count; oo++)
                        {
                            //    try
                            //    {
                            //        usercodeoutputs.Add(outputs[oo]);  
                            //    }
                            //    catch
                            //{
                            usercodeoutputs.Add(null);
                            //}
                        }
                        //store, in case we anna display that
                        inlist = lcount;
                        if (!stopDBU)
                        {
                            doubleClickMe_userCode(clickXRatio, clickYRatio, clickZRatio);
                            if (oneTimeDoubleClickListInteraction) stopDBU = true;

                        }
                        if (this.GetType().GetMethod("doubleClickMe_userCode").DeclaringType == this.GetType())
                        {
                            if (usercodeoutputs.Count == outputs.Count)
                                for (int oo = 0; oo < outputs.Count; oo++)
                                {
                                    //if (usercodeoutputs.Count>oo  )
                                    if (outputs[oo] is ArrayList)
                                        (outputs[oo] as ArrayList).Add(usercodeoutputs[oo]);
                                    //else
                                    //{
                                    //    (outputs[oo] as ArrayList).Add("Not set");
                                    //}
                                }
                        }

                    }
                    if (this.GetType().GetMethod("doubleClickMe_userCode").DeclaringType == this.GetType())
                    {

                        userdoIprocess();
                    }
                }
                else
                {
                    usercodeoutputs = new ArrayList();
                    for (int oo = 0; oo < outputs.Count; oo++)
                    {
                        usercodeoutputs.Add(null);
                    }
                    doubleClickMe_userCode(clickXRatio, clickYRatio, clickZRatio);
                    if (this.GetType().GetMethod("doubleClickMe_userCode").DeclaringType == this.GetType())
                    {

                        //to account for possibility of inputs and outputs changed
                        if (outputs.Count == usercodeoutputs.Count)
                        {
                            for (int oo = 0; oo < outputs.Count; oo++)
                            {
                                outputs[oo] = usercodeoutputs[oo];
                            }
                        }
                        userdoIprocess();
                    }
                }
            }
            else
            {
                doubleClickMe_userCode(clickXRatio, clickYRatio, clickZRatio);
                userdoIprocess();
            }

        }

        /// <summary>
        /// This method is overriden to set output values when user double clicks on a module in the GUI.
        /// </summary>
        /// <param name="clickXRatio">X double click relative to node bounding box lower left corner and box x size</param>
        /// <param name="clickYRatio">Y double click relative to node bounding box lower left corner and box y size</param>
        /// <param name="clickZRatio">Z double click relative to node bounding box lower left corner and box z size</param>
        public virtual void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
        {


        }
        /// <summary>
        /// If doubleClickMe_userCode is overwritten, this should be updated with a description of what it does which will show up in node information.
        /// </summary>
        public string doubleClickDescription = "";
        /// <summary>
        ///Method override for nodes with custom properties and/or variable number of inputs and generally more custom, this gets called when loading from an XML file.
        ///It is called after the overriden constructor for the child PUPPIModule class.
        /// Needs a corresponding saveModuleSettings to string method override.
        /// </summary>
        /// <param name="savedSettings"></param>
        public virtual void initOnLoad(string savedSettings)
        {

        }
        /// <summary>
        /// Writes custom node data when saving. Needs to be overridden for PUPPIModules with variable inputs and properties of the child class that don't exist on the base PUPPIModule class.
        /// </summary>
        /// <returns></returns>
        public virtual string saveSettings()
        {
            return "";
        }

        internal void dragOver_visualUpdate_me(double startXRatio, double startYRatio, double startZRatio, double currentXRatio, double currentYRatio, double currentZRatio)
        {
            dragOver_visualUpdate_usercode(startXRatio, startYRatio, startZRatio, currentXRatio, currentYRatio, currentZRatio);
        }

        //gesture over object
        internal void gestureObjectMe(double startXRatio, double startYRatio, double startZRatio, double endXRatio, double endYRatio, double endZRatio)
        {
            if (useMyPUPPIcanvas != null)
                useMyPUPPIcanvas.resetNumberCallsOnInteraction();
            //prepare inputs and outputs for setting
            dblclkpreviousoutputs.Clear();
            for (int oi = 0; oi < outputs.Count; oi++)
            {
                dblclkpreviousoutputs.Add(outputs[oi] as object);
            }
            if (completeGestureOverride == false)
            {
                if (listprocess == true)
                {
                    if (countListMode == 0)
                    {
                        //have user input number of elements if setting list
                        string celements = "0";
                        formutils.InputBox("Please enter number of elements for list mode", "", ref celements);
                        countListMode = Convert.ToInt32(celements);
                        if (countListMode <= 0)
                        {
                            countListMode = 1;
                        }
                    }
                    //reset outputs to blank arrays
                    for (int oo = 0; oo < outputs.Count; oo++)
                    {
                        outputs[oo] = new ArrayList();
                    }
                    for (int lcount = 0; lcount < countListMode; lcount++)
                    {

                        //set user code outputs to default values
                        //these are used by the user changed processing code so that they don't worry
                        //about list mode
                        usercodeoutputs = new ArrayList();
                        for (int oo = 0; oo < outputs.Count; oo++)
                        {
                            //    try
                            //    {
                            //        usercodeoutputs.Add(outputs[oo]);  
                            //    }
                            //    catch
                            //{
                            usercodeoutputs.Add(null);
                            //}
                        }
                        //store, in case we wanna display that
                        inlist = lcount;
                        gestureMe_userCode(startXRatio, startYRatio, startZRatio, endXRatio, endYRatio, endZRatio);
                        if (this.GetType().GetMethod("gestureMe_userCode").DeclaringType == this.GetType())
                        {
                            for (int oo = 0; oo < outputs.Count; oo++)
                            {
                                (outputs[oo] as ArrayList).Add(usercodeoutputs[oo]);
                            }
                        }

                    }
                    if (this.GetType().GetMethod("gestureMe_userCode").DeclaringType == this.GetType())
                    {
                        userdoIprocess();
                    }
                }
                else
                {
                    usercodeoutputs = new ArrayList();
                    for (int oo = 0; oo < outputs.Count; oo++)
                    {
                        usercodeoutputs.Add(null);
                    }
                    gestureMe_userCode(startXRatio, startYRatio, startZRatio, endXRatio, endYRatio, endZRatio);
                    //if overriden
                    if (this.GetType().GetMethod("gestureMe_userCode").DeclaringType == this.GetType())
                    {
                        if (outputs.Count == usercodeoutputs.Count)
                        {
                            for (int oo = 0; oo < outputs.Count; oo++)
                            {
                                outputs[oo] = usercodeoutputs[oo];
                            }
                        }
                        userdoIprocess();
                    }
                }
            }
            else
            {
                gestureMe_userCode(startXRatio, startYRatio, startZRatio, endXRatio, endYRatio, endZRatio);
                userdoIprocess();
            }

        }


        /// <summary>
        /// This method is overriden to update node custom renderer, if any, when user drags over a single node.
        /// This method is usually complemented by the gesture_usercode method which updates the PUPPIModule based on the drag-drop operation.
        /// Attempts to force node processing from this method could make the GUI unresponsive.
        /// If you implement this function, PUPPIGUISettings.drawGestureLine should be set to false.
        /// </summary>
        /// <param name="startXRatio">X mouse start relative to node bounding box lower left corner and box x size</param>
        /// <param name="startYRatio">Y mouse start relative to node bounding box lower left corner and box y size</param>
        /// <param name="startZRatio">Z mouse start relative to node bounding box lower left corner and box Z size</param>
        /// <param name="currentXRatio">X mouse current relative to node bounding box lower left corner and box x size</param>
        /// <param name="currentYRatio">Y mouse current relative to node bounding box lower left corner and box y size</param>
        /// <param name="currentZRatio">Z mouse current relative to node bounding box lower left corner and box Z size</param>
        public virtual void dragOver_visualUpdate_usercode(double startXRatio, double startYRatio, double startZRatio, double currentXRatio, double currentYRatio, double currentZRatio)
        {

        }


        /// <summary>
        /// This method is overriden to set output values and update node custom renderer, if any, when user drag-drops over a single node.
        /// </summary>
        /// <param name="startXRatio">X gesture start relative to node bounding box lower left corner and box x size</param>
        /// <param name="startYRatio">Y gesture start relative to node bounding box lower left corner and box y size</param>
        /// <param name="startZRatio">Z gesture start relative to node bounding box lower left corner and box Z size</param>
        /// <param name="endXRatio">X gesture end relative to node bounding box lower left corner and box x size</param>
        /// <param name="endYRatio">Y gesture end relative to node bounding box lower left corner and box y size</param>
        /// <param name="endZRatio">Z gesture end relative to node bounding box lower left corner and box Z size</param>
        public virtual void gestureMe_userCode(double startXRatio, double startYRatio, double startZRatio, double endXRatio, double endYRatio, double endZRatio)
        {

        }
        /// <summary>
        /// If gestureMe_userCode is overwritten, this should be updated with a description of what it does which will show up in node information.
        /// </summary>
        public string gestureDescription = "";



        //separate function to add input
        //isclass is 1 or 0
        internal protected void addinput(string name, int iindex, string typename, int isclass)
        {
            //if (isclass == 1)
            //{
            inputnames.Add(name);
            inputs.Add(new PUPPIInParameter());
            //}
            methodparm mp = new methodparm();
            mp.isoutput = false;
            mp.index = iindex;
            mp.typename = typename;
            methodparms.Add(mp);
        }
        //adds output witha  default value
        internal protected void addoutput(string name, object dout, int oindex, string typename)
        {


            outputnames.Add(name);
            if (isparameterlessconstructor)
            {
                Type outtype = Type.GetType(typename);
                if (outtype == null)
                {
                    outtype = Type.GetType(typename, (name1) =>
                    {
                        return AppDomain.CurrentDomain.GetAssemblies().Where(z => z.FullName == name1.FullName).FirstOrDefault();
                    }, null, true);
                }
                ConstructorInfo ctor = outtype.GetConstructor(Type.EmptyTypes);
                object instance;
                // if (outtype.ContainsGenericParameters == false)
                //{
                instance = ctor.Invoke(null);
                //}
                //else
                //{
                //   instance = outtype.MakeGenericType().GetConstructor(Type.EmptyTypes).Invoke(null)   ;  
                //}

                outputs.Add(instance);
                completeProcessOverride = true;
            }
            else if (moduleisenumeration)
            {

                Type enumtype = Type.GetType(typename);
                if (enumtype == null)
                {
                    enumtype = Type.GetType(typename, (name1) =>
                    {
                        return AppDomain.CurrentDomain.GetAssemblies().Where(z => z.FullName == name1.FullName).FirstOrDefault();
                    }, null, true);
                }
                System.Array enumValues = System.Enum.GetValues(enumtype);
                for (int i = 0; i < enumValues.Length; i++)
                {
                    string st = enumValues.GetValue(i).ToString();
                    if (st == name)
                    {
                        outputs.Add(enumValues.GetValue(i));
                        break;
                    }

                }
                completeProcessOverride = true;
            }
            else
            {
                outputs.Add(dout);
            }
            methodparm mp = new methodparm();
            mp.isoutput = true;
            mp.index = oindex;
            mp.typename = typename;
            methodparms.Add(mp);

        }
        //override process_usercode as constructor
        internal void construct_me()
        {
            try
            {
                //class type
                Type outtype = Type.GetType(methodparms[0].typename);
                if (outtype == null)
                {
                    outtype = Type.GetType(methodparms[0].typename, (name1) =>
                    {
                        return AppDomain.CurrentDomain.GetAssemblies().Where(z => z.FullName == name1.FullName).FirstOrDefault();
                    }, null, true);
                }
                //now the parameter types
                int paramnum = methodparms.Count - 1;
                System.Type[] paramtypes = new System.Type[paramnum];
                for (int pc = 0; pc < paramnum; pc++)
                {
                    Type ptype = Type.GetType(methodparms[pc + 1].typename);
                    if (ptype == null)
                    {
                        ptype = Type.GetType(methodparms[pc + 1].typename, (name1) =>
                        {
                            return AppDomain.CurrentDomain.GetAssemblies().Where(z => z.FullName == name1.FullName).FirstOrDefault();
                        }, null, true);
                    }
                    paramtypes[pc] = ptype as Type;
                }
                object[] paramvals = new object[paramnum];
                for (int pc = 0; pc < paramnum; pc++)
                {


                    if (methodparms[pc + 1].isoutput == false)
                    {
                        try
                        {
                            paramvals[pc] = System.Activator.CreateInstance(paramtypes[pc] as Type);
                        }
                        catch
                        {

                        }
                        //initialize param if input
                        Type ttype = paramtypes[pc] as Type;
                        try
                        {
                            paramvals[pc] = Convert.ChangeType(usercodeinputs[methodparms[pc + 1].index], paramtypes[pc]);// usercodeinputs[methodparms[pc + 1].index];
                        }
                        catch
                        {
                            paramvals[pc] = usercodeinputs[methodparms[pc + 1].index];
                        }
                    }
                    else
                    {
                        paramvals[pc] = System.Activator.CreateInstance(paramtypes[pc] as Type);
                    }

                }
                //constructor
                ConstructorInfo ctor = outtype.GetConstructor(paramtypes);
                object instance = ctor.Invoke(paramvals);
                //update output with class instance and any output values
                usercodeoutputs[0] = instance;
                for (int pc = 0; pc < paramnum; pc++)
                {

                    if (methodparms[pc + 1].isoutput == true)
                    {
                        usercodeoutputs[methodparms[pc + 1].index] = paramvals[pc];
                    }

                }
                anymessage = "";
            }
            catch (Exception exy)
            {
                //to see errors in debug
                if (PUPPIDebugger.debugenabled)
                {
                    PUPPIDebugger.log(" Node GUID " + GUID.ToString() + " call (autogenerated) " + numbercalls.ToString() + utils.StringConstants.constructorerrordebuglog + exy.ToString());
                }
                try
                {
                    anymessage = "";
                    string[] splitter = { "at System.Reflection" };
                    anymessage = exy.ToString().Split(splitter, StringSplitOptions.None)[0];
                    if (anymessage == "") anymessage = exy.ToString();
                }
                catch
                {
                    anymessage = exy.ToString();
                }
                usercodeoutputs[0] = "error";
            }


        }
        /// <summary>
        /// For modules derived from class methods or object constructors, gets the types expected of the inputs.
        /// </summary>
        /// <returns>A list with a type for each input. Null if module is not method or constructor with parameters</returns>
        public List<Type> getInputTypes()
        {
            if (inputs.Count == 0) return null;
            if (methodparms == null || methodparms.Count == 0) return null;
            List<Type> rl = new List<Type>();
            int ii = 0;
            while (ii < inputs.Count)
            {
                foreach (methodparm mp in methodparms)
                {
                    if (mp.index == ii && mp.isoutput == false)
                    {
                        string tn = mp.typename;
                        Type ptype = Type.GetType(tn);
                        if (ptype == null)
                        {
                            try
                            {
                                ptype = Type.GetType(tn, (name1) =>
                                {
                                    return AppDomain.CurrentDomain.GetAssemblies().Where(z => z.FullName == name1.FullName).FirstOrDefault();
                                }, null, true);
                            }
                            catch
                            {
                                ptype = null;
                            }
                        }
                        rl.Add(ptype);
                        ii++;
                        break;
                    }
                }
            }


            return rl;
        }

        /// <summary>
        /// For modules derived from class methods or object constructors, gets the types expected of the outputs.
        /// </summary>
        /// <returns>A list with a type for each output. Null if module is not method or constructor with parameters</returns>
        public List<Type> getOutputTypes()
        {
            if (outputs.Count == 0) return null;
            if (methodparms == null || methodparms.Count == 0) return null;
            List<Type> rl = new List<Type>();
            int ii = 0;
            while (ii < outputs.Count)
            {
                Type ptype = null;
                foreach (methodparm mp in methodparms)
                {
                    if (mp.index == ii && mp.isoutput == true)
                    {
                        string tn = mp.typename;
                        ptype = Type.GetType(tn);

                        if (ptype == null)
                        {
                            try
                            {

                                //        //try looping through loaded assemblies
                                //        foreach (Assembly aaaa in AppDomain.CurrentDomain.GetAssemblies())
                                //        {
                                //            ptype = null;
                                //            try
                                //            {
                                //                ptype = aaaa.GetType(tn);
                                //            }
                                //            catch
                                //            {
                                //                ptype = null;
                                //            }
                                //            if (ptype != null) break;
                                //        }

                                if (ptype == null)
                                {
                                    ptype = Type.GetType(tn, (name1) =>
                                    {
                                        return AppDomain.CurrentDomain.GetAssemblies().Where(z => z.FullName == name1.FullName).FirstOrDefault();
                                    }, null, true);
                                }
                            }
                            catch (Exception exy)
                            {
                                ptype = null;
                            }
                        }

                        break;
                    }
                }
                rl.Add(ptype);
                ii++;
            }


            return rl;
        }


        //override process_usercode as method
        internal void method_me()
        {
            try
            {

                //if not static, first input is class instance calling
                //if not void, first output is return
                //if not static, last output is class instance after calling method 

                //class or struct type type
                Type intype = Type.GetType(ctn);


                if (intype == null)
                {
                    intype = Type.GetType(ctn, (name1) =>
                    {
                        return AppDomain.CurrentDomain.GetAssemblies().Where(z => z.FullName == name1.FullName).FirstOrDefault();
                    }, null, true);
                }


                //now the parameter types
                int paramnum = methodparms.Count;
                int startparam = 0;
                if (outputnames.Count > 0 && outputnames[outputnames.Count - 1].StartsWith("InstOut:")) paramnum -= 1;
                if (outputnames.Count > 0 && outputnames[0].StartsWith("Ret:")) startparam += 1;
                if (inputnames[0].StartsWith("Inst:")) startparam += 1;


                System.Type[] paramtypes = new System.Type[paramnum - startparam];
                for (int pc = startparam; pc < paramnum; pc++)
                {
                    Type ptype = Type.GetType(methodparms[pc].typename);
                    if (ptype == null)
                    {
                        ptype = Type.GetType(methodparms[pc].typename, (name1) =>
                        {
                            return AppDomain.CurrentDomain.GetAssemblies().Where(z => z.FullName == name1.FullName).FirstOrDefault();
                        }, null, true);
                    }
                    paramtypes[pc - startparam] = ptype as Type;
                }
                object[] paramvals = new object[paramnum - startparam];
                for (int pc = 0; pc < paramnum - startparam; pc++)
                {

                    //initialize param if input
                    Type ttype = paramtypes[pc] as Type;
                    if (methodparms[pc + startparam].isoutput == false)
                    {

                        Type tttt = paramtypes[pc] as Type;
                        //will catch immutable error
                        try
                        {
                            paramvals[pc] = System.Activator.CreateInstance(paramtypes[pc] as Type);
                        }
                        catch
                        {

                        }
                        try
                        {
                            paramvals[pc] = Convert.ChangeType(usercodeinputs[methodparms[pc + startparam].index], tttt);
                        }
                        catch
                        {
                            paramvals[pc] = usercodeinputs[methodparms[pc + startparam].index];
                        }
                    }
                    else
                    {
                        //paramvals[pc] = System.Activator.CreateInstance(paramtypes[pc] as Type);
                        try
                        {
                            paramvals[pc] = System.Activator.CreateInstance(paramtypes[pc] as Type);
                        }
                        catch (Exception exy)
                        {
                            paramvals[pc] = null;
                        }
                    }

                }
                //method
                object result = null;
                object cco = null;
                MethodInfo mthod = intype.GetMethod(methodname, paramtypes);
                if (mthod.ReturnType != typeof(void))
                {
                    //replaced intype.GetConstructors().Count()
                    if ((intype.IsClass == true || intype.GetConstructors().Count() > 0) && mthod.IsStatic == false)
                    {
                        if (usercodeinputs[0] != null)
                        {
                            object myc = usercodeinputs[0];
                            cco = DeepCopy(myc, 0);
                            if (cco != null)
                                result = mthod.Invoke(cco, paramvals);
                            else
                                result = "Failed Instance Clone";
                        }
                        else
                            result = "Null Instance";
                    }
                    else
                    {
                        if (mthod.IsStatic == true)
                        {


                            result = mthod.Invoke(intype, paramvals);
                        }
                        else
                        {
                            //structure with no constructor with parameters
                            if (usercodeinputs[0] != null)
                            {
                                object myc = usercodeinputs[0];
                                cco = DeepCopy(myc, 0);
                                if (cco != null)
                                    result = mthod.Invoke(cco, paramvals);
                                else
                                    result = "Failed Instance Clone";
                            }
                            else
                                result = "Null Instance";
                        }

                    }
                }
                else
                {
                    //will try some deepcopy with custom function
                    //might not work every time
                    //replaced intype.GetConstructors().Count()
                    if ((intype.IsClass == true || intype.GetConstructors().Count() > 0) && mthod.IsStatic == false)
                    {
                        if (usercodeinputs[0] != null)
                        {
                            object myc = usercodeinputs[0];
                            cco = DeepCopy(myc, 0);
                            if (cco != null)
                                mthod.Invoke(cco, paramvals);
                            else
                                result = "Failed Instance Clone";
                        }
                        else
                        {
                            result = "Null Instance";
                        }
                    }
                    else
                    {
                        if (mthod.IsStatic == true)
                        {
                            result = mthod.Invoke(intype, paramvals);
                            result = 0;
                        }
                            //structure with no constructors
                        else
                        {
                            if (usercodeinputs[0] != null)
                            {
                                object myc = usercodeinputs[0];
                                cco = DeepCopy(myc, 0);
                                if (cco != null)
                                    mthod.Invoke(cco, paramvals);
                                else
                                    result = "Failed Instance Clone";
                            }
                            else
                            {
                                result = "Null Instance";
                            }
                        }
                    }
                }

                //update output with class instance and any output values
                if (mthod.ReturnType != typeof(void))
                {
                    usercodeoutputs[0] = result;
                }
                //else
                //{
                //    usercodeoutputs[0] = usercodeinputs[0];
                //}


                for (int pc = 0; pc < paramnum - startparam; pc++)
                {

                    if (methodparms[pc + startparam].isoutput == true)
                    {
                        usercodeoutputs[methodparms[pc + startparam].index] = paramvals[pc];
                    }

                }
                //finally we still need to pass the class
                //if (mthod.ReturnType != typeof(void))
                //{
                if ((intype.IsClass == true || intype.GetConstructors().Count() > 0) && mthod.IsStatic == false)
                {
                    //object myclass = usercodeinputs[0];
                    //result = DeepCopy(myclass, 0);

                    usercodeoutputs[usercodeoutputs.Count - 1] = cco;
                }

                // }

                anymessage = "";
            }
            catch (Exception exy)
            {
                //to see errors in debug
                if (PUPPIDebugger.debugenabled)
                {
                    PUPPIDebugger.log(" Node GUID " + GUID.ToString() + " call (autogenerated) " + numbercalls.ToString() + " method error: " + exy.ToString());
                }
                usercodeoutputs[0] = "error";
                try
                {
                    anymessage = "";
                    string[] splitter = { "at System.Reflection" };
                    anymessage = exy.ToString().Split(splitter, StringSplitOptions.None)[0];
                    if (anymessage == "") anymessage = exy.ToString();
                }
                catch
                {
                    anymessage = exy.ToString();
                }
            }

        }
        //attempt to deepcopy objects
        private static ObservableCollection<T> DeepCopyIEnumerable<T>(IEnumerable<T> list)
    where T : ICloneable
        {
            return new ObservableCollection<T>(list.Select(x => x.Clone()).Cast<T>());
        }
        // Deep clone





        internal static object DeepCopy(object obj, int level)
        {
            if (obj != null)
            {
                try
                {

                    if (obj.GetType().IsSerializable)
                    {
                        return obj.DeepClone();
                    }
                    else
                    {


                        //        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        //        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        //        {
                        //            bf.Serialize(ms, obj);
                        //            ms.Seek(0, System.IO.SeekOrigin.Begin);
                        //            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binForm = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                        //            return  (Object)binForm.Deserialize(ms);

                        //        }

                        //}
                        //else
                        //{
                        //    Object robj = null;
                        //    try
                        //    {
                        //        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        //        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        //        {
                        //            bf.Serialize(ms, obj);
                        //            ms.Seek(0, System.IO.SeekOrigin.Begin);
                        //            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binForm = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                        //            robj = (Object)binForm.Deserialize(ms);
                        //            if (robj != null)
                        //                return robj;
                        //        }
                        //    }
                        //    catch(Exception exy)
                        //    {
                        //        robj = null;
                        //    }
                        //    if (robj == null)
                        //    {
                        var memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);

                        var newCopy = memberwiseClone.Invoke(obj, new object[0]);

                        foreach (var field in newCopy.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                        {
                            //to prevent stack overflow we can't copy fields with same type as original field
                            if (!field.FieldType.IsPrimitive && field.FieldType != typeof(string) && field.FieldType != newCopy.GetType())
                            {
                                if (level < PUPPIGUISettings.stackOverflowGuard)
                                {
                                    var fieldCopy = DeepCopy(field.GetValue(obj), level + 1);
                                    field.SetValue(newCopy, fieldCopy);
                                }
                            }

                        }
                        return newCopy;
                    }
                    //    else
                    //    {
                    //        return robj;
                    //    }
                    //}
                    //else
                    //{
                    //    var memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
                    //    //var newCopy=obj;
                    //    //if (obj is  IEnumerable )
                    //    //{
                    //    //    newCopy = memberwiseClone.Invoke(obj, new object[0]);
                    //    //    //Type teee = (obj as IEnumerable) .GetType().GetGenericArguments()[0];
                    //    //    //if (teee is ICloneable)
                    //    //    //{
                    //    //    //    ICloneable tic = teee as ICloneable;

                    //    //    //    newCopy = DeepCopyIEnumerable <tic > (obj as IEnumerable<tic>);
                    //    //    //}
                    //    //}
                    //    //else
                    //    //{
                    //    var newCopy = memberwiseClone.Invoke(obj, new object[0]);
                    //    //}
                    //    foreach (var field in newCopy.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    //    {
                    //        //to prevent stack overflow we can't copy fields with same type as original field
                    //        if (!field.FieldType.IsPrimitive && field.FieldType != typeof(string) && field.FieldType != newCopy.GetType())
                    //        {
                    //            if (level < PUPPIGUISettings.stackOverflowGuard)
                    //            {
                    //                var fieldCopy = DeepCopy(field.GetValue(obj), level + 1);
                    //                field.SetValue(newCopy, fieldCopy);
                    //            }
                    //        }
                    //        //else if (field.FieldType.IsPrimitive || field.FieldType == typeof(string))
                    //        //{
                    //        //    field.SetValue(newCopy, field.GetValue(obj));
                    //        //}
                    //    }
                    //    return newCopy;
                    //}
                }
                catch
                {
                    if (PUPPIDebugger.debugenabled)
                    {
                        PUPPIDebugger.log(utils.StringConstants.failedtodeepcopy + " " + obj.GetType().ToString());

                    }
                    try
                    {
                        object nobject = new object();
                        nobject = obj;
                        return nobject;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }



        //internal static object DeepCopy(object obj, int level)
        //{
        //    if (obj != null)
        //    {
        //        try
        //        {

        //            if (obj.GetType().IsSerializable)
        //            {
        //                return obj.DeepClone();
        //            }
        //            else
        //            {
        //                var memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
        //                //var newCopy=obj;
        //                //if (obj is  IEnumerable )
        //                //{
        //                //    newCopy = memberwiseClone.Invoke(obj, new object[0]);
        //                //    //Type teee = (obj as IEnumerable) .GetType().GetGenericArguments()[0];
        //                //    //if (teee is ICloneable)
        //                //    //{
        //                //    //    ICloneable tic = teee as ICloneable;

        //                //    //    newCopy = DeepCopyIEnumerable <tic > (obj as IEnumerable<tic>);
        //                //    //}
        //                //}
        //                //else
        //                //{
        //                var newCopy = memberwiseClone.Invoke(obj, new object[0]);
        //                //}
        //                foreach (var field in newCopy.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        //                {
        //                    //to prevent stack overflow we can't copy fields with same type as original field
        //                    if (!field.FieldType.IsPrimitive && field.FieldType != typeof(string) && field.FieldType != newCopy.GetType())
        //                    {
        //                        if (level < PUPPIGUISettings.stackOverflowGuard)
        //                        {
        //                            var fieldCopy = DeepCopy(field.GetValue(newCopy), level + 1);
        //                            field.SetValue(newCopy, fieldCopy);
        //                        }
        //                    }
        //                    else if (field.FieldType.IsPrimitive || field.FieldType == typeof(string))
        //                    {

        //                    }
        //                }
        //                return newCopy;
        //            }
        //        }
        //        catch
        //        {
        //            if (PUPPIDebugger.debugenabled)
        //            {
        //                PUPPIDebugger.log(utils.StringConstants.failedtodeepcopy + " " + obj.GetType().ToString());

        //            }
        //            try
        //            {
        //                object nobject = new object();
        //                nobject = obj;
        //                return nobject;
        //            }
        //            catch
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets a PUPPI module just upstream of the current module.
        /// </summary>
        /// <param name="inputIndex">The input to which this PUPPI module is connected to</param>
        /// <returns>The PUPPI Module</returns>
        public PUPPIModule getUpstreamModule(int inputIndex)
        {
            if (inputIndex < inputs.Count && inputIndex >= 0)
            {
                if (inputs[inputIndex] != null && inputs[inputIndex].module != null)
                {
                    return inputs[inputIndex].module as PUPPIModule;
                }
            }
            return null;
        }
        /// <summary>
        /// Gets the value of an input as object. Returns null if any error.
        /// </summary>
        /// <param name="inputIndex">Valid input index</param>
        /// <returns>Value of an input as object. Null if any error.</returns>
        public object getInputValue(int inputIndex)
        {
            if (inputIndex < inputs.Count && inputIndex >= 0)
            {
                if (inputs[inputIndex] != null && inputs[inputIndex].module != null)
                {
                    try
                    {
                        return inputs[inputIndex].module.outputs[inputs[inputIndex].outParIndex];
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a list of PUPPI modules connected to the specified output index
        /// </summary>
        /// <param name="outputIndex">Valid output index of the module</param>
        /// <returns>List of PUPPI modules</returns>
        public List<PUPPIModule> getDownstreamModules(int outputIndex)
        {
            List<PUPPIModule> o = new List<PUPPIModule>();
            if (outputIndex >= 0 && outputIndex < outputs.Count)
            {

                if (useMyPUPPIcanvas != null)
                {
                    if (useMyPUPPIcanvas.stacks.ContainsKey(GUID.ToString()))
                    {

                        List<PUPPICanvas.ModViz3DNode> lm = useMyPUPPIcanvas.stacks[GUID.ToString()].getAllNodesDownstream(outputIndex);
                        foreach (PUPPICanvas.ModViz3DNode pm in lm)
                        {
                            if (pm.logical_representation != null)
                            {
                                o.Add(pm.logical_representation as PUPPIModule);
                            }
                        }

                    }
                }
            }
            return o;

        }
        //no canvas
        internal List<PUPPIModule> getDownstreamModulesFromList(int outputIndex, List<PUPPIModule> plist)
        {
            List<PUPPIModule> o = new List<PUPPIModule>();
            if (outputIndex >= 0 && outputIndex < outputs.Count)
            {
                foreach (PUPPIModule pm in plist)
                {
                    foreach (PUPPIInParameter pi in pm.inputs)
                    {
                        if (pi.module == this)
                        {
                            o.Add(pm);
                        }
                    }
                }


            }
            return o;

        }


    }
    ///// <summary>
    ///// Special module with customized user interaction. Dragging a lever marker changes a value and causes the widget to process.
    ///// </summary>
    //public class PUPPIControlWidget:PUPPIModule
    //{
    //    public List<double> values;
    //    public int intermediarySteps;
    //    public List<double> xPositions;
    //    public List<double> yPositions;
    //}
    //internal struct WidgetPoint { }
    internal static class Cloner
    {
        static Dictionary<Type, Delegate> _cachedIL = new Dictionary<Type, Delegate>();

        internal static T Clone<T>(T myObject)
        {
            Delegate myExec = null;

            if (!_cachedIL.TryGetValue(typeof(T), out myExec))
            {
                var dymMethod = new DynamicMethod("DoClone", typeof(T), new Type[] { typeof(T) }, true);
                var cInfo = myObject.GetType().GetConstructor(new Type[] { });

                var generator = dymMethod.GetILGenerator();

                var lbf = generator.DeclareLocal(typeof(T));

                generator.Emit(OpCodes.Newobj, cInfo);
                generator.Emit(OpCodes.Stloc_0);

                foreach (var field in myObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    // Load the new object on the eval stack... (currently 1 item on eval stack)
                    generator.Emit(OpCodes.Ldloc_0);
                    // Load initial object (parameter)          (currently 2 items on eval stack)
                    generator.Emit(OpCodes.Ldarg_0);
                    // Replace value by field value             (still currently 2 items on eval stack)
                    generator.Emit(OpCodes.Ldfld, field);
                    // Store the value of the top on the eval stack into the object underneath that value on the value stack.
                    //  (0 items on eval stack)
                    generator.Emit(OpCodes.Stfld, field);
                }

                // Load new constructed obj on eval stack -> 1 item on stack
                generator.Emit(OpCodes.Ldloc_0);
                // Return constructed object.   --> 0 items on stack
                generator.Emit(OpCodes.Ret);

                myExec = dymMethod.CreateDelegate(typeof(Func<T, T>));

                _cachedIL.Add(typeof(T), myExec);
            }

            return ((Func<T, T>)myExec)(myObject);
        }
    }

    //to store values in user inputs and outputs for method calls when geertaing automatic objects
    internal class methodparm
    {
        public int index = -1;
        public bool isoutput = false;
        public string typename = "";
    }


    /// <summary>
    /// Input Parameter type. References a module whose output is connected to the current module.
    /// </summary>
    public class PUPPIInParameter
    {
        /// <summary>
        /// Access to the module feeding into the input.
        /// </summary>
        public PUPPIModule module { get; set; }
        /// <summary>
        /// The index of the output feeding into this input.
        /// </summary>
        public int outParIndex { get; set; }
        /// <summary>
        /// Name of the input.
        /// </summary>
        public string inputname;
        /// <summary>
        /// Some input  parameters can be made optional even in non-complete overriden mode.
        /// </summary>
        public bool isoptional = false;
        internal bool inputAutomaticListMode = false;

        //this updates model that input has changed
        internal event inputchangedhandler inchan;
        internal delegate void inputchangedhandler();

        public PUPPIInParameter()
        {
            module = null;
            outParIndex = -1;
            inputname = "";
            isoptional = false;
        }
        public PUPPIInParameter(string inpname)
        {
            module = null;
            outParIndex = -1;
            inputname = inpname;
            isoptional = false;
        }
        internal bool getinputfrom(PUPPIModule othermodule, int outputindex)
        {
            if (othermodule != null && outputindex >= 0 && outputindex < othermodule.outputs.Count)
            {
                module = othermodule;
                outParIndex = outputindex;
                sourcenodeevent(module);
                return true;

            }
            return false;
        }
        internal void sourcenodeevent(PUPPIModule module)
        {
            //event handler
            module.prochan += new PUPPIModule.processhandler(transmitchange);
        }
        //this gets called when input has changed
        internal void transmitchange()
        {
            if (inchan != null)
            {
                inchan();
            }
        }
        internal void disconnect()
        {
            module = null;
            outParIndex = -1;
            //unsubscribe event handler
        }
    }
    /// <summary>
    /// This Static Class holds methods for creating PUPPI visual programming modules automatically from existing libraries and types.
    /// </summary>
    public static class AutomaticPUPPImodulesCreator
    {

        /// <summary>
        /// Finds a type by name starting in the executing assembly and continuing through other assemblies.
        /// </summary>
        /// <param name="tName">Type's name</param>
        /// <returns>a Type</returns>
        public static Type findTypeByName(string tName)
        {
            Type tm = null;
            //first try in current project
            var ca = Assembly.GetExecutingAssembly();
            Type[] internalTypes = ca.GetExportedTypes();
            try
            {
                foreach (Type tip in internalTypes)
                {
                    if (tip.Name == tName)
                    {
                        tm = tip;
                        break;
                    }
                }
            }
            catch
            {
                tm = null;

            }
            if (tm == null)
            {

                foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        tm = a.GetType(tName);
                    }
                    catch
                    {
                        tm = null;
                    }
                    if (tm != null) break;
                }
            }
            //if (tm == null)
            //{
            //    //try to get without namespace
            //    char[] splitters={'+'};
            //    string tnns = tName.Split(splitters)[tName.Split(splitters).Length -1];
            //    char[] bplitters = { '+' };
            //    tnns = tnns.Split(bplitters)[tnns.Split(bplitters).Length - 1];
            //    Assembly[] aaasss = AppDomain.CurrentDomain.GetAssemblies();
            //    List<Assembly> aias = aaasss.ToList<Assembly>();
            //    aias.Insert(0, ca);

            //    foreach (Assembly a in aias)
            //    {
            //        bool fder = false;
            //        foreach (Type ttttt in a.GetExportedTypes())
            //        {
            //           if ( ttttt.Name.Contains(tnns))
            //           {
            //               tm = ttttt;
            //               fder = true;
            //               break;
            //           }
            //        }
            //        if (fder == true) break;
            //    }
            //}
            return tm;
        }
        /// <summary>
        /// Provided a PUPPIModule, returns all the PUPPI Modules from the same assembly.
        /// </summary>
        /// <param name="sampleModule">A PUPPIModule</param>
        /// <returns>Arraylist of Type from which PUPPIModules can be instantiated.</returns>
        public static ArrayList getPUPPIModulesFromSameAssembly(PUPPIModule sampleModule)
        {
            ArrayList a = new ArrayList();
            foreach (Type pee in sampleModule.GetType().Assembly.GetTypes())
            {
                if (pee.BaseType == sampleModule.GetType().BaseType)
                {
                    a.Add(pee);
                }
            }
            return a;
        }

        /// <summary>
        /// Given a folder with DLLs containing PUPPIModule types, it will return lists of PUPPIModule types which can be added to a menu.
        /// Automatically loads assemblies
        /// </summary>
        /// <param name="pathToPUPPIModuleDLLs">a valid path to a folder containing DLLs with PUPPIModules</param>
        /// <param name="originTypeNames">Output of original type names, each type name identifying one or more PUPPIModules determined from that type's methods ,constructors and enums.</param>
        /// <param name="loadedDllFiles">Output of dll files that contain valid PUPPIModules that were loaded.</param>
        /// <returns>A list of ArrayLists holding PUPPIModule types. Each list itme corresponds to one origin Type name</returns>
        public static List<ArrayList> importPUPPIModulesFromFolder(string pathToPUPPIModuleDLLs, out List<string> originTypeNames, out List<string> loadedDllFiles)
        {
            originTypeNames = new List<string>();
            loadedDllFiles = new List<string>();
            List<ArrayList> aaa = new List<ArrayList>();

            if (Directory.Exists(pathToPUPPIModuleDLLs))
            {
                string[] fileEntries = Directory.GetFiles(pathToPUPPIModuleDLLs);
                foreach (string fileName in fileEntries)
                {
                    if (fileName.EndsWith(".dll"))
                    {
                        Assembly aa = null;
                        try
                        {
                            aa = Assembly.LoadFrom(fileName);
                        }
                        catch
                        {
                            aa = null;
                        }
                        if (aa != null)
                        {
                            ArrayList a = new ArrayList();
                            try
                            {
                                foreach (Type pee in aa.GetTypes())
                                {
                                    if (pee.BaseType == typeof(PUPPIModule))
                                    {
                                        a.Add(pee);
                                    }
                                }
                            }
                            catch
                            {

                            }
                            if (a.Count > 0)
                            {
                                string ln = "";
                                Type t = a[0] as Type;
                                //the last . in the namespace name should get the original type
                                string nsn = t.Namespace;
                                string[] sp = new string[1];
                                sp[0] = ".";
                                string[] pp = nsn.Split(sp, StringSplitOptions.None);
                                if (pp[pp.GetLength(0) - 1] != "")
                                {
                                    ln = pp[pp.GetLength(0) - 1];
                                }
                                else
                                {
                                    ln = nsn;
                                }
                                if (ln == "") ln = "None";
                                aaa.Add(a);
                                originTypeNames.Add(ln);
                                loadedDllFiles.Add(fileName);
                            }
                        }

                    }
                }
            }
            else
            {
                throw new Exception("Invalid PUPPIModule DLL folder " + pathToPUPPIModuleDLLs);
            }
            return aaa;
        }


        /// <summary>
        /// Creates an ArrayList of PUPPIModules (types) based on Constructors and Methods found in MyType.
        /// Methods can be filtered, but all constructors are picked.
        /// This function does NOT pick up class fields without getters and setters.
        /// </summary>
        /// <param name="Mytype">A type (use command .GetType() on a class instance or typeof on a static class)</param>
        /// <param name="includedefaultconstructors">If false, default parameterless constructors are not added to the returned list</param>
        /// <param name="namefilter">List of method names that should be picked (optional)</param>
        /// <param name="replacefilter">If namefilter used, thisLis twill replace the name of the method with the one specified here.</param>
        /// <returns>ArrayList with Types</returns>
        public static ArrayList makeTypePUPPImodules(Type Mytype, bool includedefaultconstructors = true, List<string> namefilter = null, List<string> replacefilter = null)
        {
          
            ArrayList CPUPPImodules = new ArrayList();
            //static classes should still work
            //abstract instantiable won't work
            if (Mytype.IsAbstract && !Mytype.IsSealed) return CPUPPImodules;


            //object fromclass
            int isclass = 1;
            //  CPUPPImodules = new ArrayList(); 
            //Get the type.
            //Type Mytype = fromclass.GetType();
            string classtypename = Mytype.Name;
            var an = new AssemblyName("PUPPI" + classtypename + "as");
            AssemblyBuilder mb = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);

            //also for high level enums,, not nested types
            if (Mytype.IsEnum)
            {
                Type pE = makePUPPIModuleFromEnum(Mytype, hac(Mytype.Name), mb);
                if (pE != null)
                    CPUPPImodules.Add(pE);
            }
            else
            {

                //constructors
                int ccnt = 0;
                var bci = Mytype.GetConstructors();//.OrderBy(aci => aci.Name).ThenBy(aci => String.Join(",", aci.GetParameters().Select(x => x.Name + x.ParameterType.Name).ToArray()).Length);

                foreach (ConstructorInfo cinfo in bci)
                {
                    //gonna use a try-catch
                    try
                    {
                        ParameterInfo[] Myarray = cinfo.GetParameters();
                        //if parameterless excluded
                        if (Myarray.Length == 0 && includedefaultconstructors == false)
                        {   //to preserve compatibility we still increment
                            ccnt++;
                            continue;
                        }
                        //constructor name
                        string hame = "c";
                        foreach (ParameterInfo pinfo in Myarray)
                        {
                            hame += pinfo.Name;
                            hame += pinfo.ParameterType.Name;

                        }

                        Type t = AutomaticPUPPImodulesCreator.makePUPPIModuleFromConstructor(cinfo, hac(hame), mb);
                        if (t != null)
                        {
                            CPUPPImodules.Add(t);
                        }
                        else
                        {

                        }


                        ccnt++;
                    }
                    catch
                    {
                        ccnt++;
                    }

                }


                //iterate through methods
                //each method becomes a puypi module
                //class istelf is an input parameter
                //just for now, a counter to make sure no duplicates
                int mcnt = 0;
                MethodInfo[] smi = Mytype.GetMethods();
                var bmi = smi;//.OrderBy(ami => ami.Name).ThenBy(ami => ami.ReturnType.Name).ThenBy(ami => String.Join(",", ami.GetParameters().Select(x => x.Name + x.ParameterType.Name).ToArray()).Length);

                foreach (MethodInfo methinfo in bmi)
                {
                    try
                    {
                        string cr = "";

                        string hame = "m";

                        ParameterInfo[] Myarray = methinfo.GetParameters();
                        foreach (ParameterInfo pinfo in Myarray)
                        {
                            hame += pinfo.Name;
                            hame += pinfo.ParameterType.Name;

                        }

                        int hah = hac(hame);
                        //check if we have the method in the array
                        if (namefilter != null)
                        {
                            if (namefilter.Contains(methinfo.Name) == false)
                            {
                                //we still increment this for consistency
                                mcnt++;
                                continue;
                            }

                            if (replacefilter != null)
                            {
                                if (replacefilter[namefilter.IndexOf(methinfo.Name)] != null)
                                {
                                    cr = replacefilter[namefilter.IndexOf(methinfo.Name)] as string;// +"__" + hah.ToString();
                                }
                            }
                        }

                        Type t = AutomaticPUPPImodulesCreator.makePUPPIModuleFromMethod(methinfo, hah, mb, cr);
                        if (t != null)
                        {
                            CPUPPImodules.Add(t);
                        }
                        else
                        {

                        }
                        mcnt++;
                    }
                    catch
                    {
                        mcnt++;
                    }

                }
                //ArrayList a = makeEnumMembersIntoPUPPIModules(Mytype, mb);
                //for (int aa = 0; aa < a.Count; aa++)
                //{
                //    Type t = a[aa] as Type;
                //    if (t != null)
                //    {
                //        CPUPPImodules.Add(t);
                //    }
                //    else
                //    {

                //    }
                //}
                ////iterate through properties to maken setter and getter modules
                //int pcnt = 0;
                //foreach (PropertyInfo  pi in Mytype.GetProperties() )
                //{
                //    try
                //    {
                //        string cr = "";

                //        MethodInfo pg=pi.GetGetMethod();
                //        MethodInfo ps=pi.GetSetMethod();
                //        if (pg!=null)
                //        {
                //        CPUPPImodules.Add(AutomaticPUPPImodulesCreator.makePUPPIModuleFromMethod(pg , pcnt, mb, cr));
                //        }
                //        if (ps!=null)
                //        { 
                //        CPUPPImodules.Add(AutomaticPUPPImodulesCreator.makePUPPIModuleFromMethod(ps, pcnt, mb, cr));
                //        }
                //        pcnt++;
                //    }
                //    catch
                //    {
                //        pcnt++;
                //    }

                //}
            }
            return CPUPPImodules;

        }




        /// <summary>
        /// Recursively Creates an ArrayList of PUPPIModules (types) based on Constructors and Methods found in MyType and its fields.
        /// Methods can be filtered, but all constructors are picked.
        /// </summary>
        /// <param name="Mytype">A type (use command .GetType() on a class instance or typeof on a static class)</param>
        /// <param name="includedefaultconstructors">If false, default parameterless constructors are not added to the returned list</param>
        /// <param name="excludetypes">Fields to be excluded from creating PUPPI modules of.</param>
        /// <param name="namefilter">List of method names that should be picked (optional)</param>
        /// <param name="replacefilter">If namefilter used, thisListwill replace the name of the method with the one specified here.</param>
        /// <returns>ArrayList with Types</returns>
        public static ArrayList makeTypeAndSubtypesPUPPImodules(Type Mytype, bool includedefaultconstructors, List<Type> excludetypes = null, List<string> namefilter = null, List<string> replacefilter = null)
        {
            if (excludetypes == null)
            {
                excludetypes = new List<Type>();
            }
            ArrayList allmodules = new ArrayList();
            //FieldInfo[] myField = Mytype.GetFields();
            //for (int i = 0; i < myField.Length; i++)
            //{
            //    // Determine whether or not each field is a special name. 
            //    if (excludetypes.Contains(myField[i].FieldType) == false)
            //    {
            //        excludetypes.Add(myField[i].FieldType);
            //        allmodules.AddRange(makeTypeAndSubtypesPUPPImodules(myField[i].FieldType, includedefaultconstructors, excludetypes, namefilter, replacefilter));

            //    }
            //}

            Type[] myTypes = Mytype.GetNestedTypes();
            for (int i = 0; i < myTypes.Length; i++)
            {
                // Determine whether or not each field is a special name. 
                if (excludetypes.Contains(myTypes[i]) == false)
                {

                    allmodules.AddRange(makeTypeAndSubtypesPUPPImodules(myTypes[i], includedefaultconstructors, excludetypes, namefilter, replacefilter));
                    excludetypes.Add(myTypes[i]);
                }
            }
            if (excludetypes.Contains(Mytype) == false)
                allmodules.AddRange(makeTypePUPPImodules(Mytype, includedefaultconstructors, namefilter, replacefilter));
            //ArrayList a = makeEnumMembersIntoPUPPIModules(Mytype);
            //for (int aa = 0; aa < a.Count; aa++)
            //{
            //    Type t = a[aa] as Type;
            //    if (t != null)
            //    {
            //        CPUPPImodules.Add(t);
            //    }
            //    else
            //    {

            //    }
            //}
            return allmodules;
        }
        /// <summary>
        /// Creates a list of ArrayLists which will contain PUPPI Modules from all the Types and subtypes in a namespace.
        /// This can be used to create one or more menus with PUPPI Modules.
        /// </summary>
        /// <param name="namespacename">The namespace name, make sure it's the whole name including parent namespaces</param>
        /// <param name="includedefaultconstructors">If false, default parameterless constructors are not added to the returned list</param>
        /// <param name="toptypenames">Top level types found in namespace have their names output to a list, so that menus can be created, each menu being an ArrayList of PUPPI Modules from the return list of ArrayLists.</param>
        /// <param name="excludetypes">List of Types to exclude</param>
        /// <param name="namefilter">List of method names that should be picked (optional)</param>
        /// <param name="replacefilter">If namefilter used, thisListwill replace the name of the method with the one specified here.</param>
        /// <returns>A List of ArrayLists, each ArrayList contains PUPPIModule Types</returns>
        public static List<ArrayList> makeNamespacePUPPImoduleLists(string namespacename, bool includedefaultconstructors, out List<string> toptypenames, List<Type> excludetypes = null, List<string> namefilter = null, List<string> replacefilter = null)
        {
           
            toptypenames = new List<string>();
            if (excludetypes == null)
            {
                excludetypes = new List<Type>();
            }
            List<ArrayList> namespacemodules = new List<ArrayList>();
            // return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpaceqqq, StringComparison.Ordinal)).ToArray();
            Type tm = null;
            Type[] internalTypes = null;
            //first try in current project
            var ca = Assembly.GetExecutingAssembly();

            try
            {
                internalTypes = ca.GetTypes().Where(t => String.Equals(t.Namespace, namespacename, StringComparison.Ordinal)).ToArray();

                foreach (Type tip in internalTypes)
                {
                    ArrayList nsm = makeTypeAndSubtypesPUPPImodules(tip, includedefaultconstructors, excludetypes, namefilter, replacefilter);
                    if (nsm.Count > 0)
                    {
                        namespacemodules.Add(nsm);
                        toptypenames.Add(tip.Name);
                        excludetypes.Add(tip);
                    }
                }
            }
            catch
            {

            }
            //now look in other assemblies too

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a != ca)
                {
                    try
                    {
                        internalTypes = a.GetTypes().Where(t => String.Equals(t.Namespace, namespacename, StringComparison.Ordinal)).ToArray();
                        foreach (Type tip in internalTypes)
                        {
                            ArrayList nsm = makeTypeAndSubtypesPUPPImodules(tip, includedefaultconstructors, excludetypes, namefilter, replacefilter);
                            if (nsm.Count > 0)
                            {
                                namespacemodules.Add(nsm);
                                toptypenames.Add(tip.Name);
                                excludetypes.Add(tip);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
            //force load assemblies
            if (namespacemodules.Count == 0)
            {
                if (ca.GetName().FullName.StartsWith("PUPPI,")) ca = Assembly.GetEntryAssembly();
                if (ca == null) ca = utils.PUPPIUtils.getNonPUPPIAssembly();
                if (ca == null) ca = new StackTrace().GetFrames().Last().GetMethod().Module.Assembly;
                //load referenced assemblies
                foreach (AssemblyName an in ca.GetReferencedAssemblies())
                {
                    try
                    {
                        Assembly.Load(an);
                    }
                    catch (Exception exy)
                    {

                    }
                }


                foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                {

                    try
                    {
                        internalTypes = a.GetTypes().Where(t => String.Equals(t.Namespace, namespacename, StringComparison.Ordinal)).ToArray();
                        foreach (Type tip in internalTypes)
                        {
                            ArrayList nsm = makeTypeAndSubtypesPUPPImodules(tip, includedefaultconstructors, excludetypes, namefilter, replacefilter);
                            if (nsm.Count > 0)
                            {
                                namespacemodules.Add(nsm);
                                toptypenames.Add(tip.Name);
                                excludetypes.Add(tip);
                            }
                        }
                    }
                    catch
                    {

                    }
                }

                //still empty
                //load from all dlls in folder of assembly except self and PUPPI
                if (namespacemodules.Count == 0)
                {
                    string codeBase = ca.CodeBase;
                    UriBuilder uri = new UriBuilder(codeBase);
                    string path = Uri.UnescapeDataString(uri.Path);
                    string dirPath = Path.GetDirectoryName(path);
                    string[] fileEntries = Directory.GetFiles(dirPath);
                    foreach (string fileName in fileEntries)
                    {
                        if (fileName.EndsWith(".dll") && !fileName.EndsWith("PUPPI.dll"))
                        {
                            try
                            {
                                Assembly.LoadFrom(fileName);
                            }
                            catch
                            {

                            }
                        }
                    }
                    foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                    {

                        try
                        {
                            internalTypes = a.GetTypes().Where(t => String.Equals(t.Namespace, namespacename, StringComparison.Ordinal)).ToArray();
                            foreach (Type tip in internalTypes)
                            {
                                ArrayList nsm = makeTypeAndSubtypesPUPPImodules(tip, includedefaultconstructors, excludetypes, namefilter, replacefilter);
                                if (nsm.Count > 0)
                                {
                                    namespacemodules.Add(nsm);
                                    toptypenames.Add(tip.Name);
                                    excludetypes.Add(tip);
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }

            }

            return namespacemodules;
        }

    
        /// <summary>
        /// Creates a list of ArrayLists which will contain PUPPI Modules from all the methods saved in an .mtps file generated with the PUPPI Namespace Explorer.
        /// If Assembly specied in MTPS not already loaded DLL is loaded with Unsafe Loading
        /// This can be used to create one or more menus with PUPPI Modules.
        /// </summary>
        /// <param name="filepath">Relative path to a .mtps file.</param>
        /// <param name="includedefaultconstructors">If false, default parameterless constructors are not added to the returned list</param>
        /// <param name="toptypenames">Top level types found in namespace have their names output to a list, so that menus can be created, each menu being an ArrayList of PUPPI Modules from the return list of ArrayLists.</param>
        /// <returns>A List of ArrayLists, each ArrayList contains PUPPIModule Types</returns>

        public static List<ArrayList> makePUPPImoduleListsFromMTPS(string filepath, out List<string> toptypenames)
        {
            try
            {
                try
                {
                    // System.Windows.Forms.MessageBox.Show("removing event ");
                    AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                }
                catch { } //System.Windows.Forms.MessageBox.Show("aDDING event ");
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
          
            toptypenames = new List<string>();


            List<ArrayList> namespacemodules = new List<ArrayList>();
            Type tm = null;
            Type[] internalTypes = null;

            //read all types and associated namespaces from file
            List<string> typeListFile = new List<string>();
            List<string> namespaceListFile = new List<string>();
            List<string> methodListFile = new List<string>();
            int counter = 0;
            string line;

            //  if (!System.IO.File.Exists(filepath )) throw new Exception("MTPS file not found:" +filepath );    
            System.IO.StreamReader file = new System.IO.StreamReader(filepath);
            string assemblyName = "";
            //the path
            string ap = "";
            char[] commaDelimiterChars = { ',' };
            char[] spaceDelimiterChars = { '*' };

            while ((line = file.ReadLine()) != null)
            {
                if (counter == 0)
                {
                    //get assembly name

                    string[] words = line.Split(commaDelimiterChars);
                    assemblyName = words[0];
                    ap = words[4];
                }
                else
                {
                    string[] words = line.Split(spaceDelimiterChars);
                    namespaceListFile.Add(words[0]);
                    typeListFile.Add(words[1]);
                    methodListFile.Add(words[2]);
                }
                counter++;
            }

            file.Close();
            //check assembly loaded
            List<string> asn = AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>().ConvertAll(a => a.GetName().Name);
            if (!asn.Contains(assemblyName))
            {
                bool tl = false;
                try
                {
                    Assembly.UnsafeLoadFrom(ap);// .LoadFrom(ap);
                    asn = AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>().ConvertAll(a => a.GetName().Name);
                    if (!asn.Contains(assemblyName)) tl = true;
                }
                catch
                {
                    tl = true;
                }
                if (tl == true)
                {
                    Assembly aa = null;
                    try
                    {
                        string[] cr = { @"\" };
                        //load from same folder
                        string np = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + @"\" + ap.Split(cr, StringSplitOptions.None)[ap.Split(cr, StringSplitOptions.None).Length - 1]).LocalPath;
                        aa = Assembly.LoadFrom(np);
                    }
                    catch
                    {
                        aa = null;

                    }
                    if (aa == null)
                    {
                        //try better method find same folder
                        var ca = Assembly.GetExecutingAssembly();
                        if (ca.GetName().FullName.StartsWith("PUPPI,")) ca = Assembly.GetEntryAssembly();
                        if (ca == null) ca = utils.PUPPIUtils.getNonPUPPIAssembly();
                        if (ca == null) ca = new StackTrace().GetFrames().Last().GetMethod().Module.Assembly;
                        string[] cr = { @"\" };
                        string np = new Uri(System.IO.Path.GetDirectoryName(ca.CodeBase) + @"\" + ap.Split(cr, StringSplitOptions.None)[ap.Split(cr, StringSplitOptions.None).Length - 1]).LocalPath;
                        try
                        {
                            aa = Assembly.LoadFrom(np);
                        }
                        catch
                        {
                            aa = null;
                        }
                        if (aa == null) throw new Exception("Failed to load assembly " + assemblyName);
                    }
                }

            }

            //get unique namespaces
            HashSet<string> uniqueNamespaces = new HashSet<string>(namespaceListFile);
            //make a tree
            TreeNode myTreeNode = new TreeNode();
            //iterate through namespaces
            foreach (string namespacename in uniqueNamespaces)
            {
                TreeNode nsn = new TreeNode();
                nsn.Text = namespacename;

                //collect all types and methods for that namespace
                List<string> myDoubleTypes = new List<string>();
                foreach (string typespacename in typeListFile)
                {
                    if (namespaceListFile[typeListFile.IndexOf(typespacename)] == namespacename)
                    {
                        myDoubleTypes.Add(typespacename);
                    }
                }
                //now get unique types
                HashSet<string> uniqueTypes = new HashSet<string>(myDoubleTypes);
                foreach (string uType in uniqueTypes)
                {
                    TreeNode tsn = new TreeNode();
                    tsn.Text = uType;
                    foreach (string methodspacename in methodListFile)
                    {
                        if (namespaceListFile[methodListFile.IndexOf(methodspacename)] == namespacename && typeListFile[methodListFile.IndexOf(methodspacename)] == uType)
                        {
                            tsn.Nodes.Add(methodspacename);
                        }
                    }
                    nsn.Nodes.Add(tsn);
                }
                myTreeNode.Nodes.Add(nsn);
            }


            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                //in case there are multiple assemblies with same name it will stop once one has types
                bool fsia = false;
                if (a.GetName().ToString().Split(commaDelimiterChars)[0] == assemblyName)
                {
                    try
                    {
                        //iterate through namespaces
                        foreach (TreeNode nsn in myTreeNode.Nodes)
                        {



                            internalTypes = a.GetTypes().Where(t => String.Equals(t.Namespace, nsn.Text, StringComparison.Ordinal)).ToArray();

                            foreach (TreeNode tsn in nsn.Nodes)
                            {
                                foreach (Type tip in internalTypes)
                                {

                                    if (tip.Name == tsn.Text)
                                    {
                                        ArrayList nsm = new ArrayList();
                                        var an = new AssemblyName("PUPPI" + tip.Name + "as");
                                        AssemblyBuilder mab = null;

                                        mab = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
                                        //ModuleBuilder bm = null;
                                        //if (assemblySaveFolder!="" ) bm=mab.DefineDynamicModule("PUPPI" + tip.Name + "module","PUPPI" + tip.Name + "as.dll") ;

                                        foreach (TreeNode msn in tsn.Nodes)
                                        {

                                            string[] ur = { "__" };
                                            string[] pr = { "(" };
                                            string[] cr = { " Caption " };
                                            string[] words = msn.Text.Split(ur, StringSplitOptions.RemoveEmptyEntries);

                                            if (words[0].StartsWith("Mthd:"))
                                            {
                                                string mn = words[0].Replace("Mthd:", "");
                                                if (mn == "") continue;
                                                string mycnt = words[2].Split(pr, StringSplitOptions.RemoveEmptyEntries)[0];
                                                if (mycnt == "") continue;
                                                int mcnt = 0;
                                                MethodInfo[] smi = tip.GetMethods();
                                                var bmi = smi;//.OrderBy(ami => ami.Name).ThenBy(ami => ami.ReturnType.Name).ThenBy(ami => String.Join(",", ami.GetParameters().Select(x => x.Name + x.ParameterType.Name).ToArray()).Length);

                                                foreach (MethodInfo mi in bmi)
                                                {

                                                    string hame = "m";
                                                    ParameterInfo[] Myarray = mi.GetParameters();
                                                    foreach (ParameterInfo pinfo in Myarray)
                                                    {
                                                        hame += pinfo.Name;
                                                        hame += pinfo.ParameterType.Name;

                                                    }
                                                    int hah = hac(hame);
                                                    //we will do the match  simply by count
                                                    if (mi.Name == mn && hah.ToString() == mycnt)
                                                    {
                                                        //if captioned
                                                        string cn = "";
                                                        string[] wwww = words[2].Split(cr, StringSplitOptions.RemoveEmptyEntries);
                                                        if (wwww.Length == 2) cn = wwww[1];

                                                        Type tmm = makePUPPIModuleFromMethod(mi, hah, mab, cn);
                                                        if (tmm != null) nsm.Add(tmm);
                                                        fsia = true;
                                                        break;
                                                    }


                                                    mcnt++;
                                                }
                                            }
                                            else if (words[0].StartsWith("New"))
                                            {
                                                string mn = words[0].Replace("New ", "");
                                                if (mn == "") continue;
                                                string mycnt = words[1].Split(pr, StringSplitOptions.RemoveEmptyEntries)[0];
                                                if (mycnt == "") continue;
                                                string[] cur = { "_" };
                                                string ccnt = mycnt.Split(cur, StringSplitOptions.RemoveEmptyEntries)[0];
                                                int mcnt = 0;

                                                foreach (ConstructorInfo mi in tip.GetConstructors().OrderBy(aci => aci.Name).ThenBy(aci => String.Join(",", aci.GetParameters().Select(x => x.Name + x.ParameterType.Name).ToArray()).Length))
                                                {

                                                    ParameterInfo[] Myarray = mi.GetParameters();
                                                    string hame = "c";
                                                    foreach (ParameterInfo pinfo in Myarray)
                                                    {
                                                        hame += pinfo.Name;
                                                        hame += pinfo.ParameterType.Name;

                                                    }
                                                    int hah = hac(hame);
                                                    //we will do the match  simply by count
                                                    if (hah.ToString() == ccnt)
                                                    {
                                                        //if captioned
                                                        Type tcc = AutomaticPUPPImodulesCreator.makePUPPIModuleFromConstructor(mi, hah, mab);
                                                        if (tcc != null) nsm.Add(tcc);
                                                        fsia = true;
                                                        break;
                                                    }


                                                    mcnt++;
                                                }

                                            }
                                        }

                                        ////now get all the enums 
                                        //ArrayList ae = makeEnumMembersIntoPUPPIModules(tip, mab);
                                        //for (int aei = 0; aei < ae.Count; aei++)
                                        //{
                                        //    Type tae = ae[aei] as Type;
                                        //    if (tae != null)
                                        //    {
                                        //        bool fd = false;
                                        //        for (int j = 0; j < nsm.Count; j++)
                                        //        {
                                        //            Type lae = nsm[j] as Type;
                                        //            if (lae.Name == tae.Name)
                                        //            {
                                        //                fd = true;
                                        //                fsia = true;
                                        //                break;

                                        //            }
                                        //        }
                                        //        if (!fd) nsm.Add(tae);
                                        //    }
                                        //}

                                        if (tip.IsEnum)
                                        {
                                            Type tae = makePUPPIModuleFromEnum(tip, hac(tip.Name), mab) as Type;
                                            if (tae != null)
                                            {
                                                bool fd = false;
                                                for (int j = 0; j < nsm.Count; j++)
                                                {
                                                    Type lae = nsm[j] as Type;
                                                    if (lae.Name == tae.Name)
                                                    {
                                                        fd = true;
                                                        fsia = true;
                                                        break;
                                                    }
                                                }
                                                if (!fd) nsm.Add(tae);
                                            }

                                        }
                                        if (nsm.Count > 0)
                                        {
                                            ////constructors
                                            //ArrayList cccc = makeTypePUPPImodules(tip, includedefaultconstructors, new List<string>());
                                            //nsm.InsertRange(0, cccc);
                                            namespacemodules.Add(nsm);
                                            toptypenames.Add(tip.Name);

                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exy)
                    {
                        //keep looking
                        fsia = false;
                    }
                    if (fsia) break;
                }
            }
            return namespacemodules;
        }
        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

            dllName = dllName.Replace(".", "_");

            if (dllName.EndsWith("_resources")) return null;
            string an = Assembly.GetExecutingAssembly().GetName().Name;
            //System.Windows.Forms.MessageBox.Show(an);  
            //  System.Windows.Forms.MessageBox.Show(an);
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(an + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

            byte[] bytes = (byte[])rm.GetObject(dllName);

            if (bytes != null && bytes.Length > 0) return System.Reflection.Assembly.Load(bytes); else return null;
        }

        /// <summary>
        /// Creates a list of ArrayLists which will contain PUPPI Modules from all the methods saved in an .mtps file generated with the PUPPI Namespace Explorer.
        /// These are exported to specified folder in assemblies, one per type, with namespace PUPPIAutoModules. To create assemblies that are used at runtime, use makePUPPImoduleListsFromMTPS
        /// </summary>
        /// <param name="filepath">Relative path to a .mtps file.</param>
        /// <param name="toptypenames">Top level types found in namespace have their names output to a list, so that menus can be created, each menu being an ArrayList of PUPPI Modules from the return list of ArrayLists.</param>
        /// <param name="assemblySaveFolder">Dll assemblies for each type are saved to the folder specified. No \ at the end</param>
        /// <param name="suffix">String added at the end of every generated assembly file.</param>
        /// <returns>A List of ArrayLists, each ArrayList contains PUPPIModule Types</returns>

        public static List<ArrayList> exportPUPPImoduleListsFromMTPS(string filepath, out List<string> toptypenames, string assemblySaveFolder, string suffix = "as")
        {

            suffix = suffix.Replace(".", "").Replace(" ", "");
            if (suffix == "") suffix = "as";
            try
            {
                try
                {
                    // System.Windows.Forms.MessageBox.Show("removing event ");
                    AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                }
                catch { } //System.Windows.Forms.MessageBox.Show("aDDING event ");
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }

     

            if (assemblySaveFolder.Last() == '\\') assemblySaveFolder = assemblySaveFolder.Remove(assemblySaveFolder.Length - 1);
            if (assemblySaveFolder == "") throw new Exception("No assembly deployment folder specified!");



            if (!System.IO.Directory.Exists(assemblySaveFolder)) throw new Exception("Assembly folder does not exist!");
            return ePmLFM(filepath, out  toptypenames, assemblySaveFolder, suffix);


        }
        internal static List<ArrayList> ePmLFM(string filepath, out List<string> toptypenames, string assemblySaveFolder, string suffix = "as")
        {
            
            toptypenames = new List<string>();
            List<ArrayList> namespacemodules = new List<ArrayList>();
            Type tm = null;
            Type[] internalTypes = null;

            //read all types and associated namespaces from file
            List<string> typeListFile = new List<string>();
            List<string> namespaceListFile = new List<string>();
            List<string> methodListFile = new List<string>();
            int counter = 0;
            string line;

            //  if (!System.IO.File.Exists(filepath )) throw new Exception("MTPS file not found:" +filepath );    
            System.IO.StreamReader file = new System.IO.StreamReader(filepath);
            string assemblyName = "";
            //the path
            string ap = "";
            char[] commaDelimiterChars = { ',' };
            char[] spaceDelimiterChars = { '*' };

            while ((line = file.ReadLine()) != null)
            {
                if (counter == 0)
                {
                    //get assembly name

                    string[] words = line.Split(commaDelimiterChars);
                    assemblyName = words[0];
                    ap = words[4];
                }
                else
                {
                    string[] words = line.Split(spaceDelimiterChars);
                    namespaceListFile.Add(words[0]);
                    typeListFile.Add(words[1]);
                    methodListFile.Add(words[2]);
                }
                counter++;
            }

            file.Close();
            //check assembly loaded
            List<string> asn = AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>().ConvertAll(a => a.GetName().Name);
            if (!asn.Contains(assemblyName))
            {
                bool tl = false;
                try
                {
                    Assembly.LoadFrom(ap);
                    asn = AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>().ConvertAll(a => a.GetName().Name);
                    if (!asn.Contains(assemblyName)) tl = true;
                }
                catch
                {
                    tl = true;
                }
                if (tl == true)
                {
                    Assembly aa = null;
                    try
                    {
                        string[] cr = { @"\" };
                        //load from same folder
                        string np = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + @"\" + ap.Split(cr, StringSplitOptions.None)[ap.Split(cr, StringSplitOptions.None).Length - 1]).LocalPath;
                        Assembly.LoadFrom(np);
                    }
                    catch
                    {
                        aa = null;

                    }
                    if (aa == null)
                    {
                        //try better method find same folder
                        var ca = Assembly.GetExecutingAssembly();
                        if (ca.GetName().FullName.StartsWith("PUPPI,")) ca = Assembly.GetEntryAssembly();
                        if (ca == null) ca = utils.PUPPIUtils.getNonPUPPIAssembly();
                        if (ca == null) ca = new StackTrace().GetFrames().Last().GetMethod().Module.Assembly;
                        string[] cr = { @"\" };
                        string np = new Uri(System.IO.Path.GetDirectoryName(ca.CodeBase) + @"\" + ap.Split(cr, StringSplitOptions.None)[ap.Split(cr, StringSplitOptions.None).Length - 1]).LocalPath;
                        try
                        {
                            aa = Assembly.LoadFrom(np);
                        }
                        catch
                        {
                            aa = null;
                        }
                        if (aa == null) throw new Exception("Failed to load assembly " + assemblyName);
                    }
                }
            }

            //get unique namespaces
            HashSet<string> uniqueNamespaces = new HashSet<string>(namespaceListFile);
            //make a tree
            TreeNode myTreeNode = new TreeNode();
            //iterate through namespaces
            foreach (string namespacename in uniqueNamespaces)
            {
                TreeNode nsn = new TreeNode();
                nsn.Text = namespacename;

                //collect all types and methods for that namespace
                List<string> myDoubleTypes = new List<string>();
                foreach (string typespacename in typeListFile)
                {
                    if (namespaceListFile[typeListFile.IndexOf(typespacename)] == namespacename)
                    {
                        myDoubleTypes.Add(typespacename);
                    }
                }
                //now get unique types
                HashSet<string> uniqueTypes = new HashSet<string>(myDoubleTypes);
                foreach (string uType in uniqueTypes)
                {
                    TreeNode tsn = new TreeNode();
                    tsn.Text = uType;
                    foreach (string methodspacename in methodListFile)
                    {
                        if (namespaceListFile[methodListFile.IndexOf(methodspacename)] == namespacename && typeListFile[methodListFile.IndexOf(methodspacename)] == uType)
                        {
                            tsn.Nodes.Add(methodspacename);
                        }
                    }
                    nsn.Nodes.Add(tsn);
                }
                myTreeNode.Nodes.Add(nsn);
            }


            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                bool fsia = false;
                if (a.GetName().ToString().Split(commaDelimiterChars)[0] == assemblyName)
                {
                    try
                    {
                        //iterate through namespaces
                        foreach (TreeNode nsn in myTreeNode.Nodes)
                        {



                            internalTypes = a.GetTypes().Where(t => String.Equals(t.Namespace, nsn.Text, StringComparison.Ordinal)).ToArray();

                            foreach (TreeNode tsn in nsn.Nodes)
                            {
                                try
                                {
                                    foreach (Type tip in internalTypes)
                                    {
                                        if (tip.IsAbstract && !tip.IsSealed) continue;
                                        if (tip.Name == tsn.Text)
                                        {
                                            ArrayList nsm = new ArrayList();
                                            var an = new AssemblyName("PUPPI" + tip.Name.Replace("<", "").Replace(">", "") + suffix);
                                            AssemblyBuilder mab = null;
                                            if (assemblySaveFolder != "")
                                                mab = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndSave, assemblySaveFolder);
                                            else
                                                mab = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
                                            ModuleBuilder bm = null;
                                            if (assemblySaveFolder != "") bm = mab.DefineDynamicModule("PUPPI" + tip.Name.Replace("<", "").Replace(">", "") + "module", "PUPPI" + tip.Name.Replace("<", "").Replace(">", "") + suffix + ".dll");

                                            foreach (TreeNode msn in tsn.Nodes)
                                            {

                                                string[] ur = { "__" };
                                                string[] pr = { "(" };
                                                string[] cr = { " Caption " };
                                                string[] words = msn.Text.Split(ur, StringSplitOptions.RemoveEmptyEntries);

                                                if (words[0].StartsWith("Mthd:"))
                                                {
                                                    string mn = words[0].Replace("Mthd:", "");
                                                    if (mn == "") continue;
                                                    string mycnt = words[2].Split(pr, StringSplitOptions.RemoveEmptyEntries)[0];
                                                    if (mycnt == "") continue;
                                                    int mcnt = 0;
                                                    MethodInfo[] smi = tip.GetMethods();
                                                    var bmi = smi;//.OrderBy(ami => ami.Name).ThenBy(ami => ami.ReturnType.Name).ThenBy(ami => String.Join(",", ami.GetParameters().Select(x => x.Name + x.ParameterType.Name).ToArray()).Length);

                                                    foreach (MethodInfo mi in bmi)
                                                    {

                                                        string hame = "m";
                                                        ParameterInfo[] Myarray = mi.GetParameters();
                                                        foreach (ParameterInfo pinfo in Myarray)
                                                        {
                                                            hame += pinfo.Name;
                                                            hame += pinfo.ParameterType.Name;

                                                        }
                                                        int hah = hac(hame);
                                                        //we will do the match  by hashcode from parameter types and names
                                                        if (mi.Name == mn && hah.ToString() == mycnt)
                                                        {
                                                            //if captioned
                                                            string cn = "";
                                                            string[] wwww = words[2].Split(cr, StringSplitOptions.RemoveEmptyEntries);
                                                            if (wwww.Length == 2) cn = wwww[1];
                                                            Type tam = makePUPPIModuleFromMethod(mi, hah, mab, cn, bm);
                                                            if (tam != null) nsm.Add(tam);
                                                            fsia = true;
                                                            break;
                                                        }


                                                        mcnt++;
                                                    }
                                                }
                                                else if (words[0].StartsWith("New"))
                                                {
                                                    string mn = words[0].Replace("New ", "");
                                                    if (mn == "") continue;
                                                    string mycnt = words[1].Split(pr, StringSplitOptions.RemoveEmptyEntries)[0];
                                                    if (mycnt == "") continue;
                                                    string[] cur = { "_" };
                                                    string ccnt = mycnt.Split(cur, StringSplitOptions.RemoveEmptyEntries)[0];
                                                    int mcnt = 0;

                                                    foreach (ConstructorInfo mi in tip.GetConstructors())//.OrderBy(aci => aci.Name).ThenBy(aci => String.Join(",", aci.GetParameters().Select(x => x.Name + x.ParameterType.Name).ToArray()).Length))
                                                    {

                                                        ParameterInfo[] Myarray = mi.GetParameters();
                                                        string hame = "c";
                                                        foreach (ParameterInfo pinfo in Myarray)
                                                        {
                                                            hame += pinfo.Name;
                                                            hame += pinfo.ParameterType.Name;

                                                        }
                                                        int hah = hac(hame);
                                                        //we will do the match  simply by count
                                                        if (hah.ToString() == ccnt)
                                                        {
                                                            //if captioned
                                                            Type tcc = AutomaticPUPPImodulesCreator.makePUPPIModuleFromConstructor(mi, hah, mab, bm);
                                                            if (tcc != null) nsm.Add(tcc);
                                                            fsia = true;
                                                            break;
                                                        }


                                                        mcnt++;
                                                    }

                                                }
                                                // ArrayList ae = makeEnumMembersIntoPUPPIModules(tip, mab,bm);
                                                //now get all the enums 
                                                //ArrayList ae = makeEnumMembersIntoPUPPIModules(tip, mab, bm);
                                                //for (int aei = 0; aei < ae.Count; aei++)
                                                //{
                                                //    Type tae = ae[aei] as Type;
                                                //    if (tae != null)
                                                //    {
                                                //        bool fd = false;
                                                //        for (int j = 0; j < nsm.Count; j++)
                                                //        {
                                                //            Type lae = nsm[j] as Type;
                                                //            if (lae.Name == tae.Name)
                                                //            {
                                                //                fd = true;
                                                //                fsia = true;
                                                //                break;
                                                //            }
                                                //        }
                                                //        if (!fd) nsm.Add(tae);
                                                //    }
                                                //}

                                                if (tip.IsEnum)
                                                {
                                                    Type tae = makePUPPIModuleFromEnum(tip, hac(tip.Name), mab, bm) as Type;
                                                    if (tae != null)
                                                    {
                                                        bool fd = false;
                                                        for (int j = 0; j < nsm.Count; j++)
                                                        {
                                                            Type lae = nsm[j] as Type;
                                                            if (lae.Name == tae.Name)
                                                            {
                                                                fd = true;
                                                                fsia = true;
                                                                break;
                                                            }
                                                        }
                                                        if (!fd) nsm.Add(tae);
                                                    }

                                                }
                                                //for (int aei = 0; aei < ae.Count; aei++)
                                                //{
                                                //    Type tae = ae[aei] as Type;
                                                //    if (tae != null) nsm.Add(tae);
                                                //}
                                                //if (tip.IsEnum)
                                                //{
                                                //    nsm.Add(makePUPPIModuleFromEnum(tip, 0, mab,bm));
                                                //}

                                                //nsm.AddRange(makeEnumMembersIntoPUPPIModules(tip, mab, bm));
                                            }
                                            if (nsm.Count > 0)
                                            {
                                                ////constructors
                                                //ArrayList cccc = makeTypePUPPImodules(tip, includedefaultconstructors, new List<string>());
                                                //nsm.InsertRange(0, cccc);
                                                namespacemodules.Add(nsm);
                                                toptypenames.Add(tip.Name);
                                                Assembly mainAssembly = typeof(PUPPIModule).Assembly;

                                                //Module[] mods = mainAssembly.GetModules();

                                                //foreach (Module m in mods)

                                                //                                        mab.
                                                if (assemblySaveFolder != "")
                                                {

                                                    mab.Save(an.Name + ".dll");


                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                                catch (Exception exy)
                                {
                                    if (PUPPIDebugger.debugenabled == true)
                                    {
                                        PUPPIDebugger.log("Error creating  PUPPIModule from method for export to file" + exy.ToString());
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception exy)
                    {
                        if (PUPPIDebugger.debugenabled == true)
                        {
                            PUPPIDebugger.log("Error creating PUPPIModule assembly file from MTPS  " + exy.ToString());
                        }
                        fsia = false;
                    }
                    if (fsia) break;
                }
            }
            return namespacemodules;
        }


        /// <summary>
        /// Creates a list of ArrayLists which will contain PUPPI Modules from all the methods in a loaded namespace.
        /// These are exported to specified folder in assemblies, one per type, with namespace PUPPIAutoModules. To create assemblies that are used at runtime, use makePUPPImoduleListsFromNamespace
        /// </summary>
        /// <param name="namespaceName">Full namespace name</param>
        /// <param name="toptypenames">Top level types found in namespace have their names output to a list, so that menus can be created, each menu being an ArrayList of PUPPI Modules from the return list of ArrayLists.</param>
        /// <param name="assemblySaveFolder">Dll assemblies for each type are saved to the folder specified. No \ at the end</param>
        /// <param name="assemblyName">>optional, full or part of the assembly name, if same namespace name in multiple assemblies</param>
        /// <param name="suffix">String added at the end of every generated assembly file.</param>
        /// <returns>A List of ArrayLists, each ArrayList contains PUPPIModule Types</returns>
        public static List<ArrayList> exportPUPPImoduleListsFromNamespace(string namespaceName, out List<string> toptypenames, string assemblySaveFolder, string assemblyName = "", string suffix = "as")
        {
            suffix = suffix.Replace(".", "").Replace(" ", "");
            if (suffix == "") suffix = "as";
            try
            {
                try
                {
                    // System.Windows.Forms.MessageBox.Show("removing event ");
                    AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                }
                catch { } //System.Windows.Forms.MessageBox.Show("aDDING event ");
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
       
            if (assemblySaveFolder.Last() == '\\') assemblySaveFolder = assemblySaveFolder.Remove(assemblySaveFolder.Length - 1);
            if (assemblySaveFolder == "") throw new Exception("No assembly deployment folder specified!");

            if (!System.IO.Directory.Exists(assemblySaveFolder)) throw new Exception("Assembly folder does not exist!");

            return ePmLFN(namespaceName, out toptypenames, assemblySaveFolder, assemblyName, suffix);



        }

        internal static List<ArrayList> ePmLFN(string namespaceName, out List<string> toptypenames, string assemblySaveFolder, string assemblyName = "", string suffix = "as")
        {
     
            toptypenames = new List<string>();
            List<ArrayList> namespacemodules = new List<ArrayList>();
            Type tm = null;
            Type[] internalTypes = null;

            //read all types and associated namespaces from file
            List<string> typeListFile = new List<string>();
            List<string> namespaceListFile = new List<string>();
            List<string> methodListFile = new List<string>();
            int counter = 0;
            //test make sure it's loaded
            bool found = false;
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assemblyName == "" || a.GetName().ToString().Contains(assemblyName))
                {
                    try
                    {
                        internalTypes = a.GetTypes().Where(t => String.Equals(t.Namespace, namespaceName, StringComparison.Ordinal)).ToArray();
                        if (internalTypes.Length > 0)
                        {
                            found = true;
                            break;
                        }
                    }
                    catch
                    {
                        internalTypes = null;
                    }
                }
            }
            var ca = Assembly.GetExecutingAssembly();
            if (found == false)
            {
                if (ca.GetName().FullName.StartsWith("PUPPI,")) ca = Assembly.GetEntryAssembly();
                if (ca == null) ca = utils.PUPPIUtils.getNonPUPPIAssembly();
                if (ca == null) ca = new StackTrace().GetFrames().Last().GetMethod().Module.Assembly;
                //load referenced assemblies
                foreach (AssemblyName an in ca.GetReferencedAssemblies())
                {
                    try
                    {
                        Assembly.Load(an);
                    }
                    catch (Exception exy)
                    {

                    }
                }


                foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                {

                    if (assemblyName == "" || a.GetName().ToString().Contains(assemblyName))
                    {
                        try
                        {
                            internalTypes = a.GetTypes().Where(t => String.Equals(t.Namespace, namespaceName, StringComparison.Ordinal)).ToArray();
                            if (internalTypes.Length > 0)
                            {
                                found = true;
                                break;
                            }
                        }
                        catch
                        {
                            internalTypes = null;
                        }
                    }
                }
            }
            if (found == false)
            {
                //still empty
                //load from all dlls in folder of assembly except self and PUPPI
                string codeBase = ca.CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                string dirPath = Path.GetDirectoryName(path);
                string[] fileEntries = Directory.GetFiles(dirPath);
                foreach (string fileName in fileEntries)
                {
                    if (fileName.EndsWith(".dll") && !fileName.EndsWith("PUPPI.dll"))
                    {
                        try
                        {
                            Assembly.LoadFrom(fileName);
                        }
                        catch
                        {

                        }
                    }
                }
                foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assemblyName == "" || a.GetName().ToString().Contains(assemblyName))
                    {

                        try
                        {
                            internalTypes = a.GetTypes().Where(t => String.Equals(t.Namespace, namespaceName, StringComparison.Ordinal)).ToArray();
                            if (internalTypes.Length > 0)
                            {
                                found = true;
                                break;
                            }
                        }
                        catch
                        {
                            internalTypes = null;
                        }
                    }
                }


            }

            if (found == false)
                throw new Exception("Failed to find namespace " + namespaceName);

            internalTypes = null;

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {

                if (assemblyName == "" || a.GetName().ToString().Contains(assemblyName))
                {


                    internalTypes = a.GetTypes().Where(t => String.Equals(t.Namespace, namespaceName, StringComparison.Ordinal)).ToArray();
                    if (internalTypes.Length > 0)
                    {

                        foreach (Type tip in internalTypes)
                        {
                            //invalid
                            if (tip.IsAbstract && !tip.IsSealed) continue;
                            try
                            {

                                ArrayList nsm = new ArrayList();
                                var an = new AssemblyName("PUPPI" + tip.Name.Replace("<", "").Replace(">", "") + suffix);
                                AssemblyBuilder mab = null;
                                if (assemblySaveFolder != "")
                                    mab = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndSave, assemblySaveFolder);
                                else
                                    mab = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
                                ModuleBuilder bm = null;
                                if (assemblySaveFolder != "") bm = mab.DefineDynamicModule("PUPPI" + tip.Name.Replace("<", "").Replace(">", "") + "module", "PUPPI" + tip.Name.Replace("<", "").Replace(">", "") + suffix + ".dll");


                                int mcnt = 0;
                                MethodInfo[] smi = tip.GetMethods();
                                var bmi = smi;//.OrderBy(ami => ami.Name).ThenBy(ami => ami.ReturnType.Name).ThenBy(ami => String.Join(",", ami.GetParameters().Select(x => x.Name + x.ParameterType.Name).ToArray()).Length);

                                foreach (MethodInfo mi in bmi)
                                {



                                    string cn = "";
                                    string hame = "m";
                                    ParameterInfo[] Myarray = mi.GetParameters();
                                    foreach (ParameterInfo pinfo in Myarray)
                                    {
                                        hame += pinfo.Name;
                                        hame += pinfo.ParameterType.Name;

                                    }
                                    Type tam = makePUPPIModuleFromMethod(mi, hac(hame), mab, cn, bm);
                                    if (tam != null) nsm.Add(tam);




                                    mcnt++;
                                }

                                mcnt = 0;
                                foreach (ConstructorInfo mi in tip.GetConstructors())//.OrderBy(aci => aci.Name).ThenBy(aci => String.Join(",", aci.GetParameters().Select(x => x.Name + x.ParameterType.Name).ToArray()).Length))
                                {

                                    ParameterInfo[] Myarray = mi.GetParameters();
                                    string hame = "c";
                                    foreach (ParameterInfo pinfo in Myarray)
                                    {
                                        hame += pinfo.Name;
                                        hame += pinfo.ParameterType.Name;

                                    }

                                    //if captioned
                                    Type tcc = AutomaticPUPPImodulesCreator.makePUPPIModuleFromConstructor(mi, hac(hame), mab, bm);
                                    if (tcc != null) nsm.Add(tcc);



                                    mcnt++;
                                }


                                // ArrayList ae = makeEnumMembersIntoPUPPIModules(tip, mab,bm);
                                //now get all the enums 
                                ArrayList ae = makeEnumMembersIntoPUPPIModules(tip, mab, bm);
                                for (int aei = 0; aei < ae.Count; aei++)
                                {
                                    Type tae = ae[aei] as Type;
                                    if (tae != null)
                                    {
                                        bool fd = false;
                                        for (int j = 0; j < nsm.Count; j++)
                                        {
                                            Type lae = nsm[j] as Type;
                                            if (lae.Name == tae.Name)
                                            {
                                                fd = true;
                                                break;
                                            }
                                        }
                                        if (!fd) nsm.Add(tae);
                                    }
                                }

                                if (tip.IsEnum)
                                {
                                    Type tae = makePUPPIModuleFromEnum(tip, hac(tip.Name), mab, bm) as Type;
                                    if (tae != null)
                                    {
                                        bool fd = false;
                                        for (int j = 0; j < nsm.Count; j++)
                                        {
                                            Type lae = nsm[j] as Type;
                                            if (lae.Name == tae.Name)
                                            {
                                                fd = true;
                                                break;
                                            }
                                        }
                                        if (!fd) nsm.Add(tae);
                                    }

                                }

                                if (nsm.Count > 0)
                                {
                                    ////constructors
                                    //ArrayList cccc = makeTypePUPPImodules(tip, includedefaultconstructors, new List<string>());
                                    //nsm.InsertRange(0, cccc);
                                    namespacemodules.Add(nsm);
                                    toptypenames.Add(tip.Name);
                                    Assembly mainAssembly = typeof(PUPPIModule).Assembly;

                                    //Module[] mods = mainAssembly.GetModules();

                                    //foreach (Module m in mods)

                                    //                                        mab.
                                    if (assemblySaveFolder != "")
                                    {

                                        mab.Save(an.Name + ".dll");


                                    }
                                }
                            }
                            catch
                            {

                            }
                        }
                        break;
                    }
                }
            }
            return namespacemodules;
        }



        internal static ArrayList makeEnumMembersIntoPUPPIModules(Type classType, AssemblyBuilder ab, ModuleBuilder mb = null)
        {
            ArrayList rn = new ArrayList();
            int ecnt = 0;
            foreach (Type t in classType.GetNestedTypes())
            {
                if (t.IsEnum)
                {
                    int hah = hac(t.Name);
                    rn.Add(makePUPPIModuleFromEnum(t, hah, ab, mb));
                    ecnt++;
                }
            }
            return rn;
        }

        /// <summary>
        /// Makes a single method from a specified Type into a PUPPI Module.
        /// </summary>
        /// <param name="Mytype">A type (use command .GetType() on a class instance or typeof on a static class)</param>
        /// <param name="methodname">Name of the method</param>
        /// <param name="parametertypes">List of method parameter types to ensure unique method specified.Can be null for parameterless methods.</param>
        /// <returns>PUPPIModule Type</returns>
        public static Type makeMethodIntoPUPPIModuleType(Type Mytype, string methodname, List<Type> parametertypes = null)
        {


            int isclass = 1;
            string classtypename = Mytype.Name;
            string nametrail = "";
            if (parametertypes == null || parametertypes.Count == 0)
            {
                nametrail = "noparams";
            }
            else
            {
                foreach (Type tt in parametertypes)
                {
                    nametrail += tt.Name;
                }
            }
            var an = new AssemblyName("PUPPI" + classtypename + "as" + methodname + nametrail);
            AssemblyBuilder mb = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);

            //now get the methods with this name
            MethodInfo[] foundbyname = Mytype.GetMethods().Where(meth => String.Equals(meth.Name, methodname, StringComparison.Ordinal)).ToArray();
            MethodInfo fmethod = null;
            if (foundbyname != null)
            {


                for (int i = 0; i < foundbyname.Length; i++)
                {
                    MethodInfo mymem = foundbyname[i];
                    bool mfound = true;
                    int gp = 0;
                    if (mymem.GetParameters().Length == 0 && (parametertypes != null && parametertypes.Count > 0))
                    { mfound = false; continue; }
                    foreach (ParameterInfo pi in mymem.GetParameters())
                    {
                        if (parametertypes == null || parametertypes.Count == 0)
                        {
                            mfound = false;
                            break;
                        }
                        if (pi.ParameterType != parametertypes[gp])
                        {
                            mfound = false;
                            break;
                        }
                        gp++;
                    }

                    if (mfound == true)
                    {
                        fmethod = foundbyname[i];
                        break;
                    }
                }
            }

            if (fmethod == null)
                return null;
            // MethodInfo[] smi = Mytype.GetMethods();
            //var bmi = smi.OrderBy(ami => ami.Name).ThenBy(ami => ami.ReturnType.Name).ThenBy(ami => String.Join(",", ami.GetParameters().Select(x => x.Name + x.ParameterType.Name).ToArray()).Length);
            //List<MethodInfo> nmi = bmi.ToList<MethodInfo>();


            //int mcnt = Mytype.GetMethods().ToList<MethodInfo>().IndexOf(fmethod);
            //int mcnt = nmi.IndexOf(fmethod);

            string hame = "m";
            ParameterInfo[] Myarray = fmethod.GetParameters();
            foreach (ParameterInfo pinfo in Myarray)
            {
                hame += pinfo.Name;
                hame += pinfo.ParameterType.Name;

            }
            return makePUPPIModuleFromMethod(fmethod, hac(hame), mb);

        }
        /// <summary>
        /// Instantiates a PUPPIModule as base class from a PUPPIModule type, to be able to add them to menus.
        /// </summary>
        /// <param name="PUPPIModuleClass">a Type that inherits from PUPPIModule</param>
        public static PUPPIModule instantiatePUPPIModule(Type PUPPIModuleClass)
        {
            return (PUPPIModule)Activator.CreateInstance(PUPPIModuleClass);
        }


        internal static Type makePUPPIModuleFromMethod(MethodInfo methinfo, int mcnt, AssemblyBuilder myAssemblyBuilder, string caption_replace = "", ModuleBuilder fileModuleBuilder = null)
        {
            try
            {

                //avoid static methods with no arguments for now
                if (methinfo.IsStatic == true && methinfo.GetParameters().LongLength == 0) return null;
                int isclass = 1;
                Type Mytype = methinfo.DeclaringType;
                string classtypename = Mytype.Name;
                //var an = new AssemblyName("PUPPI" + classtypename + "_" + methinfo.Name + mcnt.ToString() + "as");
                // AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
                ModuleBuilder myModuleBuilder;
                if (fileModuleBuilder != null)
                {
                    myModuleBuilder = fileModuleBuilder;
                }
                else
                {
                    myModuleBuilder = myAssemblyBuilder.DefineDynamicModule(Mytype.Name + "Module" + methinfo.Name + mcnt.ToString());

                }
                ParameterInfo[] Myarray = methinfo.GetParameters();
                var typeSignature = "";
                //if (fileModuleBuilder != null)
                //{
                typeSignature = "PUPPIAutoModules.Method." + Mytype.FullName.Replace("__", "").Replace("+", "") + "." + methinfo.Name + "__" + mcnt.ToString();
                //}
                //else
                //{
                //  typeSignature = "Mthd:" + methinfo.Name + "__" + Mytype.Name + "__" + mcnt.ToString();
                //}

                //for saving file
                caption_replace = caption_replace.Replace(".", "").Replace("__", "").Replace("+", "");
                string newname = typeSignature;
                if (caption_replace != "")
                {



                    newname = "PUPPIAutoModules.Method." + caption_replace;



                }



                TypeBuilder tb = myModuleBuilder.DefineType(typeSignature
                                    , TypeAttributes.Public |
                                    TypeAttributes.Class
                    /*|TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout*/
                                    , new PUPPIModule().GetType());
                //override constructor
                ConstructorBuilder ctor1 = tb.DefineConstructor(
      MethodAttributes.Public,
      CallingConventions.Standard, null);

                ILGenerator ctor1IL = ctor1.GetILGenerator();
                ////if saved as a file makes sure assembly loaded
                //if (fileModuleBuilder != null)
                //{
                //    MethodInfo la = tb.BaseType.GetMethod("selfLoad", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                //    ctor1IL.Emit(OpCodes.Ldarg_0);
                //    ctor1IL.Emit(OpCodes.Call, la);
                //}



                ctor1IL.Emit(OpCodes.Ldarg_0);
                //get base constructor
                var bc = tb.BaseType.GetConstructor(System.Type.EmptyTypes);
                ctor1IL.Emit(OpCodes.Call, bc);
                ctor1IL.Emit(OpCodes.Ldarg_0);
                //name
                //get an existing field
                FieldInfo ff = tb.BaseType.GetField("name", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                ctor1IL.Emit(OpCodes.Ldstr, newname);
                ctor1IL.Emit(OpCodes.Stfld, ff);
                //get an existing field
                ctor1IL.Emit(OpCodes.Ldarg_0);
                FieldInfo fff = tb.BaseType.GetField("methodname", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                ctor1IL.Emit(OpCodes.Ldstr, methinfo.Name);
                ctor1IL.Emit(OpCodes.Stfld, fff);
                //set calling type name for easier retrieval
                ctor1IL.Emit(OpCodes.Ldarg_0);
                fff = tb.BaseType.GetField("ctn", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                ctor1IL.Emit(OpCodes.Ldstr, Mytype.AssemblyQualifiedName);
                ctor1IL.Emit(OpCodes.Stfld, fff);


                //add inputs
                //first one with the class itself
                //but if we have a struct we don't have constructors so...
                //also, if the method is static
                MethodInfo addinputinfo = tb.BaseType.GetMethod("addinput", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                //replace ccnt==0
                // if (Mytype.IsClass == false || methinfo.IsStatic == true)
                if (methinfo.IsStatic == true)
                {
                    isclass = 0;
                }
                else
                {
                    ctor1IL.Emit(OpCodes.Ldarg_0);
                    ctor1IL.Emit(OpCodes.Ldstr, "Inst:" + Mytype.Name);
                    ctor1IL.Emit(OpCodes.Ldc_I4, 0);
                    ctor1IL.Emit(OpCodes.Ldstr, Mytype.AssemblyQualifiedName);
                    ctor1IL.Emit(OpCodes.Ldc_I4, isclass);
                    ctor1IL.Emit(OpCodes.Call, addinputinfo);
                }
                //and when we call we need to take that into account

                //method return type
                //if void,reference return


                MethodInfo addoutputinfo = tb.BaseType.GetMethod("addoutput", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);


                string rtype = "nada";
                if (methinfo.ReturnType != typeof(void))
                {
                    //for simple types
                    ctor1IL.Emit(OpCodes.Ldarg_0);

                    rtype = "Ret:" + methinfo.ReturnType.Name;

                    //else
                    //{
                    //    rtype = "InstOut:" + Mytype.Name;
                    //}
                    ctor1IL.Emit(OpCodes.Ldstr, rtype);
                    //if (methinfo.ReturnType==typeof(int))
                    //{
                    ctor1IL.Emit(OpCodes.Ldc_I4, 0);
                    ctor1IL.Emit(OpCodes.Ldc_I4, 0);
                    //ctor1IL.EmitCall(OpCodes.Call,addoutputinfo, new Type[] { typeof(string), typeof(int) });
                    //if (methinfo.ReturnType != typeof(void))
                    //{
                    ctor1IL.Emit(OpCodes.Ldstr, methinfo.ReturnType.AssemblyQualifiedName);
                    //}
                    //else
                    //{
                    //    ctor1IL.Emit(OpCodes.Ldstr, Mytype.AssemblyQualifiedName);
                    //}
                    ctor1IL.Emit(OpCodes.Call, addoutputinfo);
                }
                else
                {

                    int aaaa = 0;
                }


                //get method input parameters and return
                //parameters with out will become outputs

                int iindex = 0;
                // first input was class instance if not static
                if (isclass == 1)
                {
                    iindex = 1;
                }
                //now we consider all params as class params
                isclass = 1;
                int oindex = 0;


                //first output can be taken over by return if not void
                if (methinfo.ReturnType != typeof(void))
                {
                    oindex = 1;
                }

                string parlist = "(";
                foreach (ParameterInfo pinfo in Myarray)
                {
                    string useName = "";
                    if (pinfo.Name == null) useName = pinfo.ParameterType.Name; else useName = pinfo.Name;
                    if (pinfo.IsOut == true)
                    {
                        addoutputinfo = tb.BaseType.GetMethod("addoutput", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                        //for simple types
                        ctor1IL.Emit(OpCodes.Ldarg_0);
                        ctor1IL.Emit(OpCodes.Ldstr, useName);
                        //if (methinfo.ReturnType==typeof(int))
                        //{
                        ctor1IL.Emit(OpCodes.Ldc_I4, 0);
                        ctor1IL.Emit(OpCodes.Ldc_I4, oindex);
                        //ctor1IL.EmitCall(OpCodes.Call,addoutputinfo, new Type[] { typeof(string), typeof(int) });
                        ctor1IL.Emit(OpCodes.Ldstr, pinfo.ParameterType.AssemblyQualifiedName);
                        ctor1IL.Emit(OpCodes.Call, addoutputinfo);
                        oindex++;
                        parlist += "out " + pinfo.ParameterType.Name + " " + useName + ",";
                    }
                    else
                    {
                        addinputinfo = tb.BaseType.GetMethod("addinput", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                        ctor1IL.Emit(OpCodes.Ldarg_0);
                        ctor1IL.Emit(OpCodes.Ldstr, useName);
                        ctor1IL.Emit(OpCodes.Ldc_I4, iindex);
                        ctor1IL.Emit(OpCodes.Ldstr, pinfo.ParameterType.AssemblyQualifiedName);
                        ctor1IL.Emit(OpCodes.Ldc_I4, isclass);
                        ctor1IL.Emit(OpCodes.Call, addinputinfo);
                        iindex++;
                        parlist += pinfo.ParameterType.Name + " " + useName + ",";
                    }
                }

                ////output reference to class at the end so that we can track changes
                //if (methinfo.ReturnType != typeof(void) && methinfo.IsStatic == false)
                //{
                //    addoutputinfo = tb.BaseType.GetMethod("addoutput", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                //    //for simple types
                //    ctor1IL.Emit(OpCodes.Ldarg_0);


                //    rtype = "InstOut:" + Mytype.Name;

                //    ctor1IL.Emit(OpCodes.Ldstr, rtype);
                //    ctor1IL.Emit(OpCodes.Ldc_I4, 0);
                //    ctor1IL.Emit(OpCodes.Ldc_I4, oindex);
                //    ctor1IL.Emit(OpCodes.Ldstr, Mytype.AssemblyQualifiedName);

                //    ctor1IL.Emit(OpCodes.Call, addoutputinfo);
                //}
                //9-4-16
                //output reference to class at the end so that we can track changes
                if (methinfo.IsStatic == false)
                {

                    addoutputinfo = tb.BaseType.GetMethod("addoutput", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                    //for simple types
                    ctor1IL.Emit(OpCodes.Ldarg_0);


                    rtype = "InstOut:" + Mytype.Name;

                    ctor1IL.Emit(OpCodes.Ldstr, rtype);
                    ctor1IL.Emit(OpCodes.Ldc_I4, 0);
                    ctor1IL.Emit(OpCodes.Ldc_I4, oindex);
                    ctor1IL.Emit(OpCodes.Ldstr, Mytype.AssemblyQualifiedName);

                    ctor1IL.Emit(OpCodes.Call, addoutputinfo);
                }



                //description to show up in menu
                FieldInfo desf = tb.BaseType.GetField("description", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                ctor1IL.Emit(OpCodes.Ldarg_0);
                ctor1IL.Emit(OpCodes.Ldstr, newname + parlist + ")");
                ctor1IL.Emit(OpCodes.Stfld, desf);
                ctor1IL.Emit(OpCodes.Ret);
                ctor1IL.Emit(OpCodes.Ret);



                ////override process usercode



                Type t = tb.CreateType();
                return t;
            }
            catch (Exception exy)
            {
                if (PUPPIDebugger.debugenabled)
                {
                    PUPPIDebugger.log(utils.StringConstants.failedtomakemethodmodule + " " + methinfo.Name);
                }
                return null;
            }
        }
        //my own hash code immitation, not unique but should be enough for this
        private static int hac(string s)
        {
            int i = 0;
            foreach (char c in s)
            {
                i += c;
            }
            return i;
        }
        internal static Type makePUPPIModuleFromEnum(Type enumType, int ecnt, AssemblyBuilder myAssemblyBuilder, ModuleBuilder fileModuleBuilder = null)
        {
            if (!enumType.IsEnum) return null;

            //gonna use a try-catch
            try
            {
                int isclass = 1;
                string classtypename = "";
                try
                {
                    Type Mytype = enumType.DeclaringType;
                    classtypename = Mytype.Name;
                }
                catch
                {
                    classtypename = "";
                }


                ModuleBuilder myModuleBuilder;
                TypeBuilder tb;
                if (fileModuleBuilder == null)
                {
                    myModuleBuilder = myAssemblyBuilder.DefineDynamicModule(classtypename + "Module" + enumType.Name + ecnt.ToString());

                }
                else
                {
                    myModuleBuilder = fileModuleBuilder;


                }
                string typesignature = "PUPPIAutoModules.Enum." + enumType.FullName.Replace("__", "").Replace("+", ".") + "__" + ecnt.ToString();
                tb = myModuleBuilder.DefineType(typesignature
                               , TypeAttributes.Public |
                               TypeAttributes.Class

                               , new PUPPIModule().GetType());

                //override constructor
                ConstructorBuilder ctor1 = tb.DefineConstructor(
      MethodAttributes.Public,
      CallingConventions.Standard, null);

                ILGenerator ctor1IL = ctor1.GetILGenerator();
                //     Type[] tttt=new Type[1];
                //    tttt[0]=typeof(string);

                //ctor1IL.EmitCall(OpCodes.Call, typeof(System.Windows.Forms.MessageBox).GetMethod("Show",  tttt ),tttt);
                //ctor1IL.Emit(OpCodes.Pop);
                // remove the return value of Show from the stack
                //ctor1IL.Emit(OpCodes.Ret);
                ctor1IL.Emit(OpCodes.Ldarg_0);
                //get base constructor
                var bc = tb.BaseType.GetConstructor(System.Type.EmptyTypes);
                ctor1IL.Emit(OpCodes.Call, bc);

                ctor1IL.Emit(OpCodes.Ldarg_0);
                FieldInfo f4f = tb.BaseType.GetField("moduleisenumeration", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                ctor1IL.Emit(OpCodes.Ldc_I4, 1);
                ctor1IL.Emit(OpCodes.Stfld, f4f);


                ctor1IL.Emit(OpCodes.Ldarg_0);
                //name
                //get an existing field
                FieldInfo ff = tb.BaseType.GetField("name", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                ctor1IL.Emit(OpCodes.Ldstr, typesignature);
                ctor1IL.Emit(OpCodes.Stfld, ff);

                FieldInfo[] infos;

                infos = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
                System.Array enumValues = System.Enum.GetValues(enumType);


                //get method input parameters and return
                //parameters with out will become outputs
                //store indices so we can retrieve at constructor override    
                int oindex = 0;

                foreach (FieldInfo fi in infos)
                {



                    MethodInfo addoutputinfo = tb.BaseType.GetMethod("addoutput", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                    ctor1IL.Emit(OpCodes.Ldarg_0);
                    ctor1IL.Emit(OpCodes.Ldstr, fi.Name);
                    int fg = Convert.ToInt32(enumValues.GetValue(oindex));
                    ctor1IL.Emit(OpCodes.Ldc_I4, 0);


                    //now the output index
                    ctor1IL.Emit(OpCodes.Ldc_I4, oindex);
                    ctor1IL.Emit(OpCodes.Ldstr, enumType.AssemblyQualifiedName);
                    ctor1IL.Emit(OpCodes.Call, addoutputinfo);
                    oindex++;



                }

                //description to show up in menu
                FieldInfo desf = tb.BaseType.GetField("description", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                ctor1IL.Emit(OpCodes.Ldarg_0);
                ctor1IL.Emit(OpCodes.Ldstr, typesignature);
                ctor1IL.Emit(OpCodes.Stfld, desf);
                ctor1IL.Emit(OpCodes.Ret);



                return tb.CreateType();

            }
            catch (Exception exy)
            {
                return null;
            }
        }


        internal static Type makePUPPIModuleFromConstructor(ConstructorInfo cinfo, int ccnt, AssemblyBuilder myAssemblyBuilder, ModuleBuilder fileModuleBuilder = null)
        {

            //gonna use a try-catch
            try
            {
                int isclass = 1;
                Type Mytype = cinfo.DeclaringType;
                if (Mytype.ContainsGenericParameters) return null;
                string classtypename = Mytype.Name;
                ParameterInfo[] Myarray = cinfo.GetParameters();
                ModuleBuilder myModuleBuilder;
                TypeBuilder tb;
                if (fileModuleBuilder == null)
                {
                    myModuleBuilder = myAssemblyBuilder.DefineDynamicModule(Mytype.Name + "Module" + cinfo.Name + ccnt.ToString());
                    //     tb = myModuleBuilder.DefineType("PUPPI" + Mytype.Name + ccnt.ToString() + "_" + Myarray.Count().ToString()
                    //                   , TypeAttributes.Public |
                    //                 TypeAttributes.Class
                    /*|TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout*/
                    //               , new PUPPIModule().GetType());
                }
                else
                {
                    myModuleBuilder = fileModuleBuilder;

                }

                string typeSignature = "PUPPIAutoModules.Constructor." + Mytype.FullName.Replace("__", "").Replace("+", "") + "__" + ccnt.ToString() + "__" + Myarray.Count().ToString();
                tb = myModuleBuilder.DefineType(typeSignature, TypeAttributes.Public | TypeAttributes.Class, new PUPPIModule().GetType());


                //override constructor
                ConstructorBuilder ctor1 = tb.DefineConstructor(
      MethodAttributes.Public,
      CallingConventions.Standard, null);

                ILGenerator ctor1IL = ctor1.GetILGenerator();

                //if (fileModuleBuilder != null)
                //{
                //    MethodInfo la = tb.BaseType.GetMethod("selfLoad", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                //    ctor1IL.Emit(OpCodes.Ldarg_0);
                //    ctor1IL.Emit(OpCodes.Call, la);
                //}

                ctor1IL.Emit(OpCodes.Ldarg_0);
                //get base constructor
                var bc = tb.BaseType.GetConstructor(System.Type.EmptyTypes);
                ctor1IL.Emit(OpCodes.Call, bc);
                ctor1IL.Emit(OpCodes.Ldarg_0);
                //name
                //get an existing field
                FieldInfo ff = tb.BaseType.GetField("name", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                ctor1IL.Emit(OpCodes.Ldstr, typeSignature);
                ctor1IL.Emit(OpCodes.Stfld, ff);



                //flag foe constructor or method
                //get an existing field
                ctor1IL.Emit(OpCodes.Ldarg_0);
                FieldInfo f4f = tb.BaseType.GetField("isconstructor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                ctor1IL.Emit(OpCodes.Ldc_I4, 1);
                ctor1IL.Emit(OpCodes.Stfld, f4f);

                int ipc = 0;
                foreach (ParameterInfo pinfo in Myarray)
                {

                    if (pinfo.IsOut == false)
                    {
                        ipc++;
                    }
                }
                if (ipc == 0)
                {
                    ctor1IL.Emit(OpCodes.Ldarg_0);
                    FieldInfo f6f = tb.BaseType.GetField("isparameterlessconstructor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                    ctor1IL.Emit(OpCodes.Ldc_I4, 1);
                    ctor1IL.Emit(OpCodes.Stfld, f6f);
                }

                //return rthe class
                MethodInfo addoutputinfo = tb.BaseType.GetMethod("addoutput", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                //for simple types
                ctor1IL.Emit(OpCodes.Ldarg_0);
                ctor1IL.Emit(OpCodes.Ldstr, Mytype.Name);
                //if (methinfo.ReturnType==typeof(int))
                //{
                ctor1IL.Emit(OpCodes.Ldc_I4, 0);
                //ctor1IL.EmitCall(OpCodes.Call,addoutputinfo, new Type[] { typeof(string), typeof(int) });
                ctor1IL.Emit(OpCodes.Ldc_I4, 0);
                //now the variable type
                ctor1IL.Emit(OpCodes.Ldstr, Mytype.AssemblyQualifiedName);
                ctor1IL.Emit(OpCodes.Call, addoutputinfo);




                //get method input parameters and return
                //parameters with out will become outputs
                //store indices so we can retrieve at constructor override    
                string parlist = "(";
                int iindex = 0;
                int oindex = 1;
                foreach (ParameterInfo pinfo in Myarray)
                {
                    string useName = "";
                    if (pinfo.Name == null) useName = pinfo.ParameterType.Name; else useName = pinfo.Name;
                    if (pinfo.IsOut == true)
                    {

                        addoutputinfo = tb.BaseType.GetMethod("addoutput", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                        //for simple types
                        ctor1IL.Emit(OpCodes.Ldarg_0);
                        ctor1IL.Emit(OpCodes.Ldstr, useName);
                        //if (methinfo.ReturnType==typeof(int))
                        //{
                        ctor1IL.Emit(OpCodes.Ldc_I4, 0);
                        //ctor1IL.EmitCall(OpCodes.Call,addoutputinfo, new Type[] { typeof(string), typeof(int) });
                        //now the output index
                        ctor1IL.Emit(OpCodes.Ldc_I4, oindex);
                        ctor1IL.Emit(OpCodes.Ldstr, pinfo.ParameterType.AssemblyQualifiedName);
                        ctor1IL.Emit(OpCodes.Call, addoutputinfo);
                        oindex++;
                        parlist += "out " + pinfo.ParameterType.Name + " " + useName + ",";
                    }
                    else
                    {


                        MethodInfo addinputinfo = tb.BaseType.GetMethod("addinput", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                        ctor1IL.Emit(OpCodes.Ldarg_0);
                        ctor1IL.Emit(OpCodes.Ldstr, useName);
                        ctor1IL.Emit(OpCodes.Ldc_I4, iindex);
                        ctor1IL.Emit(OpCodes.Ldstr, pinfo.ParameterType.AssemblyQualifiedName);
                        ctor1IL.Emit(OpCodes.Ldc_I4, isclass);
                        ctor1IL.Emit(OpCodes.Call, addinputinfo);
                        iindex++;
                        parlist += pinfo.ParameterType.Name + " " + useName + ",";
                    }

                }
                parlist += ")";
                //description to show up in menu
                FieldInfo desf = tb.BaseType.GetField("description", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                ctor1IL.Emit(OpCodes.Ldarg_0);
                ctor1IL.Emit(OpCodes.Ldstr, typeSignature + parlist);
                ctor1IL.Emit(OpCodes.Stfld, desf);
                ctor1IL.Emit(OpCodes.Ret);



                return tb.CreateType();


            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Makes a class constructor into a PUPPI Module.
        /// </summary>
        /// <param name="Mytype">A type (use command .GetType() on a class instance or typeof on a static class)</param>
        /// <param name="parametertypes">List of parameter types for the constructor. Can be null for parameterless constructors</param>
        /// <returns></returns>
        public static Type makeConstructorIntoPUPPIModuleType(Type Mytype, List<Type> parametertypes)
        {

            int isclass = 1;
            string classtypename = Mytype.Name;
            string nametrail = "";
            if (parametertypes == null || parametertypes.Count == 0)
            {
                nametrail = "noparams";
            }
            else
            {
                foreach (Type tt in parametertypes)
                {
                    nametrail += tt.Name;
                }
            }
            var an = new AssemblyName("PUPPI" + classtypename + "as" + nametrail);
            AssemblyBuilder mb = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);

            //now get the methods with this name
            ConstructorInfo[] fbn = Mytype.GetConstructors();

            var bci = fbn;//.OrderBy(aci => aci.Name).ThenBy(aci => String.Join(",", aci.GetParameters().Select(x => x.Name + x.ParameterType.Name).ToArray()).Length);

            ConstructorInfo fc = null;
            if (fbn != null)
            {


                for (int i = 0; i < fbn.Count(); i++)
                {
                    ConstructorInfo myc = fbn[i];
                    bool mfound = true;
                    int gp = 0;
                    if (myc.GetParameters().Length == 0 && (parametertypes != null && parametertypes.Count > 0))
                    { mfound = false; continue; }
                    else if (myc.GetParameters().Length == 0 && (parametertypes == null || parametertypes.Count == 0))
                    //parameterless    
                    {
                        mfound = true; fc = fbn[i]; break;
                    }
                    foreach (ParameterInfo pi in myc.GetParameters())
                    {
                        if (parametertypes == null || parametertypes.Count == 0)
                        {
                            mfound = false;
                            break;
                        }
                        if (pi.ParameterType != parametertypes[gp])
                        {
                            mfound = false;
                            break;
                        }
                        gp++;
                    }

                    if (mfound == true)
                    {
                        fc = fbn[i];
                        break;
                    }
                }
            }

            if (fc == null)
                return null;
            ParameterInfo[] Myarray = fc.GetParameters();
            string hame = "c";
            foreach (ParameterInfo pinfo in Myarray)
            {
                hame += pinfo.Name;
                hame += pinfo.ParameterType.Name;

            }
            //int ccnt = bci.ToList<ConstructorInfo>().IndexOf(fc);
            return makePUPPIModuleFromConstructor(fc, hac(hame), mb);

        }
    }
    //helper methods
    internal static class ModuleHelperFunctions
    {
        internal static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }
    }
    namespace PUPPIPremadeModules
    {
        //container for multiple nodes

        public class PUPPINodeContainer : PUPPIModule
        {
            //if container
            internal List<PUPPIModule> PMsubModules;
            //need to know where to send the inputs
            // inpindex_smindex_sminpindex
            internal List<string> PMsmInMap;
            //need to retrieve outputs from contained modules
            //outindex_omindex_omoutindex
            internal List<string> PMsmOutMap;
            //keep list of old input values to see what has changed and run process from there, to not slow it down too much
            //internal ArrayList prev_inputs;


            public PUPPINodeContainer()
                : base()
            {
                name = "Container";
                description = "Holds multiple nodes";
                // completeProcessOverride = true;
                PMsubModules = new List<PUPPIModule>();
                PMsmInMap = new List<string>();
                PMsmOutMap = new List<string>();
                //prev_inputs = new ArrayList();
                completeDoubleClickOverride = true;
            }
            public PUPPINodeContainer(List<PUPPIModule> containedModules)
                : base()
            {
                PMsubModules = new List<PUPPIModule>();
                PMsmInMap = new List<string>();
                PMsmOutMap = new List<string>();
                name = "Container";
                description = "Holds multiple nodes";
                //set inputs
                //prev_inputs = new ArrayList();
                completeDoubleClickOverride = true;
                initializeFromModules(containedModules);

            }

            public void initializeFromModules(List<PUPPIModule> containedNodes)
            {


                //inputs not satisfied
                List<PUPPIModule> usp = new List<PUPPIModule>();
                List<int> usii = new List<int>();
                //outputs not satisfied
                List<PUPPIModule> osp = new List<PUPPIModule>();
                List<int> osii = new List<int>();
                bool makeGUIDs = false;
                foreach (PUPPIModule p in containedNodes)
                {
                    if (p.GUID == -1)
                    {
                        makeGUIDs = true;
                        break;
                    }
                }
                int pin = 1000;
                foreach (PUPPIModule p in containedNodes)
                {
                    if (makeGUIDs)
                    {
                        p.GUID = pin;
                    }

                    pin++;
                    PMsubModules.Add(p);
                    int ii = 0;
                    foreach (PUPPIInParameter pi in p.inputs)
                    {
                        bool sf = false;
                        if (pi != null)
                        {
                            //check inputs contained within node
                            foreach (PUPPIModule pp in containedNodes)
                            {
                                if (p != pp)
                                {
                                    if (pi.module == pp)
                                    {
                                        sf = true;
                                        break;
                                    }
                                }
                            }
                            if (sf == false)
                            {
                                //input not satisfied from inside
                                usp.Add(p);
                                usii.Add(ii);
                            }
                        }
                        ii++;
                    }


                    for (int io = 0; io < p.outputs.Count; io++)
                    {
                        bool sf = false;
                        //check inputs contained within node
                        foreach (PUPPIModule pp in containedNodes)
                        {
                            if (p != pp)
                            {
                                foreach (PUPPIInParameter ppi in pp.inputs)
                                {
                                    if (ppi.module == p && ppi.outParIndex == io)
                                    {
                                        sf = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (sf == false)
                        {
                            //output not satisfied from inside
                            osp.Add(p);
                            osii.Add(io);
                        }
                    }


                }
                //create connections
                // inpindex_smindex_sminpindex
                int ij = 0;
                foreach (PUPPIModule up in usp)
                {
                    PUPPIInParameter ip = new PUPPIInParameter();
                    ip.isoptional = up.inputs[usii[ij]].isoptional;

                    ip.inputAutomaticListMode = up.inputs[usii[ij]].inputAutomaticListMode;
                    inputs.Add(ip);
                    inputnames.Add(up.cleancap + ":" + up.inputnames[usii[ij]]);

                    PMsmInMap.Add(ij.ToString() + "_" + PMsubModules.IndexOf(up).ToString() + "_" + usii[ij].ToString());

                    ij++;

                }
                //need to process once before setting outputs
                //regardless of program live run settings
                foreach (PUPPIModule pp in PMsubModules)
                {
                    if (pp.inputs.Count == 0)
                    {
                        pp.process();
                    }
                }


                //create connections
                ////outindex_omindex_omoutindex
                //add default outputs
                ij = 0;
                foreach (PUPPIModule up in osp)
                {
                    outputs.Add(up.outputs[osii[ij]]);
                    outputnames.Add(up.cleancap + ":" + up.outputnames[osii[ij]]);
                    PMsmOutMap.Add(ij.ToString() + "_" + PMsubModules.IndexOf(up).ToString() + "_" + osii[ij].ToString());
                    ij++;

                }
                //reset number calls for modules inside
                foreach (PUPPIModule pp in PMsubModules)
                {
                    pp.resetModuleNumberCalls();
                }

            }


            public override void process_usercode()
            {

                foreach (PUPPIModule pp in PMsubModules)
                {
                    pp.resetModuleNumberCalls();
                }
                //process as container
                if (PMsubModules.Count > 0)
                {

                    //keep the temp modules here so that we can execute all of them at the end when connections are made
                    List<PUPPIModule> fI = new List<PUPPIModule>();

                    //get the inputs

                    //for (int oi = 0; oi < inputs.Count; oi++)
                    //{
                    //  bool os = false;

                    //bool bl = PUPPIruntimesettings.processyesno;
                    //PUPPIruntimesettings.processyesno = false;

                    foreach (string ss in PMsmInMap)
                    {

                        char[] ser = { '_' };
                        string[] s = ss.Split(ser);
                        try
                        {
                            int oi = Convert.ToInt16(s[0]);

                            PUPPIModule pi = PMsubModules[Convert.ToInt16(s[1])];
                            int inputPIIndex = Convert.ToInt32(s[2]);
                            //connect the input to a temporary module
                            //this is so we can handle optionals too without messing up
                            if (usercodeinputs[oi] != null)
                            {
                                PUPPIModel.PUPPIPremadeModules.OutputModule sI = new PUPPIPremadeModules.OutputModule(usercodeinputs[oi]);
                                PUPPIInParameter zI = new PUPPIInParameter();
                                zI.getinputfrom(sI, 0);
                                pi.connect_input(zI, inputPIIndex, false, false);

                                fI.Add(sI);
                            }


                            //os = true;
                        }
                        catch
                        {

                        }
                        //      if (os) break;

                    }



                    //PUPPIruntimesettings.processyesno = bl;
                    //}
                    //regen upstreamleft
                    //foreach (PUPPIModule tu in PMsubModules )
                    //{
                    //    tu.upstreamleft = tu.mandinputs();
                    //}


                    if (inputs.Count > 0)
                    {
                        //if (prev_inputs.Count != usercodeinputs.Count)
                        //{
                        // Parallel.ForEach(fI, (sI) =>
                        //{
                        //    sI.doIprocess();
                        //});

                        foreach (PUPPIModule sI in fI)
                        {
                            sI.resetUSLDList(PMsubModules);
                            sI.increaseUSLDList(new List<PUPPIInParameter>(), PMsubModules);
                        }

                        foreach (PUPPIModule sI in fI)
                            sI.doIprocess();
                        //}
                        // else
                        // {
                        //make sure only changed process
                        // for (int i = 0; i < usercodeinputs.Count; i++)
                        //  {
                        // if (usercodeinputs[i] != prev_inputs[i] )
                        // {
                        //     fI[i].doIprocess();
                        // }
                        // }
                        //}
                    }
                    else
                    {
                        //process all nodes with no inputs


                        foreach (PUPPIModule p in PMsubModules)
                        {
                            if (p.inputs.Count == 0)
                            {
                                p.resetUSLDList(PMsubModules);
                                p.increaseUSLDList(new List<PUPPIInParameter>(), PMsubModules);
                            }
                        }
                        foreach (PUPPIModule p in PMsubModules)
                        {
                            if (p.inputs.Count == 0)
                            {
                                p.doIprocess();
                            }
                        }

                        //foreach (PUPPIModule p in PMsubModules)
                        //{
                        //    if (p.inputs.Count == 0)
                        //    {
                        //        p.doIprocess();
                        //    }
                        //}
                    }
                    //get the outputs

                    foreach (string ss in PMsmOutMap)
                    {

                        char[] ser = { '_' };
                        string[] s = ss.Split(ser);
                        try
                        {
                            int oi = Convert.ToInt16(s[0]);

                            PUPPIModule pi = PMsubModules[Convert.ToInt16(s[1])];
                            //too expensive
                            //outputs[oi] = DeepCopy(pi.outputs[Convert.ToInt32(s[2])] as object, 0);
                            usercodeoutputs[oi] = pi.outputs[Convert.ToInt32(s[2])];

                        }
                        catch
                        {

                        }


                    }

                    //finally, disconnect unsatisfied inputs
                    // bl = PUPPIruntimesettings.processyesno;
                    //PUPPIruntimesettings.processyesno = false;
                    // bool os = false;
                    foreach (string ss in PMsmInMap)
                    {
                        // for (int oi = 0; oi < inputs.Count; oi++)
                        //{
                        char[] ser = { '_' };
                        string[] s = ss.Split(ser);
                        try
                        {
                            int oi = Convert.ToInt16(s[0]);

                            PUPPIModule pi = PMsubModules[Convert.ToInt16(s[1])];
                            int inputPIIndex = Convert.ToInt32(s[2]);
                            //no canvas check
                            pi.disconnect_input(inputPIIndex, false);

                            //os = true;
                            // break;

                            //if (os) break;
                        }
                        catch
                        {

                        }
                        //}
                    }
                    //PUPPIruntimesettings.processyesno = bl;
                    //reset prev_inputs
                    //   prev_inputs.Clear();
                    //if (usercodeinputs!=null )
                    //prev_inputs.AddRange(usercodeinputs);


                }
            }


            public void getInternalConnections(out List<string> mappings)
            {
                //sourceIndex_sourceOutput_destIndex_destInput 
                mappings = new List<string>();
                int i = 0;
                foreach (PUPPIModule p in PMsubModules)
                {
                    PMsubModules.Add(p);
                    int ii = 0;
                    foreach (PUPPIInParameter pi in p.inputs)
                    {
                        bool sf = false;
                        if (pi != null)
                        {
                            int iii = 0;
                            //check inputs contained within node
                            foreach (PUPPIModule pp in PMsubModules)
                            {
                                if (p != pp)
                                {
                                    if (pi.module == pp)
                                    {
                                        sf = true;
                                        break;
                                    }
                                }
                                iii++;
                            }
                            if (sf == true)
                            {
                                //input satisfied from inside
                                string sl = iii.ToString() + "_" + pi.outParIndex.ToString() + "_" + i.ToString() + "_" + ii.ToString();
                            }
                        }
                        ii++;
                    }

                    i++;



                }


            }

            public override string saveSettings()
            {
                string cs = "";
                Dictionary<int, PUPPIModule> pd = new Dictionary<int, PUPPIModule>();
                foreach (PUPPIModule p in PMsubModules)
                {
                    pd.Add(p.GUID, p);
                }
                cs = PUPPIAbstraction.PUPPIProgramModel.writeToXML(pd);
                //XmlDocument doc = new XmlDocument();
                //XmlNode containerelem = doc.CreateNode(XmlNodeType.Element, "container", "");
                //foreach (PUPPIModule sp in PMsubModules)
                //{
                //    XmlNode seleme = doc.CreateNode(XmlNodeType.Element, "submodule", "");
                //    XmlNode neleme = doc.CreateNode(XmlNodeType.Element, "PUPPImodule", "");
                //    neleme.InnerText = sp.GetType().ToString();  
                //    seleme.AppendChild(neleme);
                //    //saved settings, this will get recursive
                //    XmlNode teleme = doc.CreateNode(XmlNodeType.Element, "savedSettings", "");
                //    teleme.InnerText = sp.saveSettings();
                //    seleme.AppendChild(teleme);
                //    containerelem.AppendChild(seleme);
                //}
                ////mappings
                //foreach (string s in PMsmInMap)
                //{
                //    XmlNode seleme = doc.CreateNode(XmlNodeType.Element, "imapping", "");
                //    seleme.InnerText = s;
                //    containerelem.AppendChild(seleme);
                //}
                //foreach (string s in PMsmOutMap)
                //{
                //    XmlNode seleme = doc.CreateNode(XmlNodeType.Element, "omapping", "");
                //    seleme.InnerText = s;
                //    containerelem.AppendChild(seleme);
                //}
                //doc.AppendChild(containerelem); 
                //cs = doc.OuterXml;
                return cs;
            }


            public override void initOnLoad(string savedSettings)
            {
                PMsubModules = new List<PUPPIModule>();
                PMsmInMap = new List<string>();
                PMsmOutMap = new List<string>();
                Dictionary<int, PUPPIModule> savedNodes = PUPPIAbstraction.PUPPIProgramModel.readFromXML(savedSettings);
                List<PUPPIModule> pl = savedNodes.Values.ToList<PUPPIModule>();
                initializeFromModules(pl);
                //XmlDocument doc = new XmlDocument();
                //try
                //{

                //    doc.LoadXml(savedSettings);
                //    XmlNodeList nodes = doc.DocumentElement.SelectNodes("/container/submodule");

                //    foreach (XmlNode mynode in nodes)
                //    {

                //        XmlNode childnode = mynode["PUPPImodule"];


                //        Type t = PUPPIModel.AutomaticPUPPImodulesCreator.findTypeByName(childnode.InnerText);
                //        if (t == null) throw new Exception(" Failed to find container submodule type " + childnode.InnerText + " in loaded assemblies");
                //        PUPPIModule pm = PUPPIModel.AutomaticPUPPImodulesCreator.instantiatePUPPIModule(t) as PUPPIModule;
                //        childnode = mynode["savedSettings"];
                //        pm.initOnLoad(childnode.InnerText);
                //        PMsubModules.Add(pm);  

                //    }

                //    nodes = doc.DocumentElement.SelectNodes("/container/imapping");

                //    foreach (XmlNode mynode in nodes)
                //    {
                //        PMsmInMap.Add(mynode.InnerText);
                //    }
                //    nodes = doc.DocumentElement.SelectNodes("/container/omapping");

                //    foreach (XmlNode mynode in nodes)
                //    {
                //        PMsmOutMap.Add(mynode.InnerText);
                //    }

                //}
                //catch (Exception exy)
                //{
                //    MessageBox.Show("Failed to load settings on container: " + exy.ToString());

                //}
            }

            public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
            {
                //show submodules
                ListForm ff = new ListForm();
                ff.setTitle("Modules in Container");

                foreach (PUPPIModule p in PMsubModules)
                {
                    ff.addListItem(p.ToString());
                }
                ff.ShowDialog();
            }


        }

        //simple data storage
        public class OutputModule : PUPPIModule
        {
            public OutputModule()
                : base()
            {
                outputs.Add(0);
                outputnames.Add("Output");
            }
            public OutputModule(object setOutput)
                : base()
            {
                outputs.Add(setOutput);
                outputnames.Add("output");
            }
        }

        //where we keep data input modules
        namespace DataInputModules
        {
            /// <summary>
            /// The <see cref="DataInputModules"/> namespace contains premade PUPPI modules that allows the user to feed data into the visual program
            /// </summary>

            [System.Runtime.CompilerServices.CompilerGenerated]
            class NamespaceDoc
            {
            }
            /// <summary>
            /// Single number input when double clicking. 
            /// </summary>
            public class PUPPINumber : PUPPIModule
            {
                public PUPPINumber(double number)
                    : base()
                {
                    name = "Number input";//=" + number.ToString() ;
                    outputs.Add(number);
                    outputnames.Add("Value");
                    doubleClickDescription = "Enter number(s)";
                }
                public PUPPINumber()
                    : base()
                {
                    double number = 0.0;
                    name = "Number input";//=" + number.ToString();
                    outputs.Add(number);
                    outputnames.Add("Value");
                    doubleClickDescription = "Enter number(s)";
                }
                /// <summary>
                /// Override action when user double clicks on node. Typically for data input.
                /// </summary>
                public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
                {

                    double number = 0;
                    string entered = "";
                    // double 
                    entered = "";

                    if (countListMode == 0)
                    {
                        if (formutils.InputBox("Decimal entry ", "Please enter a number!", ref entered) == System.Windows.Forms.DialogResult.OK)
                        {

                            try
                            {
                                number = Convert.ToDouble(entered);

                            }
                            catch
                            {
                                number = 0;
                            }
                        }
                        else
                        {
                            try
                            {
                                number = Convert.ToDouble(dblclkpreviousoutputs[0]);

                            }
                            catch
                            {
                                number = 0;
                            }
                        }
                    }
                    else
                    {
                        if (formutils.InputBox("Decimal entry " + (inlist + 1).ToString() + "/" + countListMode.ToString(), "Please enter a number!", ref entered) == System.Windows.Forms.DialogResult.OK)
                        {
                            try
                            {
                                number = Convert.ToDouble(entered);
                            }
                            catch
                            {
                                number = 0;
                            }
                        }
                        else
                        {
                            try
                            {
                                ArrayList pvo = dblclkpreviousoutputs[0] as ArrayList;
                                number = Convert.ToDouble(pvo[inlist]);
                            }
                            catch
                            {
                                number = 0;
                            }
                        }
                    }
                    if ((number % 1) == 0)
                    {
                        usercodeoutputs[0] = Convert.ToInt32(number);
                    }
                    else
                    {
                        usercodeoutputs[0] = number;
                    }
                    name = "Number input";//=" + number.ToString();




                }
                public override void process_usercode()
                {
                    if (inputs.Count != 0)
                    {
                        double number = 0;
                        PUPPIInParameter pim = inputs[0];
                        if (pim.module.outputs[pim.outParIndex] is double || pim.module.outputs[pim.outParIndex] is int)
                        {
                            number = (double)pim.module.outputs[pim.outParIndex];
                        }
                        outputs[0] = number;
                        name = "Number input";//=" + number.ToString();
                    }
                }

            }
            /// <summary>
            /// Single random number generated. Updates when double clicking.
            /// </summary>
            public class PUPPIRandNumber : PUPPIModule
            {
                Random seed;
                public PUPPIRandNumber()
                    : base()
                {
                    int inMyRandSeed = System.Environment.TickCount;
                    seed = new Random(inMyRandSeed);

                    double number = seed.NextDouble();
                    name = "Rand Number";
                    description = "Generates a random number between 0-1.Double click to get new random number";
                    outputs.Add(number);
                    outputnames.Add("Value");
                    doubleClickDescription = "Generate new random number";
                }
                //if no inputs, set by user
                public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
                {

                    double number = 0;



                    number = seed.NextDouble();

                    usercodeoutputs[0] = number;






                }


            }
            /// <summary>
            /// A list of random numbers with an input for the number of items
            /// </summary>
            public class PUPPIRandNumberList : PUPPIModule
            {
                Random seed;
                public PUPPIRandNumberList()
                    : base()
                {
                    completeProcessOverride = true;

                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("List count");
                    name = "Rand List";
                    description = "Automatically generates a list of random numbers based on input";
                    outputs.Add(new ArrayList());
                    outputnames.Add("Random List");
                }

                public override void process_usercode()
                {
                    try
                    {
                        int inMyRandSeed = System.Environment.TickCount;
                        seed = new Random(inMyRandSeed);
                        outputs[0] = new ArrayList();
                        int nc = Convert.ToInt32(inputs[0].module.outputs[inputs[0].outParIndex]);
                        if (nc > 0)
                        {
                            for (int i = 0; i < nc; i++)
                            {
                                (outputs[0] as ArrayList).Add(seed.NextDouble());
                            }
                        }

                    }
                    catch
                    {
                        outputs[0] = "error";
                    }
                }



            }

            /// <summary>
            /// Automatically generates a list of strings using optional prefix,0 based  numbers and optional suffix
            /// </summary>
            public class PUPPIStringList : PUPPIModule
            {
                Random seed;
                public PUPPIStringList()
                    : base()
                {
                    completeProcessOverride = true;


                    PUPPIInParameter pref = new PUPPIInParameter();
                    pref.isoptional = true;
                    inputs.Add(pref);
                    inputnames.Add("Prefix (opt)");

                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("List count");

                    PUPPIInParameter suf = new PUPPIInParameter();
                    suf.isoptional = true;
                    inputs.Add(suf);
                    inputnames.Add("Suffix (opt)");

                    name = "String List";
                    description = "Automatically generates a list of strings using optional prefix,0 based numbers and optional suffix";
                    outputs.Add(new ArrayList());
                    outputnames.Add("String List");
                }

                public override void process_usercode()
                {
                    try
                    {
                        ArrayList nc = new ArrayList();
                        int number = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);
                        if (number <= 0)
                        {
                            outputs[0] = "invalid count";
                            return;
                        }
                        string prefix = "";
                        if (inputs[0].module != null)
                        {
                            prefix = inputs[0].module.outputs[inputs[0].outParIndex].ToString();
                        }
                        string suffix = "";
                        if (inputs[2].module != null)
                        {
                            suffix = inputs[2].module.outputs[inputs[2].outParIndex].ToString();
                        }
                        for (int i = 0; i < number; i++)
                        {
                            nc.Add(prefix + i.ToString() + suffix);
                        }
                        outputs[0] = nc;

                    }
                    catch
                    {
                        outputs[0] = "error";
                    }
                }



            }

            /// <summary>
            /// A table input with customizable rows and columns. Returns an Arraylist of arraylists.
            /// </summary>
            public class PUPPIGridInput : PUPPIModule
            {

                public PUPPIGridInput()
                    : base()
                {
                    completeProcessOverride = true;
                    completeDoubleClickOverride = true;
                    name = "Grid input";//=" + number.ToString();
                    outputs.Add(new ArrayList());
                    outputnames.Add("Value");
                    doubleClickDescription = "Enter grid data";
                }
                //if no inputs, set by user
                public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
                {
                    ArrayList aa = new ArrayList();
                    try
                    {
                        aa = outputs[0] as ArrayList;
                    }
                    catch
                    {
                        aa = new ArrayList();
                    }
                    if (aa == null) aa = new ArrayList();
                    PUPPI.griddataentry pg = new PUPPI.griddataentry(aa);
                    pg.ShowDialog();
                    if (pg.doupdate == true)
                    {
                        outputs[0] = pg.griddata;

                    }

                }
                public override void process_usercode()
                {

                }

            }

            /// <summary>
            /// Creates a grid from a CSV file.Can be viewed wih double click.
            /// </summary>
            public class PUPPIGridFromCSV : PUPPIModule
            {

                public PUPPIGridFromCSV()
                    : base()
                {
                    completeProcessOverride = true;
                    completeDoubleClickOverride = true;
                    name = "Grid From CSV";//=" + number.ToString();
                    outputs.Add(new ArrayList());
                    outputnames.Add("Grid");
                    //file path
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("File Path");
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Separator String");

                }
                public override void process_usercode()
                {
                    try
                    {
                        ArrayList a = new ArrayList();
                        var reader = new StreamReader(inputs[0].module.outputs[inputs[0].outParIndex].ToString());
                        string sep = inputs[1].module.outputs[inputs[1].outParIndex].ToString();
                        string[] sp = new string[1];
                        sp[0] = sep;

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            string[] values = line.Split(sp, StringSplitOptions.None);
                            ArrayList ba = new ArrayList();
                            for (int i = 0; i < values.Length; i++)
                            {
                                //ba.Add(Convert.ToDouble(values[i])); 
                                ba.Add(values[i]);
                            }
                            a.Add(ba);
                        }
                        outputs[0] = a;
                    }
                    catch (Exception exy)
                    {

                        outputs[0] = "error";
                    }


                }

                private void DisableControls(Control con)
                {
                    foreach (Control c in con.Controls)
                    {
                        if (c.Text != "Cancel")
                            DisableControls(c);
                    }
                    if (con.Text != "Cancel" && con.Name != "tableLayoutPanel1")
                        con.Enabled = false;
                }


                public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
                {
                    ArrayList aa = new ArrayList();
                    try
                    {
                        aa = outputs[0] as ArrayList;
                    }
                    catch
                    {
                        aa = new ArrayList();
                    }
                    if (aa == null) aa = new ArrayList();
                    PUPPI.griddataentry pg = new PUPPI.griddataentry(aa);
                    foreach (Control co in pg.Controls)
                    {

                        DisableControls(co);
                    }
                    pg.ShowDialog();


                }


            }


            /// <summary>
            /// Base with no inputs or outputs, just to group modules.
            /// </summary>
            public class PUPPIBlank : PUPPIModule
            {
                public PUPPIBlank()
                    : base()
                {
                    completeProcessOverride = true;
                    name = "Blank Base";
                }
            }
            /// <summary>
            /// Outputs are the same as inputs for easier identification in more complex programs.
            /// </summary>
            public class PUPPIPassThrough : PUPPIModule
            {
                object stuff;
                public PUPPIPassThrough()
                    : base()
                {

                    name = "Pass/Store";
                    outputs.Add("empty");
                    description = "Outputs the input. When input disconnected, output remains, so it can be used as variable storage";
                    outputnames.Add("The Input");
                    inputnames.Add("Input");
                    completeProcessOverride = true;
                    PUPPIInParameter pi = new PUPPIInParameter();
                    pi.isoptional = true;
                    inputs.Add(pi);
                    stuff = null;

                }
                public override void process_usercode()
                {
                    try
                    {
                        if (inputs[0].module != null)
                        {
                            stuff = inputs[0].module.outputs[inputs[0].outParIndex];

                            outputs[0] = stuff;
                        }
                        else
                        {
                            outputs[0] = stuff;
                        }
                    }
                    catch (Exception exy)
                    {
                        outputs[0] = "error";
                    }

                }

            }
        }

        namespace MathModules
        {
            /// <summary>
            /// The <see cref="MathModules"/> namespace contains premade PUPPI modules for basic Math operations. Normally the Math menu also will contain modules generated from the System.Math namespace.
            /// </summary>

            [System.Runtime.CompilerServices.CompilerGenerated]
            class NamespaceDoc
            {
            }
            /// <summary>
            /// Addition of two numbers.
            /// </summary>
            public class PUPPIAddition : PUPPIModule
            {
                public PUPPIAddition()
                    : base()
                {
                    name = "Addition";
                    description = "Adds I1 to I2. Returns double type number.";
                    outputs.Add(0);
                    outputnames.Add("Result");
                    inputnames.Add("I1");
                    inputnames.Add("I2");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                }
                public override void process_usercode()
                {


                    //double number1 = 0;
                    //if (usercodeinputs[0] is double)
                    //{
                    //    number1 = (double)usercodeinputs[0];


                    //}
                    //else if (usercodeinputs[0] is int)
                    //{
                    //    number1 = Convert.ToDouble((int)usercodeinputs[0]);

                    //}
                    //else
                    //{

                    //}
                    //double number2 = 0;
                    //if (usercodeinputs[1] is double)
                    //{
                    //    number2 = (double)usercodeinputs[1];


                    //}
                    //else if (usercodeinputs[1] is int)
                    //{

                    //}
                    try
                    {
                        double number1 = Convert.ToDouble(usercodeinputs[0]);
                        double number2 = Convert.ToDouble(usercodeinputs[1]);
                        usercodeoutputs[0] = number1 + number2;
                    }
                    catch
                    {
                        usercodeoutputs[0] = "error";
                    }


                    return;
                }
            }


            /// <summary>
            /// Addition of two numbers.
            /// </summary>
            public class PUPPISubtraction : PUPPIModule
            {
                public PUPPISubtraction()
                    : base()
                {
                    name = "Subtraction";
                    description = "Subtracts I2 from I1. Returns double type number.";
                    outputs.Add(0);
                    outputnames.Add("Result");
                    inputnames.Add("I1");
                    inputnames.Add("I2");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                }
                public override void process_usercode()
                {



                    try
                    {
                        double number1 = Convert.ToDouble(usercodeinputs[0]);
                        double number2 = Convert.ToDouble(usercodeinputs[1]);
                        usercodeoutputs[0] = number1 - number2;
                    }
                    catch
                    {
                        usercodeoutputs[0] = "error";
                    }


                    return;
                }
            }


            /// <summary>
            /// Returns the greater of two numbers.
            /// </summary>
            public class PUPPIBiggestNumber : PUPPIModule
            {
                public PUPPIBiggestNumber()
                    : base()
                {
                    name = "Biggest";
                    description = "Returns the greater of two numbers";
                    outputs.Add(0);
                    outputnames.Add("Biggest #");
                    inputnames.Add("I1");
                    inputnames.Add("I2");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                }
                public override void process_usercode()
                {

                    //double number1 = 0;
                    //if (usercodeinputs[0] is double)
                    //{
                    //    number1 = (double)usercodeinputs[0];


                    //}
                    //else if (usercodeinputs[0] is int)
                    //{
                    //    number1 = Convert.ToDouble((int)usercodeinputs[0]);

                    //}
                    //double number2 = 0;
                    //if (usercodeinputs[1] is double)
                    //{
                    //    number2 = (double)usercodeinputs[1];


                    //}
                    //else if (usercodeinputs[1] is int)
                    //{
                    //    number2 = Convert.ToDouble((int)usercodeinputs[1]);
                    //}
                    try
                    {
                        double number1 = Convert.ToDouble(usercodeinputs[0]);
                        double number2 = Convert.ToDouble(usercodeinputs[1]);
                        if (number1 > number2) usercodeoutputs[0] = number1; else usercodeoutputs[0] = number2;
                    }
                    catch
                    {
                        usercodeoutputs[0] = "error";
                    }



                    return;
                }
            }
            /// <summary>
            /// Returns 1 if two numbers are equal, 0 otherwise.
            /// </summary>
            public class PUPPIAreEqual : PUPPIModule
            {
                public PUPPIAreEqual()
                    : base()
                {
                    name = "I1==I2";
                    description = "Returns 1 if two numbers are equal, 0 otherwise.";
                    outputs.Add(0);
                    outputnames.Add("T/F");
                    inputnames.Add("I1");
                    inputnames.Add("I2");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                }
                public override void process_usercode()
                {

                    //double number1 = 0;
                    //if (usercodeinputs[0] is double)
                    //{
                    //    number1 = (double)usercodeinputs[0];


                    //}
                    //else if (usercodeinputs[0] is int)
                    //{
                    //    number1 = Convert.ToDouble((int)usercodeinputs[0]);

                    //}
                    //double number2 = 0;
                    //if (usercodeinputs[1] is double)
                    //{
                    //    number2 = (double)usercodeinputs[1];


                    //}
                    //else if (usercodeinputs[1] is int)
                    //{
                    //    number2 = Convert.ToDouble((int)usercodeinputs[1]);
                    //}
                    try
                    {
                        double number1 = Convert.ToDouble(usercodeinputs[0]);
                        double number2 = Convert.ToDouble(usercodeinputs[1]);
                        if (number1 == number2) usercodeoutputs[0] = 1; else usercodeoutputs[0] = 0;
                    }
                    catch
                    {
                        usercodeoutputs[0] = "error";
                    }




                    return;
                }
            }

            /// <summary>
            /// Returns 1 if input 1 > input 2, 0 otherwise
            /// </summary>
            public class PUPPIGreaterThan : PUPPIModule
            {
                public PUPPIGreaterThan()
                    : base()
                {
                    name = "I1>I2";
                    description = "Returns 1 if input 1 > input 2, 0 otherwise.";
                    outputs.Add(0);
                    outputnames.Add("T/F");
                    inputnames.Add("I1");
                    inputnames.Add("I2");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                }
                public override void process_usercode()
                {


                    try
                    {
                        double number1 = Convert.ToDouble(usercodeinputs[0]);
                        double number2 = Convert.ToDouble(usercodeinputs[1]);
                        if (number1 > number2) usercodeoutputs[0] = 1; else usercodeoutputs[0] = 0;
                    }
                    catch
                    {
                        usercodeoutputs[0] = "error";
                    }




                    return;
                }
            }
            /// <summary>
            /// Multiplies two numbers.
            /// </summary>
            public class PUPPIMultiplication : PUPPIModule
            {
                public PUPPIMultiplication()
                    : base()
                {
                    name = "Multiplication";
                    outputs.Add(0);
                    outputnames.Add("Result");
                    inputnames.Add("I1");
                    inputnames.Add("I2");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                }
                public override void process_usercode()
                {

                    //double number1 = 0;
                    //if (usercodeinputs[0] is double)
                    //{
                    //    number1 = (double)usercodeinputs[0];


                    //}
                    //else if (usercodeinputs[0] is int)
                    //{
                    //    number1 = Convert.ToDouble((int)usercodeinputs[0]);

                    //}
                    //double number2 = 0;
                    //if (usercodeinputs[1] is double)
                    //{
                    //    number2 = (double)usercodeinputs[1];


                    //}
                    //else if (usercodeinputs[1] is int)
                    //{
                    //    number2 = Convert.ToDouble((int)usercodeinputs[1]);
                    //}
                    try
                    {
                        double number1 = Convert.ToDouble(usercodeinputs[0]);
                        double number2 = Convert.ToDouble(usercodeinputs[1]);
                        usercodeoutputs[0] = number1 * number2;
                    }
                    catch
                    {
                        usercodeoutputs[0] = "error";
                    }




                    return;
                }
            }
            /// <summary>
            /// Raises one input to the power of the other.
            /// </summary>
            public class PUPPIPower : PUPPIModule
            {
                public PUPPIPower()
                    : base()
                {
                    name = "Power";
                    outputs.Add(0);
                    outputnames.Add("Result");
                    inputnames.Add("Base");
                    inputnames.Add("Exp");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                }
                public override void process_usercode()
                {

                    //double number1 = 0;
                    //if (usercodeinputs[0] is double)
                    //{
                    //    number1 = (double)usercodeinputs[0];


                    //}
                    //else if (usercodeinputs[0] is int)
                    //{
                    //    number1 = Convert.ToDouble((int)usercodeinputs[0]);

                    //}
                    //double number2 = 0;
                    //if (usercodeinputs[1] is double)
                    //{
                    //    number2 = (double)usercodeinputs[1];


                    //}
                    //else if (usercodeinputs[1] is int)
                    //{
                    //    number2 = Convert.ToDouble((int)usercodeinputs[1]);
                    //}
                    try
                    {
                        double number1 = Convert.ToDouble(usercodeinputs[0]);
                        double number2 = Convert.ToDouble(usercodeinputs[1]);
                        usercodeoutputs[0] = Math.Pow(number1, number2);

                    }
                    catch
                    {
                        usercodeoutputs[0] = "error";
                    }


                    return;
                }
            }
            /// <summary>
            /// Divides the inputs, with error check.
            /// </summary>
            public class PUPPIDivision : PUPPIModule
            {
                public PUPPIDivision()
                    : base()
                {
                    name = "Division";
                    outputs.Add(0);
                    outputnames.Add("Result");
                    inputnames.Add("I1");
                    inputnames.Add("I2");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                }
                public override void process_usercode()
                {

                    //double number1 = 0;
                    //if (usercodeinputs[0] is double)
                    //{
                    //    number1 = (double)usercodeinputs[0];


                    //}
                    //else if (usercodeinputs[0] is int)
                    //{
                    //    number1 = Convert.ToDouble((int)usercodeinputs[0]);

                    //}
                    //double number2 = 0;
                    //if (usercodeinputs[1] is double)
                    //{
                    //    number2 = (double)usercodeinputs[1];


                    //}
                    //else if (usercodeinputs[1] is int)
                    //{
                    //    number2 = Convert.ToDouble((int)usercodeinputs[1]);
                    //}
                    try
                    {
                        double number1 = Convert.ToDouble(usercodeinputs[0]);
                        double number2 = Convert.ToDouble(usercodeinputs[1]);
                        //only works if number to dividenot 0
                        if (number2 != 0) usercodeoutputs[0] = number1 / number2;
                    }
                    catch
                    {
                        usercodeoutputs[0] = "error";
                    }



                    return;
                }
            }
            /// <summary>
            /// Returns the divisor and remainder of the integer inputs.
            /// </summary>
            public class PUPPIDivMod : PUPPIModule
            {
                public PUPPIDivMod()
                    : base()
                {
                    name = "Div & Mod";
                    outputs.Add(0);
                    outputnames.Add("Div");
                    outputs.Add(0);
                    outputnames.Add("Mod");

                    inputnames.Add("Num");
                    inputnames.Add("Den");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                }
                public override void process_usercode()
                {

                    int number1 = 0;

                    number1 = Convert.ToInt16(usercodeinputs[0]);


                    int number2 = 0;

                    number2 = Convert.ToInt16(usercodeinputs[1]);



                    //only works if number to dividenot 0
                    long testrem = 0;
                    long testdiv = 0;
                    if (number2 != 0) { testdiv = Math.DivRem((long)number1, (long)number2, out testrem); }
                    usercodeoutputs[0] = testdiv;
                    usercodeoutputs[1] = testrem;


                    return;
                }
            }
            /// <summary>
            /// Solves the Euler heat equation in 2D.
            /// </summary>
            public class Euler2DHeat : PUPPIModule
            {
                public Euler2DHeat()
                    : base()
                {
                    name = "Euler2DHeat";
                    outputs.Add(0);
                    outputnames.Add("TijN+1");

                    inputnames.Add("Tij");
                    inputnames.Add("Ti-1j");
                    inputnames.Add("Ti+1j");
                    inputnames.Add("Tij+1");
                    inputnames.Add("Tij-1");
                    inputnames.Add("dx");
                    inputnames.Add("dy");
                    inputnames.Add("dt");
                    inputnames.Add("k");

                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());

                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());



                }
                public override void process_usercode()
                {
                    double[] invals = new double[9];
                    for (int dcount = 0; dcount < 9; dcount++)
                    {



                        if (usercodeinputs[dcount] is double)
                        {
                            invals[dcount] = (double)usercodeinputs[dcount];


                        }
                        else if (usercodeinputs[dcount] is int)
                        {
                            invals[dcount] = Convert.ToDouble((int)usercodeinputs[dcount]);

                        }
                    }
                    //check dx dy<>0
                    if (invals[5] <= 0 || invals[6] <= 0)
                    {
                        usercodeoutputs[0] = -1;
                    }
                    else
                    {
                        usercodeoutputs[0] = invals[0] + invals[7] * invals[8] * ((invals[2] - 2 * invals[0] + invals[1]) / (invals[5] * invals[5]) + (invals[3] - 2 * invals[0] + invals[4]) / (invals[6] * invals[6]));
                    }


                    return;
                }
            }
        }
        namespace LogicModules
        {
            /// <summary>
            /// The <see cref="LogicModules"/> namespace contains PUPPI modules for basic logic operations.
            /// </summary>

            [System.Runtime.CompilerServices.CompilerGenerated]
            class NamespaceDoc
            {
            }


            ///// <summary>
            ///// Used by Conditional nodes to start branches or loops.
            ///// </summary>
            //public class PUPPIBranchStart : PUPPIModule
            //{
            //    public PUPPIBranchStart()
            //        : base()
            //    {

            //        name = "Branch Start";
            //        description = "Set input value with a Conditional node"; 
            //        outputs.Add("empty");
            //        outputnames.Add("Next Value");
            //        inputnames.Add("Initial Value");

            //        inputs.Add(new PUPPIInParameter());

            //    }
            //    public override void process_usercode()
            //    {
            //        usercodeoutputs[0] = usercodeinputs[0];  

            //    }

            //}

            /// <summary>
            /// Takes one input but returns no outputs. Can be used when making containers to close off unneeded outputs.
            /// </summary>
            public class PUPPITERMINATOR : PUPPIModule
            {
                public PUPPITERMINATOR()
                    : base()
                {
                    name = "Terminator";

                    description = "Takes one input but returns no outputs. Can be used when making containers to close off unneeded outputs.";
                    inputnames.Add("Input");

                    inputs.Add(new PUPPIInParameter());
                }
                public override void process_usercode()
                {



                }
            }


            public class PUPPIRunTimer : PUPPIModule
            {
                static bool isCurrentlyRunning = false;
                bool isRunning = false;
                int runs = 0;
                public PUPPIRunTimer()
                    : base()
                {
                    name = "Run Timer";
                    description = "Runs the program every x milliseconds (counted after program ends) as specified in input. Has state on/off set through double clicking. Default is off.";
                    doubleClickDescription = "Toggle state on / off/ Default is off";
                    inputnames.Add("Interval(ms)");
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                    completeDoubleClickOverride = true;
                    outputs.Add("unused");
                    outputnames.Add("Status");
                    unique = true;
                }
                public override void process_usercode()
                {
                    if (isRunning == true )
                    {
                        if (isCurrentlyRunning == false)
                        {
                            int myInterval = 5000;
                            try
                            {
                                myInterval = Convert.ToInt32(inputs[0].module.outputs[inputs[0].outParIndex]);
                                if (myInterval <= 0)
                                {
                                    outputs[0] = "invalid interval";

                                    return;
                                }
                                Task t = Task.Run(() =>
                                    {
                                       
                                        if (useMyPUPPIcanvas != null)
                                        {
                                            //already scheduled to be called
                                            if (useMyPUPPIcanvas.commandQueue.Contains("runcanvasprogramcommand"))
                                            {
                                               

                                            }
                                            else
                                            {
                                                isCurrentlyRunning = true;

                                                //wait to finish running queue
                                                while (PUPPIModel.PUPPIModule.concurrentProcesses + PUPPIServer.PUPPICanvasTCPServer.NumberClientThreads + PUPPIServer.PUPPICanvasHTTPServer.NumberClientThreads > 0)
                                                {
                                                    Thread.Sleep(10);
                                                }
                                                Thread.Sleep(myInterval);





                                                //useMyPUPPIcanvas.runPUPPIprogram(); 
                                                //wait to finish running queue
                                                while (PUPPIModel.PUPPIModule.concurrentProcesses + PUPPIServer.PUPPICanvasTCPServer.NumberClientThreads + PUPPIServer.PUPPICanvasHTTPServer.NumberClientThreads > 0 || PUPPICanvas.mouseisdown || useMyPUPPIcanvas.commandQueue.Count > 0)
                                                {
                                                    Thread.Sleep(10);
                                                }


                                                if (useMyPUPPIcanvas.somewhereProcessingStopped != null)
                                                {
                                                    Utilities.sendCommandToCanvas("runcanvasprogramcommand");
                                                    isCurrentlyRunning = false;
                                                    useMyPUPPIcanvas.Dispatcher.BeginInvoke(new EventHandler(useMyPUPPIcanvas.somewhereProcessingStopped), useMyPUPPIcanvas, null);
                                                    //  somewhereProcessingStopped(this, null);
                                                    //});
                                                }
                                                else
                                                {
                                                    int hh = 0;
                                                }
                                               
                                                //

                                                isCurrentlyRunning = false;
                                            }
                                        }
                                       
                                    });
                                runs++;
                                outputs[0] = "Ran: " + runs.ToString();
                            }
                            catch
                            {

                                outputs[0] = "error";
                                isCurrentlyRunning = false;
                            }
                        }

                    }
                    else
                    {
                        outputs[0] = "disabled";
                    }
                    
                }

                public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
                {
                    runs = 0;
                    isRunning = !isRunning;  
                }

            }

            /// <summary>
            /// Basic OR operation between 2 inputs, returns 1 if true. Inputs are converted to booleans.
            /// </summary>
            public class PUPPIOR : PUPPIModule
            {
                public PUPPIOR()
                    : base()
                {
                    name = "OR";
                    outputs.Add(0);
                    outputnames.Add("Result");
                    inputnames.Add("I1");
                    inputnames.Add("I2");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                }
                public override void process_usercode()
                {


                    bool b1 = false;
                    bool b2 = false;
                    try
                    {
                        b1 = Convert.ToBoolean(usercodeinputs[0]);
                        b2 = Convert.ToBoolean(usercodeinputs[1]);
                    }
                    catch
                    {
                        usercodeoutputs[0] = 0;
                        return;
                    }
                    usercodeoutputs[0] = Convert.ToInt16(b1 || b2);




                    return;
                }
            }
            /// <summary>
            /// Basic AND operation between 2 inputs, returns 1 if true. Inputs are converted to booleans.
            /// </summary>
            public class PUPPIAND : PUPPIModule
            {
                public PUPPIAND()
                    : base()
                {
                    name = "AND";
                    outputs.Add(0);
                    outputnames.Add("Result");
                    inputnames.Add("I1");
                    inputnames.Add("I2");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                }
                public override void process_usercode()
                {


                    bool b1 = false;
                    bool b2 = false;
                    try
                    {
                        b1 = Convert.ToBoolean(usercodeinputs[0]);
                        b2 = Convert.ToBoolean(usercodeinputs[1]);
                    }
                    catch
                    {
                        usercodeoutputs[0] = 0;
                        return;
                    }
                    usercodeoutputs[0] = Convert.ToInt16(b1 && b2);




                    return;
                }
            }

            /// <summary>
            /// Waits specified number of milliseconds
            /// </summary>
            public class PUPPIWait : PUPPIModule
            {
                public PUPPIWait()
                    : base()
                {
                    name = "Wait";
                    outputs.Add(0);
                    outputnames.Add("Dummy");
                    description = "Waits specified number of milliseconds";
                    inputnames.Add("Duration (ms)");

                    inputs.Add(new PUPPIInParameter());

                }
                public override void process_usercode()
                {




                    try
                    {
                        int wd = Convert.ToInt32(usercodeinputs[0]);
                        if (wd > 0)
                        {
                            Thread.Sleep(wd);
                            usercodeoutputs[0] = wd;
                        }
                    }
                    catch
                    {

                        usercodeoutputs[0] = "error";
                    }






                }


            }


            /// <summary>
            /// Takes one input that is passed through and a dummy input, thus joining two workflow branches to direct program flow downstream after both branches have processed
            /// </summary>
            public class PUPPIJoinBranches : PUPPIModule
            {
                public PUPPIJoinBranches()
                    : base()
                {
                    name = "Join Branches";
                    outputs.Add(0);
                    outputnames.Add("I1 passed");
                    inputnames.Add("I1(pass)");
                    inputnames.Add("I2(dummy)");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                }
                public override void process_usercode()
                {



                    usercodeoutputs[0] = usercodeinputs[0];





                }
            }

            ///// <summary>
            ///// Sets an output of a node of the canvas to a specified value and causes nodes downstream to process.
            ///// Can be used to send information apart from connections.
            ///// </summary>
            //public class PUPPISetOutput : PUPPIModule
            //{
            //    public PUPPISetOutput()
            //    : base()
            //    {
            //        name = "Set Output";
            //        description = "Sets an output of a node of the canvas to a specified value and causes nodes downstream to process. Can be used to send information apart from connections.";
            //        inputnames.Add("Node GUID");
            //        inputs.Add(new PUPPIInParameter());
            //        inputnames.Add("Output Index");
            //        inputs.Add(new PUPPIInParameter());
            //        inputnames.Add("Value");
            //        inputs.Add(new PUPPIInParameter());
            //        completeProcessOverride = true;
            //        outputs.Add("Not Run");
            //       outputnames.Add("Operation Result"); 
            //     }

            //    public override void process_usercode()
            //    {
            //        outputs[0] = "Not performed";
            //        try
            //        {

            //            if (useMyPUPPIcanvas != null)
            //            {
            //                object i1 = inputs[0].module.outputs[inputs[0].outParIndex];
            //                string s1 = Convert.ToString(i1); 
            //                if (s1=="")
            //                {
            //                    outputs[0] = "Invalid GUID";
            //                    return;
            //                }
            //                object i2 = inputs[1].module.outputs[inputs[1].outParIndex];
            //                int oi = Convert.ToInt16(i2);  
            //                if (oi<0)
            //                {
            //                    outputs[0] = "Invalid output index";
            //                    return;
            //                }
            //                object i3 = inputs[2].module.outputs[inputs[2].outParIndex];
            //                useMyPUPPIcanvas.setMyNodeOnCanvasOutput(s1, oi, i3);
            //                outputs[0] = "Performed";
            //            }
            //            else
            //            {
            //                outputs[0] = "No canvas";
            //            }
            //        }
            //        catch
            //        {
            //            outputs[0] = "Error";
            //        }
            //    }            
            //}


            //public class PUPPIForceBranch:PUPPIModule
            //{
            //     public PUPPIForceBranch()
            //        : base()
            //    {
            //        name = "Branch Flow";
            //        description = "Takes one input and sends it to two outputs. Used for conditionals, loops etc.";
            //        inputs.Add(new PUPPIInParameter());
            //        inputnames.Add("Anything");
            //        outputs.Add("nothing");
            //        outputnames.Add("Branch 1");
            //        outputs.Add("nothing");
            //        outputnames.Add("Branch 2");
            //        completeProcessOverride = true; 

            //    }
            //     public override void process_usercode()
            //     {

            //         outputs[0] = inputs[0].module.outputs[inputs[0].outParIndex];
            //         outputs[1] = inputs[0].module.outputs[inputs[0].outParIndex];
            //     }
            //}




            ///// <summary>
            ///// If fourth input is not 0, sets an output of a node of the canvas to a specified value and causes nodes downstream to process.
            ///// Can be used to for branching and loops.
            ///// </summary>
            //public class PUPPISetInputIfTrue : PUPPIModule
            //{
            //    int ngi = -1;
            //    int ngo = -1;
            //    int ii = -1;
            //    int oi = -1;
            //    long ml = 0;
            //    long icount = 0;
            //    public PUPPISetInputIfTrue()
            //        : base()
            //    {
            //        name = "Conditional Set Output";
            //        description = "Sets an output of a node of the canvas to a specified value and causes nodes downstream to process. Can be used to send information apart from connections, for creating loops or branching.";
            //        doubleClickDescription = "Double click to set vhich node input to get value from node output"; 
            //        inputnames.Add("1/0");
            //        inputs.Add(new PUPPIInParameter());


            //        completeProcessOverride = true;
            //        outputs.Add("Not Run");
            //        outputnames.Add("Value Set");
            //        outputs.Add(0);
            //        outputnames.Add("Done");  
            //    }

            //    public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
            //    {
            //        PUPPI.setNodeOutputForm sn = new PUPPI.setNodeOutputForm();
            //        sn.iG = ngi;
            //        sn.ii = ii;
            //        sn.oG = ngo;
            //        sn.oi = oi;
            //        sn.ml = ml; 
            //        sn.ShowDialog();
            //        ngi = sn.iG;
            //        ii = sn.ii;
            //        ngo = sn.oG;
            //        oi = sn.oi;
            //        ml = sn.ml;

            //    }

            //    public override void process_usercode()
            //    {
            //        outputs[0] = "Not Running";
            //        outputs[1] = 1;
            //        if (ml == 0 || icount <= ml)
            //        {

            //            try
            //            {

            //                if (useMyPUPPIcanvas != null)
            //                {



            //                    object i4 = inputs[0].module.outputs[inputs[0].outParIndex];
            //                    if (i4 == null)
            //                    {
            //                        outputs[0] = "No conditional";
            //                        return;
            //                    }
            //                    int ii4 = Convert.ToInt16(i4);


            //                    //if (ii4 != 0)
            //                    //{
            //                        object setit = null;

            //                        if (ngo < 0)
            //                        {
            //                            outputs[0] = "Invalid output node GUID";
            //                            return;
            //                        }
            //                        else
            //                        {
            //                            if (useMyPUPPIcontroller != null)
            //                            {
            //                                PUPPIModule pa = useMyPUPPIcontroller.getNodeModuleByGUID(ngo.ToString());
            //                                if (pa == null)
            //                                {
            //                                    outputs[0] = "Invalid output node GUID";
            //                                    return;
            //                                }
            //                                if (oi < 0 || oi >= pa.outputs.Count)
            //                                {
            //                                    outputs[0] = "Invalid output index";
            //                                    return;
            //                                }
            //                                setit = pa.outputs[oi];

            //                            }
            //                            else
            //                            {
            //                                outputs[0] = "No canvas";
            //                                return;
            //                            }
            //                        }

            //                        bool bulla = Convert.ToBoolean(ii4);
            //                        if (bulla==false)
            //                        {
            //                            icount = 0;
            //                            outputs[1] = 1;
            //                            outputs[0] = setit;

            //                        }
            //                        else
            //                        {
            //                            outputs[0] = setit;
            //                            icount++;


            //                            outputs[1] = 0;

            //                        }
            //                        useMyPUPPIcanvas.setMyNodeOnCanvasInput(this, ngi.ToString(), ii, setit, bulla); 

            //                        //if (useMyPUPPIcanvas.setMyNodeOnCanvasInput(ngi.ToString(), ii, setit))
            //                        //{
            //                        //    outputs[1] = 1;
            //                        //}

            //                    //}
            //                    //else
            //                    //{
            //                    //    //reset iteration counter
            //                    //    icount = 0;
            //                    //}
            //                }
            //                else
            //                {
            //                    outputs[0] = "No canvas";
            //                }
            //            }
            //            catch
            //            {
            //                outputs[0] = "Error";
            //            }
            //        }
            //        else
            //        {
            //            outputs[0] = "Limit reached";
            //        }
            //    }

            //    public override string saveSettings()
            //    {
            //        return ngi.ToString() + "_" + ngo.ToString() + "_" + ii.ToString() + "_" + oi.ToString() + "_" + ml.ToString(); ;    
            //    }

            //    public override void initOnLoad(string savedSettings)
            //    {
            //        char[] splitter = { '_' };
            //        string[] sp = savedSettings.Split(splitter);
            //        try
            //        {
            //            ngi = Convert.ToInt16(sp[0]);
            //            ngo = Convert.ToInt16(sp[1]);
            //            ii = Convert.ToInt16(sp[2]);
            //            oi = Convert.ToInt16(sp[3]);  
            //            //for compatibility with older
            //            if (sp.Length==5 )
            //            {
            //                ml = Convert.ToInt64(sp[4]);  
            //            }
            //            else
            //            {
            //                ml = 0;
            //            }
            //        }
            //        catch
            //        {

            //        }
            //    }

            //}

            /// <summary>
            /// If this node's input 0 is true (>0), output is set to value of input 1 and processing continues downstream of this node."
            /// </summary>
            public class PUPPIConditionalPass : PUPPIModule
            {
               
                public PUPPIConditionalPass()
                    : base()
                {
                    // isconditional = true; 
                    name = "Conditional Pass";
                    description = "If this node's input 0 is true (>0), output is set to value of input 1 and processing continues downstream of this node.";
                    inputnames.Add("1/0");
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Pass Value");
                    inputs.Add(new PUPPIInParameter());
                    outputs.Add(null);
                    outputnames.Add("Pass");  
                    completeProcessOverride = true;

                }

              
                public override void process_usercode()
                {
                   
                        try
                        {
                                object i4 = inputs[0].module.outputs[inputs[0].outParIndex];
                                if (i4 == null)
                                {

                                    return;
                                }
                                int ii4 = Convert.ToInt16(i4);
                                              

                                bool doprocess = Convert.ToBoolean(ii4);
                                haltProcess = !doprocess;
                            if (doprocess )
                            {
                                outputs[0] = inputs[1].module.outputs[inputs[1].outParIndex];
                            }
                        }
                        catch
                        {
                            haltProcess = true; 
                        }
                   
                }

                

            }

            /// <summary>
            /// If this node's input is true, sets an input of specified node equal to output of another node and causes node with input to process.
            /// Can be used to for branching and loops.
            /// </summary>
            public class PUPPIConditional : PUPPIModule
            {
                internal int ngi = -1;
                int ngo = -1;
                internal int ii = -1;
                int oi = -1;
                long ml = 0;
                long icount = 0;
                internal object setme = null;
                internal bool doprocess = false;
                public PUPPIConditional()
                    : base()
                {
                    // isconditional = true; 
                    name = "Conditional Jump";
                    description = "If this node's input is true, sets an input of specified node equal to output of another node and causes node with input to process. Can be used to send information apart from connections, for creating loops or branching.";
                    doubleClickDescription = "Double click to set which node input to get value from node output";
                    inputnames.Add("1/0");
                    inputs.Add(new PUPPIInParameter());
                    setme = null;
                    doprocess = false;
                    completeProcessOverride = true;

                }

                public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
                {
                    PUPPI.setNodeOutputForm sn = new PUPPI.setNodeOutputForm();
                    sn.iG = ngi;
                    sn.ii = ii;
                    sn.oG = ngo;
                    sn.oi = oi;
                    sn.ml = ml;
                    sn.ShowDialog();
                    ngi = sn.iG;
                    ii = sn.ii;
                    ngo = sn.oG;
                    oi = sn.oi;
                    ml = sn.ml;

                }

                public override void process_usercode()
                {
                    doprocess = false;
                    if (ml == 0 || icount <= ml)
                    {

                        try
                        {

                            if (useMyPUPPIcanvas != null)
                            {



                                object i4 = inputs[0].module.outputs[inputs[0].outParIndex];
                                if (i4 == null)
                                {

                                    return;
                                }
                                int ii4 = Convert.ToInt16(i4);


                                //if (ii4 != 0)
                                //{
                                setme = null;
                                int setin = -1;
                                if (ngo < 0)
                                {

                                    return;
                                }
                                else
                                {
                                    if (useMyPUPPIcontroller != null)
                                    {
                                        PUPPIModule pa = useMyPUPPIcontroller.getNodeModuleByID(ngo.ToString());
                                        if (pa == null)
                                        {

                                            return;
                                        }
                                        if (oi < 0 || oi >= pa.outputs.Count)
                                        {

                                            return;
                                        }
                                        setme = PUPPIModel.Utilities.CloneObject(pa.outputs[oi]);
                                        PUPPIModel.Utilities.commandObjs.Add(setme);
                                        setin = PUPPIModel.Utilities.commandObjs.Count - 1;
                                    }
                                    else
                                    {

                                        return;
                                    }
                                }

                                doprocess = Convert.ToBoolean(ii4);
                                if (doprocess == false)
                                {
                                    icount = 0;


                                }
                                else
                                {

                                    icount++;
                                    addCanvasCommand("SetInput_|_" + ngi.ToString() + "_|_" + ii.ToString() + "_|_" + setin.ToString());


                                }



                            }
                            else
                            {

                            }
                        }
                        catch
                        {

                        }
                    }
                    else
                    {

                    }
                }

                public override string saveSettings()
                {
                    return ngi.ToString() + "_" + ngo.ToString() + "_" + ii.ToString() + "_" + oi.ToString() + "_" + ml.ToString(); ;
                }

                public override void initOnLoad(string savedSettings)
                {
                    char[] splitter = { '_' };
                    string[] sp = savedSettings.Split(splitter);
                    try
                    {
                        ngi = Convert.ToInt16(sp[0]);
                        ngo = Convert.ToInt16(sp[1]);
                        ii = Convert.ToInt16(sp[2]);
                        oi = Convert.ToInt16(sp[3]);
                        //for compatibility with older
                        if (sp.Length == 5)
                        {
                            ml = Convert.ToInt64(sp[4]);
                        }
                        else
                        {
                            ml = 0;
                        }
                    }
                    catch
                    {

                    }
                }

            }
            /// <summary>
            /// Forces the canvas visuals to update while program is running.
            /// </summary>
            public class PUPPIUpdateCanvas : PUPPIModule
            {
                public PUPPIUpdateCanvas()
                    :base()
                {
                    name = "Update Canvas";
                    description = "Forces the canvas visuals to update while program is running. ";
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Dummy");
                    outputs.Add(0);
                    outputnames.Add("Dummy");  

                }
                public override void process_usercode()
                {
                    Dispatcher.CurrentDispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
                }
            }

            /// <summary>
            /// If input 1 is true, returns input 2, otherwise returns input 3.
            /// </summary>
            public class PUPPI_IF : PUPPIModule
            {
                public PUPPI_IF()
                    : base()
                {
                    name = "IF Statement";
                    outputs.Add(0);
                    outputnames.Add("Result");
                    inputnames.Add("Conditional T/F");
                    inputnames.Add("Input For True");
                    inputnames.Add("Input For False");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                }
                public override void process_usercode()
                {


                    bool b1 = false;

                    try
                    {
                        b1 = Convert.ToBoolean(usercodeinputs[0]);
                        if (b1) usercodeoutputs[0] = usercodeinputs[1]; else usercodeoutputs[0] = usercodeinputs[2];
                    }
                    catch
                    {
                        usercodeoutputs[0] = 0;
                        return;
                    }





                    return;
                }
            }

            /// <summary>
            /// Users can write their own scriopt using inputs and outputs of the node. Inputs and outputs are considered ArrayLists
            /// </summary>
            //public class PUPPIScript : PUPPIModule
            //{
            //    string storedScript = "";
            //    public PUPPIScript()
            //        : base()
            //    {
            //        name = "Script";
            //        outputs.Add(new ArrayList());
            //        outputnames.Add("Script Out. ArrLst.");
            //        inputs.Add(new PUPPIInParameter());
            //        inputnames.Add("Script In. ArrLst.");
            //    }
            //    public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
            //    {
            //        PUPPI.PUPPIScriptingWindow p = new PUPPI.PUPPIScriptingWindow();
            //        PUPPIModel.PUPPIScripting.PUPPIRuntimeScript ps = new PUPPIScripting.PUPPIRuntimeScript();
            //        //if (useMyPUPPIcanvas != null)
            //        ps.storedScriptData = storedScript;
            //        if (ps.storedScriptData != "")
            //        {
            //            ps.setUpMyScript();
            //            p.selfUpdateFromScriptClass(ps);
            //        }

            //        if (usercodeinputs != null && usercodeinputs[0] != null)
            //        {
            //            if (usercodeinputs[0] is ICollection)
            //            {
            //                p.winInputsFromNode.AddRange(usercodeinputs[0] as ICollection);
            //            }
            //            else
            //            {
            //                p.winInputsFromNode.Add(usercodeinputs[0]);
            //            }
            //        }
            //        else
            //        {

            //        }

            //        p.ShowDialog();
            //        if (p.storedScriptData != "nope")
            //        {
            //            storedScript = p.storedScriptData;
            //        }

            //    }
            //    public override void process_usercode()
            //    {
            //        PUPPIScripting.PUPPIRuntimeScript p = new PUPPIScripting.PUPPIRuntimeScript();
            //        //if (useMyPUPPIcanvas!=null )
            //        p.storedScriptData = storedScript;
            //        if (p.storedScriptData != "")
            //        {
            //            p.setUpMyScript();

            //            if (usercodeinputs[0] is ICollection)
            //            {
            //                p.inputsFromNode.AddRange(usercodeinputs[0] as ICollection);
            //            }
            //            else
            //            {
            //                p.inputsFromNode.Add(usercodeinputs[0]);
            //            }
            //            //p.inputsFromNode.AddRange(usercodeinputs[0] as ArrayList);
            //            p.runMyScript(p.justTheScript);
            //            usercodeoutputs[0] = p.resultOutputs;
            //        }
            //    }
            //    public override void initOnLoad(string savedSettings)
            //    {
            //        storedScript = savedSettings;
            //    }
            //    public override string saveSettings()
            //    {
            //        return storedScript;
            //    }
            //    //only at end
            //    public void addScriptInput()
            //    {

            //    }
            //    public void removeScriptInput()
            //    {

            //    }
            //    public void addScriptOutput()
            //    {

            //    }
            //    public void removeScriptOutput()
            //    {

            //    }
            //}






            /// <summary>
            /// Script module with variable number of inputs and outputs.
            /// </summary>
            public class PUPPIInteractiveScript : PUPPIModule
            {


                internal string storedScriptData;
                internal List<string> assemblyPaths;
                internal List<string> assemblyNameSpaces;
                internal List<string> assemblyNames;
                internal string justTheScript;
                internal string scriptProgrammingLanguage;
                internal string aS = "";
                internal bool autoLoadScriptNamespaces;
                static List<string> myPaths;
                internal ArrayList scriptInputValues;
                internal ArrayList scriptOutputValues;
                internal Assembly mya;

                public PUPPIInteractiveScript()
                    : base()
                {
                    name = "Script";
                    description = "A customizable script editor.";
                    assemblyPaths = new List<string>();
                    assemblyNameSpaces = new List<string>();
                    assemblyNames = new List<string>();
                    scriptInputValues = new ArrayList();
                    scriptOutputValues = new ArrayList();
                    mya = null;
                    oneTimeDoubleClickListInteraction = true;
                    doubleClickDescription = "Edit script, references,inputs and outputs";
                }


                public override void initOnLoad(string savedSettings)
                {
                    inputs.Clear();
                    outputs.Clear();
                    inputnames.Clear();
                    outputnames.Clear();
                    storedScriptData = savedSettings;
                    mya = null;
                    int nin = 0;
                    int nop = 0;
                    if (storedScriptData != "")
                    {
                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.LoadXml(storedScriptData);

                            XmlNode sasc = doc.SelectSingleNode("/savedNodeScript");
                            for (int i = 0; i < sasc.ChildNodes.Count; i++)
                            {
                                XmlNode chi = sasc.ChildNodes[i];
                                if (chi.Name == "input")
                                {
                                    PUPPIInParameter ti = new PUPPIInParameter();
                                    ti.isoptional = true;
                                    inputs.Add(ti);
                                    inputnames.Add(chi.InnerText);

                                }
                                if (chi.Name == "output")
                                {
                                    outputs.Add("0");
                                    outputnames.Add(chi.InnerText);
                                }
                            }
                        }
                        catch
                        {
                            throw new Exception("Invalid script data");
                        }
                        setUpMyScript();
                    }

                    //for (int i = 0; i < nin; i++)
                    //{
                    //    PUPPIInParameter ti = new PUPPIInParameter();
                    //    ti.isoptional = true;
                    //    inputs.Add(ti);
                    //    inputnames.Add("Input" + (i + 1).ToString());
                    //}
                    //for (int i = 0; i < nop; i++)
                    //{
                    //    outputs.Add(0);
                    //    outputnames.Add("Output" + (i + 1).ToString());
                    //}

                }
                public override string saveSettings()
                {
                    return storedScriptData;
                }
                internal void setUpMyScript()
                {
                    if (storedScriptData != null && storedScriptData != "")
                    {

                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.LoadXml(storedScriptData);


                            XmlNode sasc = doc.SelectSingleNode("/savedNodeScript");
                            assemblyPaths.Clear();
                            for (int i = 0; i < sasc.ChildNodes.Count; i++)
                            {
                                XmlNode chi = sasc.ChildNodes[i];
                                if (chi.Name == "scriptProgLang")
                                {
                                    scriptProgrammingLanguage = chi.InnerText;
                                }
                                if (chi.Name == "scriptProgram")
                                {
                                    justTheScript = chi.InnerText;
                                }
                                if (chi.Name == "namespaceLoad")
                                {
                                    try
                                    {
                                        autoLoadScriptNamespaces = Convert.ToBoolean(chi.InnerText);
                                    }
                                    catch
                                    {
                                        autoLoadScriptNamespaces = true;
                                    }
                                }

                                if (chi.Name == "loadedAssemblyDLL")
                                {
                                    string myap = chi.InnerText;
                                    try
                                    {
                                        Assembly a = null;
                                        try
                                        {
                                            a = Assembly.LoadFile(myap);
                                        }
                                        catch
                                        {
                                            a = null;
                                        }
                                        if (a == null)
                                        {
                                            //try from local folder
                                            string curfile = getModuleFileNamePath();
                                            if (curfile != "")
                                            {
                                                string dp = System.IO.Path.GetDirectoryName(curfile);
                                                string cp = System.IO.Path.GetDirectoryName(chi.InnerText);
                                                myap = chi.InnerText.Replace(cp, dp);
                                                try
                                                {
                                                    a = Assembly.LoadFile(myap);
                                                }
                                                catch
                                                {
                                                    a = null;
                                                }
                                            }

                                        }
                                        if (a != null)
                                        {
                                            List<string> an = getAssemblyNamespaceNames(a);
                                            for (int j = 0; j < an.Count; j++)
                                            {
                                                if (!assemblyNameSpaces.Contains(an[j]) && an[j] != "System" && an[j] != "System.Collections") assemblyNameSpaces.Add(an[j]);
                                            }

                                            assemblyPaths.Add(myap);
                                            //assemblyNames.Add(a.FullName);
                                        }

                                    }
                                    catch
                                    {
                                        throw new Exception("Failed to load assembly " + chi.InnerText + " into script");

                                    }

                                }
                            }
                        }
                        catch (Exception exy)
                        {

                            throw new Exception("Invalid script data: " + exy.ToString());

                        }
                    }
                }

                public override void process_usercode()
                {

                    if (storedScriptData != "")
                    {
                        setUpMyScript();
                        scriptInputValues.Clear();
                        if (usercodeinputs != null)
                            for (int i = 0; i < usercodeinputs.Count; i++)
                            {
                                scriptInputValues.Add(usercodeinputs[i]);
                            }
                        runMyScript(justTheScript, false);
                        if (usercodeoutputs != null)
                            if (scriptOutputValues.Count == usercodeoutputs.Count)
                            {
                                usercodeoutputs.Clear();
                                for (int i = 0; i < scriptOutputValues.Count; i++)
                                {
                                    usercodeoutputs.Add(scriptOutputValues[i]);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < usercodeoutputs.Count; i++)
                                    usercodeoutputs[i] = "Script run Error";
                            }


                    }
                }
                public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
                {
                    PUPPI.PUPPIScriptingWindow p = new PUPPI.PUPPIScriptingWindow();

                    p.selfUpdateFromScriptClass(this);
                    //p.origALN = p.autoLoadScriptNamespaces;
                    //p.origAPs = new List<string>();
                    //p.origAPs.AddRange(p.winAssemblyPaths);
                    //p.origCS = (this.scriptProgrammingLanguage == "C#");
                    //p.origScript = this.justTheScript;
                    //p.origSSD = this.storedScriptData;  

                    p.ShowDialog();
                    //in case no inputs
                    if (inputs.Count == 0)
                    {
                        process_usercode();
                    }

                }

                internal void runMyScript(string scriptText, bool rinteract = true)
                {

                    if (PUPPIDebugger.debugenabled)
                    {
                        PUPPIDebugger.log("Started " + utils.StringConstants.srunscri);
                    }
                    scriptOutputValues.Clear();
                    for (int oi = 0; oi < usercodeoutputs.Count; oi++)
                    {
                        scriptOutputValues.Add("Not set");
                    }
                    if (mya == null)
                    {
                        CompilerResults results;
                        string myScript = scriptText;
                        string source;
                        #region C#
                        myPaths = new List<string>();

                        foreach (string aP in assemblyPaths)
                        {
                            myPaths.Add(aP);

                        }

                        //Assembly aaaa = Assembly.GetExecutingAssembly();

                        //    if (aaaa != null)
                        //    {
                        //        List<string> an = getAssemblyNamespaceNames(aaaa);
                        //        for (int j = 0; j < an.Count; j++)
                        //        {
                        //            if (!assemblyNameSpaces.Contains(an[j]) && an[j] != "System" && an[j] != "System.Collections") assemblyNameSpaces.Add(an[j]);
                        //        }
                        //    }


                        if (scriptProgrammingLanguage == "C#")
                        {
                            source = @"namespace myPUPPIScriptNamespace
{
using System;";
                            source += "using System.Collections;\n";
                            //  source += "using PUPPIModel.Utilities;\n";
                            //source += "using System.Collections.Generic;\n";
                            if (autoLoadScriptNamespaces)
                            {
                                for (int ume = 0; ume < assemblyNameSpaces.Count; ume++)
                                {
                                    source += "using " + assemblyNameSpaces[ume] + ";\n";
                                }


                            }
                            source += @"
  
    public class myPUPPIScriptClass
    {

        public ArrayList nodeInputs {get;set;}
        public ArrayList scriptOutputs {get;set;}
        public myPUPPIScriptClass()
        {
                nodeInputs=new ArrayList();
                scriptOutputs=new ArrayList();

";
                            source += @"
        }
        public void myPUPPIScriptFunction()
        {
            "
                                     + @myScript +
                                     " }  }} ";

                            Dictionary<string, string> providerOptions = new Dictionary<string, string>
                {
                    {"CompilerVersion", "v4.0"}
                };
                            CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

                            CompilerParameters compilerParams = new CompilerParameters
                            {
                                GenerateInMemory = true,
                                GenerateExecutable = false
                            };

                            compilerParams.ReferencedAssemblies.Add("System.dll");
                            //compilerParams.ReferencedAssemblies.Add(@".\PUPPI.dll");
                            foreach (string aP in assemblyPaths)
                            {
                                //string aa = aP.Replace(@"\\", @"\");
                                compilerParams.ReferencedAssemblies.Add(aP);

                            }

                            aS = source;
                            try
                            {
                                results = provider.CompileAssemblyFromSource(compilerParams, source);
                            }
                            catch (Exception exy)
                            {
                                if (rinteract)
                                    MessageBox.Show("Failed to compile. Close the script node and try again");
                                if (PUPPIDebugger.debugenabled)
                                {
                                    PUPPIDebugger.log("Failed " + utils.StringConstants.srunscri + ":" + exy.ToString());
                                }
                                return;

                            }
                            if (results == null)
                            {
                                return;
                            }


                        }
                        #endregion
                        #region VB
                        else
                        {
                            source =
                            @"imports System
imports System.Collections
";
                            source += "\n";
                            if (autoLoadScriptNamespaces)
                                for (int ume = 0; ume < assemblyNameSpaces.Count; ume++)
                                {
                                    source += "imports " + assemblyNameSpaces[ume] + "\n";
                                }
                            source += @"
namespace myPUPPIScriptNamespace
    public class myPUPPIScriptClass
    
    Private _nodeInputs As ArrayList

    Public Property nodeInputs() As ArrayList
	    Get
	     Return _nodeInputs
	    End Get
	    Set(ByVal value As ArrayList)
	        _nodeInputs = value
	    End Set
    End Property

 Private _scriptOutputs As ArrayList

    Public Property scriptOutputs() As ArrayList
	    Get
	     Return _scriptOutputs
	    End Get
	    Set(ByVal value As ArrayList)
	        _scriptOutputs = value
	    End Set
    End Property
        
        public Sub New()
                _nodeInputs=new ArrayList()
                _scriptOutputs=new ArrayList()   
        End Sub

        public Sub myPUPPIScriptFunction()
        
            "
                            + @myScript +
                            " \nEnd Sub \n End Class\n End Namespace";

                            Dictionary<string, string> providerOptions = new Dictionary<string, string>
                {
                    {"CompilerVersion", "v4.0"}
                };
                            VBCodeProvider provider = new VBCodeProvider(providerOptions);

                            CompilerParameters compilerParams = new CompilerParameters
                            {
                                GenerateInMemory = true,
                                GenerateExecutable = false
                            };
                            compilerParams.ReferencedAssemblies.Add("System.dll");
                            // compilerParams.ReferencedAssemblies.Add(@".\PUPPI.dll");
                            foreach (string aP in assemblyPaths)
                            {
                                //string aa = aP.Replace(@"\\", @"\");
                                compilerParams.ReferencedAssemblies.Add(aP);
                            }

                            aS = source;

                            try
                            {
                                results = provider.CompileAssemblyFromSource(compilerParams, source);
                            }
                            catch (Exception exy)
                            {
                                if (rinteract)
                                    MessageBox.Show("Failed to compile. Close the script node and try again");
                                if (PUPPIDebugger.debugenabled)
                                {
                                    PUPPIDebugger.log("Failed " + utils.StringConstants.srunscri + ":" + exy.ToString());
                                }
                                return;

                            }
                            if (results == null)
                            {
                                return;
                            }

                        }
                        #endregion
                        if (results.Errors.Count != 0)
                        {
                            string compilationErrors = "Compile failed!\n";
                            foreach (CompilerError ce in results.Errors)
                            {
                                compilationErrors += ce.ErrorText + " at line:" + ce.Line.ToString() + "\n";
                            }
                            //compilationErrors += source;
                            if (rinteract)
                                MessageBox.Show(compilationErrors);
                            if (PUPPIDebugger.debugenabled)
                            {
                                PUPPIDebugger.log("Failed " + utils.StringConstants.srunscri + ":" + compilationErrors);
                            }
                            return;
                        }
                        mya = results.CompiledAssembly;
                    }
                    AppDomain currentDomain = AppDomain.CurrentDomain;
                    try
                    {

                        currentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromSavedFolder);
                        object o = mya.CreateInstance("myPUPPIScriptNamespace.myPUPPIScriptClass");
                        Type t = o.GetType();
                        MethodInfo mi = t.GetMethod("myPUPPIScriptFunction");

                        //set inputs
                        PropertyInfo iPropInfo = t.GetProperty("nodeInputs", BindingFlags.Instance | BindingFlags.Public |
                                BindingFlags.NonPublic);
                        iPropInfo.SetValue(o, scriptInputValues);
                        //set outputs with current values
                        PropertyInfo oPropInfo = t.GetProperty("scriptOutputs", BindingFlags.Instance | BindingFlags.Public |
                               BindingFlags.NonPublic);
                        oPropInfo.SetValue(o, scriptOutputValues);

                        mi.Invoke(o, null);
                        oPropInfo = t.GetProperty("scriptOutputs", BindingFlags.Instance | BindingFlags.Public |
                               BindingFlags.NonPublic);
                        var oValue = oPropInfo.GetValue(o, null);
                        scriptOutputValues = oValue as ArrayList;

                    }
                    catch (Exception exy)
                    {
                        if (PUPPIDebugger.debugenabled)
                        {
                            PUPPIDebugger.log("Failed " + utils.StringConstants.srunscri + ":" + exy.ToString());
                        }
                        if (rinteract)
                            MessageBox.Show("Script run error!\n" + exy.ToString());
                    }
                    currentDomain.AssemblyResolve -= new ResolveEventHandler(LoadFromSavedFolder);
                    if (PUPPIDebugger.debugenabled)
                    {
                        PUPPIDebugger.log("Finished " + utils.StringConstants.srunscri);
                    }
                }



                //saves the stuff in node script format
                internal void saveScriptForStorage()
                {
                    XmlDocument doc = new XmlDocument();
                    XmlNode mn = doc.CreateNode(XmlNodeType.Element, "savedNodeScript", "");
                    XmlNode n;
                    n = doc.CreateNode(XmlNodeType.Element, "scriptProgLang", "");
                    n.InnerText = scriptProgrammingLanguage;
                    mn.AppendChild(n);
                    n = doc.CreateNode(XmlNodeType.Element, "scriptProgram", "");
                    n.InnerText = justTheScript;
                    mn.AppendChild(n);
                    for (int k = 0; k < assemblyPaths.Count; k++)
                    {
                        n = doc.CreateNode(XmlNodeType.Element, "loadedAssemblyDLL", "");
                        n.InnerText = assemblyPaths[k];
                        mn.AppendChild(n);

                    }

                    n = doc.CreateNode(XmlNodeType.Element, "namespaceLoad", "");
                    n.InnerText = autoLoadScriptNamespaces.ToString();
                    mn.AppendChild(n);

                    for (int i = 0; i < inputs.Count; i++)
                    {
                        n = doc.CreateNode(XmlNodeType.Element, "input", "");
                        n.InnerText = inputnames[i];
                        mn.AppendChild(n);
                    }

                    for (int i = 0; i < outputs.Count; i++)
                    {
                        n = doc.CreateNode(XmlNodeType.Element, "output", "");
                        n.InnerText = outputnames[i];
                        mn.AppendChild(n);
                    }


                    storedScriptData = mn.OuterXml;

                }


                //utilities
                internal static List<string> getAssemblyNamespaceNames(Assembly myAssembly)
                {
                    //goes through namespaces and types
                    List<string> namespaceNames = new List<string>();

                    foreach (var type in myAssembly.GetTypes())
                    {
                        if (type.IsPublic)
                        {
                            string ns = type.Namespace;
                            if (ns != null && !namespaceNames.Contains(ns) && ns.Contains("<") == false)
                            {
                                namespaceNames.Add(ns);

                            }

                        }
                    }
                    return namespaceNames;
                }
                class ProxyDomain : MarshalByRefObject
                {
                    public Assembly GetAssembly(string assemblyPath)
                    {
                        try
                        {
                            return Assembly.LoadFrom(assemblyPath);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(ex.Message);
                        }
                    }
                }
                public class Stage : MarshalByRefObject
                {
                    public void LoadAssembly(Byte[] data)
                    {
                        Assembly.Load(data);
                    }
                }
                void loadAllAssembliesAndReferencedRecursive(string aP)
                {
                    string aa = aP.Replace(@"\\", @"\");

                    try
                    {


                        Assembly aaa = Assembly.LoadFile(aP);
                        Type toa = aaa.GetTypes()[0];
                        object oa = System.Activator.CreateInstance(toa);
                        var assemblies = AppDomain.CurrentDomain.GetAssemblies();


                        foreach (AssemblyName aaaa in aaa.GetReferencedAssemblies())
                        {
                            var assembly = (from a in assemblies
                                            where a.FullName == aaaa.FullName
                                            select a).SingleOrDefault();
                            if (assembly == null)
                            {
                                try
                                {
                                    //assume gac for now
                                    string saaaa = Assembly.ReflectionOnlyLoad(aaaa.FullName).Location;
                                    loadAllAssembliesAndReferencedRecursive(saaaa);
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                static Assembly LoadFromSavedFolder(object sender, ResolveEventArgs args)
                {
                    foreach (string assemblyPath in myPaths)
                    {

                        string aN = new AssemblyName(args.Name).Name + ".dll";
                        //Console.WriteLine(aN);
                        if (assemblyPath.Contains(aN))
                        {

                            if (File.Exists(assemblyPath) == false) return null;
                            Assembly assembly = Assembly.LoadFrom(assemblyPath);

                            return assembly;
                        }
                    }
                    return null;

                }

            }


            /// <summary>
            /// Loop end module. Nodes upstream are in a loop. 
            /// </summary>
            //public class PUPPILoopEnd: PUPPIModule
            //{
            //    public PUPPILoopEnd(): base()
            //{
            //    name = "Complete Loop";
            //    completeProcessOverride = true;
            //    description = "Loop close.";
            //    inputs.Add(new PUPPIInParameter());
            //    inputs.Add(new PUPPIInParameter());
            //    inputs.Add(new PUPPIInParameter());

            //    inputnames.Add("Iterated Value");
            //    inputnames.Add("Loop Start Feedback");

            //    inputnames.Add("Loop Complete");
            //    outputs.Add(0);
            //    outputnames.Add("Final Value");  

            //}
            //    public override void process_usercode()
            //    {
            //        try
            //        {
            //            bool lc= Convert.ToInt32(inputs[2].module.outputs[inputs[2].outParIndex])>0;
            //            if (lc==true)
            //            {
            //                //loop ended, pass input
            //                outputs[0] = inputs[1].module.outputs[inputs[1].outParIndex];
            //            }
            //            else
            //            {
            //                //iterate
            //                sendUpstreamSignal(1, 0, inputs[0].module.outputs[inputs[0].outParIndex]); 
            //            }

            //        }
            //        catch
            //        {
            //            outputs[0] = "Error"; 
            //        }
            //    }
            //}
            /// <summary>
            /// Loop start module. Nodes downstream are in a loop.
            /// </summary>
            //public class PUPPILoopStart : PUPPIModule
            //{
            //    public PUPPILoopStart()
            //        : base()
            //    {
            //        name = "Start Loop";
            //        completeProcessOverride = true;
            //        description = "Loop start.";

            //        inputs.Add(new PUPPIInParameter());
            //        inputnames.Add("Input to iterate");

            //        outputnames.Add("Iterated Input");
            //        outputs.Add("nothing");

            //        outputs.Add("to loop end");
            //        outputnames.Add("Loop End Feedback");





            //    }
            //    public override void process_usercode()
            //    {
            //        try
            //        {
            //            outputs[0] = inputs[0].module.outputs[inputs[0].outParIndex];
            //            outputs[1] = "processing";

            //        }
            //        catch
            //        {
            //            outputs[1] = "Error";
            //            outputs[0] = "Error";
            //        }
            //    }
            //}
        }
        namespace DataExchangeModules
        {
            /// <summary>
            /// The <see cref="DataExchangeModules"/> namespace contains PUPPI modules for interoperability.
            /// </summary>

            [System.Runtime.CompilerServices.CompilerGenerated]
            class NamespaceDoc
            {
            }
            /// <summary>
            /// Converts an instance of a class or a value type into another type. All class members are assumed value type so it only drills one level deep for now
            /// </summary>
            public class PUPPIShallowInteroperability : PUPPIModule
            {
                internal List<string> valtypeNames;
                internal List<string> valtypeMatches;
                //internal List<string> classtypeNames;
                //internal List<string> classtypeMatches;
                public PUPPIShallowInteroperability()
                    : base()
                {
                    name = "Shallow Interop";
                    description = "Converts an instance of a class or a value type into another type. All class members are assumed value type so it only drills one level deep for now";
                    doubleClickDescription = "Double click on nod to generate field name mapping";
                    completeDoubleClickOverride = true;
                    outputs.Add("not set");
                    outputnames.Add("Result");
                    inputnames.Add("Object");
                    inputnames.Add("Convert To");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    valtypeNames = new List<string>();
                    valtypeMatches = new List<string>();
                    //classtypeNames = new List<string>();
                    //classtypeMatches = new List<string>();  

                }
                //save mapping
                public override string saveSettings()
                {

                    XmlDocument doc = new XmlDocument();

                    XmlNode vcm;

                    vcm = doc.CreateNode(XmlNodeType.Element, "typeMapping", "");

                    for (int nc = 0; nc < valtypeNames.Count; nc++)
                    {

                        XmlNode vme = doc.CreateNode(XmlNodeType.Element, "valueMap", "");
                        XmlNode st = doc.CreateNode(XmlNodeType.Element, "valueTypeName", "");
                        st.InnerText = valtypeNames[nc];
                        vme.AppendChild(st);
                        XmlNode dt = doc.CreateNode(XmlNodeType.Element, "valueTypeMatch", "");
                        dt.InnerText = valtypeMatches[nc];
                        vme.AppendChild(dt);
                        vcm.AppendChild(vme);
                    }
                    doc.AppendChild(vcm);
                    return vcm.OuterXml;

                }
                //read mapping
                public override void initOnLoad(string savedSettings)
                {
                    XmlDocument doc = new XmlDocument();

                    doc.LoadXml(savedSettings);
                    XmlNodeList nodes = doc.DocumentElement.SelectNodes("/typeMapping/valueMap");

                    foreach (XmlNode mynode in nodes)
                    {
                        foreach (XmlNode childnode in mynode.ChildNodes)
                        {
                            if (childnode.Name == "valueTypeName")
                            {
                                string tName = childnode.InnerText;
                                valtypeNames.Add(tName);
                            }
                            if (childnode.Name == "valueTypeMatch")
                            {
                                string vName = childnode.InnerText;
                                valtypeMatches.Add(vName);
                            }

                        }
                    }
                }

                public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
                {
                    PUPPI.InteropMatch pi = new PUPPI.InteropMatch();
                    //set initial values in case we cancel
                    foreach (string s in valtypeNames)
                    {
                        pi.oVTN.Add(s);
                    }
                    foreach (string s in valtypeMatches)
                    {
                        pi.oVTM.Add(s);
                    }
                    pi.ShowDialog();
                    valtypeNames.Clear();
                    valtypeMatches.Clear();
                    foreach (string s in pi.dVTN)
                    {
                        valtypeNames.Add(s);
                    }
                    foreach (string s in pi.dVTM)
                    {
                        valtypeMatches.Add(s);
                    }

                }

                public override void process_usercode()
                {
                    if (usercodeinputs[0] != null && usercodeinputs[1] != null)
                    {
                        object tc = usercodeinputs[0];
                        object ct = usercodeinputs[1];
                        usercodeoutputs[0] = interme(tc, ct);
                    }
                }
                internal object interme(object toConvert, object ct)
                {
                    Type toCty = toConvert.GetType();
                    Type ctTy = ct.GetType();
                    object oo = null;

                    //List<FieldInfo> cTff = ctTy.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).ToList();
                    //List<FieldInfo> tCff = toCty.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).ToList();

                    List<PropertyInfo> cTff = ctTy.GetProperties().ToList();
                    List<PropertyInfo> tCff = toCty.GetProperties().ToList();


                    //type mismatch
                    if ((cTff.Count == 0 && tCff.Count != 0) || (cTff.Count != 0 && tCff.Count == 0))
                        return oo;
                    //value types but not structs
                    if ((cTff.Count == 0 && tCff.Count == 0) && toCty.IsValueType && ctTy.IsValueType)
                    {
                        try
                        {
                            oo = Convert.ChangeType(toConvert, ctTy);
                        }
                        catch
                        {
                            oo = null;
                        }
                        return oo;
                    }

                    //now we know both have fields but they can be refernece types-class or value types - struct

                    //if (!toCty.IsValueType  )
                    //{
                    //create new instance from type to convert to
                    object oao = System.Activator.CreateInstance(ctTy);
                    //look through fields
                    foreach (PropertyInfo fafa in tCff)
                    {

                        string ts = "";
                        //clean name
                        string tcs = fafa.Name.Replace("<", "");
                        char[] spl = { '>' };
                        tcs = tcs.Split(spl)[0];

                        if (valtypeNames.Contains(tcs))
                        {
                            ts = valtypeMatches[valtypeNames.IndexOf(tcs)];

                        }
                        else
                        {
                            ts = tcs;
                        }
                        //match with fields in type t convert to
                        foreach (PropertyInfo fa in cTff)
                        {
                            //clean name
                            string cts = fa.Name.Replace("<", "");
                            cts = cts.Split(spl)[0];
                            if (cts == ts)
                            {

                                bool success = true;
                                try
                                {
                                    fa.SetValue(oao, Convert.ChangeType(fafa.GetValue(toConvert), fa.PropertyType));
                                }
                                catch (Exception exy)
                                {
                                    success = false;
                                }
                                if (success) break;
                            }
                        }

                    }
                    return oao;
                    //}
                    //else
                    //{ 


                    //}
                }

            }
            /// <summary>
            /// Gets the value of an object property by name.
            /// </summary>
            public class PUPPIGetPropertyByName : PUPPIModule
            {
                public PUPPIGetPropertyByName()
                    : base()
                {
                    name = "Get Property By Name";
                    description = "Gets the value of the named property of an object.Right click node outputs for properties.";
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Object");
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Property Name");
                    outputs.Add("not set");
                    outputnames.Add("Prop Value");
                }
                public override void process_usercode()
                {
                    try
                    {
                        object ooo = usercodeinputs[0];
                        if (ooo == null) return;
                        string s = usercodeinputs[1].ToString().Replace(" ", "");
                        if (s == "") return;
                        List<PropertyInfo> mps = ooo.GetType().GetProperties().ToList();
                        foreach (PropertyInfo pinfo in mps)
                        {
                            if (pinfo.Name == s)
                            {

                                usercodeoutputs[0] = pinfo.GetValue(ooo, null);
                                return;
                            }
                        }
                        usercodeoutputs[0] = "Prop not found";
                    }
                    catch (Exception exy)
                    {
                        usercodeoutputs[0] = exy.ToString();
                    }
                }
            }
            /// <summary>
            /// Sets the value of an object property by name.
            /// </summary>
            public class PUPPISetPropertyByName : PUPPIModule
            {
                public PUPPISetPropertyByName()
                    : base()
                {
                    name = "Set Property By Name";
                    description = "Sets the value of the named property of an object.Returns clone of object with new prop value.Right click node outputs for properties.";
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Object");
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Property Name");

                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Property Value");

                    outputs.Add("not set");
                    outputnames.Add("New Object");
                }
                public override void process_usercode()
                {
                    try
                    {
                        object ooo = usercodeinputs[0];
                        if (ooo == null) return;
                        string s = usercodeinputs[1].ToString().Replace(" ", "");
                        if (s == "") return;
                        object value = usercodeinputs[2];

                        object cco = DeepCopy(ooo, 0);
                        if (cco == null)
                        {
                            usercodeoutputs[0] = "Failed clone instance";
                            return;
                        }

                        List<PropertyInfo> mps = cco.GetType().GetProperties().ToList();
                        foreach (PropertyInfo pinfo in mps)
                        {
                            if (pinfo.Name == s)
                            {

                                pinfo.SetValue(cco, value);
                                usercodeoutputs[0] = cco;
                                return;
                            }
                        }
                        usercodeoutputs[0] = "Prop not found";


                    }
                    catch (Exception exy)
                    {
                        usercodeoutputs[0] = exy.ToString();
                    }
                }
            }

            public class InputPUPPIClassProperties : PUPPIModule
            {
                string classname = "";
                bool run = false;
                public InputPUPPIClassProperties():base()
                {
                    name = "Input Class Properties";
                    description = "Generates a new object from input object, with all property setter and getter methods";
                    outputs.Add(null);
                    outputnames.Add("New Obj.");
                    PUPPIInParameter ti = new PUPPIInParameter();
                    ti.isoptional = true;
                    inputs.Add(ti);
                    inputnames.Add("Example Obj");
                    canBeInContainer = false;
                    classname = "";
                    run = false;
                }


                public override void process_usercode()
                {

                    if (inputs[0].module == null)
                    {
                        
                        return;
                    }
                    object oo = usercodeinputs[0];//inputs[0].module.outputs[inputs[0].outParIndex];
                        object templater = null;
                        if (oo != null)
                        {

                                templater = oo;
                            //}
                            if (templater == null) return;

                        }
                        else
                        {
                            canBeInContainer = false;
                            return;
                        }

                    if (inputs.Count == 1)
                    {
                        //populate inputs and outputs
                        repop(templater.GetType());
                        //need to rerun
                        if (classname != "")

                            setNodeCaption(classname + " Fields");

                        if (run == false)
                        {
                            run = true;
                            addCanvasCommand("runcanvasprogramcommand");
                        }
                        //i guess no fields
                        else
                        {
                            usercodeoutputs[0] = templater;
                        }


                    }
                    else
                    {
                        if (classname == "") return;
                       
                        Object os = Utilities.CloneObject(templater);//null;
                      
                        if (os==null)
                        {
                            usercodeoutputs[0] = "Invalid class";
                            return;
                        }
                        //run it
                        List<PropertyInfo> mps = os.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToList();
                        int inputC = 1;
                        foreach (PropertyInfo pinfo in mps)
                        {

                            MethodInfo setMethod = pinfo.GetSetMethod();

                            if (setMethod != null) 
                            {
                                if (usercodeinputs[inputC]!=null)
                                pinfo.SetValue(os, usercodeinputs[inputC]);
                                inputC++;
                            }
                            
                        }
                        //changed object
                        usercodeoutputs[0] = os;
                        int outputC=1;
                        foreach (PropertyInfo pinfo in mps)
                        {
                            MethodInfo getMethod = pinfo.GetGetMethod();
                            if (getMethod != null)
                            {
                                usercodeoutputs[outputC] = pinfo.GetValue(os);
                                outputC++;
                            }
                        }
                    }
                   


                }
                
                

                internal void repop(Type t)
                {
                    resetModuleNumberCalls();
                    List<PropertyInfo> mps = t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToList();
                    classname = t.FullName;//AssemblyQualifiedName;
                    
                    foreach (PropertyInfo pinfo in mps)
                    {
                         MethodInfo setMethod = pinfo.GetSetMethod();

                         if (setMethod != null)
                         {
                             PUPPIInParameter ti = new PUPPIInParameter();
                             ti.isoptional = true;
                             inputs.Add(ti);
                             inputnames.Add("Set " + pinfo.Name);
                         }
                        MethodInfo getMethod = pinfo.GetGetMethod();
                        if (getMethod != null)
                        {
                            outputs.Add(null);
                            outputnames.Add("Get " + pinfo.Name);
                        }
                    }
                    canBeInContainer = true;
                }

                //save how many inputs we have
                public override string saveSettings()
                {
                    return classname; 
                }
                public override void initOnLoad(string savedSettings)
                {
                    classname = savedSettings;  
                    if (classname != "")
                    {


                        Type myclass = AutomaticPUPPImodulesCreator.findTypeByName(savedSettings);
                        // Type myclass = Type.GetType (savedSettings);
                        if (myclass!=null)
                        {
                            repop(myclass); 
                        }
                    }
 
                }
              

            }

            public class InputPUPPIClassFields : PUPPIModule
            {
                string classname = "";
                bool run = false;
                public InputPUPPIClassFields()
                    : base()
                {
                    name = "Input Class Fields";
                    description = "Generates a new object from input object, with all fields (variables) that can be set or retrieved";
                    outputs.Add(null);
                    outputnames.Add("New Obj.");
                    PUPPIInParameter ti = new PUPPIInParameter();
                    ti.isoptional = true;
                    inputs.Add(ti);
                    inputnames.Add("Example Obj");
                    canBeInContainer = false;
                    classname = "";
                    run = false;
                }


                public override void process_usercode()
                {

                    if (inputs[0].module == null)
                    {

                        return;
                    }
                    object oo = usercodeinputs[0]; //inputs[0].module.outputs[inputs[0].outParIndex];
                    object templater = null;
                    if (oo != null)
                    {

                      templater = Utilities.CloneObject(oo);
                    }
                    else
                    {
                        canBeInContainer = false;
                        return;
                    }

                    if (inputs.Count == 1)
                    {

                       
                        //populate inputs and outputs
                        repop(templater.GetType());
                        //need to rerun
                        if (classname!="")
                            
                                setNodeCaption(classname +" Fields");

                        if (run==false)
                        {
                            run = true;
                            addCanvasCommand("runcanvasprogramcommand");
                        }
                            //i guess no fields
                        else
                        {
                            usercodeoutputs[0] = templater;
                        }

                           

                    }
                    else
                    {
                        if (classname == "") return;
                       
                        Object os = templater;//null;
                       
                        //run it
                        List<FieldInfo> mps = os.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToList();//.Where(f => f.IsPublic)
                        int inputC = 1;
                        foreach (FieldInfo pinfo in mps)
                        {
                                                                 
                                if (usercodeinputs[inputC] != null)
                                    
                                    pinfo.SetValue(os, usercodeinputs[inputC]);
                                inputC++;
                            
                        }
                        //changed object
                        usercodeoutputs[0] = os;
                        int outputC = 1;
                        foreach (FieldInfo pinfo in mps)
                        {
                           
                                usercodeoutputs[outputC] = pinfo.GetValue(os);
                                outputC++;
                            
                        }
                    }


                }



                internal void repop(Type t)
                {
                    resetModuleNumberCalls();

                    List<FieldInfo> mps = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToList();
                    
                    //List < FieldInfo > mps = t.GetFields().Where(f => f.IsPublic).ToList();
                    classname = t.FullName;//.Name;///.AssemblyQualifiedName;
                                        foreach (FieldInfo pinfo in mps)
                    {
                        
                            PUPPIInParameter ti = new PUPPIInParameter();
                            ti.isoptional = true;
                            inputs.Add(ti);
                            inputnames.Add("Set " + pinfo.Name);
                           
                            outputs.Add(null);
                            outputnames.Add("Get " + pinfo.Name);
                        
                    }
                    canBeInContainer = true;
                }

                //save how many inputs we have
                public override string saveSettings()
                {
                    return classname;
                }
                public override void initOnLoad(string savedSettings)
                {
                    classname = savedSettings;
                    if (classname != "")
                    {

                        Type myclass = PUPPIModel.AutomaticPUPPImodulesCreator.findTypeByName(savedSettings);
                        if (myclass != null)
                        {
                            repop(myclass);
                        }
                    }

                }


            }

            /// <summary>
            /// Converts variable to string. So you don't have to include ToString method for everything.
            /// </summary>
            public class PUPPIVariableToString : PUPPIModule
            {
                public PUPPIVariableToString()
                    : base()
                {
                    name = "Variable to String";
                    description = "Converts any variable/object to string.Some automatically generated modules need specific variable type as input.";

                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Variable");
                    outputs.Add("not set");
                    outputnames.Add("String");
                }
                public override void process_usercode()
                {
                    try
                    {
                        usercodeoutputs[0] = usercodeinputs[0].ToString();
                    }
                    catch
                    {
                        usercodeoutputs[0] = "error";
                    }
                }

            }
            /// <summary>
            /// Converts and enumerable or collection (Lis, ArrayList etc) into a string with a specified separator between elements.
            /// </summary>
            public class PUPPIListToString : PUPPIModule
            {
                public PUPPIListToString()
                    : base()
                {
                    name = "List to String ";
                    description = "Converts typed or generic list to string placing separator between list elements.";
                    outputs.Add("null");
                    outputnames.Add("String");

                    inputnames.Add("List");
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Sep. String");
                    inputs.Add(new PUPPIInParameter());

                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {
                    ArrayList myo = new ArrayList();
                    if (inputs[0].module == null || inputs[1].module == null)
                    {
                        outputs[0] = "null";
                        return;
                    }
                    object ol = inputs[0].module.outputs[inputs[0].outParIndex];
                    object os = inputs[1].module.outputs[inputs[1].outParIndex];
                    if (ol == null || os == null)
                    {
                        outputs[0] = "null";
                        return;
                    }
                    string ss = Convert.ToString(os);
                    //if (ol.GetType() != typeof(string))
                    //{
                    //if (ol is ICollection)
                    //{
                    //    myo = new ArrayList(o  as ICollection);

                    //}
                    //else if (ol is IEnumerable)
                    //{
                    //    myo = PUPPIModule.makeMeAnArrayList(ol  as IEnumerable);
                    //}
                    if (ol is IEnumerable)
                    {
                        myo = makeCollOrEnumIntoArrayList(ol);
                    }
                    else
                    {
                        outputs[0] = "null";
                        return;
                    }

                    if (myo.Count == 0)
                    {
                        outputs[0] = "null";
                        return;
                    }
                    string re = "";
                    for (int i = 0; i < myo.Count; i++)
                    {
                        re += myo[i].ToString();
                        if (i < myo.Count - 1)
                            re += ss;
                    }
                    outputs[0] = re;
                    //}
                    //    //already a string
                    //else
                    //{
                    //    outputs[0] = ol; 
                    //}


                }
            }

            /// <summary>
            /// Converts variable to integer.
            /// </summary>
            public class PUPPIVariableToInteger : PUPPIModule
            {
                public PUPPIVariableToInteger()
                    : base()
                {
                    name = "Variable to Integer";
                    description = "Converts any numeric style variable to integer. Decimals removed. Some automatically generated modules need specific variable type as input.";
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Variable");
                    outputs.Add("not set");
                    outputnames.Add("Integer");
                }
                public override void process_usercode()
                {
                    try
                    {
                        if (usercodeinputs[0] is Boolean)
                        {
                            if ((bool)usercodeinputs[0] == true)
                            {
                                usercodeoutputs[0] = 1;
                            }
                            else
                            {
                                usercodeoutputs[0] = 0;
                            }
                            return;
                        }

                        string sasa = usercodeinputs[0].ToString();
                        double dp = double.Parse(sasa);
                        usercodeoutputs[0] = Convert.ToInt32(dp);
                    }
                    catch
                    {
                        usercodeoutputs[0] = "conversion error";
                    }
                }

            }

            internal class receivedData
            {
                public string text;
                public byte[] data;
                public string savedFilePath;
                public object obj;
            }
            internal class CStateObject
            {
                // Client socket.
                public Socket workSocket = null;
                // Size of receive buffer.
                public const int BufferSize = 8092;
                // Receive buffer.
                public byte[] buffer = new byte[BufferSize];
                // Received data string.
                public StringBuilder sb = new StringBuilder();
                //received data bytes
                public byte[] dataReceived = null;
            }




            internal static class AsynchronousClient
            {
                // The port number for the remote device.
                private static int port = 11000;

                private static string spassword = "";
                //to save file
                internal static string filex = "nout";
                internal static string nid = "nonode";
                // ManualResetEvent instances signal completion.
                private static ManualResetEvent connectDone =
                    new ManualResetEvent(false);
                private static ManualResetEvent sendDone =
                    new ManualResetEvent(false);
                private static ManualResetEvent receiveDone =
                    new ManualResetEvent(false);

                // The response from the remote device.
                private static String response = String.Empty;
                private static byte[] binaryData = null;
                private static String filePath = String.Empty;
                private static object retrievedObject = null;
                static IPHostEntry ipHostInfo;
                static IPAddress ipAddress;
                static IPEndPoint remoteEP;
                internal static int timeoutVal;

                internal static void initializeClient(string serverIPAddress, int connectionPort, string password, int tov)
                {
                    // Establish the remote endpoint for the socket.

                    // ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                    // ipAddress = ipHostInfo.AddressList[6];
                    ipAddress = IPAddress.Parse(serverIPAddress);
                    port = connectionPort;
                    remoteEP = new IPEndPoint(ipAddress, port);
                    spassword = password;
                    timeoutVal = tov;
                    if (timeoutVal <= 0) timeoutVal = 60000;

                }


                internal static receivedData sendServerCommand(string myCommand)
                {
                    // Connect to a remote device.
                    try
                    {


                        // client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true); 
                        // Connect to the remote endpoint.


                        using (Socket client = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream, ProtocolType.Tcp))
                        {
                            filePath = String.Empty;
                            response = String.Empty;
                            binaryData = null;
                            receivedData rd = new receivedData();
                            connectDone = new ManualResetEvent(false);
                            sendDone = new ManualResetEvent(false);
                            receiveDone = new ManualResetEvent(false);

                            client.BeginConnect(remoteEP,
                                new AsyncCallback(ConnectCallback), client);
                            bool connectSuccessfully = connectDone.WaitOne(timeoutVal);
                            if (connectSuccessfully == false)
                            {
                                rd.text = "timeout";
                                rd.savedFilePath = "nofile";
                                rd.data = null;
                                rd.obj = null;
                                try
                                {
                                    client.Shutdown(SocketShutdown.Both);
                                    client.Close();
                                }
                                catch
                                {

                                }
                                return rd;
                            }



                            // Send test data to the remote device.
                            Send(client, myCommand + spassword + "<EOF>");
                            bool sentSuccessfully = sendDone.WaitOne(timeoutVal);
                            if (sentSuccessfully == false)
                            {
                                rd.text = "timeout";
                                rd.savedFilePath = "nofile";
                                rd.data = null;
                                rd.obj = null;
                                try
                                {
                                    client.Shutdown(SocketShutdown.Both);
                                    client.Close();
                                }
                                catch
                                {

                                }
                                return rd;
                            }

                            // Receive the response from the remote device.
                            Receive(client);
                            bool receivedSuccessfully = receiveDone.WaitOne(timeoutVal);
                            if (receivedSuccessfully == false)
                            {
                                rd.text = "timeout";
                                rd.savedFilePath = "nofile";
                                rd.data = null;
                                rd.obj = null;
                                try
                                {
                                    client.Shutdown(SocketShutdown.Both);
                                    client.Close();
                                }
                                catch
                                {

                                }
                                return rd;
                            }

                            // Release the socket.
                            client.Shutdown(SocketShutdown.Both);
                            client.Close();
                            rd.text = response;
                            rd.savedFilePath = filePath;
                            rd.data = binaryData;
                            rd.obj = retrievedObject;
                            return rd;

                        }



                    }
                    catch (Exception e)
                    {
                        receivedData rd = new receivedData();
                        rd.text = "error";
                        rd.savedFilePath = "nofile";
                        rd.data = null;
                        rd.obj = null;
                        return rd;
                    }
                }

                private static void ConnectCallback(IAsyncResult ar)
                {
                    try
                    {
                        // Retrieve the socket from the state object.
                        Socket client = (Socket)ar.AsyncState;

                        // Complete the connection.
                        client.EndConnect(ar);

                        // Signal that the connection has been made.
                        connectDone.Set();
                    }
                    catch (Exception e)
                    {
                        //  Console.WriteLine(e.ToString());
                    }
                }

                private static void Receive(Socket client)
                {
                    try
                    {
                        // Create the state object.
                        CStateObject state = new CStateObject();
                        state.workSocket = client;

                        // Begin receiving the data from the remote device.
                        client.BeginReceive(state.buffer, 0, CStateObject.BufferSize, 0,
                            new AsyncCallback(ReceiveCallback), state);
                    }
                    catch (Exception e)
                    {

                    }
                }
                private static Object ByteArrayToObject(byte[] arrBytes)
                {
                    System.IO.MemoryStream memStream = new System.IO.MemoryStream();
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binForm = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    memStream.Write(arrBytes, 0, arrBytes.Length);
                    memStream.Seek(0, System.IO.SeekOrigin.Begin);
                    Object obj = (Object)binForm.Deserialize(memStream);
                    return obj;
                }

                private static bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
                {
                    try
                    {
                        // Open file for reading
                        System.IO.FileStream _FileStream =
                           new System.IO.FileStream(_FileName, System.IO.FileMode.Create,
                                                    System.IO.FileAccess.Write);
                        // Writes a block of bytes to this stream using data from
                        // a byte array.
                        _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

                        // close file stream
                        _FileStream.Close();

                        return true;
                    }
                    catch (Exception _Exception)
                    {
                        // Error
                        //Console.WriteLine("Exception caught in process: {0}",
                        //                  _Exception.ToString());
                    }

                    // error occured, return false
                    return false;
                }
                private static void ReceiveCallback(IAsyncResult ar)
                {
                    try
                    {
                        // Retrieve the state object and the client socket 
                        // from the asynchronous state object.
                        CStateObject state = (CStateObject)ar.AsyncState;
                        Socket client = state.workSocket;

                        // Read data from the remote device.
                        int bytesRead = client.EndReceive(ar);

                        if (bytesRead > 0)
                        {

                            if (state.dataReceived == null)
                            {
                                state.dataReceived = new byte[0];
                            }
                            byte[] newData = new byte[bytesRead];
                            Array.ConstrainedCopy(state.buffer, 0, newData, 0, bytesRead);
                            // There might be more data, so store the data received so far.
                            // state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                            state.dataReceived = state.dataReceived.Concat(newData).ToArray();
                            // Get the rest of the data.
                            client.BeginReceive(state.buffer, 0, CStateObject.BufferSize, 0,
                                new AsyncCallback(ReceiveCallback), state);
                        }
                        else
                        {
                            if (state.dataReceived != null)
                            {
                                // All the data has arrived; put it in response.
                                if (state.dataReceived.Length > 2)
                                {

                                    byte dataTypeFlag = state.dataReceived[0];




                                    byte[] readData = new byte[state.dataReceived.Length - 1];
                                    Array.ConstrainedCopy(state.dataReceived, 1, readData, 0, state.dataReceived.Length - 1);
                                    if (dataTypeFlag == 0)
                                    {
                                        response = System.Text.Encoding.ASCII.GetString(readData);
                                        binaryData = null;
                                        filePath = "nofile";
                                        retrievedObject = null;
                                    }
                                    else if (dataTypeFlag == 2)
                                    {
                                        response = "checkobject";
                                        binaryData = (byte[])readData.Clone();
                                        retrievedObject = ByteArrayToObject(binaryData);
                                        filePath = "nofile";
                                    }
                                    else if (dataTypeFlag == 1)
                                    {
                                        response = "checkfile";
                                        binaryData = (byte[])readData.Clone();
                                        retrievedObject = null;
                                        filePath = System.IO.Path.GetTempPath() + nid + "." + filex;
                                        try
                                        {
                                            bool b = ByteArrayToFile(filePath, binaryData);
                                            if (b == false) filePath = "error";
                                        }
                                        catch
                                        {
                                            filePath = "error";
                                        }

                                    }
                                    //no idea
                                    else
                                    {
                                        response = "unknowntype";
                                        binaryData = (byte[])readData.Clone();
                                        retrievedObject = null;
                                        filePath = "nofile";
                                    }
                                }
                            }
                            // Signal that all bytes have been received.
                            receiveDone.Set();
                        }
                    }
                    catch (Exception e)
                    {
                        // Console.WriteLine(e.ToString());
                    }
                }

                private static void Send(Socket client, String data)
                {
                    // Convert the string data to byte data using ASCII encoding.
                    byte[] byteData = Encoding.ASCII.GetBytes(data);

                    // Begin sending the data to the remote device.
                    client.BeginSend(byteData, 0, byteData.Length, 0,
                        new AsyncCallback(SendCallback), client);
                }

                private static void SendCallback(IAsyncResult ar)
                {
                    try
                    {
                        // Retrieve the socket from the state object.
                        Socket client = (Socket)ar.AsyncState;

                        // Complete sending the data to the remote device.
                        int bytesSent = client.EndSend(ar);
                        //  Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                        // Signal that all bytes have been sent.
                        sendDone.Set();
                    }
                    catch (Exception e)
                    {
                        //   Console.WriteLine(e.ToString());
                    }
                }
            }

            /// <summary>
            /// Retrieves information from a node on a PUPPICAD server
            /// </summary>
            public class PUPPIRemoteNodeOutput : PUPPIModule
            {
                byte sc1 = 2;
                byte sc2 = 87;
                string mps = "";
                string mip = "";
                int mp = 0;
                int timeo = 0;
                public PUPPIRemoteNodeOutput()
                    : base()
                {
                    name = "Remote Output";
                    description = "Gets the object at designated output of designated node on a remote canvas server.";
                    doubleClickDescription = "Set connection parameters";
                    inputnames.Add("Node ID");
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Output Index");
                    inputs.Add(new PUPPIInParameter());
                    outputnames.Add("Object");
                    outputs.Add(null);
                    completeDoubleClickOverride = true;

                }
                public override void process_usercode()
                {
                    AsynchronousClient.filex = "nout";
                    AsynchronousClient.nid = "nonode";
                    int nodeid = -1;
                    int outputid = -1;

                    try
                    {
                        nodeid = Convert.ToInt16(usercodeinputs[0]);
                    }
                    catch
                    {
                        nodeid = -1;
                    }

                    try
                    {
                        outputid = Convert.ToInt16(usercodeinputs[1]);
                    }
                    catch
                    {
                        outputid = -1;
                    }

                    if (nodeid < 0 || outputid < 0)
                    {
                        usercodeoutputs[0] = "Invalid node/output id";
                        return;
                    }

                    try
                    {
                        AsynchronousClient.initializeClient(mip, mp, mps, timeo);
                    }
                    catch (Exception exy)
                    {
                        usercodeoutputs[0] = "Client init error.";
                        return;

                    }
                    receivedData rd = AsynchronousClient.sendServerCommand("getnodeoutputvalue_|_" + nodeid.ToString() + "_|_" + outputid.ToString());
                    usercodeoutputs[0] = rd.obj;


                }

                public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
                {
                    PUPPI.remoteClient rec = new PUPPI.remoteClient();
                    rec.pps = mps;
                    rec.ips = mip;
                    rec.prts = mp.ToString();
                    rec.tms = timeo.ToString();
                    rec.ShowDialog();
                    try
                    {
                        mps = rec.pps;
                        mip = rec.ips;
                        mp = Convert.ToInt16(rec.prts);
                        timeo = Convert.ToInt32(rec.tms);
                    }
                    catch
                    {
                        MessageBox.Show("Error setting parameters. Clearing");
                        mps = "";
                        mip = "";
                        mp = 0;
                        timeo = 0;
                    }
                }

                public override string saveSettings()
                {
                    byte[] PtoBytes = Encoding.ASCII.GetBytes(mps);
                    byte[] TPtoBytes = new byte[PtoBytes.Length * 2];
                    int j = 0;
                    for (int i = 0; i < PtoBytes.Length; i++)
                    {
                        int newb = PtoBytes[i] + sc1 + sc2;
                        if (newb > 126)
                        {
                            byte tbt = Convert.ToByte(newb - 126);
                            if (tbt < 32)
                            {
                                TPtoBytes[j] = Convert.ToByte(tbt + 32);
                                j++;
                                TPtoBytes[j] = Convert.ToByte(124);
                                j++;
                            }
                            else
                            {
                                TPtoBytes[j] = Convert.ToByte(tbt);
                                j++;
                                TPtoBytes[j] = Convert.ToByte(125);
                                j++;
                            }
                        }
                        else
                        {
                            TPtoBytes[j] = Convert.ToByte(newb);
                            j++;
                            TPtoBytes[j] = Convert.ToByte(126);
                            j++;
                        }
                    }
                    TPtoBytes = TPtoBytes.Reverse().ToArray();
                    string smps = Encoding.ASCII.GetString(TPtoBytes);

                    return smps + "_|_|_" + mip + "_|_|_" + mp.ToString() + "_|_|_" + timeo.ToString();
                }

                public override void initOnLoad(string savedSettings)
                {
                    try
                    {
                        string[] seppa = { "_|_|_" };
                        string[] splitta = savedSettings.Split(seppa, StringSplitOptions.None);
                        mp = Convert.ToInt16(splitta[2]);
                        timeo = Convert.ToInt32(splitta[3]);
                        mip = splitta[1];
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
                        mps = Encoding.ASCII.GetString(bita);
                    }
                    catch
                    {
                        mps = "";
                        mip = "error";
                        mp = 0;
                        timeo = 0;
                    }
                }




            }


            /// <summary>
            /// Sets a node output on remote PUPPICanvas. Can be string, double or integer, or lists containing one of those types of values.
            /// </summary>
            public class PUPPISetRemoteNodeOutput : PUPPIModule
            {
                byte sc1 = 2;
                byte sc2 = 87;
                string mps = "";
                string mip = "";
                int mp = 0;
                int timeo = 0;
                public PUPPISetRemoteNodeOutput()
                    : base()
                {
                    name = "Remote Set Output";
                    description = " Sets a node output on remote PUPPICanvas. Can be string, double or integer, or lists containing one of those types of values.";
                    doubleClickDescription = "Set connection parameters";
                    inputnames.Add("Value");
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Node ID");
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Output Index");
                    inputs.Add(new PUPPIInParameter());

                    outputnames.Add("Execution");
                    outputs.Add("");
                    completeDoubleClickOverride = true;
                }
                public override void process_usercode()
                {


                    int nodeid = -1;
                    int outputid = -1;

                    try
                    {
                        nodeid = Convert.ToInt16(usercodeinputs[1]);
                    }
                    catch
                    {
                        nodeid = -1;
                    }

                    try
                    {
                        outputid = Convert.ToInt16(usercodeinputs[2]);
                    }
                    catch
                    {
                        outputid = -1;
                    }

                    if (nodeid < 0 || outputid < 0)
                    {
                        usercodeoutputs[0] = "Invalid node/output id";
                        return;
                    }
                    try
                    {
                        DataExchangeModules.AsynchronousClient.initializeClient(mip, mp, mps, timeo);
                    }
                    catch (Exception exy)
                    {
                        usercodeoutputs[0] = "Client init error.";
                        return;

                    }
                    if (usercodeinputs[0] is IEnumerable && !(usercodeinputs[0] is string))
                    {
                        ArrayList orlist = new ArrayList();
                        orlist = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]);
                        if (orlist.Count > 0)
                        {
                            string setme = "";
                            for (int i = 0; i < orlist.Count; i++)
                            {
                                setme += orlist[i].ToString();
                                if (i < orlist.Count - 1)
                                    setme += ",";
                            }

                            if (orlist[0] is string)
                            {
                                DataExchangeModules.receivedData rd = DataExchangeModules.AsynchronousClient.sendServerCommand("setoutputliststring_|_" + nodeid.ToString() + "_|_" + outputid.ToString() + "_|_" + setme);
                                usercodeoutputs[0] = rd.text;

                            }
                            else if (orlist[0] is int)
                            {
                                DataExchangeModules.receivedData rd = DataExchangeModules.AsynchronousClient.sendServerCommand("setoutputlistinteger_|_" + nodeid.ToString() + "_|_" + outputid.ToString() + "_|_" + setme);
                                usercodeoutputs[0] = rd.text;

                            }
                            else if (orlist[0] is double)
                            {
                                DataExchangeModules.receivedData rd = DataExchangeModules.AsynchronousClient.sendServerCommand("setoutputlistdouble_|_" + nodeid.ToString() + "_|_" + outputid.ToString() + "_|_" + setme);
                                usercodeoutputs[0] = rd.text;

                            }
                            else
                            {
                                usercodeoutputs[0] = "Invalid list data type. ";
                                return;
                            }
                        }
                    }
                    else
                    {

                        if (usercodeinputs[0] is string)
                        {
                            DataExchangeModules.receivedData rd = DataExchangeModules.AsynchronousClient.sendServerCommand("setoutputstring_|_" + nodeid.ToString() + "_|_" + outputid.ToString() + "_|_" + usercodeinputs[0].ToString());
                            usercodeoutputs[0] = rd.text;
                        }
                        else if (usercodeinputs[0] is int)
                        {
                            DataExchangeModules.receivedData rd = DataExchangeModules.AsynchronousClient.sendServerCommand("setoutputinteger_|_" + nodeid.ToString() + "_|_" + outputid.ToString() + "_|_" + usercodeinputs[0].ToString());
                            usercodeoutputs[0] = rd.text;
                        }
                        else if (usercodeinputs[0] is double)
                        {
                            DataExchangeModules.receivedData rd = DataExchangeModules.AsynchronousClient.sendServerCommand("setoutputdouble_|_" + nodeid.ToString() + "_|_" + outputid.ToString() + "_|_" + usercodeinputs[0].ToString());
                            usercodeoutputs[0] = rd.text;
                        }
                        else
                        {
                            usercodeoutputs[0] = "invalid data type";
                        }

                    }







                }

                public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
                {
                    PUPPI.remoteClient rec = new PUPPI.remoteClient();
                    rec.pps = mps;
                    rec.ips = mip;
                    rec.prts = mp.ToString();
                    rec.tms = timeo.ToString();
                    rec.ShowDialog();
                    try
                    {
                        mps = rec.pps;
                        mip = rec.ips;
                        mp = Convert.ToInt16(rec.prts);
                        timeo = Convert.ToInt32(rec.tms);
                    }
                    catch
                    {
                        MessageBox.Show("Error setting parameters. Clearing");
                        mps = "";
                        mip = "";
                        mp = 0;
                        timeo = 0;
                    }
                }

                public override string saveSettings()
                {
                    byte[] PtoBytes = Encoding.ASCII.GetBytes(mps);
                    byte[] TPtoBytes = new byte[PtoBytes.Length * 2];
                    int j = 0;
                    for (int i = 0; i < PtoBytes.Length; i++)
                    {
                        int newb = PtoBytes[i] + sc1 + sc2;
                        if (newb > 126)
                        {
                            byte tbt = Convert.ToByte(newb - 126);
                            if (tbt < 32)
                            {
                                TPtoBytes[j] = Convert.ToByte(tbt + 32);
                                j++;
                                TPtoBytes[j] = Convert.ToByte(124);
                                j++;
                            }
                            else
                            {
                                TPtoBytes[j] = Convert.ToByte(tbt);
                                j++;
                                TPtoBytes[j] = Convert.ToByte(125);
                                j++;
                            }
                        }
                        else
                        {
                            TPtoBytes[j] = Convert.ToByte(newb);
                            j++;
                            TPtoBytes[j] = Convert.ToByte(126);
                            j++;
                        }
                    }
                    TPtoBytes = TPtoBytes.Reverse().ToArray();
                    string smps = Encoding.ASCII.GetString(TPtoBytes);

                    return smps + "_|_|_" + mip + "_|_|_" + mp.ToString() + "_|_|_" + timeo.ToString();
                }

                public override void initOnLoad(string savedSettings)
                {
                    try
                    {
                        string[] seppa = { "_|_|_" };
                        string[] splitta = savedSettings.Split(seppa, StringSplitOptions.None);
                        mp = Convert.ToInt16(splitta[2]);
                        mip = splitta[1];
                        timeo = Convert.ToInt32(splitta[3]);
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
                        mps = Encoding.ASCII.GetString(bita);
                    }
                    catch
                    {
                        mps = "";
                        mip = "error";
                        mp = 0;
                        timeo = 0;
                    }
                }

                private class StateObject
                {
                    // Client socket.
                    public Socket workSocket = null;
                    // Size of receive buffer.
                    public const int BufferSize = 256;
                    // Receive buffer.
                    public byte[] buffer = new byte[BufferSize];
                    // Received data string.
                    public StringBuilder sb = new StringBuilder();
                    //received data bytes
                    public byte[] dataReceived = null;
                }

            }

            /// <summary>
            /// Runs a canvas program on a PUPPICAD server
            /// </summary>
            public class PUPPIRunRemoteCanvasProgram : PUPPIModule
            {
                byte sc1 = 2;
                byte sc2 = 87;
                string mps = "";
                string mip = "";
                int mp = 0;
                int timeo = 0;
                public PUPPIRunRemoteCanvasProgram()
                    : base()
                {
                    name = "Remote Run";
                    description = " Runs a canvas program on a PUPPICAD server";
                    doubleClickDescription = "Set connection parameters";
                    inputnames.Add("Dummy Input");
                    PUPPIInParameter pi = new PUPPIInParameter();
                    pi.isoptional = true;
                    inputs.Add(pi);
                    outputnames.Add("Execution");
                    outputs.Add("");
                    completeDoubleClickOverride = true;
                }
                public override void process_usercode()
                {




                    try
                    {
                        DataExchangeModules.AsynchronousClient.initializeClient(mip, mp, mps, timeo);
                    }
                    catch (Exception exy)
                    {
                        usercodeoutputs[0] = "Client init error.";
                        return;

                    }

                    DataExchangeModules.receivedData rd = DataExchangeModules.AsynchronousClient.sendServerCommand("runcanvasprogramcommand");

                    usercodeoutputs[0] = rd.text;


                }

                public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
                {
                    PUPPI.remoteClient rec = new PUPPI.remoteClient();
                    rec.pps = mps;
                    rec.ips = mip;
                    rec.prts = mp.ToString();
                    rec.tms = timeo.ToString();
                    rec.ShowDialog();
                    try
                    {
                        mps = rec.pps;
                        mip = rec.ips;
                        mp = Convert.ToInt16(rec.prts);
                        timeo = Convert.ToInt32(rec.tms);
                    }
                    catch
                    {
                        MessageBox.Show("Error setting parameters. Clearing");
                        mps = "";
                        mip = "";
                        mp = 0;
                        timeo = 0;
                    }
                }

                public override string saveSettings()
                {
                    byte[] PtoBytes = Encoding.ASCII.GetBytes(mps);
                    byte[] TPtoBytes = new byte[PtoBytes.Length * 2];
                    int j = 0;
                    for (int i = 0; i < PtoBytes.Length; i++)
                    {
                        int newb = PtoBytes[i] + sc1 + sc2;
                        if (newb > 126)
                        {
                            byte tbt = Convert.ToByte(newb - 126);
                            if (tbt < 32)
                            {
                                TPtoBytes[j] = Convert.ToByte(tbt + 32);
                                j++;
                                TPtoBytes[j] = Convert.ToByte(124);
                                j++;
                            }
                            else
                            {
                                TPtoBytes[j] = Convert.ToByte(tbt);
                                j++;
                                TPtoBytes[j] = Convert.ToByte(125);
                                j++;
                            }
                        }
                        else
                        {
                            TPtoBytes[j] = Convert.ToByte(newb);
                            j++;
                            TPtoBytes[j] = Convert.ToByte(126);
                            j++;
                        }
                    }
                    TPtoBytes = TPtoBytes.Reverse().ToArray();
                    string smps = Encoding.ASCII.GetString(TPtoBytes);

                    return smps + "_|_|_" + mip + "_|_|_" + mp.ToString() + "_|_|_" + timeo.ToString();
                }

                public override void initOnLoad(string savedSettings)
                {
                    try
                    {
                        string[] seppa = { "_|_|_" };
                        string[] splitta = savedSettings.Split(seppa, StringSplitOptions.None);
                        mp = Convert.ToInt16(splitta[2]);
                        mip = splitta[1];
                        timeo = Convert.ToInt32(splitta[3]);
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
                        mps = Encoding.ASCII.GetString(bita);
                    }
                    catch
                    {
                        mps = "";
                        mip = "error";
                        mp = 0;
                        timeo = 0;
                    }
                }

                private class StateObject
                {
                    // Client socket.
                    public Socket workSocket = null;
                    // Size of receive buffer.
                    public const int BufferSize = 256;
                    // Receive buffer.
                    public byte[] buffer = new byte[BufferSize];
                    // Received data string.
                    public StringBuilder sb = new StringBuilder();
                    //received data bytes
                    public byte[] dataReceived = null;
                }







            }




            /// <summary>
            /// Converts variable to double.
            /// </summary>
            public class PUPPIVariableToDouble : PUPPIModule
            {
                public PUPPIVariableToDouble()
                    : base()
                {
                    name = "Variable to Double";
                    description = "Converts any numeric style variable to double. Some automatically generated modules need specific variable type as input.";
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Variable");
                    outputs.Add("not set");
                    outputnames.Add("Double");
                }
                public override void process_usercode()
                {
                    try
                    {
                        if (usercodeinputs[0] is Boolean)
                        {
                            if ((bool)usercodeinputs[0] == true)
                            {
                                usercodeoutputs[0] = 1.0;
                            }
                            else
                            {
                                usercodeoutputs[0] = 0.0;
                            }
                            return;
                        }
                        string sasa = usercodeinputs[0].ToString();
                        double dp = double.Parse(sasa);
                        usercodeoutputs[0] = dp;
                    }
                    catch
                    {
                        usercodeoutputs[0] = "conversion error";
                    }
                }

            }

            /// <summary>
            /// Runs an executable file with a path and list of arguments and returns output for parsing
            /// </summary>
            public class PUPPIExecute : PUPPIModule
            {
                public PUPPIExecute()
                    : base()
                {
                    name = "Exe file";
                    description = "Run an executable file with specified arguments list of strings or arraylist containing strings or even just a string and retrieve output as string";
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Arguments");
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Exe path");
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Window T/F");
                    outputs.Add(null);
                    outputnames.Add("Console Out");

                }
                public override void process_usercode()
                {
                    ArrayList linput = new ArrayList();
                    if (usercodeinputs[0] is string)
                    {
                        linput.Add(usercodeinputs[0] as string);
                    }
                    else
                        if (usercodeinputs[0] is ICollection)
                            linput = new ArrayList(usercodeinputs[0] as ICollection);
                        else if (usercodeinputs[0] is IEnumerable)
                            linput = PUPPIModule.makeMeAnArrayList(usercodeinputs[0] as IEnumerable);
                        else
                        {
                            usercodeoutputs[0] = "Invalid arguments";
                            return;
                        }
                    //generate argument
                    string al = "";
                    for (int i = 0; i < linput.Count; i++)
                    {
                        al += linput[i] as string;
                        if (i != linput.Count - 1) al += " ";
                    }
                    string ep = "";
                    if (usercodeinputs[1] is string)
                    {
                        ep = usercodeinputs[1] as string;
                        //if (!File.Exists(ep))
                        //{
                        //    usercodeoutputs[0] = "Invalid exe";
                        //    return;
                        //}
                    }
                    else
                    {
                        usercodeoutputs[0] = "Invalid exe";
                        return;
                    }

                    bool winmode = false;
                    try
                    {
                        winmode = Convert.ToBoolean(usercodeinputs[2]);
                    }
                    catch
                    {
                        winmode = false;
                    }
                    //do our own exception handling here
                    //try
                    //{ 
                    var proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = ep,
                            Arguments = al,
                            UseShellExecute = false,
                            RedirectStandardOutput = !winmode,
                            CreateNoWindow = !winmode
                        }
                    };

                    string procout = "";
                    proc.Start();
                    if (!winmode)
                        while (!proc.StandardOutput.EndOfStream)
                        {
                            string line = proc.StandardOutput.ReadLine();
                            procout += line + "\n";
                        }
                    usercodeoutputs[0] = procout;
                    //}
                    //    catch(Exception exy)
                    //    {
                    //        usercodeoutputs[0] = "Failed to start process:" + exy.ToString();   
                    //    }

                }
            }



        }



        namespace NetworkingModules
        {
            /// <summary>
            /// The <see cref="NetworkingModules"/> namespace contains PUPPI modules for advanced client/server operations.
            /// </summary>

            [System.Runtime.CompilerServices.CompilerGenerated]
            class NamespaceDoc
            {
            }
            /// <summary>
            /// Sends a command to a PUPPICanvas server and retrieves result as text,object or file
            /// </summary>
            public class PUPPIRemoteCommand : PUPPIModule
            {
                byte sc1 = 2;
                byte sc2 = 87;
                string mps = "";
                string mip = "";
                int mp = 0;
                int timeo = 0;
                public PUPPIRemoteCommand()
                    : base()
                {
                    name = "Remote Command";
                    description = "Sends a command to a PUPPICanvas server and retrieves result as text,object or file";
                    doubleClickDescription = "Set connection parameters";
                    inputnames.Add("Command string");
                    inputs.Add(new PUPPIInParameter());
                    PUPPIInParameter pi = new PUPPIInParameter();
                    pi.isoptional = true;
                    inputs.Add(pi);
                    inputnames.Add("Save file ext");
                    outputnames.Add("Raw data");
                    outputs.Add(null);
                    outputnames.Add("Text");
                    outputs.Add("");
                    outputnames.Add("Object");
                    outputs.Add(null);
                    outputnames.Add("File path");
                    outputs.Add("");
                    completeDoubleClickOverride = true;
                }
                public override void process_usercode()
                {
                    DataExchangeModules.AsynchronousClient.filex = "nout";
                    if (usercodeinputs[1] != null)
                    {
                        DataExchangeModules.AsynchronousClient.filex = usercodeinputs[1].ToString().Replace(".", "");

                    }
                    string command = usercodeinputs[0].ToString();

                    if (GUID == -1)
                        DataExchangeModules.AsynchronousClient.nid = "nonode";
                    else
                        DataExchangeModules.AsynchronousClient.nid = GUID.ToString();

                    try
                    {
                        DataExchangeModules.AsynchronousClient.initializeClient(mip, mp, mps, timeo);
                    }
                    catch (Exception exy)
                    {
                        usercodeoutputs[1] = "Client init error.";
                        return;

                    }

                    DataExchangeModules.receivedData rd = DataExchangeModules.AsynchronousClient.sendServerCommand(command);
                    usercodeoutputs[0] = rd.data;
                    usercodeoutputs[1] = rd.text;
                    usercodeoutputs[2] = rd.obj;
                    usercodeoutputs[3] = rd.savedFilePath;

                }

                public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
                {
                    PUPPI.remoteClient rec = new PUPPI.remoteClient();
                    rec.pps = mps;
                    rec.ips = mip;
                    rec.prts = mp.ToString();
                    rec.tms = timeo.ToString();
                    rec.ShowDialog();
                    try
                    {
                        mps = rec.pps;
                        mip = rec.ips;
                        mp = Convert.ToInt16(rec.prts);
                        timeo = Convert.ToInt32(rec.tms);
                    }
                    catch
                    {
                        MessageBox.Show("Error setting parameters. Clearing");
                        mps = "";
                        mip = "";
                        mp = 0;
                        timeo = 0;
                    }
                }

                public override string saveSettings()
                {
                    byte[] PtoBytes = Encoding.ASCII.GetBytes(mps);
                    byte[] TPtoBytes = new byte[PtoBytes.Length * 2];
                    int j = 0;
                    for (int i = 0; i < PtoBytes.Length; i++)
                    {
                        int newb = PtoBytes[i] + sc1 + sc2;
                        if (newb > 126)
                        {
                            byte tbt = Convert.ToByte(newb - 126);
                            if (tbt < 32)
                            {
                                TPtoBytes[j] = Convert.ToByte(tbt + 32);
                                j++;
                                TPtoBytes[j] = Convert.ToByte(124);
                                j++;
                            }
                            else
                            {
                                TPtoBytes[j] = Convert.ToByte(tbt);
                                j++;
                                TPtoBytes[j] = Convert.ToByte(125);
                                j++;
                            }
                        }
                        else
                        {
                            TPtoBytes[j] = Convert.ToByte(newb);
                            j++;
                            TPtoBytes[j] = Convert.ToByte(126);
                            j++;
                        }
                    }
                    TPtoBytes = TPtoBytes.Reverse().ToArray();
                    string smps = Encoding.ASCII.GetString(TPtoBytes);

                    return smps + "_|_|_" + mip + "_|_|_" + mp.ToString() + "_|_|_" + timeo.ToString();
                }

                public override void initOnLoad(string savedSettings)
                {
                    try
                    {
                        string[] seppa = { "_|_|_" };
                        string[] splitta = savedSettings.Split(seppa, StringSplitOptions.None);
                        mp = Convert.ToInt16(splitta[2]);
                        mip = splitta[1];
                        timeo = Convert.ToInt32(splitta[3]);
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
                        mps = Encoding.ASCII.GetString(bita);
                    }
                    catch
                    {
                        mps = "";
                        mip = "error";
                        mp = 0;
                        timeo = 0;
                    }
                }

                private class StateObject
                {
                    // Client socket.
                    public Socket workSocket = null;
                    // Size of receive buffer.
                    public const int BufferSize = 256;
                    // Receive buffer.
                    public byte[] buffer = new byte[BufferSize];
                    // Received data string.
                    public StringBuilder sb = new StringBuilder();
                    //received data bytes
                    public byte[] dataReceived = null;
                }







            }

        }

        namespace ListModules
        {
            /// <summary>
            /// The <see cref="ListModules"/> namespace contains PUPPI modules for generating and accessing generic lists.
            /// </summary>

            [System.Runtime.CompilerServices.CompilerGenerated]
            class NamespaceDoc
            {
            }
            /// <summary>
            /// Counts the items in an  Arraylist
            /// </summary>
            public class PUPLISTCOUNT : PUPPIModule
            {
                public PUPLISTCOUNT()
                    : base()
                {
                    name = "Count Items/Rows";
                    description = "Counts the number of items in an 1D ArrayList or typed List or the number of rows in a 2D Grid";
                    outputs.Add(0);
                    outputnames.Add("Number");
                    inputnames.Add("Coll/Grid");

                    inputs.Add(new PUPPIInParameter());

                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {



                    try
                    {

                        if (inputs[0].module.outputs[inputs[0].outParIndex] is ICollection)
                            outputs[0] = (inputs[0].module.outputs[inputs[0].outParIndex] as ICollection).Count;
                        else if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                            outputs[0] = PUPPIModule.makeMeAnArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as IEnumerable).Count;

                    }
                    catch (Exception exy)
                    {
                        string[] atsplit = { " at " };
                        outputs[0] = "error: " + exy.ToString().Split(atsplit, StringSplitOptions.None)[0];
                        return;
                    }





                    return;
                }
            }

            /// <summary>
            /// Every time it is run, the input value is added to a list stored up to capacity. FIFO.
            /// </summary>
            public class PUPPILISTACCUM : PUPPIModule
            {
                ArrayList accum; 
                public PUPPILISTACCUM ()
                    :base()
                {
                    name = "List Accumulator";
                    description = "Every time it is run, the input value is added to a list stored up to capacity. FIFO.";
                    doubleClickDescription = "Empty accumulator"; 
                    inputnames.Add("Value");
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Capacity");
                    inputs.Add(new PUPPIInParameter());
                    outputnames.Add("ArrayList");  
                    outputs.Add(null);
                    completeProcessOverride = true;
                    completeDoubleClickOverride = true; 
                    accum = new ArrayList();  

 
                }
                public override void process_usercode()
                {
                    int capac = -1;
                    if (inputs[0].module == null || inputs[1].module == null)
                    {
                        outputs[0] = "not set";
                        return;
                    }
                    object vals = inputs[0].module.outputs[inputs[0].outParIndex];
                    try
                    {
                        capac = Convert.ToInt32(inputs[1].module.outputs[inputs[1].outParIndex]);
                    }
                    catch
                    {
                        capac = -1;
                    }
                    if (capac <= 0)
                    {
                        outputs[0] = "Invalid capacity";
                        return;
                    }
                    while (accum.Count>=capac   )
                    {
                        accum.RemoveAt(0);
 
                    }
                    accum.Add(vals);
                    outputs[0] = accum.Clone() ; 
 
                }
                public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
                {
                    accum.Clear(); 
                }
            }
            /// <summary>
            /// Gets just an item from a Collection
            /// </summary>
            public class PUPLISTGET : PUPPIModule
            {
                public PUPLISTGET()
                    : base()
                {
                    name = "Get List Item";
                    description = "Retrieves an item from a 1D typed or generic list.";
                    outputs.Add(0);
                    outputnames.Add("Result");
                    inputnames.Add("Collection");
                    inputnames.Add("Index");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {



                    try
                    {
                        //ArrayList linput = new ArrayList (inputs[0].module.outputs[inputs[0].outParIndex] as ICollection) ;
                        ArrayList linput = new ArrayList();
                        if (inputs[0].module.outputs[inputs[0].outParIndex] is ICollection)
                            linput = new ArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as ICollection);
                        else if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                            linput = PUPPIModule.makeMeAnArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as IEnumerable);
                        int index = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);
                        if (index > linput.Count - 1 || index < 0)
                        {
                            outputs[0] = "outbounds";
                            return;
                        }
                        else
                        {
                            outputs[0] = linput[index];
                        }
                    }
                    catch (Exception exy)
                    {
                        string[] atsplit = { " at " };
                        outputs[0] = "error: " + exy.ToString().Split(atsplit, StringSplitOptions.None)[0];
                        return;
                    }





                    return;
                }
            }

            /// <summary>
            /// Gets first item from a Collection
            /// </summary>
            public class PUPLISTGETFIRST : PUPPIModule
            {
                public PUPLISTGETFIRST()
                    : base()
                {
                    name = "Get First Item";
                    description = "Retrieves first item from a 1D typed or generic list.";
                    outputs.Add(0);
                    outputnames.Add("Result");
                    inputnames.Add("Collection");
                    inputnames.Add("Index");
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {



                    try
                    {
                        //ArrayList linput = new ArrayList (inputs[0].module.outputs[inputs[0].outParIndex] as ICollection) ;
                        ArrayList linput = new ArrayList();
                        if (inputs[0].module.outputs[inputs[0].outParIndex] is ICollection)
                            linput = new ArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as ICollection);
                        else if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                            linput = PUPPIModule.makeMeAnArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as IEnumerable);
                        int index = 0;
                        if (index > linput.Count - 1 || index < 0)
                        {
                            outputs[0] = "outbounds";
                            return;
                        }
                        else
                        {
                            outputs[0] = linput[index];
                        }
                    }
                    catch (Exception exy)
                    {
                        string[] atsplit = { " at " };
                        outputs[0] = "error: " + exy.ToString().Split(atsplit, StringSplitOptions.None)[0];
                        return;
                    }





                    return;
                }
            }



            /// <summary>
            /// Gets last item from a Collection
            /// </summary>
            public class PUPLISTGETLAST : PUPPIModule
            {
                public PUPLISTGETLAST()
                    : base()
                {
                    name = "Get Last Item";
                    description = "Retrieves last item from a 1D typed or generic list.";
                    outputs.Add(0);
                    outputnames.Add("Result");
                    inputnames.Add("Collection");
                    inputnames.Add("Index");
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {



                    try
                    {
                        //ArrayList linput = new ArrayList (inputs[0].module.outputs[inputs[0].outParIndex] as ICollection) ;
                        ArrayList linput = new ArrayList();
                        if (inputs[0].module.outputs[inputs[0].outParIndex] is ICollection)
                            linput = new ArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as ICollection);
                        else if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                            linput = PUPPIModule.makeMeAnArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as IEnumerable);
                        int index = linput.Count - 1;
                        if (index > linput.Count - 1 || index < 0)
                        {
                            outputs[0] = "outbounds";
                            return;
                        }
                        else
                        {
                            outputs[0] = linput[index];
                        }
                    }
                    catch (Exception exy)
                    {
                        string[] atsplit = { " at " };
                        outputs[0] = "error: " + exy.ToString().Split(atsplit, StringSplitOptions.None)[0];
                        return;
                    }





                    return;
                }
            }


            /// <summary>
            /// Sets just an item in a collection by index
            /// </summary>
            public class PUPLISTSETITEM : PUPPIModule
            {
                public PUPLISTSETITEM()
                    : base()
                {
                    name = "Set List Item";
                    description = "Sets an item from a 1D typed or generic list.";
                    outputs.Add(0);
                    outputnames.Add("ArrayList");
                    inputnames.Add("Collection");
                    inputnames.Add("Index");
                    inputnames.Add("Value");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {



                    try
                    {
                        //ArrayList linput = new ArrayList (inputs[0].module.outputs[inputs[0].outParIndex] as ICollection) ;
                        ArrayList pinput = new ArrayList();
                        //if (inputs[0].module.outputs[inputs[0].outParIndex]  is ICollection)
                        //    linput = new ArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex])  as ICollection);
                        //else if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                        //    linput = PUPPIModule.makeMeAnArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex]) as IEnumerable);
                        int index = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);
                        pinput = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]);

                        ArrayList linput = new ArrayList(pinput);

                        if (index > linput.Count - 1 || index < 0)
                        {
                            outputs[0] = "outbounds";
                            return;
                        }

                        else
                        {
                            if (inputs[2].module != null)
                                linput[index] = inputs[2].module.outputs[inputs[2].outParIndex];
                            outputs[0] = linput;
                        }
                    }
                    catch (Exception exy)
                    {
                        string[] atsplit = { " at " };
                        outputs[0] = "error: " + exy.ToString().Split(atsplit, StringSplitOptions.None)[0];
                        return;
                    }





                    return;
                }
            }


            /// <summary>
            ///  Gets a range from an Arraylist or typed List as an Arraylist
            /// </summary>
            public class PUPLISTGETRANGE : PUPPIModule
            {
                public PUPLISTGETRANGE()
                    : base()
                {
                    name = "Get List Range";
                    description = "Returns a range of items from a 1D ArrayList or typed List as an ArrayList";
                    outputs.Add(new ArrayList());
                    outputnames.Add("Range");
                    inputnames.Add("ArrayList");
                    inputnames.Add("Start Index");
                    inputnames.Add("End Index");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {



                    try
                    {
                        ArrayList linput = new ArrayList();
                        if (inputs[0].module.outputs[inputs[0].outParIndex] is ICollection)
                            linput = new ArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as ICollection);
                        else if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                            linput = PUPPIModule.makeMeAnArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as IEnumerable);
                        int sindex = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);
                        int eindex = Convert.ToInt16(inputs[2].module.outputs[inputs[1].outParIndex]);
                        if (sindex < 0 || eindex > linput.Count - 1 || sindex > eindex)
                        {
                            outputs[0] = "outbounds";
                            return;
                        }
                        else
                        {
                            ArrayList ranger = linput.GetRange(sindex, eindex - sindex + 1);
                            outputs[0] = ranger;
                            return;
                        }
                    }
                    catch (Exception exy)
                    {
                        string[] atsplit = { " at " };
                        outputs[0] = "error: " + exy.ToString().Split(atsplit, StringSplitOptions.None)[0];
                        return;
                    }





                    return;
                }


            }


            /// <summary>
            ///  Sorts an ArrayList/Collection/List by parameter given as output of class stored in Arraylist,or direct comparison
            /// </summary>
            public class PUPLISTSORT : PUPPIModule
            {
                public PUPLISTSORT()
                    : base()
                {
                    name = "Sort List";
                    outputs.Add(new ArrayList());
                    description = " Sorts an ArrayList/Collection/List by parameter given as output of class stored in Arraylist,or direct comparison";
                    outputnames.Add("Sorted List");
                    inputnames.Add("ArrayList");
                    inputnames.Add("Property Name");
                    inputnames.Add("Decrease T/F");
                    inputs.Add(new PUPPIInParameter());
                    PUPPIInParameter p1 = new PUPPIInParameter();
                    p1.isoptional = true;
                    inputs.Add(p1);
                    PUPPIInParameter p2 = new PUPPIInParameter();
                    p2.isoptional = true;
                    inputs.Add(p2);
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {


                    ArrayList sorted = new ArrayList();
                    try
                    {

                        bool typecast = false;
                        string paramName = "";
                        if (inputs[1].module == null)
                        {

                        }
                        else
                        {
                            paramName = inputs[1].module.outputs[inputs[1].outParIndex].ToString();
                            typecast = true;
                        }
                        bool decreasing = false;
                        if (inputs[2].module == null)
                        {

                        }
                        else
                        {
                            int i = Convert.ToInt16(inputs[2].module.outputs[inputs[2].outParIndex]);
                            if (i > 0) decreasing = true;

                        }
                        ArrayList linput = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]);
                        if (typecast)
                        {
                            for (int i = 0; i < linput.Count; i++)
                            {

                                object o = linput[i];
                                int insertin = 0;
                                double cv = Convert.ToDouble(o.GetType().GetProperty(paramName).GetValue(o, null));
                                //go through
                                if (sorted.Count > 0)
                                {
                                    for (int s = 0; s < sorted.Count; s++)
                                    {
                                        //figure out if to insert before or after current item
                                        double scv = Convert.ToDouble((sorted[s] as object).GetType().GetProperty(paramName).GetValue(sorted[s] as object, null));
                                        double pcv;
                                        double ncv;
                                        if (s == 0) pcv = Double.MinValue; else pcv = Convert.ToDouble((sorted[s - 1] as object).GetType().GetProperty(paramName).GetValue(sorted[s - 1] as object, null));
                                        if (s == sorted.Count - 1) ncv = Double.MaxValue; else ncv = Convert.ToDouble((sorted[s + 1] as object).GetType().GetProperty(paramName).GetValue(sorted[s + 1] as object, null));
                                        if (cv >= pcv && cv <= scv)
                                        {
                                            insertin = s;
                                            break;
                                        }
                                        else if (cv >= scv && cv <= ncv)
                                        {
                                            insertin = s + 1;
                                            break;
                                        }


                                    }
                                }
                                sorted.Insert(insertin, linput[i]);

                            }
                        }
                        else
                        {

                            for (int i = 0; i < linput.Count; i++)
                            {
                                int insertin = 0;
                                double cv = Convert.ToDouble(linput[i]);
                                //go through
                                if (sorted.Count > 0)
                                {
                                    for (int s = 0; s < sorted.Count; s++)
                                    {
                                        //figure out if to insert before or after current item
                                        double scv = Convert.ToDouble(sorted[s]);
                                        double pcv;
                                        double ncv;
                                        if (s == 0) pcv = Double.MinValue; else pcv = Convert.ToDouble(sorted[s - 1]);
                                        if (s == sorted.Count - 1) ncv = Double.MaxValue; else ncv = Convert.ToDouble(sorted[s + 1]);
                                        if (cv >= pcv && cv <= scv)
                                        {
                                            insertin = s;
                                            break;
                                        }
                                        else if (cv >= scv && cv <= ncv)
                                        {
                                            insertin = s + 1;
                                            break;
                                        }


                                    }
                                }
                                sorted.Insert(insertin, linput[i]);

                            }
                        }
                        if (decreasing) sorted.Reverse();

                    }
                    catch (Exception exy)
                    {
                        string[] atsplit = { " at " };
                        outputs[0] = "error: " + exy.ToString().Split(atsplit, StringSplitOptions.None)[0];
                        return;
                    }





                    outputs[0] = sorted;
                }


            }
            /// <summary>
            /// Returns 0 based index of the first item equal to input.-1 if not found or error.
            /// </summary>
            public class PUPLISTGetFirstIndex : PUPPIModule
            {

                public PUPLISTGetFirstIndex()
                    : base()
                {
                    name = "Get Index";
                    description = "Returns 0 based index of the first item equal to input.-1 if not found or error.";
                    outputs.Add(-1);
                    outputnames.Add("Index");
                    inputnames.Add("Collection");
                    inputnames.Add("Value");
                    PUPPIInParameter p1 = new PUPPIInParameter();
                    inputs.Add(p1);
                    PUPPIInParameter p2 = new PUPPIInParameter();
                    inputs.Add(p2);
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {
                    try
                    {
                        ArrayList linput = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]);
                        object tofind = inputs[1].module.outputs[inputs[1].outParIndex];
                        for (int i = 0; i < linput.Count; i++)
                        {
                            object isit = linput[i];
                            if (tofind.Equals(isit))
                            {
                                outputs[0] = i;
                                return;
                            }
                        }
                        outputs[0] = -1;

                    }
                    catch
                    {
                        outputs[0] = -1;
                    }
                }
            }

            /// <summary>
            ///  Finds in a List/ArrayList the value input of the input parameter for the objects in the list 
            ///  Returns an ArrayList of objects with matching values.
            /// </summary>
            public class PUPLISTFindByPropVal : PUPPIModule
            {
                public PUPLISTFindByPropVal()
                    : base()
                {
                    name = "Find by PropVal";
                    description = "Returns a list of objects with matching Proverty Value for Property Name ";
                    outputs.Add(new ArrayList());
                    outputnames.Add("Found Objects List");
                    inputnames.Add("Collection");
                    inputnames.Add("Property Name");
                    inputnames.Add("Property Value");
                    inputs.Add(new PUPPIInParameter());
                    PUPPIInParameter p1 = new PUPPIInParameter();
                    inputs.Add(p1);
                    PUPPIInParameter p2 = new PUPPIInParameter();
                    inputs.Add(p2);
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {


                    ArrayList foundO = new ArrayList();
                    try
                    {


                        string paramName = "";
                        if (inputs[0].module == null)
                        {
                            outputs[0] = "Null input";
                            return;
                        }
                        if (inputs[1].module == null)
                        {
                            outputs[0] = "Null input";
                            return;
                        }
                        else
                        {
                            paramName = inputs[1].module.outputs[inputs[1].outParIndex].ToString();

                        }
                        bool decreasing = false;
                        if (inputs[2].module == null)
                        {
                            outputs[0] = "Null input";
                            return;
                        }

                        ArrayList linput = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]); //new ArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex]) as ICollection);

                        for (int i = 0; i < linput.Count; i++)
                        {


                            var o = linput[i];
                            PropertyInfo pi = o.GetType().GetProperty(paramName, BindingFlags.Public | BindingFlags.Instance);
                            if (pi != null)
                            {
                                var myO = pi.GetValue(o, null);
                                var yourO = inputs[2].module.outputs[inputs[2].outParIndex];
                                if (myO.Equals(yourO))
                                {
                                    foundO.Add(linput[i]);
                                }//try type conversion
                                else
                                {
                                    var nyour = Convert.ChangeType(yourO, myO.GetType());
                                    if (myO.Equals(nyour))

                                        foundO.Add(linput[i]);

                                }
                            }
                            else
                            {
                                //try field
                                FieldInfo fi = o.GetType().GetField(paramName, BindingFlags.Public | BindingFlags.Instance);
                                var myO = fi.GetValue(o);
                                var yourO = inputs[2].module.outputs[inputs[2].outParIndex];
                                if (myO.Equals(yourO))
                                {
                                    foundO.Add(linput[i]);
                                }
                                else
                                {
                                    var nyour = Convert.ChangeType(yourO, myO.GetType());
                                    if (myO.Equals(nyour))

                                        foundO.Add(linput[i]);
                                }
                            }


                        }



                    }
                    catch (Exception exy)
                    {
                        string[] atsplit = { " at " };
                        outputs[0] = "error: " + exy.ToString().Split(atsplit, StringSplitOptions.None)[0];
                        return;
                    }





                    outputs[0] = foundO;
                }


            }

            /// <summary>
            ///  Finds in a List/ArrayList the objects of same type as Object input 
            ///  Returns an ArrayList of objects with matching type.
            /// </summary>
            public class PUPLISTFindByType : PUPPIModule
            {
                public PUPLISTFindByType()
                    : base()
                {
                    name = "Find By Type";
                    description = "Returns a list of objects with matching type to Object input ";
                    outputs.Add(new ArrayList());
                    outputnames.Add("Found Objects List");
                    inputnames.Add("Collection");
                    inputnames.Add("Object");
                    inputs.Add(new PUPPIInParameter());
                    PUPPIInParameter p1 = new PUPPIInParameter();
                    inputs.Add(p1);
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {


                    ArrayList foundO = new ArrayList();
                    try
                    {


                        string paramName = "";
                        if (inputs[0].module == null)
                        {
                            outputs[0] = "Null input";
                            return;
                        }
                        if (inputs[1].module == null)
                        {
                            outputs[0] = "Null input";
                            return;
                        }


                        ArrayList linput = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]);//new ArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex])  as ICollection);
                        var yourO = inputs[1].module.outputs[inputs[1].outParIndex];
                        for (int i = 0; i < linput.Count; i++)
                        {


                            var o = linput[i];

                            if (o.GetType() == yourO.GetType()) foundO.Add(linput[i]);


                        }



                    }
                    catch (Exception exy)
                    {
                        string[] atsplit = { " at " };
                        outputs[0] = "error: " + exy.ToString().Split(atsplit, StringSplitOptions.None)[0];
                        return;
                    }





                    outputs[0] = foundO;
                }


            }

            /// <summary>
            /// Generates an Arraylist of numeric values like a for loop.
            /// </summary>
            public class PUPLISTFOR : PUPPIModule
            {
                public PUPLISTFOR()
                    : base()
                {
                    name = "For List";
                    outputs.Add(new ArrayList());
                    outputnames.Add("ArrayList");
                    description = "Generates an Arraylist of numeric values like a for loop.";
                    inputnames.Add("StartValue");
                    inputnames.Add("EndValue");
                    inputnames.Add("StepSize");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {



                    try
                    {
                        ArrayList added = new ArrayList();
                        //need to make sure we can process properly based on inputs
                        //check if inputs are all integers
                        if (inputs[0].module.outputs[inputs[0].outParIndex] is int && inputs[1].module.outputs[inputs[1].outParIndex] is int && inputs[2].module.outputs[inputs[2].outParIndex] is int)
                        {
                            int startl = Convert.ToInt16(inputs[0].module.outputs[inputs[0].outParIndex]);
                            int endl = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);
                            int stepl = Convert.ToInt16(inputs[2].module.outputs[inputs[2].outParIndex]);
                            if (startl <= endl)
                            {
                                if (stepl <= 0)
                                {
                                    outputs[0] = "inv. step <=0";
                                    return;
                                }
                            }
                            else
                            {
                                if (stepl >= 0)
                                {
                                    outputs[0] = "inv. step >=0";
                                    return;
                                }
                            }

                            if (stepl > 0)
                            {
                                for (int cont = startl; cont <= endl; cont += stepl)
                                {
                                    added.Add(cont);
                                }
                            }
                            else
                            {
                                for (int cont = startl; cont >= endl; cont += stepl)
                                {
                                    added.Add(cont);
                                }
                            }
                        }
                        else
                        {
                            double startl = Convert.ToDouble(inputs[0].module.outputs[inputs[0].outParIndex]);
                            double endl = Convert.ToDouble(inputs[1].module.outputs[inputs[1].outParIndex]);
                            double stepl = Convert.ToDouble(inputs[2].module.outputs[inputs[2].outParIndex]);
                            if (startl <= endl)
                            {
                                if (stepl <= 0)
                                {
                                    outputs[0] = "inv. step <=0";
                                    return;
                                }
                            }
                            else
                            {
                                if (stepl >= 0)
                                {
                                    outputs[0] = "inv. step >=0";
                                    return;
                                }
                            }
                            if (stepl > 0)
                            {
                                for (double cont = startl; cont <= endl + PUPPIGUISettings.numberMatchToler; cont += stepl)
                                {
                                    added.Add(cont);
                                }
                            }
                            else
                            {
                                for (double cont = startl; cont >= endl - PUPPIGUISettings.numberMatchToler; cont += stepl)
                                {
                                    added.Add(cont);
                                }
                            }
                        }


                        outputs[0] = added;

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
            /// Clones an Arraylist as 123...,123... etc into an Arraylist of Arraylists
            /// </summary>
            public class PUPLISTCloneRow : PUPPIModule
            {
                public PUPLISTCloneRow()
                    : base()
                {
                    name = "Clone(row)";
                    description = "Clones an item or repeats an array row x times: 123,123,123...";
                    outputs.Add(new ArrayList());
                    outputnames.Add("ArrayList");
                    inputnames.Add("Item/ArrL");
                    inputnames.Add("Count");

                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {



                    try
                    {
                        ArrayList added = new ArrayList();
                        //check if inputs are all integers
                        //this one we can only do integer



                        int countl = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);
                        ArrayList orlist = new ArrayList();
                        if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable && inputs[0].module.outputs[inputs[0].outParIndex].GetType() != typeof(string))
                        {

                            //if (inputs[0].module.outputs[inputs[0].outParIndex] is ICollection)
                            //    orlist = new ArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex]) as ICollection);
                            //else if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                            //    orlist = PUPPIModule.makeMeAnArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex])  as IEnumerable);
                            orlist = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]);

                        }
                        for (int cont = 0; cont < countl; cont++)
                        {
                            //repeat list




                            if (orlist.Count > 0)
                            {
                                //ArrayList roww = inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList;
                                for (int rc = 0; rc < orlist.Count; rc++)
                                {
                                    added.Add(orlist[rc]);
                                }
                            }
                            else
                            {
                                added.Add(inputs[0].module.outputs[inputs[0].outParIndex]);
                            }
                        }




                        outputs[0] = added;

                    }
                    catch (Exception exy)
                    {
                        outputs[0] = "error";
                        return;
                    }





                    return;
                }
            }
            /// <summary>
            /// Clones an Arraylist as 111,222... etc into an Arraylist of Arraylists
            /// </summary>
            public class PUPLISTCloneCol : PUPPIModule
            {
                public PUPLISTCloneCol()
                    : base()
                {
                    name = "Clone(col)";
                    description = "Clones an item or repeats an array col x times: 111,222,333...";
                    outputs.Add(new ArrayList());
                    outputnames.Add("ArrayList");
                    inputnames.Add("Item/ArrL");
                    inputnames.Add("Count");

                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {



                    try
                    {
                        ArrayList added = new ArrayList();
                        //check if inputs are all integers
                        //this one we can only do integer



                        int countl = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);
                        ArrayList orlist = new ArrayList();

                        if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable && inputs[0].module.outputs[inputs[0].outParIndex].GetType() != typeof(string))
                        {
                            //if (inputs[0].module.outputs[inputs[0].outParIndex] is ICollection)
                            //    orlist = new ArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex])  as ICollection);
                            //else if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                            //    orlist = PUPPIModule.makeMeAnArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex])  as IEnumerable);
                            orlist = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]);

                        }
                        if (orlist.Count > 0)
                        {
                            //ArrayList roww = inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList;
                            for (int rc = 0; rc < orlist.Count; rc++)
                            {
                                for (int cont = 0; cont < countl; cont++)
                                {
                                    //repeat list



                                    added.Add(orlist[rc]);
                                }


                            }
                        }
                        else
                        {
                            for (int cont = 0; cont < countl; cont++)
                            {
                                //repeat list



                                added.Add(inputs[0].module.outputs[inputs[0].outParIndex]);
                            }


                        }





                        outputs[0] = added;

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
            /// Converts a generic ArrayList to a Typed list based on the type of the first element in ArrayList
            /// </summary>
            public class PUPPIGen2TypedList : PUPPIModule
            {
                public PUPPIGen2TypedList()
                    : base()
                {
                    name = "Lst->Typed Lst";
                    description = "Converts the typical PUPPI list (generic) to a typed list based on the type of the first object in generic list";
                    outputs.Add("empty");
                    outputnames.Add("Typed List");
                    inputnames.Add("ArrayList");
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {
                    try
                    {

                        //check if list to add to exists
                        if (inputs[0].module != null)
                        {
                            if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                            {
                                ArrayList orlist = new ArrayList();
                                //if (inputs[0].module.outputs[inputs[0].outParIndex] is ICollection)
                                //    orlist = new ArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex]) as ICollection);
                                //else if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                                //    orlist = PUPPIModule.makeMeAnArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex])  as IEnumerable);
                                orlist = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]);
                                if (orlist.Count > 0)
                                {
                                    object o = orlist[0];
                                    Type t = o.GetType();
                                    var listType = typeof(List<>);
                                    var constructedListType = listType.MakeGenericType(t);

                                    var instance = (IList)Activator.CreateInstance(constructedListType);
                                    for (int i = 0; i < orlist.Count; i++)
                                    {
                                        instance.Add(Convert.ChangeType(orlist[i], t));
                                    }
                                    outputs[0] = instance;
                                }
                            }
                            else
                            {
                                outputs[0] = "Invalid input type";
                            }
                        }
                    }
                    catch
                    {
                        outputs[0] = "error";
                    }
                }

            }
            /// <summary>
            /// Converts a typed List to an Array
            /// </summary>
            public class TypedList2Array : PUPPIModule
            {
                public TypedList2Array()
                    : base()
                {
                    name = "Typed Lst->Array";
                    description = "Converts a list of elements of a certain type to an array of elements of that type";
                    outputs.Add("empty");
                    outputnames.Add("Array");
                    inputnames.Add("Typed List");
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {
                    try
                    {

                        //check if list to add to exists
                        if (inputs[0].module != null)
                        {
                            if (inputs[0].module.outputs[inputs[0].outParIndex] is IList)
                            {
                                IList ii = inputs[0].module.outputs[inputs[0].outParIndex] as IList;
                                Type t = ii.GetType().GetGenericArguments()[0];
                                Array a = Array.CreateInstance(t, ii.Count);
                                for (int i = 0; i < ii.Count; i++)
                                {
                                    a.SetValue(ii[i], i);
                                }
                                outputs[0] = a;
                            }
                            else
                            {
                                outputs[0] = "Invalid input type";
                            }
                        }

                    }
                    catch
                    {
                        outputs[0] = "error";
                    }
                }


            }


            /// <summary>
            /// Converts a one dimensional array to a typed list
            /// </summary>
            public class Array2TypedList : PUPPIModule
            {
                public Array2TypedList()
                    : base()
                {
                    name = "Array->Typed List";
                    description = " Converts a one dimensional array to a typed list";
                    outputs.Add("empty");
                    outputnames.Add("List");
                    inputnames.Add("1D Array");
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {
                    try
                    {

                        //check if list to add to exists
                        if (inputs[0].module != null)
                        {

                            Array mya = inputs[0].module.outputs[inputs[0].outParIndex] as Array;
                            int lbrow = mya.GetLowerBound(0);

                            int ubrow = mya.GetUpperBound(0);


                            Type vart = mya.GetValue(lbrow).GetType();
                            Type listType = typeof(List<>).MakeGenericType(vart);
                            IList myro = Activator.CreateInstance(listType) as IList;
                            for (int i = lbrow; i <= ubrow; i++)
                            {



                                myro.Add(mya.GetValue(i));
                            }
                            outputs[0] = myro;

                        }
                    }
                    catch
                    {
                        outputs[0] = "error";
                    }

                }


            }




            /// <summary>
            /// Adds an item or Arraylist to an existing Arraylist
            /// </summary>
            public class PUPPIListAdd : PUPPIModule
            {
                //if true, when adding or removing inputs the number of inputs stays constant
                //can only be in container if locked
                //locked set by double click
                bool locked = false;
                public PUPPIListAdd()
                    : base()
                {
                    name = "Add to/ Create List";
                    description = "Combines two or more objects in a list or adds an object to a list or a list or adds two or more lists together. Result is a generic list.";
                    outputs.Add(new ArrayList());
                    outputnames.Add("ArrayList");
                    PUPPIInParameter ti = new PUPPIInParameter();
                    ti.isoptional = true;
                    inputs.Add(ti);
                    ti = new PUPPIInParameter();
                    ti.isoptional = true;
                    inputs.Add(ti);
                    inputnames.Add("Obj/Lst 0");
                    inputnames.Add("Obj/Lst 1");
                    completeDoubleClickOverride = true;
                    doubleClickDescription = "Set input add/remove lock. Only locked nodes can be in containers";
                    canBeInContainer = false;

                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {



                    try
                    {
                        ArrayList added = new ArrayList();


                        for (int i = 0; i < inputs.Count; i++)
                        {

                            if (inputs[i].module != null)
                            {
                                if (inputs[i].module.outputs[inputs[i].outParIndex] is IEnumerable && !(inputs[i].module.outputs[inputs[i].outParIndex].GetType() == typeof(string)))
                                {
                                    ArrayList orlist = new ArrayList();
                                    //if (inputs[i].module.outputs[inputs[i].outParIndex] is ICollection)
                                    //    orlist = new ArrayList(PUPPIModel.Utilities.CloneObject(inputs[i].module.outputs[inputs[i].outParIndex])  as ICollection);
                                    //else if (inputs[i].module.outputs[inputs[i].outParIndex] is IEnumerable)
                                    //    orlist = PUPPIModule.makeMeAnArrayList(PUPPIModel.Utilities.CloneObject(inputs[i].module.outputs[inputs[i].outParIndex])  as IEnumerable);

                                    orlist = makeCollOrEnumIntoArrayList(inputs[i].module.outputs[inputs[i].outParIndex]);

                                    //ArrayList orlist = inputs[i].module.outputs[inputs[i].outParIndex] as ArrayList;
                                    for (int ocount = 0; ocount < orlist.Count; ocount++)
                                    {
                                        added.Add(orlist[ocount]);
                                    }
                                }
                                else
                                {
                                    added.Add(inputs[i].module.outputs[inputs[i].outParIndex]);
                                }
                            }
                        }

                        if (!locked)
                        {
                            //add new input automatically if last one connected
                            if (inputs[inputs.Count - 1].module != null)
                            {
                                PUPPIInParameter ti = new PUPPIInParameter();
                                ti.isoptional = true;
                                inputs.Add(ti);
                                inputnames.Add("Obj/Lst " + (inputs.Count - 1).ToString());
                                resetModuleNumberCalls();
                            }
                            //remove unconnected if last two unconnected
                            else if (inputs.Count > 2 && inputs[inputs.Count - 2].module == null)
                            {
                                inputnames.RemoveAt(inputs.Count - 1);
                                inputs.RemoveAt(inputs.Count - 1);
                                resetModuleNumberCalls();

                            }
                        }
                        outputs[0] = added;

                    }
                    catch (Exception exy)
                    {
                        outputs[0] = "error";
                        // forceMyNodeToUpdate(); 
                        return;
                    }




                    // forceMyNodeToUpdate(); 
                    return;
                }
                //



                //save how many inputs we have
                public override string saveSettings()
                {
                    return inputs.Count.ToString() + "_" + locked.ToString();
                }
                public override void initOnLoad(string savedSettings)
                {
                    try
                    {
                        char[] spli = { '_' };
                        string[] ss = savedSettings.Split(spli, StringSplitOptions.None);
                        int ni = Convert.ToInt16(ss[0]);
                        int ai = 0;
                        if (ni > 2) ai = ni - 2;
                        for (int i = 0; i < ai; i++)
                        {
                            PUPPIInParameter ti = new PUPPIInParameter();
                            ti.isoptional = true;
                            inputs.Add(ti);
                            inputnames.Add("Obj/Lst " + (i + 2).ToString());
                        }
                        locked = Convert.ToBoolean(ss[1]);
                        if (locked) canBeInContainer = true; else canBeInContainer = false;
                    }
                    catch
                    {

                    }
                }
                public override void doubleClickMe_userCode(double clickXRatio, double clickYRatio, double clickZRatio)
                {
                    if (locked)
                    {
                        MessageBox.Show("Input count unlocked");
                    }
                    else
                    {
                        MessageBox.Show("Input count locked");
                    }
                    locked = !locked;
                    if (locked) canBeInContainer = true; else canBeInContainer = false;
                }

            }

            /// <summary>
            /// Two lists whose objects need to be interacting with each other in every combination posible are repeated such that there is a pair of elements for each possible combination.
            /// </summary>
            public class PUPPIListCombo : PUPPIModule
            {
                public PUPPIListCombo()
                    : base()
                {
                    name = "Combo Lists";
                    description = "Two lists whose objects need to be interacting with each other in every combination posible are repeated such that there is a pair of elements for each possible combination.Equivalent to using Clone Row and Clone Col on two lists.";
                    outputs.Add(null);
                    outputnames.Add("Combo Coll 1");
                    outputs.Add(null);
                    outputnames.Add("Combo Coll 2");


                    inputnames.Add("Coll 1");
                    inputnames.Add("Coll 2");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());



                    completeProcessOverride = true;
                }

                public override void process_usercode()
                {



                    try
                    {
                        ArrayList added = new ArrayList();
                        //check if list to add to exists
                        if (inputs[0].module != null && inputs[1].module != null)
                        {

                            ArrayList alist1 = new ArrayList();
                            //if (inputs[0].module.outputs[inputs[0].outParIndex] is ICollection)
                            //    alist1 = new ArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex] ) as ICollection);
                            //else if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                            //    alist1 = PUPPIModule.makeMeAnArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex])  as IEnumerable);
                            if (alist1 is IEnumerable)

                                alist1 = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]);

                            ArrayList alist2 = new ArrayList();
                            //if (inputs[1].module.outputs[inputs[1].outParIndex] is ICollection)
                            //    alist2 = new ArrayList(PUPPIModel.Utilities.CloneObject(inputs[1].module.outputs[inputs[1].outParIndex])  as ICollection);
                            //else if (inputs[1].module.outputs[inputs[1].outParIndex] is IEnumerable)
                            //    alist2 = PUPPIModule.makeMeAnArrayList(PUPPIModel.Utilities.CloneObject(inputs[1].module.outputs[inputs[1].outParIndex])  as IEnumerable);
                            if (alist2 is IEnumerable)
                                alist2 = makeCollOrEnumIntoArrayList(inputs[1].module.outputs[inputs[1].outParIndex]);

                            if (alist1.Count == 0)
                            {
                                outputs[0] = "Empty list";
                                return;
                            }
                            if (alist2.Count == 0)
                            {
                                outputs[1] = "Empty list";
                                return;
                            }

                            ArrayList l1 = new ArrayList();
                            ArrayList l2 = new ArrayList();
                            for (int i = 0; i < alist2.Count; i++)
                            {
                                for (int j = 0; j < alist1.Count; j++)
                                {
                                    l1.Add(alist1[j]);
                                }

                            }
                            for (int i = 0; i < alist2.Count; i++)
                            {
                                for (int j = 0; j < alist1.Count; j++)
                                {
                                    l2.Add(alist2[i]);
                                }
                                outputs[0] = l1;
                                outputs[1] = l2;
                            }

                        }
                    }
                    catch
                    {
                        outputs[0] = "error";
                        outputs[1] = "error";
                    }
                }
            }




            /// <summary>
            /// Two Arraylists with same number of elements joined into a 2D list (Arraylist of arraylists)
            /// </summary>
            public class PUPPIListCollate : PUPPIModule
            {
                public PUPPIListCollate()
                    : base()
                {
                    name = "Collate Lists";
                    description = "Two lists of items (or lists),having the same number of elements, are joined into a list of lists";
                    outputs.Add(new ArrayList());
                    outputnames.Add("Collated ArrL");

                    inputnames.Add("ArrayList1");
                    inputnames.Add("ArrayList2");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());

                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {



                    try
                    {
                        ArrayList added = new ArrayList();
                        //check if list to add to exists
                        if (inputs[0].module != null && inputs[1].module != null)
                        {

                            ArrayList alist1 = new ArrayList();
                            //if (inputs[0].module.outputs[inputs[0].outParIndex] is ICollection)
                            //    alist1 = new ArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex]) as ICollection);
                            //else if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                            //    alist1 = PUPPIModule.makeMeAnArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex]) as IEnumerable);

                            if (alist1 is IEnumerable)
                                alist1 = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]);

                            ArrayList alist2 = new ArrayList();
                            //if (inputs[1].module.outputs[inputs[1].outParIndex] is ICollection)
                            //    alist2 = new ArrayList(inputs[1].module.outputs[inputs[1].outParIndex] as ICollection);
                            //else if (inputs[1].module.outputs[inputs[1].outParIndex] is IEnumerable)
                            //    alist2 = PUPPIModule.makeMeAnArrayList(inputs[1].module.outputs[inputs[1].outParIndex] as IEnumerable);
                            if (alist2 is IEnumerable)
                                alist2 = makeCollOrEnumIntoArrayList(inputs[1].module.outputs[inputs[1].outParIndex]);

                            //  ArrayList alist1 = inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList;
                            //ArrayList alist2 = inputs[1].module.outputs[inputs[1].outParIndex] as ArrayList;
                            if (alist1.Count != alist2.Count) throw new Exception("not match");
                            for (int ocount = 0; ocount < alist1.Count; ocount++)
                            {
                                ArrayList newme = new ArrayList();
                                ////make typed list if of same type but they are not ArrayList
                                //if (alist1[ocount].GetType() == alist2[ocount].GetType() && !(alist1[ocount] is ArrayList))
                                //{
                                //    Type ltype = alist1[ocount].GetType();
                                //    Type genericListType = typeof(List<>).MakeGenericType(ltype);
                                //    IList ila = (IList)Activator.CreateInstance(genericListType);
                                //    ila.Add(alist1[ocount]);
                                //    ila.Add(alist2[ocount]);
                                //    added.Add(ila);
                                //}
                                //else
                                //{
                                if (alist1[ocount] is ArrayList)
                                {
                                    newme.InsertRange(0, alist1[ocount] as ArrayList);

                                }
                                else
                                {
                                    newme.Add(alist1[ocount]);
                                }
                                if (alist2[ocount] is ArrayList)
                                {
                                    newme.InsertRange(newme.Count, alist1[ocount] as ArrayList);

                                }
                                else
                                {
                                    newme.Add(alist2[ocount]);
                                }
                                added.Add(newme);



                            }
                        }



                        outputs[0] = added;

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
            /// Gets a single value from a grid (ArrayList of Arraylists or typed Lists) by row and column.
            /// </summary>
            public class PUPPIGridGet : PUPPIModule
            {
                public PUPPIGridGet()
                    : base()
                {
                    name = "Get Grid Item";
                    outputs.Add(0);
                    description = "Gets a single value from a grid (ArrayList of Arraylists or typed Lists) by row and column.";
                    outputnames.Add("Grid Item");
                    inputnames.Add("Grid");
                    inputnames.Add("Row Index");
                    inputnames.Add("Col Index");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {


                    if (inputs[0].module != null)
                    {
                        try
                        {
                            ArrayList linput = inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList;
                            int rowindex = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);
                            int colindex = Convert.ToInt16(inputs[2].module.outputs[inputs[2].outParIndex]);
                            if (rowindex > linput.Count - 1 || rowindex < 0)
                            {
                                outputs[0] = "outbounds";
                                return;
                            }
                            else
                            {
                                ArrayList getrow = makeCollOrEnumIntoArrayList(linput[rowindex]) as ArrayList;
                                if (colindex > getrow.Count - 1 || colindex < 0)
                                {
                                    outputs[0] = "outbounds";
                                    return;
                                }
                                else
                                {
                                    outputs[0] = getrow[colindex];
                                }
                            }
                        }
                        catch
                        {
                            outputs[0] = "error";
                            return;
                        }


                    }



                }
            }

            /// <summary>
            /// Sets a single value in a grid (ArrayList of Arraylists or typed Lists) by row and column.
            /// </summary>
            public class PUPPIGridSetItem : PUPPIModule
            {
                public PUPPIGridSetItem()
                    : base()
                {
                    name = "Set Grid Item";
                    outputs.Add(0);
                    description = "Sets a single value in a grid (ArrayList of Arraylists or typed Lists) by row and column.";
                    outputnames.Add("Grid");
                    inputnames.Add("Grid");
                    inputnames.Add("Row Index");
                    inputnames.Add("Col Index");
                    inputnames.Add("Value");
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {


                    if (inputs[0].module != null)
                    {
                        try
                        {
                            ArrayList linput = new ArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList);
                            int rowindex = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);
                            int colindex = Convert.ToInt16(inputs[2].module.outputs[inputs[2].outParIndex]);
                            if (rowindex > linput.Count - 1 || rowindex < 0)
                            {
                                outputs[0] = "outbounds";
                                return;
                            }
                            else
                            {
                                ArrayList getrow = makeCollOrEnumIntoArrayList(linput[rowindex]) as ArrayList;
                                if (colindex > getrow.Count - 1 || colindex < 0)
                                {
                                    outputs[0] = "outbounds";
                                    return;
                                }
                                else if (inputs[3].module != null)
                                {
                                    getrow[colindex] = inputs[3].module.outputs[inputs[3].outParIndex];
                                    linput[rowindex] = getrow;
                                    outputs[0] = linput;
                                }
                            }
                        }
                        catch
                        {
                            outputs[0] = "error";
                            return;
                        }


                    }



                }
            }

            /// <summary>
            /// Gets a row from a PUPPI grid
            /// </summary>
            public class PUPPIGridGetRow : PUPPIModule
            {
                public PUPPIGridGetRow()
                    : base()
                {
                    name = "Get Grid Row";
                    outputs.Add(0);
                    outputnames.Add("Row");
                    inputnames.Add("Grid");
                    inputnames.Add("Row Index");

                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;

                }
                public override void process_usercode()
                {


                    if (inputs[0].module != null)
                    {
                        try
                        {
                            ArrayList linput = inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList;
                            int rowindex = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);

                            if (rowindex > linput.Count - 1 || rowindex < 0)
                            {
                                outputs[0] = "outbounds";
                                return;
                            }
                            else
                            {
                                // ArrayList getrow = linput[rowindex] as ArrayList;

                                // outputs[0] = getrow;
                                object r = linput[rowindex];
                                outputs[0] = r;
                            }
                        }
                        catch
                        {
                            outputs[0] = "error";
                            return;
                        }





                    }
                }
            }


            /// <summary>
            /// Gets a column from a PUPPI Grid as an Arraylist
            /// </summary>
            public class PUPPIGridGetCol : PUPPIModule
            {
                public PUPPIGridGetCol()
                    : base()
                {
                    name = "Get Grid Column";
                    outputs.Add(0);
                    outputnames.Add("Column");
                    inputnames.Add("Grid");
                    inputnames.Add("Col Index");

                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;

                }
                public override void process_usercode()
                {

                    if (inputs[0].module != null)
                    {

                        try
                        {
                            ArrayList linput = inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList;
                            int colindex = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);
                            //test if column number valid
                            if (linput.Count == 0) { outputs[0] = "blank array"; return; }



                            ArrayList frow = makeCollOrEnumIntoArrayList(linput[0]); //= linput[0] as ArrayList;
                            if (colindex > frow.Count - 1 || colindex < 0)
                            {
                                outputs[0] = "outbounds";
                                return;
                            }
                            else
                            {
                                ArrayList mycol = new ArrayList();
                                for (int ro = 0; ro < linput.Count; ro++)
                                {
                                    frow = makeCollOrEnumIntoArrayList(linput[ro]);//linput[ro] as ArrayList;
                                    mycol.Add(frow[colindex]);
                                }
                                outputs[0] = mycol;

                            }
                        }
                        catch
                        {
                            outputs[0] = "error";
                            return;
                        }





                    }
                }
            }



            /// <summary>
            /// Inserts a row provided as a Collection type at specified 0 based row index. If no row index,row is added at the end.
            /// </summary>
            public class PUPPIGridInsertRow : PUPPIModule
            {
                public PUPPIGridInsertRow()
                    : base()
                {
                    description = "Inserts a row provided as a Collection type at specified 0 based row index. If no row index,row is added at the end.";
                    name = "Insert Grid Row";
                    outputs.Add("not set");
                    outputnames.Add("New Grid");

                    inputnames.Add("Grid");
                    inputs.Add(new PUPPIInParameter());


                    inputnames.Add("Collectn");
                    inputs.Add(new PUPPIInParameter());


                    inputnames.Add("Row Index");
                    PUPPIInParameter pi = new PUPPIInParameter();
                    pi.isoptional = true;
                    inputs.Add(pi);
                    completeProcessOverride = true;

                }
                public override void process_usercode()
                {


                    if (inputs[0].module != null && inputs[1].module != null)
                    {
                        object colltoadd = inputs[1].module.outputs[inputs[1].outParIndex];
                        try
                        {
                            //ArrayList linput = inputs[0].module.outputs[inputs[0].outParIndex].DeepClone() as ArrayList;
                            //ArrayList rarray = linput.Clone() as ArrayList;
                            ArrayList rarray = new ArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList);
                            if (inputs[2].module != null)
                            {
                                int rowindex = Convert.ToInt16(inputs[2].module.outputs[inputs[2].outParIndex]);

                                if (rowindex >= rarray.Count - 1 || rowindex < 0)
                                {
                                    outputs[0] = "outbounds";
                                    return;
                                }
                                else
                                {

                                    rarray.Insert(rowindex, colltoadd);
                                    outputs[0] = rarray;
                                }
                            }
                            else
                            {
                                rarray.Add(colltoadd);
                                outputs[0] = rarray;

                            }
                        }
                        catch
                        {
                            outputs[0] = "error";
                            return;
                        }

                    }
                }
            }

            /// <summary>
            /// Inserts a column provided as a Collection type at specified 0 based column index. If no column index,column is added at the end.
            /// </summary>
            public class PUPPIGridInsertCol : PUPPIModule
            {
                public PUPPIGridInsertCol()
                    : base()
                {
                    description = " Inserts a column provided as a Collection type at specified 0 based column index. If no column index,column is added at the end.";
                    name = "Insert Grid Col";
                    outputs.Add("not set");
                    outputnames.Add("New Grid");

                    inputnames.Add("Grid");
                    inputs.Add(new PUPPIInParameter());


                    inputnames.Add("Collectn");
                    inputs.Add(new PUPPIInParameter());


                    inputnames.Add("Col Index");
                    PUPPIInParameter pi = new PUPPIInParameter();
                    pi.isoptional = true;
                    inputs.Add(pi);
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {

                    if (inputs[0].module != null && inputs[1].module != null)
                    {

                        try
                        {
                            ArrayList linput = new ArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList);
                            if (linput.Count == 0) { outputs[0] = "blank array"; return; }
                            //check column length
                            ArrayList acoll = new ArrayList();
                            if (inputs[1].module.outputs[inputs[1].outParIndex] is IEnumerable)
                            {
                                acoll = makeCollOrEnumIntoArrayList(inputs[1].module.outputs[inputs[1].outParIndex]);
                                if (acoll.Count != linput.Count)
                                {
                                    outputs[0] = "Grid and list don't match";
                                    return;
                                }
                            }
                            else
                            {
                                outputs[0] = "Invalid list data";
                                return;
                            }

                            if (inputs[2].module != null)
                            {
                                //test if column index valid
                                int colindex = Convert.ToInt16(inputs[2].module.outputs[inputs[2].outParIndex]);
                                ArrayList frow = makeCollOrEnumIntoArrayList(linput[0]); //= linput[0] as ArrayList;
                                if (colindex >= frow.Count - 1 || colindex < 0)
                                {
                                    outputs[0] = "outbounds";
                                    return;
                                }
                                else
                                {
                                    ArrayList mycol = new ArrayList();
                                    for (int ro = 0; ro < linput.Count; ro++)
                                    {
                                        frow = makeCollOrEnumIntoArrayList(linput[ro]);//linput[ro] as ArrayList;
                                        frow.Insert(colindex, acoll[ro]);
                                        mycol.Add(frow);
                                    }
                                    outputs[0] = mycol;

                                }
                            }
                            else
                            {
                                ArrayList mycol = new ArrayList();
                                for (int ro = 0; ro < linput.Count; ro++)
                                {
                                    ArrayList frow = makeCollOrEnumIntoArrayList(linput[ro]);//linput[ro] as ArrayList;
                                    frow.Add(acoll[ro]);
                                    mycol.Add(frow);
                                }
                                outputs[0] = mycol;
                            }
                        }
                        catch
                        {
                            outputs[0] = "error";
                            return;
                        }





                    }
                }
            }

            /// <summary>
            /// Splits a PUPPI Grid by row index or a collection by item index
            /// </summary>
            public class PUPPIGridSplitByRow : PUPPIModule
            {
                public PUPPIGridSplitByRow()
                    : base()
                {
                    name = "Split Index/Row";
                    description = "Splits a PUPPI Grid by row index or a collection by item index.Returns ArrayLists";
                    outputs.Add(0);
                    outputnames.Add("Grid/ArrList 1");
                    outputs.Add(0);
                    outputnames.Add("Grid/ArrList 2");
                    inputnames.Add("Grid/List");
                    inputnames.Add("Index");

                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;

                }
                public override void process_usercode()
                {


                    if (inputs[0].module != null)
                    {
                        try
                        {
                            ArrayList linput = new ArrayList(makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]));//  //(inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList).Clone() as ArrayList;
                            int rowindex = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);

                            if (rowindex > linput.Count - 1 || rowindex < 0)
                            {
                                outputs[0] = "outbounds";
                                return;
                            }
                            else
                            {
                                ArrayList aa = new ArrayList();
                                ArrayList ba = new ArrayList();
                                for (int i = 0; i < rowindex; i++)
                                {
                                    aa.Add(linput[i]);
                                }
                                for (int i = rowindex; i < linput.Count; i++)
                                {
                                    ba.Add(linput[i]);
                                }
                                outputs[0] = aa;
                                outputs[1] = ba;
                            }
                        }
                        catch
                        {
                            outputs[0] = "error";
                            return;
                        }

                    }
                }
            }


            /// <summary>
            /// Removes a row from a PUPPI grid
            /// </summary>
            public class PUPPIGridRemoveRow : PUPPIModule
            {
                public PUPPIGridRemoveRow()
                    : base()
                {
                    name = "Remove Item/Row";
                    description = "Removes an item from a list or row from a PUPPI grid. Valid 0 based row index needed";
                    outputs.Add(0);
                    outputnames.Add("Grid/ArrList");
                    inputnames.Add("Grid/List");
                    inputnames.Add("Index");

                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;

                }
                public override void process_usercode()
                {


                    if (inputs[0].module != null)
                    {
                        try
                        {
                            ArrayList linput = new ArrayList(makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]));// (inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList).Clone() as ArrayList;
                            int rowindex = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);

                            if (rowindex > linput.Count - 1 || rowindex < 0)
                            {
                                outputs[0] = "outbounds";
                                return;
                            }
                            else
                            {
                                linput.RemoveAt(rowindex);
                                outputs[0] = linput;
                            }
                        }
                        catch
                        {
                            outputs[0] = "error";
                            return;
                        }

                    }
                }
            }


            /// <summary>
            /// Removes a column from a PUPPI Grid.
            /// </summary>
            public class PUPPIGridRemoveCol : PUPPIModule
            {
                public PUPPIGridRemoveCol()
                    : base()
                {
                    name = "Remove Grid Column";
                    description = "Removes a column from a PUPPI Grid based on required valid 0 based index";
                    outputs.Add(0);
                    outputnames.Add("New Grid");
                    inputnames.Add("Grid");
                    inputnames.Add("Col Index");

                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;

                }
                public override void process_usercode()
                {

                    if (inputs[0].module != null)
                    {

                        try
                        {
                            ArrayList linput = new ArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList);
                            int colindex = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);
                            //test if column number valid
                            if (linput.Count == 0) { outputs[0] = "blank array"; return; }



                            ArrayList frow = makeCollOrEnumIntoArrayList(linput[0]); //= linput[0] as ArrayList;
                            if (colindex > frow.Count - 1 || colindex < 0)
                            {
                                outputs[0] = "outbounds";
                                return;
                            }
                            else
                            {
                                ArrayList mycol = new ArrayList();
                                for (int ro = 0; ro < linput.Count; ro++)
                                {
                                    frow = makeCollOrEnumIntoArrayList(linput[ro]);//linput[ro] as ArrayList;
                                    frow.RemoveAt(colindex);
                                    mycol.Add(frow);
                                }
                                outputs[0] = mycol;

                            }
                        }
                        catch
                        {
                            outputs[0] = "error";
                            return;
                        }





                    }
                }
            }


            /// <summary>
            /// Splits a PUPPIGrid by valid column index into two grids
            /// </summary>
            public class PUPPIGridSplitByCol : PUPPIModule
            {
                public PUPPIGridSplitByCol()
                    : base()
                {
                    name = "Split Grid by Col";
                    description = "Splits a PUPPIGrid into two grids based on required valid 0 based column index";
                    outputs.Add(0);
                    outputnames.Add("New Grid 1");
                    outputs.Add(0);
                    outputnames.Add("New Grid 2");
                    inputnames.Add("Grid");
                    inputnames.Add("Col Index");

                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;

                }
                public override void process_usercode()
                {

                    if (inputs[0].module != null)
                    {

                        try
                        {
                            ArrayList linput = new ArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList);
                            int colindex = Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]);
                            //test if column number valid
                            if (linput.Count == 0) { outputs[0] = "blank array"; return; }



                            ArrayList frow = makeCollOrEnumIntoArrayList(linput[0]); //= linput[0] as ArrayList;
                            if (colindex > frow.Count - 1 || colindex < 0)
                            {
                                outputs[0] = "outbounds";
                                return;
                            }
                            else
                            {
                                ArrayList leftside = new ArrayList();
                                ArrayList rightside = new ArrayList();
                                for (int ro = 0; ro < linput.Count; ro++)
                                {
                                    frow = makeCollOrEnumIntoArrayList(linput[ro]);//linput[ro] as ArrayList;
                                    ArrayList lrow = new ArrayList();
                                    ArrayList rrow = new ArrayList();
                                    for (int j = 0; j < colindex; j++)
                                    {
                                        lrow.Add(frow[j]);
                                    }
                                    for (int j = colindex; j < frow.Count; j++)
                                    {
                                        rrow.Add(frow[j]);
                                    }
                                    leftside.Add(lrow);
                                    rightside.Add(rrow);
                                }
                                outputs[0] = leftside;
                                outputs[1] = rightside;

                            }
                        }
                        catch
                        {
                            outputs[0] = "error";
                            return;
                        }





                    }
                }
            }


            /// <summary>
            /// Gets the number of columns in a Grid (ArrayList of Arraylists or typed Lists).
            /// </summary>
            public class PUPPIGridColumnCount : PUPPIModule
            {
                public PUPPIGridColumnCount()
                    : base()
                {
                    name = "Count Grid Columns";
                    outputs.Add(0);
                    description = "Gets the number of columns in a Grid (ArrayList of Arraylists or typed Lists).";
                    outputnames.Add("Grid Item");
                    inputnames.Add("Grid");

                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {


                    if (inputs[0].module != null)
                    {
                        try
                        {
                            ArrayList linput = inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList;
                            if (linput.Count > 0)
                            {
                                ArrayList getrow = makeCollOrEnumIntoArrayList(linput[0]) as ArrayList;
                                outputs[0] = getrow.Count;
                            }
                            else
                            {
                                outputs[0] = "Empty grid";
                            }
                        }
                        catch
                        {
                            outputs[0] = "error";
                            return;
                        }


                    }



                }
            }




            /// <summary>
            /// Transposes a grid (switches rows and columns)
            /// </summary>
            public class PUPPIGridTranspose : PUPPIModule
            {
                public PUPPIGridTranspose()
                    : base()
                {
                    name = "Transpose Grid";
                    description = "Transposes a grid (switches rows and columns)";
                    outputs.Add("not set");
                    outputnames.Add("Transposed");
                    inputnames.Add("Grid");

                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;

                }
                public override void process_usercode()
                {


                    if (inputs[0].module != null)
                    {
                        try
                        {
                            ArrayList linput = new ArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList);

                            if (linput.Count == 0) { outputs[0] = "empty grid"; return; }



                            ArrayList frow = makeCollOrEnumIntoArrayList(linput[0]);
                            int colcount = frow.Count;
                            if (colcount == 0) { outputs[0] = "empty grid"; return; }
                            ArrayList transposed = new ArrayList();
                            for (int j = 0; j < frow.Count; j++)
                            {
                                ArrayList mynewrow = new ArrayList();
                                for (int i = 0; i < linput.Count; i++)
                                {
                                    ArrayList myoldrow = makeCollOrEnumIntoArrayList(linput[i]);
                                    mynewrow.Add(myoldrow[j]);
                                }
                                transposed.Add(mynewrow);
                            }

                            outputs[0] = transposed;


                        }
                        catch
                        {
                            outputs[0] = "error";
                            return;
                        }





                    }
                }
            }

            /// <summary>
            /// Converts a 1D Arraylist to a PUPPI Grid based on specified number of rows and columns, with padding if needed on last row.
            /// </summary>
            public class PUPPIList2Grid : PUPPIModule
            {
                public PUPPIList2Grid()
                    : base()
                {
                    name = "List->Grid";
                    description = "Converts a 1D list into a 2D grid.\nIf not exact division by number of rows, last row gets\n repeated last value added to match number columns.";
                    outputs.Add(0);
                    outputnames.Add("Grid");
                    inputnames.Add("List");
                    inputnames.Add("Number Rows");

                    inputs.Add(new PUPPIInParameter());
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;

                }



                public override void process_usercode()
                {



                    try
                    {
                        ArrayList linput = new ArrayList();
                        //if (inputs[0].module.outputs[inputs[0].outParIndex] is ICollection)
                        //    linput = new ArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex])  as ICollection);
                        //else if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                        //    linput = PUPPIModule.makeMeAnArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex])  as IEnumerable);


                        linput = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]);

                        int numberows = Math.Abs(Convert.ToInt16(inputs[1].module.outputs[inputs[1].outParIndex]));
                        //test if column number valid
                        if (linput.Count == 0) { outputs[0] = "blank array"; return; }

                        if (numberows > linput.Count || numberows <= 0)
                        {
                            outputs[0] = "outbounds";
                            return;
                        }
                        else
                        {
                            ArrayList result = new ArrayList();

                            int cols = Convert.ToInt16(Math.Floor((double)(linput.Count) / (double)(numberows)));
                            //to account for last row
                            if (cols * numberows < linput.Count)
                            {
                                cols = Convert.ToInt16(Math.Floor((double)(linput.Count) / (double)(numberows - 1)));
                            }
                            int elindex = 0;

                            while (elindex < linput.Count)
                            {
                                ArrayList mycol = new ArrayList();
                                if (elindex + cols <= linput.Count)
                                {
                                    mycol = linput.GetRange(elindex, cols);
                                    elindex += cols;
                                }
                                else
                                {
                                    for (int i = 0; i < cols; i++)
                                    {
                                        if (elindex + i < linput.Count)
                                        {
                                            mycol.Add(linput[elindex + i]);
                                        }
                                        else
                                        {
                                            mycol.Add(linput[linput.Count - 1]);
                                        }
                                    }
                                    //end the loop
                                    elindex = linput.Count;
                                }
                                result.Add(mycol);

                            }
                            outputs[0] = result;

                        }
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
            /// Converts a PUPPI grid to a 1D Arraylist by putting rows one after another.
            /// </summary>
            public class PUPPIGrid2List : PUPPIModule
            {
                public PUPPIGrid2List()
                    : base()
                {
                    name = "Grid->List";
                    description = "Converts a 2D grid into a 1D list by adding all rows";
                    outputs.Add(0);
                    outputnames.Add("List");
                    inputnames.Add("Grid");


                    inputs.Add(new PUPPIInParameter());

                    completeProcessOverride = true;

                }
                public override void process_usercode()
                {



                    try
                    {
                        ArrayList linput = new ArrayList(inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList);

                        //test if column number valid
                        if (linput.Count == 0) { outputs[0] = "blank grid"; return; }

                        ArrayList result = new ArrayList();
                        for (int i = 0; i < linput.Count; i++)
                        {
                            ArrayList aa = makeCollOrEnumIntoArrayList(linput[i]); //linput[i] as ArrayList;
                            for (int j = 0; j < aa.Count; j++)
                            {
                                result.Add(aa[j]);
                            }
                        }

                        outputs[0] = result;

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
            /// Converts a PUPPI grid to a 2D Array.
            /// </summary>
            public class PUPPIGrid2DArray : PUPPIModule
            {
                public PUPPIGrid2DArray()
                    : base()
                {
                    name = "Grid->2D Array";
                    description = "Converts a PUPPI 2D grid into a 2D Array. All values in grid have to be of same type. Numeric string grids converted to double.";
                    outputs.Add(0);
                    outputnames.Add("2D Array");
                    inputnames.Add("Grid");


                    inputs.Add(new PUPPIInParameter());

                    completeProcessOverride = true;

                }
                public override void process_usercode()
                {

                    if (inputs[0].module != null)
                    {

                        try
                        {
                            ArrayList linput = inputs[0].module.outputs[inputs[0].outParIndex] as ArrayList;

                            //test if column number valid
                            if (linput.Count == 0) { outputs[0] = "blank grid"; return; }
                            ArrayList aa = makeCollOrEnumIntoArrayList(linput[0]);
                            if (aa.Count == 0) { outputs[0] = "blank grid"; return; }
                            object oi = aa[0];
                            //Type tarray = oi.GetType().MakeArrayType(2);
                            int[] lts = new int[2];
                            lts[0] = linput.Count;
                            lts[1] = aa.Count;
                            bool makedouble = false;
                            double lala = 0;
                            if (oi.GetType() == typeof(string))
                            {
                                string sss = oi.ToString();

                                makedouble = double.TryParse(sss, out lala);
                            }
                            if (makedouble == false)
                            {
                                var y = Array.CreateInstance(oi.GetType(), lts);

                                for (int i = 0; i < linput.Count; i++)
                                {
                                    aa = makeCollOrEnumIntoArrayList(linput[i]); //linput[i] as ArrayList;
                                    for (int j = 0; j < aa.Count; j++)
                                    {
                                        y.SetValue(aa[j], new[] { i, j });
                                    }
                                }

                                outputs[0] = y;
                            }
                            else
                            {
                                var y = Array.CreateInstance(typeof(double), lts);

                                for (int i = 0; i < linput.Count; i++)
                                {
                                    aa = makeCollOrEnumIntoArrayList(linput[i]); //linput[i] as ArrayList;
                                    for (int j = 0; j < aa.Count; j++)
                                    {
                                        lala = 0;
                                        makedouble = double.TryParse(aa[j].ToString(), out lala);
                                        y.SetValue(lala, new[] { i, j });
                                    }
                                }

                                outputs[0] = y;

                            }

                        }

                        catch (Exception exy)
                        {
                            outputs[0] = "error: " + exy.ToString();
                            return;
                        }




                    }

                }
            }
            /// <summary>
            /// Converts a 2D Array into a PUPPI Grid (ArrayList of Lists)
            /// </summary>
            public class PUPPI2DArray2Grid : PUPPIModule
            {
                public PUPPI2DArray2Grid()
                    : base()
                {
                    name = "2D Array->Grid";
                    description = "Converts a 2D Array into a PUPPI Grid (ArrayList of Lists)";
                    outputs.Add("not set");
                    outputnames.Add("Grid");
                    inputnames.Add("2D Array");


                    inputs.Add(new PUPPIInParameter());

                    completeProcessOverride = true;

                }
                public override void process_usercode()
                {


                    if (inputs[0].module != null)
                    {
                        try
                        {
                            Array mya = (inputs[0].module.outputs[inputs[0].outParIndex]) as Array;
                            int lbrow = mya.GetLowerBound(0);
                            int lbcol = mya.GetLowerBound(1);
                            int ubrow = mya.GetUpperBound(0);
                            int ubcol = mya.GetUpperBound(1);
                            ArrayList myg = new ArrayList();
                            Type vart = mya.GetValue(lbrow, lbcol).GetType();
                            Type listType = typeof(List<>).MakeGenericType(vart);

                            for (int i = lbrow; i <= ubrow; i++)
                            {

                                IList myro = Activator.CreateInstance(listType) as IList;
                                for (int j = lbcol; j <= ubcol; j++)
                                {
                                    myro.Add(mya.GetValue(i, j));
                                }
                                myg.Add(myro);
                            }
                            outputs[0] = myg;

                        }

                        catch (Exception exy)
                        {
                            outputs[0] = "error: " + exy.ToString();
                            return;
                        }

                    }




                }
            }

            /// <summary>
            /// Removes empty rows from grid or null items from list
            /// </summary>
            public class PUPPIClearEmpty: PUPPIModule
            {
                public PUPPIClearEmpty():
                    base()
                {
                    name = "Clear Empty";
                    description = "Removes empty rows from grid or null items from list";
                    inputs.Add(new PUPPIInParameter());
                    inputnames.Add("Grid or List");
                    outputs.Add(null);
                    outputnames.Add("Pruned Arraylist");
                    completeProcessOverride = true; 
                }
                public override void process_usercode()
                {

                    if (inputs[0].module != null)
                    {

                        if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                        {
                            ArrayList orlist = new ArrayList();
                            orlist = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]);


                            ArrayList results = new ArrayList();
                            try
                            {
                                foreach (object o in orlist )
                                {
                                    if (o == null) continue;
                                    if (o is IEnumerable )
                                    {
                                        ArrayList ao = makeCollOrEnumIntoArrayList(o);
                                        if (ao.Count == 0) continue;
                                    }
                                    results.Add(o);  
                                }
                            }
                            catch
                            {
                                outputs[0] = "error";
                                return;
                            }
                            outputs[0] = results;
                        }
                    }
                }
            }


            /// <summary>
            /// Converts Grid which could be ArrayList of ArrayLists into ArrayList of Typed Lists assuming all elements on row of same type.
            /// </summary>
            public class Grid2TypedLists : PUPPIModule
            {
                public Grid2TypedLists()
                    : base()
                {
                    name = "Grid->Typed Lsts";
                    description = "Converts the typical PUPPI Grid (ArrayList of ArrayLists) to an ArrayList of typed lists based on the type of the first object in each generic list in the grid";
                    outputs.Add("empty");
                    outputnames.Add("ArrayList of Typed Lists");
                    inputnames.Add("Grid");
                    inputs.Add(new PUPPIInParameter());
                    completeProcessOverride = true;
                }
                public override void process_usercode()
                {
                    if (inputs[0].module != null)
                    {
                        try
                        {

                            //check if list to add to exists
                            if (inputs[0].module != null)
                            {
                                if (inputs[0].module.outputs[inputs[0].outParIndex] is IEnumerable)
                                {
                                    ArrayList orlist = new ArrayList();
                                    //if (inputs[0].module.outputs[inputs[0].outParIndex]  is ICollection)
                                    //    orlist = new ArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex])  as ICollection);
                                    //else if (inputs[0].module.outputs[inputs[0].outParIndex]  is IEnumerable)
                                    //    orlist = PUPPIModule.makeMeAnArrayList(PUPPIModel.Utilities.CloneObject(inputs[0].module.outputs[inputs[0].outParIndex])  as IEnumerable);
                                    orlist = makeCollOrEnumIntoArrayList(inputs[0].module.outputs[inputs[0].outParIndex]);
                                    if (orlist.Count > 0)
                                    {
                                        ArrayList allitems = new ArrayList();
                                        for (int oic = 0; oic < orlist.Count; oic++)
                                        {
                                            ArrayList mlist = new ArrayList();
                                            object o = orlist[oic];
                                            if (o is IEnumerable)
                                            {
                                                if (o is ICollection)
                                                    mlist = new ArrayList(o as ICollection);
                                                else if (o is IEnumerable)
                                                    mlist = PUPPIModule.makeMeAnArrayList(o as IEnumerable);
                                                if (mlist.Count > 0)
                                                {
                                                    object oo = mlist[0];
                                                    Type t = oo.GetType();
                                                    var listType = typeof(List<>);
                                                    var constructedListType = listType.MakeGenericType(t);

                                                    var instance = (IList)Activator.CreateInstance(constructedListType);

                                                    for (int iii = 0; iii < mlist.Count; iii++)
                                                    {
                                                        instance.Add(mlist[iii]);
                                                    }

                                                    allitems.Add(instance);
                                                }
                                            }
                                            else
                                            {
                                                Type t = o.GetType();
                                                var listType = typeof(List<>);
                                                var constructedListType = listType.MakeGenericType(t);

                                                var instance = (IList)Activator.CreateInstance(constructedListType);
                                                instance.Add(o);
                                                allitems.Add(instance);
                                            }

                                        }
                                        outputs[0] = allitems;
                                    }
                                }
                                else
                                {
                                    outputs[0] = "Invalid input type";
                                }
                            }
                        }
                        catch
                        {
                            outputs[0] = "error";
                        }
                    }
                }

            }
        }


    }
    //    namespace PUPPIScripting
    //    {
    //        internal class PUPPIRuntimeScript
    //        {
    //            internal ArrayList inputsFromNode;
    //            internal ArrayList resultOutputs;
    //            //xml
    //            internal string storedScriptData;
    //            internal List<string> assemblyPaths;
    //            internal List<string> assemblyNameSpaces;
    //            internal List<string> assemblyNames;
    //            internal string justTheScript;
    //            internal string scriptProgrammingLanguage;
    //            internal string aS = "";
    //            internal bool autoLoadScriptNamespaces;
    //            internal PUPPIRuntimeScript()
    //            {
    //                scriptProgrammingLanguage = "C#";
    //                storedScriptData = "";
    //                justTheScript = "";
    //                inputsFromNode = new ArrayList();
    //                resultOutputs = new ArrayList();
    //                assemblyPaths = new List<string>();
    //                assemblyNameSpaces = new List<string>();
    //                assemblyNames = new List<string>();
    //                autoLoadScriptNamespaces = true;
    //            }
    //            internal void setUpMyScript()
    //            {
    //                if (storedScriptData != "")
    //                {
    //                    XmlDocument doc = new XmlDocument();
    //                    try
    //                    {
    //                        doc.LoadXml(storedScriptData);
    //                    }
    //                    catch
    //                    {
    //                        throw new Exception("Invalid script data");
    //                    }
    //                    XmlNode sasc = doc.SelectSingleNode("/savedNodeScript");
    //                    for (int i = 0; i < sasc.ChildNodes.Count; i++)
    //                    {
    //                        XmlNode chi = sasc.ChildNodes[i];
    //                        if (chi.Name == "scriptProgLang")
    //                        {
    //                            scriptProgrammingLanguage = chi.InnerText;
    //                        }
    //                        if (chi.Name == "scriptProgram")
    //                        {
    //                            justTheScript = chi.InnerText;
    //                        }
    //                        if (chi.Name == "namespaceLoad")
    //                        {
    //                            try
    //                            {
    //                                autoLoadScriptNamespaces = Convert.ToBoolean(chi.InnerText);
    //                            }
    //                            catch
    //                            {
    //                                autoLoadScriptNamespaces = true;
    //                            }
    //                        }

    //                        if (chi.Name == "loadedAssemblyDLL")
    //                        {

    //                            try
    //                            {
    //                                Assembly a = Assembly.LoadFile(chi.InnerText);
    //                                if (a != null)
    //                                {
    //                                    List<string> an = getAssemblyNamespaceNames(a);
    //                                    for (int j = 0; j < an.Count; j++)
    //                                    {
    //                                        if (!assemblyNameSpaces.Contains(an[j])) assemblyNameSpaces.Add(an[j]);
    //                                    }

    //                                    assemblyPaths.Add(chi.InnerText);
    //                                    //assemblyNames.Add(a.FullName);
    //                                }

    //                            }
    //                            catch
    //                            {
    //                                MessageBox.Show("Failed to load assembly " + chi.InnerText + " into script");

    //                            }

    //                        }
    //                    }
    //                }
    //            }
    //            //changes the node with the prescribed number of inputs and outputs
    //            //only on load
    //            internal void setUpScriptInputsOutputs()
    //            {

    //            }


    //            static List<string> myPaths;

    //            static Assembly LoadFromSavedFolder(object sender, ResolveEventArgs args)
    //            {
    //                foreach (string assemblyPath in myPaths)
    //                {

    //                    string aN = new AssemblyName(args.Name).Name + ".dll";
    //                    //Console.WriteLine(aN);
    //                    if (assemblyPath.Contains(aN))
    //                    {

    //                        if (File.Exists(assemblyPath) == false) return null;
    //                        Assembly assembly = Assembly.LoadFrom(assemblyPath);

    //                        return assembly;
    //                    }
    //                }
    //                return null;

    //            }


    //            internal void runMyScript(string scriptText)
    //            {
    //                CompilerResults results;
    //                string myScript = scriptText;
    //                string source;
    //                #region C#
    //                myPaths = new List<string>();

    //                foreach (string aP in assemblyPaths)
    //                {
    //                    myPaths.Add(aP);

    //                }


    //                if (scriptProgrammingLanguage == "C#")
    //                {
    //                    source = @"namespace myPUPPIScriptNamespace
    //{
    //using System;";
    //                    source += "using System.Collections;\n";
    //                    //source += "using System.Collections.Generic;\n";
    //                    if (autoLoadScriptNamespaces)
    //                        for (int ume = 0; ume < assemblyNameSpaces.Count; ume++)
    //                        {
    //                            source += "using " + assemblyNameSpaces[ume] + ";\n";
    //                        }
    //                    source += @"
    //  
    //    public class myPUPPIScriptClass
    //    {
    //
    //        public ArrayList nodeInputs {get;set;}
    //        public ArrayList scriptOutputs {get;set;}
    //        public myPUPPIScriptClass()
    //        {
    //                nodeInputs=new ArrayList();
    //                scriptOutputs=new ArrayList();
    //
    //";
    //                    source += @"
    //        }
    //        public void myPUPPIScriptFunction()
    //        {
    //            "
    //                             + @myScript +
    //                             " }  }} ";

    //                    Dictionary<string, string> providerOptions = new Dictionary<string, string>
    //                {
    //                    {"CompilerVersion", "v3.5"}
    //                };
    //                    CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

    //                    CompilerParameters compilerParams = new CompilerParameters
    //                    {
    //                        GenerateInMemory = true,
    //                        GenerateExecutable = false
    //                    };

    //                    compilerParams.ReferencedAssemblies.Add("System.dll");
    //                    foreach (string aP in assemblyPaths)
    //                    {
    //                        //string aa = aP.Replace(@"\\", @"\");
    //                        compilerParams.ReferencedAssemblies.Add(aP);

    //                    }

    //                    aS = source;
    //                    results = provider.CompileAssemblyFromSource(compilerParams, source);


    //                }
    //                #endregion
    //                #region VB
    //                else
    //                {
    //                    source =
    //                    @"imports System
    //imports System.Collections";
    //                    if (autoLoadScriptNamespaces)
    //                        for (int ume = 0; ume < assemblyNameSpaces.Count; ume++)
    //                        {
    //                            source += "imports " + assemblyNameSpaces[ume] + "\n";
    //                        }
    //                    source += @"
    //namespace myPUPPIScriptNamespace
    //    public class myPUPPIScriptClass
    //    
    //    Private _nodeInputs As ArrayList
    //
    //    Public Property nodeInputs() As ArrayList
    //	    Get
    //	     Return _nodeInputs
    //	    End Get
    //	    Set(ByVal value As ArrayList)
    //	        _nodeInputs = value
    //	    End Set
    //    End Property
    //
    // Private _scriptOutputs As ArrayList
    //
    //    Public Property scriptOutputs() As ArrayList
    //	    Get
    //	     Return _scriptOutputs
    //	    End Get
    //	    Set(ByVal value As ArrayList)
    //	        _scriptOutputs = value
    //	    End Set
    //    End Property
    //        
    //        public Sub New()
    //                _nodeInputs=new ArrayList()
    //                _scriptOutputs=new ArrayList()   
    //        End Sub
    //
    //        public Sub myPUPPIScriptFunction()
    //        
    //            "
    //                    + @myScript +
    //                    " \nEnd Sub \n End Class\n End Namespace";

    //                    Dictionary<string, string> providerOptions = new Dictionary<string, string>
    //                {
    //                    {"CompilerVersion", "v3.5"}
    //                };
    //                    VBCodeProvider provider = new VBCodeProvider(providerOptions);

    //                    CompilerParameters compilerParams = new CompilerParameters
    //                    {
    //                        GenerateInMemory = true,
    //                        GenerateExecutable = false
    //                    };

    //                    foreach (string aP in assemblyPaths)
    //                    {
    //                        //string aa = aP.Replace(@"\\", @"\");
    //                        compilerParams.ReferencedAssemblies.Add(aP);
    //                    }

    //                    aS = source;
    //                    results = provider.CompileAssemblyFromSource(compilerParams, source);


    //                }
    //                #endregion
    //                if (results.Errors.Count != 0)
    //                {
    //                    string compilationErrors = "Compile failed!\n";
    //                    foreach (CompilerError ce in results.Errors)
    //                    {
    //                        compilationErrors += ce.ErrorText + " at line:" + ce.Line.ToString() + "\n";
    //                    }
    //                    //compilationErrors += source;
    //                    MessageBox.Show(compilationErrors);
    //                    return;
    //                }
    //                AppDomain currentDomain = AppDomain.CurrentDomain;
    //                try
    //                {

    //                    currentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromSavedFolder);
    //                    object o = results.CompiledAssembly.CreateInstance("myPUPPIScriptNamespace.myPUPPIScriptClass");
    //                    Type t = o.GetType();
    //                    MethodInfo mi = t.GetMethod("myPUPPIScriptFunction");

    //                    //set inputs
    //                    PropertyInfo iPropInfo = t.GetProperty("nodeInputs", BindingFlags.Instance | BindingFlags.Public |
    //                            BindingFlags.NonPublic);
    //                    iPropInfo.SetValue(o, inputsFromNode);


    //                    mi.Invoke(o, null);
    //                    PropertyInfo oPropInfo = t.GetProperty("scriptOutputs", BindingFlags.Instance | BindingFlags.Public |
    //                            BindingFlags.NonPublic);
    //                    var oValue = oPropInfo.GetValue(o, null);
    //                    resultOutputs = oValue as ArrayList;

    //                }
    //                catch (Exception exy)
    //                {
    //                    MessageBox.Show("Script run error!\n" + exy.ToString());
    //                }
    //                currentDomain.AssemblyResolve -= new ResolveEventHandler(LoadFromSavedFolder);
    //            }
    //            public class Stage : MarshalByRefObject
    //            {
    //                public void LoadAssembly(Byte[] data)
    //                {
    //                    Assembly.Load(data);
    //                }
    //            }
    //            void loadAllAssembliesAndReferencedRecursive(string aP)
    //            {
    //                string aa = aP.Replace(@"\\", @"\");

    //                try
    //                {


    //                    Assembly aaa = Assembly.LoadFile(aP);
    //                    Type toa = aaa.GetTypes()[0];
    //                    object oa = System.Activator.CreateInstance(toa);
    //                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();


    //                    foreach (AssemblyName aaaa in aaa.GetReferencedAssemblies())
    //                    {
    //                        var assembly = (from a in assemblies
    //                                        where a.FullName == aaaa.FullName
    //                                        select a).SingleOrDefault();
    //                        if (assembly == null)
    //                        {
    //                            try
    //                            {
    //                                //assume gac for now
    //                                string saaaa = Assembly.ReflectionOnlyLoad(aaaa.FullName).Location;
    //                                loadAllAssembliesAndReferencedRecursive(saaaa);
    //                            }
    //                            catch
    //                            {

    //                            }
    //                        }
    //                    }
    //                }
    //                catch
    //                {

    //                }
    //            }

    //            class ProxyDomain : MarshalByRefObject
    //            {
    //                public Assembly GetAssembly(string assemblyPath)
    //                {
    //                    try
    //                    {
    //                        return Assembly.LoadFrom(assemblyPath);
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        throw new InvalidOperationException(ex.Message);
    //                    }
    //                }
    //            }

    //            //saves the stuff in node script format
    //            internal void saveScriptForStorage()
    //            {
    //                XmlDocument doc = new XmlDocument();
    //                XmlNode mn = doc.CreateNode(XmlNodeType.Element, "savedNodeScript", "");
    //                XmlNode n;
    //                n = doc.CreateNode(XmlNodeType.Element, "scriptProgLang", "");
    //                n.InnerText = scriptProgrammingLanguage;
    //                mn.AppendChild(n);
    //                n = doc.CreateNode(XmlNodeType.Element, "scriptProgram", "");
    //                n.InnerText = justTheScript;
    //                mn.AppendChild(n);
    //                for (int k = 0; k < assemblyPaths.Count; k++)
    //                {
    //                    n = doc.CreateNode(XmlNodeType.Element, "loadedAssemblyDLL", "");
    //                    n.InnerText = assemblyPaths[k];
    //                    mn.AppendChild(n);

    //                }
    //                storedScriptData = mn.OuterXml;

    //            }
    //            internal static List<string> getAssemblyNamespaceNames(Assembly myAssembly)
    //            {
    //                //goes through namespaces and types
    //                List<string> namespaceNames = new List<string>();

    //                foreach (var type in myAssembly.GetTypes())
    //                {
    //                    string ns = type.Namespace;
    //                    if (ns != null && !namespaceNames.Contains(ns) && ns.Contains("<") == false)
    //                    {
    //                        namespaceNames.Add(ns);

    //                    }

    //                }
    //                return namespaceNames;
    //            }
    //        }
    //    }
    internal static class ExtensionMethods
    {
        // Deep clone
        internal static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    public static class Utilities
    {
        /// <summary>
        /// Clones a class instance or primitive, so that it can be passed between nodes without altering original.
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Clone</returns>
        public static object CloneObject(object obj)
        {
            return PUPPIModule.DeepCopy(obj, 0);
        }

        internal static ArrayList commandObjs;
        /// <summary>
        /// Sends a command to the canvas as a string, which gets executed after all modules process. Will only work if called from inside module process usercode overriden method.
        /// Commands issued as text have the same syntax as GUI controller commands
        /// </summary>
        /// <param name="text">Format is CommandBody_|_argument1_|_argument2 etc</param>
        /// <returns>command result</returns>
        public static void sendCommandToCanvas(string text)
        {

            if (PUPPIModel.PUPPIModule.useMyPUPPIcanvas != null)
            {
                PUPPIModel.PUPPIModule.useMyPUPPIcanvas.commandQueue.Add(text);
            }
        }
        /// <summary>
        /// Gets the GUID of the node added last on the canvas or -1
        /// </summary>
        /// <returns>GUID as integer</returns>
        public static int getLastAddedNodeGUID()
        {
            if (PUPPIModel.PUPPIModule.useMyPUPPIcanvas != null)
            {
                return (PUPPICanvas.ModViz3DNode.GUIDcount - 1);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Gets the results of the last batch of commands sent to the canvas as text
        /// </summary>
        /// <returns>List of command results as they were executed or null if no canvas</returns>
        public static List<string> getScriptCommandResults()
        {
            if (PUPPIModel.PUPPIModule.useMyPUPPIcanvas != null)
            {
                return new List<string>(PUPPIModel.PUPPIModule.useMyPUPPIcanvas.commandQueueResults);
            }
            else
            {
                return null;
            }
        }
        /*
        /// <summary>
        /// Gets the GUID list of the nodes added last on the canvas
        /// </summary>
        /// <param name="numberNodes">number of nodes to reveal, if greater than number of nodes on canvas all nodes returned</param>
        /// <returns>GUIDs as integer list or null for null canvas</returns>
        public static List<int> getLastAddedGUIDs(int numberNodes)
        {
            if (PUPPIModel.PUPPIModule.useMyPUPPIcanvas != null)
            {

            }
            else
            {
                return null;
            }
        }*/

        //if (PUPPIModel.PUPPIModule.useMyPUPPIcontroller!=null   )
        //{
        //    return PUPPIModel.PUPPIModule.useMyPUPPIcontroller.sendCommandAsText(text);
        //}
        //else
        //{
        //    return "No Canvas";
        //}

    }
}


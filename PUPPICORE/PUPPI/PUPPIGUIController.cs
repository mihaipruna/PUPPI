using System;
using System.Collections.Generic;
using System.Collections;
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
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Reflection;
using PUPPIGUI;
using System.Diagnostics;
using HelixToolkit;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Threading;
namespace PUPPIGUIController
{
    /// <summary>
    /// The <see cref="PUPPIGUIController"/> namespace contains methods to control the layout of the graphical user interface, including  the canvas for
    ///nodes, menus, the map view and tree view, and the 3d modeling window
    /// </summary>

    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }

    ///<summary>
    ///Instance of a PUPPI Program Canvas window where menu modules can be dragged to create visual programming nodes.
    ///Can be used with the PUPPIGUIController to create Windows Forms of WPF controls.
    ///</summary>
    public class PUPPIProgramCanvas
    {

       

        internal PUPPICanvas pcanvas;
        ///<summary>
        /// Access to the WPF User Control containing the PUPPI Program Canvas. Note that the User Control
        /// can only be initialized through the PUPPIProgramCanvas class.
        ///</summary>
        public PUPPICanvasControl myuserWPFControl { get; internal set; }
        ///<summary>
        /// Access to the Form Control containing the PUPPI Program Canvas, added diretcly to a Windows Form. 
        /// This is initialized when the static method AddPUPPIProgramCanvastoForm  is used.
        ///</summary>
        public ElementHost myFormControl { get; internal set; }
        ///<summary>
        /// Initializes an instance of the PUPPI Program Canvas. User Controls for WPF are also instantiated. 
        ///To add this to a regular Windows Form, you need to use the static method AddPUPPIProgramCanvastoForm
        /// <param name="width">Width of the canvas in px.Default 800. This dimension applies to form element. WPF element stretches.</param>
        /// <param name="height">Height of the canvas in px.Default 600.This dimension applies to form element. WPF element stretches.</param>
        /// <param name="startLocked">Starts the canvas in locked mode (no editing, hidden nodes and connections not shown). Default is false.</param>
        ///</summary>
        public PUPPIProgramCanvas(int width = 800, int height = 600, bool startLocked = false)
        {
            // System.Windows.Forms.MessageBox.Show("asse");     
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

            PUPPIProgramCanvas1(width, height, startLocked);

        }

        private void PUPPIProgramCanvas1(int width = 800, int height = 600, bool locked = false)
        {
 
           
            myuserWPFControl = new PUPPICanvasControl();
            pcanvas = new PUPPICanvas(0, 0, width, height, locked);

            //    myuserWPFControl.Width = width;
            //      myuserWPFControl.Height = height;
            //   myuserWPFControl.UserControlCanvas.Width = width;

            //   myuserWPFControl.UserControlCanvas.Height = height; 
            //  myuserWPFControl.UserControlCanvas.Background = new SolidColorBrush(Colors.White);  
            myuserWPFControl.UserControlCanvas.Children.Add(pcanvas.canvasview);
            //gets a reference of the canvas in the PUPPIModel
            PUPPIModel.PUPPIModule.useMyPUPPIcanvas = pcanvas;
            PUPPIModel.PUPPIModule.useMyPUPPIcontroller = this;
            //  PUPPIModel.PUPPIModule.GUIController = this;  

            pcanvas.clickedCanvasEvent += (sender, args) => { askedCanvasClickEvent(sender); };
            // pcanvas.droppedCanvasEvent += (sender, args) => { askedCanvasDropEvent(sender); };
            pcanvas.newCanvasEvent += (sender, args) => { askedCanvasNewEvent(); };
            pcanvas.loadCanvasEvent += (sender, args) => { askedCanvasloadEvent(); };
            pcanvas.importCanvasEvent += (sender, args) => { askedCanvasimportEvent(); };
            pcanvas.unlockCanvasEvent += (sender, args) => { askedCanvasUnlockEvent(); };
            pcanvas.lockCanvasEvent += (sender, args) => { askedCanvasLockEvent(); };

        }

        /// <summary>
        /// Adds a PUPPIModule to a searchable list of modules which can be used to set node inputs. Useful if Module Keep menus are not defined.
        /// </summary>
        /// <param name="myModule"></param>
        public void addPUPPIModuleSearchInstance(PUPPIModel.PUPPIModule myModule)
        {
            if (pcanvas != null)
            {
                if (pcanvas.cvavailablePUPPIModules != null)
                {
                    if (pcanvas.cvavailablePUPPIModules.ContainsKey(myModule.name) == false)
                    {
                        pcanvas.cvavailablePUPPIModules.Add(myModule.name, myModule);
                    }
                }
            }
        }
        /// <summary>
        /// Set if connections lines between nodes are updated. Set it to false when adding larg numbers of nodes for instance, then set it to true when done.
        /// Setting it to true causes the conenctions to refresh
        /// </summary>
        internal bool performNodeConnectionRenderUpdate
        {
            get
            {
                if (pcanvas != null)
                {
                    return pcanvas.blockconnupdates;
                }
                return false;
            }
            set
            {
                if (pcanvas != null)
                {
                    pcanvas.blockconnupdates = value;
                    if (value == true)
                    {
                        pcanvas.regenerate_connections(true);
                    }

                }
            }
        }
        /// <summary>
        /// Sends a command to the canvas as a string, to be executed immediately. 
        /// Commands issued as text have the same syntax as GUI controller commands
        /// </summary>
        /// <param name="text">Format is CommandBody_|_argument1_|_argument2 etc</param>
        /// <returns>command result</returns>
        public string sendImmediateCommandAsText(string text)
        {
            return pcanvas.interpretMyTextualCommand(text);
        }


        /// <summary>
        /// Gets a report that is human readable with number of nodes, connections etc
        /// </summary>
        /// <returns>Report as a multiline string</returns>
        public string canvasReadableReport()
        {
            if (pcanvas == null) return "null Canvas";
            return pcanvas.readMyCanvasStatus();

        }
        /// <summary>
        /// Gets nodes and connections bounding boxes, captions and points respectively. This is lighter than a full saved canvas and includes connection point positions.
        /// Example return (XML format):
        /// |canvas|
        ///     |node|
        ///         |details|[Node ID:]0_|_[Caption:]Terminator_|_[Module:]PUPPIModel.PUPPIPremadeModules.LogicModules.PUPPITERMINATOR_|_[Bounding Box:]-1,-1.5,0,1,1,0.5_|_[Parent:]null_|_[Number Children:]0_|_[Hidden On Locked Canvas:]False_|_[Node Custom Renderer:]null|/details|
        ///         |input|[Bounding Box:]-1.1,-1.1,0.25,0.2,0.2,0.251_|_[Name:]Input_|_[Value:]2|/input|
        ///     |/node|
        ///     |node|
        ///         |details|[Node ID:]1_|_[Caption:]Number input_|_[Module:]PUPPIModel.PUPPIPremadeModules.DataInputModules.PUPPINumber_|_[Bounding Box:]-3,-1.5,0,1,1,0.5_|_[Parent:]null_|_[Number Children:]0_|_[Hidden On Locked Canvas:]False_|_[Node Custom Renderer:]null|/details|
        ///         |output|[Bounding Box:]-2.1,-1.1,0.25,0.2,0.2,0.251_|_[Name:]Value_|_[Value:]2|/output|
        ///     |/node|
        ///     |conn|[ID:]conn_1_0_0_0_|_[SNodeID:]1_|_[DNodeID:]0_|_[OutputIndex:]0_|_[InputIndex:]0_|_[Points:]{-1.90958333333333,-1.09708333333333,0.263:|:-1.87496794871795,-1.09708333333333,0.263:|:-1.87496794871795,-1.09708333333333,0.1:|:-1.61189102564103,-1.06208333333333,0.1:|:-1.55489102564103,-1.05482083333333,0.1:|:-1.53296794871795,-1.05219583333333,0.1:|:-1.46573717948718,-1.04458333333333,0.1:|:-1.14419871794872,-1.00958333333333,0.1:|:-1.14419871794872,-1.00958333333333,0.3755:|:-1.10958333333333,-1.00958333333333,0.3755}|/conn|
        ///|/canvas|
        ///</summary>
        /// <returns>XML string</returns>
        public string XMLcanvasLayout()
        {
            return pcanvas.saveCanvasRepresentationToXML();
        }

        /// <summary>
        /// Gets the names of PUPPIModules indexed in the canvas.
        /// </summary>
        /// <returns>List of strings</returns>
        public List<String> getIndexedModuleNames()
        {
            List<string> pumok = new List<string>();
            foreach (string s in pcanvas.cvavailablePUPPIModules.Keys)
            {
                pumok.Add(pcanvas.cvavailablePUPPIModules[s].GetType().FullName);
            }
            return pumok;
        }

        /// <summary>
        /// Inquires if the canvas is locked to user editing.
        /// </summary>
        /// <returns>Boolea canvas lock status</returns>
        public bool getCanvasLocked()
        {
            return pcanvas.thiscanvasdefinitelylocked;
        }
        /// <summary>
        /// Sets canvas locked to user editing
        /// </summary>
        /// <param name="lockStatus">Boolean, true for lock, false to unlock</param>
        public void setCanvasLocked(bool lockStatus)
        {
            pcanvas.setthiscanvaslockstatus(lockStatus);
        }

        /// <summary>
        /// Event raised when a new canvas command is given
        /// </summary>
        public event EventHandler newCanvasOperationEvent;
        internal void askedCanvasNewEvent()
        {
            if (newCanvasOperationEvent != null) newCanvasOperationEvent(this, EventArgs.Empty);
        }



        /// <summary>
        /// Event raised when canvas clicked on. sender.ToString() gives the Node or Inputr/Output ID and LB or RB
        /// </summary>
        public event EventHandler clickCanvasOperationEvent;
        internal void askedCanvasClickEvent(object es)
        {
            if (clickCanvasOperationEvent != null && es != null) clickCanvasOperationEvent(es, EventArgs.Empty);
        }

        ///// <summary>
        ///// Event raised when canvas dropped on. sender.ToString() gives the Node or Inputr/Output ID
        ///// </summary>
        //public event EventHandler dropCanvasOperationEvent;
        //internal void askedCanvasDropEvent(object es)
        //{
        //    if (dropCanvasOperationEvent != null) dropCanvasOperationEvent(es, EventArgs.Empty);
        //}

        /// <summary>
        /// Event raised when a load canvas command is given
        /// </summary>
        public event EventHandler loadCanvasOperationEvent;
        internal void askedCanvasloadEvent()
        {
            if (loadCanvasOperationEvent != null) loadCanvasOperationEvent(this, EventArgs.Empty);
        }
        /// <summary>
        /// Event raised when a import canvas command is given
        /// </summary>
        public event EventHandler importCanvasOperationEvent;
        internal void askedCanvasimportEvent()
        {
            if (importCanvasOperationEvent != null) importCanvasOperationEvent(this, EventArgs.Empty);
        }
        /// <summary>
        /// Event raised when a lock canvas command is given
        /// </summary>
        public event EventHandler lockCanvasOperationEvent;
        internal void askedCanvasLockEvent()
        {
            if (lockCanvasOperationEvent != null) lockCanvasOperationEvent(this, EventArgs.Empty);
        }
        /// <summary>
        /// Event raised when a unlock canvas command is given
        /// </summary>
        public event EventHandler unlockCanvasOperationEvent;
        internal void askedCanvasUnlockEvent()
        {
            if (unlockCanvasOperationEvent != null) unlockCanvasOperationEvent(this, EventArgs.Empty);
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
        /// Read access to visual (non-interacting) elements placed programmatically on the canvas.
        /// </summary>
        public List<PUPPICustomRenderer> visualElements { get; internal set; }
        /// <summary>
        /// Updates the static (non-interacting) elements placed on the canvas.
        /// </summary>
        /// <param name="cRenderer">a PUPPI Custom Renderer object or null</param>
        public void updateCanvasVisualElements(PUPPICustomRenderer cRenderer)
        {
            if (pcanvas != null)
            {
                pcanvas.staticVisuals = cRenderer;
                pcanvas.renderStaticVisual();
            }
        }
        /// <summary>
        /// Adds a node to the PUPPI Canvas based on position coordinates and a PUPPI Module
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="myModule">valid PUPPIModule</param>
        /// <returns>ID of added node or -1 if it failed to add</returns>
        public string addNodeToCanvasCommand(double x, double y, PUPPIModel.PUPPIModule myModule)
        {
            if (pcanvas == null) return "-1";
            Point3D p = new Point3D(x, y, 0);
            return pcanvas.addANodeToTheCanvas(p, myModule).ToString();
        }

        /// <summary>
        /// Adds a node on top of a node. Only works on a 3D canvas and if  base node accepts children
        /// </summary>
        /// <param name="baseNodeID">Valid ID</param>
        /// <param name="myModule">PUPPIModule</param>
        /// <returns>New node ID or "-1" if failed</returns>
        public string addNodeOnNodeCommand(string baseNodeID, PUPPIModel.PUPPIModule myModule)
        {
            if (pcanvas == null) return "-1";
            int nn = -1;
            try
            {
                nn = Convert.ToInt16(baseNodeID);
            }
            catch
            {
                nn = -1;
            }
            if (nn == -1) return "-1";
            if (myModule == null) return "-1";
            return pcanvas.addNodeOnTopOfNodeToTheCanvas(nn, myModule).ToString();
        }
        /// <summary>
        /// Gets the file name and path  of the program curently loaded on the canvas.
        /// </summary>
        /// <returns>File path</returns>
        public string getCanvasLoadedFile()
        {
            if (pcanvas == null) return "";
            return pcanvas.fileCurLdd;
        }


        /// <summary>
        /// Gets the results of the last batch of commands sent to the canvas as text
        /// </summary>
        /// <returns>List of command results as they were executed or null if no canvas</returns>
        public List<string> getScriptCommandResults()
        {
            if (pcanvas != null)
            {
                return new List<string>(pcanvas.commandQueueResults);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Returns the node custom renderer elements in their current state
        /// </summary>
        /// <param name="nodeID">valid node ID as string</param>
        /// <returns>string with XML representation of custom renderer</returns>
        public string getNodeRendererStateXML(string nodeID)
        {
            if (pcanvas == null) return "nullcanvas";
            if (pcanvas.stacks.ContainsKey(nodeID) == false) return "notfound";
            try
            {
                PUPPICanvas.ModViz3DNode mb = pcanvas.stacks[nodeID];
                if (mb.nodeCustomRenderer == null) return "nullRenderer";
                return mb.nodeCustomRenderer.saveCustomRendererStateToXML();
            }
            catch
            {
                return "error";
            }
        }


        //            /// <summary>
        //            /// Returns the node custom renderer elements in their current state
        //            /// </summary>
        //            /// <param name="nodeID">valid node ID as string</param>
        //            /// <returns>string with XML representation of custom renderer</returns>
        //            public string getNodeRendererImage(string nodeID)
        //            {
        //                if (pcanvas == null) return "nullcanvas";
        //                if (pcanvas.stacks.ContainsKey(nodeID) == false) return "notfound";
        //                try
        //                {
        //                    PUPPICanvas.ModViz3DNode mb = pcanvas.stacks[nodeID];
        //                    if (mb.nodeCustomRenderer == null) return "nullRenderer";
        //                    Visual3D v = mb.nodeCustomRenderer.model3D as Visual3D;
        //                    RenderTargetBitmap renderTargetBitmap =
        //new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);
        //                    renderTargetBitmap.Render(v.);
        //                    PngBitmapEncoder pngImage = new PngBitmapEncoder();
        //                    pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
        //                }
        //                catch
        //                {
        //                    return "error";
        //                }
        //            }

        /// <summary>
        /// Gets the time the canvas was changed last
        /// </summary>
        /// <returns>DateTime in string format</returns>
        public string getCanvasChangedTime()
        {
            if (pcanvas == null) return "nullcanvas";
            return (pcanvas.lastChanged.ToString());
        }

        /// <summary>
        /// Asks a node to perform its module gesture override operation
        /// </summary>
        /// <param name="nodeID">valid node ID</param>
        /// <param name="startXRatio">Gesture start X ratio related to node dimensions</param>
        /// <param name="startYRatio">Gesture start Y ratio related to node dimensions</param>
        /// <param name="startZRatio">Gesture start Z ratio related to node dimensions</param>
        /// <param name="endXRatio">Gesture end X ratio related to node dimensions</param>
        /// <param name="endYRatio">Gesture end Y ratio related to node dimensions</param>
        /// <param name="endZRatio">Gesture end Z ratio related to node dimensions</param>
        public void performNodeGesture(string nodeID, double startXRatio, double startYRatio, double startZRatio, double endXRatio, double endYRatio, double endZRatio)
        {
            if (pcanvas == null) return;
            pcanvas.doAGestureOnANode(nodeID, startXRatio, startYRatio, startZRatio, endXRatio, endYRatio, endZRatio);
        }
        /// <summary>
        /// Asks a node to perform its module double click override operation
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="clickXRatio">Double Click location X ratio related to node dimensions</param>
        /// <param name="clickYRatio">Double Click location Y ratio related to node dimensions</param>
        /// <param name="clickZRatio">Double Click location Z ratio related to node dimensions</param>
        public void performNodeDoubleClick(string nodeID, double clickXRatio, double clickYRatio, double clickZRatio)
        {

            if (pcanvas == null) return;
            pcanvas.doADoubleClickonANode(nodeID, clickXRatio, clickYRatio, clickZRatio);

        }


        /// <summary>
        /// gets full node information in XML format
        /// </summary>
        /// <param name="nodeID">valid node ID</param>
        /// <returns>node data in XML format. values separated by _|_</returns>
        public string getNodeInformationXML(string nodeID)
        {
            if (pcanvas == null) return "nullcanvas";
            if (pcanvas.stacks.ContainsKey(nodeID) == false) return "notfound";
            try
            {
                PUPPICanvas.ModViz3DNode mb = pcanvas.stacks[nodeID];
                return mb.ninfoXML();
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// Saves a node renderer as an image to file
        /// </summary>
        /// <param name="sourceNodeID">valid node ID as string</param>
        /// <returns>image file path in temp folder</returns>
        public string exportNodeRendererImage(string nodeID)
        {
            if (pcanvas == null)
            {
                return "nullcanvas";
            }
            return (pcanvas.expNCR2File(nodeID));
        }

        /// <summary>
        /// Gets the path to an image file showing a view of the node renderer. When server is not running, requires first exportNodeRendererImage
        /// </summary>
        /// <param name="nodeID">valid node ID as string</param>
        /// <returns>file pah if found, various error messags if not</returns>
        public string getNodeRendererImageFileName(string nodeID)
        {

            if (pcanvas == null)
            {
                return "nullcanvas";
            }
            return (pcanvas.getNCRFile(nodeID));

        }


        /// <summary>
        /// Sets whether a node can intersect with other nodes. Solid nodes cannot intersect other nodes unless overriden by PUPPIGUISettings.
        /// </summary>
        /// <param name="nodeID">valid node ID (string)</param>
        /// <param name="isSolid">boolean</param>
        public void setNodeSolid(string nodeID, bool isSolid)
        {
            if (pcanvas == null)
            {
                return;
            }
            if (pcanvas.stacks.ContainsKey(nodeID) == false) return;

            pcanvas.stacks[nodeID].isSolid = isSolid;

        }

        /// <summary>
        /// Connects two nodes.
        /// </summary>
        /// <param name="sourceNodeID">ID of source node.</param>
        /// <param name="sourceNodeOutputIndex">Index of source output.</param>
        /// <param name="destinationNodeID">ID of destination node.</param>
        /// <param name="destinationInputIndex">Destination node input index</param>
        /// <returns>True if successful</returns>
        public bool connectNodesCommand(string sourceNodeID, int sourceNodeOutputIndex, string destinationNodeID, int destinationInputIndex)
        {
            if (pcanvas == null) return false;
            try
            {
                return pcanvas.connectNode2Node(Convert.ToInt16(sourceNodeID), sourceNodeOutputIndex, Convert.ToInt16(destinationNodeID), destinationInputIndex);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Disconnects a node's input
        /// </summary>
        /// <param name="destinationNodeID">ID of node</param>
        /// <param name="destinationInputIndex">Input index</param>
        /// <returns>if operation was successful</returns>
        public bool disconnectNodeInputCommand(string destinationNodeID, int destinationInputIndex)
        {
            if (pcanvas == null) return false;
            try
            {
                return pcanvas.disconnectANodesInput(Convert.ToInt16(destinationNodeID), destinationInputIndex);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Causes the canvas visual program to run
        /// </summary>
        public void runCanvasProgramCommand()
        {
            if (pcanvas == null) return;
            pcanvas.runPUPPIprogram();
        }
        internal static bool timerIsRunning = false;
        static bool isCurrentlyRunning = false;

        void executeTimerRun(Object state)
        {
            if (timerIsRunning == true)
            {
                try
                {
                    if (isCurrentlyRunning == false)
                    {
                        //already scheduled to be called
                        if (pcanvas.commandQueue.Contains("runcanvasprogramcommand"))
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
                            //wait to finish running queue
                            while (PUPPIModel.PUPPIModule.concurrentProcesses + PUPPIServer.PUPPICanvasTCPServer.NumberClientThreads + PUPPIServer.PUPPICanvasHTTPServer.NumberClientThreads > 0 || PUPPICanvas.mouseisdown || pcanvas.commandQueue.Count > 0)
                            {
                                Thread.Sleep(10);
                            }


                            if (pcanvas.somewhereProcessingStopped != null)
                            {
                                pcanvas.commandQueue.Add("runcanvasprogramcommand");
                                isCurrentlyRunning = false;
                                pcanvas.Dispatcher.BeginInvoke(new EventHandler(pcanvas.somewhereProcessingStopped), pcanvas, null);

                            }
                            else
                            {

                            }



                            isCurrentlyRunning = false;
                        }





                    }
                }
                catch
                {

                    timerIsRunning = false;
                    isCurrentlyRunning = false;
                }
            }

        }


        System.Threading.Timer t = null;


        /// <summary>
        /// Starts a timer that runs the canvas periodically
        /// </summary>
        /// <param name="interval">Interval in miliseconds</param>
        /// <returns>True if timers started, false if it is already running or errors encountered</returns>
        public bool startCanvasRunTimer(long interval)
        {
            if (pcanvas == null) return false;
            if (timerIsRunning) return false;
            timerIsRunning = true;
            t = new System.Threading.Timer(executeTimerRun, null, 0, interval);
            if (timerIsRunning == false)
            {
                t.Change(Timeout.Infinite, Timeout.Infinite);
            }


            return timerIsRunning;
        }
        /// <summary>
        /// Stops the canvas run timer
        /// </summary>
        /// <returns>True if timer stopped, false if timer was not running</returns>
        public bool stopCanvasRunTimer()
        {

            if (timerIsRunning == false) return false;
            timerIsRunning = false;
            if (t == null) return false;
            t.Change(Timeout.Infinite, Timeout.Infinite);
            return true;
        }

        /// <summary>
        /// Disconnects all nodes connected to a node's output
        /// </summary>
        /// <param name="sourceNodeID">ID of node</param>
        /// <param name="sourceOutputIndex">Output index</param>
        /// <returns>if operation was successful</returns>
        public bool disconnectNodeOutputCommand(string sourceNodeID, int sourceOutputIndex)
        {
            if (pcanvas == null) return false;
            return pcanvas.discoAllNodeOutputs(sourceNodeID, sourceOutputIndex);
        }
        /// <summary>
        /// Gets the output of a node (cloned object)
        /// </summary>
        /// <param name="nodeID">valid node ID</param>
        /// <param name="outputIndex">valid output index</param>
        /// <returns>Object</returns>
        public object getNodeOutputValue(string nodeID, int outputIndex)
        {
            if (pcanvas == null) return null;
            if (pcanvas.stacks.ContainsKey(nodeID) == false) return null;
            PUPPIModel.PUPPIModule pm = pcanvas.stacks[nodeID].logical_representation;
            if (pm == null) return null;
            if (outputIndex < 0 || outputIndex > pm.outputs.Count - 1) return null;
            return PUPPIModel.Utilities.CloneObject(pm.outputs[outputIndex]);
        }

        /// <summary>
        /// Gets a list of all the nodes on the canvas.
        /// </summary>
        /// <returns>A dictionary of string keys being the node IDs and string values being a description of each node's pertinent information.</returns>
        public Dictionary<string, string> getAllNodes()
        {
            if (pcanvas == null) return new Dictionary<string, string>();
            return pcanvas.getMyNodesOnCanvas();
        }
        /// <summary>
        /// Gets the PUPPIModule of a node on the canvas.
        /// </summary>
        /// <param name="nodeID">Valid node ID</param>
        /// <returns>PUPPIModule derived object or null</returns>
        public PUPPIModel.PUPPIModule getNodeModuleByID(string nodeID)
        {
            if (pcanvas == null) return null;
            if (pcanvas.stacks.ContainsKey(nodeID) == false) return null;
            return pcanvas.stacks[nodeID].logical_representation;
        }

        /// <summary>
        /// Sets the captions on nodes on canvas (unless defined in a custom node renderer) to cleaned up versions of the PUPPIModule name
        /// </summary>
        public void resetCanvasNodeCaptions()
        {
            if (pcanvas == null) return;
            pcanvas.resetAllCanvasNodeCaptions();
        }

        /// <summary>
        /// Sets a node caption programmatically.
        /// </summary>
        /// <param name="nodeID">Valid node ID</param>
        /// <param name="newCaption">New caption, can't be empty</param>
        /// <returns>If the operation was successful</returns>
        public bool changeNodeCaption(string nodeID, string newCaption)
        {
            if (pcanvas == null) return false;
            return pcanvas.setANodeCaptionByGUID(nodeID, newCaption);
        }

        /// <summary>
        /// Collapses the selected nodes on the PUPPI Canvas into a single container nodes.
        /// </summary>
        /// <param name="containerID">outputs the ID of the newly created container node, "-1" if it fails</param>
        /// <param name="newContainerCaption">Caption to be displayed onf new container node</param>
        /// <returns>If the operation was successful</returns>
        public bool collapseCanvasSelection(out string containerID, string newContainerCaption)
        {

            if (newContainerCaption == "") newContainerCaption = "Container";
            /*

            string msg = "";
            bool bial = PUPPIruntimesettings.processyesno;
            PUPPIruntimesettings.processyesno = false;
            int iuw = 0;
            bool b = mycanvas.collapseSelectionToContainerNode(out msg, out iuw);
            PUPPIruntimesettings.processyesno = bial;
            if (b)
            {
                mycanvas.runPUPPIprogram();
            }
            else
            {
                if (!msg.StartsWith("There is at least one output which connects"))
                {
                    MessageBox.Show("Failed to collapse nodes: " + msg);
                    mycanvas.undome();
                }
                else
                {
                    MessageBox.Show(msg);
                }
            }

            */
            containerID = "-1";
            if (pcanvas == null) return false;
            string msg = "";
            int i = -1;
            bool bial = PUPPIruntimesettings.processyesno;
            PUPPIruntimesettings.processyesno = false;
            int iuw = 0;
            bool pf = pcanvas.collapseSelectionToContainerNode(out msg, out i, newContainerCaption);
            PUPPIruntimesettings.processyesno = bial;
            if (pf)
            {
                pcanvas.runPUPPIprogram();
            }
            else
            {


                //pcanvas.undome();

            }
            containerID = i.ToString();
            return pf;

        }
        /// <summary>
        /// Selects the nodes on the canvas with supplied IDs.
        /// </summary>
        /// <param name="nodeIDs">List of IDs as strings</param>
        public void selectNodesByIDs(List<string> nodeIDs)
        {
            if (pcanvas == null) return;
            List<int> nGs = new List<int>();
            foreach (string s in nodeIDs)
            {
                try
                {
                    nGs.Add(Convert.ToInt16(s));
                }
                catch
                {

                }
            }
            pcanvas.selectCanvasNodesBySuppliedIDs(nGs);
        }

        /// <summary>
        /// Gets a description of the node.
        /// </summary>
        /// <param name="NodeID"> ID of node</param>
        /// <returns>Node description</returns>
        public string getNodeInformation(string NodeID)
        {
            if (pcanvas == null) return "null canvas";
            return pcanvas.getMyNodeOnCanvasDescription(NodeID);
        }

        /// <summary>
        /// Causes a node on canvas to process, updating outputs and nodes downstream
        /// </summary>
        /// <param name="NodeID">valid Node ID</param>
        public void forceNodeProcess(string NodeID)
        {
            if (pcanvas != null) pcanvas.tellMyNodeOnCanvasToProcess(NodeID);
        }
        ///// <summary>
        ///// Sets a node's output to specified value, and causes modules downstream of said output to process. Care must be used with this instruction as it can result in infinite loops.
        ///// </summary>
        ///// <param name="NodeID">valid Node ID</param>
        ///// <param name="outputIndex">valid module output index</param>
        ///// <param name="outputValue">generic form of the desired value</param>
        //public void setNodeOutput(string NodeID,int outputIndex, object outputValue)
        //{
        //    if (pcanvas != null) pcanvas.setMyNodeOnCanvasOutput(NodeID, outputIndex, outputValue);
        //}

        /// <summary>
        /// Gets the a node's bounding box coordinates
        /// </summary>
        /// <param name="NodeID">valid node ID </param>
        /// <param name="xStart">output parameter, start X</param>
        /// <param name="yStart">output parameter, start Y</param>
        /// <param name="zStart">output parameter, start Z</param>
        /// <param name="xSize">output parameter, size X</param>
        /// <param name="ySize">output parameter, size Y</param>
        /// <param name="zSize">output parameter, size Z</param>
        /// <returns></returns>
        public bool getNodeBoundingBox(string NodeID, out double xStart, out double yStart, out double zStart, out double xSize, out double ySize, out double zSize)
        {

            xStart = 0;
            yStart = 0;
            zStart = 0;

            xSize = 0;
            ySize = 0;
            zSize = 0;
            if (pcanvas == null) return false;
            return pcanvas.getANodeBoundingBoxByGUID(NodeID, out xStart, out yStart, out zStart, out xSize, out ySize, out zSize);
        }

        /// <summary>
        /// Pops up the map view in the PUPPI canvas. Used for scripting or user defined menus;
        /// </summary>
        public void mapCanvasCommand()
        {
            if (pcanvas == null) return;
            PUPPIMapViewForm pmv = new PUPPIMapViewForm();
            pmv.pc = pcanvas;
            pmv.ShowDialog();
        }


        /// <summary>
        /// Resets the camera to default position
        /// </summary>
        public void resetCanvasCameraCommand()
        {
            if (pcanvas == null) return;
            pcanvas.camera_reset();
        }
        /// <summary>
        /// Pops up the tree view in the PUPPI canvas. Used for scripting or user defined menus;
        /// </summary>
        public void treeCanvasCommand()
        {
            if (pcanvas == null) return;
            PUPPITreeViewForm ptv = new PUPPITreeViewForm();
            ptv.mycanvas = pcanvas;
            ptv.ShowDialog();
        }
        /// <summary>
        /// Refreshes the connections between nodes. Used for scripting or user defined menus;
        /// </summary>
        public void refreshConnections()
        {
            if (pcanvas == null) return;
            pcanvas.regenerate_connections(true);
        }

        /// <summary>
        /// Undo in the PUPPI canvas. Used for scripting or user defined menus;
        /// </summary>
        public void undoCanvasCommand()
        {
            if (pcanvas == null) return;
            pcanvas.undome();
        }
        /// <summary>
        /// Exports an image or 3D model representation of the canvas (file type determined by fileName extension).
        /// </summary>
        /// <param name="fileName">File to export to. Leave empty (default) for interactive mode</param>
        public void canvasSnapshotCommand(string fileName = "")
        {
            if (pcanvas == null) return;
            pcanvas.exportcanvassnapshot(fileName);
        }

        /// <summary>
        /// Opens import file dialog. Used for scripting or user defined menus;
        /// </summary>
        /// <param name="importFile">File name + path to import. Leave empty (default) for interactive mode.</param>
        public void importCanvasCommand(string importFile = "")
        {
            if (pcanvas == null) return;
            pcanvas.importfile(importFile);
        }

        /// <summary>
        /// Opens import file as container dialog. Used for scripting or user defined menus;
        /// </summary>
        /// <param name="importFile">File name + path to import. Leave empty (default) for interactive mode.</param>
        public void importCanvasASContainerCommand(string importFile = "")
        {
            if (pcanvas == null) return;
            pcanvas.importfileAC(importFile);
        }

        /// <summary>
        /// Rearranges the nodes and stacks on canvas based on connections.
        /// </summary>
        /// <param name="columnCount">Number of columns to place nodes on, has to be greater than 0 </param>
        public void rearrangeCanvasNodesCommand(int columnCount)
        {
            if (pcanvas == null) return;
            pcanvas.rearrangenice(columnCount);
        }

        /// <summary>
        /// Explodes container node with specified ID.
        /// </summary>
        /// <param name="NodeID">Valid ID string</param>
        /// <returns>A list of node IDs, if unsuccessful, an empty list</returns>
        public List<string> explodeContainerNode(string NodeID)
        {
            List<string> l = new List<string>();
            if (pcanvas == null) return l;
            try
            {
                bool bab = pcanvas.explodeContainerGetGUIDs(Convert.ToInt16(NodeID), out l);
            }
            catch
            {

            }
            return l;
        }

        /// <summary>
        /// Undo in the PUPPI canvas. Used for scripting or user defined menus;
        /// </summary>
        public void redoCanvasCommand()
        {
            if (pcanvas == null) return;
            pcanvas.redome();
        }
        /// <summary>
        /// New PUPPI canvas. Used for scripting or user defined menus;
        /// </summary>
        /// <param name="noPrompt">If set to true, doesn't prompt the user if canvas is not saved.Default is false.</param>
        public void newCanvasCommand(bool noPrompt = false)
        {
            if (pcanvas == null) return;
            pcanvas.newcanvas(noPrompt);
        }
        /// <summary>
        /// Open file dialog. Used for scripting or user defined menus;
        /// </summary>
        /// <param name="fileToOpen">If anything is put in here, it will open the path given without prompts.Default is empty string.</param>
        public void openCanvasCommand(string fileToOpen = "")
        {
            if (pcanvas == null) return;
            pcanvas.openfile(fileToOpen);
        }
        /// <summary>
        /// Save-as dialog. Used for scripting or user defined menus;
        /// </summary>
        /// <param name="saveToFile">File to save to. Leave emtpy (default) for interactive mode</param>
        public void saveCanvasCommand(string saveToFile = "")
        {
            if (pcanvas == null) return;


            pcanvas.saveas(pcanvas.stacks.Values.ToList<PUPPICanvas.ModViz3DNode>(), true, saveToFile);
        }

        /// <summary>
        /// Returns a string containing the saved canvas
        /// </summary>
        /// <returns>string containing XML</returns>
        public string saveCanvasToString()
        {
            if (pcanvas == null) return "no canvas";

            string nott = "";
            pcanvas.savexml("", pcanvas.stacks.Values.ToList<PUPPICanvas.ModViz3DNode>(), false, out nott);
            return nott;

        }

      

        /// <summary>
        /// Exit application. Used for scripting or user defined menus;
        /// </summary>
        public void exitCanvasCommand()
        {
            if (pcanvas == null) return;
            pcanvas.exitapplication();
        }
        /// <summary>
        /// Pops up the About PUPPI text. Used for scripting or user defined menus;
        /// </summary>
        void aboutPUPPICanvasCommand()
        {
            if (pcanvas == null) return;
            PUPPI.aboutlicense abb = new PUPPI.aboutlicense();
            abb.Show();
        }
        /// <summary>
        /// Opens specified help .chm file. Used for scripting or user defined menus;
        /// </summary>
        void hlpshowCanvasCommand(string helpFilePath)
        {
            if (pcanvas == null) return;
            System.Windows.Forms.Help.ShowHelp(null, helpFilePath);


        }
        /// <summary>
        /// Performs a zoom-extents operation on canvas.
        /// </summary>
        /// <param name="milliSeconds">Animation duration in milliSeconds</param>
        public void zoomExtentsCanvasCommand(int milliSeconds)
        {
            if (pcanvas == null) return;
            pcanvas.myCanvasPleaseZoomExtents(milliSeconds);
        }
        /// <summary>
        /// Selects all the nodes on the canvas. Used for scripting or user defined menus;
        /// </summary>
        public void selectAllNodesCanvasCommand()
        {
            if (pcanvas == null) return;
            pcanvas.selectallnodes();
        }
        /// <summary>
        /// Unselects all nodes on canvas. Used for scripting or user defined menus;
        /// </summary>
        public void unSelectAllNodesCanvasCommand()
        {
            if (pcanvas == null) return;
            pcanvas.deselectallnodes();
        }
        /// <summary>
        /// Copies selected nodes to clipboard.Used for scripting or user defined menus;
        /// </summary>
        public void copySelectedNodesCanvasCommand()
        {
            if (pcanvas == null) return;
            pcanvas.copyselectiontoclipboard();
        }
        /// <summary>
        /// Opens up dialong to export selected nodes to a PUPPI program file.Used for scripting or user defined menus;
        /// </summary>
        /// <param name="exportFilePath">A file to export to. Leave empty (default) for interfactive mode.</param>
        public void exportSelectedNodesCanvasCommand(string exportFilePath = "")
        {
            if (pcanvas == null) return;
            pcanvas.export_selected_nodes(exportFilePath);
        }
        /// <summary>
        /// Deletes selected nodes.Used for scripting or user defined menus;
        /// </summary>
        public void deleteSelectedNodesCanvasCommand()
        {
            if (pcanvas == null) return;
            pcanvas.delete_selected_nodes();
        }
        /// <summary>
        /// Sets selected nodes to be either hidden or shown on a locked canvas. If this is done while canvas is locked, the canvas needs to be saved and reloaded.
        /// </summary>
        /// <param name="hideStatus">if true, nodes are set to hidden, if false, hidden flag set to false</param>
        public void hideSelectedNodesOnLockedCanvasCommand(bool hideStatus)
        {
            if (pcanvas == null) return;
            pcanvas.set_hidden_selected_nodes_on_locked_canvas(hideStatus);
        }

        /// <summary>
        /// Deletes a node on Canvas by supplied ID
        /// </summary>
        /// <param name="nodeID">valid ID</param>
        public void deleteCanvasNodeByID(string nodeID)
        {
            if (pcanvas == null) return;
            try
            {
                int dint = Convert.ToInt16(nodeID);
                pcanvas.deleteMyNodeOnCanvasbyID(dint);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Sets whether a node is hidden on locked canvas or not. If running this command on a locked canvas, the canvas needs to be reloaded.
        /// </summary>
        /// <param name="nodeID">valid node ID</param>
        /// <param name="isHidden">true to hide node on locked canvas, false to show</param>
        public void setHiddenCanvasNodeByID(string nodeID, bool isHidden)
        {
            if (pcanvas == null) return;
            try
            {
                int dint = Convert.ToInt16(nodeID);
                pcanvas.setHiddenMyNodeOnCanvasbyID(dint, isHidden);
            }
            catch
            {

            }
        }


        /// <summary>
        /// Moves a node. On a 3D Canvas, entire stack gets moved.
        /// </summary>
        /// <param name="nodeID">valid ID</param>
        /// <param name="xOffset">x displacement</param>
        /// <param name="yOffset">y displacement</param>
        public void moveNodeByID(string nodeID, double xOffset, double yOffset)
        {
            if (pcanvas == null) return;
            pcanvas.forceNodeMoveUpdate(nodeID, xOffset, yOffset);
        }

        /// <summary>
        /// Gets the IDs of selected nodes.
        /// </summary>
        /// <returns>List of string IDS</returns>
        public List<string> getSelectedNodesIDs()
        {
            List<string> sg = new List<string>();
            if (pcanvas == null) return sg;
            sg = pcanvas.getNodesThatAreSelectedOnCanvas();
            return sg;
        }

        /// <summary>
        /// Paste at specified position.Used for scripting or user defined menus;
        /// </summary>
        public void pastedSelectedNodesCanvasCommand(double x, double y)
        {
            if (pcanvas == null) return;
            pcanvas.pastefromclip(new System.Windows.Media.Media3D.Point3D(x, y, 0));
        }
        /// <summary>
        /// Sets the camera position .Used for scripting or user defined menus;
        /// </summary>
        public void setCanvasCameraPosition(double x, double y, double z)
        {
            if (pcanvas == null) return;
            pcanvas.setcameraposition(new System.Windows.Media.Media3D.Point3D(x, y, z));
        }
        /// <summary>
        /// Selects node in region.Used for scripting or user defined menus;
        /// </summary>
        public void selectNodesInRegionCanvasCommand(double x_start, double y_start, double x_end, double y_end)
        {
            if (pcanvas == null) return;
            System.Windows.Media.Media3D.Rect3D r = new System.Windows.Media.Media3D.Rect3D(Math.Min(x_start, x_end), Math.Min(y_start, y_end), 0, Math.Abs(x_start - x_end), Math.Abs(y_start - y_end), PUPPIGUISettings.textRaise);

            pcanvas.select_canvas_nodes_by_region(r);
        }


    }
    /// <summary>
    /// Contains methods for adding PUPPI GUI Controls directly to a Windows Form.
    /// </summary>

    public static class FormTools
    {
        /// <summary>
        /// Adds an already initialized PUPPI Program Canvas Control to a Windows Form 
        /// </summary>
        /// <param name="PUPPIprogcanvas">A PUPPIProgramCanvass instance</param>
        /// <param name="mywindowsform">The form where this is added (usually thype "this")</param>
        /// <param name="leftpos">Distance to left edge of form</param>
        /// <param name="toppos">Distance to top edge of form</param>
        public static void AddPUPPIProgramCanvastoForm(PUPPIProgramCanvas PUPPIprogcanvas, Form mywindowsform, int leftpos = 0, int toppos = 0)
        {
            //if (FormIsInVS)
            //{
            //    System.Windows.Forms.MessageBox.Show("Invalid License");     

            //}
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            if (PUPPIprogcanvas != null && mywindowsform != null)
            {

                PUPPIprogcanvas.myFormControl = new ElementHost();
                PUPPIprogcanvas.myFormControl.Left = leftpos;
                PUPPIprogcanvas.myFormControl.Top = toppos;
                PUPPIprogcanvas.myFormControl.Child = PUPPIprogcanvas.myuserWPFControl;
                PUPPIprogcanvas.myFormControl.Height = Convert.ToInt16(PUPPIprogcanvas.pcanvas.thei);
                PUPPIprogcanvas.myFormControl.Width = Convert.ToInt16(PUPPIprogcanvas.pcanvas.twid);

                mywindowsform.Controls.Add(PUPPIprogcanvas.myFormControl);
                //if (PUPPIprogcanvas.IsControlDesignerHosted(PUPPIprogcanvas.myFormControl))
                //{
                //    System.Windows.Forms.MessageBox.Show("Invalid License");
                //}
            }
            else
            {
                throw new ArgumentException("One or more invalid arguments in AddPUPPIProgramCanvastoForm call");
            }
        }

        /// <summary>
        /// Adds an already initialized PUPPI Menu Box Control to a Windows Form 
        /// </summary>
        /// <param name="mymenubox">A PUPPIModuleKeepMenu instance</param>
        /// <param name="mywindowsform">The form where this is added (usually thype "this")</param>
        /// <param name="leftpos">Distance to left edge of form</param>
        /// <param name="toppos">Distance to top edge of for</param>
        public static void AddPUPPIModuleKeepMenutoForm(PUPPIModuleKeepMenu mymenubox, Form mywindowsform, int leftpos = 0, int toppos = 0)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            if (mymenubox != null && mywindowsform != null)
            {

                mymenubox.myFormControl = new ElementHost();
                mymenubox.myFormControl.AutoSize = false;

                mymenubox.myFormControl.Left = leftpos;
                mymenubox.myFormControl.Top = toppos;
                mymenubox.myFormControl.Child = mymenubox.myuserWPFControl;
                mymenubox.myFormControl.Height = Convert.ToInt16(mymenubox.my_my_menu.winheight);
                mymenubox.myFormControl.Width = Convert.ToInt16(mymenubox.my_my_menu.winwidth);
                mywindowsform.Controls.Add(mymenubox.myFormControl);
            }
            else
            {
                throw new ArgumentException("One or more invalid arguments in AddPUPPIModuleKeepMenutoForm call");
            }
        }
        /// <summary>
        ///  Adds an already initialized PUPPI Drop Down Menu to a Windows Form 
        /// </summary>
        /// <param name="mydropdownmenu">A mydropdownmenu instance</param>
        /// <param name="mywindowsform">The form where this is added (usually thype "this")</param>
        /// <param name="leftpos">Distance to left edge of form</param>
        /// <param name="toppos">Distance to top edge of for</param>
        /// <param name="menuwidth">Width reserved for menu title</param>
        /// <param name="menuheight">Height reserved for menu title</param>
        public static void AddPUPPIDropDownMenutoForm(PUPPIDropDownMenu mydropdownmenu, Form mywindowsform, int leftpos = 0, int toppos = 0, int menuwidth = 40, int menuheight = 20)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            if (mydropdownmenu != null && mywindowsform != null)
            {

                mydropdownmenu.myFormControl = new ElementHost();
                mydropdownmenu.myFormControl.Left = leftpos;
                mydropdownmenu.myFormControl.Top = toppos;
                mydropdownmenu.myFormControl.Width = menuwidth;
                mydropdownmenu.myFormControl.Height = menuheight;
                mydropdownmenu.myFormControl.Child = mydropdownmenu.pgddm;
                mywindowsform.Controls.Add(mydropdownmenu.myFormControl);
            }
            else
            {
                throw new ArgumentException("One or more invalid arguments in AddPUPPIDropDownMenutoForm call");
            }
        }
        /// <summary>
        /// Adds an already initialized PUPPI CAD View to a Windows Form 
        /// </summary>
        /// <param name="pcadview">PUPPICADView object, already instantiated.</param>
        /// <param name="mywindowsform">A Windows form</param>
        /// <param name="leftpos">Top left corner horizontal position in px.Default is 0.</param>
        /// <param name="toppos">Top left corner vertical down position in px.Default is 0.</param>
        public static void AddPUPPICADViewToForm(PUPPICADView pcadview, Form mywindowsform, int leftpos = 0, int toppos = 0)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            if (pcadview != null && mywindowsform != null)
            {

                pcadview.myFormControl = new ElementHost();
                pcadview.myFormControl.Left = leftpos;
                pcadview.myFormControl.Top = toppos;
                pcadview.myFormControl.Child = pcadview.myuserWPFControl;
                pcadview.myFormControl.Height = pcadview.p3dview.th;
                pcadview.myFormControl.Width = pcadview.p3dview.tw;
                mywindowsform.Controls.Add(pcadview.myFormControl);
            }
            else
            {
                throw new ArgumentException("One or more invalid arguments in AddPUPPICADViewToForm call");
            }
        }
        //checks design mode
        internal static bool FormIsInVS
        {
            get
            {
                Process p = Process.GetCurrentProcess();
                bool result = false;

                if (p.ProcessName.ToLower().Trim().IndexOf("vshost") != -1)
                    result = true;
                p.Dispose();

                return result;
            }
        }
        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

            dllName = dllName.Replace(".", "_");

            if (dllName.EndsWith("_resources")) return null;
            string an = Assembly.GetExecutingAssembly().GetName().Name;
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(an + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

            byte[] bytes = (byte[])rm.GetObject(dllName);

            if (bytes != null && bytes.Length > 0) return System.Reflection.Assembly.Load(bytes); else return null;
        }
    }
    ///<summary>
    ///Instance of a PUPPI Module Keep Menu, where PUPPI Modules can be placed. The PUPPI Modules are dragged from here onto the Program Canvas
    ///where they become visual programming nodes.
    ///</summary>
    public class PUPPIModuleKeepMenu
    {
        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

            dllName = dllName.Replace(".", "_");

            if (dllName.EndsWith("_resources")) return null;
            string an = Assembly.GetExecutingAssembly().GetName().Name;
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(an + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

            byte[] bytes = (byte[])rm.GetObject(dllName);

            if (bytes != null && bytes.Length > 0) return System.Reflection.Assembly.Load(bytes); else return null;
        }
        internal PUPPIMenu my_my_menu;
        ///<summary>
        /// Access to the WPF User Control containing this PUPPI Menu. Note that the user control can only be initialized through the PUPPIModuleKeepMenu class.
        ///</summary>
        public PUPPIMenuControl myuserWPFControl { get; internal set; }
        public PUPPIGUIController.PUPPIProgramCanvas myPUPPICanvas = null;
      

        ///<summary>
        /// Access to the Form Control containing the PUPPI Menu Box, added directly to a Windows Form. 
        /// This is initialized when the static method AddPUPPIModuleKeepMenutoForm  is used.
        ///</summary>
        public ElementHost myFormControl { get; internal set; }
        /// <summary>
        /// Initializes an empty Module Keep.
        ///To add this to a regular Windows Form, you need to use the static method AddPUPPIModuleKeepMenutoForm 
        /// </summary>
        /// <param name="menutitle">The tittle of the menu, displayed at the top. Default is "Menu"</param>
        /// <param name="menuboxwidth">The width of the menu box area. Default is 100. This dimension applies to form element. WPF element stretches.</param>
        /// <param name="menuboxheight">The height of the menu box area. Default is 100.This dimension applies to form element. WPF element stretches.</param>
        /// <param name="menuitemwidth">The width of a draggable menu item. Default is 80.</param>
        /// <param name="menuitemheight">The height of a draggable menu item. Default is 20.</param>
        /// <param name="max_menu_items_per_row">Maximum number of menu items per row in box. Default is 1</param>
        /// <param name="aPUPPICanvas">This optional parameter is used to reference a. PUPPICanvas, for adding the modules to the searchable list of modules in the canvas.Default is null.</param>
        public PUPPIModuleKeepMenu(string menutitle = "Menu", int menuboxwidth = 100, int menuboxheight = 100, int menuitemwidth = 80, int menuitemheight = 20, int max_menu_items_per_row = 1, PUPPIGUIController.PUPPIProgramCanvas aPUPPICanvas = null, string menuToolTip = "")
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            PUPPIGUI.PUPPIruntimesettings.mycallerassembly = Assembly.GetCallingAssembly();
            PUPPIModuleKeepMenu1(menutitle, menuboxwidth, menuboxheight, menuitemwidth, menuitemheight, max_menu_items_per_row, aPUPPICanvas, menuToolTip);

        }
        //plist.additem(new PUPPIModel.PUPPIPremadeModules.ListModules.PUPLISTGET());
        private void PUPPIModuleKeepMenu1(string menutitle = "Menu", int menuboxwidth = 100, int menuboxheight = 100, int menuitemwidth = 80, int menuitemheight = 20, int max_menu_items_per_row = 1, PUPPIGUIController.PUPPIProgramCanvas pg = null, string menuToolTip = "")
        {
            //  PUPPIMenu hobjects = new PUPPIMenu(990, 20, "Helper Objects", 120, 20, 140, 80, 1);
            my_my_menu = new PUPPIMenu(0, 0, menutitle, menuitemwidth, menuitemheight, menuboxwidth, menuboxheight, max_menu_items_per_row, menuToolTip);
            myuserWPFControl = new PUPPIMenuControl();
            //myuserWPFControl.Width = menuboxwidth;
            //myuserWPFControl.Height = menuboxheight;
            //myuserWPFControl.UserControlCanvas.Width = menuboxwidth;
            //myuserWPFControl.UserControlCanvas.Height = menuboxheight;
            myuserWPFControl.UserControlCanvas.Children.Add(my_my_menu.maindisplay);

 
            myPUPPICanvas = pg;
        }


        /// <summary>
        ///Adds a new button to the menu, color can be customized. The button is based on a PUPPI Module.
        ///Colors and alpha are given in fractions (0-1)
        /// </summary>
        /// <param name="mymodule">Existing instantiated PUPPIModule</param>
        /// <param name="redcolor">>Button color fraction of red (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="greencolor">Button color fraction of green (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="bluecolor">Button color fraction of blue (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="alpha">Button fraction of alpha (opacity) (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <returns>True if successful, false in case of an error.</returns>
        public bool AddMenuButton(PUPPIModel.PUPPIModule mymodule, double redcolor = -1, double greencolor = -1, double bluecolor = -1, double alpha = -1)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
           
            try
            {
                my_my_menu.additem(mymodule, true, redcolor, greencolor, bluecolor, alpha);
                if (myPUPPICanvas != null)
                {
                    if (myPUPPICanvas.pcanvas != null)
                    {
                        if (myPUPPICanvas.pcanvas.cvavailablePUPPIModules.ContainsKey(mymodule.name) == false)
                        {
                            myPUPPICanvas.pcanvas.cvavailablePUPPIModules.Add(mymodule.name, mymodule);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Updates the menu button tooltip.
        /// </summary>
        /// <param name="mymodule">PUPPIModule matching the one used by the button.</param>
        /// <param name="newToolTip">New toolm tip</param>
        /// <returns>If the tooltip was changed.</returns>
        public bool ChangeMenuButtonTooltip(PUPPIModel.PUPPIModule mymodule, string newToolTip)
        {
            try
            {
                return my_my_menu.changeitemtt(mymodule, newToolTip);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///Adds a list of modules to the menu as buttons, all same color.
        /// </summary>
        /// <param name="modulist">ArrayList of types derived from base class PUPPIModule</param>
        /// <param name="redcolor">Button color fraction of red (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="greencolor">Button color fraction of green (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="bluecolor">Button color fraction of blue (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="alpha">Button fraction of alpha (opacity) (0-1). By default gets set to default values in PUPPIGUISettings</param>
        public void AddMenuButtonList(ArrayList modulist, double redcolor = -1, double greencolor = -1, double bluecolor = -1, double alpha = -1)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            for (int i = 0; i < modulist.Count; i++)
            {
                try
                {
                    PUPPIModel.PUPPIModule mymodule = System.Activator.CreateInstance(modulist[i] as Type) as PUPPIModel.PUPPIModule;
                    if (mymodule != null)
                    {
                        //    pm.additem(tt);
                        bool added = AddMenuButton(mymodule, redcolor, greencolor, bluecolor, alpha);
                    }
                }
                catch (Exception exy)
                {
                    int ii = 0;
                }
            }
        }

        //public bool addButtonIcon(PUPPIModel.PUPPIModule mymodule)
        //{
        //    if (baloob == false) System.Windows.Application.Current.Shutdown();
        //    try
        //    {
        //        my_my_menu.addMenuButtonIconImage(mymodule); 
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}


        internal void AddPUPPIPremadeModules(string assemblyname, double redcolor = -1, double greencolor = -1, double bluecolor = -1, double alpha = -1)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            Type[] typelist = PUPPIModel.ModuleHelperFunctions.GetTypesInNamespace(Assembly.GetExecutingAssembly(), assemblyname);
            ArrayList buttonmenus = new ArrayList();
            for (int i = 0; i < typelist.Length; i++)
            {
                buttonmenus.Add(typelist[i]);
            }
            AddMenuButtonList(buttonmenus, redcolor, greencolor, bluecolor, alpha);

        }
        /// <summary>
        /// Adds the premade PUPPI Data Input modules to the Menu
        /// </summary>
        /// <param name="redcolor">Button color fraction of red (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="greencolor">Button color fraction of green (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="bluecolor">Button color fraction of blue (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="alpha">Button color fraction of alpha(opacity) (0-1). By default gets set to default values in PUPPIGUISettings</param>
        public void AddPUPPIPremadeDataInputModules(double redcolor = -1, double greencolor = -1, double bluecolor = -1, double alpha = -1)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            AddPUPPIPremadeModules("PUPPIModel.PUPPIPremadeModules.DataInputModules", redcolor, greencolor, bluecolor, alpha);
        }


        /// <summary>
        /// Adds the premade PUPPI Data Exchange modules to the Menu
        /// </summary>
        /// <param name="redcolor">Button color fraction of red (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="greencolor">Button color fraction of green (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="bluecolor">Button color fraction of blue (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="alpha">Button color fraction of alpha(opacity) (0-1). By default gets set to default values in PUPPIGUISettings</param>
        public void AddPUPPIPremadeDataExchangeModules(double redcolor = -1, double greencolor = -1, double bluecolor = -1, double alpha = -1)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            AddPUPPIPremadeModules("PUPPIModel.PUPPIPremadeModules.DataExchangeModules", redcolor, greencolor, bluecolor, alpha);
        }



        /// <summary>
        /// Adds the modules necessary for generating 3D models.
        /// </summary>
        /// <param name="myCADview">Instance of PUPPICADView, needed for generation of 3D models</param>
        /// <param name="redcolor">Button color fraction of red (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="greencolor">Button color fraction of green (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="bluecolor">Button color fraction of blue (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="alpha">Button color fraction of alpha(opacity) (0-1). By default gets set to default values in PUPPIGUISettings</param>
        public void AddPUPPICADModules(PUPPICADView myCADview, double redcolor = -1, double greencolor = -1, double bluecolor = -1, double alpha = -1)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            PUPPICAD.PUPPICADModel ppc = new PUPPICAD.PUPPICADModel(myCADview);
            PUPPICAD.PUPPIGetCADViewContents pgc = new PUPPICAD.PUPPIGetCADViewContents(myCADview);
            AddMenuButton(ppc as PUPPIModel.PUPPIModule, redcolor, greencolor, bluecolor, alpha);
            AddMenuButton(pgc as PUPPIModel.PUPPIModule, redcolor, greencolor, bluecolor, alpha);
            AddMenuButton(new PUPPICAD.PUPPIPoint3DObject(), redcolor, greencolor, bluecolor, alpha);
            AddMenuButton(new PUPPICAD.PUPPICAD_File_Load(), redcolor, greencolor, bluecolor, alpha);
            AddMenuButton(new PUPPICAD.PUPPIBox_Point_VectorDiag(), redcolor, greencolor, bluecolor, alpha);

            AddMenuButton(new PUPPICAD.PUPPISphere_Point_Rad(), redcolor, greencolor, bluecolor, alpha);
            AddMenuButton(new PUPPICAD.PUPPICyl_Point_VectorAxis_Rad(), redcolor, greencolor, bluecolor, alpha);

            AddMenuButton(new PUPPICAD.PUPPITruncone_Point_VectorAxis_Rad(), redcolor, greencolor, bluecolor, alpha);
            AddMenuButton(new PUPPICAD.PUPPIFillPolyline(), redcolor, greencolor, bluecolor, alpha);
            AddMenuButton(new PUPPICAD.PUPPISurfaceCap(), redcolor, greencolor, bluecolor, alpha);
            AddMenuButton(new PUPPICAD.PUPPIExtrude_Point_VectorAxis_Shape(), redcolor, greencolor, bluecolor, alpha);
            AddMenuButton(new PUPPICAD.PUPPIOpenExtrude_Point_VectorAxis_Shape(), redcolor, greencolor, bluecolor, alpha);

            AddMenuButton(new PUPPICAD.PUPPILoft(), redcolor, greencolor, bluecolor, alpha);
            AddMenuButton(new PUPPICAD.PUPPISurfaceLoft(), redcolor, greencolor, bluecolor, alpha);
            AddMenuButton(new PUPPICAD.PUPPIPlineObject(), redcolor, greencolor, bluecolor, alpha);
            AddMenuButton(new PUPPICAD.PUPPISketchObject(), redcolor, greencolor, bluecolor, alpha);


            AddMenuButton(new PUPPICAD.PUPPIPlaneObject(), redcolor, greencolor, bluecolor, alpha);
            AddMenuButton(new PUPPICAD.PUPPIPlanarMesh(), redcolor, greencolor, bluecolor, alpha);

            AddMenuButton(new PUPPICAD.PUPPI3DModel2File(), redcolor, greencolor, bluecolor, alpha);
            AddMenuButton(new PUPPICAD.PUPPICombine3DModels(), redcolor, greencolor, bluecolor, alpha);

            AddMenuButton(new PUPPICAD.PUPPIWireframeRepresentation(), redcolor, greencolor, bluecolor, alpha);

            AddMenuButton(new PUPPICAD.PUPPICloneAndPlace(), redcolor, greencolor, bluecolor, alpha);

        }


        /// <summary>
        /// Adds the premade PUPPI Math Modules to the Menu
        /// </summary>
        /// <param name="redcolor">Button color fraction of red (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="greencolor">Button color fraction of green (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="bluecolor">Button color fraction of blue (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="alpha">Button color fraction of alpha(opacity) (0-1). By default gets set to default values in PUPPIGUISettings</param>
        public void AddPUPPIPremadeMathModules(double redcolor = -1, double greencolor = -1, double bluecolor = -1, double alpha = -1)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            AddPUPPIPremadeModules("PUPPIModel.PUPPIPremadeModules.MathModules", redcolor, greencolor, bluecolor, alpha);
        }

        /// <summary>
        /// Adds the premade PUPPI List Modules to the Menu
        /// </summary>
        /// <param name="redcolor">Button color fraction of red (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="greencolor">Button color fraction of green (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="bluecolor">Button color fraction of blue (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="alpha">Button color fraction of alpha(opacity) (0-1). By default gets set to default values in PUPPIGUISettings</param>
        public void AddPUPPIPremadeListModules(double redcolor = -1, double greencolor = -1, double bluecolor = -1, double alpha = -1)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            AddPUPPIPremadeModules("PUPPIModel.PUPPIPremadeModules.ListModules", redcolor, greencolor, bluecolor, alpha);
        }

        /// Adds the premade PUPPI Logic Modules to the Menu
        /// </summary>
        /// <param name="redcolor">Button color fraction of red (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="greencolor">Button color fraction of green (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="bluecolor">Button color fraction of blue (0-1). By default gets set to default values in PUPPIGUISettings</param>
        /// <param name="alpha">Button color fraction of alpha(opacity) (0-1). By default gets set to default values in PUPPIGUISettings</param>
        public void AddPUPPIPremadeLogicModules(double redcolor = -1, double greencolor = -1, double bluecolor = -1, double alpha = -1)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            AddPUPPIPremadeModules("PUPPIModel.PUPPIPremadeModules.LogicModules", redcolor, greencolor, bluecolor, alpha);
        }

    }

    ///<summary>
    ///Class for managing dropdown menus, for commands already defined in PUPPI, and also for new commands.
    ///</summary>
    public class PUPPIDropDownMenu
    {
        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

            dllName = dllName.Replace(".", "_");

            if (dllName.EndsWith("_resources")) return null;
            string an = Assembly.GetExecutingAssembly().GetName().Name;
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(an + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

            byte[] bytes = (byte[])rm.GetObject(dllName);

            if (bytes != null && bytes.Length > 0) return System.Reflection.Assembly.Load(bytes); else return null;
        }
        internal PUPPICanvas pcanvas;
        internal PUPPIGeneralDropDownMenu pgddm;
        /// <summary>
        ///Direct access to the top menu item for use with WPF controls.
        /// </summary>
        public System.Windows.Controls.MenuItem mymenuWPFControlMenuItem { get; internal set; }
        public ElementHost myFormControl { get; internal set; }
        /// <summary>
        /// Instantiates a new drop down menu, where different commands to control PUPPI can be added.
        ///To add this to a regular Windows Form, you need to use the static method AddPUPPIGeneralDropDownMenutoForm
        ///Needs a reference to an existing PUPPIProgramCanvas
        /// </summary>
        /// <param name="pcm">An existing PUPPIProgramCanvas object.</param>
        /// <param name="name">Name of the menu. Defualts to "Drop Down Menu"</param>

        public PUPPIDropDownMenu(PUPPIProgramCanvas pcm, string name = "Drop Down Menu")
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            PUPPIDropDownMenu1(pcm, name);

        }

        private void PUPPIDropDownMenu1(PUPPIProgramCanvas pcm, string name = "Drop Down Menu")
        {
           
            pcanvas = pcm.pcanvas;
            pgddm = new PUPPIGeneralDropDownMenu(pcanvas, name);
            mymenuWPFControlMenuItem = pgddm.dropTopMenuItem;
        }


        /// <summary>
        ///Adds the standard options for a file menu (open, save etc)
        /// </summary>
        public void addStandardFileMenuOptions()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addNewCanvasMenuOption();
            pgddm.addOpenCanvasMenuOption();
            pgddm.addImportCanvasMenuOption();
            pgddm.addImportCanvasACMenuOption();
            pgddm.addSaveCanvasMenuOption();
            pgddm.addExportSnapshotMenuOption();
            pgddm.addExitMenuOption();

        }
        /// <summary>
        ///Adds the standard options for an edit menu (undo and redo)
        /// </summary>
        public void addStandardEditMenuOptions()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addUndoMenuOption();
            pgddm.addRedoMenuOption();
            pgddm.addSelectAllMenuOption();
            pgddm.addSelectNoneMenuOption();
            pgddm.addCopySelectionMenuOption();
            pgddm.addPaste00MenuOption();
            pgddm.addExportSelectionMenuOption();
            pgddm.adddeleteSelectionMenuOption();
            pgddm.addCollapseSelectionMenuOption();
            pgddm.addResetAllNodeCaptionsOption();
            pgddm.addNbNMenuOption();
            pgddm.addHideSelectedMenuOption();
            pgddm.addShowSelectedMenuOption();


        }
        /// <summary>
        ///Adds the standard options for a view menu (map view and tree view pop up forms)
        /// </summary>
        public void addStandardViewMenuOptions()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addMapViewMenuOption();
            pgddm.addTreeViewMenuOption();
            pgddm.addSetCameraPositionMenuOption();
            pgddm.addRARnodesMenuOption();
            pgddm.addRefrConnMenuOption();
            pgddm.addLockCanvasMenuOption();
            pgddm.addunLockCanvasMenuOption();
        }
        /// <summary>
        ///Adds the standard options for a help menu (a help file and an about page with licenses)
        /// </summary>
        /// <param name="helpfilepath">The path to the help file should be calculated on the fly:
        ///example:
        ///Uri.UnescapeDataString(new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)) .AbsolutePath)+@"/PUPPI-user-help.chm</param>
        public void addStandardHelpMenuOptions(string helpfilepath, bool addDefaultShortcut = true)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addHelpMenuOption(helpfilepath);
            pgddm.addAboutPUPPIMenuOption();

        }

        /// <summary>
        ///Adds the new canvas option to the menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addNewCanvasOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addNewCanvasMenuOption();

        }

        /// <summary>
        /// Adds a custom command to the dropdown menu.
        /// </summary>
        /// <param name="menuCommandName">Name of command as it shows up in the menu</param>
        /// <param name="myAction">an Action type variable, has to be void and parameterless</param>
        /// <param name="toolTip">The tooltip that shows up when hovering over the menu item.</param>
        /// <param name="enabledOnLockedCanvas">If the menu item is enabled on a locked canvas.</param>
        public void addCustomCommandToMenu(string menuCommandName, Action myAction, string toolTip, bool enabledOnLockedCanvas = true)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addCustomCommandMenuOption(menuCommandName, myAction, toolTip, !enabledOnLockedCanvas);
        }

        /// <summary>
        ///Adds the open canvas file option to the menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addOpenCanvasOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addOpenCanvasMenuOption();

        }


        /// <summary>
        ///Adds the lock canvas file option to the menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addLockCanvasOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addLockCanvasMenuOption();

        }


        /// <summary>
        ///Adds the unlock canvas file option to the menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addUnlockCanvasOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addunLockCanvasMenuOption();

        }

        /// <summary>
        ///Adds the save canvas to file option to the menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addSaveCanvasOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addSaveCanvasMenuOption();

        }
        /// <summary>
        ///Adds the exit application option to the menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addExitApplicationOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addExitMenuOption();
        }
        /// <summary>
        ///Adds the import canvas file to current canvas option to the menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addImportCanvasOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addImportCanvasMenuOption();
        }


        /// <summary>
        ///Adds the import canvas file to current canvas as a single container node option to the menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addImportCanvasAsContainerOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addImportCanvasACMenuOption();
        }

        /// <summary>
        ///Adds the command to open the form display licensing and other information about PUPPI  to the menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addAboutPUPPIOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addAboutPUPPIMenuOption();
        }
        /// <summary>
        ///Adds the command to open the top-down map view of the PUPPI program to the menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addMapViewOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addMapViewMenuOption();
        }
        /// <summary>
        ///Adds the command to redraw and refit connections between nodes to the menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addRefreshAllConnectionsOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addRefrConnMenuOption();
        }

        /// <summary>
        ///Adds the command to redo the last undone action performed on the canvas  to the menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addRedoOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addRedoMenuOption();
        }

        /// <summary>
        ///Adds the command to add a node to the canvas by searching for modules indexed in the canvas.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addNodebyModuleNameOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addNbNMenuOption();
        }

        /// <summary>
        ///Adds the command to paste at 0,0.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addPasteCopiedNodesToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addPaste00MenuOption();
        }


        /// <summary>
        ///Adds the command to undo the last action performed on the canvas  to the menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addUndoOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addUndoMenuOption();
        }
        //// <summary>
        //// Adds the command to undo select all nodes on the canvas  to this menu.This way menus can be customized with
        ////only the options desired by the developer.
        //// </summary>
        ////public void addSelectAllOptionToMenu()
        ////{
        ////    try
        ////    {
        ////        try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
        ////        catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        ////    }
        ////    catch
        ////    {

        ////    }
        ////    pgddm.addSelectAllMenuOption(); 
        ////}

        /// <summary>
        /// Adds the command to select all nodes on the canvas  to this menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addSelectAllOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addSelectAllMenuOption();
        }

        /// <summary>
        /// Adds the command to select all nodes on the canvas  to this menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addHideSelectedOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addHideSelectedMenuOption();
        }


        /// <summary>
        /// Adds the command to select all nodes on the canvas  to this menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addShowSelectedOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addShowSelectedMenuOption();
        }


        /// <summary>
        /// Adds the command to reset captions on all nodes on the canvas  to this menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addResetCaptionsOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addResetAllNodeCaptionsOption();
        }


        /// <summary>
        /// Adds the command to clear selection of nodes to this menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addSelectNoneOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addSelectNoneMenuOption();
        }

        /// <summary>
        /// Adds the command to copy selection of nodes to clipboard to this menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addCopySelectionOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addCopySelectionMenuOption();
        }


        /// <summary>
        /// Adds the command to export selection to a file to this menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addExportSelectionOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addExportSelectionMenuOption();
        }


        /// <summary>
        /// Adds the command to delete selected nodes  to this menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addDeleteSelectionOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.adddeleteSelectionMenuOption();
        }

        /// <summary>
        /// Adds the command to collapse selected nodes  to this menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addCollapseSelectionOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addCollapseSelectionMenuOption();
        }


        /// <summary>
        ///Adds the command to open the node and children tree view of the PUPPI program to the menu.This way menus can be customized with
        ///only the options desired by the developer.
        /// </summary>
        public void addTreeViewOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addTreeViewMenuOption();
        }

        /// <summary>
        ///Adds the option to open a help file
        ///The path to the help file should be calculated on the fly:
        ///example:
        ///Uri.UnescapeDataString(new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)) .AbsolutePath)+@"/PUPPI-user-help.chm
        /// </summary>
        public void addHelpOptionToMenu(string helpfilepath)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addHelpMenuOption(helpfilepath);


        }
        /// <summary>
        /// So the user can set the position of the camera by coordinates. In 3D camera will look at origin and in 2D down.
        /// </summary>
        public void addSetCameraPositionOptionToMenu(string helpfilepath)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addSetCameraPositionMenuOption();


        }
        /// <summary>
        /// Adds the option to rearrange nodes on the canvas in a better way based on connections to a menu.
        /// </summary>
        public void addRearrangeNodesOptionToMenu()
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addRARnodesMenuOption();


        }

        /// <summary>
        /// This saves the current layout of the PUPPI canvas in various formats.
        /// </summary>
        public void addExportSnapshotOptionToMenu(string helpfilepath)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
            pgddm.addExportSnapshotMenuOption();


        }


    }

    ///<summary>
    ///Instance of a PUPPI 3D Window , used for rendering objects created with the PUPPI3DModeling functions
    ///Can be used with the PUPPIGUIController to create Windows Forms of WPF controls.
    ///</summary>
    public class PUPPICADView
    {
        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

            dllName = dllName.Replace(".", "_");

            if (dllName.EndsWith("_resources")) return null;
            string an = Assembly.GetExecutingAssembly().GetName().Name;
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(an + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

            byte[] bytes = (byte[])rm.GetObject(dllName);

            if (bytes != null && bytes.Length > 0) return System.Reflection.Assembly.Load(bytes); else return null;
        }

        internal int sobjin = -1;
        internal PUPPICAD.PUPPI3DView p3dview;
        ///<summary>
        /// Access to the WPF User Control containing the PUPPI Program Canvas. Note that the User Control
        /// can only be initialized through the PUPPIProgramCanvas class.
        ///</summary>
        public PUPPICanvasControl myuserWPFControl { get; internal set; }
        ///<summary>
        /// Access to the Form Control containing the PUPPI Program Canvas, added diretcly to a Windows Form. 
        /// This is initialized when the static method AddPUPPIProgramCanvastoForm  is used.
        ///</summary>
        public ElementHost myFormControl { get; internal set; }


        /// <summary>
        ///  Initializes an instance of the PUPPI Program Canvas. User Controls for WPF are also instantiated. 
        /// To add this to a regular Windows Form, you need to use the static method AddPUPPIProgramCanvastoForm
        /// </summary>
        /// <param name="width">Width of the canvas in px.Default 800.This dimension applies to form element. WPF element stretches.</param>
        /// <param name="height">Height of the canvas in px.Default 600. This dimension applies to form element. WPF element stretches.</param>
        public PUPPICADView(int width = 800, int height = 600)
        {
            try
            {
                try { AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve); }
                catch { } AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            }
            catch
            {

            }
  
            PUPPICADView1(width, height);
        }
        private void PUPPICADView1(int width = 800, int height = 600)
        {

            myuserWPFControl = new PUPPICanvasControl();
            p3dview = new PUPPICAD.PUPPI3DView(0, 0, width, height);
            p3dview.clicked3DW += (sender, args) => { askedCADWClickEvent(sender); };
           // p3dview.dropped3DW += (sender, args) => { askedCADWDropEvent(sender); };
            //myuserWPFControl.Width = width;
            //myuserWPFControl.Height = height;
            //myuserWPFControl.UserControlCanvas.Width = width;
            //myuserWPFControl.UserControlCanvas.Height = height;
            myuserWPFControl.UserControlCanvas.Children.Add(p3dview.canvasview);

        }
        /// <summary>
        /// Gets the objects inside the CAD window
        /// </summary>
        /// <returns>A list of object type ModelVisual3D</returns>
        public List<ModelVisual3D> getCADWindowObjects()
        {
            List<ModelVisual3D> r = new List<ModelVisual3D>();
            foreach (Visual3D vv in p3dview.hv.Children)
            {
                //no lights
                if (vv is ModelVisual3D)
                {
                    ModelVisual3D v = vv as ModelVisual3D;
                    

                    //no lights
                    if (v!=p3dview.tramo && v != p3dview.tlv3d && v != p3dview.dlv3d && v != p3dview.flv3d && v != p3dview.blv3d && v != p3dview.llv3d && v != p3dview.rlv3d && v != p3dview.cv)
                    {
                        r.Add(v);
                    }
                }
            }
            return r;
        }
        /// <summary>
        /// gets the bounding box of objects in view
        /// </summary>
        /// <returns>Rect3D</returns>
        public Rect3D ObjectsBBOX()
        {
            return p3dview.getsfebbox();
        }

        /// <summary>
        /// Saves the content of the CAD window so that when a new PUPPI canvas is open or loaded, the objects created by the previous canvas are added to the CAD view;
        /// </summary>
        public void keepCADWindowObjects()
        {
            savedCADWinObjects = getCADWindowObjects();
        }
        /// <summary>
        /// Does not save the content of the CAD window so that when a new PUPPI canvas is open or loaded, the objects created by the previous canvas are added to the CAD view;
        /// </summary>
        public void clearCADWindowObjects()
        {
            savedCADWinObjects = null;
        }
        /// <summary>
        /// Adds a ModelVisual3D object to the 3D view
        /// </summary>
        /// <param name="mv"></param>
        public void addModelVisual3D(ModelVisual3D mv)
        {
            p3dview.hv.Children.Add(mv);
            Rect3D bbox = ObjectsBBOX();
           
            p3dview.redolighting(bbox);

        }
        /// <summary>
        /// Returns the index of the object the manipulator is on or -1 if no object selected
        /// </summary>
        /// <returns>integer</returns>
        public int GetSelectedObjectIndex()
        {
            return sobjin;
        }

        /// <summary>
        /// Adds an object created from a string representing an ASCII STL to the 3D view
        /// </summary>
        /// <param name="STLA">Ascii STL</param>
        /// <param name="separator">line separator</param>
        public void addStringSTL(string STLA,string separator)
        {
            ModelVisual3D mv = PUPPICAD.HelperClasses.utilities.ReadSTLFromString(STLA, separator);
            p3dview.hv.Children.Add(mv);
            Rect3D bbox = ObjectsBBOX();
            p3dview.redolighting(bbox);

        }


        /// <summary>
        /// Removes object at index
        /// </summary>
        /// <param name="index">valid index</param>
        public void RemoveModelVisual3DByIndex(int index)
        {
            int iindex = -1;
            int cindex = 0;
            bool found = false;
            foreach (Visual3D vv in p3dview.hv.Children)
            {
                //no lights
                if (vv is ModelVisual3D)
                {
                    ModelVisual3D v = vv as ModelVisual3D;
                    //no lights
                    if (v!=p3dview.tramo && v != p3dview.tlv3d && v != p3dview.dlv3d && v != p3dview.flv3d && v != p3dview.blv3d && v != p3dview.llv3d && v != p3dview.rlv3d && v != p3dview.cv)
                    {
                        iindex++;
                        if (iindex == index)
                        {
                            found = true;
                            break;
                        }
                    }
                }
                cindex++;
            }
            if (found)
            {
                p3dview.hv.Children.RemoveAt(cindex);
                Rect3D bbox = ObjectsBBOX();
                if (sobjin == iindex) sobjin = -1;
                p3dview.redolighting(bbox);
            }
        }
        /// <summary>
        /// Removes object by name
        /// </summary>
        /// <param name="name">Name</param>
        public void RemoveModelVisual3DByName(string name)
        {

            int cindex = 0;
            bool found = false;
            int iindex = -1;
            foreach (Visual3D vv in p3dview.hv.Children)
            {
                //no lights
                if (vv is ModelVisual3D)
                {
                    ModelVisual3D v = vv as ModelVisual3D;
                    //no lights
                    if (v != p3dview.tramo && v != p3dview.tlv3d && v != p3dview.dlv3d && v != p3dview.flv3d && v != p3dview.blv3d && v != p3dview.llv3d && v != p3dview.rlv3d && v != p3dview.cv)
                    {
                        iindex++;
                        if (v.GetName() != null && v.GetName() == name)
                        {
                            found = true;
                            break;
                        }
                    }
                }
                cindex++;
            }
            if (found)
            {
                p3dview.hv.Children.RemoveAt(cindex);
                Rect3D bbox = ObjectsBBOX();
                if (sobjin == iindex) sobjin = -1;
                p3dview.redolighting(bbox);
            }
        }

        /// <summary>
        /// Deletes all ModelVisual3D objects in the window
        /// </summary>
        public void RemoveAllObjects()
        {
            foreach (ModelVisual3D mv in getCADWindowObjects())
            {
                try
                {
                    p3dview.hv.Children.Remove(mv);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Hardcode Transforms object at index
        /// </summary>
        /// <param name="index">valid index</param>
        public void HardcodeTransformsModelVisual3DByIndex(int index)
        {
            int iindex = -1;
            int cindex = 0;
            bool found = false;
            foreach (Visual3D vv in p3dview.hv.Children)
            {
                //no lights
                if (vv is ModelVisual3D)
                {
                    ModelVisual3D v = vv as ModelVisual3D;
                    //no lights
                    if (v != p3dview.tramo && v != p3dview.tlv3d && v != p3dview.dlv3d && v != p3dview.flv3d && v != p3dview.blv3d && v != p3dview.llv3d && v != p3dview.rlv3d && v != p3dview.cv)
                    {
                        iindex++;
                        if (iindex == index)
                        {
                            found = true;
                            break;
                        }
                    }
                }
                cindex++;
            }
            if (found)
            {

                ModelVisual3D founded = (p3dview.hv.Children[cindex] as ModelVisual3D).cloneVisualAndChildren();
                founded = PUPPICAD.HelperClasses.utilities.hardcodeTransforms(founded);
                p3dview.hv.Children.RemoveAt(cindex);
                p3dview.hv.Children.Add(founded);

                
            }
        }
        /// <summary>
        /// Hardcode Transforms object by name
        /// </summary>
        /// <param name="name">Name</param>
        public void HardcodeTransformsModelVisual3DByName(string name)
        {

            int cindex = 0;
            bool found = false;
            foreach (Visual3D vv in p3dview.hv.Children)
            {
                //no lights
                if (vv is ModelVisual3D)
                {
                    ModelVisual3D v = vv as ModelVisual3D;
                    //no lights
                    if (v != p3dview.tramo && v != p3dview.tlv3d && v != p3dview.dlv3d && v != p3dview.flv3d && v != p3dview.blv3d && v != p3dview.llv3d && v != p3dview.rlv3d && v != p3dview.cv)
                    {

                        if (v.GetName() != null && v.GetName() == name)
                        {
                            found = true;
                            break;
                        }
                    }
                }
                cindex++;
            }
            if (found)
            {
                ModelVisual3D founded = (p3dview.hv.Children[cindex] as ModelVisual3D).cloneVisualAndChildren();
                founded = PUPPICAD.HelperClasses.utilities.hardcodeTransforms(founded);
                p3dview.hv.Children.RemoveAt(cindex);
                p3dview.hv.Children.Add(founded);
            }
        }

        /// <summary>
        /// Changes object color at index
        /// </summary>
        /// <param name="index">valid index</param>
        ///  /// <param name="redRatio">0..1</param>
        /// <param name="greenRatio">0..1</param>
        /// <param name="blueRatio">0..1</param>
        /// <param name="alphaRatio">0..1</param>
        public void ChangeModelVisual3DColorByIndex(int index, double redRatio, double greenRatio, double blueRatio, double alphaRatio)
        {
            int iindex = -1;
            int cindex = 0;
            bool found = true;
            foreach (Visual3D vv in p3dview.hv.Children)
            {
                //no lights
                if (vv is ModelVisual3D)
                {
                    ModelVisual3D v = vv as ModelVisual3D;
                    //no lights
                    if (v != p3dview.tramo && v != p3dview.tlv3d && v != p3dview.dlv3d && v != p3dview.flv3d && v != p3dview.blv3d && v != p3dview.llv3d && v != p3dview.rlv3d && v != p3dview.cv)
                    {
                        iindex++;
                        if (iindex == index)
                        {
                            found = true;
                            break;

                        }
                    }
                }
                cindex++;
            }
            if (!found) return;
            ModelVisual3D ml = p3dview.hv.Children[index] as ModelVisual3D;
            if (ml == null) return;
            ModelVisual3D malao = PUPPICAD.HelperClasses.utilities.SetObjectColor(ml, redRatio, blueRatio, greenRatio, alphaRatio);

            p3dview.hv.Children.RemoveAt(index);
            p3dview.hv.Children.Add(malao);
        }
        /// <summary>
        /// Changes object color by name
        /// </summary>
        /// <param name="name">Name as set when added</param>
        /// <param name="redRatio">0..1</param>
        /// <param name="greenRatio">0..1</param>
        /// <param name="blueRatio">0..1</param>
        /// <param name="alphaRatio">0..1</param>
        public void ChangeModelVisual3DColorByName(string name,double redRatio,double greenRatio,double blueRatio,double alphaRatio)
        {

            int cindex = 0;
            bool found = false;
            foreach (Visual3D vv in p3dview.hv.Children)
            {
                //no lights
                if (vv is ModelVisual3D)
                {
                    ModelVisual3D v = vv as ModelVisual3D;
                    //no lights
                    if (v != p3dview.tramo && v != p3dview.tlv3d && v != p3dview.dlv3d && v != p3dview.flv3d && v != p3dview.blv3d && v != p3dview.llv3d && v != p3dview.rlv3d && v != p3dview.cv)
                    {

                        if (v.GetName() != null && v.GetName() == name)
                        {
                            found = true;
                            break;
                        }
                           
                    }
                }
                cindex++;
            }
            if (!found) return;
            ModelVisual3D ml = p3dview.hv.Children[cindex] as ModelVisual3D;
            if (ml == null) return;
            ModelVisual3D malao = PUPPICAD.HelperClasses.utilities.SetObjectColor(ml, redRatio, blueRatio, greenRatio, alphaRatio);
            
            p3dview.hv.Children.RemoveAt(cindex);
            p3dview.hv.Children.Add(malao);
        }

        /// <summary>
        /// Add combined manipulator to object by name
        /// </summary>
        /// <param name="name">Valid name</param>
        /// <param name="placementPosition">Absolute coords of placement of manipulator if positioning is set to exact</param>
        /// <param name="canRotate">User can rotate objects</param>
        /// <param name="positioning">Positioning mode flag, Exact (specify point),Bounding Box Corner or Bounding Box Center</param>
        public void AddCombinedManipulatorByName(string name, Point3D placementPosition, bool canRotate = true, PUPPIGUISettings.CADWindowManipulatorPosition positioning = PUPPIGUISettings.CADWindowManipulatorPosition.BoxCorner)
        {
            if (p3dview.hv.Children.Contains(p3dview.tramo)) p3dview.hv.Children.Remove(p3dview.tramo);
            int cindex = 0;
            bool found = false;
            int iindex = -1;
            foreach (Visual3D vv in p3dview.hv.Children)
            {
                
                //no lights
                if (vv is ModelVisual3D)
                {
                    ModelVisual3D v = vv as ModelVisual3D;
                    //no lights
                    if (v != p3dview.tramo && v != p3dview.tlv3d && v != p3dview.dlv3d && v != p3dview.flv3d && v != p3dview.blv3d && v != p3dview.llv3d && v != p3dview.rlv3d && v != p3dview.cv)
                    {
                        iindex++;
                        if (v.GetName()!=null &&  v.GetName()== name)
                        {
                            found = true;
                            break;
                        }
                    }
                }
                cindex++;
            }
            if (found)
            {
                ModelVisual3D mn = p3dview.hv.Children[cindex] as ModelVisual3D;
                if (mn != null)
                {

                    sobjin = iindex;
                    p3dview.AddCombinedManipulator(mn,canRotate,placementPosition,positioning);
                }
            }
        }
        /// <summary>
        /// Adds a manipulator to object with specified index (index as appears in collection getCADWindowObject())
        /// </summary>
        /// <param name="index">Can figure out with GetCADWindowObjects function</param>
        /// <param name="placementPosition">Absolute coords of placement of manipulator if positioning is set to exact</param>
        /// <param name="canRotate">User can rotate objects</param>
        /// <param name="positioning">Positioning mode flag, Exact (specify point),Bounding Box Corner or Bounding Box Center.
        /// When CADWindowManipulatorPosition is Exact, PUPPIGUISettings.CADManipExactDisplacement determines how much to offset (X,Y,Z) the manipulator from point clicked to allow double clicking. 
        /// </param>
        public void AddCombinedManipulatorByIndex(int index, Point3D placementPosition, bool canRotate = true, PUPPIGUISettings.CADWindowManipulatorPosition positioning = PUPPIGUISettings.CADWindowManipulatorPosition.BoxCorner)
        {
            bool found = false;
            if (p3dview.hv.Children.Contains(p3dview.tramo)) p3dview.hv.Children.Remove(p3dview.tramo);
            int iindex = -1;
            int cindex = 0;
            foreach (Visual3D vv in p3dview.hv.Children)
            {
                //no lights
                if (vv is ModelVisual3D)
                {
                    ModelVisual3D v = vv as ModelVisual3D;
                    if (v != p3dview.tramo && v != p3dview.tlv3d && v != p3dview.dlv3d && v != p3dview.flv3d && v != p3dview.blv3d && v != p3dview.llv3d && v != p3dview.rlv3d && v != p3dview.cv)
                    {
                        iindex++;
                        if (iindex == index)
                        {
                            found = true;
                            break;
                        }
                    }
                }
                cindex++;
            }
            if (found)
            {
                ModelVisual3D mn = p3dview.hv.Children[cindex] as ModelVisual3D;
                p3dview.AddCombinedManipulator(mn, canRotate, placementPosition, positioning);
                sobjin = index;
            }
        }

        /// <summary>
        /// Removes the manipulator
        /// </summary>
        public void RemoveManipulator()
        {
            try
            {
                if (p3dview.hv.Children.Contains(p3dview.tramo as Visual3D))
                {
                    p3dview.hv.Children.Remove(p3dview.tramo as Visual3D);
                    sobjin = -1;
                }
            }
            catch
            {

            }
        }
        
        /// <summary>
        /// Event raised when CAD window clicked on. sender.ToString() gives the object name, if any, index among objects in CAD window button clicked and coordinates. separator is _|_
        /// Example: LB_|_name_|_index_|_{x,y,z}
        /// </summary>
        public event EventHandler clickCADWOperationEvent;
        internal void askedCADWClickEvent(object es)
        {
            if (clickCADWOperationEvent != null && es != null) clickCADWOperationEvent(es, EventArgs.Empty);
        }

        /////// <summary>
        /////// Event raised when CAD window dropped on. sender.ToString() gives the object name, if any, index among objects in CAD window and coordinates. separator is _|_
        /////// Example: DROP_|_name_|_index_|_{x,y,z}
        /////// </summary>
        ////public event EventHandler dropCADWOperationEvent;
        //internal void askedCADWDropEvent(object es)
        //{
        //    if (dropCADWOperationEvent != null && es != null) dropCADWOperationEvent(es, EventArgs.Empty);
        //}

        internal List<ModelVisual3D> savedCADWinObjects;
    }

}


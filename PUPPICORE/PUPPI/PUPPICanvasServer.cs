using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using PUPPIModel;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
//this is required for ArrayList
using System.Collections;
using System.Media;
using PUPPIGUI;
using System.Threading;
using System.Reflection;
using System.IO;

namespace PUPPIServer
{
    /// <summary>
    /// This class contains commands to operate a canvas TCP socket server which allows clients to access canvas layout and node information, and send commands to the canvas.
    /// </summary>
    public static class PUPPICanvasTCPServer
    {



        internal static int NumberClientThreads = 0;

        /// <summary>
        /// Starts TCP socket canvas server
        /// </summary>
        /// <param name="myCanvasController">PUPPIGUIController</param>
        /// <param name="IP">local IP Address (string)</param>
        /// <param name="openPort">port (integer). Port needs to be open for public connections</param>
        /// <param name="password">password (string)</param>
        /// <param name="provideFullUpdate">if true, canvas data is stored every time changes are made, useful for clients that need to render canvas </param>
        /// <returns>publicIP__localIP__port or error, as string</returns>
        public static string startServer(PUPPIGUIController.PUPPIProgramCanvas myCanvasController, string IP, int openPort, string password, bool provideFullUpdate)
        {
            PUPPIruntimesettings.PUPPICanvasTCPServerIsRunning = true;
            PUPPIruntimesettings.PUPPICanvasServerUpdatingFully = provideFullUpdate;
            if (myCanvasController != null && myCanvasController.pcanvas != null)
                myCanvasController.pcanvas.doAnInitialServerUpdate();
            Thread serverev = new Thread(() => PUPPIServer.PUPPICanvasTCPServer.startMyPUPPISrver(myCanvasController, IP, openPort, password, provideFullUpdate));

            serverev.IsBackground = true;
            serverev.Name = "server";
            serverev.Start();

            string conn = "error";
            DateTime sst = DateTime.Now;
            DateTime nower = DateTime.Now;
            double elapmi = ((TimeSpan)(nower - sst)).TotalMilliseconds;
            while (conn == "error" && elapmi < 10000)
            {
                conn = PUPPIServer.PUPPICanvasTCPServer.getMeConnectionInformation();
                nower = DateTime.Now;
                elapmi = ((TimeSpan)(nower - sst)).TotalMilliseconds;

            }
            return conn;
        }

        /// <summary>
        /// Stops the TCP socket server
        /// </summary>
        public static void stopServer()
        {
            PUPPIruntimesettings.PUPPICanvasTCPServerIsRunning = false;
        }
        internal static void startMyPUPPISrver(PUPPIGUIController.PUPPIProgramCanvas myCanvasControllerme, string IPA, int openPortME, string passwordserverme, bool provideMyFullUpdatesme)
        {
            AsynchronousSocketListenerMe asyl = new AsynchronousSocketListenerMe(myCanvasControllerme, IPA, openPortME, passwordserverme);

            AsynchronousSocketListenerMe.StartListening();

        }

        internal static string getMeConnectionInformation()
        {
            return AsynchronousSocketListenerMe.getMeConnData();
        }
        // State object for reading client data asynchronously
        internal class StateObject
        {
            // Client  socket.
            public Socket workSocket = null;
            // Size of receive buffer.
            public const int BufferSize = 1024;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
            // Received data string.
            public StringBuilder sb = new StringBuilder();


        }
        internal class AsynchronousSocketListenerMe
        {

            private static string spassword = "";
            internal static PUPPIGUIController.PUPPIProgramCanvas myPUPPIGUI;
            private static IPAddress ipAddress = null;
            // Thread signal.
            public static ManualResetEvent allDone = new ManualResetEvent(false);
            private static int port = 11000;
            internal AsynchronousSocketListenerMe(PUPPIGUIController.PUPPIProgramCanvas myCanvasController, string IPA, int openPort, string password)
            {
                port = openPort;
                spassword = password;
                myPUPPIGUI = myCanvasController;
                try
                {
                    ipAddress = IPAddress.Parse(IPA);
                }
                catch
                {
                    ipAddress = null;
                }
                // if (ipAddress == null) throw new Exception("Invalid IP Address");
            }


            internal static string getMeConnData()
            {
                if (ipAddress == null) return "error";

                try
                {
                    string pubIp = "";
                    try
                    {
                        pubIp = new System.Net.WebClient().DownloadString("https://api.ipify.org");
                    }
                    catch
                    {
                        pubIp = "failedpublic";
                    }

                    return pubIp + "__" + ipAddress.ToString() + "__" + port.ToString();
                }
                catch
                {
                    return "error";
                }
            }

            internal static void StartListening()
            {
               
                if (ipAddress == null) return;

                if (myPUPPIGUI != null && myPUPPIGUI.pcanvas != null)
                {
                    PUPPICanvas.currentCanvasStatusServer = myPUPPIGUI.pcanvas.readMyCanvasStatus();
                    if (PUPPIGUISettings.serverExportHiddenNodes)
                    {
                        PUPPICanvas.currentXMLRepServer = myPUPPIGUI.pcanvas.saveCanvasRepresentationToXML();
                    }
                    else
                    {
                        PUPPICanvas.currentXMLRepServer = myPUPPIGUI.pcanvas.saveCanvasRepresentationToXMLNoHI();

                    }
                }

                NumberClientThreads = 0;
                // Data buffer for incoming data.
                byte[] bytes = new Byte[1024];



                // IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());  //Dns.Resolve(Dns.GetHostName());
                //// IPAddress ipAddress = ipHostInfo.AddressList[0];


                // foreach (var addr in ipHostInfo.AddressList)
                // {
                //     if (addr.AddressFamily == AddressFamily.InterNetwork)        // this is IPv4
                //     {
                //         ipAddress = addr;
                //         break;
                //     }
                // }
                // if (ipAddress == null)
                //     throw new Exception("Error finding an IPv4 address for localhost");

                // Establish the local endpoint for the socket.
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.
                Socket listener = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                //   listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                //  ;
                // Bind the socket to the local endpoint and listen for incoming connections.



                try
                {
                    listener.Bind(localEndPoint);

                    listener.Listen(100);




                    while (PUPPIruntimesettings.PUPPICanvasTCPServerIsRunning)
                    {
                        // Set the event to nonsignaled state.
                        allDone.Reset();

                        // Start an asynchronous socket to listen for connections.
                        // Console.WriteLine("Waiting for a connection...");
                        listener.BeginAccept(
                            new AsyncCallback(AcceptCallback),
                            listener);
                        //  listener.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.KeepAlive, 0);
                        // Wait until a connection is made before continuing.
                        allDone.WaitOne();
                    }

                }
                catch (Exception e)
                {
                    //  MessageBox.Show(e.ToString());  
                    //Console.WriteLine(e.ToString());
                }

                //  Console.WriteLine("\nPress ENTER to continue...");
                //  Console.Read();

            }
            internal static void AcceptCallback(IAsyncResult ar)
            {

                // Signal the main thread to continue.
                allDone.Set();

                // Get the socket that handles the client request.
                Socket listener = (Socket)ar.AsyncState;

                Socket handler = listener.EndAccept(ar);

                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }
            internal static void ReadCallback(IAsyncResult ar)
            {
                NumberClientThreads++;
                String content = String.Empty;
                string[] sep = { "_|_" };
                // Retrieve the state object and the handler socket
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;
                try
                {
                    // Read data from the client socket. 
                    int bytesRead = handler.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        // There  might be more data, so store the data received so far.
                        state.sb.Append(Encoding.ASCII.GetString(
                            state.buffer, 0, bytesRead));

                        // Check for end-of-file tag. If it is not there, read 
                        // more data.
                        content = state.sb.ToString();

                        if (content.IndexOf(spassword + "<EOF>") > -1)
                        {
                            // All the data has been read from the 
                            // client. Display it on the console.
                            // Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                            //     content.Length, content);
                            // Echo the data back to the client.


                            string myCommand = content.Replace(spassword + "<EOF>", "");

                            if (PUPPIDEBUG.PUPPIDebugger.debugenabled)
                            {
                                PUPPIDEBUG.PUPPIDebugger.log("TCP server started executing command: " + myCommand);
                            }

                            string resendBack = "";
                            if (myCommand.ToLower() == "canvasgetstatus")
                            {
                                resendBack = PUPPICanvas.currentCanvasStatusServer;   //myPUPPIGUI.canvasReadableReport();// + "<EOF>";
                            }
                            else
                                if (myCommand.ToLower() == "canvasgetlayout")
                                {
                                    resendBack = PUPPICanvas.currentXMLRepServer; //myPUPPIGUI.XMLcanvasLayout();// + "<EOF>";   
                                }
                                //else
                                else if (myCommand.ToLower().Contains("getnodeinformationxml"))
                                {

                                    string[] myInput = myCommand.Split(sep, StringSplitOptions.None);
                                    string infonode = "";
                                    if (PUPPICanvas.nodeInfoXMLServer.ContainsKey(myInput[1]))
                                    {
                                        infonode = PUPPICanvas.nodeInfoXMLServer[myInput[1]];
                                    }
                                    resendBack = infonode;//myPUPPIGUI.pcanvas.interpretMyTextualCommand(myCommand);//  + "<EOF>";
                                }
                                //else
                                //if (myCommand.ToLower().Contains("getnoderendererstatexml"))
                                //{
                                //    resendBack = myPUPPIGUI.pcanvas.interpretMyTextualCommand(myCommand);// +"<EOF>";
                                //}

                                else
                                    if (myCommand.ToLower().Contains("getnoderendererimage"))
                                    {

                                        string[] myInput = myCommand.Split(sep, StringSplitOptions.None);

                                        string filenamer = ""; //myPUPPIGUI.pcanvas.interpretMyTextualCommand("getnoderendererimagefilename_|_" + myInput[1]);
                                        resendBack = "";

                                        if (PUPPICanvas.nodeRendererImageFilesServer.ContainsKey(myInput[1]))
                                        {
                                            filenamer = PUPPICanvas.nodeRendererImageFilesServer[myInput[1]];
                                        }
                                        //else
                                        //{
                                        //    if (PUPPICanvas.nodeRendererModel3DServer.ContainsKey(myInput[1]))
                                        //    {
                                        //        List<Model3D> gll = PUPPICanvas.nodeRendererModel3DServer[myInput[1]];
                                        //        ModelVisual3D mlv = new ModelVisual3D();
                                        //        foreach (Model3D mld in gll)
                                        //        {
                                        //            ModelVisual3D mllv = new ModelVisual3D();
                                        //            mllv.Content = mld.Clone();
                                        //            mlv.Children.Add(mllv);

                                        //        }
                                        //        if (mlv.Children.Count > 0)
                                        //        {
                                        //            string rootName = "ROnTheFlyN" + myInput[1];
                                        //            filenamer = PUPPICAD.HelperClasses.utilities.renderModelVisual3D(mlv, "", rootName);
                                        //        }
                                        //    }
                                        //}
                                        if (System.IO.File.Exists(filenamer))
                                            SendFile(handler, filenamer);
                                        else
                                            resendBack = "notfound";

                                        // resendBack = myPUPPIGUI.pcanvas.interpretMyTextualCommand(myCommand) + "<EOF>";
                                    }
                                    else
                                        if (myCommand.ToLower().Contains("getnoderendererstlstring"))
                                        {

                                            string[] myInput = myCommand.Split(sep, StringSplitOptions.None);

                                            resendBack = "";

                                            if (PUPPICanvas.nodeRendererSTLServer.ContainsKey(myInput[1]))
                                            {
                                                resendBack = PUPPICanvas.nodeRendererSTLServer[myInput[1]];
                                            }
                                            else if (PUPPICanvas.nodeRendererModel3DServer.ContainsKey(myInput[1]))
                                            {

                                                List<Model3D> gll = PUPPICanvas.nodeRendererModel3DServer[myInput[1]];
                                                ModelVisual3D mlv = new ModelVisual3D();
                                                foreach (Model3D mld in gll)
                                                {
                                                    ModelVisual3D mllv = new ModelVisual3D();
                                                    mllv.Content = mld.Clone();
                                                    mlv.Children.Add(mllv);

                                                }
                                                ArrayList linesA = new ArrayList();

                                                PUPPICAD.HelperClasses.helperfunctions.writegeomSTLrecursivelyWT(mlv, linesA, "ROnTheFlyN" + myInput[1], new Transform3DGroup());



                                                string stllines = "";
                                                for (int i = 0; i < linesA.Count; i++)
                                                {
                                                    string line = linesA[i].ToString();
                                                    stllines += "\n" + line;
                                                }

                                                resendBack = stllines;
                                            }
                                            else
                                                resendBack = "notfound";
                                        }
                                        else
                                            if (myCommand.ToLower() == "getindexedmodulenames")
                                            {
                                                resendBack = myPUPPIGUI.sendImmediateCommandAsText("getindexedmodulenames");//   + "<EOF>";
                                            }
                                            else if (myCommand.ToLower().Contains("getnodeoutputvalue"))
                                            {
                                                string[] myInput = myCommand.Split(sep, StringSplitOptions.None);
                                                string nid = myInput[1];
                                                int oindex = Convert.ToInt16(myInput[2]);
                                                while (PUPPIModule.concurrentProcesses > 0)
                                                    Thread.Sleep(10);
                                                object o = myPUPPIGUI.getNodeOutputValue(nid, oindex);
                                                if (o != null)
                                                {
                                                    SendMyData(handler, o);
                                                }
                                                else
                                                {
                                                    resendBack = "null";
                                                }
                                            }


                                            //else
                                            //if (myCommand.ToLower()  == "canvasgetsave")
                                            //{
                                            //    resendBack = myPUPPIGUI.saveCanvasToString();// +"<EOF>";
                                            //}
                                            //else if (myCommand.ToLower() == "getcanvaschangedtime")
                                            //{
                                            //    resendBack = myPUPPIGUI.getCanvasChangedTime() ;// +"<EOF>";
                                            //}
                                            else
                                            {
                                                myPUPPIGUI.pcanvas.commandQueue.Add(myCommand);
                                                resendBack = "sent";
                                            }
                            // string response = myPUPPIGUI.sendCommandAsText(myCommand);



                            //if (content.Replace("<EOF>", "").ToLower() == "status")
                            //{



                            //if (response.ToLower() !="success")
                            //{
                            //Send(handler, response + "<EOF>");
                            if (resendBack != "")
                                SendString(handler, resendBack);
                            //}


                            //}
                            if (PUPPIDEBUG.PUPPIDebugger.debugenabled)
                            {
                                PUPPIDEBUG.PUPPIDebugger.log("Canvas TCP server finished executing command: " + myCommand);
                            }
                        }
                        else
                        {
                            // Not all data received. Get more.
                            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReadCallback), state);
                        }

                    }
                }
                catch
                {

                }
                if (NumberClientThreads > 0) NumberClientThreads--;
            }
            //private static void Send(Socket handler, String data)
            //{
            //    // Convert the string data to byte data using ASCII encoding.
            //    byte[] byteData = Encoding.ASCII.GetBytes(data);

            //    // Begin sending the data to the remote device.
            //    handler.BeginSend(byteData, 0, byteData.Length, 0,
            //        new AsyncCallback(SendCallback), handler);
            //}

            private static void SendString(Socket handler, String data)
            {

                byte[] isfile = { 0 };

                // Convert the string data to byte data using ASCII encoding.
                byte[] tData = Encoding.ASCII.GetBytes(data);
                byte[] byteData = isfile.Concat(tData).ToArray();
                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }


            private static void SendFile(Socket handler, String filename)
            {

                byte[] fileData = System.IO.File.ReadAllBytes(filename);
                //System.IO.FileInfo fi = new System.IO.FileInfo(filename);
                //long fsize = fi.Length;
                ////four bytes
                //byte[] flenght = System.BitConverter.GetBytes(fsize);
                //flag byte
                byte[] isfile = { 1 };
                byte[] byteData = isfile.Concat(fileData).ToArray();

                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);

                //handler.BeginSendFile(filename, new AsyncCallback(SendCallback), handler);

                //// Begin sending the data to the remote device.
                //handler.BeginSend(byteData, 0, byteData.Length, 0,
                //    new AsyncCallback(SendCallback), handler);
            }

            static byte[] ObjectToByteArray(object obj)
            {
                if (obj == null)
                    return null;
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    bf.Serialize(ms, obj);
                    return ms.ToArray();
                }
            }
            //byte flag is
            //0 text, 1 bynary file, 2 binary data
            private static void SendMyData(Socket handler, object myData)
            {

                byte[] fileData = ObjectToByteArray(myData);

                //flag byte
                byte[] isfile = { 2 };
                byte[] byteData = isfile.Concat(fileData).ToArray();

                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);


            }
            private static void SendCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the socket from the state object.
                    Socket handler = (Socket)ar.AsyncState;

                    // Complete sending the data to the remote device.
                    int bytesSent = handler.EndSend(ar);
                    //Console.WriteLine("Sent {0} bytes to client.", bytesSent);



                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                }
                catch (Exception e)
                {
                    // Console.WriteLine(e.ToString());
                }
            }
        }
    }

    public static class PUPPICanvasHTTPServer
    {
        internal static int NumberClientThreads = 0;
        static int myhttpprt;
        static PUPPIGUIController.PUPPIProgramCanvas myPUPPIGUI;
        static string myhttppwd;
        static HttpListener mylistener;
        /// <summary>
        /// Starts an HTTPListener
        /// If you get "error" as return try cmd as admin for your port e.g.: netsh http add urlacl url=http://*:8088/ user=everyone
        /// You can delete the allocation as follows: netsh http delete urlacl url=http://*:8088/
        /// For external connections you need to enable firewall inbound rule: BranchCache Content Retrieval (HTTP-In)
        /// Warning: the above could pose security risks! Activate at own risk!
        /// </summary>
        /// <param name="myCanvasController">PUPPIProgramCanvas to interact with</param>
        /// <param name="port">integer port value (port needs to be open for public connections)</param>
        /// <param name="password">string password</param>
        /// <returns>Public IP __ port or "error" if failed </returns>
        public static string startServer(PUPPIGUIController.PUPPIProgramCanvas myCanvasController, int port, string password)
        {
            myPUPPIGUI = myCanvasController;
            myhttpprt = port;
            myhttppwd = password;
            return startMyHTTPServer();
        }
        /// <summary>
        /// Stops the HTTPListener
        /// </summary>
        public static void stopServer()
        {
            try
            {
                mylistener.Stop();
            }
            catch
            {

            }
            PUPPIruntimesettings.PUPPICanvasHTTPServerIsRunning = false;

        }
        //netsh http add urlacl url=http://+:8088/ user=everyone
        static string startMyHTTPServer()
        {

  

            if (myPUPPIGUI != null && myPUPPIGUI.pcanvas != null)
            {
                PUPPICanvas.currentCanvasStatusServer = myPUPPIGUI.pcanvas.readMyCanvasStatus();
                if (PUPPIGUISettings.serverExportHiddenNodes)
                {
                    PUPPICanvas.currentXMLRepServer = myPUPPIGUI.pcanvas.saveCanvasRepresentationToXML();
                }
                else
                {
                    PUPPICanvas.currentXMLRepServer = myPUPPIGUI.pcanvas.saveCanvasRepresentationToXMLNoHI();

                }
            }
            NumberClientThreads = 0;
            mylistener = new HttpListener();
            mylistener.Prefixes.Add("http://*:" + myhttpprt.ToString() + "/");
            //  mylistener.Prefixes.Add("http://localhost/");
            try
            {
                PUPPIruntimesettings.PUPPICanvasHTTPServerIsRunning = true;
                //this will do visual updates, no other reason to launch HTTP server
                PUPPIruntimesettings.PUPPICanvasServerUpdatingFully = true;
                if (myPUPPIGUI != null && myPUPPIGUI.pcanvas != null)
                    myPUPPIGUI.pcanvas.doAnInitialServerUpdate();
                mylistener.Start();
                ProcessMyAsync(mylistener);
                return getMeConnectionInformation();
            }
            catch (Exception exy)
            {
                PUPPIruntimesettings.PUPPICanvasHTTPServerIsRunning = false;
                return "error";
            }

        }

        internal static string getMeConnectionInformation()
        {
            try
            {
                string pubIp = "";
                try
                {
                    pubIp = new System.Net.WebClient().DownloadString("https://api.ipify.org");
                }
                catch
                {
                    pubIp = "failedpublic";
                }

                return pubIp + "__" + myhttpprt.ToString();
            }
            catch
            {
                return "error";
            }
        }

        static async Task ProcessMyAsync(HttpListener mylistener)
        {

            while (PUPPIruntimesettings.PUPPICanvasHTTPServerIsRunning)
            {
                HttpListenerContext context = await mylistener.GetContextAsync();
                HandleMyRequestAsync(context);

                // TODO: figure out the best way add ongoing tasks to OngoingTasks.
            }
        }

        static async Task HandleMyRequestAsync(HttpListenerContext mycontext)
        {

            // Do processing here, possibly affecting KeepGoing to make the 
            // server shut down.

            await Task.Delay(PUPPIGUISettings.HTTPServerProcessDelay);
            MyPerform(mycontext);
        }

        static void MyPerform(HttpListenerContext ctx)
        {
            NumberClientThreads++;
            try
            {
                HttpListenerRequest myHrequest = ctx.Request;
                System.IO.StreamReader myHreader = new System.IO.StreamReader(myHrequest.InputStream, myHrequest.ContentEncoding);
                // S contain parameters and values
                string s = myHreader.ReadToEnd();
                string[] pairs = s.Split('&');
                Hashtable formVars = new Hashtable();
                for (int x = 0; x < pairs.Length; x++)
                {
                    string[] item = pairs[x].Split('=');
                    formVars.Add(item[0], item[1]);
                }
                String cmd = formVars["command"].ToString();
                String myHpassword = formVars["ssw"].ToString();
                string[] sep = { "_|_" };
                if (myHpassword == myhttppwd)
                {



                    string myCommand = cmd.Replace("%7C", "|").Replace("%5F", "_");

                    if (PUPPIDEBUG.PUPPIDebugger.debugenabled)
                    {
                        PUPPIDEBUG.PUPPIDebugger.log("HTTP server started executing command: " + myCommand);
                    }

                    string resendBack = "";
                    if (myCommand.ToLower() == "canvasgetstatus")
                    {
                        resendBack = PUPPICanvas.currentCanvasStatusServer;
                    }
                    else
                        if (myCommand.ToLower() == "canvasgetlayout")
                        {
                            resendBack = PUPPICanvas.currentXMLRepServer;
                        }
                        //else
                        else if (myCommand.ToLower().Contains("getnodeinformationxml"))
                        {

                            string[] myInput = myCommand.Split(sep, StringSplitOptions.None);
                            string infonode = "";
                            if (PUPPICanvas.nodeInfoXMLServer.ContainsKey(myInput[1]))
                            {
                                infonode = PUPPICanvas.nodeInfoXMLServer[myInput[1]];
                            }
                            resendBack = infonode;
                        }


                        else
                            if (myCommand.ToLower().Contains("getnoderendererimage"))
                            {

                                string[] myInput = myCommand.Split(sep, StringSplitOptions.None);

                                string filenamer = "";
                                resendBack = "";

                                if (PUPPICanvas.nodeRendererImageFilesServer.ContainsKey(myInput[1]))
                                {
                                    filenamer = PUPPICanvas.nodeRendererImageFilesServer[myInput[1]];
                                }
                                //else
                                //{
                                //    if (PUPPICanvas.nodeRendererModel3DServer.ContainsKey(myInput[1]))
                                //    {
                                //        List<Model3D> gll = PUPPICanvas.nodeRendererModel3DServer[myInput[1]];
                                //        ModelVisual3D mlv = new ModelVisual3D();
                                //        foreach (Model3D mld in gll)
                                //        {
                                //            ModelVisual3D mllv = new ModelVisual3D();
                                //            mllv.Content = mld.Clone();
                                //            mlv.Children.Add(mllv);

                                //        }
                                //        if (mlv.Children.Count > 0)
                                //        {
                                //            string rootName = "ROnTheFlyN" + myInput[1];
                                //            filenamer = PUPPICAD.HelperClasses.utilities.renderModelVisual3D(mlv, "", rootName);
                                //        }
                                //    }
                                //}
                                if (System.IO.File.Exists(filenamer))
                                {

                                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(filenamer))
                                    {
                                        using (MemoryStream m = new MemoryStream())
                                        {
                                            image.Save(m, image.RawFormat);
                                            byte[] imageBytes = m.ToArray();

                                            // Convert byte[] to Base64 String
                                            string base64String = Convert.ToBase64String(imageBytes);
                                            resendBack = base64String;
                                        }
                                    }

                                    //resendBack = ""; 
                                    //HttpListenerResponse myHresponse = ctx.Response;

                                    //byte[] buffer = System.IO.File.ReadAllBytes(filenamer);


                                    //myHresponse.Headers.Add("Access-Control-Allow-Credentials", "true");
                                    //myHresponse.Headers.Add("Access-Control-Allow-Origin", "*");
                                    //myHresponse.Headers.Add("Access-Control-Origin", "*");

                                    //myHresponse.ContentType = "image/png";

                                    //myHresponse.ContentLength64 = buffer.Length;
                                    //// Get a response stream and write the response to it.

                                    //Stream output = myHresponse.OutputStream;
                                    //output.Write(buffer, 0, buffer.Length);

                                    //// You must close the output stream.
                                    //output.Close();



                                }
                                else
                                    resendBack = "notfound";


                            }
                            else
                                if (myCommand.ToLower().Contains("getnoderendererstlstring"))
                                {

                                    string[] myInput = myCommand.Split(sep, StringSplitOptions.None);

                                    resendBack = "";

                                    if (PUPPICanvas.nodeRendererSTLServer.ContainsKey(myInput[1]))
                                    {
                                        resendBack = PUPPICanvas.nodeRendererSTLServer[myInput[1]];
                                    }
                                    else if (PUPPICanvas.nodeRendererModel3DServer.ContainsKey(myInput[1]))
                                    {

                                        List<Model3D> gll = PUPPICanvas.nodeRendererModel3DServer[myInput[1]];
                                        ModelVisual3D mlv = new ModelVisual3D();
                                        foreach (Model3D mld in gll)
                                        {
                                            ModelVisual3D mllv = new ModelVisual3D();
                                            mllv.Content = mld.Clone();
                                            mlv.Children.Add(mllv);

                                        }
                                        ArrayList linesA = new ArrayList();

                                        PUPPICAD.HelperClasses.helperfunctions.writegeomSTLrecursivelyWT(mlv, linesA, "ROnTheFlyN" + myInput[1], new Transform3DGroup());



                                        string stllines = "";
                                        for (int i = 0; i < linesA.Count; i++)
                                        {
                                            string line = linesA[i].ToString();
                                            stllines += "\n" + line;
                                        }

                                        resendBack = stllines;
                                    }
                                    else
                                        resendBack = "notfound";
                                }
                                else
                                    if (myCommand.ToLower() == "getindexedmodulenames")
                                    {
                                        resendBack = myPUPPIGUI.sendImmediateCommandAsText("getindexedmodulenames");//   + "<EOF>";
                                    }
                                    else if (myCommand.ToLower().Contains("getnodeoutputvalue"))
                                    {
                                        string[] myInput = myCommand.Split(sep, StringSplitOptions.None);
                                        string nid = myInput[1];
                                        int oindex = Convert.ToInt16(myInput[2]);
                                        while (PUPPIModule.concurrentProcesses > 0)
                                            Thread.Sleep(10);
                                        object o = myPUPPIGUI.getNodeOutputValue(nid, oindex);
                                        if (o != null)
                                        {
                                            resendBack = o.ToString();
                                        }
                                        else
                                        {
                                            resendBack = "null";
                                        }
                                    }



                                    else
                                    {
                                        myPUPPIGUI.pcanvas.commandQueue.Add(myCommand);
                                        resendBack = "sent";
                                    }

                    if (resendBack != "")
                    {

                        HttpListenerResponse myHresponse = ctx.Response;
                        //string myresponseString = cmd;// "<HTML><BODY> Hello world!</BODY></HTML>";
                        byte[] buffer = Encoding.UTF8.GetBytes(resendBack);
                        myHresponse.Headers.Add("Access-Control-Allow-Credentials", "true");
                        myHresponse.Headers.Add("Access-Control-Allow-Origin", "*");
                        myHresponse.Headers.Add("Access-Control-Origin", "*");

                        myHresponse.ContentType = "text/plain";
                        myHresponse.ContentEncoding = System.Text.UTF8Encoding.UTF8;
                        myHresponse.ContentLength64 = buffer.Length;
                        // Get a response stream and write the response to it.

                        Stream output = myHresponse.OutputStream;
                        output.Write(buffer, 0, buffer.Length);

                        // You must close the output stream.
                        output.Close();

                    }
                    if (PUPPIDEBUG.PUPPIDebugger.debugenabled)
                    {
                        PUPPIDEBUG.PUPPIDebugger.log("Canvas HTTP server finished executing command: " + myCommand);
                    }


                }
            }
            catch
            {

            }
            if (NumberClientThreads > 0) NumberClientThreads--;
        }
    }
}

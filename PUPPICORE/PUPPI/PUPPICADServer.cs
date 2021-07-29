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
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
namespace PUPPIServer
{
    /// <summary>
    /// This class contains commands to operate a CAD TCP socket server which allows clients to access CAD layout and node information, and send commands to the CAD.
    /// </summary>
    public static class PUPPICADTCPServer
    {


        /// <summary>
        /// Classes whose methods can be executed by calling them by name from clients, using command execmethod_|_classname_|_methodname_|_argument1_|_argument2....
        /// Objects need to be of different classes.
        /// </summary>
        public static List<object> exeClasses { get; set; }



        internal static int NumberClientThreads = 0;

        /// <summary>
        /// Starts TCP socket CAD server
        /// </summary>
        /// <param name="myCADController">PUPPIGUIController</param>
        /// <param name="IP">local IP Address (string)</param>
        /// <param name="openPort">port (integer). Port needs to be open for public connections</param>
        /// <param name="password">password (string)</param>
        /// <param name="provideFullUpdate">if true, CAD data is stored every time changes are made, useful for clients that need to render CAD </param>
        /// <returns>publicIP__localIP__port or error, as string</returns>
        public static string startServer(PUPPIGUIController.PUPPICADView myCADController, string IP, int openPort, string password)
        {
            PUPPIruntimesettings.PUPPICADTCPServerIsRunning = true;
            exeClasses = new List<object>();
            if (myCADController == null || myCADController.p3dview == null) return "no view";
            myCADController.p3dview.doAnInitialServerUpdate();
            Thread serverev = new Thread(() => PUPPIServer.PUPPICADTCPServer.startMyPUPPISrver(myCADController, IP, openPort, password));

            serverev.IsBackground = true;
            serverev.Name = "server";
            serverev.Start();

            string conn = "error";
            DateTime sst = DateTime.Now;
            DateTime nower = DateTime.Now;
            double elapmi = ((TimeSpan)(nower - sst)).TotalMilliseconds;
            while (conn == "error" && elapmi < 10000)
            {
                conn = PUPPIServer.PUPPICADTCPServer.getMeConnectionInformation();
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
            PUPPIruntimesettings.PUPPICADTCPServerIsRunning = false;
        }
        internal static void startMyPUPPISrver(PUPPIGUIController.PUPPICADView myCADControllerme, string IPA, int openPortME, string passwordserverme)
        {
            AsynchronousSocketListenerMe asyl = new AsynchronousSocketListenerMe(myCADControllerme, IPA, openPortME, passwordserverme);

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
            internal static PUPPIGUIController.PUPPICADView myPUPPIGUI;
            private static IPAddress ipAddress = null;
            // Thread signal.
            public static ManualResetEvent allDone = new ManualResetEvent(false);
            private static int port = 11000;
            internal AsynchronousSocketListenerMe(PUPPIGUIController.PUPPICADView myCADController, string IPA, int openPort, string password)
            {
                port = openPort;
                spassword = password;
                myPUPPIGUI = myCADController;
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

                if (myPUPPIGUI == null || myPUPPIGUI.p3dview == null) return;

        

                NumberClientThreads = 0;
                // Data buffer for incoming data.
                byte[] bytes = new Byte[1024];



                
                // Establish the local endpoint for the socket.
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.
                Socket listener = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
            


                try
                {
                    listener.Bind(localEndPoint);

                    listener.Listen(100);




                    while (PUPPIruntimesettings.PUPPICADTCPServerIsRunning)
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
                            string[] sepp = { "_|_" };
                            string[] myInput = myCommand.Split(sepp, StringSplitOptions.None);
                            if (PUPPIDEBUG.PUPPIDebugger.debugenabled)
                            {
                                PUPPIDEBUG.PUPPIDebugger.log("TCP server started executing command: " + myCommand);
                            }

                            string resendBack = "";
                            if (myInput[0].ToLower() == "cadgetstatus")
                            {
                                resendBack = PUPPICAD.PUPPI3DView.currentCADStatusServer;   //myPUPPIGUI.CADReadableReport();// + "<EOF>";
                            }
                            else
                                if (myInput[0].ToLower() == "cadgetlayout")
                                {
                                    resendBack = PUPPICAD.PUPPI3DView.currentXMLRepServer; //myPUPPIGUI.XMLCADLayout();// + "<EOF>";   
                                }


                            //else
                                //    if (myCommand.ToLower().Contains("getnoderendererimage"))
                                //    {

                            //        string[] myInput = myCommand.Split(sep, StringSplitOptions.None);

                            //        string filenamer = ""; //myPUPPIGUI.p3dview.interpretMyTextualCommand("getnoderendererimagefilename_|_" + myInput[1]);
                                //        resendBack = "";

                            //        if (PUPPICAD.nodeRendererImageFilesServer.ContainsKey(myInput[1]))
                                //        {
                                //            filenamer = PUPPICAD.nodeRendererImageFilesServer[myInput[1]];
                                //        }
                                //        if (System.IO.File.Exists(filenamer))
                                //            SendFile(handler, filenamer);
                                //        else
                                //            resendBack = "notfound";

                            //        // resendBack = myPUPPIGUI.p3dview.interpretMyTextualCommand(myCommand) + "<EOF>";
                                //    }
                                //    else
                                //        if (myCommand.ToLower().Contains("getnoderendererstlstring"))
                                //        {

                            //            string[] myInput = myCommand.Split(sep, StringSplitOptions.None);

                            //            resendBack = "";

                            //            if (PUPPICAD.nodeRendererSTLServer.ContainsKey(myInput[1]))
                                //            {
                                //                resendBack = PUPPICAD.nodeRendererSTLServer[myInput[1]];
                                //            }
                                //            else
                                //                resendBack = "notfound";
                                //        }

                                else if (myInput[0].ToLower().Contains("add"))
                                 {
                                    myPUPPIGUI.p3dview.commandQueue.Add(myCommand);
                                    resendBack = PUPPICAD.PUPPI3DView.modelGUIDs[PUPPICAD.PUPPI3DView.modelGUIDCount];

                                }

                                else if (myInput[0].ToLower().Contains("remove"))
                                {
                                    myPUPPIGUI.p3dview.commandQueue.Add(myCommand);
                                    resendBack = "sent";// PUPPICAD.PUPPI3DView.modelGUIDs[PUPPICAD.PUPPI3DView.modelGUIDCount];

                                }
                                else if (myInput[0].ToLower()=="getobject")
                                {
                                    resendBack = "notfound";
                                   
                                    List<Model3D> ml = new List<Model3D>();
                                    string myGUID = myInput[1];
                                    string[] seppg = { "--" };
                                    for (int j = 0; j < myPUPPIGUI.p3dview.serverObjects.Count; j++)
                                    {
                                        //"--"
                                        string s = myPUPPIGUI.p3dview.serverObjects.Keys.ToList<string>()[j].Split(seppg,StringSplitOptions.None)[0];
                                        if (s==myGUID)
                                        {
                                            ml.Add(myPUPPIGUI.p3dview.serverObjects.Values.ToList<Model3D>()[j]);
                                            break;
                                        }
                                    }
                                    if (ml.Count > 0)
                                    {
                                        resendBack = "";
                                        ArrayList lss = new ArrayList();
                                        ModelVisual3D mvl = new ModelVisual3D();
                                        foreach (Model3D gl in ml)
                                        {
                                            ModelVisual3D ngl = new ModelVisual3D();
                                            //  GeometryModel3D mcl = new GeometryModel3D();
                                            // mcl.Geometry = gl;
                                            ngl.Content = gl.Clone();
                                            mvl.Children.Add(ngl);
                                        }
                                        PUPPICAD.HelperClasses.helperfunctions.writegeomSTLrecursivelyWT(mvl, lss, myGUID, new Transform3DGroup());
                                        foreach (string lsss in lss)
                                        {
                                            resendBack += lsss;
                                        }
                                    }
                                }
                                else if (myInput[0].ToLower()=="execmethod")
                                {
                                    //execmethod_|_classname_|_methodname_|_argument1_|_argument2
                                    resendBack = "notfound";
                                   
                                    string classname = myInput[1];
                                    string methodname = myInput[2];
                                    List<string> argals = new List<string>();
                                    if (myInput.Length>3)
                                    {
                                        for (int i=0;i<myInput.Length-3;i++)
                                        {
                                            argals.Add(myInput[i+3]);
                                        }
                                    }
                                    resendBack = "Function returned: "+PUPPI.PUPPIStateEngine.Fexeunc(exeClasses,classname, methodname, argals);

                                }
                                else
                                {
                                    myPUPPIGUI.p3dview.commandQueue.Add(myCommand);
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
                                PUPPIDEBUG.PUPPIDebugger.log("CAD TCP server finished executing command: " + myCommand);
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

    public static class PUPPICADHTTPServer
    {

        /// <summary>
        /// Classes whose methods can be executed by calling them by name from clients, using command execmethod_|_classname_|_methodname_|_argument1_|_argument2....
        /// Objects need to be of different classes.
        /// </summary>
        public static List<object> exeClasses { get; set; }

        internal static int NumberClientThreads = 0;
        static int myhttpprt;
        static PUPPIGUIController.PUPPICADView myPUPPIGUI;
        static string myhttppwd;
        static HttpListener mylistener;
        /// <summary>
        /// Starts an HTTPListener
        /// If you get "error" as return try cmd as admin for your port e.g.: netsh http add urlacl url=http://*:8088/ user=everyone
        /// You can delete the allocation as follows: netsh http delete urlacl url=http://*:8088/
        /// For external connections you need to enable firewall inbound rule: BranchCache Content Retrieval (HTTP-In)
        /// Warning: the above could pose security risks! Activate at own risk!
        /// </summary>
        /// <param name="myCADController">PUPPICADView to interact with</param>
        /// <param name="port">integer port value (port needs to be open for public connections)</param>
        /// <param name="password">string password</param>
        /// <returns>Public IP __ port or "error" if failed </returns>
        public static string startServer(PUPPIGUIController.PUPPICADView myCADController, int port, string password)
        {
            myPUPPIGUI = myCADController;
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
            PUPPIruntimesettings.PUPPICADHTTPServerIsRunning = false;

        }
        //netsh http add urlacl url=http://+:8088/ user=everyone
        static string startMyHTTPServer()
        {

        
            if (myPUPPIGUI == null || myPUPPIGUI.p3dview == null) return "no view";

            exeClasses = new List<object>();

            PUPPICAD.PUPPI3DView.currentCADStatusServer = myPUPPIGUI.p3dview.readMyCADStatus();

            PUPPICAD.PUPPI3DView.currentXMLRepServer = myPUPPIGUI.p3dview.saveCADRepresentationToXML();


            NumberClientThreads = 0;
            mylistener = new HttpListener();
            mylistener.Prefixes.Add("http://*:" + myhttpprt.ToString() + "/");
            //  mylistener.Prefixes.Add("http://localhost/");
            try
            {
                PUPPIruntimesettings.PUPPICADHTTPServerIsRunning = true;

                if (myPUPPIGUI != null && myPUPPIGUI.p3dview != null)
                    myPUPPIGUI.p3dview.doAnInitialServerUpdate();
                mylistener.Start();
                ProcessMyAsync(mylistener);
                return getMeConnectionInformation();
            }
            catch (Exception exy)
            {
                PUPPIruntimesettings.PUPPICADHTTPServerIsRunning = false;
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

            while (PUPPIruntimesettings.PUPPICADHTTPServerIsRunning)
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
                        PUPPIDEBUG.PUPPIDebugger.log("HTTP CAD server started executing command: " + myCommand);
                    }

                    string resendBack = "";
                    if (myCommand.ToLower() == "CADgetstatus")
                    {
                        resendBack = PUPPICAD.PUPPI3DView.currentCADStatusServer;
                    }
                    else
                        if (myCommand.ToLower() == "CADgetlayout")
                        {
                            resendBack = PUPPICAD.PUPPI3DView.currentXMLRepServer;
                        }


                        //else
                        //    if (myCommand.ToLower().Contains("getnoderendererimage"))
                        //    {

                        //        string[] myInput = myCommand.Split(sep, StringSplitOptions.None);

                        //        string filenamer = "";
                        //        resendBack = "";

                        //        if (PUPPICAD.nodeRendererImageFilesServer.ContainsKey(myInput[1]))
                        //        {
                        //            filenamer = PUPPICAD.nodeRendererImageFilesServer[myInput[1]];
                        //        }
                        //        if (System.IO.File.Exists(filenamer))
                        //        {

                        //            using (System.Drawing.Image image = System.Drawing.Image.FromFile(filenamer))
                        //            {
                        //                using (MemoryStream m = new MemoryStream())
                        //                {
                        //                    image.Save(m, image.RawFormat);
                        //                    byte[] imageBytes = m.ToArray();

                        //                    // Convert byte[] to Base64 String
                        //                    string base64String = Convert.ToBase64String(imageBytes);
                        //                    resendBack = base64String;
                        //                }
                        //            }

                        //            //resendBack = ""; 
                        //            //HttpListenerResponse myHresponse = ctx.Response;

                        //            //byte[] buffer = System.IO.File.ReadAllBytes(filenamer);


                        //            //myHresponse.Headers.Add("Access-Control-Allow-Credentials", "true");
                        //            //myHresponse.Headers.Add("Access-Control-Allow-Origin", "*");
                        //            //myHresponse.Headers.Add("Access-Control-Origin", "*");

                        //            //myHresponse.ContentType = "image/png";

                        //            //myHresponse.ContentLength64 = buffer.Length;
                        //            //// Get a response stream and write the response to it.

                        //            //Stream output = myHresponse.OutputStream;
                        //            //output.Write(buffer, 0, buffer.Length);

                        //            //// You must close the output stream.
                        //            //output.Close();



                        //        }
                        //        else
                        //            resendBack = "notfound";


                        //    }
                        //    else
                        //        if (myCommand.ToLower().Contains("getnoderendererstlstring"))
                        //        {

                        //            string[] myInput = myCommand.Split(sep, StringSplitOptions.None);

                        //            resendBack = "";

                        //            if (PUPPICAD.nodeRendererSTLServer.ContainsKey(myInput[1]))
                        //            {
                        //                resendBack = PUPPICAD.nodeRendererSTLServer[myInput[1]];
                        //            }
                        //            else
                        //                resendBack = "notfound";
                        //        }

                        else if (myCommand.ToLower().Contains("add"))
                        {
                            myPUPPIGUI.p3dview.commandQueue.Add(myCommand);
                            resendBack = PUPPICAD.PUPPI3DView.modelGUIDs[PUPPICAD.PUPPI3DView.modelGUIDCount];

                        }
                        else if (myCommand.ToLower().Contains("getobject"))
                        {

                            resendBack = "notfound";
                            string[] sepp = { "_|_" };
                            string[] myInput = myCommand.Split(sepp, StringSplitOptions.None);
                            List<Model3D> ml = new List<Model3D>();
                            string myGUID = myInput[1];
                            for (int j = 0; j < myPUPPIGUI.p3dview.serverObjects.Count; j++)
                            {

                                string sls = myPUPPIGUI.p3dview.serverObjects.Keys.ToList<string>()[j];
                                if (sls.Contains(myGUID))
                                {
                                    ml.Add(myPUPPIGUI.p3dview.serverObjects.Values.ToList<Model3D>()[j]);
                                    break;
                                }
                            }
                            if (ml.Count > 0)
                            {
                                resendBack = "";
                                ArrayList lss = new ArrayList();
                                ModelVisual3D mvl = new ModelVisual3D();
                                foreach (Model3D gl in ml)
                                {
                                    ModelVisual3D ngl = new ModelVisual3D();
                                    //  GeometryModel3D mcl = new GeometryModel3D();
                                    // mcl.Geometry = gl;
                                    ngl.Content = gl.Clone();
                                    mvl.Children.Add(ngl);
                                }
                                PUPPICAD.HelperClasses.helperfunctions.writegeomSTLrecursivelyWT(mvl, lss, myGUID, new Transform3DGroup());
                                foreach (string lsss in lss)
                                {
                                    resendBack += lsss;
                                }
                            }
                        }

                        else if (myCommand.ToLower() == "execmethod")
                        {
                            //execmethod_|_classname_|_methodname_|_argument1_|_argument2
                            resendBack = "notfound";
                            string[] sepp = { "_|_" };
                            string[] myInput = myCommand.Split(sepp, StringSplitOptions.None);
                            string classname = myInput[1];
                            string methodname = myInput[2];
                            List<string> argals = new List<string>();
                            if (myInput.Length > 3)
                            {
                                for (int i = 0; i < myInput.Length - 3; i++)
                                {
                                    argals.Add(myInput[i + 3]);
                                }
                            }
                            resendBack = "Function returned: " + PUPPI.PUPPIStateEngine.Fexeunc(exeClasses, classname, methodname, argals);

                        }

                        else
                        {
                            myPUPPIGUI.p3dview.commandQueue.Add(myCommand);
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
                        PUPPIDEBUG.PUPPIDebugger.log("CAD HTTP server finished executing command: " + myCommand);
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

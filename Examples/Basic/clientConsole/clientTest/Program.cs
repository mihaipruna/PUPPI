using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


//*******************************************************************************************************************
//sample project for connecting to the PUPPI TCP Socket server and issuing commands via console.
//use with server in PUPPITestClientServer
//see commands in Getting Started With PUPPI document
//No Warranty: THE SOFTWARE IS A WORK IN PROGRESS AND IS PROVIDED "AS IS".
//http://visualprogramminglanguage.com
//Advanced client-server project samples available to PUPPI subscribers. Contact us at sales@pupi.co
//*******************************************************************************************************************


// State object for receiving data from remote device.
public class StateObject
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

public class receivedData
{
    public string text;
    public byte[] data;
}


public static class AsynchronousClient
{
    // The port number for the remote device.
    private static int port = 11000;

    private static string spassword = "";

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
    static IPHostEntry ipHostInfo;
    static IPAddress ipAddress;
    static IPEndPoint remoteEP;


    public static void initializeClient(string serverIPAddress, int connectionPort, string password)
    {
        // Establish the remote endpoint for the socket.

        // ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        // ipAddress = ipHostInfo.AddressList[6];
        ipAddress = IPAddress.Parse(serverIPAddress);
        port = connectionPort;
        remoteEP = new IPEndPoint(ipAddress, port);
        spassword = password;

    }


    public static receivedData sendServerCommand(string myCommand)
    {
        // Connect to a remote device.
        try
        {


            // client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true); 
            // Connect to the remote endpoint.


            using (Socket client = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp))
            {
                connectDone = new ManualResetEvent(false);
                sendDone = new ManualResetEvent(false);
                receiveDone = new ManualResetEvent(false);

                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);
                bool connectSuccessfully = connectDone.WaitOne(10000);
                if (connectSuccessfully == false)
                {
                    Console.WriteLine("Timeout connecting. Press any key to exit");
                    Console.ReadLine();
                    Environment.Exit(3);
                }



                // Send test data to the remote device.
                Send(client, myCommand + spassword + "<EOF>");
                bool sentSuccessfully = sendDone.WaitOne(10000);
                if (sentSuccessfully == false)
                {
                    Console.WriteLine("Timeout connecting. Press any key to exit");
                    Console.ReadLine();
                    Environment.Exit(3);
                }

                // Receive the response from the remote device.
                Receive(client);
                bool receivedSuccessfully = receiveDone.WaitOne(60000);
                if (receivedSuccessfully == false)
                {
                    Console.WriteLine("Timeout connecting. Press any key to exit");
                    Console.ReadLine();
                    Environment.Exit(3);
                }

                // Release the socket.
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                receivedData rd = new receivedData();
                rd.text = response;
                rd.data = binaryData;

                return rd;
            }



        }
        catch (Exception e)
        {
            receivedData rd = new receivedData();
            rd.text = "error";
            rd.data = binaryData;

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

            //   Console.WriteLine("Socket connected to {0}",
            //    client.RemoteEndPoint.ToString());

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
            StateObject state = new StateObject();
            state.workSocket = client;

            // Begin receiving the data from the remote device.
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Console.WriteLine("Connection error. Press any key to exit");
            Console.ReadLine();
            Environment.Exit(3);
        }
    }

    private static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the state object and the client socket 
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
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
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
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
                            //response = state.sb.ToString();
                        }
                        else
                        {
                            response = "checkbinary";
                            binaryData = (byte[])readData.Clone();
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


    public static int Main(String[] args)
    {
        StartClient(); 

        return 0;
    }
    private static void StartClient()
    {
        // Connect to a remote device.
        try
        {
            Console.WriteLine("Enter IP");
            string iPA = Console.ReadLine();
            Console.WriteLine("Enter Port");
           int port =Convert.ToInt16 ( Console.ReadLine());
           Console.WriteLine("Enter password");
           string pass = Console.ReadLine();
           AsynchronousClient.initializeClient(iPA, port, pass);  

            while (true)
            {
               


                    string myCommand = "";

                

                    Console.WriteLine("Enter command:");
                    myCommand = Console.ReadLine();



                    if (myCommand == "quit") break;
                  string responses = AsynchronousClient.sendServerCommand(myCommand ).text;
                Console.WriteLine(responses); 

                

            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            Console.ReadLine();
        }
    }


}


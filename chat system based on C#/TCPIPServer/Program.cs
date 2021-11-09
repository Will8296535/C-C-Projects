/*
 FILE			:Program.cs
 PROJECT        :PROG2121-20F Assignment5
 PRPGRAMMER     :Will Wei
 FIRST VERSION  :2020-11-01
 DESCRIPTION	:This file contains the server of a chat system 
                 that using TCP/IP to communicate 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TCPIPServer
{
    
    class Program
    {
        static volatile bool Run = true;
        static void Main(string[] args)
        {
            List<TcpClient> clients = new List<TcpClient>();
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000 and listening for client requests.
               
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[3];
                server = new TcpListener(ipAddress, 13000);

                //Int32 port = 13000;
                //IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                //server = new TcpListener(localAddr, port);
                server.Start();

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // When server receive a request from client, it add the client to clients list
                    // and start a new thread for communication 
                    TcpClient client = server.AcceptTcpClient();
                    clients.Add(client); 
                    data clientData = new data() {
                        cs = clients,
                        c = client
                    };
                    Console.WriteLine("Connected!");
                    ParameterizedThreadStart ts = new ParameterizedThreadStart(Worker);
                    Thread clientThread = new Thread(ts);
                    clientThread.Start(clientData);

                }
            }
            catch (SocketException e)
            {
                throw new System.ArgumentException("Socket Exception.", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        }
        struct data
        {
            public List<TcpClient> cs;
            public TcpClient c;
        }

        //
        // FUNCTION : Worker
        // DESCRIPTION :
        // 
        // PARAMETERS :
        // object: o contains the client 
        // and the List of all clients that connected to the server
        // RETURNS :
        // none
        //
        public static void Worker(Object o)
        {
            data clientData = (data)o;
            TcpClient client = clientData.c;
            // Buffer for reading data
            byte[] bytes = new byte[256];
            String data = null;
            NetworkStream stream = client.GetStream();
            // Loop to receive all the data sent by the client.
            while (Run)
            {
                try
                {
                    // Receive the data from client
                    int i = stream.Read(bytes, 0, bytes.Length);
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                    if (data == "0")
                    {
                        // when receive "0" from client, close the thread
                        Run = false;
                        stream.Write(msg, 0, msg.Length);
                        stream.Flush();
                        Console.WriteLine("Sent: {0}", data);
                        // delete the handle from the list
                        clientData.cs.Remove(client);
                    }
                    // Send back a response to every client
                    else
                    {
                        foreach (TcpClient c in clientData.cs)
                        {
                            NetworkStream s = c.GetStream();
                            s.Write(msg, 0, msg.Length);
                            s.Flush();
                            Console.WriteLine("Sent: {0}", data);
                        }
                    }


                }
                catch(Exception ex)
                {
                    throw new System.ArgumentException("Error", ex);
                }
                
            }

            
        }
        
    }   
}

/*
 FILE			:MainWindw.xaml.cs
 PROJECT        :PROG2121-20F Assignment5
 PRPGRAMMER     :Will Wei
 FIRST VERSION  :2020-11-01
 DESCRIPTION	:This file contains the client of a chat system 
                 that using TCP/IP to communicate 
*/
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
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace Assignment05
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;
        private string readData = null;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        //
        // FUNCTION : Connect_Click
        // DESCRIPTION :
        // This function create a new client to connect to the server
        // and start a thread to send/receive message from server.
        // PARAMETERS :
        // None
        // RETURNS :
        // none
        //
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            readData = "Connected to Server...";
            msg();

            //client = new TcpClient("127.0.0.1", 13000);
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[3];
            client = new TcpClient(ipAddress.ToString(), 13000);

            stream = client.GetStream();
            Thread myThread = new Thread(getMessage);
            myThread.Start();
            SendButton.IsEnabled = true;
            ConnectButton.IsEnabled = false;
            message.IsEnabled = true;
            
        }
        //
        // FUNCTION : getMessage
        // DESCRIPTION :
        // This function will receive the message from server through the stream
        // and show it to the TextBlock
        // If the message is "0", then close the stream and client
        // PARAMETERS :
        // None
        // RETURNS :
        // none
        //
        private void getMessage()
        {
            while(true)
            {
                try
                {
                    
                    byte[] data = new byte[256];
                    stream.Read(data, 0, data.Length);
                    readData = System.Text.Encoding.ASCII.GetString(data);
                    // When recieve '0', disconnect from server
                    if(readData[0] == '0')
                    {
                        readData = "Disconnect";
                        msg();
                        stream.Close();
                        client.Close();
                        break;
                    }
                    else
                    {
                        msg();
                    }

                }
                catch(SocketException e)
                {
                    throw new System.ArgumentException("Socket Exception.", e);
                }
                catch(System.IO.IOException e)
                {
                    throw new System.ArgumentException("IO Exception.", e);
                }
            }
           
            
        }
        //
        // FUNCTION : Button_Clic
        // DESCRIPTION :
        // This function will get the message from massage box 
        // and send it to the server through the stream
        // PARAMETERS :
        // None
        // RETURNS :
        // none
        //
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            // Translate the passed message into ASCII and store it as a Byte array
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message.Text);

            // Send the message to the connected TcpServer.
            stream.Write(data, 0, data.Length);
            stream.Flush();

            // Empty the massage box
            message.Text = "";
            
        }
        //
        // FUNCTION : msg
        // DESCRIPTION :
        // This function will update the text in ReceiveTxt TextBlock
        // PARAMETERS :
        // None
        // RETURNS :
        // none
        //
        private void msg()
        {
            if(ReceiveTxt.Dispatcher.Thread == Thread.CurrentThread)
            {
                ReceiveTxt.Text = ReceiveTxt.Text + "\n" + readData;
            }
            else
            {
                ReceiveTxt.Dispatcher.Invoke(new Action(msg));
            }
        }
    }
}

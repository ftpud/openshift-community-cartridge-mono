using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            // IP and PORT for LISTENING 
            string IP = Environment.GetEnvironmentVariable("OPENSHIFT_MONO_IP");
            int PORT = 8080;

            ///
            Console.WriteLine("Listening " + IP + ":" + PORT);
            
            /// Start Listening
            TcpListener Listener = new TcpListener(IPAddress.Parse(IP), PORT);
            Listener.Start();
                        
            /// 
            Responser Resp = new Responser();            

            /// Listening LOOP
            while (true)
            {                
                Resp.AddClient( Listener.AcceptSocket() );
                Thread.Sleep(1);
            }
        }
    }

    /// <summary>
    /// Some class for responses
    /// Modify this one
    /// </summary>
    class Responser
    {
        // Response Text
        byte[] Header = ASCIIEncoding.UTF8.GetBytes( "HTTP/1.1 200\r\nContent-Type: text/html\r\n\r\n <h1>Hello World!</h1><br> It's working!" );
        
        // Thread
        Thread Worker;

        public Responser() { }
        
        /// <summary>
        /// Starts thread for Client
        /// </summary>
        /// <param name="Client"></param>
        public void AddClient(Socket Client)
        {
            Console.WriteLine("New Connection! " + Client.LocalEndPoint.ToString() + " -> " + Client.RemoteEndPoint.ToString());
            Worker = new Thread(new ParameterizedThreadStart(WorkerThreed));
            Worker.Start(Client);
        }

        /// <summary>
        /// Answering Threed
        /// </summary>
        /// <param name="O"></param>
        void WorkerThreed(Object O)
        {            
            Socket Client = (Socket)O;

            using (NetworkStream ns = new NetworkStream(Client)) 
            {
                ns.Write(Header, 0, Header.Length);
                Thread.Sleep(50);
            }            
                Client.Disconnect(true);

        }
    }
}

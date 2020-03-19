using System;
using System.Net;
using System.Net.Sockets;



namespace SocketNetWork
{
    /// <summary>
    /// 这个是服务器端的启动口.
    /// </summary>
    class Program
    {
        private static Socket serverSocket;
        private static Socket clientSocket;



        private const string IP = "127.0.0.1";
        private static int Port = 1001;

        static void Main(string[] args)
        { 
            Console.WriteLine("开启服务器,端口:{0}, 时间:{1}",Port,DateTime.Now);

            //创建,Server-Socket
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定 IP、Port
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(IP), Port));
            //设置最多排队3000连接请求
            serverSocket.Listen(3000);

            //开启异步接收
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);

            Console.ReadKey();
        }

        /// <summary>
        /// 初始化所有Contoller
        /// </summary>
        static void InitAllController()
        {
            RoleController.Instance.Init();
        }

        /// <summary>
        /// 与(客户端)异步连接
        /// </summary>
        static void AcceptCallBack(IAsyncResult ar)
        {
            clientSocket = serverSocket.EndAccept(ar);
            Console.WriteLine("客户端已经连接,监听到远端IP{0}: ", clientSocket.RemoteEndPoint.ToString());

            //创建client
            Client client = new Client(clientSocket);
            client.Start();

            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }

    }
}

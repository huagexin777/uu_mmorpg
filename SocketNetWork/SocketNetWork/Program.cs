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


        static void Main(string[] args)
        {
            //
            ServerSocket server = new ServerSocket("127.0.0.1", 6666);
            server.Start();

            //1.创建socket对象
            serverSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            //2.设置监听数 (排队挂起人数)
            serverSocket.Listen(3000);
            //3.绑定ip地址.
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.0.116"), 6666);
            serverSocket.Bind(ip);
            //4.异步监听连接
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);

        }

        static Tool.Meassage msg = new Tool.Meassage();

        /// <summary>
        /// 建立(客户端的)连接
        /// </summary>
        /// <param name="ar"></param>
        static void AcceptCallBack(IAsyncResult ar)
        {
            clientSocket = serverSocket.EndAccept(ar);

            //向客户端发送一条消息.
            byte[] data = System.Text.Encoding.UTF8.GetBytes("客户端已经连接过来了,远端ip地址是: " + clientSocket.RemoteEndPoint.ToString());
            clientSocket.Send(data);

            //客户端,【持续接收】信息传递.
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None,ReceiveClientCallBack, clientSocket);

            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }


        /// <summary>
        /// 接收(客户端的)信息传递
        /// </summary>
        /// <param name="ar"></param>
        static void ReceiveClientCallBack(IAsyncResult ar)
        {
            try
            {
                while (true)
                {
                    int length = clientSocket.EndReceive(ar);
                    //length > 0 属于正常接收到了数据,
                    if (length > 0)
                    {
                        msg.AddCount(length);
                        msg.ReadMessage();
                        //客户端,【持续接收】信息传递.
                        clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveClientCallBack, clientSocket);
                    }
                    //length <= 0 属于客户端自己断开了连接.
                    else
                    {
                        clientSocket.Close();
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                //属于,客户端出现了.应用程序卡死或者进程崩溃！
                Console.WriteLine(e);
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }
                throw;
            }
        }

    }
}

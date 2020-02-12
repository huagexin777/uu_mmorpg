using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketNetWork
{
    class ServerSocket
    {
        private Socket serverSocket;
        private Socket clientSocket;

        public ServerSocket(string ip,int port)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Tcp);
            serverSocket.Listen(3000);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip),port));
        }

        /// <summary>
        /// 开启
        /// </summary>
        public void Start()
        {
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }

        void AcceptCallBack(IAsyncResult ar)
        {
            clientSocket = serverSocket.EndAccept(ar);
        
            Console.WriteLine("服务器端ip = "+ serverSocket.LocalEndPoint.ToString() + "  接收到了一个客户端的连接." );

            //客户端开始监听了.
            Role role = new Role();
            ClientSocket client = new ClientSocket(clientSocket, role);
            client.Start();

            //把[当前role]添加到 角色管理类里面.
            RoleMgr.Instance.RoleList.Add(role);

            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }
    }
}

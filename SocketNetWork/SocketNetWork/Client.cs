using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketNetWork
{
    public class Client
    {
        private Role role;
        private Socket clientSocket;
        private byte[] buffer = new byte[10240];
        private MMO_MemoryStream receiveMS = new MMO_MemoryStream();


        /// <summary>
        /// 接收数据
        /// <para>队列</para>
        /// </summary>
        private Queue<byte[]> recieveQueue = new Queue<byte[]>();
        /// <summary>
        /// 发送数据
        /// <para>队列</para>
        /// </summary>
        private Queue<byte[]> sendQueue = new Queue<byte[]>();
        /// <summary>
        /// 检查队列
        /// </summary>
        private Action OnCheckQueueEvent;


        public Client(Socket socket)
        {
            clientSocket = socket;

            //将role添加进管理.
            Role role = new Role(this);
            this.role = role;
            RoleMgr.Instance.RoleList.Add(role);

            this.OnCheckQueueEvent = OnCheckQueue;
        }



        #region 1.开启-异步-消息接收

        /// <summary>
        /// 当前客户端
        /// <para>开启连接</para>
        /// </summary>
        public void Start()
        {
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecieveCallBack, clientSocket);

            Thread reciveMsg = new Thread(ReciveMsg);
            reciveMsg.Start();

            Thread sendMsg = new Thread(SendMsg);
            sendMsg.Start();
        }

        void SendMsg()
        {
           
        }

        void ReciveMsg()
        {
            while (true)
            {
                if (recieveQueue.Count > 0)
                {
                    byte[] tempbuffer = recieveQueue.Dequeue();
                    string msg = TestProtol.GetProtol(tempbuffer).ToString();
                    Console.WriteLine("接收到的消息是: " + msg);
                }
            }
        }

        /// <summary>
        /// 消息接收
        /// <para>回调函数</para>
        /// </summary>
        private void RecieveCallBack(IAsyncResult ar)
        {
            int length = clientSocket.EndReceive(ar);
            try
            {
                //有数据
                if (length > 0)
                {
                    //把接收到的数据 写入到缓冲数据流的尾部.                //--------------- (╬▔皿▔)凸  这里已经把【上一个剩余数据】的指针移到数据流尾部.
                    receiveMS.Position = receiveMS.Length;
                    //把指定长度的字节 写入(当前)数据流
                    receiveMS.Write(buffer, 0, length);                     //--------------- (╬▔皿▔)凸  这里已经把【缓冲数据流】重新写入到了【当前数据流-字节】

                    //如果,缓冲数据流的长度 > 2.说明至少有个不完整的包过来了.
                    //客户端发送过来的包头 (ushort) 长度是2
                    if (receiveMS.Length > 2)
                    {
                        //循环拆包.
                        while (true)
                        {
                            //1.把缓冲数据流指针移到0位置.
                            receiveMS.Position = 0;

                            //2. currentLength = 包体长度 
                            //   currentFullLength = 总包体长度(包头+包体)
                            int currentLength = receiveMS.ReadUShort();
                            int currentFullLength = 2 + currentLength;

                            //3.如果,缓冲数据流的长度 > 总包体长度.说明已经接收到了一个完整的包.
                            if (receiveMS.Length >= currentFullLength)
                            {
                                //收到了至少一个完整包.
                                //目标: 拿到完整包的包体,读出来.

                                //1.定义 以包头内的length为长度的 [数据流] 数组.
                                byte[] buffer1 = new byte[currentLength];
                                //2.把缓冲数据流 指针position 定位到2(也就是包头的位置),
                                //  开始读取并写入到 临时(字节)数据流中.
                                receiveMS.Position = 2;
                                receiveMS.Read(buffer1, 0, currentLength);
                                //3.入队列
                                recieveQueue.Enqueue(buffer1);


                                //------------------------处理剩余的字节-----------------------------
                                //1.拿到 剩余的字节长度.
                                int remainLength = (int)receiveMS.Length - currentFullLength;
                                //目标: 把剩余的数据流,重新写入缓冲数据流中.
                                if (remainLength > 0)
                                {
                                    //缓冲数据流的指针,移向整包尾部.
                                    receiveMS.Position = currentFullLength;

                                    //先读取剩余数据
                                    byte[] remainBuffer = new byte[remainLength];
                                    receiveMS.Read(remainBuffer, 0, remainLength);

                                    //再清空,缓冲数据指针、缓冲数据长度.
                                    receiveMS.Position = 0;
                                    receiveMS.SetLength(0);

                                    //再重新把剩余数据,写回缓冲数据流中.
                                    receiveMS.Write(remainBuffer, 0, remainBuffer.Length);
                                }
                                else
                                {
                                    //直接清空,缓冲数据指针、缓冲数据长度.
                                    receiveMS.Position = 0;
                                    receiveMS.SetLength(0);
                                    break;
                                }
                            }
                            else
                            {
                                //收到的包,不完整.
                                break;
                            }
                        }
                    }

                    //再次进行循环,接收数据流.
                    Start();
                }
                //客户端连接中断了.
                else
                {
                    Console.WriteLine("客户端{0}断开连接!" + clientSocket.RemoteEndPoint.ToString());
                    RoleMgr.Instance.RoleList.Remove(role);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("报错原因: " + e);
                Console.WriteLine("客户端{0}断开连接!" + clientSocket.RemoteEndPoint.ToString());
                RoleMgr.Instance.RoleList.Remove(role);
                throw;
            }
        }

        #endregion

        #region 2.发送消息

        public void SendData(byte[] buffer)
        {
            //1.封装数据包.
            byte[] sendBuffer = MakeData(buffer);

            lock (sendQueue)
            {
                //把数据包,加入到send队列
                sendQueue.Enqueue(sendBuffer);
                //开启委托 【-异步发送-】
                OnCheckQueueEvent.BeginInvoke(null, null);
            }
        }

        /// <summary>
        /// 封装 byte[] 数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] MakeData(byte[] data)
        {
            byte[] buff = null;
            using (MMO_MemoryStream ms = new MMO_MemoryStream())
            {
                ms.WriteUShort((ushort)data.Length);    //position = 2
                ms.Write(data, 0, data.Length);         //position = 2 + data.Length
                buff = ms.ToArray();
            }
            return buff;
        }

        /// <summary>
        /// 检查队列
        /// </summary>
        void OnCheckQueue()
        {
            lock (sendQueue)
            {
                //队列中,存有数据.请发送!
                if (sendQueue.Count > 0)
                {
                    byte[] buffer = sendQueue.Dequeue();
                    //拿到,封装好了的数据包,进行发送!
                    clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, clientSocket);
                }
            }
        }

        /// <summary>
        /// 发送回调
        /// </summary>
        void SendCallBack(IAsyncResult ar)
        {
            clientSocket.EndSend(ar);

            //继续检查队列
            
        }

        #endregion



    }
}

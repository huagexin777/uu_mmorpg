using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketNetWork
{
    class ClientSocket
    {
        private Socket clientSocket;
        private Role role;
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


        public ClientSocket(Socket socket,Role role)
        {
            clientSocket = socket;
            this.role = role;

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
        }

        /// <summary>
        /// 持续接收
        /// <para>回调函数</para>
        /// </summary>
        /// <param name="ar"></param>
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
                                byte[] buff1 = new byte[currentLength];
                                //2.把缓冲数据流 指针position 定位到2(也就是包头的位置),
                                //  开始读取并写入到 临时(字节)数据流中.
                                receiveMS.Position = 2;
                                receiveMS.Read(buff1, 0, currentLength);
                                //3.拿到数据,并临时输出.
                                using (MMO_MemoryStream ms1 = new MMO_MemoryStream(buff1))
                                {
                                    string msg1 = ms1.ReadUTF8String();
                                    Console.WriteLine("接收到了客户端的消息数据: " + msg1);
                                }
                                //4.入队列
                                recieveQueue.Enqueue(buff1);


                                //------------------------处理剩余的字节-----------------------------
                                //1.拿到 剩余的字节长度.
                                int remainLength = (int)receiveMS.Length - currentFullLength;
                                //目标: 把剩余的数据流,重新写入缓冲数据流中.
                                if (remainLength > 0)
                                {
                                    //缓冲数据流的指针,移向整包尾部.
                                    receiveMS.Position = currentFullLength;

                                    //先读取剩余数据
                                    byte[] remainBuff = new byte[remainLength];
                                    receiveMS.Read(remainBuff, 0, remainLength);

                                    //再清空,缓冲数据指针、缓冲数据长度.
                                    receiveMS.Position = 0;
                                    receiveMS.SetLength(0);

                                    //再重新把剩余数据,写回缓冲数据流中.
                                    receiveMS.Write(remainBuff, 0, remainBuff.Length);
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
                    Console.WriteLine("客户端{0}断开连接!" + clientSocket.LocalEndPoint.ToString());
                    RoleMgr.Instance.RoleList.Remove(role);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("报错原因: " + e);
                Console.WriteLine("客户端{0}断开连接!" + clientSocket.LocalEndPoint.ToString());
                RoleMgr.Instance.RoleList.Remove(role);
                throw;
            }
        }

        #endregion

        #region 2.发送消息

        public void SendData(string msg)
        {
            //1.转换数组包
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
            //2.得到封装后的数据包
            byte[] sendBuff = MakeData(data);

            lock (sendQueue)
            {
                //把数据包,加入到send队列
                sendQueue.Enqueue(sendBuff);
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
            OnCheckQueueEvent();
        }

        #endregion

    }
}

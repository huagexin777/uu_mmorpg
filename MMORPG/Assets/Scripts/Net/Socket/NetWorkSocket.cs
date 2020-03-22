using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

class NetWorkSocket:MonoBehaviour
{
    #region 单例

    private static NetWorkSocket _instance;
    public static NetWorkSocket Instance 
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<NetWorkSocket>();
            }
            return _instance;
        }
    }

    #endregion

    //private
    private Socket clientSocket;
    private byte[] buffer = new byte[1024];
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
    /// 检查(发送)队列事件
    /// </summary>
    private Action OnCheck_SendQueueEvent;

    /// <summary>
    /// 连接成功事件
    /// </summary>
    public Action OnConncetSuccess;

    void Awake()
    {
        
    }

    void Start()
    {



    }


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TestProtol test = new TestProtol()
        //    {
        //        Name = "林大侠",
        //        Sex = 1,
        //        Age = 20,
        //        Des = "米西奥斯",
        //    };
        //    SendData(test.ToArray());
        //};
    }

    /// <summary>
    /// 建立连接
    /// </summary>
    public NetWorkSocket Connect()
    {
        //如果,socket已经存在 并且处于连接状态中 则直接返回.
        if (clientSocket != null && clientSocket.Connected) { return null; }

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            //连接
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(GlobalInit.currentServer.ip), GlobalInit.currentServer.port));
            OnCheck_SendQueueEvent = OnCheckQueue;

            //连接成功事件
            OnConncetSuccess?.Invoke();

            //开启,消息接收!
            Thread t = new Thread(OnRecieveLisener);
            t.Start();
        }
        catch (Exception e)
        {
            Debug.LogError("连接失败! " + e);
        }
      

        return this;
    }

    void OnConnectCallBack(IAsyncResult ar)
    {
        clientSocket.EndConnect(ar);

        
    }

    void OnRecieveLisener() 
    {
        while (true)
        {
            int length = clientSocket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
            Debug.LogError(length);
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
                                //3.入队列
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
                        }// //循环拆包.
                    }//if(receiveMS.Length > 2)---->  接收到了数据.
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }


    #region 2.发送消息

    

    public void SendData(byte[] buffer)
    {
        //1.转换数组包
        //byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
        //2.得到封装后的数据包
        byte[] sendBuffer = MakeData(buffer);

        lock (sendQueue)
        {
            //把数据包,加入到send队列
            sendQueue.Enqueue(sendBuffer);
            //开启委托 【-异步发送-】
            OnCheck_SendQueueEvent.BeginInvoke(null, null);
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
                clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }
        }
    }

    #endregion

}
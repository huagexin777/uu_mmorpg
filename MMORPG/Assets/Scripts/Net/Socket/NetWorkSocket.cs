using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class NetWorkSocket 
{
    private static NetWorkSocket _instance;
    public static NetWorkSocket Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_instance)
                {
                    _instance = new NetWorkSocket();
                }
            }
            return _instance;
        }
    }


    /// <summary>
    /// ��ǰ������
    /// <para>�ֽ�����</para>
    /// </summary>
    private byte[] buffer = new byte[10240];
    /// <summary>
    /// ���ݻ�����
    /// </summary>
    private MMO_MemoryStream receiveMS = new MMO_MemoryStream();

    /// <summary>
    /// ��������
    /// <para>����</para>
    /// </summary>
    private Queue<byte[]> recieveQueue = new Queue<byte[]>();
    /// <summary>
    /// ��������
    /// <para>����</para>
    /// </summary>
    private Queue<byte[]> sendQueue = new Queue<byte[]>();
    /// <summary>
    /// ������
    /// </summary>
    private Action OnCheckQueueEvent;

    private Socket clientSocket;


    private int recieveAmount = 5;

    


    #region 1.���ӷ�����

    /// <summary>
    /// ���ӷ�����
    /// </summary>
    public void ConnetionServer(string ip, int port)
    {
        //�Ѿ���������,ֱ��return
        if (clientSocket != null && clientSocket.Connected) return;

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            //��ʼ����
            clientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), ConnectCallBack, clientSocket);
            OnCheckQueueEvent = OnCheckQueue;
            Debug.Log("���ӳɹ�!");
        }
        catch (Exception e)
        {
            Debug.LogError("�޷����ӷ�������,����: " + e);
            throw;
        }
    }
    
    /// <summary>
    /// ���ӷ�����-�ص�
    /// </summary>
    void ConnectCallBack(IAsyncResult ar)
    {
        clientSocket.EndConnect(ar);
        Debug.LogError("���յ���Զ�˷�����IP = " + clientSocket.RemoteEndPoint.ToString());

        //clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecieveCallBack, clientSocket);
        StartRecieve();
    }

    #endregion

    #region 2.������Ϣ

    /// <summary>
    /// ������Ϣ���ջص�.
    /// </summary>
    public void StartRecieve()
    {
        if (clientSocket == null && clientSocket.Connected == false) { return; }

        try
        {
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecieveCallBack, clientSocket);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// ��������
    /// <para>�ص�����</para>
    /// </summary>
    /// <param name="ar"></param>
    private void RecieveCallBack(IAsyncResult ar)
    {
        int length = clientSocket.EndReceive(ar);
        try
        {
            //������
            if (length > 0)
            {
                //�ѽ��յ������� д�뵽������������β��.                //--------------- (�p����)͹  �����Ѿ��ѡ���һ��ʣ�����ݡ���ָ���Ƶ�������β��.
                receiveMS.Position = receiveMS.Length;
                //��ָ�����ȵ��ֽ� д��(��ǰ)������
                receiveMS.Write(buffer, 0, length);                     //--------------- (�p����)͹  �����Ѿ��ѡ�����������������д�뵽�ˡ���ǰ������-�ֽڡ�

                //���,�����������ĳ��� > 2.˵�������и��������İ�������.
                //�ͻ��˷��͹����İ�ͷ (ushort) ������2
                if (receiveMS.Length > 2)
                {
                    //ѭ�����.
                    while (true)
                    {
                        //1.�ѻ���������ָ���Ƶ�0λ��.
                        receiveMS.Position = 0;

                        //2. currentLength = ���峤�� 
                        //   currentFullLength = �ܰ��峤��(��ͷ+����)
                        int currentLength = receiveMS.ReadUShort();
                        int currentFullLength = 2 + currentLength;

                        //3.���,�����������ĳ��� > �ܰ��峤��.˵���Ѿ����յ���һ�������İ�.
                        if (receiveMS.Length >= currentFullLength)
                        {
                            //�յ�������һ��������.
                            //Ŀ��: �õ��������İ���,������.

                            //1.���� �԰�ͷ�ڵ�lengthΪ���ȵ� [������] ����.
                            byte[] buff1 = new byte[currentLength];
                            //2.�ѻ��������� ָ��position ��λ��2(Ҳ���ǰ�ͷ��λ��),
                            //  ��ʼ��ȡ��д�뵽 ��ʱ(�ֽ�)��������.
                            receiveMS.Position = 2;
                            receiveMS.Read(buff1, 0, currentLength);
                            //3.�õ�����,����ʱ���.
                            using (MMO_MemoryStream ms1 = new MMO_MemoryStream(buff1))
                            {
                                string msg1 = ms1.ReadUTF8String();
                                Console.WriteLine("���յ��˷������˵���Ϣ����: " + msg1);
                            }
                            //4.�����
                            recieveQueue.Enqueue(buff1);


                            //------------------------����ʣ����ֽ�-----------------------------
                            //1.�õ� ʣ����ֽڳ���.
                            int remainLength = (int)receiveMS.Length - currentFullLength;
                            //Ŀ��: ��ʣ���������,����д�뻺����������.
                            if (remainLength > 0)
                            {
                                //������������ָ��,��������β��.
                                receiveMS.Position = currentFullLength;

                                //�ȶ�ȡʣ������
                                byte[] remainBuff = new byte[remainLength];
                                receiveMS.Read(remainBuff, 0, remainLength);

                                //�����,��������ָ�롢�������ݳ���.
                                receiveMS.Position = 0;
                                receiveMS.SetLength(0);

                                //�����°�ʣ������,д�ػ�����������.
                                receiveMS.Write(remainBuff, 0, remainBuff.Length);
                            }
                            else
                            {
                                //ֱ�����,��������ָ�롢�������ݳ���.
                                receiveMS.Position = 0;
                                receiveMS.SetLength(0);
                                break;
                            }
                        }
                        else
                        {
                            //�յ��İ�,������.
                            break;
                        }
                    }
                }

                //�ٴν���ѭ��,����������.
                StartRecieve();
            }
            else
            {
                Console.WriteLine("�ͻ���{0}�Ͽ�����!" + clientSocket.LocalEndPoint.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("����ԭ��: " + e);
            Console.WriteLine("�ͻ���{0}�Ͽ�����!" + clientSocket.LocalEndPoint.ToString());
            throw;
        }
    }

    #endregion

    #region 3.������Ϣ

    public void SendData(string msg)
    {
        //1.ת�������
        byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
        //2.�õ���װ������ݰ�
        byte[] sendBuff = MakeData(data);

        lock (sendQueue)
        {
            //�����ݰ�,���뵽send����
            sendQueue.Enqueue(sendBuff);
            //����ί�� ��-�첽����-��
            OnCheckQueueEvent.BeginInvoke(null, null);
        }
    }

    /// <summary>
    /// ��װ byte[] ����
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
    /// ������
    /// </summary>
    void OnCheckQueue()
    {
        lock (sendQueue)
        {
            //������,��������.�뷢��!
            if (sendQueue.Count > 0)
            {
                byte[] buffer = sendQueue.Dequeue();
                //�õ�,��װ���˵����ݰ�,���з���!
                clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, clientSocket);
            }
        }
    }

    /// <summary>
    /// ���ͻص�
    /// </summary>
    void SendCallBack(IAsyncResult ar)
    {
        clientSocket.EndSend(ar);

        //����������
        OnCheckQueueEvent();
    }

    #endregion

}

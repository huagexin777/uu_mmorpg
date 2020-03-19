using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;


//short int long float double bool
public class MMO_MemoryStream : MemoryStream
{

    public MMO_MemoryStream() { }

    public MMO_MemoryStream(byte[] buffer) : base(buffer)
    {

    }


    //字节(byte)      位(bit)  1byte = 8bit
    //int           = 4字节
    //short         = 2字节
    //long          = 8字节
    //float         = 16字节
    //double        = 32字节
    //bool          = 1字节

    #region short
    /// <summary>
    /// 读取short(16位)类型数据
    /// </summary>
    public short ReadShort()
    {
        byte[] buffer = new byte[2];
        base.Read(buffer, 0, 2);
        return BitConverter.ToInt16(buffer, 0);
    }

    /// <summary>
    /// 写入short(16位)类型数据
    /// </summary>
    public void WriteShort(short value)
    {
        byte[] buffer = BitConverter.GetBytes(value);
        base.Write(buffer, 0, buffer.Length);
    }
    #endregion
    #region ushort
    /// <summary>
    /// 读取ushort(16位无符号)类型数据
    /// </summary>
    public ushort ReadUShort()
    {
        byte[] buffer = new byte[2];
        base.Read(buffer, 0, 2);
        return BitConverter.ToUInt16(buffer, 0);
    }

    /// <summary>
    /// 写入ushort(16位无符号)类型数据
    /// </summary>
    public void WriteUShort(ushort value)
    {
        byte[] buffer = BitConverter.GetBytes(value);
        base.Write(buffer, 0, buffer.Length);
    }
    #endregion

    #region int
    /// <summary>
    /// 读取int(32位)类型数据
    /// </summary>
    public int ReadInt()
    {
        byte[] buffer = new byte[4];
        base.Read(buffer, 0, 4);
        return BitConverter.ToInt32(buffer, 0);
    }

    /// <summary>
    /// 写入int(32位)类型数据
    /// </summary>
    public void WriteInt(int value)
    {
        byte[] buffer = BitConverter.GetBytes(value);
        base.Write(buffer, 0, buffer.Length);
    }
    #endregion
    #region uint
    /// <summary>
    /// 读取uint(32位无符号)类型数据
    /// </summary>
    public uint ReadUInt()
    {
        byte[] buffer = new byte[4];
        base.Read(buffer, 0, 4);
        return BitConverter.ToUInt32(buffer, 0);
    }

    /// <summary>
    /// 写入int(32位无符号)类型数据
    /// </summary>
    public void WriteUInt(uint value)
    {
        byte[] buffer = BitConverter.GetBytes(value);
        base.Write(buffer, 0, buffer.Length);
    }
    #endregion


    #region long
    /// <summary>
    /// 读取long(64位)类型数据
    /// </summary>
    public long ReadLong()
    {
        byte[] buffer = new byte[8];
        base.Read(buffer, 0, 8);
        return BitConverter.ToInt64(buffer, 0);
    }

    /// <summary>
    /// 写入long(64位)类型数据
    /// </summary>
    public void WriteLong(long value)
    {
        byte[] buffer = BitConverter.GetBytes(value);
        base.Write(buffer, 0, buffer.Length);
    }
    #endregion
    #region ulong
    /// <summary>
    /// 读取long(64位无符号)类型数据
    /// </summary>
    public ulong ReadULong()
    {
        byte[] buffer = new byte[8];
        base.Read(buffer, 0, 8);
        return BitConverter.ToUInt64(buffer, 0);
    }

    /// <summary>
    /// 写入long(64位无符号)类型数据
    /// </summary>
    public void WriteULong(ulong value)
    {
        byte[] buffer = BitConverter.GetBytes(value);
        base.Write(buffer, 0, buffer.Length);
    }
    #endregion

    #region float
    /// <summary>
    /// 读取float(32位)类型数据
    /// </summary>
    public float ReadFloat()
    {
        byte[] buffer = new byte[4];
        base.Read(buffer, 0, 4);
        return BitConverter.ToSingle(buffer, 0);
    }

    /// <summary>
    /// 读取float(32位)类型数据
    /// </summary>
    public void WriteFloat(float value)
    {
        byte[] buffer = BitConverter.GetBytes(value);
        base.Write(buffer, 0, buffer.Length);
    }
    #endregion

    #region double
    /// <summary>
    /// 读取double(64位)类型数据
    /// </summary>
    public double ReadDouble()
    {
        byte[] buffer = new byte[8];
        base.Read(buffer, 0, 8);
        return BitConverter.ToSingle(buffer, 0);
    }

    /// <summary>
    /// 读取double(64位)类型数据
    /// </summary>
    public void WriteDouble(double value)
    {
        byte[] buffer = BitConverter.GetBytes(value);
        base.Write(buffer, 0, buffer.Length);
    }
    #endregion

    #region bool
    /// <summary>
    /// 读取bool数据
    /// </summary>
    public bool ReadBool()
    {
        return base.ReadByte() == 1;
    }

    /// <summary>
    /// 写入bool数据
    /// </summary>
    public void WriteBool(bool value)
    {
        base.WriteByte((byte)(value == true ? 1 : 0));
    }
    #endregion




    //----------------------------------字符串操作----------------------------------


    /// <summary>
    /// 从流中读取一个string数组
    /// </summary>
    /// <returns></returns>
    public string ReadUTF8String()
    {
        ushort len = this.ReadUShort();
        byte[] arr = new byte[len];

        base.Read(arr, 0, len);                 //1.读取长度
        return Encoding.UTF8.GetString(arr);    //2.读取数据
    }


    /// <summary>
    /// 把一个string数组写入流
    /// </summary>
    /// <returns></returns>
    public void WriteUTF8String(string str)
    {
        //C#是用的Unicode编码，变量可以是汉字，所以一个字符占2个字节

        byte[] arr = Encoding.UTF8.GetBytes(str);
        //Debug.Log(arr.Length);
        if (arr.Length > 65535)
        {
            throw new InvalidCastException("字符串超出范围!");
        }

        this.WriteUShort((ushort)arr.Length);   //1.写入长度
        base.Write(arr, 0, arr.Length);         //2.写入数据
    }
}

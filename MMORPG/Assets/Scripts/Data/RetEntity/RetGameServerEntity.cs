using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//: byte 不能在这里用.JsonMapper会转换失败
public enum EnumEntityStatus 
{
    Deleted = 0,
    Released = 1
}


/// <summary>
/// ret服务器实体类
/// </summary>
public class RetGameServerEntity
{
    /// <summary>
    /// 编号
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public EnumEntityStatus Status { get; set; }

    /// <summary>
    /// 渠道号-状态
    /// </summary>
    public byte ChannelStatus { get; set; }

    /// <summary>
    /// 运行-状态
    /// </summary>
    public byte RunStatus { get; set; }

    /// <summary>
    /// 是否推荐
    /// </summary>
    public bool IsCommand { get; set; }

    /// <summary>
    /// 是否新服
    /// </summary>
    public bool IsNew { get; set; }

    /// <summary>
    /// 区服名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    /// 端口号
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    public override string ToString()
    {
        return string.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}", Id, Status, ChannelStatus, RunStatus, IsCommand, IsNew, Name, Ip, Port, CreateTime, UpdateTime);
    }
}
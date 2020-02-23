/// <summary>
/// 类名 : AccountEntity
/// 作者 : 北京-边涯
/// 说明 : 
/// 创建日期 : 2020-02-22 21:04:45
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mmcoy.Framework.AbstractBase;

/// <summary>
/// 
/// </summary>
[Serializable]
public partial class AccountEntity : MFAbstractEntity
{
    #region 重写基类属性
    /// <summary>
    /// 主键
    /// </summary>
    public override int? PKValue
    {
        get
        {
            return this.Id;
        }
        set
        {
            this.Id = value;
        }
    }
    #endregion

    #region 实体属性

    /// <summary>
    /// 编号
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public EnumEntityStatus Status { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Pwd { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Mail { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int Money { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public short ChannelId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int LastLogOnServerId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string LastLogOnServerName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime LastLogOnServerTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int LastLogOnRoleId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string LastLogOnRoleNickName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int LastLogOnRoleJobId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string DeviceIdentifier { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string DeviceModel { get; set; }


    #endregion
}

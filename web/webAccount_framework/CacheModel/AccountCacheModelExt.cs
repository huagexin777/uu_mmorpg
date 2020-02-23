using Mmcoy.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class AccountCacheModel
{
    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public MFReturnValue<int> Register(string username, string password,short channel)
    {
        return this.DBModel.Register(username, password, channel);
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="channelId"></param>
    /// <returns></returns>

    public AccountEntity Login(string username, string password)
    {
        return this.DBModel.Login(username,password);
    }

    }
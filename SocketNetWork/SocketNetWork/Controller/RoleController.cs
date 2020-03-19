using SocketNetWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RoleController : Singleton<RoleController>
{

    public void Init() 
    {
        EventDispatcher.Instance.AddEventListener(ProtoCodeDef.RoleOperation_LogOnGameServer, OnLogOnGameServer);
    }

    /// <summary>
    /// LogOnGameServer-回调
    /// </summary>
    /// <param name="role"></param>
    /// <param name="buffer"></param>
    private void OnLogOnGameServer(Role role,byte[] buffer)
    {
        RoleOperation_LogOnGameServerProto protol = RoleOperation_LogOnGameServerProto.GetProto(buffer);

        // 根据玩家账号id 查询玩家属下角色
        LogOnGameServerReturn(role, protol.AccountId);
    }


    /// <summary>
    /// 根据玩家账号id 查询玩家属下角色
    /// </summary>
    /// <param name="role"></param>
    /// <param name="accountId"></param>
    void LogOnGameServerReturn(Role role,int accountId)
    {
        RoleOperation_LogOnGameServerReturnProto retProtol = new RoleOperation_LogOnGameServerReturnProto();
        List<RoleEntity> list = RoleCacheModel.Instance.GetList(condition: string.Format("AccountId={0}", accountId));
        if (list != null || list.Count != 0)
        {
            retProtol.RoleCount = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                RoleOperation_LogOnGameServerReturnProto.RoleItem roleItem = new RoleOperation_LogOnGameServerReturnProto.RoleItem()
                {
                    RoleId = (int)list[i].Id,
                    RoleNickName = list[i].NickName,
                    RoleJob = (byte)list[i].JobId,
                    RoleLevel = list[i].Level,
                };
                retProtol.RoleList.Add(roleItem);
            }
        }

        //发送协议
        role.client.SendData(retProtol.ToArray());
    }

    public override void Dispose()
    {
        base.Dispose();
    }

}
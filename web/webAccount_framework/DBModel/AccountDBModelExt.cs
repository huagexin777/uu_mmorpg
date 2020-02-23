using Mmcoy.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

public partial class AccountDBModel
{

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public MFReturnValue<int> Register(string username, string password, short channelId)
    {
        MFReturnValue<int> reValue = new MFReturnValue<int>();

        using (SqlConnection conn = new SqlConnection(DBConn.DBAccount))
        {
            conn.Open();
            //开始事务目的,只打开一次数据库
            SqlTransaction trans = conn.BeginTransaction();

            //参数: 1:表名 2.(待知) 3.查询条件 4.事务trans 5.取消自动状态(这里状态应用于账号找回)
            List<AccountEntity> accountEntities = this.GetListWithTran(this.TableName, "Id", "UserName='" + username + "'", isAutoStatus: false, trans: trans);

            //说明用户名不存在
            if (accountEntities == null || accountEntities.Count == 0)
            {
                AccountEntity ac = new AccountEntity();
                ac.Status = Mmcoy.Framework.AbstractBase.EnumEntityStatus.Released; //要设置状态
                ac.UserName = username;
                ac.Pwd = MFEncryptUtil.Md5(password);//加密
                ac.ChannelId = channelId;
                ac.LastLogOnServerTime = DateTime.Now;
                ac.CreateTime = DateTime.Now;
                ac.UpdateTime = DateTime.Now;


                MFReturnValue<object> temp = this.Create(trans, ac);
                if (!temp.HasError)
                {
                    //注册成功！
                    reValue.Value = (int)temp.OutputValues["Id"];
                    trans.Commit();
                }
                else
                {
                    reValue.HasError = true;
                    reValue.Message = "用户名已经存在";
                    trans.Rollback();
                }
            }
            else
            {
                reValue.HasError = true;
                reValue.Message = "用户名已经存在";
                trans.Rollback();
            }
        }

        return reValue;
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
        //创建查询条件 //不能用, 只能用and
        string conditional = string.Format("username='{0}' and pwd='{1}'", username, MFEncryptUtil.Md5(password));

        AccountEntity account = this.GetEntity(conditional);

        return account;
    }


    /// <summary>
    /// //测试.服务器是否连通
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public AccountEntity Get(int id)
    {
        string testStr = "Data Source=DESKTOP-OMM45J8;Initial Catalog=MMP_RPG;Persist Security Info=True;User ID=lin;Password=yrmewbha1997";
        using (SqlConnection conn = new SqlConnection(testStr))
        {
            conn.Open();

            //建立执行对象
            //SqlCommand sql = new SqlCommand("select * from Account_GetEntity where Id = @Id", conn);
            SqlCommand sql = new SqlCommand("Account_GetEntity", conn);//存储过程

            //设置命令行类型为-存储过程
            sql.CommandType = System.Data.CommandType.StoredProcedure;

            sql.Parameters.Add(new SqlParameter("@Id", id));

            using (SqlDataReader dr = sql.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        AccountEntity entity = new AccountEntity();
                        entity.Id = dr["Id"].ToInt();
                        entity.UserName = dr["UserName"].ToString();
                        entity.Pwd = dr["Pwd"].ToString();
                        return entity;
                    }
                }
            }
            return null;
        }
    }

}
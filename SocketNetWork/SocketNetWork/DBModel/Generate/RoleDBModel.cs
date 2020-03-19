/// <summary>
/// 类名 : RoleDBModel
/// 作者 : 测试
/// 说明 : 
/// 创建日期 : 2020-03-15 17:24:52
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using Mmcoy.Framework.AbstractBase;

/// <summary>
/// DBModel
/// </summary>
public partial class RoleDBModel : MFAbstractSQLDBModel<RoleEntity>
{
    #region RoleDBModel 私有构造
    /// <summary>
    /// 私有构造
    /// </summary>
    private RoleDBModel()
    {

    }
    #endregion

    #region 单例
    private static object lock_object = new object();
    private static RoleDBModel instance = null;
    public static RoleDBModel Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lock_object)
                {
                    if (instance == null)
                    {
                        instance = new RoleDBModel();
                    }
                }
            }
            return instance;
        }
    }
    #endregion

    #region 实现基类的属性和方法

    #region ConnectionString 数据库连接字符串
    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    protected override string ConnectionString
    {
        get { return DBConn.RPG_GameServer; }
    }
    #endregion

    #region TableName 表名
    /// <summary>
    /// 表名
    /// </summary>
    protected override string TableName
    {
        get { return "Role"; }
    }
    #endregion

    #region ColumnList 列名集合
    private IList<string> _ColumnList;
    /// <summary>
    /// 列名集合
    /// </summary>
    protected override IList<string> ColumnList
    {
        get
        {
            if (_ColumnList == null)
            {
                _ColumnList = new List<string> { "Id", "Status", "AccountId", "JobId", "NickName", "Sex", "Level", "Money", "Gold", "Exp", "MaxHP", "MaxMP", "CurrHP", "CurrMP", "Attack", "Defense", "Hit", "Dodge", "Crit", "Res", "Fighting", "CrateTime", "UpdateTime" };
            }
            return _ColumnList;
        }
    }
    #endregion

    #region ValueParas 转换参数
    /// <summary>
    /// 转换参数
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected override SqlParameter[] ValueParas(RoleEntity entity)
    {
        SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Id", entity.Id) { DbType = DbType.Int32 },
                new SqlParameter("@Status", entity.Status) { DbType = DbType.Byte },
                new SqlParameter("@AccountId", entity.AccountId) { DbType = DbType.Int32 },
                new SqlParameter("@JobId", entity.JobId) { DbType = DbType.Int32 },
                new SqlParameter("@NickName", entity.NickName) { DbType = DbType.String },
                new SqlParameter("@Sex", entity.Sex) { DbType = DbType.Byte },
                new SqlParameter("@Level", entity.Level) { DbType = DbType.Int32 },
                new SqlParameter("@Money", entity.Money) { DbType = DbType.Int32 },
                new SqlParameter("@Gold", entity.Gold) { DbType = DbType.Int32 },
                new SqlParameter("@Exp", entity.Exp) { DbType = DbType.Int32 },
                new SqlParameter("@MaxHP", entity.MaxHP) { DbType = DbType.Int32 },
                new SqlParameter("@MaxMP", entity.MaxMP) { DbType = DbType.Int32 },
                new SqlParameter("@CurrHP", entity.CurrHP) { DbType = DbType.Int32 },
                new SqlParameter("@CurrMP", entity.CurrMP) { DbType = DbType.Int32 },
                new SqlParameter("@Attack", entity.Attack) { DbType = DbType.Int32 },
                new SqlParameter("@Defense", entity.Defense) { DbType = DbType.Int32 },
                new SqlParameter("@Hit", entity.Hit) { DbType = DbType.Int32 },
                new SqlParameter("@Dodge", entity.Dodge) { DbType = DbType.Int32 },
                new SqlParameter("@Crit", entity.Crit) { DbType = DbType.Int32 },
                new SqlParameter("@Res", entity.Res) { DbType = DbType.Int32 },
                new SqlParameter("@Fighting", entity.Fighting) { DbType = DbType.Int32 },
                new SqlParameter("@CrateTime", entity.CrateTime) { DbType = DbType.DateTime },
                new SqlParameter("@UpdateTime", entity.UpdateTime) { DbType = DbType.DateTime },
                new SqlParameter("@RetMsg", SqlDbType.NVarChar, 255),
                new SqlParameter("@ReturnValue", SqlDbType.Int)
            };
        return parameters;
    }
    #endregion

    #region GetEntitySelfProperty 封装对象
    /// <summary>
    /// 封装对象
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="table"></param>
    /// <returns></returns>
    protected override RoleEntity GetEntitySelfProperty(IDataReader reader, DataTable table)
    {
        RoleEntity entity = new RoleEntity();
        foreach (DataRow row in table.Rows)
        {
            var colName = row.Field<string>(0);
            if (reader[colName] is DBNull)
                continue;
            switch (colName.ToLower())
            {
                case "id":
                    if (!(reader["Id"] is DBNull))
                        entity.Id = Convert.ToInt32(reader["Id"]);
                    break;
                case "status":
                    if (!(reader["Status"] is DBNull))
                        entity.Status = (EnumEntityStatus)Convert.ToInt32(reader["Status"]);
                    break;
                case "accountid":
                    if (!(reader["AccountId"] is DBNull))
                        entity.AccountId = Convert.ToInt32(reader["AccountId"]);
                    break;
                case "jobid":
                    if (!(reader["JobId"] is DBNull))
                        entity.JobId = Convert.ToInt32(reader["JobId"]);
                    break;
                case "nickname":
                    if (!(reader["NickName"] is DBNull))
                        entity.NickName = Convert.ToString(reader["NickName"]);
                    break;
                case "sex":
                    if (!(reader["Sex"] is DBNull))
                        entity.Sex = Convert.ToByte(reader["Sex"]);
                    break;
                case "level":
                    if (!(reader["Level"] is DBNull))
                        entity.Level = Convert.ToInt32(reader["Level"]);
                    break;
                case "money":
                    if (!(reader["Money"] is DBNull))
                        entity.Money = Convert.ToInt32(reader["Money"]);
                    break;
                case "gold":
                    if (!(reader["Gold"] is DBNull))
                        entity.Gold = Convert.ToInt32(reader["Gold"]);
                    break;
                case "exp":
                    if (!(reader["Exp"] is DBNull))
                        entity.Exp = Convert.ToInt32(reader["Exp"]);
                    break;
                case "maxhp":
                    if (!(reader["MaxHP"] is DBNull))
                        entity.MaxHP = Convert.ToInt32(reader["MaxHP"]);
                    break;
                case "maxmp":
                    if (!(reader["MaxMP"] is DBNull))
                        entity.MaxMP = Convert.ToInt32(reader["MaxMP"]);
                    break;
                case "currhp":
                    if (!(reader["CurrHP"] is DBNull))
                        entity.CurrHP = Convert.ToInt32(reader["CurrHP"]);
                    break;
                case "currmp":
                    if (!(reader["CurrMP"] is DBNull))
                        entity.CurrMP = Convert.ToInt32(reader["CurrMP"]);
                    break;
                case "attack":
                    if (!(reader["Attack"] is DBNull))
                        entity.Attack = Convert.ToInt32(reader["Attack"]);
                    break;
                case "defense":
                    if (!(reader["Defense"] is DBNull))
                        entity.Defense = Convert.ToInt32(reader["Defense"]);
                    break;
                case "hit":
                    if (!(reader["Hit"] is DBNull))
                        entity.Hit = Convert.ToInt32(reader["Hit"]);
                    break;
                case "dodge":
                    if (!(reader["Dodge"] is DBNull))
                        entity.Dodge = Convert.ToInt32(reader["Dodge"]);
                    break;
                case "crit":
                    if (!(reader["Crit"] is DBNull))
                        entity.Crit = Convert.ToInt32(reader["Crit"]);
                    break;
                case "res":
                    if (!(reader["Res"] is DBNull))
                        entity.Res = Convert.ToInt32(reader["Res"]);
                    break;
                case "fighting":
                    if (!(reader["Fighting"] is DBNull))
                        entity.Fighting = Convert.ToInt32(reader["Fighting"]);
                    break;
                case "cratetime":
                    if (!(reader["CrateTime"] is DBNull))
                        entity.CrateTime = Convert.ToDateTime(reader["CrateTime"]);
                    break;
                case "updatetime":
                    if (!(reader["UpdateTime"] is DBNull))
                        entity.UpdateTime = Convert.ToDateTime(reader["UpdateTime"]);
                    break;
            }
        }
        return entity;
    }
    #endregion

    #endregion
}
